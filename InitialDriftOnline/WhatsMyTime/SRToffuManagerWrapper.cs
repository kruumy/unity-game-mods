using HarmonyLib;
using System;
using System.Diagnostics;
using UnityEngine.SceneManagement;

namespace WhatsMyTime
{
    public static class SRToffuManagerWrapper
    {
        public static event EventHandler OnTofuRunCompleted;

        public static event EventHandler OnTofuRunInterupted;

        public static event EventHandler OnTofuRunStart;

        public static SRToffuManager SRToffuManager => UnityEngine.Object.FindObjectOfType<SRToffuManager>();

        public static Stopwatch Timer { get; } = new Stopwatch();
        public static void Initialize()
        {
            SceneManager.activeSceneChanged += activeSceneChanged;
            Timer.Stop();
        }

        private static void activeSceneChanged(Scene arg0, Scene arg1)
        {
            OnTofuRunInterupted?.Invoke(arg1, EventArgs.Empty);
        }

        [HarmonyPatch(typeof(SRToffuManager), "FinDeLivraison")]
        private static class FinDeLivraisonPatch
        {
            private static void Postfix(SRToffuManager __instance)
            {
                if (__instance == SRToffuManager)
                {
                    OnTofuRunCompleted?.Invoke(__instance, EventArgs.Empty);
                    Timer.Stop();
                }
            }
        }

        [HarmonyPatch(typeof(SRToffuManager), "StartCompteur")]
        private static class StartCompteurPatch
        {
            private static void Postfix(SRToffuManager __instance)
            {
                if (__instance == SRToffuManager)
                {
                    OnTofuRunStart?.Invoke(__instance, EventArgs.Empty);
                    Timer.Restart();
                }
            }
        }

        [HarmonyPatch(typeof(SRToffuManager), "StopDelivery2")]
        private static class StopDelivery2Patch
        {
            private static void Postfix(SRToffuManager __instance)
            {
                if (__instance == SRToffuManager)
                {
                    OnTofuRunInterupted?.Invoke(__instance, EventArgs.Empty);
                    Timer.Stop();
                }
            }
        }

        [HarmonyPatch(typeof(SRToffuManager), "StopDelivery")]
        private static class StopDeliveryPatch
        {
            private static void Postfix(SRToffuManager __instance)
            {
                if (__instance == SRToffuManager)
                {
                    OnTofuRunInterupted?.Invoke(__instance, EventArgs.Empty);
                    Timer.Stop();
                }
            }
        }
    }
}
