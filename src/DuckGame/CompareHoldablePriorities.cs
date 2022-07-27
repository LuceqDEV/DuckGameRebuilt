﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.CompareHoldablePriorities
// Assembly: DuckGame, Version=1.1.8175.33388, Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using System.Collections.Generic;

namespace DuckGame
{
    public class CompareHoldablePriorities : IComparer<Holdable>
    {
        private Duck _duck;

        public CompareHoldablePriorities(Duck d) => this._duck = d;

        public int Compare(Holdable h1, Holdable h2)
        {
            if (h1 == h2)
                return 0;
            if (h1 is CTFPresent)
                return -1;
            if (h2 is CTFPresent)
                return 1;
            if (h1 is TrappedDuck)
                return -1;
            if (h2 is TrappedDuck || h1 is Equipment && this._duck.HasEquipment(h1 as Equipment))
                return 1;
            if (h2 is Equipment && this._duck.HasEquipment(h2 as Equipment))
                return -1;
            if (h1.PickupPriority() == h2.PickupPriority())
            {
                Vec2 vec2 = h1.position - this._duck.position;
                double length1 = (double)vec2.length;
                vec2 = h2.position - this._duck.position;
                double length2 = (double)vec2.length;
                return length1 - length2 < -2.0 ? -1 : 1;
            }
            return h1.PickupPriority() < h2.PickupPriority() ? -1 : 1;
        }
    }
}