using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace UniStorm.Utility;

public class UniStormClouds : MonoBehaviour
{
	[HideInInspector]
	public enum CloudPerformance
	{
		Low,
		Medium,
		High,
		Ultra
	}

	[HideInInspector]
	public enum CloudShadowsType
	{
		Off,
		Simulated,
		RealTime
	}

	[HideInInspector]
	public enum CloudType
	{
		TwoD,
		Volumetric
	}

	[HideInInspector]
	public Material skyMaterial;

	[HideInInspector]
	public Material cloudsMaterial;

	[HideInInspector]
	public Material shadowsMaterial;

	[HideInInspector]
	public Material shadowsBuildingMaterial;

	[HideInInspector]
	public Transform cloudShadows;

	[HideInInspector]
	public Light sun;

	[HideInInspector]
	public Transform moon;

	[HideInInspector]
	private int[] presetResolutions = new int[4] { 1024, 2048, 2048, 2048 };

	[HideInInspector]
	private string[] keywordsA = new string[4] { "LOW", "MEDIUM", "HIGH", "ULTRA" };

	[HideInInspector]
	public CloudShadowsType CloudShadowsTypeRef;

	[HideInInspector]
	private string[] keywordsB = new string[2] { "TWOD", "VOLUMETRIC" };

	[HideInInspector]
	public CloudType cloudType = CloudType.Volumetric;

	[HideInInspector]
	public CloudPerformance performance = CloudPerformance.High;

	[HideInInspector]
	public int CloudShadowResolutionValue = 256;

	[HideInInspector]
	[Range(0f, 1f)]
	public float cloudTransparency = 0.85f;

	[HideInInspector]
	[Range(0f, 6f)]
	public int shadowBlurIterations;

	[HideInInspector]
	public CommandBuffer cloudsCommBuff;

	private int frameCount;

	private static int[] haltonSequence = new int[8] { 8, 4, 12, 2, 10, 6, 14, 1 };

	private static int[,] offset = new int[16, 2]
	{
		{ 2, 1 },
		{ 1, 2 },
		{ 2, 0 },
		{ 0, 1 },
		{ 2, 3 },
		{ 3, 2 },
		{ 3, 1 },
		{ 0, 3 },
		{ 1, 0 },
		{ 1, 1 },
		{ 3, 3 },
		{ 0, 0 },
		{ 2, 2 },
		{ 1, 3 },
		{ 3, 0 },
		{ 0, 2 }
	};

	private static int[,] bayerOffsets = new int[4, 4]
	{
		{ 0, 8, 2, 10 },
		{ 12, 4, 14, 6 },
		{ 3, 11, 1, 9 },
		{ 15, 7, 13, 5 }
	};

	private int frameIndex;

	private int haltonSequenceIndex;

	private int fullBufferIndex;

	private RenderTexture[] fullCloudsBuffer;

	private RenderTexture lowResCloudsBuffer;

	private RenderTexture[] cloudShadowsBuffer;

	[HideInInspector]
	public RenderTexture PublicCloudShadowTexture;

	private float baseCloudOffset;

	private float detailCloudOffset;

	private void Start()
	{
		if (UniStormSystem.Instance.UseRuntimeDelay == UniStormSystem.EnableFeature.Enabled)
		{
			GenerateInitialNoise();
			StartCoroutine(InitializeClouds());
		}
		else
		{
			GenerateInitialNoise();
		}
	}

	private void GenerateInitialNoise()
	{
		SetCloudDetails(performance, cloudType, CloudShadowsTypeRef);
		GetComponent<MeshRenderer>().enabled = true;
		GenerateNoise.GenerateBaseCloudNoise();
		GenerateNoise.GenerateCloudDetailNoise();
		GenerateNoise.GenerateCloudCurlNoise();
		GetComponent<MeshFilter>().sharedMesh = ProceduralHemispherePolarUVs.hemisphere;
		GetComponentsInChildren<MeshFilter>()[1].sharedMesh = ProceduralHemispherePolarUVs.hemisphereInv;
		skyMaterial.SetFloat("_uLightningTimer", 0f);
		if (CloudShadowResolutionValue == 0)
		{
			CloudShadowResolutionValue = 256;
		}
		cloudsCommBuff = new CommandBuffer();
		cloudsCommBuff.name = "Render Clouds";
		shadowsBuildingMaterial = new Material(Shader.Find("Hidden/UniStorm/CloudShadows"));
		if (UniStormSystem.Instance.UseRuntimeDelay == UniStormSystem.EnableFeature.Disabled && UniStormSystem.Instance.PlayerCamera != null)
		{
			UniStormSystem.Instance.PlayerCamera.AddCommandBuffer(CameraEvent.AfterSkybox, cloudsCommBuff);
		}
		else if (UniStormSystem.Instance.UseRuntimeDelay == UniStormSystem.EnableFeature.Disabled && UniStormSystem.Instance.PlayerCamera == null)
		{
			StartCoroutine(InitializeClouds());
		}
	}

