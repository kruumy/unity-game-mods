using BepInEx;
using System.IO;
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
        public readonly string LastCommandQueuePath = Path.Combine(Application.persistentDataPath, "ProperSave", "Saves") + "\\" + "LastCommandQueue" + ".csv";

        private void Awake()
        {
            ProperSave.SaveFile.OnGatherSaveData += SaveFile_OnGatherSaveData;
            ProperSave.Loading.OnLoadingEnded += Loading_OnLoadingEnded;
        }

        private void SaveFile_OnGatherSaveData(System.Collections.Generic.Dictionary<string, object> obj)
        {
            SaveAndLoad.Save(LastCommandQueuePath);
        }

        private void Loading_OnLoadingEnded(SaveFile _)
        {
            if (File.Exists(LastCommandQueuePath))
            {
                SaveAndLoad.Load(LastCommandQueuePath);
            }
            else
            {
                Logger.LogInfo("CommandQueue save file does not exist!");
            }
        }
    }
}
