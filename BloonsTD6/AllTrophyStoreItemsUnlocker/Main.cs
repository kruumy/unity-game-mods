using HarmonyLib;
using Il2CppAssets.Scripts.Data;
using Il2CppAssets.Scripts.Data.TrophyStore;
using Il2CppAssets.Scripts.Models.Profile;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Player;
using Il2CppAssets.Scripts.Unity.UI_New.Main;
using Il2CppSystem.IO;
using MelonLoader;

[assembly: MelonInfo(typeof(AllTrophyStoreItemsUnlocker.Main), "All Trophy Store Items Unlocker", "4.0.1", "kruumy & kenx00x")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace AllTrophyStoreItemsUnlocker
{
    public class Main : MelonMod
    {
        public override void OnApplicationStart()
        {
            MelonLogger.Msg("All Trophy Store Items Unlocker Loaded!");
        }

        [HarmonyPatch(typeof(MainMenu), "Open")]
        public class MainMenuOpen
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                Btd6Player Player = Game.instance.playerService.Player;
                Il2CppSystem.Collections.Generic.Dictionary<string, TrophyStoreSD> purchasedItems = Player.Data.trophyStorePurchasedItems;
                Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppArrayBase<TrophyStoreItem> allStoreItems = GameData.Instance.trophyStoreItems.StoreItems.ToArray();

                bool didUnlockAnything = false;
                foreach (TrophyStoreItem? storeitem in allStoreItems)
                {
                    if (!purchasedItems.ContainsKey(storeitem.id))
                    {
                        didUnlockAnything = true;
                        Game.Player.AddTrophyStoreItem(storeitem.id);
                        MelonLogger.Msg("Unlocked " + storeitem.id);
                    }
                }
                if (!didUnlockAnything)
                {
                    MelonLogger.Msg("Nothing To Unlock :(");
                }
            }
        }
    }
}