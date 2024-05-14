using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteAmmo
{
    [BepInPlugin("kruumy.InfiniteAmmo", "InfiniteAmmo", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        public void Awake()
        {
            Harmony harmony = new Harmony("kruumy.InfiniteAmmo");
            harmony.PatchAll(typeof(Patches));
        }
    }
}
