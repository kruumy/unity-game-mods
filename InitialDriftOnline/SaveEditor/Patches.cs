﻿using CodeStage.AntiCheat.Storage;
using HarmonyLib;
using MelonLoader;
using Steamworks;

namespace SaveEditor
{
    internal static class Patches
    {
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
                                MelonLogger.Msg($"Patched Invalid Level {value}");
                                value = PlayerSaveWrapper.MyLvl;
                                break;
                            }
                        case "XP":
                            {
                                MelonLogger.Msg($"Patched Invalid XP {value}");
                                value = PlayerSaveWrapper.XP;
                                break;
                            }
                    }
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
                    nScore = PlayerSaveWrapper.MyLvl;
                }
            }
        }
    }
}
