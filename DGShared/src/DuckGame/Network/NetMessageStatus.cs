﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.NetMessageStatus
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class NetMessageStatus
    {
        public int timesResent;
        public int timesDropped;
        public int framesSinceSent;
        public uint tickOnSend;

        public void Clear()
        {
            timesResent = 0;
            timesDropped = 0;
            framesSinceSent = 0;
            tickOnSend = 0U;
        }
    }
}
