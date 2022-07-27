﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.InputCode
// Assembly: DuckGame, Version=1.1.8175.33388, Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using System;
using System.Collections.Generic;

namespace DuckGame
{
    public class InputCode
    {
        public string name = "";
        public string description = "";
        public string chancyComment = "";
        public Action action;
        private Dictionary<string, InputCode.InputCodeProfileStatus> status = new Dictionary<string, InputCode.InputCodeProfileStatus>();
        public List<string> triggers = new List<string>();
        public float breakSpeed = 0.04f;
        private bool hasDoubleInputs;
        private bool _initializedDoubleInputs;
        private static Dictionary<string, InputCode> _codes = new Dictionary<string, InputCode>();

        public InputCode.InputCodeProfileStatus GetStatus(InputProfile p)
        {
            InputCode.InputCodeProfileStatus status = (InputCode.InputCodeProfileStatus)null;
            if (!this.status.TryGetValue(p.name, out status))
            {
                status = new InputCode.InputCodeProfileStatus();
                this.status[p.name] = status;
            }
            return status;
        }

        public bool Update(InputProfile p)
        {
            if (p == null)
                return false;
            if (!this._initializedDoubleInputs)
            {
                this._initializedDoubleInputs = true;
                foreach (string trigger in this.triggers)
                {
                    if (trigger.Contains("|"))
                    {
                        this.hasDoubleInputs = true;
                        break;
                    }
                }
            }
            InputCode.InputCodeProfileStatus status = this.GetStatus(p);
            if (status.lastUpdateFrame == Graphics.frame)
                return status.lastResult;
            status.lastUpdateFrame = Graphics.frame;
            status.breakTimer -= this.breakSpeed;
            if ((double)status.breakTimer <= 0.0)
                status.Break();
            string trigger1 = this.triggers[status.currentIndex];
            int num = 0;
            if (this.hasDoubleInputs && trigger1.Contains("|"))
            {
                string str1 = trigger1;
                char[] chArray = new char[1] { '|' };
                foreach (string str2 in str1.Split(chArray))
                    num |= 1 << Network.synchronizedTriggers.Count - Network.synchronizedTriggers.IndexOf(str2);
            }
            else
                num = 1 << Network.synchronizedTriggers.Count - Network.synchronizedTriggers.IndexOf(trigger1);
            if ((int)p.state == num)
            {
                if (!status.release)
                {
                    if (status.currentIndex == this.triggers.Count - 1)
                    {
                        status.Break();
                        status.lastResult = true;
                        return true;
                    }
                    status.release = true;
                    if (status.currentIndex == 0)
                        status.breakTimer = 1f;
                }
            }
            else if (p.state == (ushort)0)
            {
                if (status.release)
                    status.Progress();
            }
            else
                status.Break();
            status.lastResult = false;
            return false;
        }

        public static implicit operator InputCode(string s)
        {
            InputCode inputCode = (InputCode)null;
            if (!InputCode._codes.TryGetValue(s, out inputCode))
            {
                inputCode = new InputCode();
                inputCode.triggers = new List<string>((IEnumerable<string>)s.Split('|'));
                InputCode._codes[s] = inputCode;
            }
            return inputCode;
        }

        public class InputCodeProfileStatus
        {
            public long lastUpdateFrame;
            public bool lastResult;
            public int currentIndex;
            public bool release;
            public float breakTimer = 1f;

            public void Break()
            {
                if (this.currentIndex <= 0)
                    return;
                this.currentIndex = 0;
                this.release = false;
                this.breakTimer = 1f;
            }

            public void Progress()
            {
                ++this.currentIndex;
                this.release = false;
                this.breakTimer = 1f;
            }
        }
    }
}