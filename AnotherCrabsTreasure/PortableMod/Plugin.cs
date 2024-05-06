using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableMod
{
    [BepInPlugin("kruumy.PortableMod", "PortableMod", "1.0.0")]
    public class Plugin : BepInEx.BaseUnityPlugin
    {
        public void Awake()
        {
            var instance = new Harmony("kruumy.PortableMod");
            instance.PatchAll(typeof(Patches));
        }

        public void OnDestroy()
        {
            try
            {
                string AggroCrabLocalLow = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "LocalLow", "Aggro Crab");
                Logger.LogMessage(AggroCrabLocalLow);
                Directory.Delete(AggroCrabLocalLow, true);
            }
            catch{}
            try
            {
                string _1911AppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".1911");
                Logger.LogMessage(_1911AppData);
                Directory.Delete(_1911AppData, true);
            }
            catch{}
        }
    }
}
