using EasyIMGUI.Controls;
using System;
using UnityEngine;

namespace CameraEditor
{
    public class LabelAndSlider : HorizontalSlider
    {
        public LabelAndSlider(Func<float> getter, Action<float> setter)
        {
            Bind(getter, setter);
        }

        public string Label { get; set; } = "";
        public override void Draw()
        {
            GUILayout.Label($"{Label} = {Math.Round(Value,2)}");
            Value = GUILayout.HorizontalSlider(Value, Minimum, Maximum);
        }
    }
}
