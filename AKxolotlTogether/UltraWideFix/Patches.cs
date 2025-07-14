using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UltraWideFix
{
    public class Patches
    {
        [HarmonyPatch(typeof(Camera))]
        internal static class CameraGetterPatches
        {
            [HarmonyPatch(nameof(Camera.pixelRect), MethodType.Getter)]
            [HarmonyPostfix]
            public static void PixelRectGetter( Camera __instance, ref Rect __result )
            {
                __result.x = 0;
                __result.y = 0;
                Resolution resolution = Screen.currentResolution;
                __result.width = resolution.width;
                __result.height = resolution.height;
                __instance.aspect = (float)resolution.width / resolution.height;
            }
        }
    }
}
