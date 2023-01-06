using HarmonyLib;
using System;
using System.Diagnostics;
using UnityEngine.SceneManagement;

namespace WhatsMyTime
{
    public static class SRToffuManagerWrapper
    {
        public static event EventHandler OnTofuRunStart;

        public static event EventHandler OnTofuRunStopped;

        public static SRToffuManager SRToffuManager => UnityEngine.Object.FindObjectOfType<SRToffuManager>();

        public static Stopwatch Timer { get; } = new Stopwatch();

        public static void Initialize()
        {
            SceneManager.activeSceneChanged += activeSceneChanged;
        }

        private static void activeSceneChanged(Scene arg0, Scene arg1)
        {
            OnTofuRunStopped.Invoke(null, EventArgs.Empty);
        }

        [HarmonyPatch(typeof(SRToffuManager), "FinDeLivraison")]
        private static class FinDeLivraisonPatch
        {
            private static void Postfix(SRToffuManager __instance)
            {
                if (__instance == SRToffuManager)
                {
                    OnTofuRunStopped.Invoke(__instance, EventArgs.Empty);
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
                    OnTofuRunStart.Invoke(__instance, EventArgs.Empty);
                    Timer.Restart();
                }
            }
        }
    }
}
