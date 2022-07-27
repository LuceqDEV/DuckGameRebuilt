﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.DuckXMLExtensionMethods
// Assembly: DuckGame, Version=1.1.8175.33388, Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using System.Collections.Generic;

namespace DuckGame
{
    public static class DuckXMLExtensionMethods
    {
        public static IEnumerable<DXMLAttribute> Attributes<T>(
          this IEnumerable<T> source,
          string name)
          where T : DXMLNode
        {
            List<DXMLAttribute> dxmlAttributeList = new List<DXMLAttribute>();
            foreach (T obj in source)
            {
                DXMLNode dxmlNode = (DXMLNode)obj;
                foreach (DXMLAttribute attribute in dxmlNode.Attributes())
                {
                    if (attribute.Name == name)
                        dxmlAttributeList.Add(attribute);
                }
                IEnumerable<DXMLAttribute> collection = dxmlNode.Elements().Attributes<DXMLNode>(name);
                dxmlAttributeList.AddRange(collection);
            }
            return (IEnumerable<DXMLAttribute>)dxmlAttributeList;
        }

        public static IEnumerable<DXMLNode> Elements<T>(this IEnumerable<T> source) where T : DXMLNode
        {
            List<DXMLNode> dxmlNodeList = new List<DXMLNode>();
            foreach (T obj in source)
            {
                DXMLNode dxmlNode = (DXMLNode)obj;
                dxmlNodeList.Add(dxmlNode);
                IEnumerable<DXMLNode> collection = dxmlNode.Elements().Elements<DXMLNode>();
                dxmlNodeList.AddRange(collection);
            }
            return (IEnumerable<DXMLNode>)dxmlNodeList;
        }

        public static IEnumerable<DXMLNode> Elements<T>(
          this IEnumerable<T> source,
          string name)
          where T : DXMLNode
        {
            List<DXMLNode> dxmlNodeList = new List<DXMLNode>();
            foreach (T obj in source)
            {
                DXMLNode dxmlNode = (DXMLNode)obj;
                if (dxmlNode.Name == name)
                    dxmlNodeList.Add(dxmlNode);
                IEnumerable<DXMLNode> collection = dxmlNode.Elements().Elements<DXMLNode>(name);
                dxmlNodeList.AddRange(collection);
            }
            return (IEnumerable<DXMLNode>)dxmlNodeList;
        }
    }
}