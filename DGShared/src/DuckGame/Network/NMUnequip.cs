﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.NMUnequip
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class NMUnequip : NMEvent
    {
        public Duck duck;
        public Equipment equipment;

        public NMUnequip()
        {
        }

        public NMUnequip(Duck pDuck, Equipment pEquipment)
        {
            duck = pDuck;
            equipment = pEquipment;
        }

        public override void Activate()
        {
            if (duck != null && equipment != null)
                duck.Unequip(equipment, true);
            base.Activate();
        }
    }
}
