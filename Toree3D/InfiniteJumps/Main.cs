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
            doublejump = typeof(PlayerSystem).GetField("doublejump", BindingFlags.NonPublic | BindingFlags.Instance);
            donejumps = typeof(PlayerSystem).GetField("donejumps", BindingFlags.NonPublic | BindingFlags.Instance);
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
