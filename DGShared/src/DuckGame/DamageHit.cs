﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.DamageHit
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using System.Collections.Generic;

namespace DuckGame
{
    public class DamageHit
    {
        public Thing thing;
        public List<Vec2> points = new List<Vec2>();
        public List<DamageType> types = new List<DamageType>();
    }
}
