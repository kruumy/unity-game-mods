using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagicStorageResearch
{
	public class MagicStorageResearch : Mod
	{
        public override void Load()
        {
            MagicStorage.StorageGUI.OnRefresh += StorageGUI_OnRefresh;
        }
        public override void Unload()
        {
            MagicStorage.StorageGUI.OnRefresh -= StorageGUI_OnRefresh;
        }
        private static bool IsInJourneyMode()
        {
            return Main.netMode is not NetmodeID.Server && Main.LocalPlayer.difficulty is PlayerDifficultyID.Creative;
        }
        private void StorageGUI_OnRefresh()
        {
            if( IsInJourneyMode() )
            {
                MagicStorage.Components.TEStorageHeart storageHeart = MagicStorage.StoragePlayer.LocalPlayer.GetStorageHeart();
                if ( storageHeart != null )
                {
                    ResearchItems(storageHeart.GetStoredItems());
                }
            }
        }

        public static void ResearchItems(IEnumerable<Item> items)
        {
            foreach (var item in items)
            {
                ResearchItem(item);
            }
        }
        public static void ResearchItem( Item item )
        {
            if ( Main.LocalPlayer.creativeTracker.ItemSacrifices.TryGetSacrificeNumbers(item.type, out int amountWeHave, out int amountNeededTotal) )
            {
                int totalAmountWeHave = amountWeHave + item.stack;
                if ( amountWeHave < amountNeededTotal && totalAmountWeHave >= amountNeededTotal )
                {
                    CreativeUI.ResearchItem(item.type);
                    SoundEngine.PlaySound(SoundID.Research);
                    SoundEngine.PlaySound(SoundID.ResearchComplete);
                    Main.NewText($"Researched [i:{item.type}]", Colors.JourneyMode);
                }
            }
        }
    }
}
