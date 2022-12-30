using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MadGoat.Core.Utils;
using UnityEngine;
using UnityEngine.Rendering;

namespace MadGoat.SSAA;

[RequireComponent(typeof(Camera))]
[ExecuteAlways]
public class MadGoatSSAA : MonoBehaviour
{
	public const CameraEvent constCamEventDownsampler = CameraEvent.AfterImageEffects;

	public const CameraEvent constCamEventGrabAlphaForward = CameraEvent.AfterForwardOpaque;

	public const CameraEvent constCamEventGrabAlphaDeferred = CameraEvent.AfterForwardAlpha;

	public const CameraEvent constCamEventApplyAlpha = CameraEvent.AfterEverything;

	public const CameraEvent constCamEventPostAntiAliasing = CameraEvent.BeforeImageEffects;

	[SerializeField]
	private RenderTextureFormat internalTextureFormat = RenderTextureFormat.ARGBFloat;

	[SerializeField]
	private RenderMode internalRenderMode;

	[SerializeField]
	private float internalRenderMultiplier = 1f;

	[SerializeField]
	private float internalRenderMultiplierVertical = 1f;

	[SerializeField]
	private LayerMask internalRenderLayerMask;

	[SerializeField]
	private SsaaProfile ssaaProfileHalf = new SsaaProfile(0.5f, useDownsampling: true, Filter.POINT, 0f, 0f);

	[SerializeField]
	private SsaaProfile ssaaProfileX2 = new SsaaProfile(1.4f, useDownsampling: true, Filter.BICUBIC, 0.8f, 0.95f);

	[SerializeField]
	private SsaaProfile ssaaProfileX4 = new SsaaProfile(2f, useDownsampling: true, Filter.BICUBIC, 0.8f, 0.95f);

	[SerializeField]
	private SSAAMode ssaaMode;

	[SerializeField]
	private PostAntiAliasingMode postAntiAliasing;

	[SerializeField]
	private bool downsamplerEnabled = true;

	[SerializeField]
	private Filter downsamplerFilter = Filter.BILINEAR;

	[SerializeField]
	private float downsamplerSharpness = 0.8f;

	[SerializeField]
	private float downsamplerDistance = 1f;

	[SerializeField]
	private int adaptiveResTargetFps = 60;

	[SerializeField]
	private bool adaptiveResTargetVsync;

	[SerializeField]
	private float adaptiveResMinMultiplier = 0.5f;

	[SerializeField]
	private float adaptiveResMaxMultiplier = 1.5f;

	[SerializeField]
	private string screenshotPath = "Assets/SuperSampledSceenshots/";

	[SerializeField]
	private string screenshotPrefix = "SSAA";

	[SerializeField]
	private bool screenshotPrefixIsProduct;

	[SerializeField]
	private ImageFormat screenshotFormat;

	[SerializeField]
	[Range(0f, 100f)]
	private int screenshotQuality = 90;

	[SerializeField]
	private bool screenshotExr32;

	[SerializeField]
	private SsaaScreenshotProfile screenshotCaptureSettings = new SsaaScreenshotProfile();

	[SerializeField]
	private SSAAExtensionPointerEventsSupport extensionIPointerEvents = new SSAAExtensionPointerEventsSupport();

	[SerializeField]
	private SSAAExtensionPostProcessingStack2 extensionPostProcessingStack = new SSAAExtensionPostProcessingStack2();

	[SerializeField]
	private SSAAExtensionCinemachine extensionCinemachine = new SSAAExtensionCinemachine();

	[SerializeField]
	private bool cameraFirstTimeSetup;

	protected Camera currentCamera;

	protected Camera renderCamera;

	public RenderTextureFormat InternalTextureFormat => internalTextureFormat;

	public RenderMode InternalRenderMode
	{
		get
		{
			return internalRenderMode;
		}
		protected set
		{
			internalRenderMode = value;
		}
	}

	public float InternalRenderMultiplier
	{
		get
		{
			return internalRenderMultiplier;
		}
		protected set
		{
			internalRenderMultiplier = value;
		}
	}

	public float InternalRenderMultiplierVertical
	{
		get
		{
			return internalRenderMultiplierVertical;
		}
		protected set
		{
			internalRenderMultiplierVertical = value;
		}
	}

	public LayerMask InternalRenderLayerMask
	{
		get
		{
			return internalRenderLayerMask;
		}
		protected set
		{
			internalRenderLayerMask = value;
		}
	}

	public SsaaProfile SsaaProfileHalf
	{
		get
		{
			return ssaaProfileHalf;
		}
		set
		{
			ssaaProfileHalf = value;
		}
	}

	public SsaaProfile SsaaProfileX2
	{
		get
		{
			return ssaaProfileX2;
		}
		set
		{
			ssaaProfileX2 = value;
		}
	}

	public SsaaProfile SsaaProfileX4
	{
		get
		{
			return ssaaProfileX4;
		}
		set
		{
			ssaaProfileX4 = value;
		}
	}

	public SSAAMode SsaaMode
	{
		get
		{
			return ssaaMode;
		}
		protected set
		{
			ssaaMode = value;
		}
	}

	public PostAntiAliasingMode PostAntiAliasing
	{
		get
		{
			return postAntiAliasing;
		}
		protected set
		{
			postAntiAliasing = value;
		}
	}

	public bool DownsamplerEnabled
	{
		get
		{
			return downsamplerEnabled;
		}
		protected set
		{
			downsamplerEnabled = value;
		}
	}

	public Filter DownsamplerFilter
	{
		get
		{
			return downsamplerFilter;
		}
		protected set
		{
			downsamplerFilter = value;
		}
	}

	public float DownsamplerSharpness
	{
		get
		{
			return downsamplerSharpness;
		}
		protected set
		{
			downsamplerSharpness = value;
		}
	}

	public float DownsamplerDistance
	{
		get
		{
			return downsamplerDistance;
		}
		protected set
		{
			downsamplerDistance = value;
		}
	}

	public int AdaptiveResTargetFps
	{
		get
		{
			return adaptiveResTargetFps;
		}
		protected set
		{
			adaptiveResTargetFps = value;
		}
	}

	public bool AdaptiveResTargetVsync
	{
		get
		{
			return adaptiveResTargetVsync;
		}
		protected set
		{
			adaptiveResTargetVsync = value;
		}
	}

	public float AdaptiveResMinMultiplier
	{
		get
		{
			return adaptiveResMinMultiplier;
		}
		protected set
		{
			adaptiveResMinMultiplier = value;
		}
	}

	public float AdaptiveResMaxMultiplier
	{
		get
		{
			return adaptiveResMaxMultiplier;
		}
		protected set
		{
			adaptiveResMaxMultiplier = value;
		}
	}

	public string ScreenshotPath
	{
		get
		{
			return screenshotPath;
		}
		protected set
		{
			screenshotPath = value;
		}
	}

	public string ScreenshotPrefix
	{
		get
		{
			return screenshotPrefix;
		}
		protected set
		{
			screenshotPrefix = value;
		}
	}

	public bool ScreenshotPrefixIsProduct
	{
		get
		{
			return screenshotPrefixIsProduct;
		}
		protected set
		{
			screenshotPrefixIsProduct = value;
		}
	}

	public ImageFormat ScreenshotFormat
	{
		get
		{
			return screenshotFormat;
		}
		protected set
		{
			screenshotFormat = value;
		}
	}

