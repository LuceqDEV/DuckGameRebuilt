﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.NMDestroyProp
// Assembly: DuckGame, Version=1.1.8175.33388, Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class NMDestroyProp : NMEvent
    {
        public Thing prop;
        private byte _levelIndex;

        public NMDestroyProp(Thing t)
        {
            this.priority = NetMessagePriority.UnreliableUnordered;
            this.prop = t;
        }

        public NMDestroyProp()
        {
        }

        public override void Activate()
        {
            if (!(Level.current is GameLevel) || (int)DuckNetwork.levelIndex != (int)this._levelIndex || !(this.prop is MaterialThing prop))
                return;
            prop.NetworkDestroy();
        }

        protected override void OnSerialize()
        {
            base.OnSerialize();
            this._serializedData.Write(DuckNetwork.levelIndex);
        }

        public override void OnDeserialize(BitBuffer d)
        {
            base.OnDeserialize(d);
            this._levelIndex = d.ReadByte();
        }
    }
}