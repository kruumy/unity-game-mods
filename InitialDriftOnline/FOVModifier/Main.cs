using MelonLoader;
using UnityEngine;

namespace FOVModifier
{
    public class Main : MelonMod
    {
        private float? oldFOV = null;
        public override void OnLateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Minus))
            {
                if (Camera.FieldOfView != null)
                {
                    Camera.FieldOfView -= 5;
                    oldFOV = Camera.FieldOfView;
                    MelonLogger.Msg($"{nameof(Camera.FieldOfView)} = {Camera.FieldOfView}");
                }
            }
            else if (Input.GetKeyDown(KeyCode.Equals))
            {
                if (Camera.FieldOfView != null)
                {
                    Camera.FieldOfView += 5;
                    oldFOV = Camera.FieldOfView;
                    MelonLogger.Msg($"{nameof(Camera.FieldOfView)} = {Camera.FieldOfView}");
                }
            }
            if (Camera.FieldOfView != null && oldFOV != null && (Camera.FieldOfView != oldFOV))
            {
                Camera.FieldOfView = oldFOV;
            }
        }
    }
}
