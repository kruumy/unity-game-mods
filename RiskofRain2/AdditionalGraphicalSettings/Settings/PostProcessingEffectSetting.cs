using AddFoVSettings;

namespace AdditionalGraphicalSettings.Settings
{
    public abstract class PostProcessingEffectSetting<T> : PostProcessingSetting where T : UnityEngine.Rendering.PostProcessing.PostProcessEffectSettings
    {
        public T Effect { get; }
        public MenuCheckbox Override { get; }
        public MenuCheckbox Enable { get; }
        public PostProcessingEffectSetting()
        {
            Effect = Volume.profile.AddSettings<T>();
            Effect.SetAllOverridesTo(false, false);
            Effect.active = false;
            Effect.enabled.value = false;

            Override = new MenuCheckbox(Effect.active, $"Override {typeof(T).Name}", string.Empty, SubPanel.Graphics, true, ( bool newValue ) =>
            {
                Effect.active = newValue;
                Effect.SetAllOverridesTo(newValue, false);
            });
            Enable = new MenuCheckbox(Effect.enabled.value, $"{typeof(T).Name} Enabled", string.Empty, SubPanel.Graphics, true, ( bool newValue ) =>
            {
                Effect.enabled.value = newValue;
            });
        }
    }
}
