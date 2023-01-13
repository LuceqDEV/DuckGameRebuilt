﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.NMSetPersona
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using System.Linq;

namespace DuckGame
{
    public class NMSetPersona : NMEvent
    {
        public Profile profile;
        public byte persona;

        public NMSetPersona()
        {
        }

        public NMSetPersona(Profile pProfile, DuckPersona pPersona)
        {
            profile = pProfile;
            persona = (byte)pPersona.index;
        }

        public override void Activate()
        {
            if (profile == null || persona < 0 || persona >= Persona.alllist.Count)
                return;
            profile.persona = Persona.alllist[persona];
        }
    }
}
