using BepInEx;
using Dunhero;
using Dunhero.CreatureSystem;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MiscQOL
{
    [BepInPlugin("com.kruumy.miscQOL", "MiscQOL", "1.0.0")]
    public class MainPlugin : BaseUnityPlugin
    {
        public void Awake()
        {
            new Harmony("com.kruumy.miscQOL").PatchAll();
            Logger.LogInfo("MiscQOL loaded");
        }


        [HarmonyPatch(typeof(PlayerDash), "TryDash", MethodType.Normal)]
        class PlayerDash_TryDash_Patch
        {
            static readonly AccessTools.FieldRef<PlayerDash, int> usagesLeft = AccessTools.FieldRefAccess<PlayerDash, int>("_usagesLeft");
            static readonly AccessTools.FieldRef<PlayerDash, Player> player = AccessTools.FieldRefAccess<PlayerDash, Player>("_player");
            public static void Postfix( PlayerDash __instance )
            {
                if( player(__instance).IsOutOfCombat() )
                {
                    usagesLeft(__instance)++;
                    __instance.UpdateDashCount();
                }
            }
        }
    }
}