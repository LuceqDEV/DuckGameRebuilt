﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.LightOccluder
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class LightOccluder
    {
        public Vec2 p1;
        public Vec2 p2;
        public Color color;

        public LightOccluder(Vec2 p, Vec2 pp, Color c)
        {
            p1 = p;
            p2 = pp;
            color = c;
        }
    }
}
