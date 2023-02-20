﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.GreyBlock
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    [EditorGroup("Stuff")]
    public class GreyBlock : Block
    {
        public GreyBlock(float xpos, float ypos)
          : base(xpos, ypos)
        {
            graphic = new Sprite("greyBlock");
            center = new Vec2(8f, 8f);
            collisionOffset = new Vec2(-8f, -8f);
            collisionSize = new Vec2(16f, 16f);
            depth = -0.5f;
            _editorName = "Grey Block";
            editorTooltip = "It's a featureless grey block.";
            thickness = 4f;
            physicsMaterial = PhysicsMaterial.Metal;
            shouldbeinupdateloop = false;
        }
    }
}
