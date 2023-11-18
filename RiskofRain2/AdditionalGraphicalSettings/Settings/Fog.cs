using AdditionalGraphicalSettings.MenuAPI;

namespace AdditionalGraphicalSettings.Settings
{
    public class Fog : PostProcessingEffectSetting<RampFog>
    {
        public MenuSlider Intensity { get; }
        public MenuSlider EndColorRed { get; }
        public MenuSlider EndColorGreen { get; }
        public MenuSlider EndColorBlue { get; }
        public MenuSlider EndColorAlpha { get; }
        public MenuSlider MidColorRed { get; }
        public MenuSlider MidColorGreen { get; }
        public MenuSlider MidColorBlue { get; }
        public MenuSlider MidColorAlpha { get; }
        public MenuSlider StartColorRed { get; }
        public MenuSlider StartColorGreen { get; }
        public MenuSlider StartColorBlue { get; }
        public MenuSlider StartColorAlpha { get; }
        public MenuSlider One { get; }
        public MenuSlider Power { get; }
        public MenuSlider Zero { get; }
        public MenuSlider SkyboxStrength { get; }
        public Fog()
        {
            Effect.fogColorStart.value.a = 0f;

            Intensity = CreateMenuSliderWithResetToDefault(Effect.fogIntensity, 10, 0, false, "RampFog Intensity", string.Empty, true, ( float newValue ) =>
            {
                Effect.fogIntensity.value = newValue;
            });

            EndColorRed = CreateMenuSliderWithResetToDefault(Effect.fogColorEnd.value.r, 1, 0, false, "RampFog End Color Red", string.Empty, true, ( float newValue ) =>
            {
                Effect.fogColorEnd.value.r = newValue;
            });
            EndColorGreen = CreateMenuSliderWithResetToDefault(Effect.fogColorEnd.value.g, 1, 0, false, "RampFog End Color Green", string.Empty, true, ( float newValue ) =>
            {
                Effect.fogColorEnd.value.g = newValue;
            });
            EndColorBlue = CreateMenuSliderWithResetToDefault(Effect.fogColorEnd.value.b, 1, 0, false, "RampFog End Color Blue", string.Empty, true, ( float newValue ) =>
            {
                Effect.fogColorEnd.value.b = newValue;
            });
            EndColorAlpha = CreateMenuSliderWithResetToDefault(Effect.fogColorEnd.value.a, 1, 0, false, "RampFog End Color Alpha", string.Empty, true, ( float newValue ) =>
            {
                Effect.fogColorEnd.value.a = newValue;
            });

            MidColorRed = CreateMenuSliderWithResetToDefault(Effect.fogColorMid.value.r, 1, 0, false, "RampFog Mid Color Red", string.Empty, true, ( float newValue ) =>
            {
                Effect.fogColorMid.value.r = newValue;
            });
            MidColorGreen = CreateMenuSliderWithResetToDefault(Effect.fogColorMid.value.g, 1, 0, false, "RampFog Mid Color Green", string.Empty, true, ( float newValue ) =>
            {
                Effect.fogColorMid.value.g = newValue;
            });
            MidColorBlue = CreateMenuSliderWithResetToDefault(Effect.fogColorMid.value.b, 1, 0, false, "RampFog Mid Color Blue", string.Empty, true, ( float newValue ) =>
            {
                Effect.fogColorMid.value.b = newValue;
            });
            MidColorAlpha = CreateMenuSliderWithResetToDefault(Effect.fogColorMid.value.a, 1, 0, false, "RampFog Mid Color Alpha", string.Empty, true, ( float newValue ) =>
            {
                Effect.fogColorMid.value.a = newValue;
            });


            StartColorRed = CreateMenuSliderWithResetToDefault(Effect.fogColorStart.value.r, 1, 0, false, "RampFog Start Color Red", string.Empty, true, ( float newValue ) =>
            {
                Effect.fogColorStart.value.r = newValue;
            });
            StartColorGreen = CreateMenuSliderWithResetToDefault(Effect.fogColorStart.value.g, 1, 0, false, "RampFog Start Color Green", string.Empty, true, ( float newValue ) =>
            {
                Effect.fogColorStart.value.g = newValue;
            });
            StartColorBlue = CreateMenuSliderWithResetToDefault(Effect.fogColorStart.value.b, 1, 0, false, "RampFog Start Color Blue", string.Empty, true, ( float newValue ) =>
            {
                Effect.fogColorStart.value.b = newValue;
            });
            StartColorAlpha = CreateMenuSliderWithResetToDefault(Effect.fogColorStart.value.a, 1, 0, false, "RampFog Start Color Alpha", string.Empty, true, ( float newValue ) =>
            {
                Effect.fogColorStart.value.a = newValue;
            });

            One = CreateMenuSliderWithResetToDefault(Effect.fogOne, 10, 0, false, "RampFog One", string.Empty, true, ( float newValue ) =>
            {
                Effect.fogOne.value = newValue;
            });
            Power = CreateMenuSliderWithResetToDefault(Effect.fogPower, 5, 0, false, "RampFog Power", string.Empty, true, ( float newValue ) =>
            {
                Effect.fogPower.value = newValue;
            });
            Zero = CreateMenuSliderWithResetToDefault(Effect.fogZero, 0.9999999f, -5, false, "RampFog Zero", string.Empty, true, ( float newValue ) =>
            {
                Effect.fogZero.value = newValue;
            });
            SkyboxStrength = CreateMenuSliderWithResetToDefault(Effect.skyboxStrength, 1, 0, false, "RampFog Skybox Strength", string.Empty, true, ( float newValue ) =>
            {
                Effect.skyboxStrength.value = newValue;
            });
        }
    }
}
