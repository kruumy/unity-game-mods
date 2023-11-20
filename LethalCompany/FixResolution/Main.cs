using BepInEx;
using GameNetcodeStuff;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FixResolution
{
    [BepInPlugin("kruumy.FixResolution", "Fix Resolution", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        private void Awake()
        {
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Main));
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }

        private void SceneManager_activeSceneChanged( Scene arg0, Scene arg1 )
        {
            foreach ( UnityEngine.Object cameraobj in UnityEngine.Object.FindObjectsOfTypeAll(typeof(UnityEngine.Camera)) )
            {
                if ( cameraobj is UnityEngine.Camera camera && camera.name == "MainCamera" )
                {
                    camera.targetTexture = camera.targetTexture;
                }
            }
        }

        [HarmonyPatch(typeof(Camera), nameof(Camera.targetTexture), MethodType.Setter)]
        [HarmonyPrefix]
        private static void TargetTexturePrefix( UnityEngine.Camera __instance, ref UnityEngine.RenderTexture __0 )
        {
            if ( __0 != null && (__0.width != Screen.width || __0.height != Screen.height) )
            {
                __instance.targetTexture = null;
                __0.Release();
                __0.width = Screen.width;
                __0.height = Screen.height;
                __0.Create();
            }
        }

        [HarmonyPatch(typeof(HUDManager), "UpdateScanNodes", new System.Type[] { typeof(PlayerControllerB) })]
        [HarmonyPostfix]
        private static void UpdateScanNodesPostfix( HUDManager __instance, PlayerControllerB playerScript )
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