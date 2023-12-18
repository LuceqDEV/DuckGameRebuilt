﻿using System.Linq;

namespace DuckGame
{
    public class ItemBoxVessel : SomethingSomethingVessel
    {
        public ItemBoxVessel(Thing th) : base(th)
        {
            tatchedTo.Add(typeof(ItemBox));
            tatchedTo.Add(typeof(ItemBoxOneTime));
            tatchedTo.Add(typeof(ItemBoxRandom));
            tatchedTo.Add(typeof(PurpleBlock));

            AddSynncl("containing", new SomethingSync(typeof(ushort)));
            AddSynncl("position", new SomethingSync(typeof(int)));
            AddSynncl("_hit", new SomethingSync(typeof(bool)));
        }
        public override SomethingSomethingVessel RecDeserialize(BitBuffer b)
        {
            ushort s = b.ReadUShort();
            Thing t = null;
            try
            {
                t = Editor.CreateThing(Editor.IDToType[s]);
            }
            catch
            {
                destroyedReason = "Couldn't create itembox " + s;
                DevConsole.Log("Itembox couldn't create thing " + s);
            }
            ItemBoxVessel v = new ItemBoxVessel(t);
            return v;
        }
        public override BitBuffer RecSerialize(BitBuffer prevBuffer)
        {
            prevBuffer.Write(Editor.IDToType[t.GetType()]);
            return prevBuffer;
        }
        public override void PlaybackUpdate()
        {
            ItemBox i = (ItemBox)t;
            i.position = CompressedVec2Binding.GetUncompressedVec2((int)valOf("position"), 10000);
            i._hit = (bool)valOf("_hit");
            int hObj = (ushort)valOf("containing") - 1;
            if (hObj == -1 && i.containedObject != null)
            {
                i.containedObject.visible = true;
                i.containedObject = null;
            }
            else if (hObj != -1 && i.containedObject == null && Corderator.instance.somethingMap.Contains(hObj))
            {
                if (Corderator.instance == null) Main.SpecialCode = "corderator was null";
                i.containedObject = (PhysicsObject)Corderator.instance.somethingMap[hObj];
                i.containedObject.visible = false;
            }
            base.PlaybackUpdate();
        }
        public override void RecordUpdate()
        {
            ItemBox i = (ItemBox)t;
            if (i.containedObject != null)
            {
                addVal("containing", Corderator.Indexify(i.containedObject));
            }
            else addVal("containing", (ushort)0);
            addVal("position", CompressedVec2Binding.GetCompressedVec2(i.position, 10000));
            addVal("_hit", i._hit);
        }
    }
}
