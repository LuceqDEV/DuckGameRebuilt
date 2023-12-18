﻿namespace DuckGame
{
    [EditorGroup("Background|Parallax")]
    public class OfficeBackground : BackgroundUpdater
    {
        public OfficeBackground(float xpos, float ypos)
          : base(xpos, ypos)
        {
            graphic = new SpriteMap("backgroundIcons", 16, 16)
            {
                frame = 1
            };
            center = new Vec2(8f, 8f);
            _collisionSize = new Vec2(16f, 16f);
            _collisionOffset = new Vec2(-8f, -8f);
            depth = (Depth)0.9f;
            layer = Layer.Foreground;
            _visibleInGame = false;
            _editorName = "Office BG";
            editorCycleType = typeof(PyramidBackground);
        }

        public override void Initialize()
        {
            if (Level.current is Editor)
                return;
            backgroundColor = new Color(25, 38, 41);
            Level.current.backgroundColor = backgroundColor;
            _parallax = new ParallaxBackground("background/office", 0f, 0f, 3);
            if (_parallax.definition == null)
            {
                float speed = 0.4f;
                _parallax.AddZone(0, 0f, -speed, true);
                _parallax.AddZone(1, 0f, -speed, true);
                _parallax.AddZone(2, 0f, -speed, true);
                _parallax.AddZone(3, 0.2f, -speed, true);
                _parallax.AddZone(4, 0.2f, -speed, true);
                _parallax.AddZone(5, 0.4f, -speed, true);
                _parallax.AddZone(6, 0.8f, speed);
                _parallax.AddZone(7, 0.8f, speed);
                _parallax.AddZone(8, 0.8f, speed);
                _parallax.AddZone(9, 0.8f, speed);
                Sprite s1 = new Sprite("background/officeBuilding01")
                {
                    depth = -0.9f,
                    position = new Vec2(100f, 100f)
                };
                _parallax.AddZoneSprite(s1, 15, 0.6f, speed);
                Sprite s2 = new Sprite("background/officeBuilding01Porch")
                {
                    depth = -0.9f,
                    position = new Vec2(84f, 160f)
                };
                _parallax.AddZoneSprite(s2, 16, 0.6f, speed);
                Sprite s3 = new Sprite("background/officeBuilding02")
                {
                    depth = -0.9f,
                    position = new Vec2(300f, 120f)
                };
                _parallax.AddZoneSprite(s3, 17, 0.6f, speed);
                _parallax.AddZone(19, 0.6f, speed);
                _parallax.AddZone(20, 0.6f, speed);
                _parallax.AddZone(21, 0.6f, speed);
                _parallax.AddZone(22, 0.6f, speed);
                _parallax.AddZone(23, 0.6f, speed);
                _parallax.AddZone(24, 0.5f, speed);
                _parallax.AddZone(25, 0.4f, speed);
                _parallax.AddZone(26, 0.3f, speed);
                _parallax.AddZone(27, 0.2f, speed);
                _parallax.AddZone(28, 0.1f, speed);
                _parallax.AddZone(29, 0f, speed);
            }
            Level.Add(_parallax);
        }

        public override void Update() => base.Update();

        public override void Terminate() => Level.Remove(_parallax);

        public static string backgroundtextdata = @"[yChunk, distance, speed, moving]
[yChunk, distance, speed, moving, sprite, spriteX, spriteY, spriteDepth]

0, 0.0, -0.4, true
1, 0.0, -0.4, true
2, 0.0, -0.4, true
3, 0.2, -0.4, true
4, 0.2, -0.4, true
5, 0.4, -0.4, true
6, 0.8, 0.4, false
7, 0.8, 0.4, false
8, 0.8, 0.4, false
9, 0.8, 0.4, false

15, 0.6, 0.4, false, background/officeBuilding01, 100, 100, 0.9
16, 0.6, 0.4, false, background/officeBuilding01Porch, 84, 160, -0.9
17, 0.6, 0.4, false, background/officeBuilding02, 300, 120, -0.9

19, 0.6, 0.4, false
20, 0.6, 0.4, false
21, 0.6, 0.4, false
22, 0.6, 0.4, false
23, 0.6, 0.4, false
24, 0.5, 0.4, false
25, 0.4, 0.4, false
26, 0.3, 0.4, false
27, 0.2, 0.4, false
28, 0.1, 0.4, false
29, 0.0, 0.4, false";
    }
}
