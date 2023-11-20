using BepInEx;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

namespace RemoveCameraFilter
{
    [BepInPlugin("kruumy.RemoveCameraFilter", "Remove Camera Filter", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        private void Awake()
        {
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }

        private void SceneManager_activeSceneChanged( Scene arg0, Scene arg1 )
        {
            foreach ( var volumeobj in UnityEngine.Object.FindObjectsOfTypeAll(typeof(UnityEngine.Rendering.HighDefinition.CustomPassVolume)) )
            {
                if ( volumeobj is CustomPassVolume volume )
                {
                    volume.enabled = false;
                    volume.isGlobal = false;
                }
            }
        }
    }
}
