﻿using System;

namespace DuckGame
{

    public struct ProgressValue
    {
        public double Value;
        public double MaximumValue;
        public double MinimumValue;
        public double IncrementSize;

        public double NormalizedValue
        {
            get => (Value - MinimumValue) / (MaximumValue - MinimumValue);
            set => Value = value * (MaximumValue - MinimumValue) + MinimumValue;
        }
        
        public static double Normalize(double value, double min, double max) => (value - min) / (max - min);

        public bool Completed => IncrementSize >= 0 ? NormalizedValue >= 1 : NormalizedValue <= 0;

        public ProgressValue(ProgressValue p) : this(p.Value, p.IncrementSize, p.MinimumValue, p.MaximumValue) { }

        public ProgressValue(double value, double incrementSize, double min, double max)
        {
            if (min > max)
                throw new Exception("Minimum size cannot be less than the maximum size");

            Value = value;
            MaximumValue = max;
            MinimumValue = min;
            IncrementSize = incrementSize;
        }
        
        public ProgressValue(double value, double min, double max)
        {
            if (min > max)
                throw new Exception("Minimum size cannot be less than the maximum size");

            Value = value;
            MaximumValue = max;
            MinimumValue = min;
            IncrementSize = Normalize(value, min, max) * (max - min);
        }

        public string GenerateBar(int characterCount = 30, char filled = '#', char empty = '-', Func<string, string, string>? formatFunction = null)
        {
            formatFunction ??= (done, left) => $"[{done}{left}]";
            Value = ~this;

            double fillPercentage = NormalizedValue * characterCount;

            string whiteBar = new(filled, (int)fillPercentage);
            string blackBar = new(empty, (int)(characterCount - fillPercentage));

            string fullBar = formatFunction(whiteBar, blackBar);
            fullBar = fullBar.Length == characterCount - 1
                ? fullBar + empty
                : fullBar;

            return fullBar;
        }

        // From Progress to T
        public static implicit operator float(ProgressValue f) => (float)f.Value;
        public static implicit operator double(ProgressValue f) => f.Value;
        public static implicit operator int(ProgressValue f) => (int)f.Value;

        // From T to Progress
        public static implicit operator ProgressValue(float f) => new((double)f);
        public static implicit operator ProgressValue(double f) => new(f, 0, double.MinValue, double.MaxValue);
        public static implicit operator ProgressValue(int f) => new((double)f);

        // Positive/Negative
        public static ProgressValue operator +(ProgressValue f) => f;
        public static ProgressValue operator -(ProgressValue f) => f * -1;

        // Arithmetic

        public static ProgressValue operator +(ProgressValue a, ProgressValue b)
        {
            a.Value += b.Value;
            return a;
        }

        public static ProgressValue operator -(ProgressValue a, ProgressValue b)
        {

            a.Value -= b.Value;
            return a;
        }

        public static ProgressValue operator *(ProgressValue a, ProgressValue b)
        {
            a.Value *= b.Value;
            return a;
        }

        public static ProgressValue operator /(ProgressValue a, ProgressValue b)
        {
            a.Value /= b.Value;
            return a;
        }

        // Equality
        public static bool operator ==(ProgressValue a, ProgressValue b) => a.Value == b.Value;
        public static bool operator !=(ProgressValue a, ProgressValue b) => a.Value != b.Value;
        public static bool operator >(ProgressValue a, ProgressValue b) => a.Value > b.Value;
        public static bool operator <(ProgressValue a, ProgressValue b) => a.Value < b.Value;
        public static bool operator >=(ProgressValue a, ProgressValue b) => a.Value >= b.Value;
        public static bool operator <=(ProgressValue a, ProgressValue b) => a.Value <= b.Value;

        // Increment/Decrement
        public static ProgressValue operator ++(ProgressValue p)
        {
            p += p.IncrementSize;
            return p;
        }

        public static ProgressValue operator --(ProgressValue p)
        {
            p -= p.IncrementSize;
            return p;
        }

        // Inversion
        public static ProgressValue operator !(ProgressValue p)
        {
            p.Value = p.MaximumValue - p.Value + p.MinimumValue;
            return p;
        }

        // Clamping
        public static ProgressValue operator ~(ProgressValue p)
        {
            p.Value = Maths.Clamp(p.Value, p.MinimumValue, p.MaximumValue);
            return p;
        }

        // Method Overrides
        public override bool Equals(object obj)
        {
            return obj is ProgressValue p && p.Equals(this);
        }

        public bool Equals(ProgressValue other)
        {
            return Value.Equals(other.Value)
                   && MaximumValue.Equals(other.MaximumValue)
                   && MinimumValue.Equals(other.MinimumValue)
                   && IncrementSize.Equals(other.IncrementSize);
        }
        public ProgressValue Clone()
        {
            ProgressValue progressValue = new ProgressValue
            {
                Value = Value,
                MaximumValue = MaximumValue,
                MinimumValue = MinimumValue,
                IncrementSize = IncrementSize
            };
            return progressValue;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Value.GetHashCode();
                hashCode = (hashCode * 397) ^ MaximumValue.GetHashCode();
                hashCode = (hashCode * 397) ^ MinimumValue.GetHashCode();
                hashCode = (hashCode * 397) ^ IncrementSize.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return MinimumValue == 0
                ? $"{Value}/{MaximumValue}"
                : $"{MinimumValue}/{Value}/{MaximumValue}";
        }
    }
}