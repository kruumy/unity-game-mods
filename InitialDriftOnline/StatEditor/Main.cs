using MelonLoader;
using System.Threading.Tasks;
using UnityEngine;

namespace StatEditor
{
    public class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            GUI.Initialize();
            RCC_CarControllerV3.OnRCCPlayerSpawned += OnRCCPlayerSpawned;
            MelonLogger.Msg("StatEditor Loaded!");
            MelonLogger.Msg("Press F3 to open the menu.");
        }

        private async void OnRCCPlayerSpawned(RCC_CarControllerV3 Car)
        {
            if (Car == RCC_SceneManager.Instance.activePlayerVehicle)
            {
                await Task.Delay(5000); // this is here to prevent the game from writing to the vehicles values after ours
                Preferences.Load();
            }
        }

        public override void OnLateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F3))
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
