using AdditionalGraphicalSettings.MenuAPI;
using System.Collections.Generic;

namespace AdditionalGraphicalSettings.Settings
{
    public abstract class Setting
    {
        public MenuButton ResetToDefault { get; }
        private List<IResetToDefault> Controls { get; } = new List<IResetToDefault>(2);
        public SubPanel TargetSubPanel { get; }

        public Setting( string settingsGeneralName, SubPanel subPanel )
        {
            TargetSubPanel = subPanel;
            ResetToDefault = new MenuButton($"Reset {settingsGeneralName} Settings To Default", string.Empty, TargetSubPanel, true, () =>
            {
                foreach ( var control in Controls )
                {
                    control.ResetToDefault();
                }
            });
        }

        protected MenuSlider CreateMenuSliderWithResetToDefault( float defaultValue, float maxValue, float minValue, bool wholeNumbers, string settingName, string settingDescription, bool showInPauseSettings, MenuSlider.ValueChanged callback )
        {
            MenuSlider ret = new(defaultValue, maxValue, minValue, wholeNumbers, settingName, settingDescription, TargetSubPanel, showInPauseSettings, callback);
            Controls.Add(ret);
            return ret;
        }
        protected MenuCheckbox CreateMenuCheckBoxWithResetToDefault( bool defaultValue, string settingName, string settingDescription, bool showInPauseSettings, MenuCheckbox.ValueChanged callback )
        {
            MenuCheckbox ret = new(defaultValue, settingName, settingDescription, TargetSubPanel, showInPauseSettings, callback);
            Controls.Add(ret);
            return ret;
        }
    }
}
