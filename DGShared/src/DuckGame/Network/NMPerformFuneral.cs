﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.NMPerformFuneral
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class NMPerformFuneral : NMEvent
    {
        public Duck duck;

        public NMPerformFuneral()
        {
        }

        public NMPerformFuneral(Duck d) => duck = d;

        public override void Activate()
        {
            if (duck == null)
                return;
            duck.DoFuneralStuff();
        }
    }
}
