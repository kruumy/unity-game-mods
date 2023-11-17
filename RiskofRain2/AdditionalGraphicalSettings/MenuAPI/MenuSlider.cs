using RoR2.UI;
using System;
using UnityEngine;

namespace AdditionalGraphicalSettings.MenuAPI
{
    public class MenuSlider : MenuControl<float, SettingsSlider>
    {
        public float maxValue;
        public float minValue;
        public bool wholeNumbers;

        public MenuSlider( float defaultValue, float maxValue, float minValue, bool wholeNumbers, string settingName, string settingDescription, SubPanel panelLocation, bool showInPauseSettings = true ) : base(defaultValue, settingName, settingDescription, panelLocation, showInPauseSettings)
        {
            this.maxValue = maxValue;
            this.minValue = minValue;
            this.wholeNumbers = wholeNumbers;
            On.RoR2.UI.SettingsSlider.OnSliderValueChanged += hook_OnSliderValueChanged;
        }

        public MenuSlider( float defaultValue, float maxValue, float minValue, bool wholeNumbers, string settingName, string settingDescription, SubPanel panelLocation, bool showInPauseSettings, ValueChanged callback ) : base(defaultValue, settingName, settingDescription, panelLocation, showInPauseSettings, callback)
        {
            this.maxValue = maxValue;
            this.minValue = minValue;
            this.wholeNumbers = wholeNumbers;
            On.RoR2.UI.SettingsSlider.OnSliderValueChanged += hook_OnSliderValueChanged;
        }

        public delegate void SliderChanged( float newValue );
        public event SliderChanged OnSliderChanged;

        public override void SetValue( float newValue )
        {
            base.SetValue(newValue);
            if ( controller )
            {
                controller.valueText.text = Math.Round(value, 2).ToString();
                controller.slider.value = value;
            }
        }

        protected override void AddSettingField( Transform settingsPanelInstance )
        {
            GameObject buttonToInstantiate = settingsPanelInstance.Find("SafeArea/SubPanelArea/" + GetSubPanelName(SubPanel.Gameplay) + "/Scroll View/Viewport/VerticalLayout/SettingsEntryButton, Slider (Screen Shake Scale)").gameObject;
            Transform gameplaySubPanel = settingsPanelInstance.Find("SafeArea/SubPanelArea/" + GetSubPanelName(panelLocation) + "/Scroll View/Viewport/VerticalLayout");

            GameObject buttonCopy = UnityEngine.Object.Instantiate(buttonToInstantiate);
            buttonCopy.transform.SetParent(gameplaySubPanel);
            buttonCopy.name = "SettingsEntryButton, Slider (" + settingName + ")";
            controller = buttonCopy.GetComponent<SettingsSlider>();

            controller.minValue = minValue;
            controller.maxValue = maxValue;
            controller.slider.minValue = minValue;
            controller.slider.maxValue = maxValue;
            controller.slider.wholeNumbers = wholeNumbers;
            controller.settingName = settingName;
            controller.nameToken = token;
            controller.nameLabel.token = settingName;
            controller.originalValue = defaultValue.ToString();
            SetValue(value);
            HGButton button = controller.GetComponent<HGButton>();
            button.hoverToken = settingDescription;
            revertValue = value;
        }

        void hook_OnSliderValueChanged( On.RoR2.UI.SettingsSlider.orig_OnSliderValueChanged orig, SettingsSlider self, float newValue )
        {
            orig(self, newValue);
            if ( self.nameToken == token )
            {
                SetValue(newValue);
            }
        }
    }
}
