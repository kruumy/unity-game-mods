using UnityEngine;

namespace StatEditor.Menu.Contents.DownForce
{
    public class HorizontalSlider
    {
        public static float Value
        {
            get => PlayerVehicleWrapper.DownForce;
            set => PlayerVehicleWrapper.DownForce = value;
        }
        public static void Draw()
        {
            Value = GUILayout.HorizontalSlider(Value, 0, 500);
        }
    }
}
