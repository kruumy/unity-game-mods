using System;
using UnityEngine;

namespace GraphicsEditor.Menu.Contents
{
    public static class Generics
    {
        public static void ToggleSwitch(bool value, Action<bool> valueSetter, string label)
        {
            valueSetter(GUILayout.Toggle(value, label));
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
