using System;
using System.Collections.Generic;
using MadGoat.Core.Utils;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace MadGoat.SSAA;

[RequireComponent(typeof(Camera))]
public class MadGoatSSAA_VR : MadGoatSSAA
{
	private List<XRDisplaySubsystem> vrDisplays = new List<XRDisplaySubsystem>();

	public PostAntiAliasingMode SsaaUltraOld { get; set; }

	private float VRCachedRenderScale { get; set; }

	private void Start()
	{
		if (!base.Initialized)
		{
			OnInitializeProps();
		}
	}

	private void OnEnable()
	{
		if (!VRDeviceAnyActive())
		{
			throw new Exception("VRDevice not present or not detected");
		}
		VRDeviceCacheRes();
		if (!base.Initialized)
		{
			Start();
		}
		else if (base.Pipeline == RenderPipelineUtils.PipelineType.BuiltInPipeline)
		{
			SetupDownsamplerCommandBuffer();
		}
		StartCoroutine(UpdateAdaptiveRes());
	}

	private void Update()
	{
		if (base.PerfSampler != null)
		{
			base.PerfSampler.Update();
		}
		else
		{
			base.PerfSampler = new SsaaFramerateSampler();
		}
		if (base.Pipeline == RenderPipelineUtils.PipelineType.BuiltInPipeline && base.Initialized)
		{
			ChangeMaterial(base.DownsamplerFilter);
			UpdateDownsamplerCommandBuffer();
		}
	}

	private void OnDisable()
	{
		VRDeviceUpdate(VRCachedRenderScale);
		if (base.Pipeline == RenderPipelineUtils.PipelineType.BuiltInPipeline)
		{
			ClearDownsamplerCommandBuffer();
		}
	}

	public override void OnBeginCameraRender(Camera cam)
	{
		if (!(cam != currentCamera) && base.enabled && base.Initialized && VRDeviceAnyActive())
		{
			VRDeviceUpdate(base.InternalRenderMultiplier);
		}
	}

	protected override void OnInitialize()
	{
		if (currentCamera == null)
		{
			currentCamera = GetComponent<Camera>();
		}
		base.MaterialBicubic.SetOverrideTag("RenderType", "Opaque");
		base.MaterialBicubic.SetInt("_SrcBlend", 1);
		base.MaterialBicubic.SetInt("_DstBlend", 0);
		base.MaterialBicubic.SetInt("_ZWrite", 1);
		base.MaterialBicubic.renderQueue = -1;
		base.MaterialBilinear.SetOverrideTag("RenderType", "Opaque");
		base.MaterialBilinear.SetInt("_SrcBlend", 1);
		base.MaterialBilinear.SetInt("_DstBlend", 0);
		base.MaterialBilinear.SetInt("_ZWrite", 1);
		base.MaterialBilinear.renderQueue = -1;
		base.MaterialDefault.SetOverrideTag("RenderType", "Opaque");
		base.MaterialDefault.SetInt("_SrcBlend", 1);
		base.MaterialDefault.SetInt("_DstBlend", 0);
		base.MaterialDefault.SetInt("_ZWrite", 1);
		base.MaterialDefault.renderQueue = -1;
		base.MaterialCurrent = base.MaterialDefault;
		base.MaterialOld = base.MaterialCurrent;
		if (base.Pipeline == RenderPipelineUtils.PipelineType.BuiltInPipeline)
		{
			SetupDownsamplerCommandBuffer();
		}
		base.Initialized = true;
	}

	protected void ChangeMaterial(Filter Type)
	{
		switch (Type)
		{
		case Filter.POINT:
			base.MaterialCurrent = base.MaterialDefault;
			break;
		case Filter.BILINEAR:
			base.MaterialCurrent = base.MaterialBilinear;
			break;
		case Filter.BICUBIC:
			base.MaterialCurrent = base.MaterialBicubic;
			break;
		}
		if ((!base.DownsamplerEnabled || base.InternalRenderMultiplier == 1f) && base.MaterialCurrent != base.MaterialDefault)
		{
			base.MaterialCurrent = base.MaterialDefault;
		}
		if ((base.MaterialCurrent != base.MaterialOld || SsaaUltraOld != base.PostAntiAliasing) && VRDeviceAnyActive())
		{
			SsaaUltraOld = base.PostAntiAliasing;
			base.MaterialOld = base.MaterialCurrent;
			ClearDownsamplerCommandBuffer();
			SetupDownsamplerCommandBuffer();
		}
	}

