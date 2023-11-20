using BepInEx;
using UnityEngine.SceneManagement;

namespace AlwaysScanning
{
    [BepInPlugin("kruumy.AlwaysScanning", "Always Scanning", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        private void Awake()
        {
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }

        private void SceneManager_activeSceneChanged( Scene arg0, Scene arg1 )
        {
            if ( HUDManager.Instance != null )
            {
                System.Reflection.FieldInfo playerPingingScan = typeof(HUDManager).GetField("playerPingingScan", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                playerPingingScan.SetValue(HUDManager.Instance, float.MaxValue);
            }
        }
    }
}
