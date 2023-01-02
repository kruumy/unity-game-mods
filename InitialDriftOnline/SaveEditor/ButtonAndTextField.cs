using EasyIMGUI.Controls.Base;
using MelonLoader;
using System;
using UnityEngine;

namespace SaveEditor
{
    public class ButtonAndTextField : ValueControl<string>, IContent
    {
        public GUIContent Content { get; set; } = new GUIContent("");
        public Action<int> ValueSetter { get; set; }
        public override void Draw()
        {
            if (GUILayout.Button(Content))
            {
                try
                {
                    ValueSetter.Invoke(int.Parse(Value));
                }
                catch (FormatException)
                {
                    MelonLogger.Msg($"Invalid Format at \"{Value}\"");
                }
            }
            Value = GUILayout.TextField(Value);
        }
    }
}