	private IEnumerator InitializeClouds()
	{
		yield return new WaitUntil(() => UniStormSystem.Instance.UniStormInitialized);
		UniStormSystem.Instance.PlayerCamera.AddCommandBuffer(CameraEvent.AfterSkybox, cloudsCommBuff);
	}

	public void EnsureArray<T>(ref T[] array, int size, T initialValue = default(T))
	{
		if (array == null || array.Length != size)
		{
			array = new T[size];
			for (int i = 0; i != size; i++)
			{
				array[i] = initialValue;
			}
		}
	}

	public bool EnsureRenderTarget(ref RenderTexture rt, int width, int height, RenderTextureFormat format, FilterMode filterMode, string name, int depthBits = 0, int antiAliasing = 1)
	{
		if (rt != null && (rt.width != width || rt.height != height || rt.format != format || rt.filterMode != filterMode || rt.antiAliasing != antiAliasing))
		{
			RenderTexture.ReleaseTemporary(rt);
			rt = null;
		}
		if (rt == null)
		{
			rt = RenderTexture.GetTemporary(width, height, depthBits, format, RenderTextureReadWrite.Default, antiAliasing);
			rt.name = name;
			rt.filterMode = filterMode;
			rt.wrapMode = TextureWrapMode.Repeat;
			return true;
		}
		return false;
	}

	public void SetCloudDetails(CloudPerformance performance, CloudType cloudType, CloudShadowsType cloudShadowsType, bool forceRecreateTextures = false)
	{
		if (this.performance != performance || CloudShadowsTypeRef != cloudShadowsType || this.cloudType != cloudType || forceRecreateTextures)
		{
			if (cloudShadowsBuffer != null && fullCloudsBuffer.Length != 0)
			{
				cloudShadowsBuffer[0].Release();
				cloudShadowsBuffer[1].Release();
			}
			if (lowResCloudsBuffer != null)
			{
				lowResCloudsBuffer.Release();
			}
			if (fullCloudsBuffer != null && fullCloudsBuffer.Length != 0)
			{
				fullCloudsBuffer[0].Release();
				fullCloudsBuffer[1].Release();
			}
			frameCount = 0;
		}
		this.performance = performance;
		this.cloudType = cloudType;
		CloudShadowsTypeRef = cloudShadowsType;
		switch (cloudShadowsType)
		{
		case CloudShadowsType.Off:
			cloudShadows.gameObject.GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
			sun.cookie = null;
			break;
		case CloudShadowsType.Simulated:
			cloudShadows.gameObject.GetComponentsInChildren<MeshRenderer>()[1].enabled = false;
			break;
		case CloudShadowsType.RealTime:
			cloudShadows.gameObject.GetComponentsInChildren<MeshRenderer>()[1].enabled = true;
			sun.cookie = null;
			break;
		}
		string[] shaderKeywords = skyMaterial.shaderKeywords;
		foreach (string keyword in shaderKeywords)
		{
			skyMaterial.DisableKeyword(keyword);
		}
		skyMaterial.EnableKeyword(keywordsA[(int)performance]);
		skyMaterial.EnableKeyword(keywordsB[(int)cloudType]);
	}

	private void Update()
	{
		if (UniStormSystem.Instance.UniStormInitialized)
		{
			CloudsUpdate();
		}
	}

