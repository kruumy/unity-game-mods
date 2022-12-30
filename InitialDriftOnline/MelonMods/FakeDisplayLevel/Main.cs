using HarmonyLib;
using MelonLoader;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace FakeDisplayLevel
{
    public class Main : MelonMod
    {
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {

        }
        public override void OnLateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {

            }
        }

        // TODO: not working
        [HarmonyPatch(typeof(PhotonNetwork), "RPC", typeof(PhotonView), typeof(string), typeof(RpcTarget), typeof(Player), typeof(bool), typeof(object[]))]
        private static class Patch
        {
            private static void Prefix(PhotonView view, string methodName, RpcTarget target, Player player, bool encrypt, ref object[] parameters)
            {
                if (view.IsMine && methodName == "SendRPC_PlayernameOnCars2")
                {
                    MelonLogger.Msg(parameters[parameters.Length - 1]);
                    parameters[parameters.Length - 1] = 75;
                }
            }
        }
    }
}
