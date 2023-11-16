using System.Linq;
using UnityEngine.Rendering.PostProcessing;

namespace AdditionalGraphicalSettings.Settings
{
    public class ColorGrading : CheckBoxGraphicalSetting
    {
        public MenuSlider Contrast { get; }
        public MenuSlider GainRed { get; }
        public MenuSlider GainGreen { get; }
        public MenuSlider GainBlue { get; }
        public MenuSlider GainAlpha { get; }
        public MenuSlider HueShift { get; }
        public MenuSlider Saturation { get; }
        public MenuSlider Tint { get; }
        public MenuButton ResetToDefault { get; }
        private UnityEngine.Rendering.PostProcessing.ColorGrading InternalColorGrade => UnityEngine.Object.FindObjectsOfType<PostProcessVolume>().FirstOrDefault(p => p.name == "GlobalPostProcessVolume")?.profile?.GetSetting<UnityEngine.Rendering.PostProcessing.ColorGrading>();
        public ColorGrading() : base(true, "Color Grading", string.Empty)
        {
            Contrast = CreateSlider(42.2f, 100, -100, false, "Contrast", string.Empty, ( float newValue ) => { InternalColorGrade.contrast.value = newValue; InternalColorGrade.contrast.overrideState = true; });
            GainRed = CreateSlider(1, 10, 0, false, "Red Gain", string.Empty, ( float newValue ) => { InternalColorGrade.gain.value.x = newValue; InternalColorGrade.gain.overrideState = true; });
            GainGreen = CreateSlider(1, 10, 0, false, "Green Gain", string.Empty, ( float newValue ) => { InternalColorGrade.gain.value.y = newValue; InternalColorGrade.gain.overrideState = true; });
            GainBlue = CreateSlider(1, 10, 0, false, "Blue Gain", string.Empty, ( float newValue ) => { InternalColorGrade.gain.value.z = newValue; InternalColorGrade.gain.overrideState = true; });
            GainAlpha = CreateSlider(0, 10, -2, false, "Alpha Gain", string.Empty, ( float newValue ) => { InternalColorGrade.gain.value.w = newValue; InternalColorGrade.gain.overrideState = true; });
            HueShift = CreateSlider(0, 360, 0, true, "Hue Shift", string.Empty, ( float newValue ) => { InternalColorGrade.hueShift.value = newValue; InternalColorGrade.hueShift.overrideState = true; });
            Saturation = CreateSlider(-2.5f, 100, -100, true, "Saturation", string.Empty, ( float newValue ) => { InternalColorGrade.saturation.value = newValue; InternalColorGrade.saturation.overrideState = true; });
            Tint = CreateSlider(0, 100, -100, true, "Tint", string.Empty, ( float newValue ) => { InternalColorGrade.tint.value = newValue; InternalColorGrade.tint.overrideState = true; });
            ResetToDefault = new MenuButton("Reset Color Grading To Default", string.Empty, SubPanel.Graphics, true);
            ResetToDefault.OnButtonPressed += ResetToDefault_OnButtonPressed;
        }

        private void ResetToDefault_OnButtonPressed()
        {
            Contrast.SetValue(Contrast.defaultValue);
            GainRed.SetValue(GainRed.defaultValue);
            GainGreen.SetValue(GainGreen.defaultValue);
            GainBlue.SetValue(GainBlue.defaultValue);
            GainAlpha.SetValue(GainAlpha.defaultValue);
            HueShift.SetValue(HueShift.defaultValue);
            Saturation.SetValue(Saturation.defaultValue);
            Tint.SetValue(Tint.defaultValue);
            Log.LogInfo("Reset Color Grading Settings.");
        }

        protected override void OnCheckBoxChanged( bool newValue )
        {
            InternalColorGrade.active = newValue;
            InternalColorGrade.enabled.value = newValue;
            InternalColorGrade.enabled.overrideState = true;
        }
    }
}
