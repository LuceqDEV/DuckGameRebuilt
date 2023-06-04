﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.GrenadeExplosion
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using System;
using System.Collections.Generic;

namespace DuckGame
{
    public class GrenadeExplosion : Thing
    {
        private int _explodeFrames = -1;

        public GrenadeExplosion(float xpos, float ypos)
          : base(xpos, ypos)
        {
        }

        public override void Update()
        {
            if (_explodeFrames < 0)
            {
                float x = this.x;
                float ypos = y - 2f;
                Level.Add(new ExplosionPart(x, ypos));
                int num1 = 6;
                if (Graphics.effectsLevel < 2)
                    num1 = 3;
                for (int index = 0; index < num1; ++index)
                {
                    float deg = index * 60f + Rando.Float(-10f, 10f);
                    float num2 = Rando.Float(12f, 20f);
                    Level.Add(new ExplosionPart(x + (float)Math.Cos(Maths.DegToRad(deg)) * num2, ypos - (float)Math.Sin(Maths.DegToRad(deg)) * num2));
                }
                _explodeFrames = 4;
            }
            else
            {
                --_explodeFrames;
                if (_explodeFrames != 0)
                    return;
                float x = this.x;
                float num3 = y - 2f;
                List<Bullet> varBullets = new List<Bullet>();
                for (int index = 0; index < 20; ++index)
                {
                    float num4 = (float)(index * 18f - 5f) + Rando.Float(10f);
                    ATPropExplosion type = new ATPropExplosion
                    {
                        range = 60f + Rando.Float(18f)
                    };
                    Bullet bullet = new Bullet(x + (float)(Math.Cos(Maths.DegToRad(num4)) * 6f), num3 - (float)(Math.Sin(Maths.DegToRad(num4)) * 6f), type, num4)
                    {
                        firedFrom = this
                    };
                    varBullets.Add(bullet);
                    Level.Add(bullet);
                }
                if (Network.isActive)
                {
                    Send.Message(new NMExplodingProp(varBullets), NetMessagePriority.ReliableOrdered);
                    varBullets.Clear();
                }
                if (Options.Data.flashing)
                {
                    Graphics.flashAdd = 1.3f;
                    Layer.Game.darken = 1.3f;
                }
                foreach (Window ignore in Level.CheckCircleAll<Window>(position, 40f))
                {
                    if (Level.CheckLine<Block>(position, ignore.position, ignore) == null)
                        ignore.Destroy(new DTImpact(this));
                }
                SFX.Play("explode");
                Level.Remove(this);
            }
        }
    }
}
