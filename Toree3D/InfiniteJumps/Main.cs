using MelonLoader;
using System.Reflection;

namespace InfiniteJumps
{
    public class Main : MelonMod
    {
        private FieldInfo doublejump;
        private FieldInfo donejumps;
        private PlayerSystem playerSystem;
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            playerSystem = UnityEngine.Object.FindObjectOfType<PlayerSystem>();
            if (playerSystem != null)
                MelonLogger.Msg("Found PlayerSystem Object");

            doublejump = typeof(PlayerSystem).GetField("doublejump", BindingFlags.NonPublic | BindingFlags.Instance);
            if (doublejump != null)
                MelonLogger.Msg("Found doublejump Field");

            donejumps = typeof(PlayerSystem).GetField("donejumps", BindingFlags.NonPublic | BindingFlags.Instance);
            if (donejumps != null)
                MelonLogger.Msg("Found donejumps Field");
        }
        public override void OnLateUpdate()
        {
            if (playerSystem != null)
            {
                donejumps?.SetValue(playerSystem, 0);
                doublejump?.SetValue(playerSystem, true);
            }
        }
    }
}
