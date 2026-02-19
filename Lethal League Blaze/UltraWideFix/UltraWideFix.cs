using BepInEx;
using UnityEngine;

namespace UltraWideFix
{
    [BepInPlugin("kruumy.plugins.llb.ultrawidefix", "UltraWideFix", "1.0.0")]
    [BepInProcess("LLBlaze.exe")]
    public class UltraWideFix : BaseUnityPlugin
    {
        public readonly float aspect = Screen.width / Screen.height;
        public readonly Rect pixelRect = new Rect(0, 0, Screen.width, Screen.height);
        public void FixedUpdate()
        {
            foreach (var camera in UnityEngine.Camera.allCameras)
            {
                camera.aspect = aspect;
                camera.pixelRect = pixelRect;
            }
        }
    }
}
