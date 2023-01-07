using MelonLoader;
using UnityEngine;

namespace TeleportMenu
{
    public class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            GUI.Initialize();
            MelonLogger.Msg("TeleportMenu Loaded!");
            MelonLogger.Msg("Press F6 to open the menu.");
        }

        public override void OnLateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F6))
            {
                if (!GUI.Root.IsOpen)
                {
                    GUI.Root.Open();
                    MelonLogger.Msg("Menu Opened");
                }
                else
                {
                    GUI.Root.Close();
                    MelonLogger.Msg("Menu Closed");
                }
            }
        }
    }
}
