using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using UnityEngine;

namespace BetterThirdPerson
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string PluginAuthor = "kruumy";
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginName = "BetterThirdPerson";
        public const string PluginVersion = "1.0.0";
        public static ManualLogSource logger;

        private ConfigEntry<Vector3> LocalPosition;
        private ConfigEntry<float> FieldOfView;

        public void Awake()
        {
            logger = Logger;
            LocalPosition = Config.Bind("Camera", nameof(LocalPosition), MainCameraController.LocalPosition.Default);
            FieldOfView = Config.Bind("Camera", nameof(FieldOfView), MainCameraController.FieldOfView.Default);
            MainCameraController.Init();
            MainCameraController.LocalPosition.Set(LocalPosition.Value);
            MainCameraController.FieldOfView.Value = FieldOfView.Value;
        }

        public void OnDestroy()
        {
            LocalPosition.Value = MainCameraController.LocalPosition.Get();
            FieldOfView.Value = MainCameraController.FieldOfView.Value;
        }
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MainCameraController.LocalPosition.Y += 0.25f;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MainCameraController.LocalPosition.Y -= 0.25f;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MainCameraController.LocalPosition.X -= 0.25f;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MainCameraController.LocalPosition.X += 0.25f;
            }
            else if (Input.GetKeyDown(KeyCode.RightShift))
            {
                MainCameraController.LocalPosition.Z += 0.25f;
            }
            else if (Input.GetKeyDown(KeyCode.RightControl))
            {
                MainCameraController.LocalPosition.Z -= 0.25f;
            }
            else if (Input.GetKeyDown(KeyCode.Equals))
            {
                MainCameraController.FieldOfView.Value += 5;
            }
            else if (Input.GetKeyDown(KeyCode.Minus))
            {
                MainCameraController.FieldOfView.Value -= 5;
            }
        }
    }
}