	protected void SetupDownsamplerCommandBuffer()
	{
		if (base.CbDownsampler == null)
		{
			base.CbDownsampler = new CommandBuffer();
		}
		if (VRDeviceAnyActive() && new List<CommandBuffer>(currentCamera.GetCommandBuffers(CameraEvent.AfterImageEffects)).Find((CommandBuffer x) => x.name == "SSAA_VR_APPLY") == null)
		{
			if ((bool)base.RtDownsampler)
			{
				base.RtDownsampler.Release();
			}
			base.RtDownsampler = new RenderTexture(1024, 1024, 24, base.InternalTextureFormat);
			RenderTargetIdentifier renderTargetIdentifier = new RenderTargetIdentifier(base.RtDownsampler);
			base.RtDownsampler.vrUsage = XRSettings.eyeTextureDesc.vrUsage;
			base.CbDownsampler.Clear();
			base.CbDownsampler.name = "SSAA_VR_APPLY";
			base.CbDownsampler.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
			base.CbDownsampler.Blit(BuiltinRenderTextureType.CameraTarget, renderTargetIdentifier, base.MaterialCurrent, 0);
			base.CbDownsampler.Blit(renderTargetIdentifier, BuiltinRenderTextureType.CameraTarget, (base.InternalRenderMultiplier > 1f && base.PostAntiAliasing == PostAntiAliasingMode.FSSAA && base.InternalRenderMode != RenderMode.AdaptiveResolution) ? base.MaterialFXAA : base.MaterialDefault, 0);
			currentCamera.AddCommandBuffer(CameraEvent.AfterImageEffects, base.CbDownsampler);
		}
	}

	protected void UpdateDownsamplerCommandBuffer()
	{
		base.RtDownsampler.Release();
		if (XRSettings.eyeTextureWidth > 0 && XRSettings.eyeTextureHeight != 0 && (base.RtDownsampler.width != XRSettings.eyeTextureWidth * 2 || base.RtDownsampler.height != XRSettings.eyeTextureHeight))
		{
			base.RtDownsampler.Release();
			base.RtDownsampler.width = XRSettings.eyeTextureWidth * 2;
			base.RtDownsampler.height = XRSettings.eyeTextureHeight;
			base.RtDownsampler.Create();
		}
		base.MaterialCurrent.SetOverrideTag("RenderType", "Opaque");
		base.MaterialCurrent.SetInt("_SrcBlend", 1);
		base.MaterialCurrent.SetInt("_DstBlend", 0);
		base.MaterialCurrent.SetInt("_ZWrite", 1);
		base.MaterialCurrent.renderQueue = -1;
		base.MaterialDefault.SetOverrideTag("RenderType", "Opaque");
		base.MaterialDefault.SetInt("_SrcBlend", 1);
		base.MaterialDefault.SetInt("_DstBlend", 0);
		base.MaterialDefault.SetInt("_ZWrite", 1);
		base.MaterialDefault.renderQueue = -1;
		base.MaterialCurrent.SetFloat("_ResizeWidth", XRSettings.eyeTextureWidth);
		base.MaterialCurrent.SetFloat("_ResizeHeight", XRSettings.eyeTextureHeight);
		base.MaterialCurrent.SetFloat("_Sharpness", base.DownsamplerSharpness);
		base.MaterialCurrent.SetFloat("_SampleDistance", base.DownsamplerDistance);
		base.MaterialFXAA.SetVector("_QualitySettings", new Vector3(1f, 0.063f, 0.0312f));
		base.MaterialFXAA.SetVector("_ConsoleSettings", new Vector4(0.5f, 2f, 0.125f, 0.04f));
		base.MaterialFXAA.SetFloat("_Intensity", 1f);
	}

	protected void ClearDownsamplerCommandBuffer()
	{
		if (!(currentCamera == null) && new List<CommandBuffer>(currentCamera.GetCommandBuffers(CameraEvent.AfterImageEffects)).Find((CommandBuffer x) => x.name == "SSAA_VR_APPLY") != null)
		{
			currentCamera.RemoveCommandBuffer(CameraEvent.AfterImageEffects, base.CbDownsampler);
		}
	}

