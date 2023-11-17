namespace AdditionalGraphicalSettings.Settings
{
    public class Vignette : ForEachEffectCheckBoxGraphicalSetting<UnityEngine.Rendering.PostProcessing.Vignette>
    {
        public Vignette() : base(true, "Vignette", string.Empty)
        {
        }
    }
}
