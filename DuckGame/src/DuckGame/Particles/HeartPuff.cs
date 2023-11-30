﻿namespace DuckGame
{
    public class HeartPuff : Thing
    {
        private SpriteMap _sprite;

        public HeartPuff(float xpos, float ypos)
          : base(xpos, ypos)
        {
            _sprite = new SpriteMap("heartpuff", 16, 16);
            _sprite.AddAnimation("wither", 0.35f, false, 0, 1, 2, 3, 4);
            _sprite.SetAnimation("wither");
            center = new Vec2(5f, 16f);
            alpha = 0.6f;
            depth = (Depth)0.9f;
            graphic = _sprite;
            _sprite.color = Color.Green;
        }

        public override void Update()
        {
            if (anchor != null && anchor.thing != null)
            {
                flipHorizontal = anchor.thing.offDir < 0;
                if (flipHorizontal)
                    center = new Vec2(10f, 16f);
                else
                    center = new Vec2(5f, 16f);
                angle = anchor.thing.angle;
            }
            if (!currentlyDrawing) _sprite.UpdateFrame(true);
            if (_sprite.finished)
                Level.Remove(this);
            base.Update();
        }
    }
}
