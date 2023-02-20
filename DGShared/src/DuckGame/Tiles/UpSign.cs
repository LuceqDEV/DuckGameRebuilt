﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.UpSign
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    [EditorGroup("Details|Signs")]
    public class UpSign : Thing
    {
        private SpriteMap _sprite;

        public UpSign(float xpos, float ypos)
          : base(xpos, ypos)
        {
            _sprite = new SpriteMap("upSign", 32, 32);
            graphic = _sprite;
            center = new Vec2(16f, 24f);
            _collisionSize = new Vec2(16f, 16f);
            _collisionOffset = new Vec2(-8f, -8f);
            depth = -0.5f;
            _editorName = "Up Sign";
            hugWalls = WallHug.Floor;
        }

        public override void Draw()
        {
            _sprite.frame = offDir > 0 ? 0 : 1;
            base.Draw();
        }
    }
}
