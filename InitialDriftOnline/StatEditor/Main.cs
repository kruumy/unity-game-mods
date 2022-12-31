using MelonLoader;
using UnityEngine;

namespace StatEditor
{
    public class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("StatEditor Loaded!");
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
