using BepInEx;
using HarmonyLib;
using System.Linq;
using System.Reflection;

namespace ProperSave.StopDeletingMySave
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency("com.KingEnderBrine.ProperSave")]
    public class Main : BaseUnityPlugin
    {
        public const string PluginAuthor = "kruumy";
        public const string PluginName = "StopDeletingMySave";
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginVersion = "1.0.0";

        public void Awake()
        {
            MethodInfo RunOnServerGameOver = Assembly
                .Load("ProperSave")
                .GetTypes()
                .First(t => t.Name == "Saving")
                .GetMethod("RunOnServerGameOver", BindingFlags.Static | BindingFlags.NonPublic);
            Logger.LogInfo("Found Target " + RunOnServerGameOver.Name);
            Harmony harmony = new Harmony(PluginGUID);
            HarmonyMethod harmonyMethod = new HarmonyMethod(typeof(Main), "RunOnServerGameOver_Prefix");
            harmonyMethod.method = typeof(Main).GetMethod("RunOnServerGameOver_Prefix", BindingFlags.Static | BindingFlags.NonPublic);
            Logger.LogInfo("Created Prefix " + harmonyMethod.method.Name);
            harmony.Patch(RunOnServerGameOver, harmonyMethod);
            Logger.LogInfo("Patched ProperSave.Saving.RunOnServerGameOver Successfully!");
        }

        private static bool RunOnServerGameOver_Prefix()
        {
            return false;
        }
    }
}
