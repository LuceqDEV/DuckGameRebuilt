﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.ModConfigurationException
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using System;
using System.Runtime.Serialization;

namespace DuckGame
{
    [Serializable]
    internal class ModConfigurationException : Exception
    {
        public ModConfigurationException()
        {
        }

        public ModConfigurationException(string message)
          : base(message)
        {
        }

        public ModConfigurationException(string message, Exception inner)
          : base(message, inner)
        {
        }

        protected ModConfigurationException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }
    }
}