	protected void VRDeviceFetch()
	{
		vrDisplays.Clear();
		SubsystemManager.GetInstances(vrDisplays);
	}

	protected bool VRDeviceAnyActive()
	{
		VRDeviceFetch();
		for (int i = 0; i < vrDisplays.Count; i++)
		{
			if (vrDisplays[i].running)
			{
				return true;
			}
		}
		return false;
	}

	protected void VRDeviceUpdate(float renderScale)
	{
		if (renderScale < 0.1f)
		{
			renderScale = 1f;
		}
		XRSettings.eyeTextureResolutionScale = renderScale;
	}

	protected void VRDeviceCacheRes()
	{
		VRCachedRenderScale = XRSettings.eyeTextureResolutionScale;
	}

	protected void VRManagerPackmanCheck()
	{
	}

	public override void SetAsAxisBased(float MultiplierX, float MultiplierY)
	{
		Debug.LogWarning("SetAsAxisBased is not supported in VR.\nX axis will be used as global multiplier instead.");
		base.SetAsAxisBased(MultiplierX, MultiplierY);
	}

	public override void SetAsAxisBased(float MultiplierX, float MultiplierY, Filter FilterType, float sharpnessfactor, float sampledist)
	{
		Debug.LogWarning("SetAsAxisBased is not supported in VR.\nX axis will be used as global multiplier instead.");
		base.SetAsAxisBased(MultiplierX, MultiplierY, FilterType, sharpnessfactor, sampledist);
	}

	public override void TakeScreenshot(string path, Vector2 Size, int multiplier)
	{
		Debug.LogWarning("Not available in VR mode");
	}

	public override void TakeScreenshot(string path, Vector2 Size, int multiplier, PostAntiAliasingMode postAntiAliasing)
	{
		Debug.LogWarning("Not available in VR mode");
	}

	public override void TakeScreenshot(string path, Vector2 size, int multiplier, Filter filterType, float sharpness, float sampleDistance)
	{
		Debug.LogWarning("Not available in VR mode");
	}

	public override void TakeScreenshot(string path, Vector2 size, int multiplier, Filter filterType, float sharpness, float sampleDistance, PostAntiAliasingMode postAntiAliasing)
	{
		Debug.LogWarning("Not available in VR mode");
	}

	public override void SetScreenshotModuleToPNG()
	{
		Debug.LogWarning("Not available in VR mode");
	}

	public override void SetScreenshotModuleToJPG(int quality)
	{
		Debug.LogWarning("Not available in VR mode");
	}

	public override void SetScreenshotModuleToEXR(bool EXR32)
	{
		Debug.LogWarning("Not available in VR mode");
	}

	[Obsolete]
	public override void TakeScreenshot(string path, Vector2 Size, int multiplier, float sharpness)
	{
		Debug.LogWarning("Not available in VR mode");
	}

	[Obsolete("SSAA ScreenPointToRay has been deprecated. Use Camera's API instead")]
	public override Ray ScreenPointToRay(Vector3 position)
	{
		return currentCamera.ScreenPointToRay(position);
	}

	[Obsolete("SSAA ScreenToWorldPoint has been deprecated. Use Camera's API instead")]
	public override Vector3 ScreenToWorldPoint(Vector3 position)
	{
		return currentCamera.ScreenToWorldPoint(position);
	}

	[Obsolete("SSAA ScreenToViewportPoint has been deprecated. Use Camera's API instead")]
	public override Vector3 ScreenToViewportPoint(Vector3 position)
	{
		return currentCamera.ScreenToViewportPoint(position);
	}

	[Obsolete("SSAA WorldToScreenPoint has been deprecated. Use Camera's API instead")]
	public override Vector3 WorldToScreenPoint(Vector3 position)
	{
		return currentCamera.WorldToScreenPoint(position);
	}

	[Obsolete("SSAA ViewportToScreenPoint has been deprecated. Use Camera's API instead")]
	public override Vector3 ViewportToScreenPoint(Vector3 position)
	{
		return currentCamera.ViewportToScreenPoint(position);
	}
}
