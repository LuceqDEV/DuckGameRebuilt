﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.PhysicsRopeSection
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class PhysicsRopeSection : PhysicsObject
    {
        public Vec2 tempPos;
        public Vec2 calcPos;
        public Vec2 velocity;
        public Vec2 accel;
        public PhysicsRope rope;

        public PhysicsRopeSection(float xpos, float ypos, PhysicsRope r)
          : base(xpos, ypos)
        {
            tempPos = position;
            collisionSize = new Vec2(4f, 4f);
            collisionOffset = new Vec2(-2f, -2f);
            weight = 0.1f;
            updatePhysics = false;
            rope = r;
        }
    }
}
