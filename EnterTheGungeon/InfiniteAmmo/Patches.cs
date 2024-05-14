using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteAmmo
{
    public class Patches
    {
        [HarmonyLib.HarmonyPatch(typeof(Gun), nameof(Gun.InfiniteAmmo), MethodType.Getter)]
        [HarmonyLib.HarmonyPostfix]
        public static void GunInfiniteAmmoPostFix( ref Gun __instance, ref bool __result)
        {
            __result = true;
            __instance.LocalInfiniteAmmo = true;
        }

        [HarmonyLib.HarmonyPatch(typeof(Gun), nameof(Gun.InfiniteAmmo), MethodType.Setter)]
        [HarmonyLib.HarmonyPrefix]
        public static void GunInfiniteAmmoPreFix( ref Gun __instance, ref bool value )
        {
            value = true;
            __instance.LocalInfiniteAmmo = true;
        }
    }
}
