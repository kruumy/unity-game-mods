using MelonLoader;
using UnityEngine.Rendering.HighDefinition;

namespace RemoveCameraFilter
{
    public class Main : MelonMod
    {
        public override void OnSceneWasLoaded( int buildIndex, string sceneName )
        {
            foreach ( var volumeobj in UnityEngine.Object.FindObjectsOfTypeAll(typeof(UnityEngine.Rendering.HighDefinition.CustomPassVolume)) )
            {
                if ( volumeobj is CustomPassVolume volume )
                {
                    volume.enabled = false;
                    volume.isGlobal = false;
                    LoggerInstance.Msg("Disabled CustomPassVolume");
                }
            }
        }
    }
}
