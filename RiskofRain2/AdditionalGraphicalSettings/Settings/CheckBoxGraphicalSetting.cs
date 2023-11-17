using AddFoVSettings;

namespace AdditionalGraphicalSettings.Settings
{
    public abstract class CheckBoxGraphicalSetting : GraphicalSetting
    {
        public MenuCheckbox CheckBox { get; }
        protected CheckBoxGraphicalSetting( bool defaultValue, string settingName, string settingDescription )
        {
            CheckBox = CreateCheckBox(defaultValue, settingName, settingDescription, OnCheckBoxChanged);
        }

        protected abstract void OnCheckBoxChanged( bool newValue );
    }
}
