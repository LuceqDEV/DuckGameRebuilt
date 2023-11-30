﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
    public static class Persona
    {
        private static List<DuckPersona> _personasOriginalOrder = new List<DuckPersona>()
        {
          new DuckPersona(new Vec3( byte.MaxValue,  byte.MaxValue,  byte.MaxValue),0),
          new DuckPersona(new Vec3(125f, 125f, 125f),1),
          new DuckPersona(new Vec3(247f, 224f, 90f),2),
          new DuckPersona(new Vec3(205f, 107f, 29f),3),
          new DuckPersona(new Vec3(0f, 133f, 74f), new Vec3(0f, 102f, 57f), new Vec3(0f, 173f, 97f),4),
          new DuckPersona(new Vec3( byte.MaxValue, 105f, 117f), new Vec3(207f, 84f, 94f), new Vec3( byte.MaxValue, 158f, 166f),5),
          new DuckPersona(new Vec3(49f, 162f, 242f), new Vec3(13f, 123f, 181f), new Vec3(148f, 207f, 245f),6),
          new DuckPersona(new Vec3(175f, 85f, 221f), new Vec3(141f, 36f, 194f), new Vec3(213f, 165f, 238f),7)
        };
        private static List<DuckPersona> _personasShuffled;
        public static int seed;

        private static List<DuckPersona> _personas
        {
            get
            {
                if (_personasShuffled == null)
                    Shuffle();
                return _personasShuffled;
            }
        }

        public static DuckPersona Duck1 => _personas[0];

        public static DuckPersona Duck2 => _personas[1];

        public static DuckPersona Duck3 => _personas[2];

        public static DuckPersona Duck4 => _personas[3];

        public static DuckPersona Duck5 => _personas[4];

        public static DuckPersona Duck6 => _personas[5];

        public static DuckPersona Duck7 => _personas[6];

        public static DuckPersona Duck8 => _personas[7];

        public static IEnumerable<DuckPersona> all => _personas;

        public static List<DuckPersona> alllist => _personas;
        public static void Initialize()
        {
        }

        public static void Shuffle(int pSeed = -1)
        {
            seed = pSeed >= 0 ? pSeed : Rando.Int(2147483646);
            Random generator = Rando.generator;
            Rando.generator = new Random(seed);
            _personasShuffled = _personasOriginalOrder.ToList();
            Rando.generator = generator;
        }

        public static int Number(DuckPersona p) => _personas.IndexOf(p);
    }
}
