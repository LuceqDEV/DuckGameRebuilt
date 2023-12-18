﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.MTEffect
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
    public class MTEffect
    {
        private Effect _base;
        private short _effectIndex;
        private string _effectName;

        public short effectIndex => _effectIndex;

        public string effectName => _effectName;

        public void SetEffectIndex(short index) => _effectIndex = index;

        public Effect effect => _base;

        public MTEffect(Effect tex, string cureffectName, short cureffectIndex = 0)
        {
            _base = tex;
            _effectIndex = cureffectIndex;
            _effectName = cureffectName;
        }

        public static implicit operator Effect(MTEffect tex) => tex?._base;

        public static implicit operator MTEffect(Effect tex) => Content.GetMTEffect(tex);
    }
}
