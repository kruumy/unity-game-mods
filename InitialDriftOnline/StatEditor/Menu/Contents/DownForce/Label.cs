using UnityEngine;

namespace StatEditor.Menu.Contents.DownForce
{
    public class Label
    {
        public static void Draw()
        {
            GUILayout.Label($"Down Force = {HorizontalSlider.Value}");
        }
    }
}