	private void CloudsUpdate()
	{
		cloudShadows.position = UniStormSystem.Instance.PlayerCamera.transform.position;
		frameIndex = (frameIndex + 1) % 16;
		if (frameIndex == 0)
		{
			haltonSequenceIndex = (haltonSequenceIndex + 1) % haltonSequence.Length;
		}
		fullBufferIndex ^= 1;
		float x = offset[frameIndex, 0];
		float y = offset[frameIndex, 1];
		frameCount++;
		if (frameCount < 25)
		{
			skyMaterial.EnableKeyword("PREWARM");
		}
		else if (frameCount == 25)
		{
			skyMaterial.DisableKeyword("PREWARM");
		}
		int num = presetResolutions[(int)performance];
		EnsureArray(ref fullCloudsBuffer, 2);
		EnsureArray(ref cloudShadowsBuffer, 2);
		EnsureRenderTarget(ref fullCloudsBuffer[0], num, num, RenderTextureFormat.ARGBHalf, FilterMode.Bilinear, "fullCloudBuff0");
		EnsureRenderTarget(ref fullCloudsBuffer[1], num, num, RenderTextureFormat.ARGBHalf, FilterMode.Bilinear, "fullCloudBuff1");
		EnsureRenderTarget(ref cloudShadowsBuffer[0], CloudShadowResolutionValue, CloudShadowResolutionValue, RenderTextureFormat.ARGBHalf, FilterMode.Bilinear, "cloudShadowBuff0");
		EnsureRenderTarget(ref cloudShadowsBuffer[1], num, num, RenderTextureFormat.ARGBHalf, FilterMode.Bilinear, "cloudShadowBuff1");
		EnsureRenderTarget(ref lowResCloudsBuffer, num / 4, num / 4, RenderTextureFormat.ARGBFloat, FilterMode.Point, "quarterCloudBuff");
		skyMaterial.SetTexture("_uBaseNoise", GenerateNoise.baseNoiseTexture);
		skyMaterial.SetTexture("_uDetailNoise", GenerateNoise.detailNoiseTexture);
		skyMaterial.SetTexture("_uCurlNoise", GenerateNoise.curlNoiseTexture);
		baseCloudOffset += skyMaterial.GetFloat("_uCloudsMovementSpeed") * Time.deltaTime;
		detailCloudOffset += skyMaterial.GetFloat("_uCloudsTurbulenceSpeed") * Time.deltaTime;
		skyMaterial.SetFloat("_uBaseCloudOffset", baseCloudOffset);
		skyMaterial.SetFloat("_uDetailCloudOffset", detailCloudOffset);
		skyMaterial.SetFloat("_uSize", num);
		skyMaterial.SetInt("_uCount", frameCount);
		skyMaterial.SetVector("_uJitter", new Vector2(x, y));
		skyMaterial.SetFloat("_uRaymarchOffset", (float)haltonSequence[haltonSequenceIndex] / 16f + (float)bayerOffsets[offset[frameIndex, 0], offset[frameIndex, 1]] / 16f);
		skyMaterial.SetVector("_uSunDir", sun.transform.forward);
		skyMaterial.SetVector("_uMoonDir", Vector3.Normalize(moon.forward));
		skyMaterial.SetVector("_uWorldSpaceCameraPos", UniStormSystem.Instance.PlayerCamera.transform.position);
		cloudsCommBuff.Clear();
		cloudsCommBuff.Blit(null, lowResCloudsBuffer, skyMaterial, 0);
		cloudsCommBuff.SetGlobalTexture("_uLowresCloudTex", lowResCloudsBuffer);
		cloudsCommBuff.SetGlobalTexture("_uPreviousCloudTex", fullCloudsBuffer[fullBufferIndex]);
		cloudsCommBuff.Blit(fullCloudsBuffer[fullBufferIndex], fullCloudsBuffer[fullBufferIndex ^ 1], skyMaterial, 1);
		switch (CloudShadowsTypeRef)
		{
		case CloudShadowsType.Simulated:
			shadowsBuildingMaterial.SetFloat("_uCloudsCoverage", skyMaterial.GetFloat("_uCloudsCoverage"));
			shadowsBuildingMaterial.SetFloat("_uCloudsCoverageBias", skyMaterial.GetFloat("_uCloudsCoverageBias"));
			shadowsBuildingMaterial.SetFloat("_uCloudsDensity", skyMaterial.GetFloat("_uCloudsDensity"));
			shadowsBuildingMaterial.SetFloat("_uCloudsDetailStrength", skyMaterial.GetFloat("_uCloudsDetailStrength"));
			shadowsBuildingMaterial.SetFloat("_uCloudsBaseEdgeSoftness", skyMaterial.GetFloat("_uCloudsBaseEdgeSoftness"));
			shadowsBuildingMaterial.SetFloat("_uCloudsBottomSoftness", skyMaterial.GetFloat("_uCloudsBottomSoftness"));
			shadowsBuildingMaterial.SetFloat("_uSimulatedCloudAlpha", cloudTransparency);
			cloudsCommBuff.Blit(GenerateNoise.baseNoiseTexture, cloudShadowsBuffer[0], shadowsBuildingMaterial, 3);
			PublicCloudShadowTexture = cloudShadowsBuffer[0];
			break;
		case CloudShadowsType.RealTime:
		{
			cloudsCommBuff.Blit(fullCloudsBuffer[fullBufferIndex ^ 1], cloudShadowsBuffer[0]);
			for (int i = 0; i < shadowBlurIterations; i++)
			{
				cloudsCommBuff.Blit(cloudShadowsBuffer[0], cloudShadowsBuffer[1], shadowsBuildingMaterial, 1);
				cloudsCommBuff.Blit(cloudShadowsBuffer[1], cloudShadowsBuffer[0], shadowsBuildingMaterial, 2);
			}
			break;
		}
		}
		cloudsCommBuff.SetGlobalFloat("_uLightning", 0f);
		cloudsMaterial.SetTexture("_MainTex", fullCloudsBuffer[fullBufferIndex ^ 1]);
	}
}
