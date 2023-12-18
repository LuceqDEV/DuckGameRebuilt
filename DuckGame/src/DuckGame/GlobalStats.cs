﻿using System.Collections.Generic;
using System.Reflection;

namespace DuckGame
{
    public class GlobalData : DataClass
    {
        public StatBinding matchesPlayed;
        public StatBinding ducksCrushed;
        public StatBinding kills;
        public StatBinding longestMatchPlayed;
        public StatBinding onlineWins;
        public StatBinding onlineMatches;
        public StatBinding drawsPlayed;
        public StatBinding littleDraws;
        public StatBinding timesSpawned;
        public StatBinding winsAsSwack;
        public StatBinding winsAsHair;
        public StatBinding disarms;
        public StatBinding jetFuelUsed;
        public StatBinding laserBulletsFired;
        public StatBinding strafeDistance;
        public StatBinding quacks;
        public StatBinding giantLaserKills;
        public StatBinding energyScimitarBlastKills;
        public StatBinding nettedDuckTossKills;
        public StatBinding presentsOpened;
        public StatBinding swordKills;
        public StatBinding secondsUnderwater;
        public StatBinding timeInMatches;
        public StatBinding timeInEditor;
        public StatBinding timeInArcade;
        public Dictionary<string, int> hatWins = new Dictionary<string, int>();
        public Dictionary<string, int> customMapPlayCount = new Dictionary<string, int>();
        public HashSet<ulong> blacklist = new HashSet<ulong>();

        public int flag { get; set; }

        public int angleShots { get; set; }

        public int bootedSinceUpdate { get; set; }

        public int bootedSinceSwitchHatPatch { get; set; }

        public int hatsStolen { get; set; }

        public int levelsPlayed { get; set; }

        public int killsAsSwack { get; set; }

        public int highestNewsCast { get; set; }

        public int numGachas { get; set; }

        public bool playedInTheMorning { get; set; }

        public bool playedDuringFullMoon { get; set; }

        public bool dugGrave { get; set; }

        public int timeJetpackedAsRagdoll { get; set; }

        public int unlockListIndex { get; set; }

        public bool typedJohnny { get; set; }

        public string boughtHats { get; set; }

        public bool hadTalk { get; set; }

        public int GetHatMatchWins(string hat) => hatWins.ContainsKey(hat) ? hatWins[hat] : 0;

        public GlobalData()
        {
            _nodeName = "Global";
            boughtHats = "";
            foreach (FieldInfo field in GetType().GetFields())
            {
                if (field.FieldType == typeof(StatBinding))
                {
                    StatBinding statBinding = new StatBinding();
                    statBinding.BindName(field.Name);
                    field.SetValue(this, statBinding);
                }
            }
        }
    }
}
