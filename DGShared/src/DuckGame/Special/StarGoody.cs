﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.StarGoody
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    [EditorGroup("Special|Goodies", EditorItemType.Arcade)]
    [BaggedProperty("isOnlineCapable", false)]
    public class StarGoody : Goody
    {
        public EditorProperty<bool> valid;

        public override void EditorPropertyChanged(object property) => sequence.isValid = valid.value;

        public StarGoody(float xpos, float ypos)
          : base(xpos, ypos, new Sprite("challenge/star"))
        {
            valid = new EditorProperty<bool>(true, this);
        }
    }
}
