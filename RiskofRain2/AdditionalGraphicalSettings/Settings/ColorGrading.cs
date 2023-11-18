using AdditionalGraphicalSettings.MenuAPI;

namespace AdditionalGraphicalSettings.Settings
{
    public class ColorGrading : PostProcessingEffectSetting<UnityEngine.Rendering.PostProcessing.ColorGrading>
    {
        public MenuSlider Contrast { get; }
        public MenuSlider RedGain { get; }
        public MenuSlider BlueGain { get; }
        public MenuSlider GreenGain { get; }
        public MenuSlider Gain { get; }
        public MenuSlider RedGamma { get; }
        public MenuSlider BlueGamma { get; }
        public MenuSlider GreenGamma { get; }
        public MenuSlider Gamma { get; }
        public MenuSlider HueShift { get; }
        public MenuSlider Saturation { get; }
        public MenuSlider Temperature { get; }
        public MenuSlider Tint { get; }
        public ColorGrading()
        {
            Contrast = CreateMenuSliderWithResetToDefault(Effect.contrast, 100, -100, true, "Contrast", string.Empty, true, ( float newValue ) =>
            {
                Effect.contrast.value = newValue;
            });
            RedGain = CreateMenuSliderWithResetToDefault(Effect.gain.value.x, 10, 0, false, "Red Gain", string.Empty, true, ( float newValue ) =>
            {
                Effect.gain.value.x = newValue;
            });
            GreenGain = CreateMenuSliderWithResetToDefault(Effect.gain.value.y, 10, 0, false, "Green Gain", string.Empty, true, ( float newValue ) =>
            {
                Effect.gain.value.y = newValue;
            });
            BlueGain = CreateMenuSliderWithResetToDefault(Effect.gain.value.z, 10, 0, false, "Blue Gain", string.Empty, true, ( float newValue ) =>
            {
                Effect.gain.value.z = newValue;
            });
            Gain = CreateMenuSliderWithResetToDefault(Effect.gain.value.w, 10, -2, false, "Gain", string.Empty, true, ( float newValue ) =>
            {
                Effect.gain.value.w = newValue;
            });
            RedGamma = CreateMenuSliderWithResetToDefault(Effect.gamma.value.x, 10, 0, false, "Red Gamma", string.Empty, true, ( float newValue ) =>
            {
                Effect.gamma.value.x = newValue;
            });
            GreenGamma = CreateMenuSliderWithResetToDefault(Effect.gamma.value.y, 10, 0, false, "Green Gamma", string.Empty, true, ( float newValue ) =>
            {
                Effect.gamma.value.y = newValue;
            });
            BlueGamma = CreateMenuSliderWithResetToDefault(Effect.gamma.value.z, 10, 0, false, "Blue Gamma", string.Empty, true, ( float newValue ) =>
            {
                Effect.gamma.value.z = newValue;
            });
            Gamma = CreateMenuSliderWithResetToDefault(Effect.gamma.value.w, 10, -2, false, "Gamma", string.Empty, true, ( float newValue ) =>
            {
                Effect.gamma.value.w = newValue;
            });
            HueShift = CreateMenuSliderWithResetToDefault(Effect.hueShift, 360, 0, true, "Hue Shift", string.Empty, true, ( float newValue ) =>
            {
                Effect.hueShift.value = newValue;
            });
            Saturation = CreateMenuSliderWithResetToDefault(Effect.saturation, 100, -100, true, "Saturation", string.Empty, true, ( float newValue ) =>
            {
                Effect.saturation.value = newValue;
            });
            Temperature = CreateMenuSliderWithResetToDefault(Effect.temperature, 100, -100, true, "Temperature", string.Empty, true, ( float newValue ) =>
            {
                Effect.temperature.value = newValue;
            });
            Tint = CreateMenuSliderWithResetToDefault(Effect.tint, 100, -100, true, "Tint", string.Empty, true, ( float newValue ) =>
            {
                Effect.tint.value = newValue;
            });

        }
    }
}
