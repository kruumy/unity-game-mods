using EasyIMGUI.Controls.Automatic;
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
                Dimensions = new Rect(10, 10, 300, 10),
                Controls =
                {
                    new LabelAndSlider(
                        () => RCC_SceneManager.Instance.activePlayerVehicle.downForce,
                        v => RCC_SceneManager.Instance.activePlayerVehicle.downForce = v)
                    {
                        Label = "downForce",
                        Minimum = -500,
                        Maximum = 500,
                    },
                    new LabelAndSlider(
                        () => RCC_SceneManager.Instance.activePlayerVehicle.engineTorque,
                        v => RCC_SceneManager.Instance.activePlayerVehicle.engineTorque = v)
                    {
                        Label = "engineTorque",
                        Minimum = 0,
                        Maximum = 10000,
                    },
                    new LabelAndSlider(
                        () => RCC_SceneManager.Instance.activePlayerVehicle.brakeTorque,
                        v => RCC_SceneManager.Instance.activePlayerVehicle.brakeTorque = v)
                    {
                        Label = "brakeTorque",
                        Minimum = 0,
                        Maximum = 1000000,
                    },
                    new LabelAndSlider(
                        () => RCC_SceneManager.Instance.activePlayerVehicle.maxspeed,
                        v => RCC_SceneManager.Instance.activePlayerVehicle.maxspeed = v)
                    {
                        Label = "maxspeed",
                        Minimum = 0,
                        Maximum = 1000,
                    },
                    new LabelAndSlider(
                        () => RCC_SceneManager.Instance.activePlayerVehicle.get_orgSteerAngle(),
                        v => RCC_SceneManager.Instance.activePlayerVehicle.set_orgSteerAngle(v))
                    {
                        Label = "orgSteerAngle",
                        Minimum = 0,
                        Maximum = 180,
                    },
                    new LabelAndSlider(
                        () => RCC_SceneManager.Instance.activePlayerVehicle.highspeedsteerAngle,
                        v => RCC_SceneManager.Instance.activePlayerVehicle.highspeedsteerAngle = v)
                    {
                        Label = "highspeedsteerAngle",
                        Minimum = 0,
                        Maximum = 180,
                    },
                    new LabelAndSlider(
                        () => RCC_SceneManager.Instance.activePlayerVehicle.highspeedsteerAngleAtspeed,
                        v => RCC_SceneManager.Instance.activePlayerVehicle.highspeedsteerAngleAtspeed = v)
                    {
                        Label = "highspeedsteerAngleAtspeed",
                        Minimum = 0,
                        Maximum = 1000,
                    },
                    SaveToPreferencesBtn
                }
            });
        }
    }
}
