﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.NMSpawnPlayer
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class NMSpawnPlayer : NMObjectMessage
    {
        public float xpos;
        public float ypos;
        public int duckID;
        public bool isPlayerDuck;

        public NMSpawnPlayer()
        {
        }

        public NMSpawnPlayer(float xVal, float yVal, int duck, bool playerDuck, ushort objectID)
          : base(objectID)
        {
            xpos = xVal;
            ypos = yVal;
            duckID = duck;
            isPlayerDuck = playerDuck;
        }
    }
}
