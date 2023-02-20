﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Platform
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class Platform : MaterialThing, IPlatform
    {
        public Platform(float x, float y)
          : base(x, y)
        {
            collisionSize = new Vec2(16f, 16f);
            thickness = 10f;
        }

        public Platform(float x, float y, float wid, float hi)
          : base(x, y)
        {
            collisionSize = new Vec2(wid, hi);
            thickness = 10f;
        }
    }
}
