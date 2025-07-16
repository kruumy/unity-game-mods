using BepInEx;
using HarmonyLib;
using System.Linq;
using UnityEngine.UI;

namespace BetterSensitivitySlider
{
    [BepInPlugin("BetterSensitivitySlider.kruumy", "BetterSensitivitySlider", "1.0.0")]
    public class Main : BepInEx.BaseUnityPlugin
    {
        private Harmony _harmony;
        public void Awake()
        {
            _harmony = new Harmony("BetterSensitivitySlider.kruumy");
            _harmony.PatchAll();
        }
    }
}
