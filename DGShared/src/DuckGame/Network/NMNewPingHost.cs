﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.NMNewPingHost
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    [FixedNetworkID(30016)]
    public class NMNewPingHost : NMNewPing
    {
        public NetIndex16 hostSynchronizedTime;

        public NMNewPingHost(byte pIndex)
          : base(pIndex)
        {
        }

        public NMNewPingHost()
        {
        }

        protected override void OnSerialize()
        {
            hostSynchronizedTime = Network.TickSync;
            base.OnSerialize();
        }
    }
}
