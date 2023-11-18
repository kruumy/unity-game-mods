using AdditionalGraphicalSettings.MenuAPI;
using System;

namespace AdditionalGraphicalSettings.Settings
{
    public class MotionBlur : PostProcessingEffectSetting<UnityEngine.Rendering.PostProcessing.MotionBlur>
    {
        public MenuSlider SampleCount { get; }
        public MenuSlider ShutterAngle { get; }
        public MotionBlur()
        {
            SampleCount = CreateMenuSliderWithResetToDefault(Effect.sampleCount, 64, 0, true, "MotionBlur Sample Count", string.Empty, true, ( float newValue ) =>
            {
                Effect.sampleCount.value = Convert.ToInt32(Math.Round(newValue));
            });
            ShutterAngle = CreateMenuSliderWithResetToDefault(Effect.shutterAngle, 720, 0, true, "MotionBlur Shutter Angle", string.Empty, true, ( float newValue ) =>
            {
                Effect.shutterAngle.value = newValue;
            });
        }
    }
}
