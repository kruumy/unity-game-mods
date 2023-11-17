using AdditionalGraphicalSettings.MenuAPI;

namespace AdditionalGraphicalSettings.Settings
{
    public class Outline : PostProcessingEffectSetting<SobelOutline>
    {
        public MenuSlider Intensity { get; }
        public MenuSlider Scale { get; }
        public Outline()
        {
            Intensity = new MenuSlider(Effect.outlineIntensity, 100, 0, true, "SobelOutline Intensity", string.Empty, SubPanel.Graphics, true, ( float newValue ) =>
            {
                Effect.outlineIntensity.value = newValue;
            });
            Scale = new MenuSlider(Effect.outlineScale, 100, 0, false, "SobelOutline Scale", string.Empty, SubPanel.Graphics, true, ( float newValue ) =>
            {
                Effect.outlineScale.value = newValue;
            });
        }
    }
}
