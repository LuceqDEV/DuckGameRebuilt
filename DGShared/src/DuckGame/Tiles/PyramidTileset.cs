﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.PyramidTileset
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    [EditorGroup("Blocks", EditorItemType.Pyramid)]
    [BaggedProperty("isInDemo", false)]
    public class PyramidTileset : AutoBlock
    {
        public PyramidTileset(float x, float y)
          : base(x, y, "pyramidTileset")
        {
            _editorName = "Pyramid";
            physicsMaterial = PhysicsMaterial.Metal;
            verticalWidthThick = 14f;
            verticalWidth = 12f;
            horizontalHeight = 13f;
        }
    }
}
