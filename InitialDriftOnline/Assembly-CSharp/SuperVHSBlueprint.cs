using SuperVHSCommons;
using UnityEngine;

public class SuperVHSBlueprint
{
	public float ColorBleeding = 0.264f;

	public float ColorMuteness = 1f;

	public float SharpenStrength;

	public float Brightness;

	public float Contrast;

	public bool Grayscale;

	public float BaseJitterIntensity;

	public float TemporalJitterIntensity = 0.023f;

	public float TapeJitterIntensity;

	public float PoplineFrequency;

	public float VerticalJitterIntensity;

	public float VerticalScrollingSpeed;

	public float BorderWidth = 1.34f;

	public float BottomTapeDistortionHeight = 0.064f;

	public float BottomTapeDistortionStrength = 1.3f;

	public float GeneralNoiseStrength = 0.13f;

	public float BlueNoiseStrength = 0.18f;

	public float SignalStatic;

	public Texture2D DropoutNoiseMask;

	public Vector2 DropoutNoiseMaskOffset = Vector2.zero;

	public Vector2 DropoutNoiseMaskTiling = Vector2.one;

	public float ScanlineOpacity;

	public Color ScanlineColor = Color.black;

	public bool RandomizeTapeDamage = true;

	public float TapeDamageHeight;

	public float TapeDamageSpeed = 0.5f;

	public float BorderBlur;

	public float FisheyeIntensity;

	public Color BlurColor = Color.black;

	public bool EnableResolutionScaling;

	public int Resolution = 1;

	public LayeringMethod UILayeringMethod = LayeringMethod.VCRAccurate;

	public Canvas TargetCanvas;

	public bool EnableColorEffects = true;

	public bool EnablePlaybackEffects = true;

	public bool EnableHeadSwitchingNoise = true;

	public bool EnableAnalogNoiseEffects = true;

	public bool EnableScanlines = true;

	public bool EnableTapeDamageEffects = true;

	public bool EnableDisplayEffects = true;

	public bool EnableUILayering;
}
