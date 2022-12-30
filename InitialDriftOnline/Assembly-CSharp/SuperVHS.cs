using System;
using SuperVHSCommons;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(SuperVHSRenderer), PostProcessEvent.AfterStack, "Custom/SuperVHS", true)]
public sealed class SuperVHS : PostProcessEffectSettings
{
	[Range(0f, 1f)]
	public FloatParameter ColorBleeding = new FloatParameter
	{
		value = 0.14f
	};

	[Range(0f, 1f)]
	public FloatParameter ColorMuteness = new FloatParameter
	{
		value = 0.272f
	};

	[Range(0f, 1f)]
	public FloatParameter SharpenStrength = new FloatParameter
	{
		value = 0.294f
	};

	[Range(-1f, 1f)]
	public FloatParameter Brightness = new FloatParameter
	{
		value = -0.15f
	};

	[Range(-1f, 1f)]
	public FloatParameter Contrast = new FloatParameter
	{
		value = -0.159f
	};

	public BoolParameter Grayscale = new BoolParameter
	{
		value = false
	};

	[Range(0f, 1f)]
	public FloatParameter BaseJitterIntensity = new FloatParameter
	{
		value = 0f
	};

	[Range(0f, 5f)]
	public FloatParameter TemporalJitterIntensity = new FloatParameter
	{
		value = 0.023f
	};

	[Range(0f, 1f)]
	public FloatParameter TapeJitterIntensity = new FloatParameter
	{
		value = 0f
	};

	[Range(0f, 1f)]
	public FloatParameter PoplineFrequency = new FloatParameter
	{
		value = 0.047f
	};

	[Range(0f, 5f)]
	public FloatParameter VerticalJitterIntensity = new FloatParameter
	{
		value = 0f
	};

	[Range(0f, 1f)]
	public FloatParameter VerticalScrollingSpeed = new FloatParameter
	{
		value = 0f
	};

	[Range(0f, 3f)]
	public FloatParameter BorderWidth = new FloatParameter
	{
		value = 1.34f
	};

	[Range(0f, 1f)]
	public FloatParameter BottomTapeDistortionHeight = new FloatParameter
	{
		value = 0.096f
	};

	[Range(0f, 3f)]
	public FloatParameter BottomTapeDistortionStrength = new FloatParameter
	{
		value = 1f
	};

	[Range(0f, 3f)]
	public FloatParameter GeneralNoiseStrength = new FloatParameter
	{
		value = 0.4f
	};

	[Range(0f, 3f)]
	public FloatParameter BlueNoiseStrength = new FloatParameter
	{
		value = 0.09f
	};

	[Range(0f, 1f)]
	public FloatParameter SignalStatic = new FloatParameter
	{
		value = 0f
	};

	public TextureParameter DropoutNoiseMask = new TextureParameter
	{
		defaultState = TextureParameterDefault.Black,
		value = null
	};

	[DisplayName("Offset")]
	public Vector2Parameter DropoutNoiseMaskOffset = new Vector2Parameter
	{
		value = Vector2.zero
	};

	[DisplayName("Tiling")]
	public Vector2Parameter DropoutNoiseMaskTiling = new Vector2Parameter
	{
		value = Vector2.one
	};

	[Range(0f, 1f)]
	public FloatParameter ScanlineOpacity = new FloatParameter
	{
		value = 0f
	};

	public ColorParameter ScanlineColor = new ColorParameter
	{
		value = Color.black
	};

	public BoolParameter RandomizeTapeDamage = new BoolParameter
	{
		value = true
	};

	[Range(0f, 1f)]
	public FloatParameter TapeDamageHeight = new FloatParameter
	{
		value = 0f
	};

	public FloatParameter TapeDamageSpeed = new FloatParameter
	{
		value = 0.5f
	};

	[Range(0f, 1f)]
	public FloatParameter BorderBlur = new FloatParameter
	{
		value = 0f
	};

	[Range(0f, 1f)]
	public FloatParameter FisheyeIntensity = new FloatParameter
	{
		value = 0f
	};

	public ColorParameter BlurColor = new ColorParameter
	{
		value = Color.black
	};

	public IntParameter ResolutionScale = new IntParameter
	{
		value = 1
	};

	public BoolParameter EnableUILayering = new BoolParameter
	{
		value = false
	};

	public LayeringModeParameter UILayeringMethod = new LayeringModeParameter
	{
		value = LayeringMethod.VCRAccurate
	};
}
