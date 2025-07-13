using Il2Cpp;
using MelonLoader;

[assembly: MelonInfo(typeof(ResolutionScaleFix.Main), "ResolutionScaleFix", "1.0.0", "kruumy")]
[assembly: MelonGame("Thunderful", "Wavetale")]
namespace ResolutionScaleFix
{
    public class Main : MelonMod
    {
        public override void OnSceneWasLoaded( int buildIndex, string sceneName )
        {
            RenderScaleSetter? renderScaleSetter = UnityEngine.Object.FindObjectOfType<RenderScaleSetter>();
            if ( renderScaleSetter == null )
            {
                MelonLoader.MelonLogger.Warning("RenderScaleSetter not found");
                return;
            }

            renderScaleSetter.scaleRange = new FloatRange(renderScaleSetter.scaleRange.max, renderScaleSetter.scaleRange.max);
        }
    }
}
