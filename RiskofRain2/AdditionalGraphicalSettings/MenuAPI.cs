using System;
using System.Linq;
using RoR2.UI;
using UnityEngine;



namespace AddFoVSettings
{
    public enum SubPanel
    {
        Gameplay,
        KeyboardControls,
        GamepadControls,
        Audio,
        Video,
        Graphics
    }

    public class MenuSlider
    {
        float value;
        public float defaultValue;
        public float maxValue;
        public float minValue;
        public bool wholeNumbers;

        public string settingName;
        private string token;
        public string settingDescription;
        SubPanel panelLocation;
        public bool showInPauseSettings;
        SettingsSlider settingsSlider = null;
        public float revertValue;

        private string GetSubPanelName(SubPanel panel)
        {
            switch (panel)
            {
                case SubPanel.Gameplay:
                    return "SettingsSubPanel, Gameplay";
                case SubPanel.KeyboardControls:
                    return "SettingsSubPanel, Controls (M&KB)";
                case SubPanel.GamepadControls:
                    return "SettingsSubPanel, Controls (Gamepad)";
                case SubPanel.Audio:
                    return "SettingsSubPanel, Audio";
                case SubPanel.Video:
                    return "SettingsSubPanel, Video";
                case SubPanel.Graphics:
                    return "SettingsSubPanel, Graphics";
                default:
                    return "";
            }
        }

        public MenuSlider(float defaultValue, float maxValue, float minValue, bool wholeNumbers, string settingName, string settingDescription, SubPanel panelLocation, bool showInPauseSettings = true)
        {
            this.defaultValue = defaultValue;
            this.maxValue = maxValue;
            this.minValue = minValue;
            this.wholeNumbers = wholeNumbers;
            this.settingName = settingName;
            this.settingDescription = settingDescription;
            this.panelLocation = panelLocation;
            this.showInPauseSettings = showInPauseSettings;

            //setup playerprefs
            token = settingName.Replace(" ", "");
            if (!PlayerPrefs.HasKey(token)) PlayerPrefs.SetFloat(token, defaultValue);
            value = PlayerPrefs.GetFloat(token);

            On.RoR2.UI.SettingsSlider.OnSliderValueChanged += hook_OnSliderValueChanged;
            On.RoR2.UI.MainMenu.SubmenuMainMenuScreen.OnEnter += (orig, self, mainMenuController) =>
            {
                orig(self, mainMenuController);
                AddSettingField(self.submenuPanelInstance.transform);
            };
            On.RoR2.UI.SettingsPanelController.RevertChanges += hook_RevertChanges;
            On.RoR2.UI.PauseScreenController.OpenSettingsMenu += (orig, self) =>
            {
                orig(self);
                if (showInPauseSettings)
                    AddSettingField(self.submenuObject.transform);
            };
        }

        public float GetValue()
        {
            return value;
        }

        public void SetValue(float newValue)
        {
            value = newValue;
            if (settingsSlider)
            {
                settingsSlider.valueText.text = value.ToString();
                settingsSlider.slider.value = value;
            }
            PlayerPrefs.SetFloat(token, newValue);
            if (OnSliderChanged != null)
                OnSliderChanged.Invoke(newValue);
        }

        public delegate void SliderChanged(float newValue);
        public event SliderChanged OnSliderChanged;

        void AddSettingField(Transform settingsPanelInstance)
        {
            GameObject buttonToInstantiate = settingsPanelInstance.Find("SafeArea/SubPanelArea/" + GetSubPanelName(SubPanel.Gameplay) + "/Scroll View/Viewport/VerticalLayout/SettingsEntryButton, Slider (Screen Shake Scale)").gameObject;
            Transform gameplaySubPanel = settingsPanelInstance.Find("SafeArea/SubPanelArea/" + GetSubPanelName(panelLocation) + "/Scroll View/Viewport/VerticalLayout");

            GameObject buttonCopy = GameObject.Instantiate(buttonToInstantiate);
            buttonCopy.transform.SetParent(gameplaySubPanel);
            buttonCopy.name = "SettingsEntryButton, Slider (" + settingName + ")";
            settingsSlider = buttonCopy.GetComponent<SettingsSlider>();

            settingsSlider.minValue = minValue;
            settingsSlider.maxValue = maxValue;
            settingsSlider.slider.minValue = minValue;
            settingsSlider.slider.maxValue = maxValue;
            settingsSlider.slider.wholeNumbers = wholeNumbers;
            settingsSlider.settingName = settingName;
            settingsSlider.nameToken = token;
            settingsSlider.nameLabel.token = settingName;
            settingsSlider.originalValue = defaultValue.ToString();
            SetValue(value);
            settingsSlider.GetComponent<HGButton>().hoverToken = settingDescription;
            revertValue = value;
        }

        void hook_RevertChanges(On.RoR2.UI.SettingsPanelController.orig_RevertChanges orig, SettingsPanelController self)
        {
            if (!settingsSlider) return;
            if (self.settingsControllers != null && self.settingsControllers.Contains(settingsSlider))
            {
                SetValue(revertValue);
            }
            orig(self);
        }

