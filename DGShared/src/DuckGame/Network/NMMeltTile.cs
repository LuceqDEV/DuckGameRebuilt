﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.NMMeltTile
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class NMMeltTile : NMEvent
    {
        public short x;
        public short y;

        public NMMeltTile()
        {
        }

        public NMMeltTile(Vec2 pPosition)
        {
            x = (short)pPosition.x;
            y = (short)pPosition.y;
        }

        public override void Activate()
        {
            Level.CheckPoint<SnowTileset>(new Vec2(x, y))?.Melt(false, true);
            base.Activate();
        }
    }
}
