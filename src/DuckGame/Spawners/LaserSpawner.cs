﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.LaserSpawner
// Assembly: DuckGame, Version=1.1.8175.33388, Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using System;

namespace DuckGame
{
    [EditorGroup("Spawns")]
    [BaggedProperty("isInDemo", false)]
    public class LaserSpawner : Thing
    {
        protected float _spawnWait;
        public float initialDelay;
        public float spawnTime = 10f;
        public bool spawnOnStart = true;
        public int spawnNum = -1;
        protected int _numSpawned;
        public float fireDirection;
        public float firePower = 1f;

        public float direction => this.fireDirection + (this.flipHorizontal ? 180f : 0.0f);

        public LaserSpawner(float xpos, float ypos, System.Type c = null)
          : base(xpos, ypos)
        {
            this.graphic = new Sprite("laserSpawner");
            this.center = new Vec2(8f, 8f);
            this.collisionSize = new Vec2(12f, 12f);
            this.collisionOffset = new Vec2(-6f, -6f);
            this.depth = (Depth)0.99f;
            this.hugWalls = WallHug.None;
            this._visibleInGame = false;
            this.editorTooltip = "Spawns Quad Laser bullets in the specified direction.";
        }

        public override void Initialize()
        {
            if (!this.spawnOnStart)
                return;
            this._spawnWait = this.spawnTime;
        }

        public override void Update()
        {
            if (Level.current.simulatePhysics)
                this._spawnWait += 0.0166666f;
            if (Level.current.simulatePhysics && Network.isServer && (this._numSpawned < this.spawnNum || this.spawnNum == -1) && (double)this._spawnWait >= (double)this.spawnTime)
            {
                if ((double)this.initialDelay > 0.0)
                {
                    this.initialDelay -= 0.0166666f;
                }
                else
                {
                    Vec2 travel = Maths.AngleToVec(Maths.DegToRad(this.direction)) * this.firePower;
                    Vec2 vec2 = this.position - travel.normalized * 16f;
                    Level.Add((Thing)new QuadLaserBullet(vec2.x, vec2.y, travel));
                    this._spawnWait = 0.0f;
                    ++this._numSpawned;
                }
            }
            this.angleDegrees = -this.direction;
        }

        public override void Terminate()
        {
        }

        public override BinaryClassChunk Serialize()
        {
            BinaryClassChunk binaryClassChunk = base.Serialize();
            binaryClassChunk.AddProperty("spawnTime", (object)this.spawnTime);
            binaryClassChunk.AddProperty("initialDelay", (object)this.initialDelay);
            binaryClassChunk.AddProperty("spawnOnStart", (object)this.spawnOnStart);
            binaryClassChunk.AddProperty("spawnNum", (object)this.spawnNum);
            binaryClassChunk.AddProperty("fireDirection", (object)this.fireDirection);
            binaryClassChunk.AddProperty("firePower", (object)this.firePower);
            return binaryClassChunk;
        }

        public override bool Deserialize(BinaryClassChunk node)
        {
            base.Deserialize(node);
            this.spawnTime = node.GetProperty<float>("spawnTime");
            this.initialDelay = node.GetProperty<float>("initialDelay");
            this.spawnOnStart = node.GetProperty<bool>("spawnOnStart");
            this.spawnNum = node.GetProperty<int>("spawnNum");
            this.fireDirection = node.GetProperty<float>("fireDirection");
            this.firePower = node.GetProperty<float>("firePower");
            return true;
        }

        public override DXMLNode LegacySerialize()
        {
            DXMLNode dxmlNode = base.LegacySerialize();
            dxmlNode.Add(new DXMLNode("spawnTime", (object)Change.ToString((object)this.spawnTime)));
            dxmlNode.Add(new DXMLNode("initialDelay", (object)Change.ToString((object)this.initialDelay)));
            dxmlNode.Add(new DXMLNode("spawnOnStart", (object)Change.ToString((object)this.spawnOnStart)));
            dxmlNode.Add(new DXMLNode("spawnNum", (object)Change.ToString((object)this.spawnNum)));
            dxmlNode.Add(new DXMLNode("fireDirection", (object)Change.ToString((object)this.fireDirection)));
            dxmlNode.Add(new DXMLNode("firePower", (object)Change.ToString((object)this.firePower)));
            return dxmlNode;
        }

        public override bool LegacyDeserialize(DXMLNode node)
        {
            base.LegacyDeserialize(node);
            DXMLNode dxmlNode1 = node.Element("spawnTime");
            if (dxmlNode1 != null)
                this.spawnTime = Change.ToSingle((object)dxmlNode1.Value);
            DXMLNode dxmlNode2 = node.Element("initialDelay");
            if (dxmlNode2 != null)
                this.initialDelay = Change.ToSingle((object)dxmlNode2.Value);
            DXMLNode dxmlNode3 = node.Element("spawnOnStart");
            if (dxmlNode3 != null)
                this.spawnOnStart = Convert.ToBoolean(dxmlNode3.Value);
            DXMLNode dxmlNode4 = node.Element("spawnNum");
            if (dxmlNode4 != null)
                this.spawnNum = Convert.ToInt32(dxmlNode4.Value);
            DXMLNode dxmlNode5 = node.Element("fireDirection");
            if (dxmlNode5 != null)
                this.fireDirection = Convert.ToSingle(dxmlNode5.Value);
            DXMLNode dxmlNode6 = node.Element("firePower");
            if (dxmlNode6 != null)
                this.firePower = Convert.ToSingle(dxmlNode6.Value);
            return true;
        }

        public override ContextMenu GetContextMenu()
        {
            EditorGroupMenu contextMenu = base.GetContextMenu() as EditorGroupMenu;
            contextMenu.AddItem((ContextMenu)new ContextSlider("Delay", (IContextListener)null, new FieldBinding((object)this, "spawnTime", 1f, 100f)));
            contextMenu.AddItem((ContextMenu)new ContextSlider("Initial Delay", (IContextListener)null, new FieldBinding((object)this, "initialDelay", max: 100f)));
            contextMenu.AddItem((ContextMenu)new ContextCheckBox("Start Spawned", (IContextListener)null, new FieldBinding((object)this, "spawnOnStart")));
            contextMenu.AddItem((ContextMenu)new ContextSlider("Number", (IContextListener)null, new FieldBinding((object)this, "spawnNum", -1f, 100f), 1f, "INF"));
            contextMenu.AddItem((ContextMenu)new ContextSlider("Angle", (IContextListener)null, new FieldBinding((object)this, "fireDirection", max: 360f), 1f));
            contextMenu.AddItem((ContextMenu)new ContextSlider("Power", (IContextListener)null, new FieldBinding((object)this, "firePower", 1f, 20f)));
            return (ContextMenu)contextMenu;
        }

        public override void DrawHoverInfo() => Graphics.DrawLine(this.position, this.position + Maths.AngleToVec(Maths.DegToRad(this.direction)) * (this.firePower * 5f), Color.Red, 2f, (Depth)1f);

        public override void Draw()
        {
            this.angleDegrees = -this.direction;
            base.Draw();
        }
    }
}