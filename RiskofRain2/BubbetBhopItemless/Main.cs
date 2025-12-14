using BepInEx;

namespace BubbetBhopItemless
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "kruumy";
        public const string PluginName = "Bubbet_Bhop_Itemless";
        public const string PluginVersion = "1.0.10";
        public void Awake()
        {
            Log.Init(Logger);

            BubbetBhop.Init(Config);
            Log.LogInfo(nameof(BubbetBhop) + " Initialized");
            AutoBhop.Init(Config);
            Log.LogInfo(nameof(AutoBhop) + " Initialized");

            Log.LogInfo(nameof(Awake) + " done.");
        }


    }
}
