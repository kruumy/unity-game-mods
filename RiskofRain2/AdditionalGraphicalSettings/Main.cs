using BepInEx;

namespace AddFoVSettings
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Kruumy";
        public const string PluginName = "AdditionalGraphicalSettings";
        public const string PluginVersion = "1.0.0";

        public static AdditionalGraphicalSettings.Settings.ColorGrading ColorGrading { get; } = new AdditionalGraphicalSettings.Settings.ColorGrading();
        public static AdditionalGraphicalSettings.Settings.MotionBlur MotionBlur { get; } = new AdditionalGraphicalSettings.Settings.MotionBlur();
        public static AdditionalGraphicalSettings.Settings.Vignette Vignette { get; } = new AdditionalGraphicalSettings.Settings.Vignette();
        public void Awake()
        {
            Log.Init(Logger);
            Log.LogInfo(nameof(Awake) + " done.");
        }

    }
}
