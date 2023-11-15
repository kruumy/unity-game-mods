namespace AlwaysScanning
{
    public class Main : MelonLoader.MelonMod
    {
        public override void OnSceneWasLoaded( int buildIndex, string sceneName )
        {
            if ( HUDManager.Instance != null )
            {
                System.Reflection.FieldInfo playerPingingScan = typeof(HUDManager).GetField("playerPingingScan", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                playerPingingScan.SetValue(HUDManager.Instance, float.MaxValue);
                LoggerInstance.Msg("Set field playerPingingScan to max value");
            }
        }
    }
}
