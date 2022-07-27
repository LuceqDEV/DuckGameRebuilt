﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.ProfilesCore
// Assembly: DuckGame, Version=1.1.8175.33388, Culture=neutral, PublicKeyToken=null
// MVID: C907F20B-C12B-4773-9B1E-25290117C0E4
// Assembly location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.exe
// XML documentation location: D:\Program Files (x86)\Steam\steamapps\common\Duck Game\DuckGame.xml

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DuckGame
{
    public class ProfilesCore
    {
        public List<Profile> defaultProfileMappings = new List<Profile>()
    {
      (Profile) null,
      (Profile) null,
      (Profile) null,
      (Profile) null,
      (Profile) null,
      (Profile) null,
      (Profile) null,
      (Profile) null
    };
        public List<Profile> _profiles;
        private Profile _experienceProfile;
        public Team EnvironmentTeam = new Team("Environment", "hats/noHat", true);
        public Profile EnvironmentProfile;
        public bool initialized;
        private static int numExperienceProfiles;

        public IEnumerable<Profile> all => DuckNetwork.active ? (IEnumerable<Profile>)DuckNetwork.profiles : (IEnumerable<Profile>)this._profiles;

        public List<Profile> allCustomProfiles
        {
            get
            {
                List<Profile> allCustomProfiles = new List<Profile>();
                for (int index = 8; index < this._profiles.Count; ++index)
                {
                    if (this._profiles[index].steamID == 0UL || this._profiles[index] == Profiles.experienceProfile)
                        allCustomProfiles.Add(this._profiles[index]);
                }
                return allCustomProfiles;
            }
        }

        public IEnumerable<Profile> universalProfileList
        {
            get
            {
                List<Profile> universalProfileList = new List<Profile>((IEnumerable<Profile>)this._profiles);
                universalProfileList.AddRange((IEnumerable<Profile>)DuckNetwork.profiles);
                return (IEnumerable<Profile>)universalProfileList;
            }
        }

        public Profile DefaultExperienceProfile => this._experienceProfile;

        public Profile DefaultPlayer1 => !Network.isActive ? Profile.defaultProfileMappings[0] : this.all.ElementAt<Profile>(0);

        public Profile DefaultPlayer2 => !Network.isActive ? Profile.defaultProfileMappings[1] : this.all.ElementAt<Profile>(1);

        public Profile DefaultPlayer3 => !Network.isActive ? Profile.defaultProfileMappings[2] : this.all.ElementAt<Profile>(2);

        public Profile DefaultPlayer4 => !Network.isActive ? Profile.defaultProfileMappings[3] : this.all.ElementAt<Profile>(3);

        public Profile DefaultPlayer5 => !Network.isActive ? Profile.defaultProfileMappings[4] : this.all.ElementAt<Profile>(4);

        public Profile DefaultPlayer6 => !Network.isActive ? Profile.defaultProfileMappings[5] : this.all.ElementAt<Profile>(5);

        public Profile DefaultPlayer7 => !Network.isActive ? Profile.defaultProfileMappings[6] : this.all.ElementAt<Profile>(6);

        public Profile DefaultPlayer8 => !Network.isActive ? Profile.defaultProfileMappings[7] : this.all.ElementAt<Profile>(7);

        public ProfilesCore() => this.EnvironmentProfile = new Profile("Environment", InputProfile.Get("Blank"), this.EnvironmentTeam, Persona.Duck1);

        public int DefaultProfileNumber(Profile p) => this._profiles.IndexOf(p);

        public List<Profile> active
        {
            get
            {
                List<Profile> active = new List<Profile>();
                foreach (Profile profile in Profiles.all)
                {
                    if (profile.team != null)
                        active.Add(profile);
                }
                return active;
            }
        }

        public List<Profile> activeNonSpectators
        {
            get
            {
                List<Profile> activeNonSpectators = new List<Profile>();
                foreach (Profile profile in Profiles.all)
                {
                    if (profile.team != null && profile.slotType != SlotType.Spectator)
                        activeNonSpectators.Add(profile);
                }
                return activeNonSpectators;
            }
        }

        public void Initialize()
        {
            this._profiles = new List<Profile>()
      {
        new Profile("Player1", InputProfile.Get("MPPlayer1"), Teams.Player1, Persona.Duck1, false, "PLAYER1", true),
        new Profile("Player2", InputProfile.Get("MPPlayer2"), Teams.Player2, Persona.Duck2, false, "PLAYER2", true),
        new Profile("Player3", InputProfile.Get("MPPlayer3"), Teams.Player3, Persona.Duck3, false, "PLAYER3", true),
        new Profile("Player4", InputProfile.Get("MPPlayer4"), Teams.Player4, Persona.Duck4, false, "PLAYER4", true),
        new Profile("Player5", InputProfile.Get("MPPlayer5"), Teams.Player5, Persona.Duck5, false, "PLAYER5", true),
        new Profile("Player6", InputProfile.Get("MPPlayer6"), Teams.Player6, Persona.Duck6, false, "PLAYER6", true),
        new Profile("Player7", InputProfile.Get("MPPlayer7"), Teams.Player7, Persona.Duck7, false, "PLAYER7", true),
        new Profile("Player8", InputProfile.Get("MPPlayer8"), Teams.Player8, Persona.Duck8, false, "PLAYER8", true)
      };
            Profile.defaultProfileMappings[0] = this._profiles[0];
            Profile.defaultProfileMappings[1] = this._profiles[1];
            Profile.defaultProfileMappings[2] = this._profiles[2];
            Profile.defaultProfileMappings[3] = this._profiles[3];
            Profile.defaultProfileMappings[4] = this._profiles[4];
            Profile.defaultProfileMappings[5] = this._profiles[5];
            Profile.defaultProfileMappings[6] = this._profiles[6];
            Profile.defaultProfileMappings[7] = this._profiles[7];
            Profile.loading = true;
            DevConsole.Log(DCSection.General, "Loading profiles from (" + DuckFile.profileDirectory + ")");
            string[] files = DuckFile.GetFiles(DuckFile.profileDirectory);
            DevConsole.Log(DCSection.General, "Found (" + ((IEnumerable<string>)files).Count<string>().ToString() + ") profiles.");
            List<Profile> profileList = new List<Profile>();
            foreach (string path in files)
            {
                if (!path.Contains("__backup_"))
                {
                    DuckXML duckXml = DuckFile.LoadDuckXML(path);
                    if (duckXml != null && !duckXml.invalid && duckXml.Element("Profile") != null)
                    {
                        string name = duckXml.Element("Profile").Element("Name").Value;
                        DXMLNode dxmlNode = duckXml.Element("Profile").Element("SteamID");
                        try
                        {
                            if (dxmlNode != null)
                            {
                                ulong num;
                                try
                                {
                                    num = Change.ToUInt64((object)dxmlNode.Value.Trim());
                                }
                                catch (Exception ex)
                                {
                                    num = 0UL;
                                }
                                if (num != 0UL)
                                {
                                    if (Path.GetFileNameWithoutExtension(path) != num.ToString())
                                        continue;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        bool flag = false;
                        Profile p = this._profiles.FirstOrDefault<Profile>((Func<Profile, bool>)(pro => pro.name == name));
                        if (p == null || !Profiles.IsDefault(p))
                        {
                            p = new Profile("");
                            p.fileName = path;
                            flag = true;
                        }
                        if (MonoMain.logFileOperations)
                            DevConsole.Log(DCSection.General, "Profile().Load(" + path + ")");
                        IEnumerable<DXMLNode> source = duckXml.Elements("Profile");
                        if (source != null)
                        {
                            foreach (DXMLNode element1 in source.Elements<DXMLNode>())
                            {
                                if (element1.Name == "ID" && !Profiles.IsDefault(p))
                                    p.SetID(element1.Value);
                                else if (element1.Name == "Name")
                                    p.name = element1.Value;
                                else if (element1.Name == "Mood")
                                    p.funslider = Change.ToSingle((object)element1.Value);
                                else if (element1.Name == "PreferredColor" && !Profiles.IsDefault(p))
                                    p.preferredColor = Change.ToInt32((object)element1.Value);
                                else if (element1.Name == "NS")
                                    p.numSandwiches = Change.ToInt32((object)element1.Value);
                                else if (element1.Name == "MF")
                                    p.milkFill = Change.ToInt32((object)element1.Value);
                                else if (element1.Name == "LML")
                                    p.littleManLevel = Change.ToInt32((object)element1.Value);
                                else if (element1.Name == "NLM")
                                    p.numLittleMen = Change.ToInt32((object)element1.Value);
                                else if (element1.Name == "LMB")
                                    p.littleManBucks = Change.ToInt32((object)element1.Value);
                                else if (element1.Name == "RSXP")
                                    p.roundsSinceXP = Change.ToInt32((object)element1.Value);
                                else if (element1.Name == "TimesMet")
                                    p.timesMetVincent = Change.ToInt32((object)element1.Value);
                                else if (element1.Name == "TimesMet2")
                                    p.timesMetVincentSale = Change.ToInt32((object)element1.Value);
                                else if (element1.Name == "TimesMet3")
                                    p.timesMetVincentSell = Change.ToInt32((object)element1.Value);
                                else if (element1.Name == "TimesMet4")
                                    p.timesMetVincentImport = Change.ToInt32((object)element1.Value);
                                else if (element1.Name == "TimesMet5")
                                    p.timesMetVincentHint = Change.ToInt32((object)element1.Value);
                                else if (element1.Name == "TimeOfDay")
                                    p.timeOfDay = Change.ToSingle((object)element1.Value);
                                else if (element1.Name == "CD")
                                    p.currentDay = Change.ToInt32((object)element1.Value);
                                else if (element1.Name == "Punished")
                                    p.punished = Change.ToInt32((object)element1.Value);
                                else if (element1.Name == "XtraPoints")
                                {
                                    p.xp = Change.ToInt32((object)element1.Value);
                                    if (MonoMain.logFileOperations)
                                        DevConsole.Log(DCSection.General, "Profile(" + name != null ? name : ").loadXP(" + p.xp.ToString() + ")");
                                }
                                else if (element1.Name == "FurniPositions")
                                {
                                    p.furniturePositionData = BitBuffer.FromString(element1.Value);
                                    p.prevFurniPositionData = p.furniturePositionData.ToString();
                                }
                                else if (element1.Name == "Fowner")
                                    p.furnitureOwnershipData = BitBuffer.FromString(element1.Value);
                                else if (element1.Name == "SteamID")
                                {
                                    try
                                    {
                                        p.steamID = Change.ToUInt64((object)element1.Value.Trim());
                                    }
                                    catch (Exception ex)
                                    {
                                        p.steamID = 0UL;
                                    }
                                    if (p.steamID != 0UL && !(Path.GetFileNameWithoutExtension(path) != p.steamID.ToString()))
                                        profileList.Add(p);
                                }
                                else if (element1.Name == "LastKnownName")
                                    p.lastKnownName = element1.Value;
                                else if (element1.Name == "Stats")
                                    p.stats.Deserialize(element1);
                                else if (element1.Name == "Unlocks")
                                {
                                    string[] strArray = element1.Value.Split('|');
                                    int num = Math.Min(strArray.Length, 100);
                                    for (int index = 0; index < num; ++index)
                                    {
                                        if (strArray[index] != "" && !p.unlocks.Contains(strArray[index]))
                                            p.unlocks.Add(strArray[index]);
                                    }
                                }
                                else if (element1.Name == "Tickets")
                                    p.ticketCount = Convert.ToInt32(element1.Value);
                                else if (element1.Name == "ChallengeData")
                                {
                                    try
                                    {
                                        byte[] bytes = Editor.StringToBytes(element1.Value);
                                        BitBuffer bitBuffer = new BitBuffer(bytes, false);
                                        bitBuffer.lengthInBytes = bytes.Length;
                                        while (bitBuffer.position < bitBuffer.lengthInBytes)
                                        {
                                            ChallengeSaveData challengeSaveData = ChallengeSaveData.FromBuffer(bitBuffer.ReadBitBuffer(false));
                                            if (challengeSaveData.trophy != TrophyType.Baseline || challengeSaveData.bestTime != 0)
                                            {
                                                challengeSaveData.profileID = p.id;
                                                if (challengeSaveData.trophy == TrophyType.Developer)
                                                    Options.Data.gotDevMedal = true;
                                                p.challengeData.Add(challengeSaveData.challenge, challengeSaveData);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        DevConsole.Log(DCSection.General, "Profile (" + path + ") failed to load ChallengeData:" + ex.Message);
                                    }
                                }
                                else if (element1.Name == "Mappings" && !MonoMain.defaultControls)
                                {
                                    p.inputMappingOverrides.Clear();
                                    foreach (DXMLNode element2 in element1.Elements())
                                    {
                                        if (element2.Name == "InputMapping")
                                        {
                                            DeviceInputMapping deviceInputMapping = new DeviceInputMapping();
                                            deviceInputMapping.Deserialize(element2);
                                            try
                                            {
                                                DeviceInputMapping defaultMapping = Input.GetDefaultMapping(deviceInputMapping.deviceName, deviceInputMapping.deviceGUID);
                                                if (defaultMapping != null)
                                                {
                                                    foreach (KeyValuePair<string, int> keyValuePair in defaultMapping.map)
                                                    {
                                                        if (!deviceInputMapping.map.ContainsKey(keyValuePair.Key))
                                                            deviceInputMapping.MapInput(keyValuePair.Key, keyValuePair.Value);
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                            p.inputMappingOverrides.Add(deviceInputMapping);
                                        }
                                    }
                                }
                            }
                        }
                        if (flag)
                            this._profiles.Add(p);
                    }
                }
            }
            Profile p1 = (Profile)null;
            ulong num1 = 0;
            ProfilesCore.numExperienceProfiles = 0;
            if (Steam.user == null)
            {
                p1 = Profiles.DefaultPlayer1;
            }
            else
            {
                if (Steam.user != null && Steam.user.id != 0UL)
                    Options.Data.lastSteamID = Steam.user.id;
                num1 = Options.Data.lastSteamID;
                foreach (Profile profile in Profiles.all)
                {
                    if (profile.steamID != 0UL)
                        ++ProfilesCore.numExperienceProfiles;
                    if ((long)profile.steamID == (long)num1)
                        p1 = profile;
                }
            }
            if (ProfilesCore.numExperienceProfiles == 0)
            {
                Options.Data.defaultAccountMerged = true;
                Options.Data.didAutoMerge = true;
            }
            string file = (string)null;
            if (p1 == null)
            {
                if (num1 != 0UL)
                {
                    Profile profile = this._profiles.FirstOrDefault<Profile>((Func<Profile, bool>)(x => x.name == "experience_profile" && x.id == "replace_with_steam"));
                    if (profile != null)
                    {
                        foreach (KeyValuePair<string, ChallengeSaveData> keyValuePair in profile.challengeData)
                            keyValuePair.Value.profileID = num1.ToString();
                        profile.name = num1.ToString();
                        profile.SetID(num1.ToString());
                        profile.steamID = num1;
                        p1 = profile;
                        file = profile.fileName;
                    }
                    else
                    {
                        p1 = new Profile(num1.ToString(), varID: num1.ToString());
                        p1.steamID = num1;
                        ++ProfilesCore.numExperienceProfiles;
                    }
                }
                else
                    p1 = new Profile("experience_profile", varID: "replace_with_steam");
                if (!Profiles.all.Contains<Profile>(p1))
                    Profiles.Add(p1);
                this.Save(p1);
                if (file != null)
                    DuckFile.Delete(file);
            }
            this._experienceProfile = p1;
            this._experienceProfile.defaultTeam = Teams.Player1;
            Profile.defaultProfileMappings[0] = this._experienceProfile;
            if (Options.legacyPreferredColor > 0)
                this._experienceProfile.preferredColor = Options.legacyPreferredColor;
            byte localFlippers = Profile.CalculateLocalFlippers();
            foreach (Profile profile in this._profiles)
            {
                profile.flippers = localFlippers;
                profile.ticketCount = Challenges.GetTicketCount(profile);
                if (profile.ticketCount < 0)
                    profile.ticketCount = 0;
            }
            Profile.loading = false;
            this.initialized = true;
        }

        public static int CouldAutomerge()
        {
            if (ProfilesCore.numExperienceProfiles == 1)
            {
                bool flag1 = false;
                foreach (KeyValuePair<string, ChallengeData> challenge in Challenges.challenges)
                {
                    if (Profiles.experienceProfile.GetSaveData(challenge.Key).trophy != TrophyType.Baseline)
                    {
                        flag1 = true;
                        break;
                    }
                }
                bool flag2 = false;
                foreach (KeyValuePair<string, ChallengeData> challenge in Challenges.challenges)
                {
                    if (Profiles.DefaultPlayer1.GetSaveData(challenge.Key).trophy != TrophyType.Baseline)
                    {
                        flag2 = true;
                        break;
                    }
                }
                if (flag1 | flag2 && !(flag1 & flag2))
                    return !flag2 ? 2 : 1;
            }
            return 0;
        }

        public static void TryAutomerge()
        {
            int num = ProfilesCore.CouldAutomerge();
            if (num <= 0 || Options.Data.didAutoMerge)
                return;
            Options.MergeDefault(num == 1, false);
            Options.Data.didAutoMerge = true;
        }

        public List<ProfileStatRank> GetEndOfRoundStatRankings(StatInfo stat)
        {
            List<ProfileStatRank> roundStatRankings = new List<ProfileStatRank>();
            foreach (Profile pro in this.active)
            {
                float statCalculation = pro.endOfRoundStats.GetStatCalculation(stat);
                bool flag = false;
                for (int index = 0; index < roundStatRankings.Count; ++index)
                {
                    if ((double)statCalculation > (double)roundStatRankings[index].value)
                    {
                        roundStatRankings.Insert(index, new ProfileStatRank(stat, statCalculation, pro));
                        flag = true;
                        break;
                    }
                    if ((double)statCalculation == (double)roundStatRankings[index].value)
                    {
                        roundStatRankings[index].profiles.Add(pro);
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                    roundStatRankings.Add(new ProfileStatRank(stat, statCalculation, pro));
            }
            return roundStatRankings;
        }

        public bool IsDefault(Profile p)
        {
            if (p == null)
                return false;
            if (p.linkedProfile != null)
                return this.IsDefault(p.linkedProfile);
            for (int index = 0; index < DG.MaxPlayers; ++index)
            {
                if (this._profiles[index] == p)
                    return true;
            }
            return false;
        }

        public bool IsExperience(Profile p)
        {
            if (p == null)
                return false;
            return p.linkedProfile != null ? this.IsExperience(p.linkedProfile) : p == Profiles.experienceProfile;
        }

        public bool IsDefaultName(string p)
        {
            for (int index = 0; index < DG.MaxPlayers; ++index)
            {
                if (this._profiles[index].name == p)
                    return true;
            }
            return false;
        }

        public void Add(Profile p)
        {
            this._profiles.Add(p);
            this.Save(p);
        }

        public void Remove(Profile p) => this._profiles.Remove(p);

        public void Delete(Profile p)
        {
            this._profiles.Remove(p);
            DuckFile.Delete(this.GetFileName(p));
        }

        public string GetFileName(Profile p)
        {
            if (p == this.EnvironmentProfile)
                return (string)null;
            if (p.linkedProfile != null)
                return this.GetFileName(p.linkedProfile);
            if (p.isNetworkProfile)
                return (string)null;
            if (p.fileName != null)
                return p.fileName;
            string name = p.name;
            if (p.steamID != 0UL)
            {
                if (Steam.user == null || (long)p.steamID != (long)DG.localID)
                    return (string)null;
                name = p.steamID.ToString();
            }
            return DuckFile.profileDirectory + DuckFile.ReplaceInvalidCharacters(name) + ".pro";
        }

        public void Save(Profile p, string pPrepend = null)
        {
            if (NetworkDebugger.enabled || p == this.EnvironmentProfile)
                return;
            if (p.linkedProfile != null)
            {
                if (MonoMain.logFileOperations)
                    DevConsole.Log(DCSection.General, "Profile.Save() saving linkedProfile");
                this.Save(p.linkedProfile, pPrepend);
            }
            else
            {
                if (p.isNetworkProfile)
                    return;
                if (MonoMain.logFileOperations)
                    DevConsole.Log(DCSection.General, "Profile.Save(" + p.name + ")");
                DuckXML doc = new DuckXML();
                DXMLNode node1 = new DXMLNode("Profile");
                DXMLNode node2 = new DXMLNode("Name", (object)p.formattedName);
                node1.Add(node2);
                DXMLNode node3 = new DXMLNode("ID", (object)p.id);
                node1.Add(node3);
                DXMLNode node4 = new DXMLNode("Mood", (object)p.funslider);
                node1.Add(node4);
                DXMLNode node5 = new DXMLNode("PreferredColor", (object)p.preferredColor);
                node1.Add(node5);
                DXMLNode node6 = new DXMLNode("NS", (object)p.numSandwiches);
                node1.Add(node6);
                DXMLNode node7 = new DXMLNode("MF", (object)p.milkFill);
                node1.Add(node7);
                DXMLNode node8 = new DXMLNode("LML", (object)p.littleManLevel);
                node1.Add(node8);
                DXMLNode node9 = new DXMLNode("NLM", (object)p.numLittleMen);
                node1.Add(node9);
                DXMLNode node10 = new DXMLNode("RSXP", (object)p.roundsSinceXP);
                node1.Add(node10);
                DXMLNode node11 = new DXMLNode("LMB", (object)p.littleManBucks);
                node1.Add(node11);
                DXMLNode node12 = new DXMLNode("TimesMet", (object)p.timesMetVincent);
                node1.Add(node12);
                DXMLNode node13 = new DXMLNode("TimesMet2", (object)p.timesMetVincentSale);
                node1.Add(node13);
                DXMLNode node14 = new DXMLNode("TimesMet3", (object)p.timesMetVincentSell);
                node1.Add(node14);
                DXMLNode node15 = new DXMLNode("TimesMet4", (object)p.timesMetVincentImport);
                node1.Add(node15);
                DXMLNode node16 = new DXMLNode("TimesMet5", (object)p.timesMetVincentHint);
                node1.Add(node16);
                DXMLNode node17 = new DXMLNode("TimeOfDay", (object)p.timeOfDay);
                node1.Add(node17);
                DXMLNode node18 = new DXMLNode("CD", (object)p.currentDay);
                node1.Add(node18);
                DXMLNode node19 = new DXMLNode("Punished", (object)p.punished);
                node1.Add(node19);
                if (MonoMain.logFileOperations)
                    DevConsole.Log(DCSection.General, "Profile(" + p.name + ").xp = " + p.xp.ToString());
                DXMLNode node20 = new DXMLNode("XtraPoints", (object)p.xp);
                node1.Add(node20);
                DXMLNode node21 = new DXMLNode("FurniPositions", (object)p.furniturePositionData.ToString());
                node1.Add(node21);
                DXMLNode node22 = new DXMLNode("Fowner", (object)p.furnitureOwnershipData.ToString());
                node1.Add(node22);
                DXMLNode node23 = new DXMLNode("SteamID", (object)p.steamID);
                node1.Add(node23);
                if (p.steamID != 0UL && Steam.user != null && (long)p.steamID == (long)Steam.user.id)
                {
                    DXMLNode node24 = new DXMLNode("LastKnownName", (object)Steam.user.name);
                    node1.Add(node24);
                }
                node1.Add(p.stats.Serialize());
                string varValue = "";
                foreach (string unlock in p.unlocks)
                    varValue = varValue + unlock + "|";
                if (varValue.Length > 0)
                    varValue = varValue.Substring(0, varValue.Length - 1);
                DXMLNode node25 = new DXMLNode("Unlocks", (object)varValue);
                node1.Add(node25);
                if (MonoMain.logFileOperations)
                    DevConsole.Log(DCSection.General, "Profile(" + p.name + ").ticketCount = " + p.ticketCount.ToString());
                DXMLNode node26 = new DXMLNode("Tickets", (object)p.ticketCount);
                node1.Add(node26);
                DXMLNode node27 = new DXMLNode("Mappings");
                foreach (DeviceInputMapping inputMappingOverride in p.inputMappingOverrides)
                    node27.Add(inputMappingOverride.Serialize());
                node1.Add(node27);
                if (MonoMain.logFileOperations)
                    DevConsole.Log(DCSection.General, "Profile.SavingChallengeData(" + p.name + ")");
                BitBuffer bitBuffer = new BitBuffer(false);
                foreach (KeyValuePair<string, ChallengeSaveData> keyValuePair in p.challengeData)
                {
                    try
                    {
                        BitBuffer buffer = keyValuePair.Value.ToBuffer();
                        bitBuffer.Write(buffer, true);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                if (bitBuffer.position > 0)
                {
                    if (MonoMain.logFileOperations)
                        DevConsole.Log(DCSection.General, "Profile.SavingChallengeData(" + p.name + ") (found data to save)");
                    DXMLNode node28 = new DXMLNode("ChallengeData", (object)Editor.BytesToString(bitBuffer.data));
                    node1.Add(node28);
                }
                doc.Add(node1);
                p.fileName = DuckFile.profileDirectory + DuckFile.ReplaceInvalidCharacters(p.formattedName) + ".pro";
                if (pPrepend != null)
                    DuckFile.SaveDuckXML(doc, DuckFile.profileDirectory + pPrepend + DuckFile.ReplaceInvalidCharacters(p.formattedName) + ".pro");
                else
                    DuckFile.SaveDuckXML(doc, p.fileName);
            }
        }
    }
}