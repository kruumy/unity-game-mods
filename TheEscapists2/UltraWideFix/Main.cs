using BepInEx;
using HarmonyLib;
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UltraWideFix
{
    [BepInPlugin("com.github.kruumy.UltrawideFix", "UltraWideFix", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        public static int Width { get; set; } = 2560;
        public static int Height { get; set; } = 1080;
        public void Awake()
        {
            Harmony harmony = new Harmony("com.github.kruumy.UltrawideFix");
            harmony.PatchAll();
        }
        public void LateUpdate()
        {
            if ( Input.GetKeyDown(KeyCode.F11) )
            {
                QualityManager.SetResolution(1920, 1080, true);
                RenderTargetManager.CheckForLostRTs();
                CameraManager.GetInstance()?.OnScreenResolutionChanged();
                StartCoroutine(SetUltraWide());
            }
        }
        public IEnumerator SetUltraWide()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            QualityManager.SetResolution(Width, Height, true);
            RenderTargetManager.CheckForLostRTs();
        }
    }

    [HarmonyPatch(typeof(Camera), "aspect", MethodType.Getter)]
    public static class CameraAspectPatch
    {
        public static void Postfix( Camera __instance )
        {
            __instance.aspect = (float)Main.Width / (float)Main.Height;
        }
    }
}
