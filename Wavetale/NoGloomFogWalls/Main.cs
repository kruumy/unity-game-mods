using Il2Cpp;
using MelonLoader;

[assembly: MelonInfo(typeof(NoGloomFogWalls.Main), "NoGloomFogWalls", "1.0.0", "kruumy")]
[assembly: MelonGame("Thunderful", "Wavetale")]

namespace NoGloomFogWalls
{
    public class Main : MelonMod
    {
        public override void OnSceneWasLoaded( int buildIndex, string sceneName )
        {
            foreach ( var wall in UnityEngine.Object.FindObjectsOfType<Il2Cpp.FogWall>() )
            {
                UnityEngine.Object.Destroy(wall.gameObject);
            }
        }
    }
}
