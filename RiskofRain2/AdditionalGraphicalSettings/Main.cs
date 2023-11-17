using BepInEx;

namespace AdditionalGraphicalSettings
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "kruumy";
        public const string PluginName = "AdditionalGraphicalSettings";
        public const string PluginVersion = "1.0.0";




        public void Awake()
        {
            Log.Init(Logger);
            new Settings.ColorGrading();
            new Settings.Fog();
            new Settings.MotionBlur();
            new Settings.Vignette();
            Log.LogInfo(nameof(Awake) + " done.");
        }
    }
}
