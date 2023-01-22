using MelonLoader;
using UnityEngine;

namespace OptifineZoom
{
    public class Main : MelonMod
    {
        public float FROM_FOV { get; set; }
        public float TO_FOV => FROM_FOV / 4;
        public override void OnLateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                FROM_FOV = CameraController.Instance.defaultFov;
                CameraController.Instance.defaultFov = TO_FOV;
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                CameraController.Instance.defaultFov = FROM_FOV;
            }
        }
    }
}
