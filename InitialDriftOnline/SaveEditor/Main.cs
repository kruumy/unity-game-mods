using MelonLoader;
using UnityEngine;

namespace SaveEditor
{
    public class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("SaveEditor Loaded!");
            MelonLogger.Msg("Press ` to open the menu.");
        }
        public override void OnLateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                if (!Menu.Menu.IsOpen)
                {
                    Menu.Menu.Open();
                    MelonLogger.Msg("Menu Opened");
                }
                else
                {
                    Menu.Menu.Close();
                    MelonLogger.Msg("Menu Closed");
                }
            }
        }
    }
}