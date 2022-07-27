﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Resolution
// Assembly: DuckGame, Version=1.1.8175.33388, Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DuckGame
{
    public class Resolution
    {
        public bool recommended;
        private static Resolution _lastApplied;
        private static Form _window;
        private static float _screenDPI;
        private static int _takeFocus;
        private static GraphicsDeviceManager _device;
        private static Resolution _pendingResolution;
        private static Matrix _matrix;
        public bool isDefault;
        public ScreenMode mode;
        public Vec2 dimensions;
        public static Dictionary<ScreenMode, List<Resolution>> supportedDisplaySizes;

        public static Resolution adapterResolution => Resolution.GetDefault(ScreenMode.Fullscreen);

        public static Resolution current => Options.LocalData.currentResolution;

        public static float fontSizeMultiplier => (float)Resolution.current.x / 1280f;

        public static Resolution lastApplied => Resolution._lastApplied;

        public static Vec2 size => DuckGame.Graphics._screenViewport.HasValue ? new Vec2((float)DuckGame.Graphics._screenViewport.Value.Width, (float)DuckGame.Graphics._screenViewport.Value.Height) : Resolution.current.dimensions;

        private static float GetScreenDPI()
        {
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromHwnd(IntPtr.Zero);
            float dpiX = graphics.DpiX;
            graphics.Dispose();
            return dpiX;
        }

        public static void Set(Resolution pResolution) => Resolution._pendingResolution = pResolution;

        public static void Apply()
        {
            DuckGame.Graphics.snap = 4f;
            if (Resolution._pendingResolution == null || Program.isLinux && !Keyboard.NothingPressed())
                return;
            Resolution._lastApplied = Resolution._pendingResolution;
            Options.LocalData.currentResolution = Resolution._pendingResolution;
            DevConsole.Log(DCSection.General, "Applying resolution (" + Resolution._pendingResolution.ToString() + ")");
            bool flag = false;
            foreach (DisplayMode supportedDisplayMode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if (Resolution._pendingResolution.x <= supportedDisplayMode.Width && Resolution._pendingResolution.y <= supportedDisplayMode.Height)
                {
                    flag = true;
                    if (Resolution._pendingResolution.mode == ScreenMode.Borderless)
                    {
                        Resolution._device.PreferredBackBufferWidth = Resolution.adapterResolution.x;
                        Resolution._device.PreferredBackBufferHeight = Resolution.adapterResolution.y;
                    }
                    else
                    {
                        Resolution._device.PreferredBackBufferWidth = Resolution._pendingResolution.x;
                        Resolution._device.PreferredBackBufferHeight = Resolution._pendingResolution.y;
                    }
                    Resolution._device.IsFullScreen = Resolution._pendingResolution.mode == ScreenMode.Fullscreen;
                    Resolution._device.ApplyChanges();
                    break;
                }
            }
            if (!flag)
            {
                Resolution.RestoreDefaults();
                Resolution.Apply();
            }
            else
            {
                switch (Options.LocalData.currentResolution.mode)
                {
                    case ScreenMode.Windowed:
                        DuckGame.Graphics.mouseVisible = false;
                        DuckGame.Graphics._screenBufferTarget = (RenderTarget2D)null;
                        Resolution._window.FormBorderStyle = FormBorderStyle.FixedSingle;
                        Resolution._window.Location = new System.Drawing.Point(Resolution.adapterResolution.x / 2 - Options.LocalData.currentResolution.x / 2, Resolution.adapterResolution.y / 2 - Options.LocalData.currentResolution.y / 2 - 16);
                        break;
                    case ScreenMode.Fullscreen:
                        DuckGame.Graphics.mouseVisible = false;
                        DuckGame.Graphics._screenBufferTarget = (RenderTarget2D)null;
                        break;
                    case ScreenMode.Borderless:
                        DuckGame.Graphics.mouseVisible = false;
                        DuckGame.Graphics._screenBufferTarget = new RenderTarget2D(Options.LocalData.currentResolution.x, Options.LocalData.currentResolution.y, true, RenderTargetUsage.PreserveContents);
                        Resolution._window.FormBorderStyle = FormBorderStyle.None;
                        Resolution._window.Location = new System.Drawing.Point(0, 0);
                        if (DuckGame.Graphics._screenBufferTarget.width < 400)
                        {
                            DuckGame.Graphics.snap = 1f;
                            break;
                        }
                        if (DuckGame.Graphics._screenBufferTarget.width < 800)
                        {
                            DuckGame.Graphics.snap = 2f;
                            break;
                        }
                        break;
                }
                MonoMain._screenCapture = new RenderTarget2D(Resolution.current.x, Resolution.current.y, true);
                MonoMain.RetakePauseCapture();
                LayerCore.ReinitializeLightingTargets();
                Options.ResolutionChanged();
                if (NetworkDebugger.enabled)
                    NetworkDebugger.instance.RefreshRectSizes();
                if (Layer.Game != null && Layer.Game.camera != null)
                    Layer.Game.camera.DoUpdate();
                if (Program.isLinux)
                    Resolution._takeFocus = 10;
                Resolution._pendingResolution = (Resolution)null;
                DuckGame.Graphics._screenViewport = new Viewport?();
            }
        }

        public static bool Update()
        {
            if (Resolution._takeFocus > 0)
            {
                --Resolution._takeFocus;
                if (Resolution._takeFocus == 0)
                    Resolution._window.Focus();
            }
            if (Resolution._pendingResolution == null)
                return false;
            Resolution.Apply();
            return true;
        }

        public static Matrix getTransformationMatrix()
        {
            Viewport viewport = DuckGame.Graphics.viewport;
            double xScale = (double)viewport.Width / (double)MonoMain.screenWidth;
            viewport = DuckGame.Graphics.viewport;
            double yScale = (double)viewport.Height / (double)MonoMain.screenHeight;
            return Matrix.CreateScale((float)xScale, (float)yScale, 1f);
        }

        public float aspect => this.dimensions.x / this.dimensions.y;

        public int x
        {
            get => (int)this.dimensions.x;
            set => this.dimensions.x = (float)value;
        }

        public int y
        {
            get => (int)this.dimensions.y;
            set => this.dimensions.y = (float)value;
        }

        public static void Initialize(object pWindow, GraphicsDeviceManager pDeviceManager)
        {
            Resolution._window = pWindow as Form;
            Resolution._device = pDeviceManager;
            Resolution.supportedDisplaySizes = new Dictionary<ScreenMode, List<Resolution>>();
            DevConsole.Log(DCSection.General, "Enumerating display modes (" + GraphicsAdapter.DefaultAdapter.SupportedDisplayModes.Count<DisplayMode>().ToString() + " found...)");
            if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode == null)
                throw new Exception("No graphics display modes found, your graphics card may not be supported!");
            DevConsole.Log(DCSection.General, "Default adapter size is (" + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width.ToString() + "x" + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height.ToString() + ")");
            DevConsole.Log(DCSection.General, "Registered adapter size is (" + MonoMain.instance._adapterW.ToString() + "x" + MonoMain.instance._adapterH.ToString() + ")");
            Resolution.RegisterDisplaySize(ScreenMode.Fullscreen, new Resolution()
            {
                dimensions = new Vec2((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            }, false);
            Resolution.RegisterDisplaySize(ScreenMode.Borderless, new Resolution()
            {
                dimensions = new Vec2((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            }, false);
            Resolution.RegisterDisplaySize(ScreenMode.Windowed, new Resolution()
            {
                dimensions = new Vec2((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            }, false);
            foreach (DisplayMode supportedDisplayMode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                Resolution.RegisterDisplaySize(ScreenMode.Fullscreen, new Resolution()
                {
                    dimensions = new Vec2((float)supportedDisplayMode.Width, (float)supportedDisplayMode.Height)
                }, false);
                if (supportedDisplayMode.Width <= MonoMain.instance._adapterW && supportedDisplayMode.Height <= MonoMain.instance._adapterH)
                {
                    Resolution.RegisterDisplaySize(ScreenMode.Windowed, new Resolution()
                    {
                        dimensions = new Vec2((float)supportedDisplayMode.Width, (float)supportedDisplayMode.Height)
                    }, false);
                    Resolution.RegisterDisplaySize(ScreenMode.Borderless, new Resolution()
                    {
                        dimensions = new Vec2((float)supportedDisplayMode.Width, (float)supportedDisplayMode.Height)
                    }, false);
                }
            }
            Resolution.RegisterDisplaySize(ScreenMode.Borderless, new Resolution()
            {
                dimensions = new Vec2(640f, 360f)
            }, false);
            Resolution.RegisterDisplaySize(ScreenMode.Borderless, new Resolution()
            {
                dimensions = new Vec2(320f, 180f)
            }, false);
            Resolution.RegisterDisplaySize(ScreenMode.Windowed, new Resolution()
            {
                dimensions = new Vec2(1280f, 720f)
            }, false, true);
            Resolution.RegisterDisplaySize(ScreenMode.Windowed, new Resolution()
            {
                dimensions = new Vec2(1920f, 1080f)
            }, false, true);
            Resolution.RegisterDisplaySize(ScreenMode.Windowed, new Resolution()
            {
                dimensions = new Vec2(2560f, 1440f)
            }, false, true);
            Resolution.RegisterDisplaySize(ScreenMode.Windowed, new Resolution()
            {
                dimensions = new Vec2(2880f, 1620f)
            }, false, true);
            DevConsole.Log(DCSection.General, "Finished enumerating display modes (F(" + Resolution.supportedDisplaySizes[ScreenMode.Fullscreen].Count.ToString() + ") W(" + Resolution.supportedDisplaySizes[ScreenMode.Windowed].Count.ToString() + ") B(" + Resolution.supportedDisplaySizes[ScreenMode.Borderless].Count.ToString() + "))");
            Resolution.SortDisplaySizes();
            string[] strArray = new string[7]
            {
        "Finished sorting display modes (F(",
        Resolution.supportedDisplaySizes[ScreenMode.Fullscreen].Count.ToString(),
        ") W(",
        null,
        null,
        null,
        null
            };
            int count = Resolution.supportedDisplaySizes[ScreenMode.Windowed].Count;
            strArray[3] = count.ToString();
            strArray[4] = ") B(";
            count = Resolution.supportedDisplaySizes[ScreenMode.Borderless].Count;
            strArray[5] = count.ToString();
            strArray[6] = "))";
            DevConsole.Log(DCSection.General, string.Concat(strArray));
            if (Resolution.supportedDisplaySizes[ScreenMode.Windowed].Count == 0 && Resolution.supportedDisplaySizes[ScreenMode.Fullscreen].Count > 0)
            {
                Resolution resolution = Resolution.supportedDisplaySizes[ScreenMode.Fullscreen].LastOrDefault<Resolution>();
                Resolution.RegisterDisplaySize(ScreenMode.Windowed, new Resolution()
                {
                    dimensions = resolution.dimensions
                }, false, pForce: true);
                Resolution.RegisterDisplaySize(ScreenMode.Borderless, new Resolution()
                {
                    dimensions = resolution.dimensions
                }, false, pForce: true);
            }
            Resolution.FindNearest(ScreenMode.Fullscreen, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height).isDefault = true;
            Resolution.FindNearest(ScreenMode.Windowed, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, 1.7777f, true).isDefault = true;
            Resolution.FindNearest(ScreenMode.Borderless, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height).isDefault = true;
            Resolution.RestoreDefaults();
            try
            {
                Resolution._screenDPI = Resolution.GetScreenDPI();
            }
            catch (Exception ex)
            {
                Resolution._screenDPI = 120f;
            }
        }

        public static void RestoreDefaults()
        {
            Options.LocalData.fullscreenResolution = Resolution.GetDefault(ScreenMode.Fullscreen);
            Options.LocalData.windowedResolution = Resolution.GetDefault(ScreenMode.Windowed);
            Options.LocalData.windowedFullscreenResolution = Resolution.GetDefault(ScreenMode.Borderless);
        }

        public override string ToString() => this.x.ToString() + "x" + this.y.ToString() + "x" + ((int)this.mode).ToString();

        public string ToShortString()
        {
            int num = this.x;
            string str1 = num.ToString();
            num = this.y;
            string str2 = num.ToString();
            return str1 + "x" + str2;
        }

        public static Resolution Load(string pSize, string pMemberName = null)
        {
            if (Resolution.supportedDisplaySizes == null)
                return (Resolution)null;
            try
            {
                string[] strArray = pSize.ToLower().Trim().Split('x');
                if (strArray.Length >= 2)
                {
                    int int32_1 = Convert.ToInt32(strArray[0]);
                    int int32_2 = Convert.ToInt32(strArray[1]);
                    ScreenMode pMode = ScreenMode.Windowed;
                    if (strArray.Length == 3)
                        pMode = (ScreenMode)Convert.ToInt32(strArray[2]);
                    else if (pMemberName == "windowedResolution")
                        pMode = ScreenMode.Windowed;
                    else if (pMemberName == "windowedFullscreenResolution")
                        pMode = ScreenMode.Borderless;
                    else if (pMemberName == "fullscreenResolution")
                        pMode = ScreenMode.Fullscreen;
                    Resolution nearest = Resolution.FindNearest(pMode, int32_1, int32_2);
                    if (nearest != null)
                        return nearest;
                }
            }
            catch (Exception ex)
            {
                DevConsole.Log(DCSection.General, "Failed to load resolution (" + pSize + ")");
            }
            return (Resolution)null;
        }

        public static Resolution GetDefault(ScreenMode pMode) => Resolution.supportedDisplaySizes[pMode].FirstOrDefault<Resolution>((Func<Resolution, bool>)(x => x.isDefault)) ?? Resolution.supportedDisplaySizes[pMode].Last<Resolution>();

        public static Resolution FindNearest(
          ScreenMode pMode,
          int pX,
          int pY,
          float pAspect = -1f,
          bool pRecommended = false)
        {
            Resolution nearestInternal = Resolution.FindNearest_Internal(pMode, pX, pY, pAspect, pRecommended);
            if (nearestInternal == null)
            {
                ScreenMode pMode1 = (ScreenMode)((int)(pMode + 1) % 3);
                while (pMode1 != pMode && nearestInternal == null)
                    nearestInternal = Resolution.FindNearest_Internal(pMode1, pX, pY, pAspect, pRecommended);
            }
            if (nearestInternal == null)
                DevConsole.Log(DCSection.General, "Failed to find display mode (" + pMode.ToString() + ", " + pX.ToString() + "x" + pY.ToString() + ")");
            return nearestInternal;
        }

        private static Resolution FindNearest_Internal(
          ScreenMode pMode,
          int pX,
          int pY,
          float pAspect = -1f,
          bool pRecommended = false)
        {
            Resolution nearestInternal = Resolution.supportedDisplaySizes[pMode].LastOrDefault<Resolution>();
            foreach (Resolution resolution in Resolution.supportedDisplaySizes[pMode])
            {
                float num1 = Math.Abs(resolution.aspect - pAspect);
                float num2 = Math.Abs(nearestInternal.aspect - pAspect);
                bool flag1 = (double)num1 < (double)num2 && (double)Math.Abs(num1 - num2) > 0.0500000007450581;
                bool flag2 = (double)Math.Abs(num1 - num2) < 0.0500000007450581;
                if ((nearestInternal == null || Math.Abs(resolution.x - pX) + Math.Abs(resolution.y - pY) < Math.Abs(nearestInternal.x - pX) + Math.Abs(nearestInternal.y - pY) || flag1 && (double)pAspect > 0.0 || pRecommended && resolution.recommended && !nearestInternal.recommended) && (nearestInternal == null || (double)pAspect < 0.0 || flag1 | flag2 && (resolution.recommended || !pRecommended)))
                    nearestInternal = resolution;
            }
            return nearestInternal;
        }

        internal static Resolution RegisterDisplaySize(
          ScreenMode pMode,
          Resolution pResolution,
          bool pSort = true,
          bool pRecommended = false,
          bool pForce = false)
        {
            List<Resolution> resolutionList = (List<Resolution>)null;
            if (!Resolution.supportedDisplaySizes.TryGetValue(pMode, out resolutionList))
                Resolution.supportedDisplaySizes[pMode] = resolutionList = new List<Resolution>();
            if (pResolution.x > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width || pResolution.y > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            {
                DevConsole.Log("Invalid resolution (" + pResolution.ToString() + "): Larger than adapter (" + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width.ToString() + "x" + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height.ToString() + ")");
                return (Resolution)null;
            }
            if (pMode == ScreenMode.Windowed && pResolution.x == MonoMain.instance._adapterW && pResolution.y == MonoMain.instance._adapterH && !pForce)
            {
                DevConsole.Log("Invalid resolution (" + pResolution.ToString() + "): Windowed resolution must not equal adapter size (" + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width.ToString() + "x" + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height.ToString() + ")");
                return (Resolution)null;
            }
            Resolution resolution = resolutionList.FirstOrDefault<Resolution>((Func<Resolution, bool>)(x => x.x == pResolution.x && x.y == pResolution.y));
            if (resolution == null)
                resolutionList.Add(pResolution);
            else
                pResolution = resolution;
            if (pSort)
                Resolution.supportedDisplaySizes[pMode] = Resolution.SortDisplaySizes(resolutionList);
            pResolution.mode = pMode;
            pResolution.recommended = pRecommended;
            return pResolution;
        }

        internal static void SortDisplaySizes()
        {
            Resolution.supportedDisplaySizes[ScreenMode.Windowed] = Resolution.SortDisplaySizes(Resolution.supportedDisplaySizes[ScreenMode.Windowed]);
            Resolution.supportedDisplaySizes[ScreenMode.Borderless] = Resolution.SortDisplaySizes(Resolution.supportedDisplaySizes[ScreenMode.Borderless]);
            Resolution.supportedDisplaySizes[ScreenMode.Fullscreen] = Resolution.SortDisplaySizes(Resolution.supportedDisplaySizes[ScreenMode.Fullscreen]);
        }

        internal static List<Resolution> SortDisplaySizes(List<Resolution> pList) => pList == null ? new List<Resolution>() : pList.OrderBy<Resolution, int>((Func<Resolution, int>)(x => x.x * 100 + x.y)).ToList<Resolution>();
    }
}