	public int ScreenshotQuality
	{
		get
		{
			return screenshotQuality;
		}
		protected set
		{
			screenshotQuality = value;
		}
	}

	public bool ScreenshotExr32
	{
		get
		{
			return screenshotExr32;
		}
		protected set
		{
			screenshotExr32 = value;
		}
	}

	public SsaaScreenshotProfile ScreenshotCaptureSettings
	{
		get
		{
			return screenshotCaptureSettings;
		}
		protected set
		{
			screenshotCaptureSettings = value;
		}
	}

	public SSAAExtensionPointerEventsSupport ExtensionIPointerEvents
	{
		get
		{
			return extensionIPointerEvents;
		}
		protected set
		{
			extensionIPointerEvents = value;
		}
	}

	public SSAAExtensionPostProcessingStack2 ExtensionPostProcessingStack
	{
		get
		{
			return extensionPostProcessingStack;
		}
		protected set
		{
			extensionPostProcessingStack = value;
		}
	}

	public SSAAExtensionCinemachine ExtensionCinemachine
	{
		get
		{
			return extensionCinemachine;
		}
		protected set
		{
			extensionCinemachine = value;
		}
	}

	protected Shader ShaderBilinear { get; set; }

	protected Shader ShaderBicubic { get; set; }

	protected Shader ShaderDefault { get; set; }

	protected Shader ShaderFXAA { get; set; }

	protected Shader ShaderAlpha { get; set; }

	protected Material MaterialBilinear { get; set; }

	protected Material MaterialBicubic { get; set; }

	protected Material MaterialDefault { get; set; }

	protected Material MaterialFXAA { get; set; }

	protected Material MaterialAlpha { get; set; }

	public Material MaterialCurrent { get; protected set; }

	protected Material MaterialOld { get; set; }

	public float CameraTargetWidth { get; protected set; }

	public float CameraTargetHeight { get; protected set; }

	public Camera CurrentCamera
	{
		get
		{
			return currentCamera;
		}
		protected set
		{
			currentCamera = value;
		}
	}

	public Camera RenderCamera
	{
		get
		{
			return renderCamera;
		}
		protected set
		{
			renderCamera = value;
		}
	}

	private bool ScreenshotInternalQueued { get; set; }

	private float ScreenshotTempMultiplier { get; set; }

	private bool ScreenshotTempDownsamplerEnabled { get; set; }

	private Filter ScreenshotTempDownsampler { get; set; }

	private float ScreenshotTempDownsamplerSharpness { get; set; }

	private float ScreenshotTempDownsamplerDistance { get; set; }

	private PostAntiAliasingMode ScreenshotTempPostAntiAliasing { get; set; }

	protected RenderTexture RtDownsampler { get; set; }

	protected RenderTexture RtPostAntiAliasing { get; set; }

	private RenderTexture RtGrabAlpha { get; set; }

	private RenderTexture RtApplyAlpha { get; set; }

	private RenderTexture RtScreenshotTarget { get; set; }

	private RenderTexture RtScreenshotOldTarget { get; set; }

	public CommandBuffer CbPostAntiAliasing { get; set; }

	public CommandBuffer CbGrabAlpha { get; set; }

	public CommandBuffer CbApplyAlpha { get; set; }

	protected CommandBuffer CbDownsampler { get; set; }

	protected RenderingPath CurrentCameraRenderPath { get; set; }

	protected RenderPipelineUtils.PipelineType Pipeline { get; set; }

	protected SsaaFramerateSampler PerfSampler { get; set; }

	private GameObject RenderCamGameObject { get; set; }

	protected bool Initialized { get; set; }

	private void Start()
	{
		OnInitializeProps();
		StartCoroutine(UpdateAdaptiveRes());
		StartCoroutine(OnFinishCameraRender());
	}

	private void Update()
	{
		if (RenderCamera.enabled != currentCamera.enabled)
		{
			renderCamera.enabled = currentCamera.enabled;
		}
		if (!renderCamera || !renderCamera.targetTexture)
		{
			Refresh();
			return;
		}
		if (PerfSampler != null)
		{
			PerfSampler.Update();
		}
		else
		{
			PerfSampler = new SsaaFramerateSampler();
		}
		OnMainFilterChanged(DownsamplerFilter);
		if (CurrentCameraRenderPath != currentCamera.actualRenderingPath && Pipeline == RenderPipelineUtils.PipelineType.BuiltInPipeline)
		{
			ClearCommandBuffer(renderCamera, CbGrabAlpha, (CurrentCameraRenderPath == RenderingPath.Forward) ? CameraEvent.AfterForwardOpaque : CameraEvent.AfterForwardAlpha);
			CurrentCameraRenderPath = currentCamera.actualRenderingPath;
			Refresh();
		}
		if (Pipeline != RenderPipelineUtils.DetectPipeline())
		{
			base.enabled = false;
			Pipeline = RenderPipelineUtils.DetectPipeline();
			base.enabled = true;
		}
		ExtensionIPointerEvents.OnUpdate(this);
		ExtensionPostProcessingStack.OnUpdate(this);
	}

	private void OnEnable()
	{
		if (Initialized)
		{
			Start();
		}
	}

	private void OnDisable()
	{
		if (!(renderCamera == null))
		{
			RenderTexture targetTexture = CurrentCamera.targetTexture;
			if ((bool)renderCamera.targetTexture)
			{
				renderCamera.targetTexture.Release();
			}
			renderCamera.targetTexture = null;
			renderCamera.enabled = false;
			renderCamera.tag = "Untagged";
			ClearCommandBuffer(renderCamera, CbGrabAlpha, (currentCamera.actualRenderingPath == RenderingPath.Forward) ? CameraEvent.AfterForwardOpaque : CameraEvent.AfterForwardAlpha);
			ClearCommandBuffer(renderCamera, CbApplyAlpha, CameraEvent.AfterEverything);
			ClearCommandBuffer(renderCamera, CbPostAntiAliasing, CameraEvent.BeforeImageEffects);
			ClearCommandBuffer(currentCamera, CbDownsampler, CameraEvent.AfterImageEffects);
			if (Pipeline == RenderPipelineUtils.PipelineType.UniversalPipeline || Pipeline == RenderPipelineUtils.PipelineType.HDPipeline)
			{
				RenderPipelineManager.beginCameraRendering -= OnPreRenderSRP;
			}
			float depth = currentCamera.depth;
			currentCamera.CopyFrom(renderCamera);
			currentCamera.depth = depth;
			currentCamera.targetTexture = targetTexture;
			ExtensionIPointerEvents.OnDeinitialize(this);
			ExtensionPostProcessingStack.OnDeinitialize(this);
			ExtensionCinemachine.OnDeinitialize(this);
		}
	}

	private void OnPreRender()
	{
		OnBeginCameraRender(currentCamera);
	}

	private void OnPreRenderSRP(ScriptableRenderContext context, Camera camera)
	{
		OnBeginCameraRender(currentCamera);
	}

