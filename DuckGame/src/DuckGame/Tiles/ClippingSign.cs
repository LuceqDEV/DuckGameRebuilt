﻿using System;

namespace DuckGame
{
    [EditorGroup("Details|Signs")]
    public class ClippingSign : Thing
    {
        public EditorProperty<int> style;
        private SpriteMap _sprite;

        public override void EditorPropertyChanged(object property) => (graphic as SpriteMap).frame = style.value;

        public ClippingSign(float xpos, float ypos)
          : base(xpos, ypos)
        {
            style = new EditorProperty<int>(0, this, max: 2f, increment: 1f);
            _sprite = new SpriteMap("noClippingSign", 32, 32);
            graphic = _sprite;
            center = new Vec2(16f, 24f);
            _collisionSize = new Vec2(16f, 16f);
            _collisionOffset = new Vec2(-8f, -8f);
            depth = -0.5f;
            _editorName = "No Clipping";
            editorTooltip = "I mean it!!";
            _canFlip = false;
            hugWalls = WallHug.Floor;
        }

        public override Type TabRotate(bool control)
        {
            if (control)
                return typeof(RaceSign);
            return base.TabRotate(control);
        }

        public override void Draw() => base.Draw();
    }
}
