﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DuckGame
{
    public class DuckTitle
    {
        private static List<DuckTitle> _titles = new List<DuckTitle>();
        private Dictionary<PropertyInfo, float> _requirementsFloat = new Dictionary<PropertyInfo, float>();
        private Dictionary<PropertyInfo, int> _requirementsInt = new Dictionary<PropertyInfo, int>();
        private Dictionary<PropertyInfo, string> _requirementsString = new Dictionary<PropertyInfo, string>();
        private string _name;
        private string _previousOwner;

        public static void Initialize()
        {
            foreach (string file in Content.GetFiles("Content/titles"))
            {
                IEnumerable<DXMLNode> source = DuckXML.Load(DuckFile.OpenStream(file)).Elements("Title");
                if (source != null)
                {
                    DXMLAttribute dxmlAttribute1 = source.Attributes("name").FirstOrDefault();
                    if (dxmlAttribute1 != null)
                    {
                        DuckTitle duckTitle = new DuckTitle
                        {
                            _name = dxmlAttribute1.Value
                        };
                        bool flag = false;
                        foreach (DXMLNode element in source.Elements())
                        {
                            if (element.Name == "StatRequirement")
                            {
                                DXMLAttribute statNameAttrib = element.Attributes("name").FirstOrDefault();
                                DXMLAttribute dxmlAttribute2 = element.Attributes("value").FirstOrDefault();
                                if (statNameAttrib != null && dxmlAttribute2 != null)
                                {
                                    PropertyInfo key = typeof(ProfileStats).GetProperties().FirstOrDefault(x => x.Name == statNameAttrib.Value);
                                    if (key != null)
                                    {
                                        if (key.GetType() == typeof(float))
                                            duckTitle._requirementsFloat.Add(key, Change.ToSingle(dxmlAttribute2.Value));
                                        else if (key.GetType() == typeof(int))
                                            duckTitle._requirementsFloat.Add(key, Convert.ToInt32(dxmlAttribute2.Value));
                                        else
                                            duckTitle._requirementsString.Add(key, dxmlAttribute2.Value);
                                    }
                                    else
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    flag = true;
                                    break;
                                }
                            }
                        }
                        if (!flag)
                            _titles.Add(duckTitle);
                    }
                }
            }
        }

        public static DuckTitle GetTitle(string title) => _titles.FirstOrDefault(x => x._name == title);

        public string previousOwner
        {
            get => _previousOwner;
            set => _previousOwner = value;
        }

        public static Dictionary<DuckTitle, float> ScoreTowardsTitles(Profile p)
        {
            Dictionary<DuckTitle, float> dictionary = new Dictionary<DuckTitle, float>();
            foreach (DuckTitle title in _titles)
                dictionary[title] = title.ScoreTowardsTitle(p);
            return dictionary;
        }

        public float ScoreTowardsTitle(Profile p)
        {
            float num1 = 0f;
            float num2 = 0f;
            foreach (KeyValuePair<PropertyInfo, float> keyValuePair in _requirementsFloat)
            {
                num1 += keyValuePair.Value;
                num2 += (float)keyValuePair.Key.GetValue(p.stats, null);
            }
            int num3 = 0;
            int num4 = 0;
            foreach (KeyValuePair<PropertyInfo, int> keyValuePair in _requirementsInt)
            {
                num3 += keyValuePair.Value;
                num4 += (int)keyValuePair.Key.GetValue(p.stats, null);
            }
            int num5 = 0;
            int num6 = 0;
            foreach (KeyValuePair<PropertyInfo, string> keyValuePair in _requirementsString)
            {
                ++num5;
                if ((string)keyValuePair.Key.GetValue(p.stats, null) == keyValuePair.Value)
                    ++num6;
            }
            return (float)((num2 + num4 + num6) / (num1 + num3 + num5));
        }

        public void UpdateTitles()
        {
            foreach (DuckTitle title in _titles)
                ;
        }
    }
}
