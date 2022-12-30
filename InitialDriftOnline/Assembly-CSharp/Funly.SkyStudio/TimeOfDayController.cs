using UnityEngine;

namespace Funly.SkyStudio;

[ExecuteInEditMode]
public class TimeOfDayController : MonoBehaviour
{
	public delegate void TimeOfDayDidChange(TimeOfDayController tc, float timeOfDay);

	[Tooltip("Sky profile defines the skyColors configuration for times of day. This script will animate between those skyColors values based on the time of day.")]
	[SerializeField]
	private SkyProfile m_SkyProfile;

	[Tooltip("Time is expressed in a fractional number of days that have completed.")]
	[SerializeField]
	private float m_SkyTime;

	[Tooltip("Automatically advance time at fixed speed.")]
	public bool automaticTimeIncrement;

	[Tooltip("Create a copy of the sky profile at runtime, so modifications don't affect the original Sky Profile in your project.")]
	public bool copySkyProfile;

	private SkyMaterialController m_SkyMaterialController;

	[Tooltip("Speed at which to advance time by if in automatic increment is enabled.")]
	[Range(0f, 1f)]
	public float automaticIncrementSpeed = 0.01f;

	[Tooltip("Sun orbit.")]
	public OrbitingBody sunOrbit;

	[Tooltip("Moon orbit.")]
	public OrbitingBody moonOrbit;

	[Tooltip("Controller for managing weather effects")]
	public WeatherController weatherController;

	[Tooltip("If true we'll invoke DynamicGI.UpdateEnvironment() when skybox changes. This is an expensive operation.")]
	public bool updateGlobalIllumination;

	private bool m_DidInitialUpdate;

	public static TimeOfDayController instance { get; private set; }

	public SkyProfile skyProfile
	{
		get
		{
			return m_SkyProfile;
		}
		set
		{
			if (value != null && copySkyProfile)
			{
				m_SkyProfile = Object.Instantiate(value);
			}
			else
			{
				m_SkyProfile = value;
			}
			m_SkyMaterialController = null;
			UpdateSkyForCurrentTime();
			SynchronizeAllShaderKeywords();
		}
	}

	public float skyTime
	{
		get
		{
			return m_SkyTime;
		}
		set
		{
			m_SkyTime = Mathf.Abs(value);
			UpdateSkyForCurrentTime();
		}
	}

	public SkyMaterialController SkyMaterial => m_SkyMaterialController;

	public float timeOfDay => m_SkyTime - (float)(int)m_SkyTime;

	public int daysElapsed => (int)m_SkyTime;

	public event TimeOfDayDidChange timeChangedCallback;

	private void Awake()
	{
		instance = this;
	}

	private void OnEnabled()
	{
		skyTime = m_SkyTime;
	}

	private void OnValidate()
	{
		if (base.gameObject.activeInHierarchy)
		{
			skyTime = m_SkyTime;
			skyProfile = m_SkyProfile;
		}
	}

	private void WarnInvalidSkySetup()
	{
		Debug.LogError("Your SkySystemController has an old or invalid prefab layout! Please run the upgrade tool in 'Windows -> Sky Studio -> Upgrade Sky System Controller'. Do not rename or modify any of the children in the SkySystemController hierarchy.");
	}

