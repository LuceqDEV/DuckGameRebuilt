﻿using System.Collections;
using System.Collections.Generic;

namespace DuckGame
{
    public class ObjectListImmediate : IEnumerable<Thing>, IEnumerable
    {
        private HashSet<Thing> _emptyList = new HashSet<Thing>();
        private HashSet<Thing> _bigList = new HashSet<Thing>();
        private MultiMap<System.Type, Thing, HashSet<Thing>> _objectsByType = new MultiMap<System.Type, Thing, HashSet<Thing>>();

        public List<Thing> ToList()
        {
            List<Thing> list = new List<Thing>();
            list.AddRange(_bigList);
            return list;
        }

        public HashSet<Thing> this[System.Type key]
        {
            get
            {
                if (key == typeof(Thing))
                    return _bigList;
                return _objectsByType.ContainsKey(key) ? _objectsByType[key] : _emptyList;
            }
        }

        public int Count => _bigList.Count;

        public void Add(Thing obj)
        {
            foreach (System.Type key in Editor.AllBaseTypes[obj.GetType()])
                _objectsByType.Add(key, obj);
            _bigList.Add(obj);
        }

        public void AddRange(ObjectListImmediate list)
        {
            foreach (Thing thing in list)
                Add(thing);
        }

        public void Remove(Thing obj)
        {
            foreach (System.Type key in Editor.AllBaseTypes[obj.GetType()])
                _objectsByType.Remove(key, obj);
            _bigList.Remove(obj);
        }

        public void Clear()
        {
            _bigList.Clear();
            _objectsByType.Clear();
        }

        public bool Contains(Thing obj) => _bigList.Contains(obj);

        public IEnumerator<Thing> GetEnumerator() => _bigList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
