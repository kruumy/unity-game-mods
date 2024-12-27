using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfiniteGoldenKey
{
	public class InfiniteGoldenKey : Mod
	{
        public override void Load()
        {
            ItemID.Sets.ForceConsumption[ ItemID.GoldenKey ] = false;
        }
        public override void Unload()
        {
            ItemID.Sets.ForceConsumption[ ItemID.GoldenKey ] = true;
        }
    }
    public class InfiniteGoldenKeyGlobalItem : GlobalItem
    {
        public override bool ConsumeItem( Item item, Player player )
        {
            if ( item.type == ItemID.GoldenKey )
            {
                return false;
            }
            return base.ConsumeItem(item, player);
        }
    }
}