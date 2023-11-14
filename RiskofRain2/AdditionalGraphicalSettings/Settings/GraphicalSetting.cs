using AddFoVSettings;
using System.Collections.Generic;
using static AddFoVSettings.MenuCheckbox;
using static AddFoVSettings.MenuSlider;

namespace AdditionalGraphicalSettings.Settings
{
    public abstract class GraphicalSetting
    {
        protected GraphicalSetting()
        {
            On.RoR2.CameraRigController.OnEnable += ( On.RoR2.CameraRigController.orig_OnEnable orig, RoR2.CameraRigController self ) =>
            {
                orig.Invoke(self);
                OnCameraSpawn();
            };
        }
        private List<KeyValuePair<MenuCheckbox, CheckboxChanged>> CheckBoxCallBacks { get; } = new List<KeyValuePair<MenuCheckbox, CheckboxChanged>>();
        private List<KeyValuePair<MenuSlider, SliderChanged>> SliderCallBacks { get; } = new List<KeyValuePair<MenuSlider, SliderChanged>>();
        protected virtual void OnCameraSpawn()
        {
            foreach ( var checkbox in CheckBoxCallBacks )
            {
                checkbox.Value.Invoke(checkbox.Key.GetValue());
            }
            foreach ( var slider in SliderCallBacks )
            {
                slider.Value.Invoke(slider.Key.GetValue());
            }
        }

        protected MenuCheckbox CreateCheckBox( bool defaultValue, string settingName, string settingDescription, CheckboxChanged onCheckBoxChanged )
        {
            var checkBox = new MenuCheckbox(defaultValue, settingName, settingDescription, SubPanel.Graphics, true);
            checkBox.OnCheckboxChanged += onCheckBoxChanged;
            CheckBoxCallBacks.Add(new KeyValuePair<MenuCheckbox, CheckboxChanged>(checkBox, onCheckBoxChanged));
            return checkBox;
        }
        protected MenuSlider CreateSlider( float defaultValue, float maxValue, float minValue, bool wholeNumbers, string settingName, string settingDescription, SliderChanged onValueChanged )
        {
            var slider = new MenuSlider(defaultValue, maxValue, minValue, wholeNumbers, settingName, settingDescription, SubPanel.Graphics, true);
            slider.OnSliderChanged += onValueChanged;
            SliderCallBacks.Add(new KeyValuePair<MenuSlider, SliderChanged>(slider, onValueChanged));
            return slider;
        }
    }
}
