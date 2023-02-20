﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.WireMountFlagBinding
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class WireMountFlagBinding : StateFlagBase
    {
        public override ushort ushortValue
        {
            get
            {
                _value = 0;
                if ((_thing as WireMount).action)
                    _value = 1;
                return _value;
            }
            set
            {
                _value = value;
                (_thing as WireMount).action = (_value & 1U) > 0U;
            }
        }

        public WireMountFlagBinding(GhostPriority p = GhostPriority.Normal)
          : base(p, 1)
        {
        }
    }
}
