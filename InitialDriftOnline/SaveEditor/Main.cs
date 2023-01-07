using MelonLoader;
using UnityEngine;

namespace SaveEditor
{
    public class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            GUI.Initialize();
            MelonLogger.Msg("SaveEditor Loaded!");
            MelonLogger.Msg("Press F2 to open the menu.");
        }

        public override void OnLateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F2))
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
