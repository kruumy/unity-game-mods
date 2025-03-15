using HarmonyLib;
using HighFPSSupport;
using Il2CppPlayer;
using MelonLoader;
using UnityEngine;
using static MelonLoader.MelonLogger;

[assembly: MelonInfo(typeof(Mod), nameof(HighFPSSupport), "1.0.0", "kruumy")]
[assembly: MelonGame("RubyDev", "Tiny Rogues")]
namespace HighFPSSupport
{
    public class Mod : MelonMod
    {
        public override void OnFixedUpdate()
        {
            foreach ( var rb in UnityEngine.Object.FindObjectsOfType<Rigidbody2D>() )
            {
                rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            }
        }
    }
}
