﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.UIFlagSelection
// Assembly: DuckGame, Version=1.1.8175.33388, Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace DuckGame
{
    public class UIFlagSelection : UIMenu
    {
        private static List<Sprite> _flagSprites = (List<Sprite>)null;
        private static List<string> _flagFiles = (List<string>)null;
        private int _flagSelection;
        private UIMenu _openOnClose;
        private static Dictionary<int, Sprite> _sprites = new Dictionary<int, Sprite>();
        private static Tex2D _flagTexture;
        private int numFlags = 283;
        private int _numFlagsPerRow = 22;

        public UIFlagSelection(UIMenu openOnClose, string title, float xpos, float ypos)
          : base("FLAG SELECT", xpos, ypos, 214f, 142f, "@CANCEL@BACK @SELECT@SELECT")
        {
            UIBox component = new UIBox(isVisible: false);
            component.Add((UIComponent)new UIText(" ", Color.White), true);
            component.Add((UIComponent)new UIText(" ", Color.White), true);
            component.Add((UIComponent)new UIText(" ", Color.White), true);
            component.Add((UIComponent)new UIText(" ", Color.White), true);
            component.Add((UIComponent)new UIText(" ", Color.White), true);
            component.Add((UIComponent)new UIText(" ", Color.White), true);
            component.Add((UIComponent)new UIText(" ", Color.White), true);
            component.Add((UIComponent)new UIText(" ", Color.White), true);
            component.Add((UIComponent)new UIText(" ", Color.White), true);
            component.Add((UIComponent)new UIText(" ", Color.White), true);
            component.Add((UIComponent)new UIText(" ", Color.White), true);
            component.Add((UIComponent)new UIText(" ", Color.White), true);
            component.Add((UIComponent)new UIText(" ", Color.White), true);
            this.Add((UIComponent)component, true);
            this._flagSelection = Global.data.flag;
            this._openOnClose = openOnClose;
        }

        public static Sprite GetFlag(int idx, bool smallVersion = false)
        {
            Sprite flag = (Sprite)null;
            if (smallVersion && UIFlagSelection._sprites.TryGetValue(idx, out flag))
                return flag;
            try
            {
                if (UIFlagSelection._flagTexture == null)
                    UIFlagSelection._flagTexture = Content.Load<Tex2D>("flags/flags");
                if (UIFlagSelection._flagTexture != null)
                {
                    Tex2D tex = new Tex2D(61, 41);
                    int num1 = idx % 16;
                    int num2 = idx / 16;
                    Color[] colors = new Color[2501];
                    Color[] data = new Color[UIFlagSelection._flagTexture.width * UIFlagSelection._flagTexture.height];
                    (UIFlagSelection._flagTexture.nativeObject as Texture2D).GetData<Color>(data);
                    int num3 = num1 * 61;
                    int num4 = num2 * 41;
                    for (int index1 = 0; index1 < 41; ++index1)
                    {
                        for (int index2 = 0; index2 < 61; ++index2)
                            colors[index2 + index1 * 61] = data[num3 + index2 + (num4 + index1) * UIFlagSelection._flagTexture.width];
                    }
                    tex.SetData(colors);
                    flag = new Sprite(tex);
                    UIFlagSelection._sprites[idx] = flag;
                }
                else
                    DevConsole.Log(DCSection.General, "Found no renderable flags...");
            }
            catch (Exception ex)
            {
                DevConsole.Log(DCSection.General, "GetFlag(" + idx.ToString() + ") threw an exception:");
                DevConsole.Log(DCSection.General, ex.Message);
            }
            return flag;
        }

        public static int GetNumFlags()
        {
            if (UIFlagSelection._flagFiles == null)
                UIFlagSelection.GetFlag(0);
            return UIFlagSelection._flagFiles.Count;
        }

        public override void Update()
        {
            if (this.open && this.open && !this.animating)
            {
                if (Input.Pressed("LEFT"))
                {
                    --this._flagSelection;
                    if (this._flagSelection <= 0)
                        this._flagSelection = 0;
                }
                if (Input.Pressed("RIGHT"))
                {
                    ++this._flagSelection;
                    if (this._flagSelection >= this.numFlags)
                        this._flagSelection = this.numFlags - 1;
                }
                if (Input.Pressed("UP"))
                {
                    this._flagSelection -= this._numFlagsPerRow;
                    if (this._flagSelection <= 0)
                        this._flagSelection = 0;
                }
                if (Input.Pressed("DOWN"))
                {
                    this._flagSelection += this._numFlagsPerRow;
                    if (this._flagSelection >= this.numFlags)
                        this._flagSelection = this.numFlags - 1;
                }
                if (Input.Pressed("SELECT"))
                {
                    Global.data.flag = this._flagSelection;
                    this.Close();
                    this._openOnClose.Open();
                }
                if (Input.Pressed("CANCEL"))
                {
                    this.Close();
                    this._openOnClose.Open();
                }
            }
            base.Update();
        }

        public override void Draw()
        {
            if (UIFlagSelection._flagTexture == null)
                UIFlagSelection._flagTexture = Content.Load<Tex2D>("flags/flags");
            int num1 = 0;
            int num2 = 0;
            float num3 = 0.0f;
            float num4 = 10f;
            float num5 = (float)((double)this.x - (double)this.width / 2.0 + 8.0);
            float num6 = (float)((double)this.y - (double)this.height / 2.0 + 8.0);
            for (int index = 0; index < this.numFlags; ++index)
            {
                int num7 = index % 16;
                int num8 = index / 16;
                DuckGame.Graphics.Draw(UIFlagSelection._flagTexture, new Vec2(num5 + num3, num6 + num4), new Rectangle?(new Rectangle((float)(num7 * 61), (float)(num8 * 41), 61f, 41f)), num2 == this._flagSelection ? Color.White : Color.White * 0.7f, 0.0f, Vec2.Zero, new Vec2(0.14f, 0.14f), SpriteEffects.None, (Depth)0.9f);
                num3 += 9f;
                ++num1;
                ++num2;
                if (num1 >= this._numFlagsPerRow)
                {
                    num1 = 0;
                    num4 += 8f;
                    num3 = 0.0f;
                }
            }
            base.Draw();
        }
    }
}