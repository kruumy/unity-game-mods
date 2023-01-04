using EasyIMGUI.Controls.Automatic;
using System;
using UnityEngine;

namespace StatEditor
{
    public class LabelAndSlider : HorizontalSlider
    {
        public LabelAndSlider(Func<float> getter, Action<float> setter) => Bind(getter, setter);
        public string Label { get; set; } = "";
        public override void Draw()
        {
            GUILayout.Label($"{Label} = {(int)Value}", LayoutOptions);
            Value = GUILayout.HorizontalSlider(Value, Minimum, Maximum, LayoutOptions);
        }
    }
}
