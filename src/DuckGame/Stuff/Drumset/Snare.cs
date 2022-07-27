﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Snare
// Assembly: DuckGame, Version=1.1.8175.33388, Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class Snare : Drum
    {
        private Sprite _stand;

        public Snare(float xpos, float ypos)
          : base(xpos, ypos)
        {
            this.graphic = new Sprite("drumset/snareDrum");
            this.center = new Vec2((float)(this.graphic.w / 2), (float)(this.graphic.h / 2));
            this._stand = new Sprite("drumset/snareStand");
            this._stand.center = new Vec2((float)(this._stand.w / 2), 0.0f);
            this._sound = "snare";
        }

        public override void Draw()
        {
            base.Draw();
            this._stand.depth = this.depth - 1;
            Graphics.Draw(this._stand, this.x, this.y + 3f);
        }
    }
}