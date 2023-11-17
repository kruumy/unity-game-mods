using RoR2.UI;
using UnityEngine;
using UnityEngine.Events;

namespace AdditionalGraphicalSettings.MenuAPI
{
    public class MenuButton : MenuBase
    {
        public MenuButton( string settingName, string settingDescription, SubPanel panelLocation, bool showInPauseSettings = true ) : base(settingName, settingDescription, panelLocation, showInPauseSettings)
        {
        }

        public event UnityAction OnButtonPressed;

        protected override void AddSettingField( Transform settingsPanelInstance )
        {
            GameObject buttonToInstantiate = settingsPanelInstance.Find("SafeArea/SubPanelArea/" + GetSubPanelName(SubPanel.Gameplay) + "/Scroll View/Viewport/VerticalLayout/SettingsEntryButton, Bool (Screen Distortion)").gameObject;
            Transform gameplaySubPanel = settingsPanelInstance.Find("SafeArea/SubPanelArea/" + GetSubPanelName(panelLocation) + "/Scroll View/Viewport/VerticalLayout");

            GameObject buttonCopy = UnityEngine.Object.Instantiate(buttonToInstantiate);
            buttonCopy.transform.SetParent(gameplaySubPanel);
            buttonCopy.name = "SettingsEntryButton, (" + settingName + ")";
            buttonCopy.GetComponent<CarouselController>().nameLabel.token = settingName;
            UnityEngine.Object.Destroy(buttonCopy.GetComponent<CarouselController>());
            UnityEngine.Object.Destroy(buttonCopy.transform.FindChild("CarouselRect").gameObject);
            HGButton hgButton = buttonCopy.GetComponent<HGButton>();
            hgButton.onClick.RemoveAllListeners();
            hgButton.onClick.AddListener(OnButtonPressed);
            hgButton.disableGamepadClick = false;
            hgButton.disablePointerClick = false;
        }


    }
}
