﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.KillEvent
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class KillEvent : Event
    {
        private System.Type _weapon;

        public System.Type weapon => _weapon;

        public KillEvent(Profile killerVal, Profile killedVal, System.Type weapon)
          : base(killerVal, killedVal)
        {
            _weapon = weapon;
        }
    }
}
