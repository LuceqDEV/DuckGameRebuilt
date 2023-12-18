﻿// Decompiled with JetBrains decompiler
// Type: XnaToFna.StubXDK.GamerServices.__LeaderboardWriter__
// Assembly: XnaToFna, Version=18.5.1.29483, Culture=neutral, PublicKeyToken=null
// MVID: C1D3521D-C7E9-4C43-B430-D28CC69450A3
// Assembly location: C:\Users\daniel\Desktop\Release\XnaToFna.exe

using MonoMod;
using System;
using System.Reflection;

namespace XnaToFna.StubXDK.GamerServices
{
    public static class __LeaderboardWriter__
    {
        private static Type t_LeaderboardEntry;
        private static ConstructorInfo ctor_LeaderboardEntry;

        [MonoModHook("Microsoft.Xna.Framework.GamerServices.LeaderboardEntry Microsoft.Xna.Framework.GamerServices.LeaderboardWriter::GetLeaderboard(Microsoft.Xna.Framework.GamerServices.LeaderboardIdentity)")]
        public static object GetLeaderboard(object writer, object identity)
        {
            if (t_LeaderboardEntry == null)
            {
                t_LeaderboardEntry = StubXDKHelper.GamerServicesAsm.GetType("Microsoft.Xna.Framework.GamerServices.LeaderboardEntry");
                ctor_LeaderboardEntry = t_LeaderboardEntry.GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, new Type[0], null);
            }
            return ctor_LeaderboardEntry.Invoke(new object[0]);
        }
    }
}
