using UnityEngine;

namespace StatEditor.Menu.Contents.orgSteerAngle
{
    public static class HorizontalSlider
    {
        public static float Value
        {
            get => RCC_SceneManager.Instance.activePlayerVehicle.get_orgSteerAngle();
            set => RCC_SceneManager.Instance.activePlayerVehicle.set_orgSteerAngle(value);
        }
        public static void Draw()
        {
            Value = GUILayout.HorizontalSlider(Value, 0, 180);
        }
    }
}
