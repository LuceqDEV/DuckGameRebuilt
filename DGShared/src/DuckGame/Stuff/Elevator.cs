﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Elevator
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class Elevator : MaterialThing, IPlatform
    {
        private SpriteMap _sprite;

        public Elevator(float xpos, float ypos)
          : base(xpos, ypos)
        {
            _sprite = new SpriteMap("elevator", 32, 37);
            graphic = _sprite;
            center = new Vec2(8f, 8f);
            collisionOffset = new Vec2(-8f, -6f);
            collisionSize = new Vec2(16f, 13f);
            depth = -0.5f;
            thickness = 4f;
            weight = 7f;
            flammable = 0.3f;
            collideSounds.Add("rockHitGround2");
        }
    }
}
