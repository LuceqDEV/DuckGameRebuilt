﻿namespace DuckGame
{
    public struct GhostObjectHeader
    {
        public NetworkConnection connection;
        public NetIndex16 id;
        public ushort classID;
        public byte levelIndex;
        public NetIndex8 authority;
        public NetIndex16 tick;
        public bool delta;

        public GhostObjectHeader(bool pClean)
        {
            id = new NetIndex16();
            classID = 0;
            levelIndex = 0;
            authority = (NetIndex8)0;
            tick = (NetIndex16)2;
            delta = false;
            connection = null;
        }

        public NetMessagePriority priority => !delta ? NetMessagePriority.ReliableOrdered : NetMessagePriority.UnreliableUnordered;

        public static void Serialize(
          BitBuffer pBuffer,
          GhostObject pGhost,
          NetIndex16 pTick,
          bool pDelta,
          bool pMinimal)
        {
            pBuffer.Write((ushort)(int)pGhost.ghostObjectIndex);
            pBuffer.Write((object)pGhost.thing.authority);
            if (pDelta)
            {
                pBuffer.Write(true);
                pBuffer.Write((ushort)(int)pTick);
            }
            else
            {
                pBuffer.Write(false);
                if (pGhost.thing.connection.profile != null)
                    pBuffer.Write(pGhost.thing.connection.profile.networkIndex);
                else
                    pBuffer.Write(byte.MaxValue);
                if (pMinimal)
                    return;
                pBuffer.Write(Editor.IDToType[pGhost.thing.GetType()]);
                pBuffer.Write(DuckNetwork.levelIndex);
            }
        }

        public static GhostObjectHeader Deserialize(BitBuffer pBuffer, bool pMinimal)
        {
            GhostObjectHeader ghostObjectHeader = new GhostObjectHeader(true)
            {
                id = (NetIndex16)pBuffer.ReadUShort(),
                authority = (NetIndex8)pBuffer.ReadByte()
            };
            if (pBuffer.ReadBool())
            {
                ghostObjectHeader.tick = (NetIndex16)pBuffer.ReadUShort();
                ghostObjectHeader.delta = true;
            }
            else
            {
                byte index = pBuffer.ReadByte();
                if (index != byte.MaxValue)
                    ghostObjectHeader.connection = DuckNetwork.profiles[index].connection;
                if (!pMinimal)
                {
                    ghostObjectHeader.classID = pBuffer.ReadUShort();
                    ghostObjectHeader.levelIndex = pBuffer.ReadByte();
                }
            }
            return ghostObjectHeader;
        }
    }
}
