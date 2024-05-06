using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlwaysSprint
{
    public class Patches
    {
        [HarmonyPatch(typeof(Player), "sprinting", MethodType.Getter)]
        [HarmonyPostfix]
        static void SprintPostfix( ref bool __result )
        {
            __result = true;
        }
    }
}
