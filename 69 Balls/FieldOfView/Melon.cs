using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace FieldOfView
{
    public class Melon : MelonMod
    {
        [HarmonyPatch(typeof(Camera), "fieldOfView", MethodType.Setter)]
        private static class Patch
        {
            private static void Prefix( ref float value )
            {
                value *= 1.25f;
            }
        }
    }
}
