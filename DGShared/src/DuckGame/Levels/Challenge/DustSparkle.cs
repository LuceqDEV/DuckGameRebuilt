﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.DustSparkle
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class DustSparkle
    {
        public Vec2 position;
        public Vec2 velocity;
        public float alpha;
        public float sin;

        public DustSparkle(Vec2 pos, Vec2 vel)
        {
            position = pos;
            velocity = vel;
            sin = Rando.Float(6f);
        }
    }
}
