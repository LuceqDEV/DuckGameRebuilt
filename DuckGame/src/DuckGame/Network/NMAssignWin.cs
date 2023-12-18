﻿using System.Collections.Generic;

namespace DuckGame
{
    public class NMAssignWin : NMEvent
    {
        public List<Profile> profiles = new List<Profile>();
        public Profile theRealWinnerHere;
        protected string _sound = "scoreDing";

        public NMAssignWin(List<Profile> pProfiles, Profile pTheRealWinnerHere)
        {
            profiles = pProfiles;
            theRealWinnerHere = pTheRealWinnerHere;
        }

        public NMAssignWin()
        {
        }

        protected override void OnSerialize()
        {
            base.OnSerialize();
            _serializedData.Write((byte)profiles.Count);
            foreach (Profile profile in profiles)
                _serializedData.WriteProfile(profile);
        }

        public override void OnDeserialize(BitBuffer d)
        {
            base.OnDeserialize(d);
            byte num = d.ReadByte();
            for (int index = 0; index < num; ++index)
                profiles.Add(d.ReadProfile());
        }

        public override void Activate()
        {
            SFX.Play(_sound, 0.8f);
            foreach (Profile profile in profiles)
            {
                GameMode.lastWinners.Add(profile);
                Profile p = theRealWinnerHere != null ? theRealWinnerHere : profile;
                if (profile.duck != null)
                {
                    PlusOne plusOne = new PlusOne(0f, 0f, p, testMode: true)
                    {
                        _duck = profile.duck,
                        anchor = (Anchor)profile.duck
                    };
                    plusOne.anchor.offset = new Vec2(0f, -16f);
                    plusOne.depth = (Depth)0.95f;
                    Level.Add(plusOne);
                }
            }
            if (this is NMPlusOne)
                return;
            ++GameMode.numMatchesPlayed;
        }
    }
}
