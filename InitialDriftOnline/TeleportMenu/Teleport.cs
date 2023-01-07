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
            _ = SRAdminTools.StartCoroutine(subWork());
            IEnumerator subWork()
            {
                obj.GetComponent<Rigidbody>().drag = 1000f;
                yield return new WaitForSeconds(0.2f);
                obj.transform.position = target.position;
                obj.transform.rotation = target.rotation;
                yield return new WaitForSeconds(0.2f);
                obj.GetComponent<Rigidbody>().drag = 0.01f;
            }
            MelonLogger.Msg($"Teleported {obj.name} -> {target.transform.position}");
        }

        public static void MeToPlayer(string PlayerName)
        {
            RCC_CarControllerV3 vehicle = RCC_SceneManager.Instance.get_PlayerVehicleByPlayerName(PlayerName);
            if (vehicle == null)
            {
                MelonLogger.Error($"Could not get obj from player name. '{PlayerName}'");
                return;
            }
            GoTo(RCC_SceneManager.Instance.activePlayerVehicle.gameObject, vehicle.gameObject.transform);
        }

        public static void UpTofu()
        {
            GoTo(RCC_SceneManager.Instance.activePlayerVehicle.gameObject, SRAdminTools.AkinaDown);
        }
    }
}
