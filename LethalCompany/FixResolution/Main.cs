using GameNetcodeStuff;
using HarmonyLib;
using MelonLoader;
using System.Collections.Generic;
using UnityEngine;

namespace FixResolution
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
            foreach ( UnityEngine.Object cameraobj in UnityEngine.Object.FindObjectsOfTypeAll(typeof(UnityEngine.Camera)) )
            {
                if ( cameraobj is UnityEngine.Camera camera && camera.name == "MainCamera" )
                {
                    camera.targetTexture = camera.targetTexture;
                }
            }
        }

        [HarmonyLib.HarmonyPatch(typeof(Camera), nameof(Camera.targetTexture), HarmonyLib.MethodType.Setter)]
        class CameraTexturePatch
        {
            static void Prefix( UnityEngine.Camera __instance, ref UnityEngine.RenderTexture __0 )
            {
                if ( __0 != null && (__0.width != Screen.width || __0.height != Screen.height) )
                {
                    __instance.targetTexture = null;
                    __0.Release();
                    __0.width = Screen.width;
                    __0.height = Screen.height;
                    __0.Create();
                    Logger.Msg($"Set {__0.name} to {Screen.width}x{Screen.height}");
                }
            }
        }


        [HarmonyPatch(typeof(HUDManager), "UpdateScanNodes", new System.Type[] { typeof(PlayerControllerB) })]
        class HUDManagerScanNodesPatch
        {
            static void Postfix( HUDManager __instance, PlayerControllerB playerScript )
            {
                var scanNodesField = typeof(HUDManager).GetField("scanNodes", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var scanNodes = (Dictionary<RectTransform, ScanNodeProperties>)scanNodesField.GetValue(__instance);

                foreach ( RectTransform scan in __instance.scanElements )
                {
                    if ( scanNodes.TryGetValue(scan, out ScanNodeProperties scanNodeProperties) )
                    {
                        Vector3 vector = playerScript.gameplayCamera.WorldToScreenPoint(scanNodeProperties.transform.position);
                        scan.anchoredPosition = new Vector2((vector.x - (Screen.width / 2)) / 2, (vector.y - (Screen.height / 2)) / 2);
                    }
                }
            }
        }
    }
}