	private void Update()
	{
		if (!skyProfile)
		{
			return;
		}
		if (automaticTimeIncrement && Application.isPlaying)
		{
			skyTime += automaticIncrementSpeed * Time.deltaTime;
		}
		if (sunOrbit == null || moonOrbit == null || sunOrbit.rotateBody == null || moonOrbit.rotateBody == null || sunOrbit.positionTransform == null || moonOrbit.positionTransform == null)
		{
			WarnInvalidSkySetup();
			return;
		}
		if (!m_DidInitialUpdate)
		{
			UpdateSkyForCurrentTime();
			m_DidInitialUpdate = true;
		}
		if (weatherController != null)
		{
			weatherController.UpdateForTimeOfDay(skyProfile, timeOfDay);
		}
		if (skyProfile.IsFeatureEnabled("SunFeature"))
		{
			if ((bool)sunOrbit.positionTransform)
			{
				m_SkyMaterialController.SunWorldToLocalMatrix = sunOrbit.positionTransform.worldToLocalMatrix;
			}
			if (skyProfile.IsFeatureEnabled("SunCustomTextureFeature"))
			{
				if (skyProfile.IsFeatureEnabled("SunRotationFeature"))
				{
					sunOrbit.rotateBody.AllowSpinning = true;
					sunOrbit.rotateBody.SpinSpeed = skyProfile.GetNumberPropertyValue("SunRotationSpeedKey", timeOfDay);
				}
				else
				{
					sunOrbit.rotateBody.AllowSpinning = false;
				}
			}
		}
		if (!skyProfile.IsFeatureEnabled("MoonFeature"))
		{
			return;
		}
		if ((bool)moonOrbit.positionTransform)
		{
			m_SkyMaterialController.MoonWorldToLocalMatrix = moonOrbit.positionTransform.worldToLocalMatrix;
		}
		if (skyProfile.IsFeatureEnabled("MoonCustomTextureFeature"))
		{
			if (skyProfile.IsFeatureEnabled("MoonRotationFeature"))
			{
				moonOrbit.rotateBody.AllowSpinning = true;
				moonOrbit.rotateBody.SpinSpeed = skyProfile.GetNumberPropertyValue("MoonRotationSpeedKey", timeOfDay);
			}
			else
			{
				moonOrbit.rotateBody.AllowSpinning = false;
			}
		}
	}

	public void UpdateGlobalIllumination()
	{
		DynamicGI.UpdateEnvironment();
	}

	private void SynchronizeAllShaderKeywords()
	{
		if (m_SkyProfile == null)
		{
			return;
		}
		ProfileFeatureSection[] features = m_SkyProfile.profileDefinition.features;
		for (int i = 0; i < features.Length; i++)
		{
			ProfileFeatureDefinition[] featureDefinitions = features[i].featureDefinitions;
			foreach (ProfileFeatureDefinition profileFeatureDefinition in featureDefinitions)
			{
				if (profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.ShaderKeyword)
				{
					SynchronizedShaderKeyword(profileFeatureDefinition.featureKey, profileFeatureDefinition.shaderKeyword);
				}
				else if (profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.ShaderKeywordDropdown)
				{
					for (int k = 0; k < profileFeatureDefinition.featureKeys.Length; k++)
					{
						SynchronizedShaderKeyword(profileFeatureDefinition.featureKeys[k], profileFeatureDefinition.shaderKeywords[k]);
					}
				}
			}
		}
	}

	private void SynchronizedShaderKeyword(string featureKey, string shaderKeyword)
	{
		if (skyProfile == null || skyProfile.skyboxMaterial == null)
		{
			return;
		}
		if (skyProfile.IsFeatureEnabled(featureKey))
		{
			if (!skyProfile.skyboxMaterial.IsKeywordEnabled(shaderKeyword))
			{
				skyProfile.skyboxMaterial.EnableKeyword(shaderKeyword);
			}
		}
		else if (skyProfile.skyboxMaterial.IsKeywordEnabled(shaderKeyword))
		{
			skyProfile.skyboxMaterial.DisableKeyword(shaderKeyword);
		}
	}

	private Vector3 GetPrimaryLightDirection()
	{
		if (skyProfile.IsFeatureEnabled("SunFeature") && (bool)sunOrbit)
		{
			return sunOrbit.BodyGlobalDirection;
		}
		if (skyProfile.IsFeatureEnabled("MoonFeature") && (bool)moonOrbit)
		{
			return moonOrbit.BodyGlobalDirection;
		}
		return new Vector3(0f, 1f, 0f);
	}

