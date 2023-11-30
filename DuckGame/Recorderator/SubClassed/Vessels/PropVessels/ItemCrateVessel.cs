﻿//wrote this on phone
namespace DuckGame
{
    public class ItemCrateVessel : SomethingSomethingVessel
    {
        public ItemCrateVessel(Thing th) : base(th)
        {
            tatchedTo.Add(typeof(ItemCrate));
            AddSynncl("position", new SomethingSync(typeof(int)));
            AddSynncl("hitpoints", new SomethingSync(typeof(float)));
        }
        public override SomethingSomethingVessel RecDeserialize(BitBuffer b)
        {
            ushort s = b.ReadUShort();
            if (s == 40404)
            {
                ItemCrateVessel v = new ItemCrateVessel(new ItemCrate(0, -2000));
                return v;
            }
            else
            {
                ItemCrateVessel v = new ItemCrateVessel(new ItemCrate(0, -2000) { contains = Editor.IDToType[s] });
                return v;
            }
        }
        public override BitBuffer RecSerialize(BitBuffer prevBuffer)
        {
            if (((ItemCrate)t).contains == null)
            {
                prevBuffer.Write((ushort)40404);
            }
            else prevBuffer.Write(Editor.IDToType[((ItemCrate)t).contains]);
            return prevBuffer;
        }
        public override void PlaybackUpdate()
        {
            ItemCrate c = (ItemCrate)t;
            c.position = CompressedVec2Binding.GetUncompressedVec2((int)valOf("position"), 10000);
            c._hitPoints = (float)valOf("hitpoints");
            base.PlaybackUpdate();
        }
        public override void RecordUpdate()
        {
            ItemCrate c = (ItemCrate)t;
            addVal("position", CompressedVec2Binding.GetCompressedVec2(c.position, 10000));
            addVal("hitpoints", c._hitPoints);
        }
    }
}
