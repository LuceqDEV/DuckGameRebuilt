﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.CastleTileset
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    [EditorGroup("Blocks")]
    public class CastleTileset : AutoBlock
    {
        public CastleTileset(float x, float y)
          : base(x, y, "castle")
        {
            _editorName = "Castle";
            physicsMaterial = PhysicsMaterial.Metal;
            verticalWidth = 10f;
            verticalWidthThick = 14f;
            horizontalHeight = 14f;
            _hasNubs = false;
        }
    }
}
