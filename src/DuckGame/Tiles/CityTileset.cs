﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.CityTileset
// Assembly: DuckGame, Version=1.1.8175.33388, Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    [EditorGroup("Blocks")]
    public class CityTileset : AutoBlock
    {
        public CityTileset(float x, float y)
          : base(x, y, "cityTileset")
        {
            this._editorName = "City";
            this.physicsMaterial = PhysicsMaterial.Metal;
            this.verticalWidth = 10f;
            this.verticalWidthThick = 15f;
            this.horizontalHeight = 13f;
        }

        public override void Draw() => base.Draw();
    }
}