using EasyIMGUI.Controls;
using EasyIMGUI.MelonLoader.Interface;
using System;
using static CameraEditor.CameraWrapper;

namespace CameraEditor
{
    public static class GUI
    {
        public static Menu Root { get; } = new Menu();
        public static void Initialize()
        {
            SingleButton SaveToPreferencesBtn = new SingleButton();
            SaveToPreferencesBtn.Content.text = "Save Values To File";
            SaveToPreferencesBtn.OnButtonPressed += (object sender, EventArgs e) => Preferences.Save();
            SingleButton ResetPreferencesBtn = new SingleButton();
            ResetPreferencesBtn.Content.text = "Reset To Default";
            ResetPreferencesBtn.OnButtonPressed += (object sender, EventArgs e) => Preferences.ResetAllToDefault();
            Root.Controls.Add(new Window()
            {
                Content =
                {
                    text = "CameraEditor"
                },
                Dimensions = new UnityEngine.Rect(10, 10, 500, 0),
                Controls =
                {
                    new LabelAndSlider(() => FieldOfView, v => FieldOfView = v)
                    {
                        Label = nameof(FieldOfView),
                        Minimum = 0,
                        Maximum = 180
                    },
                    new LabelAndSlider(() => Distance, v => Distance = v)
                    {
                        Label = nameof(Distance),
                        Minimum = -50,
                        Maximum = 50
                    },
                    new LabelAndSlider(() => Height, v => Height = v)
                    {
                        Label = nameof(Height),
                        Minimum = -50,
                        Maximum = 100
                    },
                    new LabelAndSlider(() => PitchAngle, v => PitchAngle = v)
                    {
                        Label = nameof(PitchAngle),
                        Minimum = -50,
                        Maximum = 100
                    },
                    new LabelAndSlider(() => YawAngle, v => YawAngle = v)
                    {
                        Label = nameof(YawAngle),
                        Minimum = -50,
                        Maximum = 50
                    },
                    new LabelAndSlider(() => OffsetX, v => OffsetX = v)
                    {
                        Label = nameof(OffsetX),
                        Minimum = -50,
                        Maximum = 50
                    },
                    new LabelAndSlider(() => OffsetY, v => OffsetY = v)
                    {
                        Label = nameof(OffsetY),
                        Minimum = -50,
                        Maximum = 50
                    },
                    new Horizontal()
                    {
                        Controls =
                        {
                            SaveToPreferencesBtn,
                            ResetPreferencesBtn
                        }
                    }
                    
                }
            });
        }
    }
}
