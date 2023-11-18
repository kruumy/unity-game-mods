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

            Setting[] settings =
            [
                new ColorGrading(),
                new Fog(),
                new MotionBlur(),
                new Outline(),
                new Sun(),
                new RenderResolution(),
                new RenderDistance()
            ];

            Log.LogInfo(nameof(Awake) + " done.");
        }
    }
}
