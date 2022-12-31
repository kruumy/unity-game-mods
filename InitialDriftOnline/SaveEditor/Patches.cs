using CodeStage.AntiCheat.Storage;
using HarmonyLib;
using Steamworks;

namespace SaveEditor
{
    internal class Patches
    {
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
