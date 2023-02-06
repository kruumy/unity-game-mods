using BepInEx;
using HarmonyLib;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ProperSave.CommandQueue
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency("com.KingEnderBrine.ProperSave")]
    [BepInDependency("com.kuberoot.commandqueue")]
    public class Main : BaseUnityPlugin
    {
        public const string PluginAuthor = "kruumy";
        public const string PluginName = "ProperSave.CommandQueue";
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginVersion = "1.0.0";
        public readonly static string LastCommandQueuePath = Path.Combine(Application.persistentDataPath, "ProperSave", "Saves") + "\\" + "LastCommandQueue" + ".csv";

        private void Awake()
        {
            Harmony harmony = new Harmony(PluginGUID);
            MethodInfo SaveGame = Assembly
               .Load("ProperSave")
               .GetTypes()
               .First(t => t.Name == "Saving")
               .GetMethod("SaveGame", BindingFlags.Static | BindingFlags.NonPublic);
            HarmonyMethod saveGame_Postfix = new HarmonyMethod(typeof(Main), "Saving_OnSavingEnded");
            saveGame_Postfix.method = typeof(Main).GetMethod("Saving_OnSavingEnded", BindingFlags.Static | BindingFlags.NonPublic);
            harmony.Patch(SaveGame, saveGame_Postfix);
            ProperSave.Loading.OnLoadingEnded += Loading_OnLoadingEnded;
        }

        private static void Loading_OnLoadingEnded(SaveFile _)
        {
            if (File.Exists(LastCommandQueuePath))
            {
                SaveAndLoad.Load(LastCommandQueuePath);
            }
        }

        private static void Saving_OnSavingEnded()
        {
            SaveAndLoad.Save(LastCommandQueuePath);
        }
    }
}
