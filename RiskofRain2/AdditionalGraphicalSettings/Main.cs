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

        public static Settings.ColorGrading ColorGrading { get; } = new Settings.ColorGrading();
        public static Settings.MotionBlur MotionBlur { get; } = new Settings.MotionBlur();
        public static Settings.Vignette Vignette { get; } = new Settings.Vignette();
        public static Settings.FogOverride Fog { get; } = new Settings.FogOverride();
        public static Settings.SunOverride Sun { get; } = new Settings.SunOverride();
        public void Awake()
        {
            Log.Init(Logger);
            Log.LogInfo(nameof(Awake) + " done.");
        }
    }
}
