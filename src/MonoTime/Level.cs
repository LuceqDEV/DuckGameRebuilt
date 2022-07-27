﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Level
// Assembly: DuckGame, Version=1.1.8175.33388, Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace DuckGame
{
    public class Level
    {
        public List<NMLevel> levelMessages = new List<NMLevel>();
        public bool isCustomLevel;
        public static bool flipH = false;
        public static bool symmetry = false;
        public static bool leftSymmetry = true;
        public static bool loadingOppositeSymmetry = false;
        public string _level = "";
        private static LevelCore _core = new LevelCore();
        public static bool skipInitialize = false;
        public bool isPreview;
        private static Queue<List<object>> _collisionLists = new Queue<List<object>>();
        private bool _simulatePhysics = true;
        private Color _backgroundColor = Color.Black;
        protected QuadTreeObjectList _things = new QuadTreeObjectList();
        protected string _id = "";
        protected Camera _camera = new Camera();
        protected NetLevelStatus _networkStatus;
        public float transitionSpeedMultiplier = 1f;
        public float lowestPoint = 1000f;
        private bool _lowestPointInitialized;
        public float highestPoint = -1000f;
        protected bool _initialized;
        private bool _levelStart;
        public bool _startCalled;
        protected bool _centeredView;
        private bool _waitingOnNewData;
        public byte networkIndex;
        public int seed;
        private bool _notifiedReady;
        private bool _initializeLater;
        public bool bareInitialize;
        protected Vec2 _topLeft = new Vec2(99999f, 99999f);
        protected Vec2 _bottomRight = new Vec2(-99999f, -99999f);
        protected bool _readyForTransition = true;
        public bool _waitingOnTransition;
        public bool cold;
        public bool suppressLevelMessage;
        private static int collectionCount;
        protected int _updateWaitFrames;
        private bool _sentLevelChange;
        private bool _calledAllClientsReady;
        public bool transferCompleteCalled = true;
        private bool _aiInitialized;
        private bool _refreshState;
        private bool initPaths;
        private Dictionary<NetworkConnection, bool> checksumReplies = new Dictionary<NetworkConnection, bool>();
        public static bool doingOnLoadedMessage = false;
        public float flashDissipationSpeed = 0.15f;
        public bool skipCurrentLevelReset;
        private int wait = 60;
        private bool _clearScreen = true;
        public bool drawsOverPauseMenu;
        private Sprite _burnGlow;
        private Sprite _burnGlowWide;
        private Sprite _burnGlowWideLeft;
        private Sprite _burnGlowWideRight;

        public string level => this._level;

        public static LevelCore core
        {
            get => Level._core;
            set => Level._core = value;
        }

        public static void InitializeCollisionLists()
        {
            MonoMain.loadMessage = "Loading Collision Lists";
            for (int index = 0; index < 10; ++index)
                Level._collisionLists.Enqueue(new List<object>());
        }

        public static List<object> GetNextCollisionList() => new List<object>();

        public static bool PassedChanceGroup(int group, float val) => group == -1 ? (double)Rando.Float(1f) < (double)val : (double)Level._core._chanceGroups[group] < (double)val;

        public static bool PassedChanceGroup2(int group, float val) => group == -1 ? (double)Rando.Float(1f) < (double)val : (double)Level._core._chanceGroups2[group] < (double)val;

        public static float GetChanceGroup2(int group) => group == -1 ? Rando.Float(1f) : Level._core._chanceGroups2[group];

        public bool simulatePhysics
        {
            get => this._simulatePhysics;
            set => this._simulatePhysics = value;
        }

        public static bool sendCustomLevels
        {
            get => Level._core.sendCustomLevels;
            set => Level._core.sendCustomLevels = value;
        }

        public static Level current
        {
            get => Level._core.nextLevel == null ? Level._core.currentLevel : Level._core.nextLevel;
            set => Level._core.nextLevel = value;
        }

        public static Level activeLevel
        {
            get => Level._core.currentLevel;
            set => Level._core.currentLevel = value;
        }

        public Color backgroundColor
        {
            get => this._backgroundColor;
            set => this._backgroundColor = value;
        }

        public static void Add(Thing thing)
        {
            if (Level._core.currentLevel == null)
                return;
            Level._core.currentLevel.AddThing(thing);
        }

        public static void Remove(Thing thing)
        {
            if (Level._core.currentLevel == null)
                return;
            Level._core.currentLevel.RemoveThing(thing);
        }

        public static void ClearThings()
        {
            if (Level._core.currentLevel == null)
                return;
            Level._core.currentLevel.Clear();
        }

        public static void UpdateCurrentLevel()
        {
            if (Level._core.currentLevel == null)
                return;
            Level._core.currentLevel.DoUpdate();
        }

        public static void DrawCurrentLevel()
        {
            if (Level._core.currentLevel == null)
                return;
            Level._core.currentLevel.DoDraw();
        }

        public static T First<T>()
        {
            IEnumerable<Thing> thing = Level.current.things[typeof(T)];
            return thing.Count<Thing>() > 0 ? (T)(object)thing.First<Thing>() : default(T);
        }

        public T FirstOfType<T>()
        {
            IEnumerable<Thing> thing = this.things[typeof(T)];
            return thing.Count<Thing>() > 0 ? (T)(object)thing.First<Thing>() : default(T);
        }

        public QuadTreeObjectList things => this._things;

        public string id => this._id;

        public Camera camera
        {
            get => this._camera;
            set => this._camera = value;
        }

        public NetLevelStatus networkStatus => this._networkStatus;

        public bool initialized => this._initialized;

        public bool initializeFunctionHasBeenRun => this._initialized && !this._initializeLater;

        public bool waitingOnNewData
        {
            get => this._waitingOnNewData;
            set => this._waitingOnNewData = value;
        }

        public virtual void DoInitialize()
        {
            if (this.waitingOnNewData)
            {
                this._initializeLater = true;
                this._initialized = true;
            }
            else if (!this._initialized)
            {
                GhostManager.context.TransferPendingGhosts();
                Random generator = Rando.generator;
                Rando.generator = new Random(this.seed + 2500);
                Level.InitChanceGroups();
                Rando.generator = generator;
                this.Initialize();
                if (!Network.isActive || Network.InLobby())
                    this.DoStart();
                this._things.RefreshState();
                this.CalculateBounds();
                this._initialized = true;
                if (this._centeredView)
                    this.camera.centerY -= (float)(((double)(DuckGame.Graphics.aspect * this.camera.width) - (double)(9f / 16f * this.camera.width)) / 2.0);
                if (!VirtualTransition.active)
                    StaticRenderer.Update();
                if (!Network.isActive)
                    return;
                this.ClientReady(DuckNetwork.localConnection);
            }
            else
            {
                foreach (Thing thing in this._things)
                    thing.AddToLayer();
            }
        }

        public virtual void LevelLoaded()
        {
        }

        public virtual void Initialize()
        {
            this._levelStart = true;
            Vote.ClearVotes();
        }

        private void DoStart()
        {
            if (this._startCalled)
                return;
            this.Start();
            this._startCalled = true;
        }

        public void SkipStart() => this._startCalled = true;

        public virtual void Start()
        {
        }

        public virtual void Terminate() => this.Clear();

        public virtual void AddThing(Thing t)
        {
            if (Thread.CurrentThread == Content.previewThread && this != Content.previewLevel)
                Content.previewLevel.AddThing(t);
            else if (t is ThingContainer)
            {
                ThingContainer thingContainer = t as ThingContainer;
                if (thingContainer.bozocheck)
                {
                    foreach (Thing thing in thingContainer.things)
                    {
                        if (!Thing.CheckForBozoData(thing))
                            this.AddThing(thing);
                    }
                }
                else
                {
                    foreach (Thing thing in thingContainer.things)
                        this.AddThing(thing);
                }
            }
            else
            {
                if (t.level != this)
                {
                    this._things.Add(t);
                    if (!Level.skipInitialize)
                        t.Added(this, !this.bareInitialize, false);
                }
                if (!Network.isActive || t.connection != null)
                    return;
                t.connection = DuckNetwork.localConnection;
            }
        }

        public virtual void RemoveThing(Thing t)
        {
            if (t == null)
                return;
            HatSelector hatSelector = t as HatSelector;
            t.DoTerminate();
            t.Removed();
            this._things.Remove(t);
            if (t.ghostObject == null || !t.isServerForObject)
                return;
            GhostManager.context.RemoveLater(t.ghostObject);
        }

        public void Clear()
        {
            foreach (Thing thing in this._things)
                thing.Removed();
            Layer.ClearLayers();
            this._things.Clear();
        }

        public Vec2 topLeft => this._topLeft;

        public Vec2 bottomRight => this._bottomRight;

        public static void InitChanceGroups()
        {
            for (int index = 0; index < Level._core._chanceGroups.Count; ++index)
                Level._core._chanceGroups[index] = Rando.Float(1f);
            for (int index = 0; index < Level._core._chanceGroups2.Count; ++index)
                Level._core._chanceGroups2[index] = Rando.Float(1f);
        }

        public virtual string LevelNameData() => this.GetType().Name;

        public virtual string networkIdentifier => "";

        public static void UpdateLevelChange()
        {
            if (Level._core.nextLevel != null)
            {
                RumbleManager.ClearRumbles(new RumbleType?());
                if (Level._core.currentLevel is IHaveAVirtualTransition && Level._core.nextLevel is IHaveAVirtualTransition && !(Level._core.nextLevel is TeamSelect2))
                    VirtualTransition.GoVirtual();
                if (Network.isActive && Level.activeLevel != null && !Level._core.nextLevel._sentLevelChange)
                {
                    byte levelIndex = DuckNetwork.levelIndex;
                    DevConsole.Log(DCSection.GhostMan, "|DGYELLOW|Performing level swap (" + levelIndex.ToString() + ")");
                    if (Level._core.currentLevel is TeamSelect2 && !(Level._core.nextLevel is TeamSelect2))
                        DuckNetwork.ClosePauseMenu();
                    if (!(Level._core.currentLevel is TeamSelect2) && Level._core.nextLevel is TeamSelect2)
                        DuckNetwork.ClosePauseMenu();
                    if (Network.isServer && !(Level._core.nextLevel is IConnectionScreen))
                    {
                        if (DuckNetwork.levelIndex > (byte)250)
                            DuckNetwork.levelIndex = (byte)1;
                        if (Level._core.nextLevel is TeamSelect2)
                            Network.ContextSwitch((byte)0);
                        else
                            Network.ContextSwitch((byte)((uint)DuckNetwork.levelIndex + 1U));
                        DuckNetwork.compressedLevelData = (MemoryStream)null;
                        string[] strArray = new string[5]
                        {
              "|DGYELLOW|Incrementing level index (",
              ((int) DuckNetwork.levelIndex - 1).ToString(),
              "->",
              null,
              null
                        };
                        levelIndex = DuckNetwork.levelIndex;
                        strArray[3] = levelIndex.ToString();
                        strArray[4] = ")";
                        DevConsole.Log(DCSection.GhostMan, string.Concat(strArray));
                        if (!Level._core.nextLevel.suppressLevelMessage)
                            Send.Message((NetMessage)new NMLevel(Level._core.nextLevel));
                        Level._core.nextLevel.networkIndex = DuckNetwork.levelIndex;
                    }
                    else if (Level._core.nextLevel is IConnectionScreen)
                        Network.ContextSwitch(byte.MaxValue);
                    Level._core.nextLevel._sentLevelChange = true;
                }
                if (!VirtualTransition.active)
                {
                    if (NetworkDebugger.enabled && NetworkDebugger.Recorder.active != null)
                        Rando.generator = new Random(NetworkDebugger.Recorder.active.seed);
                    DamageManager.ClearHits();
                    Layer.ResetLayers();
                    HUD.ClearCorners();
                    if (Level._core.currentLevel != null)
                        Level._core.currentLevel.Terminate();
                    string str1 = Level._core.currentLevel != null ? Level._core.currentLevel.LevelNameData() : "null";
                    string str2 = Level._core.nextLevel != null ? Level._core.nextLevel.LevelNameData() : "null";
                    if (Level._core.nextLevel is XMLLevel && (Level._core.nextLevel as XMLLevel).level == "RANDOM")
                        DevConsole.Log(DCSection.General, "Level Switch (" + str1 + " -> Random Level(" + (Level._core.nextLevel as XMLLevel).seed.ToString() + "))");
                    else
                        DevConsole.Log(DCSection.General, "Level Switch (" + str1 + " -> " + str2 + ")");
                    Level._core.currentLevel = Level._core.nextLevel;
                    Level._core.nextLevel = (Level)null;
                    Layer.lighting = false;
                    VirtualTransition.core._transitionLevel = (Level)null;
                    AutoUpdatables.ClearSounds();
                    SequenceItem.sequenceItems.Clear();
                    DuckGame.Graphics.GarbageDisposal(true);
                    GC.Collect(1, GCCollectionMode.Optimized);
                    ++Level.collectionCount;
                    if (!(Level._core.currentLevel is GameLevel))
                    {
                        if (MonoMain.timeInMatches > 0)
                        {
                            Global.data.timeInMatches.valueInt += MonoMain.timeInMatches / 60;
                            MonoMain.timeInMatches = 0;
                        }
                        if (MonoMain.timeInArcade > 0)
                        {
                            Global.data.timeInArcade.valueInt += MonoMain.timeInArcade / 60;
                            MonoMain.timeInArcade = 0;
                        }
                        if (MonoMain.timeInEditor > 0)
                        {
                            Global.data.timeInEditor.valueInt += MonoMain.timeInEditor / 60;
                            MonoMain.timeInEditor = 0;
                        }
                        if (!(Level._core.currentLevel is HighlightLevel))
                            DuckGame.Graphics.fadeAdd = 0.0f;
                        Steam.StoreStats();
                    }
                    foreach (Profile profile in Profiles.active)
                        profile.duck = (Duck)null;
                    SFX.StopAllSounds();
                    Level._core.currentLevel.DoInitialize();
                    if (Level._core.currentLevel is XMLLevel && (Level._core.currentLevel as XMLLevel).data != null)
                    {
                        string path = (Level._core.currentLevel as XMLLevel).data.GetPath();
                        if (path != null)
                            DevConsole.Log(DCSection.General, "Level Initialized(" + path + ")");
                    }
                    if (MonoMain.pauseMenu != null && MonoMain.pauseMenu.inWorld)
                        Level._core.currentLevel.AddThing((Thing)MonoMain.pauseMenu);
                    if (Network.isActive && DuckNetwork.duckNetUIGroup != null && DuckNetwork.duckNetUIGroup.open)
                        Level._core.currentLevel.AddThing((Thing)DuckNetwork.duckNetUIGroup);
                    Level.current._networkStatus = NetLevelStatus.WaitingForDataTransfer;
                    if (!(Level._core.currentLevel is IOnlyTransitionIn) && Level._core.currentLevel is IHaveAVirtualTransition && !(Level._core.currentLevel is TeamSelect2) && VirtualTransition.isVirtual)
                    {
                        if (Level.current._readyForTransition)
                        {
                            VirtualTransition.GoUnVirtual();
                            DuckGame.Graphics.fade = 1f;
                        }
                        else
                        {
                            Level.current._waitingOnTransition = true;
                            if (Network.isActive)
                                ConnectionStatusUI.Show();
                        }
                    }
                }
            }
            if (!Level.current._waitingOnTransition || !Level.current._readyForTransition)
                return;
            Level.current._waitingOnTransition = false;
            VirtualTransition.GoUnVirtual();
            if (!Network.isActive)
                return;
            ConnectionStatusUI.Hide();
        }

        public virtual void OnMessage(NetMessage message)
        {
        }

        public virtual void OnNetworkConnecting(Profile p)
        {
        }

        public virtual void OnNetworkConnected(Profile p)
        {
        }

        public virtual void OnNetworkDisconnected(Profile p)
        {
        }

        public virtual void OnSessionEnded(DuckNetErrorInfo error)
        {
            Level.current = error == null ? (Level)new ConnectionError("|RED|Disconnected from game.") : (Level)new ConnectionError(error.message);
            DuckNetwork.core.stopEnteringText = true;
        }

        public virtual void OnDisconnect(NetworkConnection n)
        {
        }

        public virtual void ClientReady(NetworkConnection c)
        {
            if (!this.initializeFunctionHasBeenRun)
                return;
            bool flag = true;
            foreach (Profile profile in DuckNetwork.profiles)
            {
                if (profile.connection != null && (int)profile.connection.levelIndex != (int)DuckNetwork.levelIndex)
                {
                    flag = false;
                    break;
                }
            }
            if (!flag)
                return;
            DevConsole.Log(DCSection.DuckNet, "|DGGREEN|All Clients ready! The level can begin...");
            Send.Message((NetMessage)new NMAllClientsReady());
        }

        public bool calledAllClientsReady => this._calledAllClientsReady;

        public virtual void DoAllClientsReady()
        {
            if (this._calledAllClientsReady)
                return;
            this._calledAllClientsReady = true;
            this.OnAllClientsReady();
        }

        protected virtual void OnAllClientsReady()
        {
            this._networkStatus = NetLevelStatus.Ready;
            Level.current._readyForTransition = true;
            this.DoStart();
        }

        public void TransferComplete(NetworkConnection c)
        {
            this.transferCompleteCalled = true;
            this._networkStatus = NetLevelStatus.WaitingForTransition;
            this.OnTransferComplete(c);
        }

        protected virtual void OnTransferComplete(NetworkConnection c)
        {
        }

        public virtual void SendLevelData(NetworkConnection c)
        {
        }

        public void IgnoreLowestPoint()
        {
            this._lowestPointInitialized = true;
            this.lowestPoint = 999999f;
            this._topLeft = new Vec2(-99999f, -99999f);
            this._bottomRight = new Vec2(99999f, 99999f);
        }

        public void CalculateBounds()
        {
            this._lowestPointInitialized = true;
            CameraBounds cameraBounds = this.FirstOfType<CameraBounds>();
            if (cameraBounds != null)
            {
                this._topLeft = new Vec2(cameraBounds.x - (float)((int)cameraBounds.wide / 2), cameraBounds.y - (float)((int)cameraBounds.high / 2));
                this._bottomRight = new Vec2(cameraBounds.x + (float)((int)cameraBounds.wide / 2), cameraBounds.y + (float)((int)cameraBounds.high / 2));
                this.lowestPoint = this._bottomRight.y;
                this.highestPoint = this._topLeft.y;
            }
            else
            {
                this._topLeft = new Vec2(99999f, 99999f);
                this._bottomRight = new Vec2(-99999f, -99999f);
                foreach (Block block in this._things[typeof(Block)])
                {
                    if (!(block is RockWall) && (double)block.y <= 7500.0)
                    {
                        if ((double)block.right > (double)this._bottomRight.x)
                            this._bottomRight.x = block.right;
                        if ((double)block.left < (double)this._topLeft.x)
                            this._topLeft.x = block.left;
                        if ((double)block.bottom > (double)this._bottomRight.y)
                            this._bottomRight.y = block.bottom;
                        if ((double)block.top < (double)this._topLeft.y)
                            this._topLeft.y = block.top;
                    }
                }
                foreach (AutoPlatform autoPlatform in this._things[typeof(AutoPlatform)])
                {
                    if ((double)autoPlatform.y <= 7500.0)
                    {
                        if ((double)autoPlatform.right > (double)this._bottomRight.x)
                            this._bottomRight.x = autoPlatform.right;
                        if ((double)autoPlatform.left < (double)this._topLeft.x)
                            this._topLeft.x = autoPlatform.left;
                        if ((double)autoPlatform.bottom > (double)this._bottomRight.y)
                            this._bottomRight.y = autoPlatform.bottom;
                        if ((double)autoPlatform.top < (double)this._topLeft.y)
                            this._topLeft.y = autoPlatform.top;
                    }
                }
                this.lowestPoint = this._bottomRight.y;
                this.highestPoint = this.topLeft.y;
            }
        }

        public bool HasChecksumReply(NetworkConnection pConnection)
        {
            bool flag = false;
            this.checksumReplies.TryGetValue(pConnection, out flag);
            return flag;
        }

        public void ChecksumReplied(NetworkConnection pConnection) => this.checksumReplies[pConnection] = true;

        public bool levelIsUpdating => Level._core.nextLevel == null && (!Network.isActive || this._startCalled) && !this._waitingOnTransition && this.transferCompleteCalled;

        public virtual void DoUpdate()
        {
            if (this._updateWaitFrames > 0)
            {
                if (!this._refreshState)
                {
                    this._things.RefreshState();
                    VirtualTransition.Update();
                    this._refreshState = true;
                }
                --this._updateWaitFrames;
                if (this._lowestPointInitialized)
                    return;
                this.CalculateBounds();
            }
            else
            {
                Level currentLevel = Level._core.currentLevel;
                Level._core.currentLevel = this;
                if ((double)DuckGame.Graphics.flashAdd > 0.0)
                    DuckGame.Graphics.flashAdd -= this.flashDissipationSpeed;
                else
                    DuckGame.Graphics.flashAdd = 0.0f;
                if (this._levelStart)
                {
                    DuckGame.Graphics.fade = Lerp.Float(DuckGame.Graphics.fade, 1f, 0.05f);
                    if ((double)DuckGame.Graphics.fade == 1.0)
                        this._levelStart = false;
                }
                if (Level._core.nextLevel == null && this.initializeFunctionHasBeenRun && this.levelMessages.Count > 0)
                {
                    for (int index = 0; index < this.levelMessages.Count; ++index)
                    {
                        Level.doingOnLoadedMessage = true;
                        if (this.levelMessages[index].OnLevelLoaded())
                        {
                            this.levelMessages.RemoveAt(index);
                            --index;
                        }
                        Level.doingOnLoadedMessage = false;
                    }
                }
                if (this.levelIsUpdating)
                {
                    if (this._camera != null)
                        this._camera.DoUpdate();
                    this.Update();
                    Layer.UpdateLayers();
                    this.UpdateThings();
                    this.PostUpdate();
                    this._things.RefreshState();
                    Vote.Update();
                    HUD.Update();
                }
                else
                    this._things.RefreshState();
                if (!this._notifiedReady && this._initialized && !this.waitingOnNewData)
                {
                    DevConsole.Log(DCSection.GhostMan, "Initializing level (" + DuckNetwork.levelIndex.ToString() + ")");
                    if (this._initializeLater)
                    {
                        this._initialized = false;
                        this._initializeLater = false;
                        this.DoInitialize();
                    }
                    this._notifiedReady = true;
                }
                VirtualTransition.Update();
                ConnectionStatusUI.Update();
                if (!this._aiInitialized)
                {
                    AI.InitializeLevelPaths();
                    this._aiInitialized = true;
                }
                if (this.skipCurrentLevelReset)
                    return;
                Level._core.currentLevel = currentLevel;
            }
        }

        public virtual void PostUpdate()
        {
        }

        public virtual void NetworkDebuggerPrepare()
        {
        }

        public virtual void UpdateThings()
        {
            Network.PostDraw();
            IEnumerable<Thing> thing1 = this.things[typeof(IComplexUpdate)];
            if (Network.isActive)
            {
                foreach (Thing thing2 in thing1)
                {
                    if (thing2.shouldRunUpdateLocally)
                        (thing2 as IComplexUpdate).OnPreUpdate();
                }
                foreach (Thing update in this._things.updateList)
                {
                    if (update.active)
                    {
                        if (update.shouldRunUpdateLocally)
                            update.DoUpdate();
                    }
                    else
                        update.InactiveUpdate();
                    if (Level._core.nextLevel != null)
                        break;
                }
                foreach (Thing thing3 in thing1)
                {
                    if (thing3.shouldRunUpdateLocally)
                        (thing3 as IComplexUpdate).OnPostUpdate();
                }
            }
            else
            {
                foreach (Thing thing4 in thing1)
                    (thing4 as IComplexUpdate).OnPreUpdate();
                foreach (Thing update in this._things.updateList)
                {
                    if (update.active && update.level != null)
                        update.DoUpdate();
                    if (Level._core.nextLevel != null)
                        break;
                }
                foreach (Thing thing5 in thing1)
                    (thing5 as IComplexUpdate).OnPostUpdate();
            }
        }

        public virtual void Update()
        {
        }

        public bool clearScreen
        {
            get => this._clearScreen;
            set => this._clearScreen = value;
        }

        public virtual void StartDrawing()
        {
        }

        public virtual void DoDraw()
        {
            this.StartDrawing();
            foreach (IDrawToDifferentLayers toDifferentLayers in this.things[typeof(IDrawToDifferentLayers)])
                toDifferentLayers.OnDrawLayer(Layer.PreDrawLayer);
            Layer.DrawTargetLayers();
            Vec3 vec = this.backgroundColor.ToVector3() * DuckGame.Graphics.fade;
            vec.x += DuckGame.Graphics.flashAddRenderValue;
            vec.y += DuckGame.Graphics.flashAddRenderValue;
            vec.z += DuckGame.Graphics.flashAddRenderValue;
            vec = new Vec3(vec.x + DuckGame.Graphics.fadeAddRenderValue, vec.y + DuckGame.Graphics.fadeAddRenderValue, vec.z + DuckGame.Graphics.fadeAddRenderValue);
            Color color = new Color(vec);
            color.a = this.backgroundColor.a;
            if (this.clearScreen)
            {
                if (!Options.Data.fillBackground)
                {
                    DuckGame.Graphics.Clear(color);
                }
                else
                {
                    DuckGame.Graphics.Clear(Color.Black);
                    DuckGame.Graphics.SetFullViewport();
                    Material material = DuckGame.Graphics.material;
                    DuckGame.Graphics.material = (Material)null;
                    DuckGame.Graphics.screen.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, (MTEffect)null, Matrix.Identity);
                    DuckGame.Graphics.DrawRect(new Vec2(0.0f, 0.0f), new Vec2((float)Resolution.current.x, (float)Resolution.current.y), color, - 1f);
                    DuckGame.Graphics.screen.End();
                    DuckGame.Graphics.material = material;
                    DuckGame.Graphics.RestoreOldViewport();
                }
            }
            if (Recorder.currentRecording != null)
                Recorder.currentRecording.LogBackgroundColor(this.backgroundColor);
            this.BeforeDraw();
            DuckGame.Graphics.screen.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, (MTEffect)null, this.camera.getMatrix());
            this.Draw();
            this.things.Draw();
            DuckGame.Graphics.screen.End();
            if (DevConsole.splitScreen && this is GameLevel)
                SplitScreen.Draw();
            else
                Layer.DrawLayers();
            if (DevConsole.rhythmMode && this is GameLevel)
            {
                DuckGame.Graphics.screen.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, (MTEffect)null, Layer.HUD.camera.getMatrix());
                RhythmMode.Draw();
                DuckGame.Graphics.screen.End();
            }
            this.AfterDrawLayers();
        }

        public virtual void InitializeDraw(Layer l)
        {
            if (l != Layer.HUD || !this._centeredView)
                return;
            float num = (float)((double)Resolution.size.x * (double)DuckGame.Graphics.aspect - (double)Resolution.size.x * (9.0 / 16.0));
            if ((double)num <= 0.0)
                return;
            DuckGame.Graphics.screen.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, (MTEffect)null, Matrix.Identity);
            DuckGame.Graphics.DrawRect(Vec2.Zero, new Vec2(Resolution.size.x, num / 2f), Color.Black, (Depth)0.9f);
            DuckGame.Graphics.DrawRect(new Vec2(0.0f, Resolution.size.y - num / 2f), new Vec2(Resolution.size.x, Resolution.size.y), Color.Black, (Depth)0.9f);
            DuckGame.Graphics.screen.End();
        }

        public virtual void BeforeDraw()
        {
        }

        public virtual void AfterDrawLayers()
        {
        }

        public virtual void Draw()
        {
        }

        public virtual void PreDrawLayer(Layer layer)
        {
        }

        public virtual void PostDrawLayer(Layer layer)
        {
            foreach (IEngineUpdatable engineUpdatable in MonoMain.core.engineUpdatables)
                engineUpdatable.OnDrawLayer(layer);
            foreach (IDrawToDifferentLayers toDifferentLayers in this.things[typeof(IDrawToDifferentLayers)])
                toDifferentLayers.OnDrawLayer(layer);
            if (layer == Layer.Console)
            {
                DevConsole.Draw();
                if (!Network.isActive)
                    return;
                DuckNetwork.Draw();
            }
            else if (layer == Layer.Foreground)
            {
                if ((double)layer.fade <= 0.0)
                    return;
                HUD.DrawForeground();
            }
            else if (layer == Layer.HUD)
            {
                if ((double)layer.fade <= 0.0)
                    return;
                Vote.Draw();
                HUD.Draw();
                ConnectionStatusUI.Draw();
            }
            else
            {
                if (layer == Layer.Lighting)
                    return;
                if (layer == Layer.Glow && Options.Data.fireGlow)
                {
                    foreach (MaterialThing materialThing in this.things[typeof(MaterialThing)])
                    {
                        switch (materialThing)
                        {
                            case Holdable _ when (double)materialThing.heat > 0.300000011920929 && materialThing.physicsMaterial == PhysicsMaterial.Metal:
                                if (this._burnGlow == null)
                                {
                                    this._burnGlow = new Sprite("redHotGlow");
                                    this._burnGlow.CenterOrigin();
                                }
                                this._burnGlow.alpha = (float)((double)Math.Min(materialThing.heat, 1f) / 1.0 - 0.200000002980232);
                                this._burnGlow.scale = new Vec2((materialThing.width + 22f) / (float)this._burnGlow.width, (materialThing.height + 22f) / (float)this._burnGlow.height);
                                Vec2 center = materialThing.rectangle.Center;
                                DuckGame.Graphics.Draw(this._burnGlow, center.x, center.y);
                                DuckGame.Graphics.Draw(this._burnGlow, center.x, center.y);
                                break;
                            case FluidPuddle _:
                                FluidPuddle fluidPuddle = materialThing as FluidPuddle;
                                if ((fluidPuddle.onFire || (double)fluidPuddle.data.heat > 0.5) && (double)fluidPuddle.alpha > 0.5)
                                {
                                    double num1 = (double)fluidPuddle.right - (double)fluidPuddle.left;
                                    float num2 = 16f;
                                    Math.Sin((double)fluidPuddle.fluidWave);
                                    if (this._burnGlowWide == null)
                                    {
                                        this._burnGlowWide = new Sprite("redGlowWideSharp");
                                        this._burnGlowWide.CenterOrigin();
                                        this._burnGlowWide.alpha = 0.75f;
                                        this._burnGlowWideLeft = new Sprite("redGlowWideLeft");
                                        this._burnGlowWideLeft.center = new Vec2((float)this._burnGlowWideLeft.width, (float)(this._burnGlowWideLeft.height / 2));
                                        this._burnGlowWideLeft.alpha = 0.75f;
                                        this._burnGlowWideRight = new Sprite("redGlowWideRight");
                                        this._burnGlowWideRight.center = new Vec2(0.0f, (float)(this._burnGlowWideRight.height / 2));
                                        this._burnGlowWideRight.alpha = 0.75f;
                                    }
                                    double num3 = (double)num2;
                                    int num4 = (int)Math.Floor(num1 / num3);
                                    if ((double)fluidPuddle.collisionSize.y > 8.0)
                                    {
                                        this._burnGlowWide.xscale = 16f;
                                        for (int index = 0; index < num4; ++index)
                                        {
                                            float x = (float)((double)fluidPuddle.bottomLeft.x + (double)index * (double)num2 + 11.0 - 8.0);
                                            float y = fluidPuddle.top - 1f + (float)Math.Sin((double)fluidPuddle.fluidWave + (double)index * 0.699999988079071);
                                            DuckGame.Graphics.Draw(this._burnGlowWide, x, y);
                                            if (index == 0)
                                                DuckGame.Graphics.Draw(this._burnGlowWideLeft, x, y);
                                            else if (index == num4 - 1)
                                                DuckGame.Graphics.Draw(this._burnGlowWideRight, x + 16f, y);
                                        }
                                        break;
                                    }
                                    DuckGame.Graphics.doSnap = false;
                                    this._burnGlowWide.xscale = fluidPuddle.collisionSize.x;
                                    DuckGame.Graphics.Draw(this._burnGlowWide, fluidPuddle.left, fluidPuddle.bottom - 2f);
                                    DuckGame.Graphics.Draw(this._burnGlowWideLeft, fluidPuddle.left, fluidPuddle.bottom - 2f);
                                    DuckGame.Graphics.Draw(this._burnGlowWideRight, fluidPuddle.right, fluidPuddle.bottom - 2f);
                                    DuckGame.Graphics.doSnap = true;
                                    break;
                                }
                                break;
                        }
                        materialThing.DrawGlow();
                    }
                    foreach (SmallFire smallFire in this.things[typeof(SmallFire)])
                    {
                        if (this._burnGlow == null)
                        {
                            this._burnGlow = new Sprite("redGlow");
                            this._burnGlow.CenterOrigin();
                        }
                        this._burnGlow.alpha = 0.65f * smallFire.alpha;
                        DuckGame.Graphics.Draw(this._burnGlow, smallFire.x, smallFire.y - 4f);
                    }
                }
                else if (layer == Layer.Virtual)
                {
                    VirtualTransition.Draw();
                }
                else
                {
                    if (layer != Layer.Game || !NetworkDebugger.enabled || VirtualTransition.active || this is NetworkDebugger)
                        return;
                    NetworkDebugger.DrawInstanceGameDebug();
                }
            }
        }

        public static T Nearest<T>(float x, float y, Thing ignore, Layer layer) => Level.current.NearestThing<T>(new Vec2(x, y), ignore, layer);

        public static T Nearest<T>(float x, float y, Thing ignore) => Level.current.NearestThing<T>(new Vec2(x, y), ignore);

        public static T Nearest<T>(float x, float y) => Level.current.NearestThing<T>(new Vec2(x, y));

        public static T Nearest<T>(Vec2 p) => Level.current.NearestThing<T>(p);

        public static T Nearest<T>(Vec2 point, Thing ignore, int nearIndex, Layer layer) => Level.current.NearestThing<T>(point, ignore, nearIndex, layer);

        public static T Nearest<T>(Vec2 point, Thing ignore, int nearIndex) => Level.current.NearestThing<T>(point, ignore, nearIndex);

        public static T CheckCircle<T>(float p1x, float p1y, float radius, Thing ignore) => Level.current.CollisionCircle<T>(new Vec2(p1x, p1y), radius, ignore);

        public static T CheckCircle<T>(float p1x, float p1y, float radius) => Level.current.CollisionCircle<T>(new Vec2(p1x, p1y), radius);

        public static T CheckCircle<T>(Vec2 p1, float radius, Thing ignore) => Level.current.CollisionCircle<T>(p1, radius, ignore);

        public static T CheckCircle<T>(Vec2 p1, float radius) => Level.current.CollisionCircle<T>(p1, radius);

        public static IEnumerable<T> CheckCircleAll<T>(Vec2 p1, float radius) => Level.current.CollisionCircleAll<T>(p1, radius);

        public T CollisionCircle<T>(float p1x, float p1y, float radius, Thing ignore) => this.CollisionCircle<T>(new Vec2(p1x, p1y), radius, ignore);

        public T CollisionCircle<T>(float p1x, float p1y, float radius) => this.CollisionCircle<T>(new Vec2(p1x, p1y), radius);

        public static T CheckRect<T>(float p1x, float p1y, float p2x, float p2y, Thing ignore) => Level.current.CollisionRect<T>(new Vec2(p1x, p1y), new Vec2(p2x, p2y), ignore);

        public static T CheckRect<T>(float p1x, float p1y, float p2x, float p2y) => Level.current.CollisionRect<T>(new Vec2(p1x, p1y), new Vec2(p2x, p2y));

        public static T CheckRectFilter<T>(Vec2 p1, Vec2 p2, Predicate<T> filter) => Level.current.CollisionRectFilter<T>(p1, p2, filter);

        public static T CheckRect<T>(Vec2 p1, Vec2 p2, Thing ignore) => Level.current.CollisionRect<T>(p1, p2, ignore);

        public static T CheckRect<T>(Vec2 p1, Vec2 p2) => Level.current.CollisionRect<T>(p1, p2);

        public static List<T> CheckRectAll<T>(Vec2 p1, Vec2 p2, List<T> outList) => Level.current.CollisionRectAll<T>(p1, p2, outList);

        public static IEnumerable<T> CheckRectAll<T>(Vec2 p1, Vec2 p2) => (IEnumerable<T>)Level.current.CollisionRectAll<T>(p1, p2, (List<T>)null);

        public T CollisionRect<T>(float p1x, float p1y, float p2x, float p2y, Thing ignore) => this.CollisionRect<T>(new Vec2(p1x, p1y), new Vec2(p2x, p2y), ignore);

        public T CollisionRect<T>(float p1x, float p1y, float p2x, float p2y) => this.CollisionRect<T>(new Vec2(p1x, p1y), new Vec2(p2x, p2y));

        public static T CheckLine<T>(float p1x, float p1y, float p2x, float p2y, Thing ignore) => Level.current.CollisionLine<T>(new Vec2(p1x, p1y), new Vec2(p2x, p2y), ignore);

        public static T CheckLine<T>(float p1x, float p1y, float p2x, float p2y) => Level.current.CollisionLine<T>(new Vec2(p1x, p1y), new Vec2(p2x, p2y));

        public static T CheckLine<T>(
          float p1x,
          float p1y,
          float p2x,
          float p2y,
          out Vec2 position,
          Thing ignore)
        {
            return Level.current.CollisionLine<T>(new Vec2(p1x, p1y), new Vec2(p2x, p2y), out position, ignore);
        }

        public static T CheckLine<T>(float p1x, float p1y, float p2x, float p2y, out Vec2 position) => Level.current.CollisionLine<T>(new Vec2(p1x, p1y), new Vec2(p2x, p2y), out position);

        public static T CheckLine<T>(Vec2 p1, Vec2 p2, Thing ignore) => Level.current.CollisionLine<T>(p1, p2, ignore);

        public static T CheckLine<T>(Vec2 p1, Vec2 p2) => Level.current.CollisionLine<T>(p1, p2);

        public static T CheckLine<T>(Vec2 p1, Vec2 p2, out Vec2 position, Thing ignore) => Level.current.CollisionLine<T>(p1, p2, out position, ignore);

        public static T CheckLine<T>(Vec2 p1, Vec2 p2, out Vec2 position) => Level.current.CollisionLine<T>(p1, p2, out position);

        public T CollisionLine<T>(float p1x, float p1y, float p2x, float p2y, Thing ignore) => this.CollisionLine<T>(new Vec2(p1x, p1y), new Vec2(p2x, p2y), ignore);

        public T CollisionLine<T>(float p1x, float p1y, float p2x, float p2y) => this.CollisionLine<T>(new Vec2(p1x, p1y), new Vec2(p2x, p2y));

        public static IEnumerable<T> CheckLineAll<T>(Vec2 p1, Vec2 p2) => Level.current.CollisionLineAll<T>(p1, p2);

        public IEnumerable<T> CheckLineAll<T>(float p1x, float p1y, float p2x, float p2y) => this.CollisionLineAll<T>(new Vec2(p1x, p1y), new Vec2(p2x, p2y));

        public static T CheckPoint<T>(float x, float y, Thing ignore, Layer layer) => Level.current.CollisionPoint<T>(new Vec2(x, y), ignore, layer);

        public static T CheckPoint<T>(float x, float y, Thing ignore) => Level.current.CollisionPoint<T>(new Vec2(x, y), ignore);

        public static Thing CheckPoint(System.Type pType, float x, float y, Thing ignore) => Level.current.CollisionPoint(pType, new Vec2(x, y), ignore);

        public static T CheckPoint<T>(float x, float y) => Level.current.CollisionPoint<T>(new Vec2(x, y));

        public static T CheckPointPlacementLayer<T>(float x, float y, Thing ignore = null, Layer layer = null) => Level.current.CollisionPointPlacementLayer<T>(new Vec2(x, y), ignore, layer);

        public static T CheckPoint<T>(Vec2 point, Thing ignore, Layer layer) => Level.current.CollisionPoint<T>(point, ignore, layer);

        public static T CheckPoint<T>(Vec2 point, Thing ignore) => Level.current.CollisionPoint<T>(point, ignore);

        public static T CheckPoint<T>(Vec2 point) => Level.current.CollisionPoint<T>(point);

        public static T CheckPointPlacementLayer<T>(Vec2 point, Thing ignore = null, Layer layer = null) => Level.current.CollisionPointPlacementLayer<T>(point, ignore, layer);

        public static IEnumerable<T> CheckPointAll<T>(float x, float y, Layer layer) => Level.current.CollisionPointAll<T>(new Vec2(x, y), layer);

        public static IEnumerable<T> CheckPointAll<T>(float x, float y) => Level.current.CollisionPointAll<T>(new Vec2(x, y));

        public static IEnumerable<T> CheckPointAll<T>(Vec2 point, Layer layer) => Level.current.CollisionPointAll<T>(point, layer);

        public static IEnumerable<T> CheckPointAll<T>(Vec2 point) => Level.current.CollisionPointAll<T>(point);

        public T CollisionPoint<T>(float x, float y, Thing ignore, Layer layer) => this.CollisionPoint<T>(new Vec2(x, y), ignore, layer);

        public T CollisionPoint<T>(float x, float y, Thing ignore) => this.CollisionPoint<T>(new Vec2(x, y), ignore);

        public T CollisionPoint<T>(float x, float y) => this.CollisionPoint<T>(new Vec2(x, y));

        public Thing nearest_single(
          Vec2 point,
          HashSet<Thing> things,
          Thing ignore,
          Layer layer,
          bool placementLayer = false)
        {
            Thing thing1 = (Thing)null;
            float num = float.MaxValue;
            foreach (Thing thing2 in things)
            {
                if (!thing2.removeFromLevel && thing2 != ignore && (layer == null || (placementLayer || thing2.layer == layer) && thing2.placementLayer == layer))
                {
                    float lengthSq = (point - thing2.position).lengthSq;
                    if ((double)lengthSq < (double)num)
                    {
                        num = lengthSq;
                        thing1 = thing2;
                    }
                }
            }
            return thing1;
        }

        public Thing nearest_single(Vec2 point, HashSet<Thing> things, Thing ignore)
        {
            Thing thing1 = (Thing)null;
            float num = float.MaxValue;
            foreach (Thing thing2 in things)
            {
                if (!thing2.removeFromLevel && thing2 != ignore)
                {
                    float lengthSq = (point - thing2.position).lengthSq;
                    if ((double)lengthSq < (double)num)
                    {
                        num = lengthSq;
                        thing1 = thing2;
                    }
                }
            }
            return thing1;
        }

        public Thing nearest_single(Vec2 point, HashSet<Thing> things)
        {
            Thing thing1 = (Thing)null;
            float num = float.MaxValue;
            foreach (Thing thing2 in things)
            {
                if (!thing2.removeFromLevel)
                {
                    float lengthSq = (point - thing2.position).lengthSq;
                    if ((double)lengthSq < (double)num)
                    {
                        num = lengthSq;
                        thing1 = thing2;
                    }
                }
            }
            return thing1;
        }

        public Thing nearest_single(
          Vec2 point,
          IEnumerable<Thing> things,
          Thing ignore,
          Layer layer,
          bool placementLayer = false)
        {
            Thing thing1 = (Thing)null;
            float num = float.MaxValue;
            foreach (Thing thing2 in things)
            {
                if (!thing2.removeFromLevel && thing2 != ignore && (layer == null || (placementLayer || thing2.layer == layer) && thing2.placementLayer == layer))
                {
                    float lengthSq = (point - thing2.position).lengthSq;
                    if ((double)lengthSq < (double)num)
                    {
                        num = lengthSq;
                        thing1 = thing2;
                    }
                }
            }
            return thing1;
        }

        public Thing nearest_single(Vec2 point, IEnumerable<Thing> things, Thing ignore)
        {
            Thing thing1 = (Thing)null;
            float num = float.MaxValue;
            foreach (Thing thing2 in things)
            {
                if (!thing2.removeFromLevel && thing2 != ignore)
                {
                    float lengthSq = (point - thing2.position).lengthSq;
                    if ((double)lengthSq < (double)num)
                    {
                        num = lengthSq;
                        thing1 = thing2;
                    }
                }
            }
            return thing1;
        }

        public Thing nearest_single(Vec2 point, IEnumerable<Thing> things)
        {
            Thing thing1 = (Thing)null;
            float num = float.MaxValue;
            foreach (Thing thing2 in things)
            {
                if (!thing2.removeFromLevel)
                {
                    float lengthSq = (point - thing2.position).lengthSq;
                    if ((double)lengthSq < (double)num)
                    {
                        num = lengthSq;
                        thing1 = thing2;
                    }
                }
            }
            return thing1;
        }

        public List<KeyValuePair<float, Thing>> nearest(
          Vec2 point,
          IEnumerable<Thing> things,
          Thing ignore,
          Layer layer,
          bool placementLayer = false)
        {
            List<KeyValuePair<float, Thing>> keyValuePairList = new List<KeyValuePair<float, Thing>>();
            foreach (Thing thing in things)
            {
                if (!thing.removeFromLevel && thing != ignore && (layer == null || (placementLayer || thing.layer == layer) && thing.placementLayer == layer))
                    keyValuePairList.Add(new KeyValuePair<float, Thing>((point - thing.position).lengthSq, thing));
            }
            keyValuePairList.Sort((Comparison<KeyValuePair<float, Thing>>)((x, y) => (double)x.Key >= (double)y.Key ? 1 : -1));
            return keyValuePairList;
        }

        public List<KeyValuePair<float, Thing>> nearest(
          Vec2 point,
          IEnumerable<Thing> things,
          Thing ignore)
        {
            List<KeyValuePair<float, Thing>> keyValuePairList = new List<KeyValuePair<float, Thing>>();
            foreach (Thing thing in things)
            {
                if (!thing.removeFromLevel && thing != ignore)
                    keyValuePairList.Add(new KeyValuePair<float, Thing>((point - thing.position).lengthSq, thing));
            }
            keyValuePairList.Sort((Comparison<KeyValuePair<float, Thing>>)((x, y) => (double)x.Key >= (double)y.Key ? 1 : -1));
            return keyValuePairList;
        }

        public List<KeyValuePair<float, Thing>> nearest(
          Vec2 point,
          IEnumerable<Thing> things)
        {
            List<KeyValuePair<float, Thing>> keyValuePairList = new List<KeyValuePair<float, Thing>>();
            foreach (Thing thing in things)
            {
                if (!thing.removeFromLevel)
                    keyValuePairList.Add(new KeyValuePair<float, Thing>((point - thing.position).lengthSq, thing));
            }
            keyValuePairList.Sort((Comparison<KeyValuePair<float, Thing>>)((x, y) => (double)x.Key >= (double)y.Key ? 1 : -1));
            return keyValuePairList;
        }

        public T NearestThing<T>(Vec2 point, Thing ignore, Layer layer)
        {
            System.Type key = typeof(T);
            Thing thing = !(key == typeof(Thing)) ? this.nearest_single(point, this._things[key], ignore, layer) : this.nearest_single(point, this._things[typeof(Thing)], ignore, layer);
            return thing == null ? default(T) : (T)(object)thing;
        }

        public T NearestThing<T>(Vec2 point, Thing ignore)
        {
            System.Type key = typeof(T);
            Thing thing = !(key == typeof(Thing)) ? this.nearest_single(point, this._things[key], ignore) : this.nearest_single(point, this._things[typeof(Thing)], ignore);
            return thing == null ? default(T) : (T)(object)thing;
        }

        public T NearestThing<T>(Vec2 point)
        {
            System.Type key = typeof(T);
            Thing thing = !(key == typeof(Thing)) ? this.nearest_single(point, this._things[key]) : this.nearest_single(point, this._things[typeof(Thing)]);
            return thing == null ? default(T) : (T)(object)thing;
        }

        public T NearestThing<T>(Vec2 point, Thing ignore, int nearIndex, Layer layer)
        {
            System.Type key = typeof(T);
            if (key == typeof(Thing))
            {
                List<KeyValuePair<float, Thing>> keyValuePairList = this.nearest(point, this._things[typeof(Thing)], ignore, layer);
                if (keyValuePairList.Count > nearIndex)
                    return (T)(object)keyValuePairList[nearIndex].Value;
            }
            List<KeyValuePair<float, Thing>> keyValuePairList1 = this.nearest(point, this._things[key], ignore, layer);
            return keyValuePairList1.Count > nearIndex ? (T)(object)keyValuePairList1[nearIndex].Value : default(T);
        }

        public T NearestThing<T>(Vec2 point, Thing ignore, int nearIndex)
        {
            System.Type key = typeof(T);
            if (key == typeof(Thing))
            {
                List<KeyValuePair<float, Thing>> keyValuePairList = this.nearest(point, this._things[typeof(Thing)], ignore);
                if (keyValuePairList.Count > nearIndex)
                    return (T)(object)keyValuePairList[nearIndex].Value;
            }
            List<KeyValuePair<float, Thing>> keyValuePairList1 = this.nearest(point, this._things[key], ignore);
            return keyValuePairList1.Count > nearIndex ? (T)(object)keyValuePairList1[nearIndex].Value : default(T);
        }

        public T NearestThingFilter<T>(Vec2 point, Predicate<Thing> filter)
        {
            Thing thing1 = (Thing)null;
            float num = float.MaxValue;
            foreach (Thing thing2 in this.things[typeof(T)])
            {
                if (!thing2.removeFromLevel)
                {
                    float lengthSq = (point - thing2.position).lengthSq;
                    if ((double)lengthSq < (double)num && filter(thing2))
                    {
                        num = lengthSq;
                        thing1 = thing2;
                    }
                }
            }
            return thing1 == null ? default(T) : (T)(object)thing1;
        }

        public T NearestThingFilter<T>(Vec2 point, Predicate<Thing> filter, float maxDistance)
        {
            maxDistance *= maxDistance;
            Thing thing1 = (Thing)null;
            float num = float.MaxValue;
            foreach (Thing thing2 in this.things[typeof(T)])
            {
                if (!thing2.removeFromLevel)
                {
                    float lengthSq = (point - thing2.position).lengthSq;
                    if ((double)lengthSq < (double)num && (double)lengthSq < (double)maxDistance && filter(thing2))
                    {
                        num = lengthSq;
                        thing1 = thing2;
                    }
                }
            }
            return thing1 == null ? default(T) : (T)(object)thing1;
        }

        public T CollisionCircle<T>(Vec2 p1, float radius, Thing ignore)
        {
            System.Type key = typeof(T);
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
            {
                if (!dynamicObject.removeFromLevel && dynamicObject != ignore && Collision.Circle(p1, radius, dynamicObject))
                    return (T)(object)dynamicObject;
            }
            return this._things.HasStaticObjects(key) ? this._things.quadTree.CheckCircle<T>(p1, radius, ignore) : default(T);
        }

        public T CollisionCircle<T>(Vec2 p1, float radius)
        {
            System.Type key = typeof(T);
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
            {
                if (!dynamicObject.removeFromLevel && Collision.Circle(p1, radius, dynamicObject))
                    return (T)(object)dynamicObject;
            }
            return this._things.HasStaticObjects(key) ? this._things.quadTree.CheckCircle<T>(p1, radius) : default(T);
        }

        public IEnumerable<T> CollisionCircleAll<T>(Vec2 p1, float radius)
        {
            List<object> nextCollisionList = Level.GetNextCollisionList();
            System.Type key = typeof(T);
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
            {
                if (!dynamicObject.removeFromLevel && Collision.Circle(p1, radius, dynamicObject))
                    nextCollisionList.Add((object)dynamicObject);
            }
            if (this._things.HasStaticObjects(key))
                this._things.quadTree.CheckCircleAll<T>(p1, radius, nextCollisionList);
            return nextCollisionList.AsEnumerable<object>().Cast<T>();
        }

        public T CollisionRectFilter<T>(Vec2 p1, Vec2 p2, Predicate<T> filter)
        {
            System.Type key = typeof(T);
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
            {
                if (!dynamicObject.removeFromLevel && Collision.Rect(p1, p2, dynamicObject) && filter((T)(object)dynamicObject))
                    return (T)(object)dynamicObject;
            }
            return this._things.HasStaticObjects(key) ? this._things.quadTree.CheckRectangleFilter<T>(p1, p2, filter) : default(T);
        }

        public T CollisionRect<T>(Vec2 p1, Vec2 p2, Thing ignore)
        {
            System.Type key = typeof(T);
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
            {
                if (!dynamicObject.removeFromLevel && dynamicObject != ignore && Collision.Rect(p1, p2, dynamicObject))
                    return (T)(object)dynamicObject;
            }
            return this._things.HasStaticObjects(key) ? this._things.quadTree.CheckRectangle<T>(p1, p2, ignore) : default(T);
        }

        public T CollisionRect<T>(Vec2 p1, Vec2 p2)
        {
            System.Type key = typeof(T);
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
            {
                if (!dynamicObject.removeFromLevel && Collision.Rect(p1, p2, dynamicObject))
                    return (T)(object)dynamicObject;
            }
            return this._things.HasStaticObjects(key) ? this._things.quadTree.CheckRectangle<T>(p1, p2) : default(T);
        }

        public List<T> CollisionRectAll<T>(Vec2 p1, Vec2 p2, List<T> outList)
        {
            List<T> outList1 = outList == null ? new List<T>() : outList;
            System.Type key = typeof(T);
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
            {
                if (!dynamicObject.removeFromLevel && Collision.Rect(p1, p2, dynamicObject))
                    outList1.Add((T)(object)dynamicObject);
            }
            if (this._things.HasStaticObjects(key))
                this._things.quadTree.CheckRectangleAll<T>(p1, p2, outList1);
            return outList1;
        }

        public T CollisionLine<T>(Vec2 p1, Vec2 p2, Thing ignore)
        {
            System.Type key = typeof(T);
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
            {
                if (!dynamicObject.removeFromLevel && dynamicObject != ignore && Collision.Line(p1, p2, dynamicObject))
                    return (T)(object)dynamicObject;
            }
            return this._things.HasStaticObjects(key) ? this._things.quadTree.CheckLine<T>(p1, p2, ignore) : default(T);
        }

        public T CollisionLine<T>(Vec2 p1, Vec2 p2)
        {
            System.Type key = typeof(T);
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
            {
                if (!dynamicObject.removeFromLevel && Collision.Line(p1, p2, dynamicObject))
                    return (T)(object)dynamicObject;
            }
            return this._things.HasStaticObjects(key) ? this._things.quadTree.CheckLine<T>(p1, p2) : default(T);
        }

        public T CollisionLine<T>(Vec2 p1, Vec2 p2, out Vec2 position, Thing ignore)
        {
            position = new Vec2(0.0f, 0.0f);
            System.Type key = typeof(T);
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
            {
                if (!dynamicObject.removeFromLevel && dynamicObject != ignore)
                {
                    Vec2 vec2 = Collision.LinePoint(p1, p2, dynamicObject);
                    if (vec2 != Vec2.Zero)
                    {
                        position = vec2;
                        return (T)(object)dynamicObject;
                    }
                }
            }
            return this._things.HasStaticObjects(key) ? this._things.quadTree.CheckLinePoint<T>(p1, p2, out position, ignore) : default(T);
        }

        public T CollisionLine<T>(Vec2 p1, Vec2 p2, out Vec2 position)
        {
            position = new Vec2(0.0f, 0.0f);
            System.Type key = typeof(T);
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
            {
                if (!dynamicObject.removeFromLevel)
                {
                    Vec2 vec2 = Collision.LinePoint(p1, p2, dynamicObject);
                    if (vec2 != Vec2.Zero)
                    {
                        position = vec2;
                        return (T)(object)dynamicObject;
                    }
                }
            }
            return this._things.HasStaticObjects(key) ? this._things.quadTree.CheckLinePoint<T>(p1, p2, out position) : default(T);
        }

        public IEnumerable<T> CollisionLineAll<T>(Vec2 p1, Vec2 p2)
        {
            List<object> nextCollisionList = Level.GetNextCollisionList();
            System.Type key = typeof(T);
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
            {
                if (!dynamicObject.removeFromLevel && Collision.Line(p1, p2, dynamicObject))
                    nextCollisionList.Add((object)dynamicObject);
            }
            if (this._things.HasStaticObjects(key))
            {
                List<T> source = this._things.quadTree.CheckLineAll<T>(p1, p2);
                nextCollisionList.AddRange(source.Cast<object>());
            }
            return nextCollisionList.AsEnumerable<object>().Cast<T>();
        }

        public T CollisionPoint<T>(Vec2 point, Thing ignore, Layer layer)
        {
            System.Type key = typeof(T);
            if (key == typeof(Thing))
            {
                foreach (Thing thing in this._things)
                {
                    if (!thing.removeFromLevel && thing != ignore && Collision.Point(point, thing) && (layer == null || layer == thing.layer))
                        return (T)(object)thing;
                }
            }
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
            {
                if (!dynamicObject.removeFromLevel && dynamicObject != ignore && Collision.Point(point, dynamicObject) && (layer == null || layer == dynamicObject.layer))
                    return (T)(object)dynamicObject;
            }
            return this._things.HasStaticObjects(key) ? this._things.quadTree.CheckPoint<T>(point, ignore, layer) : default(T);
        }

        public T CollisionPoint<T>(Vec2 point, Thing ignore)
        {
            System.Type key = typeof(T);
            if (key == typeof(Thing))
            {
                foreach (Thing thing in this._things)
                {
                    if (!thing.removeFromLevel && thing != ignore && Collision.Point(point, thing))
                        return (T)(object)thing;
                }
            }
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
            {
                if (!dynamicObject.removeFromLevel && dynamicObject != ignore && Collision.Point(point, dynamicObject))
                    return (T)(object)dynamicObject;
            }
            return this._things.HasStaticObjects(key) ? this._things.quadTree.CheckPoint<T>(point, ignore) : default(T);
        }

        public Thing CollisionPoint(System.Type pType, Vec2 point, Thing ignore)
        {
            if (pType == typeof(Thing))
            {
                foreach (Thing thing in this._things)
                {
                    if (!thing.removeFromLevel && thing != ignore && Collision.Point(point, thing))
                        return thing;
                }
            }
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(pType))
            {
                if (!dynamicObject.removeFromLevel && dynamicObject != ignore && Collision.Point(point, dynamicObject))
                    return dynamicObject;
            }
            return this._things.HasStaticObjects(pType) ? this._things.quadTree.CheckPoint(pType, point, ignore) : (Thing)null;
        }

        public T CollisionPoint<T>(Vec2 point)
        {
            System.Type key = typeof(T);
            if (key == typeof(Thing))
            {
                foreach (Thing thing in this._things)
                {
                    if (!thing.removeFromLevel && Collision.Point(point, thing))
                        return (T)(object)thing;
                }
            }
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
            {
                if (!dynamicObject.removeFromLevel && Collision.Point(point, dynamicObject))
                    return (T)(object)dynamicObject;
            }
            return this._things.HasStaticObjects(key) ? this._things.quadTree.CheckPoint<T>(point) : default(T);
        }

        public T QuadTreePointFilter<T>(Vec2 point, Func<Thing, bool> pFilter) => this._things.HasStaticObjects(typeof(T)) ? this._things.quadTree.CheckPointFilter<T>(point, pFilter) : default(T);

        public Thing CollisionPoint(Vec2 point, System.Type t, Thing ignore, Layer layer)
        {
            if (t == typeof(Thing))
            {
                foreach (Thing thing in this._things)
                {
                    if (!thing.removeFromLevel && thing != ignore && Collision.Point(point, thing) && (layer == null || layer == thing.layer))
                        return thing;
                }
            }
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(t))
            {
                if (!dynamicObject.removeFromLevel && dynamicObject != ignore && Collision.Point(point, dynamicObject) && (layer == null || layer == dynamicObject.layer))
                    return dynamicObject;
            }
            return this._things.HasStaticObjects(t) ? this._things.quadTree.CheckPoint(point, t, ignore, layer) : (Thing)null;
        }

        public Thing CollisionPoint(Vec2 point, System.Type t, Thing ignore)
        {
            if (t == typeof(Thing))
            {
                foreach (Thing thing in this._things)
                {
                    if (!thing.removeFromLevel && thing != ignore && Collision.Point(point, thing))
                        return thing;
                }
            }
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(t))
            {
                if (!dynamicObject.removeFromLevel && dynamicObject != ignore && Collision.Point(point, dynamicObject))
                    return dynamicObject;
            }
            return this._things.HasStaticObjects(t) ? this._things.quadTree.CheckPoint(point, t, ignore) : (Thing)null;
        }

        public Thing CollisionPoint(Vec2 point, System.Type t)
        {
            if (t == typeof(Thing))
            {
                foreach (Thing thing in this._things)
                {
                    if (!thing.removeFromLevel && Collision.Point(point, thing))
                        return thing;
                }
            }
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(t))
            {
                if (!dynamicObject.removeFromLevel && Collision.Point(point, dynamicObject))
                    return dynamicObject;
            }
            return this._things.HasStaticObjects(t) ? this._things.quadTree.CheckPoint(point, t) : (Thing)null;
        }

        public T CollisionPointPlacementLayer<T>(Vec2 point, Thing ignore = null, Layer layer = null)
        {
            System.Type key = typeof(T);
            if (key == typeof(Thing))
            {
                foreach (Thing thing in this._things)
                {
                    if (!thing.removeFromLevel && thing != ignore && Collision.Point(point, thing) && (layer == null || layer == thing.placementLayer))
                        return (T)(object)thing;
                }
            }
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
            {
                if (!dynamicObject.removeFromLevel && dynamicObject != ignore && Collision.Point(point, dynamicObject) && (layer == null || layer == dynamicObject.placementLayer))
                    return (T)(object)dynamicObject;
            }
            return this._things.HasStaticObjects(key) ? this._things.quadTree.CheckPointPlacementLayer<T>(point, ignore, layer) : default(T);
        }

        public T CollisionPointFilter<T>(Vec2 point, Predicate<Thing> filter)
        {
            System.Type key = typeof(T);
            if (key == typeof(Thing))
            {
                foreach (Thing thing in this._things)
                {
                    if (!thing.removeFromLevel && filter(thing) && Collision.Point(point, thing))
                        return (T)(object)thing;
                }
            }
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
            {
                if (!dynamicObject.removeFromLevel && filter(dynamicObject) && Collision.Point(point, dynamicObject))
                    return (T)(object)dynamicObject;
            }
            return this._things.HasStaticObjects(key) ? this._things.quadTree.CheckPointFilter<T>(point, filter) : default(T);
        }

        public IEnumerable<T> CollisionPointAll<T>(Vec2 point, Layer layer)
        {
            List<object> nextCollisionList = Level.GetNextCollisionList();
            System.Type key = typeof(T);
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
            {
                if (!dynamicObject.removeFromLevel && Collision.Point(point, dynamicObject) && (layer == null || layer == dynamicObject.layer))
                    nextCollisionList.Add((object)dynamicObject);
            }
            if (this._things.HasStaticObjects(key))
            {
                T obj = this._things.quadTree.CheckPoint<T>(point, (Thing)null, layer);
                if ((object)obj != null)
                    nextCollisionList.Add((object)obj);
            }
            return nextCollisionList.AsEnumerable<object>().Cast<T>();
        }

        public IEnumerable<T> CollisionPointAll<T>(Vec2 point)
        {
            List<object> nextCollisionList = Level.GetNextCollisionList();
            System.Type key = typeof(T);
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
            {
                if (!dynamicObject.removeFromLevel && Collision.Point(point, dynamicObject))
                    nextCollisionList.Add((object)dynamicObject);
            }
            if (this._things.HasStaticObjects(key))
            {
                T obj = this._things.quadTree.CheckPoint<T>(point);
                if ((object)obj != null)
                    nextCollisionList.Add((object)obj);
            }
            return nextCollisionList.AsEnumerable<object>().Cast<T>();
        }

        public void CollisionBullet(Vec2 point, List<MaterialThing> output)
        {
            System.Type key = typeof(MaterialThing);
            foreach (Thing dynamicObject in this._things.GetDynamicObjects(key))
            {
                if (!dynamicObject.removeFromLevel && Collision.Point(point, dynamicObject))
                    output.Add(dynamicObject as MaterialThing);
            }
            if (!this._things.HasStaticObjects(key))
                return;
            MaterialThing materialThing = this._things.quadTree.CheckPoint<MaterialThing>(point);
            if (materialThing == null)
                return;
            output.Add(materialThing);
        }

        public static T CheckRay<T>(Vec2 start, Vec2 end) => Level.current.CollisionRay<T>(start, end);

        public T CollisionRay<T>(Vec2 start, Vec2 end) => Level.CheckRay<T>(start, end, out Vec2 _);

        public static T CheckRay<T>(Vec2 start, Vec2 end, out Vec2 hitPos) => Level.current.CollisionRay<T>(start, end, out hitPos);

        public static T CheckRay<T>(Vec2 start, Vec2 end, Thing ignore, out Vec2 hitPos) => Level.current.CollisionRay<T>(start, end, ignore, out hitPos);

        public T CollisionRay<T>(Vec2 start, Vec2 end, out Vec2 hitPos)
        {
            Vec2 dir = end - start;
            float length = dir.length;
            dir.Normalize();
            Math.Ceiling((double)length);
            Stack<TravelInfo> travelInfoStack = new Stack<TravelInfo>();
            travelInfoStack.Push(new TravelInfo(start, end, length));
            while (travelInfoStack.Count > 0)
            {
                TravelInfo travelInfo = travelInfoStack.Pop();
                if ((object)Level.current.CollisionLine<T>(travelInfo.p1, travelInfo.p2) != null)
                {
                    if ((double)travelInfo.length < 8.0)
                    {
                        T obj = this.Raycast<T>(travelInfo.p1, dir, travelInfo.length, out hitPos);
                        if ((object)obj != null)
                            return obj;
                    }
                    else
                    {
                        float len = travelInfo.length * 0.5f;
                        Vec2 vec2 = travelInfo.p1 + dir * len;
                        travelInfoStack.Push(new TravelInfo(vec2, travelInfo.p2, len));
                        travelInfoStack.Push(new TravelInfo(travelInfo.p1, vec2, len));
                    }
                }
            }
            hitPos = end;
            return default(T);
        }

        public T CollisionRay<T>(Vec2 start, Vec2 end, Thing ignore, out Vec2 hitPos)
        {
            Vec2 dir = end - start;
            float length = dir.length;
            dir.Normalize();
            Math.Ceiling((double)length);
            Stack<TravelInfo> travelInfoStack = new Stack<TravelInfo>();
            travelInfoStack.Push(new TravelInfo(start, end, length));
            while (travelInfoStack.Count > 0)
            {
                TravelInfo travelInfo = travelInfoStack.Pop();
                if ((object)Level.current.CollisionLine<T>(travelInfo.p1, travelInfo.p2, ignore) != null)
                {
                    if ((double)travelInfo.length < 8.0)
                    {
                        T obj = this.Raycast<T>(travelInfo.p1, dir, ignore, travelInfo.length, out hitPos);
                        if ((object)obj != null)
                            return obj;
                    }
                    else
                    {
                        float len = travelInfo.length * 0.5f;
                        Vec2 vec2 = travelInfo.p1 + dir * len;
                        travelInfoStack.Push(new TravelInfo(vec2, travelInfo.p2, len));
                        travelInfoStack.Push(new TravelInfo(travelInfo.p1, vec2, len));
                    }
                }
            }
            hitPos = end;
            return default(T);
        }

        private T Raycast<T>(Vec2 p1, Vec2 dir, float length, out Vec2 hit)
        {
            int num = (int)Math.Ceiling((double)length);
            Vec2 point = p1;
            do
            {
                --num;
                T obj = Level.current.CollisionPoint<T>(point);
                if ((object)obj != null)
                {
                    hit = point;
                    return obj;
                }
                point += dir;
            }
            while (num > 0);
            hit = point;
            return default(T);
        }

        private T Raycast<T>(Vec2 p1, Vec2 dir, Thing ignore, float length, out Vec2 hit)
        {
            int num = (int)Math.Ceiling((double)length);
            Vec2 point = p1;
            do
            {
                --num;
                T obj = Level.current.CollisionPoint<T>(point, ignore);
                if ((object)obj != null)
                {
                    hit = point;
                    return obj;
                }
                point += dir;
            }
            while (num > 0);
            hit = point;
            return default(T);
        }

        private T Rectcast<T>(Vec2 p1, Vec2 p2, Rectangle rect, out Vec2 hit)
        {
            Vec2 vec2_1 = p2 - p1;
            int num = (int)Math.Ceiling((double)vec2_1.length);
            vec2_1.Normalize();
            Vec2 vec2_2 = p1;
            do
            {
                --num;
                T obj = Level.current.CollisionRect<T>(vec2_2 + new Vec2(rect.Top, rect.Left), vec2_2 + new Vec2(rect.Bottom, rect.Right));
                if ((object)obj != null)
                {
                    hit = vec2_2;
                    return obj;
                }
                vec2_2 += vec2_1;
            }
            while (num > 0);
            hit = vec2_2;
            return default(T);
        }
    }
}