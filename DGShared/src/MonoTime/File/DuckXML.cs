﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.DuckXML
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using System.IO;
using System.Text;

namespace DuckGame
{
    public class DuckXML : DXMLNode
    {
        public bool invalid;

        public DuckXML()
          : base("")
        {
        }

        public void Save(string file)
        {
        }

        public static DuckXML Load(Stream s)
        {
            s.Position = 0L;
            return FromString(new StreamReader(s).ReadToEnd());
        }

        public static DuckXML Load(byte[] data) => FromString(Encoding.UTF8.GetString(data));

        public static DuckXML Load(string file)
        {
            using (StreamReader sr = File.OpenText(file))
            {
                string s = sr.ReadToEnd();
                //you then have to process the string
            }

            return FromString(File.ReadAllText(file, Encoding.UTF8)); /*DuckXML.FromString(File.OpenText(file).ReadToEnd());*/
        }

        public static DuckXML FromString(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                DevConsole.Log("|DGRED|DuckXML.FromString called with empty string!");
            DuckXML duckXml = new DuckXML();
            int index = 0;
            while (true)
            {
                DXMLNode node = ReadNode(text, ref index);
                if (node != null)
                    duckXml.Add(node);
                else
                    break;
            }
            if (duckXml.NumberOfElements == 0)
                duckXml.invalid = true;
            return duckXml;
        }
    }
}
