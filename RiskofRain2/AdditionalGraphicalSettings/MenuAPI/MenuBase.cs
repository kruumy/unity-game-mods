using UnityEngine;

namespace AdditionalGraphicalSettings.MenuAPI
{
    public abstract class MenuBase
    {
        public string settingName;
        public string settingDescription;
        public SubPanel panelLocation;
        public bool showInPauseSettings;
        protected string GetSubPanelName( SubPanel panel )
        {
            return panel switch
            {
                SubPanel.Gameplay => "SettingsSubPanel, Gameplay",
                SubPanel.KeyboardControls => "SettingsSubPanel, Controls (M&KB)",
                SubPanel.GamepadControls => "SettingsSubPanel, Controls (Gamepad)",
                SubPanel.Audio => "SettingsSubPanel, Audio",
                SubPanel.Video => "SettingsSubPanel, Video",
                SubPanel.Graphics => "SettingsSubPanel, Graphics",
                _ => "",
            };
        }
        public MenuBase( string settingName, string settingDescription, SubPanel panelLocation, bool showInPauseSettings = true )
        {
            this.settingName = settingName;
            this.settingDescription = settingDescription;
            this.panelLocation = panelLocation;
            this.showInPauseSettings = showInPauseSettings;

            On.RoR2.UI.MainMenu.SubmenuMainMenuScreen.OnEnter += ( orig, self, mainMenuController ) =>
            {
                orig(self, mainMenuController);
                AddSettingField(self.submenuPanelInstance.transform);
            };
            On.RoR2.UI.PauseScreenController.OpenSettingsMenu += ( orig, self ) =>
            {
                orig(self);
                if ( showInPauseSettings )
                {
                    AddSettingField(self.submenuObject.transform);
                }
            };
        }

        protected abstract void AddSettingField( Transform settingsPanelInstance );
    }
}
