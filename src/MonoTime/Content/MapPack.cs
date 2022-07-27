﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.MapPack
// Assembly: DuckGame, Version=1.1.8175.33388, Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DuckGame
{
    public class MapPack : ContentPack
    {
        private string _name;
        private Sprite _icon;
        private Mod _mod;
        private string _needsPreviewGenerationDir;
        public string path;
        public static List<MapPack> active = new List<MapPack>();
        public static List<MapPack> _mapPacks = new List<MapPack>();
        public static ReskinPack context;
        private Tex2D _preview;

        public string name => this._mod == null ? this._name : this._mod.configuration.name;

        public Sprite icon => this._icon;

        public Mod mod => this._mod;

        public MapPack()
          : base((ModConfiguration)null)
        {
        }

        public static Mod LoadMapPack(
          string pDir,
          Mod pExistingMod = null,
          ModConfiguration pExistingConfig = null)
        {
            MapPack pPack = new MapPack()
            {
                _name = Path.GetFileName(pDir),
                path = pDir
            };
            MapPack._mapPacks.Add(pPack);
            if (pExistingMod == null && pExistingConfig == null)
            {
                if (!DuckFile.FileExists(pDir + "/preview.png"))
                    System.IO.File.Copy(DuckFile.contentDirectory + "/mappack_preview.pngfile", pDir + "/preview.png");
                if (!DuckFile.FileExists(pDir + "/icon.png"))
                    System.IO.File.Copy(DuckFile.contentDirectory + "/mappack_icon.pngfile", pDir + "/icon.png");
                if (!DuckFile.FileExists(pDir + "/mappack_info.txt"))
                {
                    string str = "Dan Rando";
                    if (Steam.user != null)
                        str = Steam.user.name;
                    DuckFile.SaveString(pPack.name + "\n" + str + "\nEdit info.txt to change this information!\n<add a 1280x720 PNG file called 'screenshot.png' to set a custom workshop image!>", pDir + "/mappack_info.txt");
                }
            }
            Mod mod = pExistingMod;
            if (mod == null)
            {
                mod = (Mod)new ClientMod(pDir + "/", pExistingConfig, "mappack_info.txt");
                mod.configuration.LoadOrCreateConfig();
                mod.configuration.SetModType(ModConfiguration.Type.MapPack);
                ModLoader.AddMod(mod);
            }
            pPack._mod = mod;
            mod.SetPriority(Priority.MapPack);
            mod.configuration.SetMapPack(pPack);
            if (DuckFile.FileExists(pDir + "/icon.png"))
            {
                try
                {
                    Tex2D tex = (Tex2D)ContentPack.LoadTexture2D(pDir + "/icon.png");
                    pPack._icon = new Sprite(tex);
                }
                catch (Exception ex)
                {
                    pPack._icon = new Sprite("default_mappack_icon");
                }
            }
            if (!mod.configuration.disabled)
            {
                MapPack.active.Add(pPack);
                if (!DuckFile.FileExists(pDir + "/screenshot.png"))
                {
                    if (DuckFile.FileExists(pDir + "/screenshot_autogen.png"))
                        pPack._preview = (Tex2D)ContentPack.LoadTexture2D(pDir + "/screenshot_autogen.png");
                    else
                        pPack._needsPreviewGenerationDir = pDir;
                }
                else
                    pPack._preview = (Tex2D)ContentPack.LoadTexture2D(pDir + "/screenshot.png");
            }
            return mod;
        }

        public static void InitializeMapPacks()
        {
            foreach (string directory in DuckFile.GetDirectories(DuckFile.mappackDirectory))
                MapPack.LoadMapPack(directory);
            if (Steam.user == null)
                return;
            foreach (string directory in DuckFile.GetDirectories(DuckFile.globalMappackDirectory))
                MapPack.LoadMapPack(directory);
        }

        public static void RegeneratePreviewsIfNecessary()
        {
            try
            {
                foreach (MapPack mapPack in MapPack._mapPacks)
                {
                    if (mapPack._needsPreviewGenerationDir != null)
                        mapPack.RegeneratePreviewImage(mapPack._needsPreviewGenerationDir + "/screenshot_autogen.png");
                }
            }
            catch (Exception ex)
            {
                DevConsole.Log("MapPack.RegeneratePreviewsIfNecessary failed with error:");
                DevConsole.Log(ex.Message);
            }
        }

        public Tex2D preview => this._preview;

        public string RegeneratePreviewImage(string pPath)
        {
            if (pPath == null)
                pPath = this.path + "/screenshot_autogen.png";
            int num1 = 1280;
            int num2 = 720;
            RenderTarget2D t = new RenderTarget2D(num1, num2);
            Viewport viewport = DuckGame.Graphics.viewport;
            RenderTarget2D renderTarget = DuckGame.Graphics.GetRenderTarget();
            Sprite sprite = new Sprite("shiny");
            DuckGame.Graphics.SetRenderTarget(t);
            DuckGame.Graphics.viewport = new Viewport(0, 0, num1, num2);
            Camera camera = new Camera(0.0f, 0.0f, (float)num1, (float)num2);
            DuckGame.Graphics.screen.Begin(SpriteSortMode.BackToFront, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, (MTEffect)null, camera.getMatrix());
            DuckGame.Graphics.Draw(sprite.texture, 0.0f, 0.0f, 4f, 4f, (Depth)0.1f);
            string[] files = Directory.GetFiles(this._mod.configuration.directory, "*.lev", SearchOption.AllDirectories);
            int num3 = 0;
            int num4 = (int)Math.Ceiling(Math.Sqrt((double)((IEnumerable<string>)files).Count<string>()));
            float num5 = (float)(1280.0 / (double)num4 / 1280.0 * 4.0);
            Vec2 zero = Vec2.Zero;
            foreach (string levelPath in files)
            {
                if (num3 != num4 * num4)
                {
                    try
                    {
                        LevelMetaData.PreviewPair preview = Content.GeneratePreview(levelPath);
                        if (preview.preview != null)
                        {
                            if (preview.preview.Width == 320)
                            {
                                if (preview.preview.Height == 200)
                                {
                                    float num6 = 0.95f;
                                    Vec2 vec2_1 = new Vec2((float)preview.preview.Width * num5 * num6, (float)preview.preview.Height * num5 * num6);
                                    Vec2 vec2_2 = new Vec2((float)preview.preview.Width * num5, (float)preview.preview.Height * num5);
                                    DuckGame.Graphics.Draw((Tex2D)preview.preview, new Vec2((float)((double)zero.x + (double)vec2_2.x / 2.0 - (double)vec2_1.x / 2.0), (float)((double)zero.y + (double)vec2_2.y / 2.0 - (double)vec2_1.y / 2.0)), new Rectangle?(new Rectangle(0.0f, 10f, 320f, 180f)), Color.White, 0.0f, Vec2.Zero, new Vec2(num6 * num5), SpriteEffects.None, (Depth)0.9f);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    ++num3;
                    if (num3 % num4 == 0)
                    {
                        zero.x = 0.0f;
                        zero.y += (float)(num2 / num4);
                    }
                    else
                        zero.x += (float)(num1 / num4);
                }
                else
                    break;
            }
            DuckGame.Graphics.screen.End();
            DuckGame.Graphics.SetRenderTarget(renderTarget);
            DuckGame.Graphics.viewport = viewport;
            this._preview = t.ToTex2D();
            FileStream fileStream = System.IO.File.Create(pPath);
            (this._preview.nativeObject as Texture2D).SaveAsPng((Stream)fileStream, this._preview.width, this._preview.height);
            fileStream.Close();
            return pPath;
        }
    }
}