﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.NMInputUpdate
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class NMInputUpdate : NetMessage
    {
        public int id;
        public int state;
        public double time;

        public NMInputUpdate()
        {
        }

        public NMInputUpdate(int idVal, int stateVal, double t)
        {
            id = idVal;
            state = stateVal;
            time = t;
        }
    }
}
