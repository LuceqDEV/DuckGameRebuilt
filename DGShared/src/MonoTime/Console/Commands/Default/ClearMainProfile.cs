﻿namespace DuckGame
{

    public static partial class DevConsoleCommands
    {
        [DevConsoleCommand(Description = "Wipes your main Duck Game profile of all data")]
        public static void ClearMainProfile()
        {
            DevConsole.Log("Your main account has been R U I N E D !", Color.Red);

            ulong steamId = Profiles.experienceProfile.steamID;
            string varName = steamId.ToString();

            steamId = Profiles.experienceProfile.steamID;
            string varID = steamId.ToString();

            Profile p = new(varName, varID: varID)
            {
                steamID = Profiles.experienceProfile.steamID
            };

            Profiles.Remove(Profiles.experienceProfile);
            Profiles.Add(p);
        }
    }
}