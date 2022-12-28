﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.RandomLevel
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class RandomLevel : DeathmatchLevel
    {
        public static int currentComplexityDepth;
        private RandomLevelNode _level;

        public RandomLevel()
          : base("RANDOM")
        {
            _level = LevelGenerator.MakeLevel();
        }

        public override void Initialize()
        {
            _level.LoadParts(0f, 0f, this);
            OfficeBackground officeBackground = new OfficeBackground(0f, 0f)
            {
                visible = false
            };
            Add(officeBackground);
            base.Initialize();
        }

        public override void Update() => base.Update();
    }
}
