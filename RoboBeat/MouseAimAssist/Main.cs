using BepInEx;
using BepInEx.Harmony;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseAimAssist
{
    [BepInPlugin("kruumy.MouseAimAssist", "MouseAimAssist", "1.0.0")]
    public class Main : BepInEx.BaseUnityPlugin
    {
        public void Awake()
        {
            Harmony harmony = new Harmony("kruumy.MouseAimAssist");
            harmony.PatchAll(typeof(Patches));
            
        }
    }
}
