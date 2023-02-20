﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.PlasmaFlare
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class PlasmaFlare : Thing
    {
        private SpriteMap _sprite;

        public PlasmaFlare(float xpos, float ypos)
          : base(xpos, ypos)
        {
            _sprite = new SpriteMap("plasmaFlare", 16, 16);
            _sprite.AddAnimation("idle", 0.7f, false, 0, 1, 2);
            _sprite.SetAnimation("idle");
            graphic = _sprite;
            center = new Vec2(0f, 16f);
        }

        public override void Update()
        {
            if (!_sprite.finished)
                return;
            Level.Remove(this);
        }
    }
}
