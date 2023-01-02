using EasyIMGUI.Controls.Base;
using UnityEngine;

namespace StatEditor
{
    public class LabelAndSlider : BindedValueControl<float>, ISlider
    {
        public string Label { get; set; } = "";
        public float Minimum { get; set; } = 0;
        public float Maximum { get; set; } = 10;
        public override void Draw()
        {
            GUILayout.Label($"{Label} = {(int)Value}");
            Value = GUILayout.HorizontalSlider(Value, Minimum, Maximum);
        }
    }
}
