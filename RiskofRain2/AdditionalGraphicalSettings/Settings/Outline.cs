using AdditionalGraphicalSettings.MenuAPI;

namespace AdditionalGraphicalSettings.Settings
{
    public class Outline : PostProcessingEffectSetting<SobelOutline>
    {
        public MenuSlider Intensity { get; }
        public MenuSlider Scale { get; }
        public Outline()
        {
            Intensity = CreateMenuSliderWithResetToDefault(Effect.outlineIntensity, 100, 0, true, "SobelOutline Intensity", string.Empty, true, ( float newValue ) =>
            {
                Effect.outlineIntensity.value = newValue;
            });
            Scale = CreateMenuSliderWithResetToDefault(Effect.outlineScale, 100, 0, false, "SobelOutline Scale", string.Empty, true, ( float newValue ) =>
            {
                Effect.outlineScale.value = newValue;
            });
        }
    }
}
