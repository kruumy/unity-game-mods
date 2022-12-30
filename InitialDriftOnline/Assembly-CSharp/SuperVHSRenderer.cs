using SuperVHSCommons;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public sealed class SuperVHSRenderer : PostProcessEffectRenderer<SuperVHS>
{
	public override void Render(PostProcessRenderContext context)
	{
		PropertySheet propertySheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/SuperVHS"));
		propertySheet.properties.SetFloat("_AberrationStrength", base.settings.ColorBleeding);
		propertySheet.properties.SetFloat("_MutingStrength", base.settings.ColorMuteness);
		propertySheet.properties.SetFloat("_Brightness", base.settings.Brightness);
		propertySheet.properties.SetFloat("_Contrast", base.settings.Contrast);
		propertySheet.properties.SetFloat("_SharpenStrength", base.settings.SharpenStrength);
		propertySheet.properties.SetInt("_Grayscale", base.settings.Grayscale ? 1 : 0);
		propertySheet.properties.SetFloat("_JitterBaseStrength", base.settings.BaseJitterIntensity);
		propertySheet.properties.SetFloat("_JitterTemporalStrength", base.settings.TemporalJitterIntensity);
		propertySheet.properties.SetFloat("_JitterTapeAlignmentStrength", base.settings.TapeJitterIntensity);
		propertySheet.properties.SetFloat("_PoplineFrequency", base.settings.PoplineFrequency);
		propertySheet.properties.SetFloat("_BorderWidth", base.settings.BorderWidth);
		propertySheet.properties.SetFloat("_VerticalJitterIntensity", base.settings.VerticalJitterIntensity);
		propertySheet.properties.SetFloat("_VerticalScrollingSpeed", base.settings.VerticalScrollingSpeed);
		propertySheet.properties.SetFloat("_BottomTapeDistortion", base.settings.BottomTapeDistortionStrength);
		propertySheet.properties.SetFloat("_BottomTapeDistortionHeight", base.settings.BottomTapeDistortionHeight);
		propertySheet.properties.SetFloat("_BlueNoise", base.settings.BlueNoiseStrength);
		propertySheet.properties.SetFloat("_GeneralNoise", base.settings.GeneralNoiseStrength);
		propertySheet.properties.SetFloat("_SignalStatic", base.settings.SignalStatic);
		propertySheet.properties.SetTexture("_NoiseMask", (base.settings.DropoutNoiseMask.value == null) ? RuntimeUtilities.blackTexture : base.settings.DropoutNoiseMask.value);
		propertySheet.properties.SetVector("_NoiseMask_Offset", base.settings.DropoutNoiseMaskOffset);
		propertySheet.properties.SetVector("_NoiseMask_Tiling", base.settings.DropoutNoiseMaskTiling);
		propertySheet.properties.SetFloat("_ScanlineOpacity", base.settings.ScanlineOpacity);
		propertySheet.properties.SetColor("_ScanlineColor", base.settings.ScanlineColor);
		propertySheet.properties.SetInt("_RandomizeTapeDamage", base.settings.RandomizeTapeDamage ? 1 : 0);
		propertySheet.properties.SetFloat("_TapeDamageHeight", base.settings.TapeDamageHeight);
		propertySheet.properties.SetFloat("_TapeDamageSpeed", base.settings.TapeDamageSpeed);
		propertySheet.properties.SetFloat("_BorderBlur", base.settings.BorderBlur);
		propertySheet.properties.SetFloat("_Fisheye", base.settings.FisheyeIntensity);
		propertySheet.properties.SetColor("_FisheyeColor", base.settings.BlurColor);
		propertySheet.properties.SetInt("_Resolution", base.settings.ResolutionScale);
		UILayerer component = context.camera.GetComponent<UILayerer>();
		RenderTexture renderTexture = ((component != null && base.settings.EnableUILayering.value) ? component.CaptureUI() : null);
		if (renderTexture == null)
		{
			propertySheet.properties.SetTexture("_UITex", RuntimeUtilities.transparentTexture);
		}
		else
		{
			propertySheet.properties.SetTexture("_UITex", renderTexture);
		}
		propertySheet.properties.SetInt("_VCRAccurateUI", (base.settings.UILayeringMethod.value == LayeringMethod.VCRAccurate) ? 1 : 0);
		context.GetScreenSpaceTemporaryRT(context.command, 0);
		context.GetScreenSpaceTemporaryRT(context.command, 1);
		context.GetScreenSpaceTemporaryRT(context.command, 2);
		context.GetScreenSpaceTemporaryRT(context.command, 3);
		context.command.BlitFullscreenTriangle(context.source, 0, propertySheet, 0);
		context.command.BlitFullscreenTriangle(0, 1, propertySheet, 1);
		context.command.BlitFullscreenTriangle(1, 2, propertySheet, 2);
		context.command.BlitFullscreenTriangle(2, 3, propertySheet, 3);
		context.command.BlitFullscreenTriangle(3, context.destination, propertySheet, 4);
	}
}
