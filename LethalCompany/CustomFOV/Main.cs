using UnityEngine;

namespace CustomFOV
{
    public class Main : MelonLoader.MelonMod
    {
        public override void OnSceneWasLoaded( int buildIndex, string sceneName )
        {
            foreach ( Object cameraobj in UnityEngine.Object.FindObjectsOfTypeAll(typeof(UnityEngine.Camera)) )
            {
                if ( cameraobj is UnityEngine.Camera camera && camera.name == "MainCamera" )
                {
                    Transform HUDHelmetPosition = camera.gameObject.transform.GetChild(0);
                    HUDHelmetPosition.localPosition = new Vector3(0.01f, -0.048f, -0.119f);
                }
            }
        }
        [HarmonyLib.HarmonyPatch(typeof(UnityEngine.Camera), nameof(UnityEngine.Camera.fieldOfView), HarmonyLib.MethodType.Setter)]
        private class CamerafieldOfViewPatch
        {
            [HarmonyLib.HarmonyPrefix]
            private static void Prefix( ref float value )
            {
                value = 90;
            }
        }
    }
}
