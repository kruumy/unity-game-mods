using System;
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
        public static void SliderWithLabel(float sliderValue, Action<float> sliderValueSetter, float sliderMin, float sliderMax, string label)
        {
            GUILayout.Label($"{label} = {(int)sliderValue}");
            sliderValueSetter.Invoke(GUILayout.HorizontalSlider(sliderValue, sliderMin, sliderMax));
        }
        public static void Button(Action work, string label)
        {
            if (GUILayout.Button(label))
            {
                work.Invoke();
            }
        }
    }
}
