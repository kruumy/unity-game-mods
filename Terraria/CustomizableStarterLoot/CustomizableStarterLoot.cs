using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace CustomizableStarterLoot
{
    public class CustomizableStarterLoot : ModPlayer
    {
        public override IEnumerable<Item> AddStartingItems( bool mediumCoreDeath )
        {
            if ( !mediumCoreDeath )
            {
                if ( Config.Instance.PrefixedItems != null && Config.Instance.StackedItems != null )
                {
                    return Config.Instance.PrefixedItems.Select(item => new Item(item.Key.Type, 1, item.Value.Type))
                        .Concat(Config.Instance.StackedItems.Select(item => new Item(item.Key.Type, (int)item.Value, 0)));
                }
                if ( Config.Instance.PrefixedItems != null )
                {
                    return Config.Instance.PrefixedItems.Select(item => new Item(item.Key.Type, 1, item.Value.Type));
                }
                if ( Config.Instance.StackedItems != null )
                {
                    return Config.Instance.StackedItems.Select(item => new Item(item.Key.Type, (int)item.Value, 0));
                }
            }
            return Enumerable.Empty<Item>();
        }
    }
}