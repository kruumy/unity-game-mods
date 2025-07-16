using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine.UI;

namespace BetterSensitivitySlider
{
    public static class Patches
    {

        internal static void ApplySensitivitySlider( SettingsManager settingsManager )
        {
            var sliderField = typeof(SettingsManager).GetField("_cameraSensitivitySlider", BindingFlags.NonPublic | BindingFlags.Instance);
            var slider = sliderField.GetValue(settingsManager) as UnityEngine.UI.Slider;
            slider.minValue = 0f;
        }

        [HarmonyPatch(typeof(SettingsManager), "Handle_InputParameters")]
        public static class SettingsManager_Handle_InputParameters_Patch
        {
            public static void Prefix( SettingsManager __instance )
            {
                ApplySensitivitySlider(__instance);
            }
        }

        [HarmonyPatch(typeof(SettingsManager), "Load_SettingsData")]
        public static class SettingsManager_Load_SettingsData_Patch
        {
            public static void Prefix( SettingsManager __instance )
            {
                ApplySensitivitySlider(__instance);
            }
        }
    }
}
