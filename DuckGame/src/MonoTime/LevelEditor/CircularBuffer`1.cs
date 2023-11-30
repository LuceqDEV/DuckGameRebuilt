﻿using System;

namespace DuckGame
{
    public class CircularBuffer<T>
    {
        protected T[] _data;
        protected int _size;
        protected int _begin;
        protected int _length;

        public CircularBuffer(int size = 100)
        {
            _data = new T[size];
            _size = size;
            _begin = 0;
            _length = 0;
        }

        public void Add(T val)
        {
            if (_length >= _size)
                AdvanceBuffer();
            _data[(_begin + _length) % _size] = val;
            ++_length;
        }

        public void AdvanceBuffer()
        {
            _begin = (_begin + 1) % _size;
            --_length;
            if (_length >= 0)
                return;
            _length = 0;
        }

        public T this[int key]
        {
            get
            {
                if (key >= _length || key < 0)
                    throw new Exception("Array Index Out Of Range");
                return _data[(_begin + key) % _size];
            }
            set => _data[(_begin + key) % _size] = value;
        }

        public int Count => _length;

        public void Clear() => _length = 0;
    }
}
