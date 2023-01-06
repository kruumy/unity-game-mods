using MelonLoader;
using System.Collections;
using UnityEngine;

namespace TeleportMenu
{
    public static class Teleport
    {
        private static SRAdminTools SRAdminTools = UnityEngine.Object.FindObjectOfType<SRAdminTools>();
        public static void UpTofu()
        {
            GoTo(SRAdminTools.AkinaDown);
        }
        public static void DownTofu()
        {
            GoTo(SRAdminTools.AkagiUp);
        }

        public static void ToPlayer(string PlayerName)
        {
            MelonLogger.Msg($"Trying {PlayerName}...");
            RCC_CarControllerV3 vehicle = RCC_SceneManager.Instance.get_PlayerVehicleByPlayerName(PlayerName);
            if (vehicle == null)
            {
                MelonLogger.Error($"Could not get car from player name. '{PlayerName}'");
                return;
            }
            GoTo(vehicle.gameObject.transform);
        }
        public static void GoTo(Transform Target)
        {
            System.Reflection.MethodInfo method = typeof(SRAdminTools).GetMethod("LobbySpawn", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Rigidbody>().drag = 1000f;
            SRAdminTools.StartCoroutine((IEnumerator)method.Invoke(SRAdminTools, new object[] { Target }));
            MelonLogger.Msg($"Teleported -> {Target.position}");
        }
    }
}
