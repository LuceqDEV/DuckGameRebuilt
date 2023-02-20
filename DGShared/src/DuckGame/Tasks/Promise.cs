﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Promise
//removed for regex reasons Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using System;
using System.Threading;

namespace DuckGame
{
    public class Promise
    {
        protected bool _finished;
        protected Delegate _delegate;

        public bool Finished
        {
            get
            {
                lock (this)
                    return _finished;
            }
            protected set
            {
                lock (this)
                    _finished = value;
            }
        }

        protected Promise(Delegate d) => _delegate = d;

        public Promise(Action action)
          : this((Delegate)action)
        {
        }

        public virtual void Execute()
        {
            _delegate.Method.Invoke(_delegate.Target, null);
            Finished = true;
        }

        public void WaitForComplete(uint waitMs = 13, uint maxAttempts = 0)
        {
            while (!Finished)
            {
                Thread.Sleep((int)waitMs);
                if (maxAttempts != 0U && --maxAttempts == 0U)
                    break;
            }
        }
    }
}
