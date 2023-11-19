using MelonLoader;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace RemoveCameraFilter
{
    public class Main : MelonMod
    {
        private static MelonLogger.Instance Logger = null;
        public override void OnLateInitializeMelon()
        {
            Logger = LoggerInstance;
        }
        public override void OnSceneWasLoaded( int buildIndex, string sceneName )
        {
            foreach ( var volumeobj in UnityEngine.Object.FindObjectsOfTypeAll(typeof(UnityEngine.Rendering.HighDefinition.CustomPassVolume)) )
            {
                if ( volumeobj is CustomPassVolume volume )
                {
                    volume.enabled = false;
                    volume.isGlobal = false;
                    LoggerInstance.Msg("Disabled CustomPassVolume");
                }
            }

            foreach ( Object cameraobj in UnityEngine.Object.FindObjectsOfTypeAll(typeof(UnityEngine.Camera)) )
            {
                if ( cameraobj is UnityEngine.Camera camera && camera.name == "MainCamera" )
                {
                    camera.targetTexture = camera.targetTexture;
                }
            }
        }


        [HarmonyLib.HarmonyPatch(typeof(Camera), nameof(Camera.targetTexture), HarmonyLib.MethodType.Setter)]
        class CameraPatch
        {
            static void Prefix( UnityEngine.Camera __instance, ref UnityEngine.RenderTexture __0 )
            {
                if ( __0 != null && (__0.width != Screen.width || __0.height != Screen.height) )
                {
                    __instance.targetTexture = null;
                    __0.Release();
                    __0.width = 1920;
                    __0.height = 1080;
                    __0.Create();
                    Logger.Msg($"Set {__0.name} to {Screen.width}x{Screen.height}");
                }
            }
        }
    }
}
