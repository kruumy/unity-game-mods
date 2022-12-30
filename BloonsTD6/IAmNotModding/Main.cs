using HarmonyLib;
using Il2CppAssets.Main.Scenes;
using Il2CppAssets.Scripts.Unity;
using MelonLoader;

[assembly: MelonInfo(typeof(IAmNotModding.Main), "I Am Not Modding", "1.0.0", "kruumy")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace IAmNotModding
{
    public class Main : MelonMod
    {
        public override void OnApplicationStart()
        {
            MelonLogger.Msg("I Am Not Modding Loaded!");
        }

        [HarmonyPatch(typeof(InitialLoadingScreen), "Update")]
        public class InitialLoadingScreenPatch
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                Modding.isModdedClient = false;
            }
        }
        [HarmonyPatch(typeof(Modding), "CheckForMods")]
        public class ModdingPatch
        {
            [HarmonyPostfix]
            public static bool Prefix(ref bool __result)
            {
                __result = false;
                return false;
            }
        }
        [HarmonyPatch(typeof(Modding), "IsModdingLibrary")]
        public class ModdingPatch2
        {
            [HarmonyPostfix]
            public static bool Prefix(ref bool __result)
            {
                __result = false;
                return false;
            }
        }
    }
}