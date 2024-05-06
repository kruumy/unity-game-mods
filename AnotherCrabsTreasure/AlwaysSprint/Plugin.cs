using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlwaysSprint
{
    [BepInPlugin("kruumy.AlwaysSprint", "AlwaysSprint", "1.0.0")]
    public class Plugin : BepInEx.BaseUnityPlugin
    {
        public void Awake()
        {
            var instance = new Harmony("kruumy.AlwaysSprint");
            instance.PatchAll(typeof(Patches));
        }
    }
}
