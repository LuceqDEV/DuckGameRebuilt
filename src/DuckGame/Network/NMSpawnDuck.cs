﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.NMSpawnDuck
// Assembly: DuckGame, Version=1.1.8175.33388, Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

namespace DuckGame
{
    public class NMSpawnDuck : NMEvent
    {
        public byte index;

        public NMSpawnDuck(byte idx) => this.index = idx;

        public NMSpawnDuck()
        {
        }

        public override void Activate()
        {
            if (this.index < (byte)0 || (int)this.index >= DuckNetwork.profiles.Count)
                return;
            Profile profile = DuckNetwork.profiles[(int)this.index];
            if (profile == null || profile.duck == null || profile.persona == null)
                return;
            if (profile.localPlayer)
                profile.duck.connection = DuckNetwork.localConnection;
            else
                profile.duck.connection = profile.connection;
            profile.duck.visible = true;
            Vec3 color = profile.persona.color;
            Level.Add((Thing)new SpawnLine(profile.duck.x, profile.duck.y, 0, 0.0f, new Color((int)color.x, (int)color.y, (int)color.z), 32f));
            Level.Add((Thing)new SpawnLine(profile.duck.x, profile.duck.y, 0, -4f, new Color((int)color.x, (int)color.y, (int)color.z), 4f));
            Level.Add((Thing)new SpawnLine(profile.duck.x, profile.duck.y, 0, 4f, new Color((int)color.x, (int)color.y, (int)color.z), 4f));
            SFX.Play("pullPin", 0.7f);
        }
    }
}