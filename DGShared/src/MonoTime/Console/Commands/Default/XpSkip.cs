﻿namespace DuckGame
{

    public static partial class DevConsoleCommands
    {
        [DevConsoleCommand(Description = "Gives you a Voodoo Vincent")]
        public static void XpSkip()
        {
            if (Profiles.experienceProfile.GetNumFurnitures(RoomEditor.GetFurniture("VOODOO VINCENT").index) > 0)
            {
                DevConsole.Log("Limit one Voodoo Vincent per customer, sorry!", Color.Red);
                return;
            }
            else if (MonoMain.pauseMenu != null)
            {
                MonoMain.pauseMenu.Close();
            }

            HUD.CloseAllCorners();
            (MonoMain.pauseMenu = new UIPresentBox(RoomEditor.GetFurniture("VOODOO VINCENT"),
                Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f)).Open();

            DevConsole.core.open ^= true;
        }
    }
}