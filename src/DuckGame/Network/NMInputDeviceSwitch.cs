﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.NMInputDeviceSwitch
// Assembly: DuckGame, Version=1.1.8175.33388, Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    [FixedNetworkID(29422)]
    public class NMInputDeviceSwitch : NMDuckNetworkEvent
    {
        public byte index;
        public byte inputType;

        public NMInputDeviceSwitch()
        {
        }

        public NMInputDeviceSwitch(byte idx, byte inpType)
        {
            this.index = idx;
            this.inputType = inpType;
        }

        public override void Activate()
        {
            if (this.index < (byte)0 || this.index > (byte)3)
                return;
            Profile profile = DuckNetwork.profiles[(int)this.index];
            if (profile != null && profile.inputProfile != null)
            {
                foreach (DeviceInputMapping inputMappingOverride in profile.inputMappingOverrides)
                {
                    if (inputMappingOverride.inputOverrideType == (int)this.inputType && inputMappingOverride.deviceOverride != null)
                    {
                        profile.inputProfile.lastActiveOverride = inputMappingOverride.deviceOverride;
                        break;
                    }
                }
            }
            base.Activate();
        }
    }
}