        void hook_OnSliderValueChanged(On.RoR2.UI.SettingsSlider.orig_OnSliderValueChanged orig, SettingsSlider self, float newValue)
        {
            orig(self, newValue);

            if (self.nameToken == token)
            {
                SetValue(newValue);
            }
        }
    }

    public class MenuCheckbox
    {
        bool value;
        public bool defaultValue;

        public string settingName;
        private string token;
        public string settingDescription;
        SubPanel panelLocation;
        public bool showInPauseSettings;
        public bool revertValue;
        public CarouselController controller;

        private string GetSubPanelName(SubPanel panel)
        {
            switch (panel)
            {
                case SubPanel.Gameplay:
                    return "SettingsSubPanel, Gameplay";
                case SubPanel.KeyboardControls:
                    return "SettingsSubPanel, Controls (M&KB)";
                case SubPanel.GamepadControls:
                    return "SettingsSubPanel, Controls (Gamepad)";
                case SubPanel.Audio:
                    return "SettingsSubPanel, Audio";
                case SubPanel.Video:
                    return "SettingsSubPanel, Video";
                case SubPanel.Graphics:
                    return "SettingsSubPanel, Graphics";
                default:
                    return "";
            }
        }

        public MenuCheckbox(bool defaultValue, string settingName, string settingDescription, SubPanel panelLocation, bool showInPauseSettings = true)
        {
            this.defaultValue = defaultValue;
            this.settingName = settingName;
            this.settingDescription = settingDescription;
            this.panelLocation = panelLocation;
            this.showInPauseSettings = showInPauseSettings;

            //setup playerprefs
            token = settingName.Replace(" ", "");
            if (!PlayerPrefs.HasKey(token))
            {
                PlayerPrefs.SetInt(token, Convert.ToInt32(defaultValue));
                Log.LogInfo("Added new token" + token);
            }
            SetValue(Convert.ToBoolean(PlayerPrefs.GetInt(token)));

            On.RoR2.UI.CarouselController.BoolCarousel += hook_BoolCarousel;
            On.RoR2.UI.MainMenu.SubmenuMainMenuScreen.OnEnter += (orig, self, mainMenuController) =>
            {
                orig(self, mainMenuController);
                AddSettingField(self.submenuPanelInstance.transform);
            };
            On.RoR2.UI.SettingsPanelController.RevertChanges += hook_RevertChanges;
            On.RoR2.UI.PauseScreenController.OpenSettingsMenu += (orig, self) =>
            {
                orig(self);
                if (showInPauseSettings)
                    AddSettingField(self.submenuObject.transform);
            };
        }

        bool StringToBool(string s)
        {
            if (s == null || s == "")
            {
                Log.LogError("string was null");
                return false;
            }
            return Convert.ToBoolean(int.Parse(s));
        }

        public bool GetValue()
        {
            return value;
        }

        public void SetValue(bool newValue)
        {
            value = newValue;
            PlayerPrefs.SetInt(token, Convert.ToInt32(newValue));
            if (OnCheckboxChanged != null)
                OnCheckboxChanged.Invoke(value);
        }

        public delegate void CheckboxChanged(bool newValue);
        public event CheckboxChanged OnCheckboxChanged;

        void AddSettingField(Transform settingsPanelInstance)
        {
            GameObject buttonToInstantiate = settingsPanelInstance.Find("SafeArea/SubPanelArea/" + GetSubPanelName(SubPanel.Gameplay) + "/Scroll View/Viewport/VerticalLayout/SettingsEntryButton, Bool (Screen Distortion)").gameObject;
            Transform gameplaySubPanel = settingsPanelInstance.Find("SafeArea/SubPanelArea/" + GetSubPanelName(panelLocation) + "/Scroll View/Viewport/VerticalLayout");

            GameObject buttonCopy = GameObject.Instantiate(buttonToInstantiate);
            buttonCopy.transform.SetParent(gameplaySubPanel);
            buttonCopy.name = "SettingsEntryButton, Bool (" + settingName + ")";
            controller = buttonCopy.GetComponent<CarouselController>();
            bool current = StringToBool(controller.GetCurrentValue());

            controller.settingName = settingName;
            controller.nameToken = token;
            controller.nameLabel.token = settingName;
            controller.originalValue = defaultValue.ToString();

            if (current != value)
            {
                SetValue(current);
                controller.BoolCarousel();
                Log.LogInfo("Bool Was Wrong");
            }

            controller.GetComponent<HGButton>().hoverToken = settingDescription;
            revertValue = value;
        }

        void hook_RevertChanges(On.RoR2.UI.SettingsPanelController.orig_RevertChanges orig, SettingsPanelController self)
        {
            if (!controller) return;
            if (self.settingsControllers != null && self.settingsControllers.Contains(controller))
            {
                SetValue(revertValue);
            }
            orig(self);
        }

        void hook_BoolCarousel(On.RoR2.UI.CarouselController.orig_BoolCarousel orig, CarouselController self)
        {
            orig(self);
            Log.LogInfo("Bool Switched");
            if (self.nameToken == token)
            {
                SetValue(!value);
            }
        }
    }

}