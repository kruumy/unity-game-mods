using MelonLoader;
using System.Threading.Tasks;
using UnityEngine;

namespace GraphicsEditor
{
    public class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            GUI.Initialize();
            RCC_CarControllerV3.OnRCCPlayerSpawned += OnRCCPlayerSpawned;
            MelonLogger.Msg("GraphicsEditor Loaded!");
            MelonLogger.Msg("Press F4 to open the menu.");
        }

        public override void OnLateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F4))
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

        private async void OnRCCPlayerSpawned(RCC_CarControllerV3 car)
        {
            if (car == RCC_SceneManager.Instance.activePlayerVehicle)
            {
                await Task.Delay(1000);
                GraphicsWrapper.Enable();
                Preferences.Load();
            }
        }
    }
}
