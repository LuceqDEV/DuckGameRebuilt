﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.ATPlasmaBlaster
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class ATPlasmaBlaster : ATLaser
    {
        public ATPlasmaBlaster()
        {
            accuracy = 0.75f;
            range = 250f;
            penetration = 1f;
            bulletSpeed = 64f;
            bulletColor = Color.Orange;
            bulletType = typeof(Bullet);
            bulletThickness = 1f;
        }
    }
}