	protected virtual void OnInitialize()
	{
		List<Camera> list = new List<Camera>(GetComponentsInChildren<Camera>());
		if (list.Find((Camera x) => x.name.Contains("_SSAA")) == null)
		{
			RenderCamGameObject = new GameObject(base.gameObject.name + "_SSAA");
			RenderCamGameObject.transform.SetParent(base.transform);
			RenderCamGameObject.transform.localPosition = Vector3.zero;
			RenderCamGameObject.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
			renderCamera = RenderCamGameObject.AddComponent<Camera>();
		}
		else
		{
			renderCamera = list.Find((Camera x) => x.name.Contains("_SSAA"));
			RenderCamGameObject = renderCamera.gameObject;
		}
		string text = currentCamera.tag;
		currentCamera.tag = "Untagged";
		renderCamera.tag = text;
		currentCamera.tag = text;
		if (!cameraFirstTimeSetup)
		{
			cameraFirstTimeSetup = true;
			InternalRenderLayerMask = currentCamera.cullingMask;
		}
		currentCamera.rect = new Rect(0f, 0f, 1f, 1f);
		if (Pipeline == RenderPipelineUtils.PipelineType.BuiltInPipeline)
		{
			currentCamera.cullingMask = 0;
		}
		RenderCamera.gameObject.SetActive(value: true);
		renderCamera.enabled = true;
		renderCamera.CopyFrom(currentCamera);
		if (Pipeline == RenderPipelineUtils.PipelineType.UniversalPipeline || Pipeline == RenderPipelineUtils.PipelineType.HDPipeline)
		{
			RenderPipelineManager.beginCameraRendering -= OnPreRenderSRP;
			RenderPipelineManager.beginCameraRendering += OnPreRenderSRP;
		}
		if (!RtDownsampler)
		{
			RtDownsampler = new RenderTexture(1024, 1024, 24, InternalTextureFormat);
		}
		RenderCamera.targetTexture = RtDownsampler;
		if (!RtGrabAlpha)
		{
			RtGrabAlpha = new RenderTexture(renderCamera.targetTexture.width, renderCamera.targetTexture.height, 1, InternalTextureFormat);
		}
		if (!RtApplyAlpha)
		{
			RtApplyAlpha = new RenderTexture(renderCamera.targetTexture.width, renderCamera.targetTexture.height, 1, InternalTextureFormat);
		}
		if (!RtPostAntiAliasing)
		{
			RtPostAntiAliasing = new RenderTexture(renderCamera.targetTexture.width, renderCamera.targetTexture.height, 1, InternalTextureFormat);
		}
		MaterialCurrent = MaterialDefault;
		ExtensionIPointerEvents.OnInitialize(this);
		ExtensionPostProcessingStack.OnInitialize(this);
		ExtensionCinemachine.OnInitialize(this);
		Initialized = true;
	}

	protected virtual void OnInitializeProps()
	{
		if (PerfSampler == null)
		{
			PerfSampler = new SsaaFramerateSampler();
		}
		CameraTargetWidth = 1024f;
		CameraTargetHeight = 1024f;
		RenderPipelineUtils.PipelineType pipeline = Pipeline;
		Pipeline = RenderPipelineUtils.DetectPipeline();
		if (pipeline != Pipeline && Pipeline == RenderPipelineUtils.PipelineType.Unsupported)
		{
			base.enabled = false;
			Debug.LogError("Unsupported Render Pipeline. SSAA is disabled");
			SymbolDefineUtils.RemoveDefine("SSAA_URP");
			SymbolDefineUtils.RemoveDefine("SSAA_HDRP");
			Refresh();
			return;
		}
		if (pipeline != Pipeline && Pipeline == RenderPipelineUtils.PipelineType.HDPipeline)
		{
			SymbolDefineUtils.AddDefine("SSAA_HDRP");
			SymbolDefineUtils.RemoveDefine("SSAA_URP");
			Refresh();
		}
		else if (pipeline != Pipeline && Pipeline == RenderPipelineUtils.PipelineType.UniversalPipeline)
		{
			SymbolDefineUtils.AddDefine("SSAA_URP");
			SymbolDefineUtils.RemoveDefine("SSAA_HDRP");
			Refresh();
		}
		else if (pipeline != Pipeline && Pipeline == RenderPipelineUtils.PipelineType.BuiltInPipeline)
		{
			SymbolDefineUtils.RemoveDefine("SSAA_URP");
			SymbolDefineUtils.RemoveDefine("SSAA_HDRP");
			Refresh();
		}
		if (CbPostAntiAliasing == null)
		{
			CbPostAntiAliasing = new CommandBuffer();
		}
		if (CbGrabAlpha == null)
		{
			CbGrabAlpha = new CommandBuffer();
		}
		if (CbApplyAlpha == null)
		{
			CbApplyAlpha = new CommandBuffer();
		}
		if (CbDownsampler == null)
		{
			CbDownsampler = new CommandBuffer();
		}
		ShaderBilinear = Shader.Find("Hidden/SSAA_Bilinear");
		ShaderBicubic = Shader.Find("Hidden/SSAA_Bicubic");
		ShaderDefault = Shader.Find("Hidden/SSAA_Def");
		if (ShaderFXAA == null)
		{
			ShaderFXAA = Shader.Find("Hidden/SSAA/FSS");
		}
		if (ShaderAlpha == null)
		{
			ShaderAlpha = Shader.Find("Hidden/SSAA_Alpha");
		}
		MaterialBilinear = new Material(ShaderBilinear);
		MaterialBicubic = new Material(ShaderBicubic);
		MaterialDefault = new Material(ShaderDefault);
		if (MaterialFXAA == null)
		{
			MaterialFXAA = new Material(ShaderFXAA);
		}
		if (MaterialAlpha == null)
		{
			MaterialAlpha = new Material(ShaderAlpha);
		}
		if (currentCamera == null)
		{
			currentCamera = GetComponent<Camera>();
		}
		OnInitialize();
	}

	public virtual void OnBeginCameraRender(Camera cam)
	{
		if (cam != currentCamera || !base.enabled || RenderCamera == null || renderCamera.targetTexture == null)
		{
			return;
		}
		RenderTexture targetTexture = renderCamera.targetTexture;
		currentCamera.cullingMask = 0;
		renderCamera.CopyFrom(currentCamera, targetTexture);
		renderCamera.depth = currentCamera.depth + 0.1f;
		renderCamera.cullingMask = InternalRenderLayerMask;
		renderCamera.clearFlags = currentCamera.clearFlags;
		renderCamera.targetTexture.filterMode = ((DownsamplerFilter != 0 || !DownsamplerEnabled) ? FilterMode.Trilinear : FilterMode.Point);
		ExtensionCinemachine.OnUpdate(this);
		if (ScreenshotCaptureSettings.takeScreenshot)
		{
			SetupScreenshotRender();
		}
		CameraTargetWidth = ((currentCamera.targetTexture != null) ? currentCamera.targetTexture.width : Screen.width);
		CameraTargetHeight = ((currentCamera.targetTexture != null) ? currentCamera.targetTexture.height : Screen.height);
		currentCamera.aspect = CameraTargetWidth * currentCamera.rect.width / (CameraTargetHeight * currentCamera.rect.height);
		renderCamera.aspect = CameraTargetWidth * renderCamera.rect.width / (CameraTargetHeight * renderCamera.rect.height);
		if ((int)(CameraTargetWidth * InternalRenderMultiplier) != renderCamera.targetTexture.width || (int)(CameraTargetHeight * ((InternalRenderMode == RenderMode.PerAxisScale) ? InternalRenderMultiplierVertical : InternalRenderMultiplier)) != renderCamera.targetTexture.height)
		{
			if (RenderTexture.active == renderCamera.targetTexture)
			{
				RenderTexture.active = null;
			}
			UpdateRtSizes();
			if (Pipeline == RenderPipelineUtils.PipelineType.BuiltInPipeline)
			{
				renderCamera.Render();
			}
		}
		if (Pipeline == RenderPipelineUtils.PipelineType.BuiltInPipeline)
		{
			UpdateDownsamplerCommandBuffer(currentCamera);
			UpdateAlphaCommandBuffer(renderCamera);
		}
		if (InternalRenderMultiplier > 1f && PostAntiAliasing == PostAntiAliasingMode.FSSAA && InternalRenderMode != RenderMode.AdaptiveResolution)
		{
			if (Pipeline == RenderPipelineUtils.PipelineType.BuiltInPipeline)
			{
				UpdatePostAntiAliasingCommandBuffer(renderCamera);
			}
		}
		else if (Pipeline == RenderPipelineUtils.PipelineType.BuiltInPipeline)
		{
			ClearCommandBuffer(renderCamera, CbPostAntiAliasing, CameraEvent.BeforeImageEffects);
		}
	}

