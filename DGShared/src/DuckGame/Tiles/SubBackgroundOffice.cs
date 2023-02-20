﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.SubBackgroundOffice
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class SubBackgroundOffice : SubBackgroundTile
    {
        public SubBackgroundOffice(float xpos, float ypos)
          : base(xpos, ypos)
        {
            graphic = new SpriteMap("officeSubBackground", 32, 32, true);
            _opacityFromGraphic = true;
            center = new Vec2(24f, 16f);
            collisionSize = new Vec2(32f, 32f);
            collisionOffset = new Vec2(-16f, -16f);
        }
    }
}
