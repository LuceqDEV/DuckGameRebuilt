﻿namespace DuckGame
{
    public class MediumTom : Drum
    {
        private Sprite _stand;

        public MediumTom(float xpos, float ypos)
          : base(xpos, ypos)
        {
            graphic = new Sprite("drumset/mediumTom");
            center = new Vec2(graphic.w / 2, graphic.h / 2);
            _stand = new Sprite("drumset/highTomStand");
            _stand.center = new Vec2(_stand.w / 2, 0f);
            _sound = "medTom";
        }

        public override void Draw()
        {
            base.Draw();
            _stand.depth = depth - 1;
            Graphics.Draw(ref _stand, x + 7f, y);
        }
    }
}
