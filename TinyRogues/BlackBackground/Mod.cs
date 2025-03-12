using MelonLoader;
using UnityEngine;
[assembly: MelonInfo(typeof(BlackBackground.Mod), nameof(BlackBackground), "1.0.0", "kruumy")]
[assembly: MelonGame("RubyDev", "Tiny Rogues")]
namespace BlackBackground
{
    public class Mod : MelonMod
    {
        public override void OnSceneWasInitialized( int buildIndex, string sceneName )
        {
            if ( Camera.main != null )
            {
                Camera.main.backgroundColor = Color.black;
            }
        }
    }
}
