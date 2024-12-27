using System.Collections.Generic;
using Terraria.ModLoader.Config;

namespace CustomizableStarterLoot
{
    public class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        public static Config Instance;
        public Dictionary<ItemDefinition, PrefixDefinition> PrefixedItems;
        public Dictionary<ItemDefinition, uint> StackedItems;
        public override void OnLoaded()
        {
            Instance = this;
        }
    }
}
