﻿using CodeStage.AntiCheat.Storage;
using HarmonyLib;
using MelonLoader;
using Steamworks;
using UnityEngine;

namespace SaveEditor
{
    public class Main : MelonMod
    {
        private bool IsMenuOpen = false;
        public override void OnLateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                if (!IsMenuOpen)
                {
                    MelonEvents.OnGUI.Subscribe(Menu.Menu.Draw);
                    IsMenuOpen = true;
                }
                else
                {
                    MelonEvents.OnGUI.Unsubscribe(Menu.Menu.Draw);
                    IsMenuOpen = false;
                }
            }
        }


        [HarmonyPatch(typeof(SteamUserStats), "UploadLeaderboardScore")]
        private static class UploadLeaderboardScorePatch
        {
            private static void Prefix(SteamLeaderboard_t hSteamLeaderboard, ELeaderboardUploadScoreMethod eLeaderboardUploadScoreMethod, ref int nScore, int[] pScoreDetails, int cScoreDetailsCount)
            {
                if (UnityEngine.Object.FindObjectOfType<LeaderboardUsersManager>().BESTLVL.LeaderboardId.Value == hSteamLeaderboard)
                {
                    nScore = Save.MyLvl;
                }
            }
        }

        [HarmonyPatch(typeof(ObscuredPrefs), "SetInt")]
        private static class SetIntPatch
        {
            private static void Prefix(string key, ref int value)
            {
                if (value == 0 || value == 1000)
                {
                    switch (key)
                    {
                        case "MyLvl":
                            {
                                value = Save.MyLvl;
                                break;
                            }
                        case "XP":
                            {
                                value = Save.XP;
                                break;
                            }
                    }

                }
            }
        }
    }
}