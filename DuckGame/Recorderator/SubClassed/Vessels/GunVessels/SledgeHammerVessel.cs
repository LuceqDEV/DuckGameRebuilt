namespace DuckGame
{
    public class SledgeHammerVessel : GunVessel
    {
        public SledgeHammerVessel(Thing th) : base(th)
        {
            tatchedTo.Add(typeof(SledgeHammer));
            AddSynncl("swing", new SomethingSync(typeof(ushort)));
        }
        public override SomethingSomethingVessel RecDeserialize(BitBuffer b)
        {
            SledgeHammerVessel v = new SledgeHammerVessel(new SledgeHammer(0, -2000));
            return v;
        }
        public override BitBuffer RecSerialize(BitBuffer prevBuffer)
        {
            return prevBuffer;
        }
        public override void PlaybackUpdate()
        {
            SledgeHammer p = (SledgeHammer)t;
            ushort hat = (ushort)valOf("swing");
            Extensions.SetPrivateFieldValue(p, "_swing", BitCrusher.UShortToFloat(hat, 13, 3) - 3);
            base.PlaybackUpdate();
        }
        public override void RecordUpdate()
        {
            SledgeHammer p = (SledgeHammer)t;
            addVal("swing", BitCrusher.FloatToUShort(p.swing + 3, 13));
            base.RecordUpdate();
        }
    }
}
