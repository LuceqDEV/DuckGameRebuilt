﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.EightPlayer
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    [EditorGroup("Spawns")]
    public class EightPlayer : Thing
    {
        public EditorProperty<bool> eightPlayerOnly = new EditorProperty<bool>(false);

        public EightPlayer(float x, float y)
          : base(x, y)
        {
            _editorName = "Eight Player";
            graphic = new Sprite("eight_player");
            center = new Vec2(8f, 8f);
            depth = (Depth)0.55f;
            _visibleInGame = false;
            editorTooltip = "Place in a level to make it an 8 Player map!";
            eightPlayerOnly._tooltip = "If true, this map will not appear when less than 5 players are present in the game.";
            solid = false;
            _collisionSize = new Vec2(0f, 0f);
        }
    }
}
