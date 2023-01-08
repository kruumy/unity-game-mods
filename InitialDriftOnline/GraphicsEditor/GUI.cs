using EasyIMGUI.Controls.Automatic;
using EasyIMGUI.MelonLoader.Interface;
using System;

namespace GraphicsEditor
{
    public static class GUI
    {
        public static Menu Root { get; } = new Menu();

        public static void Initialize()
        {
            Root.Controls.Add(new Window()
            {
                Content =
                {
                    text = "CameraEditor"
                },
                Dimensions = new UnityEngine.Rect(10, 10, 250, 0),
            });

            foreach (System.Reflection.PropertyInfo prop in typeof(GraphicsWrapper).GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public))
            {
                Toggle toggle = new Toggle();
                toggle.Content.text = prop.Name;
                toggle.Bind(() => (bool)prop.GetValue(null, null), v => prop.SetValue(null, v));
                ((Window)Root.Controls[0]).Controls.Add(toggle);
            }
            SingleButton SaveToPreferencesBtn = new SingleButton();
            SaveToPreferencesBtn.Content.text = "Save Values To File";
            SaveToPreferencesBtn.OnButtonPressed += (object sender, EventArgs e) => Preferences.Save();
            ((Window)Root.Controls[0]).Controls.Add(SaveToPreferencesBtn);
        }
    }
}
