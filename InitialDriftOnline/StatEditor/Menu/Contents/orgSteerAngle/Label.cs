using UnityEngine;

namespace StatEditor.Menu.Contents.orgSteerAngle
{
    public static class Label
    {
        public static void Draw()
        {
            GUILayout.Label($"orgSteerAngle = {(int)HorizontalSlider.Value}");
        }
    }
}
