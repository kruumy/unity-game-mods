using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UltrawideFix
{
    [BepInPlugin("com.kruumy.ultrawidefix", "UltrawideFix", "1.0.0")]
    public class MainPlugin : BaseUnityPlugin
    {
        public void Awake()
        {
            new Harmony("com.kruumy.ultrawidefix").PatchAll();
            Logger.LogInfo("UltrawideFix loaded");
        }

        [HarmonyPatch(typeof(CanvasScaler), "OnEnable", MethodType.Normal)]
        class CanvasScaler_OnEnable_Patch
        {
            public static void Postfix( CanvasScaler __instance )
            {
                __instance.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            }
        }
    }
}
