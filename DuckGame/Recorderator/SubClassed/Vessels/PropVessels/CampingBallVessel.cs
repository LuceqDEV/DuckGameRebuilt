﻿namespace DuckGame
{
    public class CampingBallVessel : SomethingSomethingVessel
    {
        public CampingBallVessel(Thing th) : base(th)
        {
            tatchedTo.Add(typeof(CampingBall));
            AddSynncl("position", new SomethingSync(typeof(int)));
            AddSynncl("velocity", new SomethingSync(typeof(int)));
        }
        public override SomethingSomethingVessel RecDeserialize(BitBuffer b)
        {
            CampingBallVessel v = new CampingBallVessel(new CampingBall(0, -2000, null));
            return v;
        }
        public override void PlaybackUpdate()
        {
            CampingBall c = (CampingBall)t;
            c.position = CompressedVec2Binding.GetUncompressedVec2((int)valOf("position"), 10000);
            c.velocity = CompressedVec2Binding.GetUncompressedVec2((int)valOf("velocity"), 20);
            base.PlaybackUpdate();
        }
        public override void RecordUpdate()
        {
            CampingBall n = (CampingBall)t;
            addVal("position", CompressedVec2Binding.GetCompressedVec2(n.position, 10000));
            addVal("velocity", CompressedVec2Binding.GetCompressedVec2(n.velocity, 20));
        }
    }
}
