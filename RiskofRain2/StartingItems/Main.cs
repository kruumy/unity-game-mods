using BepInEx;
using BepInEx.Configuration;

namespace StartingItems
{
    [BepInPlugin("kruumy.StartingItems", "StartingItems", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        public ConfigEntry<string> StartingItems;
        public void Awake()
        {
            StartingItems = Config.Bind<string>(
                "General",
                nameof(StartingItems),
                "JumpBoost,Hoof",
                "A csv (comma seperated values) of all the items to give to a player on spawn. The diplay name might not always equal the codename of the item. For example: Wax Quail = JumpBoost. To find the name out for yourself, open the console (ctrl + alt + grave (`)) and type in \"item_list\". To give muliple of an item simply just repeat it. (Hoof,Hoof,Hoof)");
            RoR2.PlayerCharacterMasterController.onPlayerAdded += PlayerCharacterMasterController_onPlayerAdded;
        }

        private void PlayerCharacterMasterController_onPlayerAdded( RoR2.PlayerCharacterMasterController player )
        {
            player.master.onBodyStart += Master_onBodyStart;
            void Master_onBodyStart( RoR2.CharacterBody body )
            {
                foreach ( var item in StartingItems.Value.Split(',') )
                {
                    body.inventory.GiveItemString(item.Trim());
                }
                player.master.onBodyStart -= Master_onBodyStart;
            }
        }

    }
}
