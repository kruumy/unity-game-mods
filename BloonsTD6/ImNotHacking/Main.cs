using HarmonyLib;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Player;
using Il2CppAssets.Scripts.Unity.UI_New.Main;
using MelonLoader;

[assembly: MelonInfo(typeof(ImNotHacking.Main), "ImNotHacking", "1.0.0", "kruumy")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace ImNotHacking
{
    public class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("ImNotHacking Loaded!");




        }

        // fix ambiguious
        [HarmonyPatch(typeof(Btd6Player), "CheckHakrStatus")]
        public static class CheckHakrStatusPatch
        {
            [HarmonyPostfix]
            public static void Postfix(ref Il2CppSystem.Threading.Tasks.Task<Btd6Player.HakrStatus> __result)
            {
                Btd6Player.HakrStatus res = __result.Result;
                res.ledrbrd = false;
                res.genrl = false;
                __result = new Il2CppSystem.Threading.Tasks.Task<Btd6Player.HakrStatus>(res);
                MelonLogger.Msg("Modified CheckHakrStatus result");
            }
        }


        [HarmonyPatch(typeof(MainMenu), "Open")]
        public static class MainMenuOpenPatch
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                Il2CppAssets.Scripts.Unity.Player.Btd6Player.HakrStatus phax = Game.Player.Hakxr;
                phax.ledrbrd = false;
                phax.genrl = false;
                Game.Player.Hakxr = phax;
                Game.Player._Hakxr_k__BackingField = phax;
                MelonLogger.Msg("Set player hack status to false");
            }
        }
    }
}