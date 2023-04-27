﻿using System.Diagnostics;

namespace DuckGame
{

    public static partial class DevConsoleCommands
    {
        [DevConsoleCommand(Description = "Opens the AdvancedConfig folder",
            Aliases = new[] {"advconf"})]
        public static void AdvancedConfig()
        {
            Process.Start(DuckGame.AdvancedConfig.SaveDirPath);
            DevConsole.Log("Opened AdvancedConfig folder");
        }
    }
}
