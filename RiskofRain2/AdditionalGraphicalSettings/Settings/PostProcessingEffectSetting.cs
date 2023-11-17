namespace AdditionalGraphicalSettings.Settings
{
    public abstract class PostProcessingEffectSetting<T> : PostProcessingSetting where T : UnityEngine.Rendering.PostProcessing.PostProcessEffectSettings
    {
        public T Effect { get; }
        public PostProcessingEffectSetting()
        {
            Effect = Volume.profile.AddSettings<T>();
            Effect.SetAllOverridesTo(false, false);
            Effect.active = false;
            Effect.enabled.value = false;
        }
    }
}
