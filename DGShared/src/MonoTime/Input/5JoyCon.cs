﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.SwitchHandheldController
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using System.Runtime.CompilerServices;

namespace DuckGame
{
    public class SwitchHandheldController : SwitchJoyConDual
    {
        public SwitchHandheldController()
          : base(0)
        {
        }

        public extern override bool isConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; }
    }
}
