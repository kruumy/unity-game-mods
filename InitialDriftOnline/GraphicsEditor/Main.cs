using MelonLoader;
using System.Threading.Tasks;
using UnityEngine;

namespace GraphicsEditor
{
    public class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            RCC_CarControllerV3.OnRCCPlayerSpawned += OnRCCPlayerSpawned;
            MelonLogger.Msg("GraphicsEditor Loaded!");
            MelonLogger.Msg("Press F4 to open the menu.");
        }

        private async void OnRCCPlayerSpawned(RCC_CarControllerV3 car)
        {
            if (car == RCC_SceneManager.Instance.activePlayerVehicle)
            {
                await Task.Delay(1000);
                RCC_SceneManager.Instance.activeMainCamera.EnablePostProcessing();
                RCC_SceneManager.Instance.activeMainCamera.DisableAllPostProcessProfiles();
                Preferences.Load();
            }
        }

        public override void OnLateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F4))
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
