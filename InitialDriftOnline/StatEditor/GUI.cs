using EasyIMGUI.Controls;
using System;
using UnityEngine;

namespace StatEditor
{
    public static class GUI
    {
        public static readonly EasyIMGUI.MelonLoader.Interface.Menu Root = new EasyIMGUI.MelonLoader.Interface.Menu();
        public static void Initialize()
        {
            SingleButton SaveToPreferencesBtn = new SingleButton();
            SaveToPreferencesBtn.Content.text = "Save Values To File";
            SaveToPreferencesBtn.OnButtonPressed += (object sender, EventArgs e) => Preferences.Save();
            Root.Controls.Add(new Window()
            {
                Content =
                {
                    text = "Stat Editor"
                },
                Dimensions = new Rect(10, 10, 300, 255),
                Controls =
                {
                    new LabelAndSlider()
                    {
                        Label = "downForce",
                        Minimum = -500,
                        Maximum = 500,
                        ValueGetter = () => RCC_SceneManager.Instance.activePlayerVehicle.downForce,
                        ValueSetter = v => RCC_SceneManager.Instance.activePlayerVehicle.downForce = v,
                    },
                    new LabelAndSlider()
                    {
                        Label = "engineTorque",
                        Minimum = 0,
                        Maximum = 10000,
                        ValueGetter = () => RCC_SceneManager.Instance.activePlayerVehicle.engineTorque,
                        ValueSetter = v => RCC_SceneManager.Instance.activePlayerVehicle.engineTorque = v,
                    },
                    new LabelAndSlider()
                    {
                        Label = "brakeTorque",
                        Minimum = 0,
                        Maximum = 1000000,
                        ValueGetter = () => RCC_SceneManager.Instance.activePlayerVehicle.brakeTorque,
                        ValueSetter = v => RCC_SceneManager.Instance.activePlayerVehicle.brakeTorque = v,
                    },
                    new LabelAndSlider()
                    {
                        Label = "maxspeed",
                        Minimum = 0,
                        Maximum = 1000,
                        ValueGetter = () => RCC_SceneManager.Instance.activePlayerVehicle.maxspeed,
                        ValueSetter = v => RCC_SceneManager.Instance.activePlayerVehicle.maxspeed = v,
                    },
                    new LabelAndSlider()
                    {
                        Label = "orgSteerAngle",
                        Minimum = 0,
                        Maximum = 180,
                        ValueGetter = () => RCC_SceneManager.Instance.activePlayerVehicle.get_orgSteerAngle(),
                        ValueSetter = v => RCC_SceneManager.Instance.activePlayerVehicle.set_orgSteerAngle(v),
                    },
                    SaveToPreferencesBtn
                }
            });
        }
    }
}
