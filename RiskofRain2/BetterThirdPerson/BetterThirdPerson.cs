using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using UnityEngine;

namespace BetterThirdPerson
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class BetterThirdPerson : BaseUnityPlugin
    {
        public const string PluginAuthor = "kruumy";
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginName = nameof(BetterThirdPerson);
        public const string PluginVersion = "1.0.0";
        public static ManualLogSource logger;

        private ConfigEntry<Vector3> CameraLocalPosition;

        public void Awake()
        {
            logger = Logger;
            CameraLocalPosition = Config.Bind("Camera", nameof(CameraLocalPosition), new Vector3(0, 0, 0));
            MainCameraController.LocalPosition.Set(CameraLocalPosition.Value);
            MainCameraController.Init();

        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MainCameraController.LocalPosition.Y += 0.25f;
                CameraLocalPosition.Value = MainCameraController.LocalPosition.Get();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MainCameraController.LocalPosition.Y -= 0.25f;
                CameraLocalPosition.Value = MainCameraController.LocalPosition.Get();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MainCameraController.LocalPosition.X -= 0.25f;
                CameraLocalPosition.Value = MainCameraController.LocalPosition.Get();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MainCameraController.LocalPosition.X += 0.25f;
                CameraLocalPosition.Value = MainCameraController.LocalPosition.Get();
            }
            else if (Input.GetKeyDown(KeyCode.RightShift))
            {
                MainCameraController.LocalPosition.Z += 0.25f;
                CameraLocalPosition.Value = MainCameraController.LocalPosition.Get();
            }
            else if (Input.GetKeyDown(KeyCode.RightControl))
            {
                MainCameraController.LocalPosition.Z -= 0.25f;
                CameraLocalPosition.Value = MainCameraController.LocalPosition.Get();
            }
        }
    }
}
