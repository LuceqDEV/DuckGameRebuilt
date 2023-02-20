﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.UndergroundTileset
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    [EditorGroup("Blocks")]
    public class UndergroundTileset : AutoBlock
    {
        public UndergroundTileset(float x, float y)
          : base(x, y, "undergroundTileset")
        {
            _editorName = "Bunker";
            physicsMaterial = PhysicsMaterial.Metal;
            verticalWidth = 10f;
            verticalWidthThick = 15f;
            horizontalHeight = 15f;
        }

        public override void Draw() => base.Draw();
    }
}
