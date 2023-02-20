﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.TutorialSign
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public abstract class TutorialSign : Thing
    {
        public TutorialSign(float xpos, float ypos, string image, string name)
          : base(xpos, ypos)
        {
            if (image == null)
                return;
            graphic = new Sprite(image);
            center = new Vec2(graphic.w / 2, graphic.h / 2);
            _collisionSize = new Vec2(16f, 16f);
            _collisionOffset = new Vec2(-8f, -8f);
            depth = -0.5f;
            _editorName = name;
            layer = Layer.Background;
        }
    }
}
