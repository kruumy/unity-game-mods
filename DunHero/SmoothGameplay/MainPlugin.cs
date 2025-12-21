using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SmoothGameplay
{
    [BepInPlugin("com.kruumy.smoothgameplay", "SmoothGameplay", "1.0.0")]
    public class MainPlugin : BaseUnityPlugin
    {
        public void Awake()
        {
            new Harmony("com.kruumy.smoothgameplay").PatchAll();
            Logger.LogInfo("SmoothGameplay loaded");
        }

        [HarmonyPatch(typeof(Rigidbody2D), nameof(Rigidbody2D.position), MethodType.Getter)]
        class Rigidbody2D_GetPosition_Patch
        {
            public static void Postfix( Rigidbody2D __instance )
            {
                if ( __instance.interpolation != RigidbodyInterpolation2D.Interpolate )
                {
                    __instance.interpolation = RigidbodyInterpolation2D.Interpolate;
                }
            }
        }

        [HarmonyPatch(typeof(Unity.Cinemachine.CinemachineBrain), "Start", MethodType.Normal)]
        class CinemachineBrain_Start_Patch
        {
            public static void Postfix( Unity.Cinemachine.CinemachineBrain __instance )
            {
                __instance.BlendUpdateMethod = Unity.Cinemachine.CinemachineBrain.BrainUpdateMethods.LateUpdate;
                __instance.UpdateMethod = Unity.Cinemachine.CinemachineBrain.UpdateMethods.LateUpdate;
            }
        }
    }
}
