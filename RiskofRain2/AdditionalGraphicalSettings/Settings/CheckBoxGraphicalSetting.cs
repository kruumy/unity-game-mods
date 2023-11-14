using AddFoVSettings;

namespace AdditionalGraphicalSettings.Settings
{
    public abstract class CheckBoxGraphicalSetting : GraphicalSetting
    {
        public MenuCheckbox CheckBox { get; }
        protected CheckBoxGraphicalSetting( bool defaultValue, string settingName, string settingDescription )
        {
            CheckBox = new MenuCheckbox(defaultValue, settingName, settingDescription, SubPanel.Graphics, true);
            CheckBox.OnCheckboxChanged += OnCheckBoxChanged;
        }

        protected override void OnCameraSpawn()
        {
            base.OnCameraSpawn();
            OnCheckBoxChanged(CheckBox.GetValue());
        }

        protected abstract void OnCheckBoxChanged( bool newValue );
    }
}
