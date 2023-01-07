using MelonLoader;
using System.Collections;
using UnityEngine;

namespace TeleportMenu
{
    public static class Teleport
    {
        private static readonly SRAdminTools SRAdminTools = UnityEngine.Object.FindObjectOfType<SRAdminTools>();

        public static void DownTofu()
        {
            GoTo(RCC_SceneManager.Instance.activePlayerVehicle.gameObject, SRAdminTools.AkagiUp);
        }
        public static void GoTo(GameObject obj, GameObject target)
        {
            GoTo(obj, target.gameObject.transform);
        }
        public static void GoTo(GameObject obj, Transform target)
        {
            SRAdminTools.StartCoroutine(subWork());
            IEnumerator subWork()
            {
                RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Rigidbody>().drag = 1000f;
                _ = Vector3.zero;
                _ = Quaternion.identity;
                yield return new WaitForSeconds(0.2f);
                Vector3 position = target.position;
                Quaternion rotation = target.rotation;
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                yield return new WaitForSeconds(0.2f);
                obj.gameObject.GetComponent<Rigidbody>().drag = 0.01f;
            }
            MelonLogger.Msg($"Teleported {obj.name} -> {target.transform.position}");
        }
        public static void MeToPlayer(string PlayerName)
        {
            MelonLogger.Msg($"Trying {PlayerName}...");
            RCC_CarControllerV3 vehicle = RCC_SceneManager.Instance.get_PlayerVehicleByPlayerName(PlayerName);
            if (vehicle == null)
            {
                MelonLogger.Error($"Could not get obj from player name. '{PlayerName}'");
                return;
            }
            GoTo(RCC_SceneManager.Instance.activePlayerVehicle.gameObject, vehicle.gameObject.transform);
        }

        public static void PlayerToMe(string PlayerName)
        {
            MelonLogger.Msg($"Trying {PlayerName}...");
            RCC_CarControllerV3 vehicle = RCC_SceneManager.Instance.get_PlayerVehicleByPlayerName(PlayerName);
            if (vehicle == null)
            {
                MelonLogger.Error($"Could not get obj from player name. '{PlayerName}'");
                return;
            }
            GoTo(vehicle.gameObject, RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform);
            // not working, need to get player client to tp aswell.
            throw new System.NotImplementedException();
        }

        public static void UpTofu()
        {
            GoTo(RCC_SceneManager.Instance.activePlayerVehicle.gameObject, SRAdminTools.AkinaDown);
        }
    }
}
