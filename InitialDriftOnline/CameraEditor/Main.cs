using MelonLoader;
using System.Threading.Tasks;
using UnityEngine;
using static RCC_CarControllerV3;

namespace CameraEditor
{
    public class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            GUI.Initialize();
            RCC_CarControllerV3.OnRCCPlayerSpawned += OnRCCPlayerSpawned;
            MelonLogger.Msg("CameraEditor Loaded!");
            MelonLogger.Msg("Press F5 to open the menu.");
        }
        private void OnRCCPlayerSpawned(RCC_CarControllerV3 Car)
        {
            if (Car == RCC_SceneManager.Instance.activePlayerVehicle)
            {
                //await Task.Delay(1000);
                Preferences.Load();
            }
        }
        public override void OnLateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F5))
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