	protected virtual IEnumerator OnFinishCameraRender()
	{
		while (base.enabled)
		{
			yield return new WaitForEndOfFrame();
			if (ScreenshotCaptureSettings.takeScreenshot)
			{
				yield return new WaitForEndOfFrame();
				yield return new WaitForEndOfFrame();
				HandleScreenshot();
			}
		}
	}

	public void OnMainFilterChanged(Filter Type)
	{
		MaterialOld = MaterialCurrent;
		switch (Type)
		{
		case Filter.POINT:
			MaterialCurrent = MaterialDefault;
			break;
		case Filter.BILINEAR:
			MaterialCurrent = MaterialBilinear;
			break;
		case Filter.BICUBIC:
			MaterialCurrent = MaterialBicubic;
			break;
		}
		if ((!DownsamplerEnabled || InternalRenderMultiplier == 1f) && MaterialCurrent != MaterialDefault)
		{
			MaterialCurrent = MaterialDefault;
		}
		if (MaterialCurrent != MaterialOld)
		{
			MaterialOld = MaterialCurrent;
			ClearCommandBuffer(currentCamera, CbDownsampler, CameraEvent.AfterImageEffects);
		}
	}

	protected void UpdatePostAntiAliasingCommandBuffer(Camera hookCamera)
	{
		if (Pipeline == RenderPipelineUtils.PipelineType.BuiltInPipeline)
		{
			MaterialFXAA.SetVector("_QualitySettings", new Vector3(1f, 0.063f, 0.0312f));
			MaterialFXAA.SetVector("_ConsoleSettings", new Vector4(0.5f, 2f, 0.125f, 0.04f));
			MaterialFXAA.SetFloat("_Intensity", 1f);
			if (new List<CommandBuffer>(hookCamera.GetCommandBuffers(CameraEvent.BeforeImageEffects)).Find((CommandBuffer x) => x.name == "SSAA_FSS") == null)
			{
				CbPostAntiAliasing.Clear();
				CbPostAntiAliasing.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
				RenderTargetIdentifier renderTargetIdentifier = new RenderTargetIdentifier(RtPostAntiAliasing);
				CbPostAntiAliasing.Blit(BuiltinRenderTextureType.CameraTarget, renderTargetIdentifier);
				CbPostAntiAliasing.Blit(renderTargetIdentifier, BuiltinRenderTextureType.CameraTarget, MaterialFXAA, 0);
				CbPostAntiAliasing.name = "SSAA_FSS";
				hookCamera.AddCommandBuffer(CameraEvent.BeforeImageEffects, CbPostAntiAliasing);
			}
		}
	}

	protected void UpdateAlphaCommandBuffer(Camera hookCamera)
	{
		if (Pipeline == RenderPipelineUtils.PipelineType.BuiltInPipeline && new List<CommandBuffer>(hookCamera.GetCommandBuffers(CameraEvent.AfterEverything)).Find((CommandBuffer x) => x.name == "SSAA_Apply_Alpha") == null)
		{
			CbGrabAlpha.Clear();
			CbApplyAlpha.Clear();
			CbGrabAlpha.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
			CbApplyAlpha.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
			RenderTargetIdentifier renderTargetIdentifier = new RenderTargetIdentifier(RtGrabAlpha);
			RenderTargetIdentifier renderTargetIdentifier2 = new RenderTargetIdentifier(RtApplyAlpha);
			CbGrabAlpha.Blit(BuiltinRenderTextureType.CameraTarget, renderTargetIdentifier, MaterialAlpha, 0);
			CbApplyAlpha.SetGlobalTexture("_MainTexA", renderTargetIdentifier);
			CbApplyAlpha.Blit(BuiltinRenderTextureType.CameraTarget, renderTargetIdentifier2);
			CbApplyAlpha.Blit(renderTargetIdentifier2, BuiltinRenderTextureType.CameraTarget, MaterialAlpha, 1);
			CbGrabAlpha.name = "SSAA_Grab_Alpha";
			CbApplyAlpha.name = "SSAA_Apply_Alpha";
			hookCamera.AddCommandBuffer((currentCamera.actualRenderingPath == RenderingPath.Forward) ? CameraEvent.AfterForwardOpaque : CameraEvent.AfterForwardAlpha, CbGrabAlpha);
			hookCamera.AddCommandBuffer(CameraEvent.AfterEverything, CbApplyAlpha);
		}
	}

	protected void UpdateDownsamplerCommandBuffer(Camera hookCamera)
	{
		if (Pipeline == RenderPipelineUtils.PipelineType.BuiltInPipeline)
		{
			MaterialCurrent.SetFloat("_ResizeWidth", CameraTargetWidth);
			MaterialCurrent.SetFloat("_ResizeHeight", CameraTargetHeight);
			MaterialCurrent.SetFloat("_Sharpness", DownsamplerSharpness);
			MaterialCurrent.SetFloat("_SampleDistance", DownsamplerDistance);
			if (hookCamera.clearFlags == CameraClearFlags.Color || hookCamera.clearFlags == CameraClearFlags.Skybox)
			{
				MaterialCurrent.SetOverrideTag("RenderType", "Opaque");
				MaterialCurrent.SetInt("_SrcBlend", 1);
				MaterialCurrent.SetInt("_DstBlend", 0);
				MaterialCurrent.SetInt("_ZWrite", 1);
				MaterialCurrent.renderQueue = -1;
			}
			else
			{
				MaterialCurrent.SetOverrideTag("RenderType", "Transparent");
				MaterialCurrent.SetInt("_SrcBlend", 1);
				MaterialCurrent.SetInt("_DstBlend", 10);
				MaterialCurrent.SetInt("_ZWrite", 0);
				MaterialCurrent.renderQueue = 3000;
			}
			if (new List<CommandBuffer>(currentCamera.GetCommandBuffers(CameraEvent.AfterImageEffects)).Find((CommandBuffer x) => x.name == "SSAA_Downsampler") == null)
			{
				CbDownsampler.Clear();
				CbDownsampler.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
				RenderTargetIdentifier source = new RenderTargetIdentifier(RtDownsampler);
				CbDownsampler.Blit(source, BuiltinRenderTextureType.CameraTarget, MaterialCurrent, 0);
				CbDownsampler.name = "SSAA_Downsampler";
				hookCamera.AddCommandBuffer(CameraEvent.AfterImageEffects, CbDownsampler);
			}
		}
	}

