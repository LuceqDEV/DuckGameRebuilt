﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.BufferedGhostProperty`1
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class BufferedGhostProperty<T> : BufferedGhostProperty
    {
        private T _value;

        public override object value
        {
            get => _value;
            set => _value = (T)value;
        }

        public override bool Refresh()
        {
            T newVal;
            if (binding.Compare(_value, out newVal))
                return false;
            _value = newVal;
            return true;
        }

        public override void UpdateFrom(StateBinding bind) => _value = bind.getTyped<T>();

        public override void Apply(float lerp)
        {
            if (lerp < 1f)
            {
                if (binding is CompressedVec2Binding)
                {
                    Vec2 typed = binding.getTyped<Vec2>();
                    Vec2 to = (Vec2)value;
                    if ((typed - to).lengthSq > 1024f)
                        binding.setTyped(to);
                    else
                        binding.setTyped(Lerp.Vec2Smooth(typed, to, lerp));
                }
                else if (binding.isRotation)
                    binding.setTyped(Maths.DegToRad(Maths.PointDirection(Vec2.Zero, Slerp(Maths.AngleToVec(binding.getTyped<float>()), Maths.AngleToVec((float)value), lerp))));
                else
                    binding.setTyped(_value);
            }
            else
            {
                if (binding.name == "netPosition")
                {
                    double x = ((Vec2)value).x;
                }
                binding.setTyped(_value);
            }
        }
    }
}
