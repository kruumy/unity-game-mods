using UnityEngine;

namespace StatEditor.Menu.Contents
{
    public static class Generics
    {
        public static void SliderWithLabel(ref float sliderValue, float sliderMin, float sliderMax, string label)
        {
            GUILayout.Label($"{label} = {(int)sliderValue}");
            sliderValue = GUILayout.HorizontalSlider(sliderValue, sliderMin, sliderMax);
        }
    }
}
