﻿using System;

namespace DuckGame
{
    [EditorGroup("Details|Signs")]
    public class FishinSign : Thing
    {
        private SpriteMap _sprite;

        public FishinSign(float xpos, float ypos)
          : base(xpos, ypos)
        {
            _sprite = new SpriteMap("goneFishin", 32, 32);
            graphic = _sprite;
            center = new Vec2(16f, 16f);
            _collisionSize = new Vec2(16f, 16f);
            _collisionOffset = new Vec2(-8f, -9f);
            depth = -0.5f;
            _editorName = "Fishin Sign";
            editorTooltip = "It really explains itself, doesn't it?";
            hugWalls = WallHug.Floor;
        }

        public override Type TabRotate(bool control)
        {
            if (control)
                return typeof(HardLeft);
            return base.TabRotate(control);
        }

        public override void Draw()
        {
            _sprite.frame = offDir > 0 ? 1 : 0;
            base.Draw();
        }
    }
}
