using BepInEx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Modernizer
{
    [BepInPlugin("kruumy.Modernizer", "Modernizer", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        public void Awake()
        {
            SceneManager.activeSceneChanged += ( Scene arg0, Scene arg1 ) =>
            {
                Application.targetFrameRate = -1;
                if
                (
                Camera.main != null &&
                Camera.main.targetTexture != null &&
                Camera.main.targetTexture.width != Screen.width
                && Camera.main.targetTexture.height != Screen.height
                )
                {
                    ChangeCameraTextureResolution(Camera.main, Screen.width, Screen.height);
                }

            };
        }

        public void ChangeCameraTextureResolution( Camera camera, int width, int height )
        {
            RenderTexture texture = camera.targetTexture;
            camera.targetTexture = null;

            texture.Release();
            texture.width = width;
            texture.height = height;
            texture.Create();

            camera.targetTexture = texture;
        }
    }
}
