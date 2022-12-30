using SuperVHSCommons;
using UnityEngine;

public class SuperVHSFilter : MonoBehaviour
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

	public float ScanlineOpacity = 1f;

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

	private int previousCullingMask;

	private Camera thisCamera;

	private Material blitMaterial;

	private RenderTexture uiTargetTexture;

	private Camera uiCamera;

	private void Start()
	{
		thisCamera = GetComponent<Camera>();
		blitMaterial = new Material(Shader.Find("Hidden/SuperVHSFilter"));
	}

	private void OnEnable()
	{
		GameObject gameObject = new GameObject("SuperVHSUICamera", typeof(Camera));
		uiCamera = gameObject.GetComponent<Camera>();
		uiTargetTexture = new RenderTexture(uiCamera.pixelWidth, uiCamera.pixelHeight, 1);
		gameObject.hideFlags = HideFlags.HideAndDontSave;
		uiCamera.depth = -128f;
		uiCamera.clearFlags = CameraClearFlags.Color;
		uiCamera.backgroundColor = new Color(1f, 1f, 1f, 0f);
		uiCamera.cullingMask = LayerMask.GetMask("UI");
		uiCamera.targetTexture = uiTargetTexture;
		if (TargetCanvas != null)
		{
			TargetCanvas.renderMode = RenderMode.ScreenSpaceCamera;
			TargetCanvas.worldCamera = uiCamera;
		}
	}

	private void OnDisable()
	{
		uiTargetTexture.Release();
		Object.DestroyImmediate(uiCamera.gameObject);
	}

	public RenderTexture CaptureUI()
	{
		if (uiCamera != null)
		{
			return uiCamera.activeTexture;
		}
		return null;
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		float num = Mathf.Clamp01(ColorBleeding);
		float num2 = Mathf.Clamp01(ColorMuteness);
		float num3 = Mathf.Clamp01(SharpenStrength);
		float num4 = Mathf.Clamp(Brightness, -1f, 1f);
		float num5 = Mathf.Clamp(Contrast, -1f, 1f);
		float num6 = Mathf.Clamp01(BaseJitterIntensity);
		float num7 = Mathf.Clamp(TemporalJitterIntensity, 0f, 5f);
		float num8 = Mathf.Clamp01(TapeJitterIntensity);
		float num9 = Mathf.Clamp(PoplineFrequency, 0f, 5f);
		float num10 = Mathf.Clamp01(VerticalJitterIntensity);
		float num11 = Mathf.Clamp01(VerticalScrollingSpeed);
		float num12 = Mathf.Clamp(BorderWidth, 0f, 3f);
		float num13 = Mathf.Clamp01(BottomTapeDistortionHeight);
		float num14 = Mathf.Clamp(BottomTapeDistortionStrength, 0f, 3f);
		float num15 = Mathf.Clamp(GeneralNoiseStrength, 0f, 3f);
		float num16 = Mathf.Clamp(BlueNoiseStrength, 0f, 3f);
		float num17 = Mathf.Clamp01(SignalStatic);
		float num18 = Mathf.Clamp01(ScanlineOpacity);
		float num19 = Mathf.Clamp01(TapeDamageHeight);
		float num20 = Mathf.Clamp01(BorderBlur);
		float num21 = Mathf.Clamp01(FisheyeIntensity);
		int value = Mathf.Clamp(Resolution, 1, int.MaxValue);
		blitMaterial.SetFloat("_AberrationStrength", EnableColorEffects ? num : 0f);
		blitMaterial.SetFloat("_MutingStrength", EnableColorEffects ? num2 : 0f);
		blitMaterial.SetFloat("_SharpenStrength", EnableColorEffects ? num3 : 0f);
		blitMaterial.SetFloat("_Brightness", EnableColorEffects ? num4 : 0f);
		blitMaterial.SetFloat("_Contrast", EnableColorEffects ? num5 : 0f);
		blitMaterial.SetInt("_Grayscale", EnableColorEffects ? (Grayscale ? 1 : 0) : 0);
		blitMaterial.SetFloat("_JitterBaseStrength", EnablePlaybackEffects ? num6 : 0f);
		blitMaterial.SetFloat("_JitterTemporalStrength", EnablePlaybackEffects ? num7 : 0f);
		blitMaterial.SetFloat("_JitterTapeAlignmentStrength", EnablePlaybackEffects ? num8 : 0f);
		blitMaterial.SetFloat("_PoplineFrequency", EnablePlaybackEffects ? num9 : 0f);
		blitMaterial.SetFloat("_BorderWidth", EnablePlaybackEffects ? num12 : 0f);
		blitMaterial.SetFloat("_VerticalJitterIntensity", EnablePlaybackEffects ? num10 : 0f);
		blitMaterial.SetFloat("_VerticalScrollingSpeed", EnablePlaybackEffects ? num11 : 0f);
		blitMaterial.SetFloat("_BottomTapeDistortion", EnableHeadSwitchingNoise ? num14 : 0f);
		blitMaterial.SetFloat("_BottomTapeDistortionHeight", EnableHeadSwitchingNoise ? num13 : 0f);
		blitMaterial.SetFloat("_BlueNoise", EnableAnalogNoiseEffects ? num16 : 0f);
		blitMaterial.SetFloat("_GeneralNoise", EnableAnalogNoiseEffects ? num15 : 0f);
		blitMaterial.SetFloat("_SignalStatic", EnableAnalogNoiseEffects ? num17 : 0f);
		blitMaterial.SetTexture("_NoiseMask", EnableAnalogNoiseEffects ? DropoutNoiseMask : null);
		blitMaterial.SetTextureOffset("_NoiseMask", DropoutNoiseMaskOffset);
		blitMaterial.SetTextureScale("_NoiseMask", DropoutNoiseMaskTiling);
		blitMaterial.SetFloat("_ScanlineOpacity", (EnableScanlines && EnableResolutionScaling) ? num18 : 0f);
		blitMaterial.SetColor("_ScanlineColor", ScanlineColor);
		blitMaterial.SetInt("_RandomizeTapeDamage", RandomizeTapeDamage ? 1 : 0);
		blitMaterial.SetFloat("_TapeDamageHeight", EnableTapeDamageEffects ? num19 : 0f);
		blitMaterial.SetFloat("_TapeDamageSpeed", TapeDamageSpeed);
		if (EnableResolutionScaling && EnableDisplayEffects)
		{
			blitMaterial.SetInt("_Resolution", value);
		}
		else
		{
			blitMaterial.SetInt("_Resolution", 1);
		}
		blitMaterial.SetFloat("_Fisheye", EnableDisplayEffects ? num21 : 0f);
		blitMaterial.SetFloat("_BorderBlur", EnableDisplayEffects ? num20 : 0f);
		blitMaterial.SetColor("_FisheyeColor", EnableDisplayEffects ? BlurColor : Color.black);
		blitMaterial.SetInt("_VCRAccurateUI", (UILayeringMethod == LayeringMethod.VCRAccurate) ? 1 : 0);
		RenderTexture value2 = (EnableUILayering ? CaptureUI() : null);
		blitMaterial.SetTexture("_UITex", value2);
		RenderTexture temporary = RenderTexture.GetTemporary(source.descriptor);
		Graphics.Blit(source, temporary, blitMaterial, 0);
		Graphics.Blit(temporary, temporary, blitMaterial, 1);
		Graphics.Blit(temporary, temporary, blitMaterial, 2);
		Graphics.Blit(temporary, temporary, blitMaterial, 3);
		Graphics.Blit(temporary, destination, blitMaterial, 4);
		RenderTexture.ReleaseTemporary(temporary);
	}
}
