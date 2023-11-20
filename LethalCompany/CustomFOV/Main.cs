using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CustomFOV
{
    [BepInPlugin("kruumy.CustomFOV", "Custom FOV", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        public static ConfigEntry<float> FieldOfView;
        public static ConfigEntry<float> HelmetZPositionOffset;
        private void Awake()
        {
            FieldOfView = Config.Bind<float>("General", "Field Of View", 90);
            HelmetZPositionOffset = Config.Bind<float>("General", "Helmet Z Position Offset", -0.119f, "The amount to move the helmet model on your head. Higher FOVs would look weird without moving the model back a bit.");
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Main));
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }

        private void SceneManager_activeSceneChanged( Scene arg0, Scene arg1 )
        {
            foreach ( Object cameraobj in UnityEngine.Object.FindObjectsOfTypeAll(typeof(UnityEngine.Camera)) )
            {
                if ( cameraobj is UnityEngine.Camera camera && camera.name == "MainCamera" )
                {
                    Transform HUDHelmetPosition = camera.gameObject.transform.GetChild(0);
                    HUDHelmetPosition.localPosition = new Vector3(HUDHelmetPosition.localPosition.x, HUDHelmetPosition.localPosition.y, HelmetZPositionOffset.Value);
                }
            }
        }

        [HarmonyLib.HarmonyPatch(typeof(UnityEngine.Camera), nameof(UnityEngine.Camera.fieldOfView), HarmonyLib.MethodType.Setter)]
        [HarmonyLib.HarmonyPrefix]
        private static void FOVPrefix( UnityEngine.Camera __instance, ref float value )
        {
            if ( __instance.name == "MainCamera" )
            {
                value = FieldOfView == null ? 90 : FieldOfView.Value;
            }

        }
    }
}
