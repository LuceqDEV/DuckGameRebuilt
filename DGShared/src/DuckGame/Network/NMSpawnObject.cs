﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.NMSpawnObject
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class NMSpawnObject : NMObjectMessage
    {
        public string name;
        public float xpos;
        public float ypos;

        public NMSpawnObject()
        {
        }

        public NMSpawnObject(string obj, float xVal, float yVal, ushort idVal)
          : base(idVal)
        {
            name = obj;
            xpos = xVal;
            ypos = yVal;
        }
    }
}
