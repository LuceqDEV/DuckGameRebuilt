﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.NMLogRequestIncoming
// Assembly: DuckGame, Version=1.1.8175.33388, Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class NMLogRequestIncoming : NMEvent
    {
        public int numChunks;

        public NMLogRequestIncoming(int pNumChunks) => this.numChunks = pNumChunks;

        public NMLogRequestIncoming()
        {
        }

        public override void Activate()
        {
            if (!DevConsole.core.requestingLogs.Contains(this.connection))
                return;
            this.connection.logTransferSize = this.numChunks;
            this.connection.logTransferProgress = 0;
        }
    }
}