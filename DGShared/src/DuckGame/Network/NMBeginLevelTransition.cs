﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.NMBeginLevelTransition
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class NMBeginLevelTransition : NMDuckNetworkEvent
    {
        public new byte levelIndex;

        public NMBeginLevelTransition()
        {
        }

        public NMBeginLevelTransition(byte pLevelIndex) => levelIndex = pLevelIndex;

        public override void Activate()
        {
        }
    }
}
