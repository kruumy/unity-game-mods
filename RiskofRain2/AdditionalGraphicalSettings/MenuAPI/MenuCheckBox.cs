using RoR2.UI;
using System;
using UnityEngine;

namespace AdditionalGraphicalSettings.MenuAPI
{
    public class MenuCheckbox : MenuControl<bool, CarouselController>
    {
        public MenuCheckbox( bool defaultValue, string settingName, string settingDescription, SubPanel panelLocation, bool showInPauseSettings = true ) : base(defaultValue, settingName, settingDescription, panelLocation, showInPauseSettings)
        {
            On.RoR2.UI.CarouselController.BoolCarousel += hook_BoolCarousel;
        }


        public MenuCheckbox( bool defaultValue, string settingName, string settingDescription, SubPanel panelLocation, bool showInPauseSettings, ValueChanged callback ) : base(defaultValue, settingName, settingDescription, panelLocation, showInPauseSettings, callback)
        {
            On.RoR2.UI.CarouselController.BoolCarousel += hook_BoolCarousel;
        }

        bool StringToBool( string s )
        {
            if ( s == null || s == "" )
            {
                return false;
            }
            return Convert.ToBoolean(int.Parse(s));
        }
        protected override void AddSettingField( Transform settingsPanelInstance )
        {
            GameObject buttonToInstantiate = settingsPanelInstance.Find("SafeArea/SubPanelArea/" + GetSubPanelName(SubPanel.Gameplay) + "/Scroll View/Viewport/VerticalLayout/SettingsEntryButton, Bool (Screen Distortion)").gameObject;
            Transform gameplaySubPanel = settingsPanelInstance.Find("SafeArea/SubPanelArea/" + GetSubPanelName(panelLocation) + "/Scroll View/Viewport/VerticalLayout");

            GameObject buttonCopy = UnityEngine.Object.Instantiate(buttonToInstantiate);
            buttonCopy.transform.SetParent(gameplaySubPanel);
            buttonCopy.name = "SettingsEntryButton, Bool (" + settingName + ")";
            controller = buttonCopy.GetComponent<CarouselController>();
            bool current = Convert.ToBoolean(controller.selectionIndex);

            controller.settingName = settingName;
            controller.nameToken = token;
            controller.nameLabel.token = settingName;
            controller.originalValue = defaultValue.ToString();

            if ( current != value )
            {
                controller.BoolCarousel();
            }

            controller.GetComponent<HGButton>().hoverToken = settingDescription;
            revertValue = value;
        }

        public override void SetValue( bool newValue )
        {
            if ( controller != null && value != newValue )
            {
                controller.BoolCarousel();
            }
            else
            {
                base.SetValue(newValue);
            }
        }

        void hook_BoolCarousel( On.RoR2.UI.CarouselController.orig_BoolCarousel orig, CarouselController self )
        {
            orig(self);
            if ( self.nameToken == token )
            {
                base.SetValue(Convert.ToBoolean(self.selectionIndex));
            }
        }
    }
}
