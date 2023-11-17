using AddFoVSettings;
using System;

namespace AdditionalGraphicalSettings.Settings
{
    public class MotionBlur : PostProcessingEffectSetting<UnityEngine.Rendering.PostProcessing.MotionBlur>
    {
        public MenuCheckbox Override { get; }
        public MenuCheckbox Enable { get; }
        public MenuSlider SampleCount { get; }
        public MenuSlider ShutterAngle { get; }
        public MotionBlur()
        {
            Override = new MenuCheckbox(Effect.active, "Override MotionBlur", string.Empty, SubPanel.Graphics, true, ( bool newValue ) =>
            {
                Effect.active = newValue;
                Effect.SetAllOverridesTo(newValue, false);
            });
            Enable = new MenuCheckbox(Effect.enabled.value, "MotionBlur Enabled", string.Empty, SubPanel.Graphics, true, ( bool newValue ) =>
            {
                Effect.enabled.value = newValue;
            });
            SampleCount = new MenuSlider(Effect.sampleCount, 64, 0, true, "MotionBlur Sample Count", string.Empty, SubPanel.Graphics, true, ( float newValue ) =>
            {
                Effect.sampleCount.value = Convert.ToInt32(Math.Round(newValue));
            });
            ShutterAngle = new MenuSlider(Effect.sampleCount, 720, 0, true, "MotionBlur Shutter Angle", string.Empty, SubPanel.Graphics, true, ( float newValue ) =>
            {
                Effect.shutterAngle.value = newValue;
            });
        }
    }
}
