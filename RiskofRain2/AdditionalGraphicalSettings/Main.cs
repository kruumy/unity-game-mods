using AdditionalGraphicalSettings.Settings;
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

            if ( Config.Bind<bool>("General", "Enable ColorGrading Settings", true, string.Empty).Value )
            {
                new ColorGrading();
            }
            if ( Config.Bind<bool>("General", "Enable RampFog Settings", true, string.Empty).Value )
            {
                new Fog();
            }
            if ( Config.Bind<bool>("General", "Enable MotionBlur Settings", true, string.Empty).Value )
            {
                new MotionBlur();
            }
            if ( Config.Bind<bool>("General", "Enable SobelOutline Settings", true, string.Empty).Value )
            {
                new Outline();
            }
            if ( Config.Bind<bool>("General", "Enable Sun Settings", true, string.Empty).Value )
            {
                new Sun();
            }
            if ( Config.Bind<bool>("General", "Enable RenderSettings Sliders (Render distance and Render Resolution)", true, string.Empty).Value )
            {
                new RenderSettings();
            }

            Log.LogInfo(nameof(Awake) + " done.");
        }
    }
}