	public void UpdateSkyForCurrentTime()
	{
		if (skyProfile == null)
		{
			return;
		}
		if (skyProfile.skyboxMaterial == null)
		{
			Debug.LogError("Your sky profile is missing a reference to the skybox material.");
			return;
		}
		if (m_SkyMaterialController == null)
		{
			m_SkyMaterialController = new SkyMaterialController();
		}
		m_SkyMaterialController.SkyboxMaterial = skyProfile.skyboxMaterial;
		if (RenderSettings.skybox == null || RenderSettings.skybox.GetInstanceID() != skyProfile.skyboxMaterial.GetInstanceID())
		{
			RenderSettings.skybox = skyProfile.skyboxMaterial;
		}
		SynchronizeAllShaderKeywords();
		m_SkyMaterialController.BackgroundCubemap = skyProfile.GetTexturePropertyValue("SkyCubemapKey", timeOfDay) as Cubemap;
		m_SkyMaterialController.SkyColor = skyProfile.GetColorPropertyValue("SkyUpperColorKey", timeOfDay);
		m_SkyMaterialController.SkyMiddleColor = skyProfile.GetColorPropertyValue("SkyMiddleColorKey", timeOfDay);
		m_SkyMaterialController.HorizonColor = skyProfile.GetColorPropertyValue("SkyLowerColorKey", timeOfDay);
		m_SkyMaterialController.GradientFadeBegin = skyProfile.GetNumberPropertyValue("HorizonTransitionStartKey", timeOfDay);
		m_SkyMaterialController.GradientFadeLength = skyProfile.GetNumberPropertyValue("HorizonTransitionLengthKey", timeOfDay);
		m_SkyMaterialController.SkyMiddlePosition = skyProfile.GetNumberPropertyValue("SkyMiddleColorPosition", timeOfDay);
		m_SkyMaterialController.StarFadeBegin = skyProfile.GetNumberPropertyValue("StarTransitionStartKey", timeOfDay);
		m_SkyMaterialController.StarFadeLength = skyProfile.GetNumberPropertyValue("StarTransitionLengthKey", timeOfDay);
		m_SkyMaterialController.HorizonDistanceScale = skyProfile.GetNumberPropertyValue("HorizonStarScaleKey", timeOfDay);
		if (skyProfile.IsFeatureEnabled("CloudFeature"))
		{
			if (skyProfile.IsFeatureEnabled("NoiseCloudFeature"))
			{
				m_SkyMaterialController.CloudTexture = skyProfile.GetTexturePropertyValue("CloudNoiseTextureKey", timeOfDay);
				m_SkyMaterialController.CloudTextureTiling = skyProfile.GetNumberPropertyValue("CloudTextureTiling", timeOfDay);
				m_SkyMaterialController.CloudDensity = skyProfile.GetNumberPropertyValue("CloudDensityKey", timeOfDay);
				m_SkyMaterialController.CloudSpeed = skyProfile.GetNumberPropertyValue("CloudSpeedKey", timeOfDay);
				m_SkyMaterialController.CloudDirection = skyProfile.GetNumberPropertyValue("CloudDirectionKey", timeOfDay);
				m_SkyMaterialController.CloudHeight = skyProfile.GetNumberPropertyValue("CloudHeightKey", timeOfDay);
				m_SkyMaterialController.CloudColor1 = skyProfile.GetColorPropertyValue("CloudColor1Key", timeOfDay);
				m_SkyMaterialController.CloudColor2 = skyProfile.GetColorPropertyValue("CloudColor2Key", timeOfDay);
				m_SkyMaterialController.CloudFadePosition = skyProfile.GetNumberPropertyValue("CloudFadePositionKey", timeOfDay);
				m_SkyMaterialController.CloudFadeAmount = skyProfile.GetNumberPropertyValue("CloudFadeAmountKey", timeOfDay);
			}
			else if (skyProfile.IsFeatureEnabled("CubemapCloudFeature"))
			{
				m_SkyMaterialController.CloudCubemap = skyProfile.GetTexturePropertyValue("CloudCubemapTextureKey", timeOfDay);
				m_SkyMaterialController.CloudCubemapRotationSpeed = skyProfile.GetNumberPropertyValue("CloudCubemapRotationSpeedKey", timeOfDay);
				m_SkyMaterialController.CloudCubemapTintColor = skyProfile.GetColorPropertyValue("CloudCubemapTintColorKey", timeOfDay);
				m_SkyMaterialController.CloudCubemapHeight = skyProfile.GetNumberPropertyValue("CloudCubemapHeightKey", timeOfDay);
				if (skyProfile.IsFeatureEnabled("CubemapCloudDoubleLayerFeature"))
				{
					m_SkyMaterialController.CloudCubemapDoubleLayerHeight = skyProfile.GetNumberPropertyValue("CloudCubemapDoubleLayerHeightKey", timeOfDay);
					m_SkyMaterialController.CloudCubemapDoubleLayerRotationSpeed = skyProfile.GetNumberPropertyValue("CloudCubemapDoubleLayerRotationSpeedKey", timeOfDay);
					m_SkyMaterialController.CloudCubemapDoubleLayerTintColor = skyProfile.GetColorPropertyValue("CloudCubemapDoubleLayerTintColorKey", timeOfDay);
					if (skyProfile.IsFeatureEnabled("CubemapCloudDoubleLayerCubemap"))
					{
						m_SkyMaterialController.CloudCubemapDoubleLayerCustomTexture = skyProfile.GetTexturePropertyValue("CloudCubemapDoubleLayerCustomTextureKey", timeOfDay);
					}
				}
			}
			else if (skyProfile.IsFeatureEnabled("CubemapNormalCloudFeature"))
			{
				m_SkyMaterialController.CloudCubemapNormalLightDirection = GetPrimaryLightDirection();
				m_SkyMaterialController.CloudCubemapNormalTexture = skyProfile.GetTexturePropertyValue("CloudCubemapNormalTextureKey", timeOfDay);
				m_SkyMaterialController.CloudCubemapNormalLitColor = skyProfile.GetColorPropertyValue("CloudCubemapNormalLitColorKey", timeOfDay);
				m_SkyMaterialController.CloudCubemapNormalShadowColor = skyProfile.GetColorPropertyValue("CloudCubemapNormalShadowColorKey", timeOfDay);
				m_SkyMaterialController.CloudCubemapNormalAmbientIntensity = skyProfile.GetNumberPropertyValue("CloudCubemapNormalAmbientIntensityKey", timeOfDay);
				m_SkyMaterialController.CloudCubemapNormalHeight = skyProfile.GetNumberPropertyValue("CloudCubemapNormalHeightKey", timeOfDay);
				m_SkyMaterialController.CloudCubemapNormalRotationSpeed = skyProfile.GetNumberPropertyValue("CloudCubemapNormalRotationSpeedKey", timeOfDay);
				if (skyProfile.IsFeatureEnabled("CubemapNormalCloudDoubleLayerFeature"))
				{
					m_SkyMaterialController.CloudCubemapNormalDoubleLayerHeight = skyProfile.GetNumberPropertyValue("CloudCubemapNormalDoubleLayerHeightKey", timeOfDay);
					m_SkyMaterialController.CloudCubemapNormalDoubleLayerRotationSpeed = skyProfile.GetNumberPropertyValue("CloudCubemapNormalDoubleLayerRotationSpeedKey", timeOfDay);
					m_SkyMaterialController.CloudCubemapNormalDoubleLayerLitColor = skyProfile.GetColorPropertyValue("CloudCubemapNormalDoubleLayerLitColorKey", timeOfDay);
					m_SkyMaterialController.CloudCubemapNormalDoubleLayerShadowColor = skyProfile.GetColorPropertyValue("CloudCubemapNormalDoubleLayerShadowKey", timeOfDay);
					if (skyProfile.IsFeatureEnabled("CubemapNormalCloudDoubleLayerCubemap"))
					{
						m_SkyMaterialController.CloudCubemapNormalDoubleLayerCustomTexture = skyProfile.GetTexturePropertyValue("CloudCubemapNormalDoubleLayerCustomTextureKey", timeOfDay);
					}
				}
			}
		}
		if (skyProfile.IsFeatureEnabled("FogFeature"))
		{
			Color colorPropertyValue = skyProfile.GetColorPropertyValue("FogColorKey", timeOfDay);
			m_SkyMaterialController.FogColor = colorPropertyValue;
			m_SkyMaterialController.FogDensity = skyProfile.GetNumberPropertyValue("FogDensityKey", timeOfDay);
			m_SkyMaterialController.FogHeight = skyProfile.GetNumberPropertyValue("FogLengthKey", timeOfDay);
			if (skyProfile.GetBoolPropertyValue("FogSyncWithGlobal", timeOfDay))
			{
				RenderSettings.fogColor = colorPropertyValue;
			}
		}
		if (skyProfile.IsFeatureEnabled("SunFeature") && (bool)sunOrbit)
		{
			sunOrbit.Point = skyProfile.GetSpherePointPropertyValue("SunPositionKey", timeOfDay);
			m_SkyMaterialController.SunDirection = sunOrbit.BodyGlobalDirection;
			m_SkyMaterialController.SunColor = skyProfile.GetColorPropertyValue("SunColorKey", timeOfDay);
			m_SkyMaterialController.SunSize = skyProfile.GetNumberPropertyValue("SunSizeKey", timeOfDay);
			m_SkyMaterialController.SunEdgeFeathering = skyProfile.GetNumberPropertyValue("SunEdgeFeatheringKey", timeOfDay);
			m_SkyMaterialController.SunBloomFilterBoost = skyProfile.GetNumberPropertyValue("SunColorIntensityKey", timeOfDay);
			if (skyProfile.IsFeatureEnabled("SunCustomTextureFeature"))
			{
				m_SkyMaterialController.SunWorldToLocalMatrix = sunOrbit.positionTransform.worldToLocalMatrix;
				m_SkyMaterialController.SunTexture = skyProfile.GetTexturePropertyValue("SunTextureKey", timeOfDay);
				if (skyProfile.IsFeatureEnabled("SunRotationFeature"))
				{
					sunOrbit.rotateBody.SpinSpeed = skyProfile.GetNumberPropertyValue("SunRotationSpeedKey", timeOfDay);
				}
			}
			if (skyProfile.IsFeatureEnabled("SunSpriteSheetFeature"))
			{
				m_SkyMaterialController.SetSunSpriteDimensions((int)skyProfile.GetNumberPropertyValue("SunSpriteColumnCountKey", timeOfDay), (int)skyProfile.GetNumberPropertyValue("SunSpriteRowCountKey", timeOfDay));
				m_SkyMaterialController.SunSpriteItemCount = (int)skyProfile.GetNumberPropertyValue("SunSpriteItemCount", timeOfDay);
				m_SkyMaterialController.SunSpriteAnimationSpeed = skyProfile.GetNumberPropertyValue("SunSpriteAnimationSpeed", timeOfDay);
			}
			if ((bool)sunOrbit.BodyLight)
			{
				if (!sunOrbit.BodyLight.enabled)
				{
					sunOrbit.BodyLight.enabled = true;
				}
				RenderSettings.sun = sunOrbit.BodyLight;
				sunOrbit.BodyLight.color = skyProfile.GetColorPropertyValue("SunLightColorKey", timeOfDay);
				sunOrbit.BodyLight.intensity = skyProfile.GetNumberPropertyValue("SunLightIntensityKey", timeOfDay);
			}
		}
		else if ((bool)sunOrbit && (bool)sunOrbit.BodyLight)
		{
			sunOrbit.BodyLight.enabled = false;
		}
		if (skyProfile.IsFeatureEnabled("MoonFeature") && (bool)moonOrbit)
		{
			moonOrbit.Point = skyProfile.GetSpherePointPropertyValue("MoonPositionKey", timeOfDay);
			m_SkyMaterialController.MoonDirection = moonOrbit.BodyGlobalDirection;
			m_SkyMaterialController.MoonColor = skyProfile.GetColorPropertyValue("MoonColorKey", timeOfDay);
			m_SkyMaterialController.MoonSize = skyProfile.GetNumberPropertyValue("MoonSizeKey", timeOfDay);
			m_SkyMaterialController.MoonEdgeFeathering = skyProfile.GetNumberPropertyValue("MoonEdgeFeatheringKey", timeOfDay);
			m_SkyMaterialController.MoonBloomFilterBoost = skyProfile.GetNumberPropertyValue("MoonColorIntensityKey", timeOfDay);
			if (skyProfile.IsFeatureEnabled("MoonCustomTextureFeature"))
			{
				m_SkyMaterialController.MoonTexture = skyProfile.GetTexturePropertyValue("MoonTextureKey", timeOfDay);
				m_SkyMaterialController.MoonWorldToLocalMatrix = moonOrbit.positionTransform.worldToLocalMatrix;
				if (skyProfile.IsFeatureEnabled("MoonRotationFeature"))
				{
					moonOrbit.rotateBody.SpinSpeed = skyProfile.GetNumberPropertyValue("MoonRotationSpeedKey", timeOfDay);
				}
			}
			if (skyProfile.IsFeatureEnabled("MoonSpriteSheetFeature"))
			{
				m_SkyMaterialController.SetMoonSpriteDimensions((int)skyProfile.GetNumberPropertyValue("MoonSpriteColumnCountKey", timeOfDay), (int)skyProfile.GetNumberPropertyValue("MoonSpriteRowCountKey", timeOfDay));
				m_SkyMaterialController.MoonSpriteItemCount = (int)skyProfile.GetNumberPropertyValue("MoonSpriteItemCount", timeOfDay);
				m_SkyMaterialController.MoonSpriteAnimationSpeed = skyProfile.GetNumberPropertyValue("MoonSpriteAnimationSpeed", timeOfDay);
			}
			if ((bool)moonOrbit.BodyLight)
			{
				if (!moonOrbit.BodyLight.enabled)
				{
					moonOrbit.BodyLight.enabled = true;
				}
				moonOrbit.BodyLight.color = skyProfile.GetColorPropertyValue("MoonLightColorKey", timeOfDay);
				moonOrbit.BodyLight.intensity = skyProfile.GetNumberPropertyValue("MoonLightIntensityKey", timeOfDay);
			}
		}
		else if ((bool)moonOrbit && (bool)moonOrbit.BodyLight)
		{
			moonOrbit.BodyLight.enabled = false;
		}
		if (skyProfile.IsFeatureEnabled("StarBasicFeature"))
		{
			m_SkyMaterialController.StarBasicCubemap = skyProfile.GetTexturePropertyValue("StarBasicCubemapKey", timeOfDay);
			m_SkyMaterialController.StarBasicTwinkleSpeed = skyProfile.GetNumberPropertyValue("StarBasicTwinkleSpeedKey", timeOfDay);
			m_SkyMaterialController.StarBasicTwinkleAmount = skyProfile.GetNumberPropertyValue("StarBasicTwinkleAmountKey", timeOfDay);
			m_SkyMaterialController.StarBasicOpacity = skyProfile.GetNumberPropertyValue("StarBasicOpacityKey", timeOfDay);
			m_SkyMaterialController.StarBasicTintColor = skyProfile.GetColorPropertyValue("StarBasicTintColorKey", timeOfDay);
			m_SkyMaterialController.StarBasicExponent = skyProfile.GetNumberPropertyValue("StarBasicExponentKey", timeOfDay);
			m_SkyMaterialController.StarBasicIntensity = skyProfile.GetNumberPropertyValue("StarBasicIntensityKey", timeOfDay);
		}
		else
		{
			if (skyProfile.IsFeatureEnabled("StarLayer1Feature"))
			{
				m_SkyMaterialController.StarLayer1DataTexture = skyProfile.starLayer1DataTexture;
				m_SkyMaterialController.StarLayer1Color = skyProfile.GetColorPropertyValue("Star1ColorKey", timeOfDay);
				m_SkyMaterialController.StarLayer1MaxRadius = skyProfile.GetNumberPropertyValue("Star1SizeKey", timeOfDay);
				m_SkyMaterialController.StarLayer1Texture = skyProfile.GetTexturePropertyValue("Star1TextureKey", timeOfDay);
				m_SkyMaterialController.StarLayer1TwinkleAmount = skyProfile.GetNumberPropertyValue("Star1TwinkleAmountKey", timeOfDay);
				m_SkyMaterialController.StarLayer1TwinkleSpeed = skyProfile.GetNumberPropertyValue("Star1TwinkleSpeedKey", timeOfDay);
				m_SkyMaterialController.StarLayer1RotationSpeed = skyProfile.GetNumberPropertyValue("Star1RotationSpeed", timeOfDay);
				m_SkyMaterialController.StarLayer1EdgeFeathering = skyProfile.GetNumberPropertyValue("Star1EdgeFeathering", timeOfDay);
				m_SkyMaterialController.StarLayer1BloomFilterBoost = skyProfile.GetNumberPropertyValue("Star1ColorIntensityKey", timeOfDay);
				if (skyProfile.IsFeatureEnabled("StarLayer1SpriteSheetFeature"))
				{
					m_SkyMaterialController.StarLayer1SpriteItemCount = (int)skyProfile.GetNumberPropertyValue("Star1SpriteItemCount", timeOfDay);
					m_SkyMaterialController.StarLayer1SpriteAnimationSpeed = (int)skyProfile.GetNumberPropertyValue("Star1SpriteAnimationSpeed", timeOfDay);
					m_SkyMaterialController.SetStarLayer1SpriteDimensions((int)skyProfile.GetNumberPropertyValue("Star1SpriteColumnCountKey", timeOfDay), (int)skyProfile.GetNumberPropertyValue("Star1SpriteRowCountKey", timeOfDay));
				}
			}
			if (skyProfile.IsFeatureEnabled("StarLayer2Feature"))
			{
				m_SkyMaterialController.StarLayer2DataTexture = skyProfile.starLayer2DataTexture;
				m_SkyMaterialController.StarLayer2Color = skyProfile.GetColorPropertyValue("Star2ColorKey", timeOfDay);
				m_SkyMaterialController.StarLayer2MaxRadius = skyProfile.GetNumberPropertyValue("Star2SizeKey", timeOfDay);
				m_SkyMaterialController.StarLayer2Texture = skyProfile.GetTexturePropertyValue("Star2TextureKey", timeOfDay);
				m_SkyMaterialController.StarLayer2TwinkleAmount = skyProfile.GetNumberPropertyValue("Star2TwinkleAmountKey", timeOfDay);
				m_SkyMaterialController.StarLayer2TwinkleSpeed = skyProfile.GetNumberPropertyValue("Star2TwinkleSpeedKey", timeOfDay);
				m_SkyMaterialController.StarLayer2RotationSpeed = skyProfile.GetNumberPropertyValue("Star2RotationSpeed", timeOfDay);
				m_SkyMaterialController.StarLayer2EdgeFeathering = skyProfile.GetNumberPropertyValue("Star2EdgeFeathering", timeOfDay);
				m_SkyMaterialController.StarLayer2BloomFilterBoost = skyProfile.GetNumberPropertyValue("Star2ColorIntensityKey", timeOfDay);
				if (skyProfile.IsFeatureEnabled("StarLayer2SpriteSheetFeature"))
				{
					m_SkyMaterialController.StarLayer2SpriteItemCount = (int)skyProfile.GetNumberPropertyValue("Star2SpriteItemCount", timeOfDay);
					m_SkyMaterialController.StarLayer2SpriteAnimationSpeed = (int)skyProfile.GetNumberPropertyValue("Star2SpriteAnimationSpeed", timeOfDay);
					m_SkyMaterialController.SetStarLayer2SpriteDimensions((int)skyProfile.GetNumberPropertyValue("Star2SpriteColumnCountKey", timeOfDay), (int)skyProfile.GetNumberPropertyValue("Star2SpriteRowCountKey", timeOfDay));
				}
			}
			if (skyProfile.IsFeatureEnabled("StarLayer3Feature"))
			{
				m_SkyMaterialController.StarLayer3DataTexture = skyProfile.starLayer3DataTexture;
				m_SkyMaterialController.StarLayer3Color = skyProfile.GetColorPropertyValue("Star3ColorKey", timeOfDay);
				m_SkyMaterialController.StarLayer3MaxRadius = skyProfile.GetNumberPropertyValue("Star3SizeKey", timeOfDay);
				m_SkyMaterialController.StarLayer3Texture = skyProfile.GetTexturePropertyValue("Star3TextureKey", timeOfDay);
				m_SkyMaterialController.StarLayer3TwinkleAmount = skyProfile.GetNumberPropertyValue("Star3TwinkleAmountKey", timeOfDay);
				m_SkyMaterialController.StarLayer3TwinkleSpeed = skyProfile.GetNumberPropertyValue("Star3TwinkleSpeedKey", timeOfDay);
				m_SkyMaterialController.StarLayer3RotationSpeed = skyProfile.GetNumberPropertyValue("Star3RotationSpeed", timeOfDay);
				m_SkyMaterialController.StarLayer3EdgeFeathering = skyProfile.GetNumberPropertyValue("Star3EdgeFeathering", timeOfDay);
				m_SkyMaterialController.StarLayer3BloomFilterBoost = skyProfile.GetNumberPropertyValue("Star3ColorIntensityKey", timeOfDay);
				if (skyProfile.IsFeatureEnabled("StarLayer3SpriteSheetFeature"))
				{
					m_SkyMaterialController.StarLayer3SpriteItemCount = (int)skyProfile.GetNumberPropertyValue("Star3SpriteItemCount", timeOfDay);
					m_SkyMaterialController.StarLayer3SpriteAnimationSpeed = (int)skyProfile.GetNumberPropertyValue("Star3SpriteAnimationSpeed", timeOfDay);
					m_SkyMaterialController.SetStarLayer3SpriteDimensions((int)skyProfile.GetNumberPropertyValue("Star3SpriteColumnCountKey", timeOfDay), (int)skyProfile.GetNumberPropertyValue("Star3SpriteRowCountKey", timeOfDay));
				}
			}
		}
		if (updateGlobalIllumination)
		{
			UpdateGlobalIllumination();
		}
		if (this.timeChangedCallback != null)
		{
			this.timeChangedCallback(this, timeOfDay);
		}
	}
}
