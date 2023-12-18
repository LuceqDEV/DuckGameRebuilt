﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.YM2612
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class YM2612
    {
        private YM2612Core _chip;

        public YM2612() => _chip = new YM2612Core();

        public void Initialize(double clock, int soundRate)
        {
            _chip.YM2612Init(clock, soundRate);
            _chip.YM2612ResetChip();
        }

        public void Update(int[] buffer, int length) => _chip.YM2612Update(buffer, length);

        public void Write(int address, int value) => _chip.YM2612Write((uint)address, (uint)value);

        public void WritePort0(int register, int value)
        {
            _chip.YM2612Write(0U, (uint)register);
            _chip.YM2612Write(1U, (uint)value);
        }

        public void WritePort1(int register, int value)
        {
            _chip.YM2612Write(2U, (uint)register);
            _chip.YM2612Write(3U, (uint)value);
        }
    }
}
