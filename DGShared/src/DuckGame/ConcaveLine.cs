﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.ConcaveLine
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using System.Collections.Generic;

namespace DuckGame
{
    public class ConcaveLine
    {
        public Vec2 p1;
        public Vec2 p2;
        public int index;
        public List<ConcaveLine> intersects = new List<ConcaveLine>();

        public ConcaveLine(Vec2 p1val, Vec2 p2val)
        {
            p1 = p1val;
            p2 = p2val;
        }
    }
}
