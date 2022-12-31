using MelonLoader;
using System;
using UnityEngine;

namespace SaveEditor.Menu.Contents
{
    public static class Generics
    {
        public static void ButtonAndTextField(ref string textFieldText, Action<int> valueSetter, string buttonLabel)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(buttonLabel))
            {
                try
                {
                    valueSetter.Invoke(int.Parse(textFieldText));
                }
                catch (FormatException)
                {
                    MelonLogger.Msg($"Invalid Format at \"{buttonLabel}\"");
                }

            }
            textFieldText = GUILayout.TextField(textFieldText);
            GUILayout.EndHorizontal();
        }
    }
}
