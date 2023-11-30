﻿using System;

namespace DuckGame
{
    public class DamageMap
    {
        public Thing thing;
        private const int size = 256;
        public byte[] bytes = new byte[256];

        public bool InRange(int x, int y)
        {
            x = (int)(x - thing.left);
            y = (int)(y - thing.top);
            x += y * 16;
            return x >= 0 && x < 256;
        }

        public bool InRange(float x, float y)
        {
            x = (float)Math.Round(x);
            y = (float)Math.Round(y);
            return InRange((int)x, (int)y);
        }

        public bool CheckPoint(int x, int y)
        {
            x = (int)(x - thing.left);
            y = (int)(y - thing.top);
            x += y * 16;
            return x < 0 || x >= 256 || bytes[x] > 0;
        }

        public bool CheckPointRelative(int x, int y)
        {
            x += y * 16;
            return x < 0 || x >= 256 || bytes[x] > 0;
        }

        public bool CheckPoint(float x, float y)
        {
            x = (float)Math.Round(x);
            y = (float)Math.Round(y);
            return CheckPoint((int)x, (int)y);
        }

        public void SetPoint(int x, int y, bool val)
        {
            x += y * 16;
            if (x < 0 || x >= 256)
                return;
            bytes[x] = val ? (byte)1 : (byte)0;
        }

        public void Damage(Vec2 point, float radius)
        {
            point.x -= thing.left;
            point.y -= thing.top;
            for (int y = 0; y < 16; ++y)
            {
                for (int x = 0; x < 16; ++x)
                {
                    if ((new Vec2(x, y) - point).length <= radius)
                        SetPoint(x, y, false);
                }
            }
        }

        public void Clear()
        {
            for (int index = 0; index < 256; ++index)
                bytes[index] = 1;
        }
    }
}
