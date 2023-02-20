﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.ScoreBoard
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class ScoreBoard : Thing
    {
        public ScoreBoard()
          : base()
        {
        }

        public override void Initialize()
        {
            int num = 0;
            foreach (Team team in Teams.all)
            {
                if (team.activeProfiles.Count > 0)
                {
                    Level.current.AddThing(new PlayerCard(num * 1f, new Vec2(-400f, 140 * num + 120), new Vec2(Graphics.width / 2 - 200, 140 * num + 120), team));
                    ++num;
                }
            }
        }
    }
}
