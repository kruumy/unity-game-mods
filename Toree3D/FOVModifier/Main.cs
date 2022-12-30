using MelonLoader;
using UnityEngine;

namespace FOVModifier
{
    public class Main : MelonMod
    {
        private float _FieldOfView = 60f;
        private float FieldOfView
        {
            get => Camera.main.fieldOfView;
            set
            {
                Camera.main.fieldOfView = value;
                _FieldOfView = value;
                MelonLogger.Msg("FOV = " + value);
            }
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            FieldOfView = _FieldOfView;
        }
        public override void OnLateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Minus))
            {
                FieldOfView -= 5;
            }
            else if (Input.GetKeyDown(KeyCode.Equals))
            {
                FieldOfView += 5;
            }
        }
    }
}
