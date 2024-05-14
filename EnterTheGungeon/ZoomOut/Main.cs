using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace ZoomOut
{
    [BepInPlugin("kruumy.ZoomOut", "ZoomOut", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        public void Awake()
        {
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }
        

        public bool SceneChanged = false;
        public void LateUpdate()
        {
            if( SceneChanged )
            {
                CameraController camera = (CameraController)UnityEngine.Object.FindObjectsOfType(typeof(CameraController)).FirstOrDefault();
                if ( camera != null )
                {
                    camera.OverrideZoomScale = 0.75f;
                    Logger.LogMessage("Camera OverideZoomScale Updated");
                    SceneChanged = false;
                }
                else { Logger.LogMessage("Could not find camera"); }
            }
        }
        private void SceneManager_activeSceneChanged( Scene arg0, Scene arg1 )
        {
            SceneChanged = true;    
        }
    }
}