	protected void ClearCommandBuffer(Camera hookCamera, CommandBuffer commandBuffer, CameraEvent cameraEvent)
	{
		if (Pipeline == RenderPipelineUtils.PipelineType.BuiltInPipeline)
		{
			commandBuffer.Clear();
			hookCamera.RemoveCommandBuffer(cameraEvent, commandBuffer);
		}
	}

	protected IEnumerator UpdateAdaptiveRes()
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(2, 5));
		if (InternalRenderMode == RenderMode.AdaptiveResolution)
		{
			int num = (AdaptiveResTargetVsync ? Screen.currentResolution.refreshRate : AdaptiveResTargetFps);
			if (PerfSampler.CurrentFps < num - 5)
			{
				InternalRenderMultiplier = Mathf.Clamp(InternalRenderMultiplier - 0.1f, AdaptiveResMinMultiplier, AdaptiveResMaxMultiplier);
			}
			else if (PerfSampler.CurrentFps >= num + (AdaptiveResTargetVsync ? (-1) : 10))
			{
				InternalRenderMultiplier = Mathf.Clamp(InternalRenderMultiplier + 0.1f, AdaptiveResMinMultiplier, AdaptiveResMaxMultiplier);
			}
		}
		if (base.enabled)
		{
			StartCoroutine(UpdateAdaptiveRes());
		}
	}

	private void UpdateRtSizes()
	{
		if (CameraTargetWidth * InternalRenderMultiplier < 128f && CameraTargetHeight * ((InternalRenderMode == RenderMode.PerAxisScale) ? InternalRenderMultiplierVertical : InternalRenderMultiplier) < 128f)
		{
			return;
		}
		try
		{
			renderCamera.targetTexture.Release();
			RtDownsampler.antiAliasing = 1;
			renderCamera.targetTexture.width = (int)(CameraTargetWidth * InternalRenderMultiplier);
			renderCamera.targetTexture.height = (int)(CameraTargetHeight * ((InternalRenderMode == RenderMode.PerAxisScale) ? InternalRenderMultiplierVertical : InternalRenderMultiplier));
			renderCamera.targetTexture.Create();
			RtGrabAlpha.Release();
			RtGrabAlpha.width = renderCamera.targetTexture.width;
			RtGrabAlpha.height = renderCamera.targetTexture.height;
			RtGrabAlpha.Create();
			RtApplyAlpha.Release();
			RtApplyAlpha.width = renderCamera.targetTexture.width;
			RtApplyAlpha.height = renderCamera.targetTexture.height;
			RtApplyAlpha.Create();
			RtPostAntiAliasing.Release();
			RtPostAntiAliasing.width = renderCamera.targetTexture.width;
			RtPostAntiAliasing.height = renderCamera.targetTexture.height;
			RtPostAntiAliasing.Create();
		}
		catch (Exception message)
		{
			Debug.LogError("Something went wrong. SSAA has been set to off");
			Debug.LogError(message);
			SetAsSSAA(SSAAMode.SSAA_OFF);
		}
	}

	private void SetupScreenshotRender()
	{
		try
		{
			if (!ScreenshotInternalQueued)
			{
				if (RtScreenshotTarget == null)
				{
					RtScreenshotTarget = new RenderTexture((int)ScreenshotCaptureSettings.outputResolution.x, (int)ScreenshotCaptureSettings.outputResolution.y, 24, internalTextureFormat);
				}
				else
				{
					RtScreenshotTarget.Release();
					RtScreenshotTarget.width = (int)ScreenshotCaptureSettings.outputResolution.x;
					RtScreenshotTarget.height = (int)ScreenshotCaptureSettings.outputResolution.y;
				}
				RtScreenshotTarget.Create();
				RtScreenshotOldTarget = currentCamera.targetTexture;
				ScreenshotTempDownsamplerEnabled = downsamplerEnabled;
				ScreenshotTempDownsampler = downsamplerFilter;
				ScreenshotTempDownsamplerSharpness = downsamplerSharpness;
				ScreenshotTempDownsamplerDistance = downsamplerDistance;
				ScreenshotTempMultiplier = internalRenderMultiplier;
				ScreenshotTempPostAntiAliasing = postAntiAliasing;
				currentCamera.targetTexture = RtScreenshotTarget;
				downsamplerEnabled = screenshotCaptureSettings.downsamplerEnabled;
				downsamplerFilter = screenshotCaptureSettings.downsamplerFilter;
				downsamplerSharpness = screenshotCaptureSettings.downsamplerSharpness;
				downsamplerDistance = screenshotCaptureSettings.downsamplerDistance;
				internalRenderMultiplier = screenshotCaptureSettings.screenshotMultiplier;
				postAntiAliasing = screenshotCaptureSettings.postAntiAliasing;
				ScreenshotInternalQueued = true;
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.ToString());
		}
	}

	private void HandleScreenshot()
	{
		if (!(currentCamera.targetTexture == null))
		{
			Material material = new Material(Shader.Find("Hidden/SSAA_Bilinear"));
			RenderTexture renderTexture = new RenderTexture(currentCamera.targetTexture.width, currentCamera.targetTexture.height, 24, RenderTextureFormat.ARGB32);
			bool sRGBWrite = GL.sRGBWrite;
			GL.sRGBWrite = true;
			if (ScreenshotCaptureSettings.downsamplerEnabled)
			{
				material.SetFloat("_ResizeWidth", (int)ScreenshotCaptureSettings.outputResolution.x);
				material.SetFloat("_ResizeHeight", (int)ScreenshotCaptureSettings.outputResolution.y);
				material.SetFloat("_Sharpness", 0.85f);
				material.SetFloat("_SampleDistance", 1f);
				Graphics.Blit(currentCamera.targetTexture, renderTexture);
			}
			else
			{
				Graphics.Blit(currentCamera.targetTexture, renderTexture);
			}
			RenderTexture.active = renderTexture;
			Texture2D texture2D = new Texture2D(RenderTexture.active.width, RenderTexture.active.height, TextureFormat.RGBA32, mipChain: false);
			texture2D.ReadPixels(new Rect(0f, 0f, RenderTexture.active.width, RenderTexture.active.height), 0, 0);
			new FileInfo(screenshotPath).Directory.Create();
			string text = (screenshotPrefixIsProduct ? Application.productName : screenshotPrefix) + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmssff") + "_" + ScreenshotCaptureSettings.outputResolution.y + "p";
			if (screenshotFormat == ImageFormat.PNG)
			{
				File.WriteAllBytes(screenshotPath + text + ".png", texture2D.EncodeToPNG());
			}
			else if (screenshotFormat == ImageFormat.JPG)
			{
				File.WriteAllBytes(screenshotPath + text + ".jpg", texture2D.EncodeToJPG(screenshotQuality));
			}
			else
			{
				File.WriteAllBytes(screenshotPath + text + ".exr", texture2D.EncodeToEXR(screenshotExr32 ? Texture2D.EXRFlags.OutputAsFloat : Texture2D.EXRFlags.None));
			}
			RenderTexture.active = null;
			renderTexture.Release();
			GL.sRGBWrite = sRGBWrite;
			UnityEngine.Object.DestroyImmediate(texture2D);
			ScreenshotCaptureSettings.takeScreenshot = false;
			ScreenshotInternalQueued = false;
			currentCamera.targetTexture = RtScreenshotOldTarget;
			downsamplerEnabled = ScreenshotTempDownsamplerEnabled;
			downsamplerFilter = ScreenshotTempDownsampler;
			downsamplerSharpness = ScreenshotTempDownsamplerSharpness;
			internalRenderMultiplier = ScreenshotTempMultiplier;
			RtScreenshotTarget.Release();
		}
	}

	public void Refresh()
	{
		base.enabled = false;
		base.enabled = true;
		MadGoatSSAA_VR component = GetComponent<MadGoatSSAA_VR>();
		if ((bool)component && component != this)
		{
			component.Refresh();
		}
	}

	public void SetAsSSAA(SSAAMode mode)
	{
		InternalRenderMode = RenderMode.SSAA;
		SsaaMode = mode;
		switch (mode)
		{
		case SSAAMode.SSAA_OFF:
			InternalRenderMultiplier = 1f;
			DownsamplerEnabled = false;
			break;
		case SSAAMode.SSAA_HALF:
			InternalRenderMultiplier = SsaaProfileHalf.multiplier;
			DownsamplerEnabled = SsaaProfileHalf.useFilter;
			DownsamplerSharpness = SsaaProfileHalf.sharpness;
			DownsamplerFilter = SsaaProfileHalf.filterType;
			DownsamplerDistance = SsaaProfileHalf.sampleDistance;
			break;
		case SSAAMode.SSAA_X2:
			InternalRenderMultiplier = SsaaProfileX2.multiplier;
			DownsamplerEnabled = SsaaProfileX2.useFilter;
			DownsamplerSharpness = SsaaProfileX2.sharpness;
			DownsamplerFilter = SsaaProfileX2.filterType;
			DownsamplerDistance = SsaaProfileX2.sampleDistance;
			break;
		case SSAAMode.SSAA_X4:
			InternalRenderMultiplier = SsaaProfileX4.multiplier;
			DownsamplerEnabled = SsaaProfileX4.useFilter;
			DownsamplerSharpness = SsaaProfileX4.sharpness;
			DownsamplerFilter = SsaaProfileX4.filterType;
			DownsamplerDistance = SsaaProfileX4.sampleDistance;
			break;
		}
		MadGoatSSAA_VR component = GetComponent<MadGoatSSAA_VR>();
		if ((bool)component && component != this)
		{
			component.SetAsSSAA(mode);
		}
	}

	public void SetAsScale(float multiplier)
	{
		if (multiplier < 0.1f)
		{
			multiplier = 0.1f;
		}
		InternalRenderMode = RenderMode.Custom;
		InternalRenderMultiplier = multiplier;
		SetDownsamplingSettings(useFilter: false);
		MadGoatSSAA_VR component = GetComponent<MadGoatSSAA_VR>();
		if ((bool)component && component != this)
		{
			component.SetAsScale(multiplier);
		}
	}

	public void SetAsScale(float multiplier, Filter filterType, float sharpness, float sampleDistance)
	{
		if (multiplier < 0.1f)
		{
			multiplier = 0.1f;
		}
		InternalRenderMode = RenderMode.Custom;
		InternalRenderMultiplier = multiplier;
		SetDownsamplingSettings(filterType, sharpness, sampleDistance);
		MadGoatSSAA_VR component = GetComponent<MadGoatSSAA_VR>();
		if ((bool)component && component != this)
		{
			component.SetAsScale(multiplier, filterType, sharpness, sampleDistance);
		}
	}

	public void SetAsScale(int percent)
	{
		percent = Mathf.Clamp(percent, 50, 200);
		InternalRenderMode = RenderMode.ResolutionScale;
		InternalRenderMultiplier = (float)percent / 100f;
		SetDownsamplingSettings(useFilter: false);
		MadGoatSSAA_VR component = GetComponent<MadGoatSSAA_VR>();
		if ((bool)component && component != this)
		{
			component.SetAsScale(percent);
		}
	}

	public void SetAsScale(int percent, Filter filterType, float sharpness, float sampleDistance)
	{
		percent = Mathf.Clamp(percent, 50, 200);
		InternalRenderMode = RenderMode.ResolutionScale;
		InternalRenderMultiplier = (float)percent / 100f;
		SetDownsamplingSettings(filterType, sharpness, sampleDistance);
		MadGoatSSAA_VR component = GetComponent<MadGoatSSAA_VR>();
		if ((bool)component && component != this)
		{
			component.SetAsScale(percent, filterType, sharpness, sampleDistance);
		}
	}

	public void SetAsAdaptive(float minMultiplier, float maxMultiplier)
	{
		if (minMultiplier < 0.1f)
		{
			minMultiplier = 0.1f;
		}
		if (maxMultiplier < minMultiplier)
		{
			maxMultiplier = minMultiplier + 0.1f;
		}
		InternalRenderMode = RenderMode.AdaptiveResolution;
		AdaptiveResMinMultiplier = minMultiplier;
		AdaptiveResMaxMultiplier = maxMultiplier;
		AdaptiveResTargetVsync = true;
		SetDownsamplingSettings(useFilter: false);
		MadGoatSSAA_VR component = GetComponent<MadGoatSSAA_VR>();
		if ((bool)component && component != this)
		{
			component.SetAsAdaptive(minMultiplier, maxMultiplier);
		}
	}

	public void SetAsAdaptive(float minMultiplier, float maxMultiplier, int targetFramerate)
	{
		if (minMultiplier < 0.1f)
		{
			minMultiplier = 0.1f;
		}
		if (maxMultiplier < minMultiplier)
		{
			maxMultiplier = minMultiplier + 0.1f;
		}
		InternalRenderMode = RenderMode.AdaptiveResolution;
		AdaptiveResMinMultiplier = minMultiplier;
		AdaptiveResMaxMultiplier = maxMultiplier;
		AdaptiveResTargetFps = targetFramerate;
		AdaptiveResTargetVsync = false;
		SetDownsamplingSettings(useFilter: false);
		MadGoatSSAA_VR component = GetComponent<MadGoatSSAA_VR>();
		if ((bool)component && component != this)
		{
			component.SetAsAdaptive(minMultiplier, maxMultiplier, targetFramerate);
		}
	}

	public void SetAsAdaptive(float minMultiplier, float maxMultiplier, int targetFramerate, Filter filterType, float sharpness, float sampleDistance)
	{
		if (minMultiplier < 0.1f)
		{
			minMultiplier = 0.1f;
		}
		if (maxMultiplier < minMultiplier)
		{
			maxMultiplier = minMultiplier + 0.1f;
		}
		InternalRenderMode = RenderMode.AdaptiveResolution;
		AdaptiveResMinMultiplier = minMultiplier;
		AdaptiveResMaxMultiplier = maxMultiplier;
		AdaptiveResTargetFps = targetFramerate;
		AdaptiveResTargetVsync = false;
		SetDownsamplingSettings(filterType, sharpness, sampleDistance);
		MadGoatSSAA_VR component = GetComponent<MadGoatSSAA_VR>();
		if ((bool)component && component != this)
		{
			component.SetAsAdaptive(minMultiplier, maxMultiplier, targetFramerate, filterType, sharpness, sampleDistance);
		}
	}

	public void SetAsAdaptive(float minMultiplier, float maxMultiplier, Filter filterType, float sharpness, float sampleDistance)
	{
		if (minMultiplier < 0.1f)
		{
			minMultiplier = 0.1f;
		}
		if (maxMultiplier < minMultiplier)
		{
			maxMultiplier = minMultiplier + 0.1f;
		}
		InternalRenderMode = RenderMode.AdaptiveResolution;
		AdaptiveResMinMultiplier = minMultiplier;
		AdaptiveResMaxMultiplier = maxMultiplier;
		AdaptiveResTargetVsync = true;
		SetDownsamplingSettings(filterType, sharpness, sampleDistance);
		MadGoatSSAA_VR component = GetComponent<MadGoatSSAA_VR>();
		if ((bool)component && component != this)
		{
			component.SetAsAdaptive(minMultiplier, maxMultiplier, filterType, sharpness, sampleDistance);
		}
	}

	public virtual void SetAsAxisBased(float multiplierX, float multiplierY)
	{
		if (multiplierX < 0.1f)
		{
			multiplierX = 0.1f;
		}
		if (multiplierY < 0.1f)
		{
			multiplierY = 0.1f;
		}
		InternalRenderMode = RenderMode.PerAxisScale;
		InternalRenderMultiplier = multiplierX;
		InternalRenderMultiplierVertical = multiplierY;
		SetDownsamplingSettings(useFilter: false);
		MadGoatSSAA_VR component = GetComponent<MadGoatSSAA_VR>();
		if ((bool)component && component != this)
		{
			component.SetAsAxisBased(multiplierX, multiplierY);
		}
	}

	public virtual void SetAsAxisBased(float multiplierX, float multiplierY, Filter filterType, float sharpness, float sampleDistance)
	{
		if (multiplierX < 0.1f)
		{
			multiplierX = 0.1f;
		}
		if (multiplierY < 0.1f)
		{
			multiplierY = 0.1f;
		}
		InternalRenderMode = RenderMode.PerAxisScale;
		InternalRenderMultiplier = multiplierX;
		InternalRenderMultiplierVertical = multiplierY;
		SetDownsamplingSettings(filterType, sharpness, sampleDistance);
		MadGoatSSAA_VR component = GetComponent<MadGoatSSAA_VR>();
		if ((bool)component && component != this)
		{
			component.SetAsAxisBased(multiplierX, multiplierY, filterType, sharpness, sampleDistance);
		}
	}

	public void SetDownsamplingSettings(bool useFilter)
	{
		DownsamplerEnabled = useFilter;
		DownsamplerFilter = (useFilter ? Filter.BILINEAR : Filter.POINT);
		DownsamplerSharpness = (useFilter ? 0.85f : 0f);
		DownsamplerDistance = (useFilter ? 0.9f : 0f);
		MadGoatSSAA_VR component = GetComponent<MadGoatSSAA_VR>();
		if ((bool)component)
		{
			component.SetDownsamplingSettings(useFilter);
		}
	}

	public void SetDownsamplingSettings(Filter FilterType, float sharpness, float sampledist)
	{
		DownsamplerEnabled = true;
		DownsamplerFilter = FilterType;
		DownsamplerSharpness = Mathf.Clamp(sharpness, 0f, 1f);
		DownsamplerDistance = Mathf.Clamp(sampledist, 0.5f, 1.5f);
		MadGoatSSAA_VR component = GetComponent<MadGoatSSAA_VR>();
		if ((bool)component && component != this)
		{
			component.SetDownsamplingSettings(FilterType, sharpness, sampledist);
		}
	}

	public void SetPostAAMode(PostAntiAliasingMode postAntiAliasing)
	{
		PostAntiAliasing = postAntiAliasing;
		MadGoatSSAA_VR component = GetComponent<MadGoatSSAA_VR>();
		if ((bool)component && component != this)
		{
			component.SetPostAAMode(postAntiAliasing);
		}
	}

	public virtual void TakeScreenshot(string path, Vector2 Size, int multiplier)
	{
		ScreenshotCaptureSettings.takeScreenshot = true;
		ScreenshotCaptureSettings.outputResolution = Size;
		ScreenshotCaptureSettings.screenshotMultiplier = multiplier;
		ScreenshotPath = path;
		ScreenshotCaptureSettings.downsamplerEnabled = false;
	}

	public virtual void TakeScreenshot(string path, Vector2 Size, int multiplier, PostAntiAliasingMode postAntiAliasing)
	{
		ScreenshotCaptureSettings.takeScreenshot = true;
		ScreenshotCaptureSettings.outputResolution = Size;
		ScreenshotCaptureSettings.screenshotMultiplier = multiplier;
		ScreenshotPath = path;
		ScreenshotCaptureSettings.downsamplerEnabled = false;
		ScreenshotCaptureSettings.postAntiAliasing = postAntiAliasing;
	}

	public virtual void TakeScreenshot(string path, Vector2 size, int multiplier, Filter filterType, float sharpness, float sampleDistance)
	{
		ScreenshotCaptureSettings.takeScreenshot = true;
		ScreenshotCaptureSettings.outputResolution = size;
		ScreenshotCaptureSettings.screenshotMultiplier = multiplier;
		ScreenshotPath = path;
		ScreenshotCaptureSettings.downsamplerEnabled = true;
		screenshotCaptureSettings.downsamplerFilter = filterType;
		ScreenshotCaptureSettings.downsamplerSharpness = Mathf.Clamp(sharpness, 0f, 1f);
		ScreenshotCaptureSettings.downsamplerDistance = Mathf.Clamp(sampleDistance, 0f, 2f);
		ScreenshotCaptureSettings.postAntiAliasing = PostAntiAliasingMode.Off;
	}

	public virtual void TakeScreenshot(string path, Vector2 size, int multiplier, Filter filterType, float sharpness, float sampleDistance, PostAntiAliasingMode postAntiAliasing)
	{
		ScreenshotCaptureSettings.takeScreenshot = true;
		ScreenshotCaptureSettings.outputResolution = size;
		ScreenshotCaptureSettings.screenshotMultiplier = multiplier;
		ScreenshotPath = path;
		ScreenshotCaptureSettings.downsamplerEnabled = true;
		screenshotCaptureSettings.downsamplerFilter = filterType;
		ScreenshotCaptureSettings.downsamplerSharpness = Mathf.Clamp(sharpness, 0f, 1f);
		ScreenshotCaptureSettings.downsamplerDistance = Mathf.Clamp(sampleDistance, 0f, 2f);
		ScreenshotCaptureSettings.postAntiAliasing = postAntiAliasing;
	}

	public virtual void SetScreenshotModuleToPNG()
	{
		ScreenshotFormat = ImageFormat.PNG;
	}

	public virtual void SetScreenshotModuleToJPG(int quality)
	{
		ScreenshotFormat = ImageFormat.JPG;
		ScreenshotQuality = Mathf.Clamp(1, 100, quality);
	}

	public virtual void SetScreenshotModuleToEXR(bool EXR32)
	{
		ScreenshotFormat = ImageFormat.EXR;
		ScreenshotExr32 = EXR32;
	}

	public virtual string GetResolution()
	{
		return (int)(CameraTargetWidth * InternalRenderMultiplier) + "x" + (int)(CameraTargetHeight * InternalRenderMultiplier);
	}

	public static void SetAllAsSSAA(SSAAMode mode)
	{
		MadGoatSSAA[] array = UnityEngine.Object.FindObjectsOfType<MadGoatSSAA>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetAsSSAA(mode);
		}
	}

	public static void SetAllAsScale(float multiplier)
	{
		MadGoatSSAA[] array = UnityEngine.Object.FindObjectsOfType<MadGoatSSAA>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetAsScale(multiplier);
		}
	}

	public static void SetAllAsScale(float multiplier, Filter filterType, float sharpness, float sampleDistance)
	{
		MadGoatSSAA[] array = UnityEngine.Object.FindObjectsOfType<MadGoatSSAA>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetAsScale(multiplier, filterType, sharpness, sampleDistance);
		}
	}

	public static void SetAllAsScale(int percent)
	{
		MadGoatSSAA[] array = UnityEngine.Object.FindObjectsOfType<MadGoatSSAA>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetAsScale(percent);
		}
	}

	public static void SetAllAsScale(int percent, Filter filterType, float sharpness, float sampleDistance)
	{
		MadGoatSSAA[] array = UnityEngine.Object.FindObjectsOfType<MadGoatSSAA>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetAsScale(percent, filterType, sharpness, sampleDistance);
		}
	}

	public static void SetAllAsAdaptive(float minMultiplier, float maxMultiplier, int targetFramerate)
	{
		MadGoatSSAA[] array = UnityEngine.Object.FindObjectsOfType<MadGoatSSAA>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetAsAdaptive(minMultiplier, maxMultiplier, targetFramerate);
		}
	}

	public static void SetAllAsAdaptive(float minMultiplier, float maxMultiplier)
	{
		MadGoatSSAA[] array = UnityEngine.Object.FindObjectsOfType<MadGoatSSAA>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetAsAdaptive(minMultiplier, maxMultiplier);
		}
	}

	public static void SetAllAsAdaptive(float minMultiplier, float maxMultiplier, int targetFramerate, Filter filterType, float sharpness, float sampleDistance)
	{
		MadGoatSSAA[] array = UnityEngine.Object.FindObjectsOfType<MadGoatSSAA>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetAsAdaptive(minMultiplier, maxMultiplier, targetFramerate, filterType, sharpness, sampleDistance);
		}
	}

	public static void SetAllAsAdaptive(float minMultiplier, float maxMultiplier, Filter filterType, float sharpness, float sampleDistance)
	{
		MadGoatSSAA[] array = UnityEngine.Object.FindObjectsOfType<MadGoatSSAA>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetAsAdaptive(minMultiplier, maxMultiplier, filterType, sharpness, sampleDistance);
		}
	}

	public static void SetAllAsAxisBased(float multiplierX, float multiplierY)
	{
		MadGoatSSAA[] array = UnityEngine.Object.FindObjectsOfType<MadGoatSSAA>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetAsAxisBased(multiplierX, multiplierY);
		}
	}

	public static void SetAllAsAxisBased(float multiplierX, float multiplierY, Filter filterType, float sharpness, float sampleDistance)
	{
		MadGoatSSAA[] array = UnityEngine.Object.FindObjectsOfType<MadGoatSSAA>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetAsAxisBased(multiplierX, multiplierY, filterType, sharpness, sampleDistance);
		}
	}

	public static void SetAllDownsamplingSettings(bool use)
	{
		MadGoatSSAA[] array = UnityEngine.Object.FindObjectsOfType<MadGoatSSAA>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetDownsamplingSettings(use);
		}
	}

	public static void SetAllDownsamplingSettings(Filter filterType, float sharpness, float sampleDistance)
	{
		MadGoatSSAA[] array = UnityEngine.Object.FindObjectsOfType<MadGoatSSAA>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetDownsamplingSettings(filterType, sharpness, sampleDistance);
		}
	}

	public static void SetAllPostAAMode(PostAntiAliasingMode mode)
	{
		MadGoatSSAA[] array = UnityEngine.Object.FindObjectsOfType<MadGoatSSAA>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetPostAAMode(mode);
		}
	}

	[Obsolete("SSAA SetAsCustom() has been deprecated. Use SetAsScale instead")]
	public void SetAsCustom(float Multiplier)
	{
		if (Multiplier < 0.1f)
		{
			Multiplier = 0.1f;
		}
		InternalRenderMode = RenderMode.Custom;
		InternalRenderMultiplier = Multiplier;
		SetDownsamplingSettings(useFilter: false);
	}

	[Obsolete("SSAA SetAsCustom() has been deprecated. Use SetAsScale instead")]
	public void SetAsCustom(float Multiplier, Filter FilterType, float sharpnessfactor, float sampledist)
	{
		if (Multiplier < 0.1f)
		{
			Multiplier = 0.1f;
		}
		InternalRenderMode = RenderMode.Custom;
		InternalRenderMultiplier = Multiplier;
		SetDownsamplingSettings(FilterType, sharpnessfactor, sampledist);
	}

	[Obsolete]
	public virtual void TakeScreenshot(string path, Vector2 Size, int multiplier, float sharpness)
	{
		TakeScreenshot(path, Size, multiplier, Filter.BICUBIC, sharpness, 1f);
	}

	[Obsolete("SSAA ScreenPointToRay has been deprecated. Use Camera's API instead")]
	public virtual Ray ScreenPointToRay(Vector3 position)
	{
		return renderCamera.ScreenPointToRay(position);
	}

	[Obsolete("SSAA ScreenToViewportPoint has been deprecated. Use Camera's API instead")]
	public virtual Vector3 ScreenToViewportPoint(Vector3 position)
	{
		return renderCamera.ScreenToViewportPoint(position);
	}

	[Obsolete("SSAA ScreenToWorldPoint has been deprecated. Use Camera's API instead")]
	public virtual Vector3 ScreenToWorldPoint(Vector3 position)
	{
		return renderCamera.ScreenToWorldPoint(position);
	}

	[Obsolete("SSAA WorldToScreenPoint has been deprecated. Use Camera's API instead")]
	public virtual Vector3 WorldToScreenPoint(Vector3 position)
	{
		return renderCamera.WorldToScreenPoint(position);
	}

	[Obsolete("SSAA ViewportToScreenPoint has been deprecated. Use Camera's API instead")]
	public virtual Vector3 ViewportToScreenPoint(Vector3 position)
	{
		return renderCamera.ViewportToScreenPoint(position);
	}

	[Obsolete("SetAsCustom() has been deprecated. Use SetAsScale instead")]
	public static void SetAllAsCustom(float Multiplier)
	{
		MadGoatSSAA[] array = UnityEngine.Object.FindObjectsOfType<MadGoatSSAA>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetAsCustom(Multiplier);
		}
	}

	[Obsolete("SetAsCustom() has been deprecated. Use SetAsScale instead")]
	public static void SetAllAsCustom(float Multiplier, Filter FilterType, float sharpnessfactor, float sampledist)
	{
		MadGoatSSAA[] array = UnityEngine.Object.FindObjectsOfType<MadGoatSSAA>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetAsCustom(Multiplier, FilterType, sharpnessfactor, sampledist);
		}
	}
}
