using HarmonyLib;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(UltraWideFix.Main), "UltraWideFix", "1.0.0", "kruumy")]
[assembly: MelonGame("Playstack", "AK-xolotl")]

namespace UltraWideFix
{
    public class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            HarmonyInstance.PatchAll(typeof(UltraWideFix.Patches));
        }

        public override void OnSceneWasLoaded( int buildIndex, string sceneName )
        {
            foreach ( var canvas in UnityEngine.Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None) )
            {
                if ( canvas.name == "ScreenBorder" )
                {
                    canvas.enabled = false;
                }
            }
        }
    }
}
