﻿using System.Collections;
using System.Linq;

namespace DuckGame
{
    //both gun and holdable vessels
    //are placeholder vessels for stuff that
    //isn't tatched to anything meaning that most
    //stuff that is just an empty gunvessel wont be
    //replayed properly, this be why replays wont be compatible with most mods
    public class GunVessel : HoldableVessel
    {
        public GunVessel(Thing th) : base(th)
        {
            AddSynncl("infoed_x", new SomethingSync(typeof(byte)));
            AddSynncl("infoed_g", new SomethingSync(typeof(byte)));
        }
        public override SomethingSomethingVessel RecDeserialize(BitBuffer b)
        {
            GunVessel v = new GunVessel(Editor.CreateThing(Editor.IDToType[b.ReadUShort()]));
            v.t.y = -2000;
            return v;
        }
        public override BitBuffer RecSerialize(BitBuffer prevBuffer)
        {
            //p sure that isn't needed but that might come back to bite me later

            //it isn't really needed but it helps a bit
            if (GetType() == typeof(GunVessel)) prevBuffer.Write(Editor.IDToType[t.GetType()]);
            return prevBuffer;
        }
        public virtual void ApplyFire()
        {
            Gun g = (Gun)t;
            g.kick = 1;
            g._flareAlpha = 1.5f;
        }
        public override void PlaybackUpdate()
        {
            Gun g = (Gun)t;
            if (!g.active)
            {
                if (g.owner != null) g.depth = g.owner.depth + 10;
                else g.depth = -0.1f;
            }
            if (syncled.ContainsKey("infoed_g"))
            {
                byte z = (byte)valOf("infoed_g");//lol? nvm it isn't as bad as it was before but imagine bad code being here ok?
                BitArray b_ARR = new BitArray(new byte[] { z });
                int divide = 64;
                int ammo = -1;
                for (int i = 0; i < 7; i++)
                {
                    if (b_ARR[i]) ammo += divide;
                    divide /= 2;
                }

                if (b_ARR[7]) ApplyFire();
                if (ammo == -1) g.infiniteAmmoVal = true;
                else
                {
                    g.ammo = ammo;
                    g.infiniteAmmoVal = false;
                }
            }
            if (syncled.ContainsKey("infoed_x"))
            {
                byte lz = (byte)valOf("infoed_x");
                BitArray br = new BitArray(new byte[] { lz });
                bool pop = br[0];
                bool puff = br[1];
                if (pop) g.PopShell(true);
                if (puff) g.DoAmmoClick();
            }
            base.PlaybackUpdate();
        }
        public override void RecordUpdate()
        {
            Gun g = (Gun)t;
            BitArray array_o_bits = new BitArray(8);

            int w = Maths.Clamp(g.ammo, 0, 125) + 1;
            if (g.infiniteAmmoVal) w = 0;
            array_o_bits[0] = (w & 64) > 0;
            array_o_bits[1] = (w & 32) > 0;
            array_o_bits[2] = (w & 16) > 0;
            array_o_bits[3] = (w & 8) > 0;
            array_o_bits[4] = (w & 4) > 0;
            array_o_bits[5] = (w & 2) > 0;
            array_o_bits[6] = (w & 1) > 0;
            array_o_bits[7] = g.recordKick;
            addVal("infoed_g", BitCrusher.BitArrayToByte(array_o_bits));

            BitArray br2 = new BitArray(8);
            br2[0] = g.recordPopShell;
            br2[1] = g.recordPuff;

            addVal("infoed_x", BitCrusher.BitArrayToByte(br2));
            g.recordPopShell = false;
            g.recordPuff = false;
            g.recordKick = false;
            base.RecordUpdate();
        }
    }
}
