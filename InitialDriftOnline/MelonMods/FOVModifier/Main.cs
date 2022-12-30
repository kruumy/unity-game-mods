using MelonLoader;
using UnityEngine;

namespace FOVModifier
{
    public class Main : MelonMod
    {
        public override void OnLateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Minus))
            {
                if (Camera.FieldOfView != null)
                {
                    Camera.FieldOfView -= 5;
                    MelonLogger.Msg($"{nameof(Camera.FieldOfView)} = {Camera.FieldOfView}");
                }
            }
            else if (Input.GetKeyDown(KeyCode.Equals))
            {
                if (Camera.FieldOfView != null)
                {
                    Camera.FieldOfView += 5;
                    MelonLogger.Msg($"{nameof(Camera.FieldOfView)} = {Camera.FieldOfView}");
                }
            }
        }
    }
}
