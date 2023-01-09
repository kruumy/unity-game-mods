using HarmonyLib;
using Il2CppAssets.Scripts.Unity.Player;
using MelonLoader;

[assembly: MelonInfo(typeof(FreeShops.Main), "FreeShops", "1.0.0", "kruumy")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace FreeShops
{

    public class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("FreeShops Loaded!");
        }

        [HarmonyPatch(typeof(Btd6Player), "SpendMonkeyMoney")]
        public static class MainMenuOpen
        {
            [HarmonyPostfix]
            public static void Prefix(ref int amount, string spentOn)
            {
                amount = 0;
            }
        }
    }
}