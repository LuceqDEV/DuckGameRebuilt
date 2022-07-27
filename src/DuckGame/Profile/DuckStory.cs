﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.DuckStory
// Assembly: DuckGame, Version=1.1.8175.33388, Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class DuckStory
    {
        public string text = "";
        public NewsSection section;

        public event DuckStory.OnStoryBeginDelegate OnStoryBegin;

        public void DoCallback()
        {
            if (this.OnStoryBegin == null)
                return;
            this.OnStoryBegin(this);
        }

        public delegate void OnStoryBeginDelegate(DuckStory story);
    }
}