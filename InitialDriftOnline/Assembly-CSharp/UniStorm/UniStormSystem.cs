using System;
using System.Collections;
using System.Collections.Generic;
using UniStorm.Effects;
using UniStorm.Utility;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace UniStorm;

public class UniStormSystem : MonoBehaviour
{
	public enum UniStormProfileTypeEnum
	{
		Import,
		Export
	}

	public enum PlatformTypeEnum
	{
		Desktop,
		VR,
		Mobile
	}

	public enum GetPlayerMethodEnum
	{
		ByTag,
		ByName
	}

	public enum UseTimeOfDayUpdateSeconds
	{
		Yes,
		No
	}

	public enum CurrentTimeOfDayEnum
	{
		Morning,
		Day,
		Evening,
		Night
	}

	public enum WeatherGenerationMethodEnum
	{
		Hourly,
		Daily
	}

	public enum EnableFeature
	{
		Enabled,
		Disabled
	}

	public enum CloudShadowResolutionEnum
	{
		_256x256,
		_512x512,
		_1024x1024
	}

	public enum FogTypeEnum
	{
		UnistormFog,
		UnityFog
	}

	public enum FogModeEnum
	{
		Exponential,
		ExponentialSquared
	}

	public enum CurrentSeasonEnum
	{
		Spring = 1,
		Summer,
		Fall,
		Winter
	}

	public enum CloudTypeEnum
	{
		_2D,
		Volumetric
	}

	public enum CloudQualityEnum
	{
		Low,
		Medium,
		High,
		Ultra
	}

	public enum TemperatureTypeEnum
	{
		Fahrenheit,
		Celsius
	}

	public enum HemisphereEnum
	{
		Northern,
		Southern
	}

	[Serializable]
	public class MoonPhaseClass
	{
		public Texture MoonPhaseTexture;

		public float MoonPhaseIntensity = 1f;
	}

	public enum StarmapTypeEnum
	{
		VeryStrongConstellations,
		StrongConstellations,
		LightConstellations
	}

	public enum CloudRenderTypeEnum
	{
		Transparent,
		Opaque
	}

	public static UniStormSystem Instance;

	public UnityEvent OnHourChangeEvent;

	public UnityEvent OnDayChangeEvent;

	public UnityEvent OnMonthChangeEvent;

	public UnityEvent OnYearChangeEvent;

	public UnityEvent OnWeatherChangeEvent;

	public UnityEvent OnWeatherGenerationEvent;

	public UnityEvent OnLightningStrikePlayerEvent;

	public UnityEvent OnLightningStrikeObjectEvent;

	public float WeatherSoundsVolume = 1f;

	public float AmbienceVolume = 1f;

	public float MusicVolume = 1f;

	public Slider TimeSlider;

	public GameObject WeatherButtonGameObject;

	public GameObject TimeSliderGameObject;

	public Dropdown WeatherDropdown;

	public EnableFeature UseUniStormMenu;

	public KeyCode UniStormMenuKey = KeyCode.Escape;

	public GameObject UniStormCanvas;

	public bool m_MenuToggle = true;

	public int TabNumber;

	public int TimeTabNumbers;

	public int WeatherTabNumbers;

	public int CelestialTabNumbers;

	public bool TimeFoldout = true;

	public bool DateFoldout = true;

	public bool TimeSoundsFoldout = true;

	public bool TimeMusicFoldout = true;

	public bool SunFoldout = true;

	public bool MoonFoldout = true;

	public bool AtmosphereFoldout = true;

	public bool FogFoldout = true;

	public bool WeatherFoldout = true;

	public bool LightningFoldout = true;

	public bool CameraFoldout = true;

	public bool SettingsFoldout = true;

	public bool CloudsFoldout = true;

	public bool PlatformFoldout = true;

	public UniStormProfile m_UniStormProfile;

	public string FilePath = "";

	public UniStormProfileTypeEnum UniStormProfileType;

	public PlatformTypeEnum PlatformType;

	public Transform PlayerTransform;

	public Camera PlayerCamera;

	public bool m_PlayerFound;

	public EnableFeature GetPlayerAtRuntime = EnableFeature.Disabled;

	public EnableFeature UseRuntimeDelay = EnableFeature.Disabled;

	public GetPlayerMethodEnum GetPlayerMethod;

	public string PlayerTag = "Player";

	public string PlayerName = "Player";

	public string CameraTag = "MainCamera";

	public string CameraName = "MainCamera";

	public DateTime UniStormDate;

	public int StartingMinute;

	public int StartingHour;

	public int Minute = 1;

	public int Hour;

	public int Day;

	public int Month;

	public int Year;

	public int DayLength = 10;

	public int NightLength = 10;

	public int TimeOfDayUpdateSeconds;

	public UseTimeOfDayUpdateSeconds UseTimeOfDayUpdateControl = UseTimeOfDayUpdateSeconds.No;

	private float TimeOfDayUpdateTimer;

	public float m_TimeFloat;

	public EnableFeature TimeFlow;

	public EnableFeature RealWorldTime = EnableFeature.Disabled;

	private float m_roundingCorrection;

	private float m_PreciseCurveTime;

	public bool m_HourUpdate;

	private float m_TimeOfDaySoundsTimer;

	private int m_TimeOfDaySoundsSeconds = 10;

	public int TimeOfDaySoundsSecondsMin = 10;

	public int TimeOfDaySoundsSecondsMax = 30;

	public List<AudioClip> MorningSounds = new List<AudioClip>();

	public List<AudioClip> DaySounds = new List<AudioClip>();

	public List<AudioClip> EveningSounds = new List<AudioClip>();

	public List<AudioClip> NightSounds = new List<AudioClip>();

	public AudioSource TimeOfDayAudioSource;

	public List<AudioClip> MorningMusic = new List<AudioClip>();

	public List<AudioClip> DayMusic = new List<AudioClip>();

	public List<AudioClip> EveningMusic = new List<AudioClip>();

	public List<AudioClip> NightMusic = new List<AudioClip>();

	public AudioSource TimeOfDayMusicAudioSource;

	public int TimeOfDayMusicDelay = 1;

	private float m_CurrentMusicClipLength;

	private float m_TimeOfDayMusicTimer;

	public EnableFeature TimeOfDaySoundsDuringPrecipitationWeather = EnableFeature.Disabled;

	public EnableFeature TransitionMusicOnTimeOfDayChange = EnableFeature.Disabled;

	private float m_CurrentClipLength;

	public bool m_UpdateTimeOfDayMusic;

	public bool m_UpdateBiomeTimeOfDayMusic;

	public int MusicTransitionLength = 3;

	private int m_LastHour;

	public CurrentTimeOfDayEnum CurrentTimeOfDay;

	public WeatherGenerationMethodEnum WeatherGenerationMethod = WeatherGenerationMethodEnum.Daily;

	public List<WeatherType> WeatherForecast = new List<WeatherType>();

	public EnableFeature ForceLowClouds = EnableFeature.Disabled;

	public int LowCloudHeight = 225;

	public int CloudDomeTrisCountX = 48;

	public int CloudDomeTrisCountY = 32;

	public bool IgnoreConditions;

	public AnimationCurve CloudyFadeControl = AnimationCurve.Linear(0f, 0.22f, 24f, 0f);

	public AnimationCurve PrecipitationGraph = AnimationCurve.Linear(1f, 0f, 13f, 100f);

	public List<WeatherType> NonPrecipiationWeatherTypes = new List<WeatherType>();

	public List<WeatherType> PrecipiationWeatherTypes = new List<WeatherType>();

	public List<WeatherType> AllWeatherTypes = new List<WeatherType>();

	public WeatherType CurrentWeatherType;

	public WeatherType NextWeatherType;

	public bool ByPassCoverageTransition;

	public int m_PrecipitationOdds = 50;

	private float m_CurrentPrecipitationAmountFloat = 1f;

	private int m_CurrentPrecipitationAmountInt = 1;

	public static bool m_IsFading;

	public int TransitionSpeed = 5;

	public int HourToChangeWeather;

	private float m_CloudFadeLevelStart = -0.05f;

	private float m_CloudFadeLevelEnd = 0.22f;

	private int m_GeneratedOdds;

	private bool m_WeatherGenerated;

	private Coroutine CloudCoroutine;

	private Coroutine FogCoroutine;

	private Coroutine WeatherEffectCoroutine;

	private Coroutine AdditionalWeatherEffectCoroutine;

	private Coroutine ParticleFadeCoroutine;

	private Coroutine StormyCloudsCoroutine;

	private Coroutine CloudTallnessCoroutine;

	private Coroutine AuroraCoroutine;

	private Coroutine FogLightFalloffCoroutine;

	private Coroutine AdditionalParticleFadeCoroutine;

	private Coroutine SunCoroutine;

	private Coroutine MoonCoroutine;

	private Coroutine WindCoroutine;

	private Coroutine SoundInCoroutine;

	private Coroutine SoundOutCoroutine;

	private Coroutine MostlyCloudyCoroutine;

	private Coroutine SunAttenuationIntensityCoroutine;

	private Coroutine AtmosphericFogCoroutine;

	private Coroutine ColorCoroutine;

	private Coroutine CloudHeightCoroutine;

	private Coroutine RainShaderCoroutine;

	private Coroutine SnowShaderCoroutine;

	private Coroutine SunColorCoroutine;

	private Coroutine CloudProfileCoroutine;

	private Coroutine CloudShadowIntensityCoroutine;

	private Coroutine MusicVolumeCoroutine;

	private Coroutine SunHeightCoroutine;

	public WindZone UniStormWindZone;

	public GameObject m_SoundTransform;

	public GameObject m_EffectsTransform;

	public Light m_LightningLight;

	private LightningSystem m_UniStormLightningSystem;

	public LightningStrike m_LightningStrikeSystem;

	public int LightningSecondsMin = 5;

	public int LightningSecondsMax = 10;

	public Color LightningColor = new Color(0.725f, 0.698f, 0.713f, 1f);

	private int m_LightningSeconds;

	private float m_LightningTimer;

	public List<AnimationCurve> LightningFlashPatterns = new List<AnimationCurve>();

	public List<AudioClip> ThunderSounds = new List<AudioClip>();

	public int LightningGroundStrikeOdds = 50;

	public GameObject LightningStrikeEffect;

	public GameObject LightningStrikeFire;

	public EnableFeature WeatherGeneration;

	public EnableFeature LightningStrikes;

	public EnableFeature CloudShadows;

	public EnableFeature LightningStrikesEmeraldAI = EnableFeature.Disabled;

	public string EmeraldAITag = "Respawn";

	public int EmeraldAIRagdollForce = 500;

	public int EmeraldAILightningDamage = 500;

	public ScreenSpaceCloudShadows m_CloudShadows;

	public float m_CurrentCloudHeight;

	public CloudShadowResolutionEnum CloudShadowResolution;

	public int CloudSpeed = 8;

	public int CloudTurbulence = 8;

	public LayerMask DetectionLayerMask;

	public List<string> LightningFireTags = new List<string>();

	public float LightningLightIntensityMin = 1f;

	public float LightningLightIntensityMax = 3f;

	public float CurrentFogAmount;

	public int LightningGenerationDistance = 100;

	public int LightningDetectionDistance = 20;

	public int m_CloudSeed;

	public Color CurrentFogColor;

	public FogTypeEnum FogType;

	public FogModeEnum FogMode;

	public UniStormAtmosphericFog m_UniStormAtmosphericFog;

	public EnableFeature UseDithering;

	public EnableFeature UseHighConvergenceSpeed = EnableFeature.Disabled;

	public EnableFeature UseRadialDistantFog = EnableFeature.Disabled;

	public float SnowAmount;

	public float CurrentWindIntensity;

	public float MostlyCloudyFadeValue;

	public float StormyHorizonBrightness = 1.4f;

	private WeatherType TempWeatherType;

	public AnimationCurve SunAttenuationCurve = AnimationCurve.Linear(0f, 1f, 24f, 3f);

	public AnimationCurve AmbientIntensityCurve = AnimationCurve.Linear(0f, 0f, 24f, 1f);

	public CurrentSeasonEnum CurrentSeason;

	public CloudTypeEnum CloudType = CloudTypeEnum.Volumetric;

	public CloudQualityEnum CloudQuality = CloudQualityEnum.High;

	public TemperatureTypeEnum TemperatureType;

	public AnimationCurve TemperatureCurve = AnimationCurve.Linear(1f, -100f, 13f, 125f);

	public AnimationCurve TemperatureFluctuation = AnimationCurve.Linear(0f, -25f, 24f, 25f);

	public int Temperature;

	public GameObject LightningStruckObject;

	public float FogLightFalloff = 9.7f;

	public float CameraFogHeight = 0.85f;

	private int m_FreezingTemperature;

	private Renderer m_CloudDomeRenderer;

	private Material m_CloudDomeMaterial;

	private Material m_SkyBoxMaterial;

	private Renderer m_StarsRenderer;

	private Material m_StarsMaterial;

	public Light m_SunLight;

	private Transform m_CelestialAxisTransform;

	public int SunRevolution = -90;

	public float SunIntensity = 1f;

	public float SunAttenuationMultipler = 1f;

	public float PrecipitationSunIntensity = 0.25f;

	public AnimationCurve SunIntensityCurve = AnimationCurve.Linear(0f, 0f, 24f, 5f);

	public AnimationCurve SunSize = AnimationCurve.Linear(0f, 1f, 24f, 10f);

	public AnimationCurve SunAtmosphericFogIntensity = AnimationCurve.Linear(0f, 2f, 24f, 2f);

	public AnimationCurve SunControlCurve = AnimationCurve.Linear(0f, 1f, 24f, 1f);

	public AnimationCurve MoonAtmosphericFogIntensity = AnimationCurve.Linear(0f, 1f, 24f, 1f);

	public AnimationCurve MoonObjectFade = AnimationCurve.Linear(0f, 1f, 24f, 1f);

	public float AtmosphericFogMultiplier = 1f;

	public Light m_MoonLight;

	public int MoonPhaseIndex = 5;

	public float MoonBrightness = 0.7f;

	public Material m_MoonPhaseMaterial;

	private Renderer m_MoonRenderer;

	private Transform m_MoonTransform;

	private Renderer m_SunRenderer;

	private Transform m_SunTransform;

	public float MoonIntensity = 1f;

	public float MoonPhaseIntensity = 1f;

	public AnimationCurve MoonIntensityCurve = AnimationCurve.Linear(0f, 0f, 24f, 5f);

	public AnimationCurve MoonSize = AnimationCurve.Linear(0f, 1f, 24f, 10f);

	private Vector3 m_MoonStartingSize;

	private GameObject m_MoonParent;

	public AnimationCurve AtmosphereThickness = AnimationCurve.Linear(0f, 1f, 24f, 3f);

	public AnimationCurve EnvironmentReflections = AnimationCurve.Linear(0f, 0f, 24f, 1f);

	public float StarSpeed = 0.75f;

	public int SunAngle = 10;

	public int MoonAngle = -10;

	public EnableFeature SunShaftsEffect;

	public EnableFeature MoonShaftsEffect;

	private UniStormSunShafts m_SunShafts;

	private UniStormSunShafts m_MoonShafts;

	public GameObject SunObject;

	public Material SunObjectMaterial;

	public HemisphereEnum Hemisphere;

	public LightShadows SunShadowType = LightShadows.Soft;

	public LightShadows MoonShadowType = LightShadows.Soft;

	public LightShadows LightningShadowType = LightShadows.Soft;

	public LightShadowResolution SunShadowResolution = LightShadowResolution.Medium;

	public LightShadowResolution MoonShadowResolution = LightShadowResolution.Medium;

	public LightShadowResolution LightningShadowResolution = LightShadowResolution.Medium;

	public float SunShadowStrength = 0.75f;

	public float MoonShadowStrength = 0.75f;

	public float LightningShadowStrength = 0.75f;

	public List<MoonPhaseClass> MoonPhaseList = new List<MoonPhaseClass>();

	public GameObject m_AuroraParent;

	public StarmapTypeEnum StarmapType = StarmapTypeEnum.LightConstellations;

	public CloudRenderTypeEnum CloudRenderType;

	public AnimationCurve SunLightShaftIntensity = AnimationCurve.Linear(0f, 1f, 24f, 1f);

	public Gradient SunLightShaftsColor;

	public float SunLightShaftsBlurSize = 4.86f;

	public int SunLightShaftsBlurIterations = 2;

	public AnimationCurve MoonLightShaftIntensity = AnimationCurve.Linear(0f, 1f, 24f, 1f);

	public Gradient MoonLightShaftsColor;

	public float MoonLightShaftsBlurSize = 3f;

	public int MoonLightShaftsBlurIterations = 2;

	public Gradient SunColor;

	public Gradient StormySunColor;

	public Gradient MoonColor;

	public Gradient SkyColor;

	public Gradient AmbientSkyLightColor;

	public Gradient StormyAmbientSkyLightColor;

	public Gradient AmbientEquatorLightColor;

	public Gradient StormyAmbientEquatorLightColor;

	public Gradient AmbientGroundLightColor;

	public Gradient StormyAmbientGroundLightColor;

	public Gradient StarLightColor;

	public Gradient FogColor;

	public Gradient FogStormyColor;

	public Gradient CloudLightColor;

	public Gradient StormyCloudLightColor;

	public Gradient CloudBaseColor;

	public Gradient CloudStormyBaseColor;

	public Gradient SkyTintColor;

	[GradientUsage(true)]
	public Gradient SunSpotColor;

	public Gradient FogLightColor;

	public Gradient StormyFogLightColor;

	public Color MoonPhaseColor = Color.white;

	public Color MoonlightColor;

	private float m_FadeValue;

	private float m_ReceivedCloudValue;

	public Gradient DefaultCloudBaseColor;

	private GradientColorKey[] CloudColorKeySwitcher;

	public Gradient DefaultFogBaseColor;

	private GradientColorKey[] FogColorKeySwitcher;

	public Gradient DefaultCloudLightColor;

	private GradientColorKey[] CloudLightColorKeySwitcher;

	public Gradient DefaultFogLightColor;

	private GradientColorKey[] FogLightColorKeySwitcher;

	public Gradient DefaultAmbientSkyLightBaseColor;

	private GradientColorKey[] AmbientSkyLightColorKeySwitcher;

	public Gradient DefaultAmbientEquatorLightBaseColor;

	private GradientColorKey[] AmbientEquatorLightColorKeySwitcher;

	public Gradient DefaultAmbientGroundLightBaseColor;

	private GradientColorKey[] AmbientGroundLightColorKeySwitcher;

	public Gradient DefaultSunLightBaseColor;

	private GradientColorKey[] SunLightColorKeySwitcher;

	public List<ParticleSystem> ParticleSystemList = new List<ParticleSystem>();

	public List<ParticleSystem> WeatherEffectsList = new List<ParticleSystem>();

	public List<ParticleSystem> AdditionalParticleSystemList = new List<ParticleSystem>();

	public List<ParticleSystem> AdditionalWeatherEffectsList = new List<ParticleSystem>();

	public List<AudioSource> WeatherSoundsList = new List<AudioSource>();

	public ParticleSystem CurrentParticleSystem;

	public float m_ParticleAmount;

	public ParticleSystem AdditionalCurrentParticleSystem;

	public bool UniStormInitialized;

	public AudioMixer UniStormAudioMixer;

	public bool UpgradedToCurrentVersion;

	private void Awake()
	{
		GameObject obj = new GameObject();
		obj.transform.SetParent(base.transform);
		obj.AddComponent<UniStormManager>();
		obj.name = "UniStorm Manager";
		Instance = this;
		InitializeCloudSettings();
	}

	private void Start()
	{
		if (GetPlayerAtRuntime == EnableFeature.Enabled)
		{
			PlayerTransform = null;
			if (UseRuntimeDelay == EnableFeature.Enabled)
			{
				StartCoroutine(InitializeDelay());
			}
			else if (UseRuntimeDelay == EnableFeature.Disabled)
			{
				if (GetPlayerMethod == GetPlayerMethodEnum.ByTag)
				{
					PlayerTransform = GameObject.FindWithTag(PlayerTag).transform;
					PlayerCamera = GameObject.FindWithTag(CameraTag).GetComponent<Camera>();
				}
				else if (GetPlayerMethod == GetPlayerMethodEnum.ByName)
				{
					PlayerTransform = GameObject.Find(PlayerName).transform;
					PlayerCamera = GameObject.Find(CameraName).GetComponent<Camera>();
				}
				InitializeUniStorm();
			}
		}
		else if (GetPlayerAtRuntime == EnableFeature.Disabled)
		{
			InitializeUniStorm();
		}
	}

	private IEnumerator InitializeDelay()
	{
		PlayerTransform = null;
		PlayerCamera = null;
		yield return new WaitWhile(() => GameObject.FindWithTag(PlayerTag) == null);
		yield return new WaitWhile(() => GameObject.FindWithTag(CameraTag) == null);
		if (GetPlayerMethod == GetPlayerMethodEnum.ByTag)
		{
			PlayerTransform = GameObject.FindWithTag(PlayerTag).transform;
			PlayerCamera = GameObject.FindWithTag(CameraTag).GetComponent<Camera>();
		}
		else if (GetPlayerMethod == GetPlayerMethodEnum.ByName)
		{
			PlayerTransform = GameObject.Find(PlayerName).transform;
			PlayerCamera = GameObject.Find(CameraName).GetComponent<Camera>();
		}
		InitializeUniStorm();
	}

	private Gradient UpdateGradient(Gradient Reference, Gradient GradientToUpdate)
	{
		GradientToUpdate = new Gradient();
		GradientColorKey[] array = new GradientColorKey[Reference.colorKeys.Length];
		array = Reference.colorKeys;
		GradientAlphaKey[] array2 = new GradientAlphaKey[Reference.alphaKeys.Length];
		array2 = Reference.alphaKeys;
		GradientToUpdate.SetKeys(array, array2);
		return GradientToUpdate;
	}

	private void InitializeUniStorm()
	{
		StopCoroutine(InitializeDelay());
		if (PlayerTransform == null || PlayerCamera == null)
		{
			Debug.LogWarning("(UniStorm has been disabled) - No player/camera has been assigned on the Player Transform/Player Camera slot.Please go to the Player & Camera tab and assign one.");
			GetComponent<UniStormSystem>().enabled = false;
		}
		else if (!PlayerTransform.gameObject.activeSelf || !PlayerCamera.gameObject.activeSelf)
		{
			Debug.LogWarning("(UniStorm has been disabled) - The player/camera game object is disabled on the Player Transform/Player Camera slot is disabled. Please go to the Player & Camera tab and ensure your player/camera is enabled.");
			GetComponent<UniStormSystem>().enabled = false;
		}
		if (!AllWeatherTypes.Contains(CurrentWeatherType))
		{
			AllWeatherTypes.Add(CurrentWeatherType);
		}
		if (MusicVolume == 0f)
		{
			MusicVolume = 0.001f;
		}
		if (AmbienceVolume == 0f)
		{
			AmbienceVolume = 0.001f;
		}
		if (WeatherSoundsVolume == 0f)
		{
			WeatherSoundsVolume = 0.001f;
		}
		UniStormAudioMixer = Resources.Load("UniStorm Audio Mixer") as AudioMixer;
		UniStormAudioMixer.SetFloat("MusicVolume", Mathf.Log(MusicVolume) * 20f);
		UniStormAudioMixer.SetFloat("AmbienceVolume", Mathf.Log(AmbienceVolume) * 20f);
		UniStormAudioMixer.SetFloat("WeatherVolume", Mathf.Log(WeatherSoundsVolume) * 20f);
		m_SoundTransform = new GameObject();
		m_SoundTransform.name = "UniStorm Sounds";
		m_SoundTransform.transform.SetParent(PlayerTransform);
		m_SoundTransform.transform.localPosition = Vector3.zero;
		m_EffectsTransform = new GameObject();
		m_EffectsTransform.name = "UniStorm Effects";
		m_EffectsTransform.transform.SetParent(PlayerTransform);
		m_EffectsTransform.transform.localPosition = Vector3.zero;
		for (int i = 0; i < AllWeatherTypes.Count; i++)
		{
			if (AllWeatherTypes[i] != null)
			{
				if (AllWeatherTypes[i].PrecipitationWeatherType == WeatherType.Yes_No.Yes && !PrecipiationWeatherTypes.Contains(AllWeatherTypes[i]))
				{
					PrecipiationWeatherTypes.Add(AllWeatherTypes[i]);
				}
				else if (AllWeatherTypes[i].PrecipitationWeatherType == WeatherType.Yes_No.No && !NonPrecipiationWeatherTypes.Contains(AllWeatherTypes[i]))
				{
					NonPrecipiationWeatherTypes.Add(AllWeatherTypes[i]);
				}
			}
			else
			{
				Debug.Log("A Weather Type from the All Weather Types list is missing. It will be excluded form the usable weather types.");
			}
		}
		for (int j = 0; j < AllWeatherTypes.Count; j++)
		{
			if (AllWeatherTypes[j] != null)
			{
				if (AllWeatherTypes[j].UseWeatherSound == WeatherType.Yes_No.Yes && AllWeatherTypes[j].WeatherSound == null)
				{
					AllWeatherTypes[j].UseWeatherSound = WeatherType.Yes_No.No;
				}
				if (AllWeatherTypes[j].UseWeatherEffect == WeatherType.Yes_No.Yes && AllWeatherTypes[j].WeatherEffect == null)
				{
					AllWeatherTypes[j].UseWeatherEffect = WeatherType.Yes_No.No;
				}
				if (AllWeatherTypes[j].UseAdditionalWeatherEffect == WeatherType.Yes_No.Yes && AllWeatherTypes[j].AdditionalWeatherEffect == null)
				{
					AllWeatherTypes[j].UseAdditionalWeatherEffect = WeatherType.Yes_No.No;
				}
				if (!ParticleSystemList.Contains(AllWeatherTypes[j].WeatherEffect) && AllWeatherTypes[j].WeatherEffect != null)
				{
					AllWeatherTypes[j].CreateWeatherEffect();
					ParticleSystemList.Add(AllWeatherTypes[j].WeatherEffect);
				}
				if (!AdditionalParticleSystemList.Contains(AllWeatherTypes[j].AdditionalWeatherEffect) && AllWeatherTypes[j].UseAdditionalWeatherEffect == WeatherType.Yes_No.Yes)
				{
					AllWeatherTypes[j].CreateAdditionalWeatherEffect();
					AdditionalParticleSystemList.Add(AllWeatherTypes[j].AdditionalWeatherEffect);
				}
				if (AllWeatherTypes[j].UseWeatherSound == WeatherType.Yes_No.Yes && AllWeatherTypes[j].WeatherSound != null)
				{
					AllWeatherTypes[j].CreateWeatherSound();
				}
			}
		}
		CloudColorKeySwitcher = new GradientColorKey[7];
		CloudColorKeySwitcher = CloudBaseColor.colorKeys;
		DefaultCloudBaseColor.colorKeys = new GradientColorKey[7];
		DefaultCloudBaseColor.colorKeys = CloudBaseColor.colorKeys;
		FogColorKeySwitcher = new GradientColorKey[7];
		FogColorKeySwitcher = FogColor.colorKeys;
		DefaultFogBaseColor.colorKeys = new GradientColorKey[7];
		DefaultFogBaseColor.colorKeys = FogColor.colorKeys;
		CloudLightColorKeySwitcher = new GradientColorKey[7];
		CloudLightColorKeySwitcher = CloudLightColor.colorKeys;
		DefaultCloudLightColor.colorKeys = new GradientColorKey[7];
		DefaultCloudLightColor.colorKeys = CloudLightColor.colorKeys;
		FogLightColorKeySwitcher = new GradientColorKey[7];
		FogLightColorKeySwitcher = FogLightColor.colorKeys;
		DefaultFogLightColor.colorKeys = new GradientColorKey[7];
		DefaultFogLightColor.colorKeys = FogLightColor.colorKeys;
		AmbientSkyLightColorKeySwitcher = new GradientColorKey[7];
		AmbientSkyLightColorKeySwitcher = AmbientSkyLightColor.colorKeys;
		DefaultAmbientSkyLightBaseColor.colorKeys = new GradientColorKey[7];
		DefaultAmbientSkyLightBaseColor.colorKeys = AmbientSkyLightColor.colorKeys;
		AmbientEquatorLightColorKeySwitcher = new GradientColorKey[7];
		AmbientEquatorLightColorKeySwitcher = AmbientEquatorLightColor.colorKeys;
		DefaultAmbientEquatorLightBaseColor.colorKeys = new GradientColorKey[7];
		DefaultAmbientEquatorLightBaseColor.colorKeys = AmbientEquatorLightColor.colorKeys;
		AmbientGroundLightColorKeySwitcher = new GradientColorKey[7];
		AmbientGroundLightColorKeySwitcher = AmbientGroundLightColor.colorKeys;
		DefaultAmbientGroundLightBaseColor.colorKeys = new GradientColorKey[7];
		DefaultAmbientGroundLightBaseColor.colorKeys = AmbientGroundLightColor.colorKeys;
		SunLightColorKeySwitcher = new GradientColorKey[6];
		SunLightColorKeySwitcher = SunColor.colorKeys;
		DefaultSunLightBaseColor.colorKeys = new GradientColorKey[6];
		DefaultSunLightBaseColor.colorKeys = SunColor.colorKeys;
		CalculatePrecipiation();
		CreateSun();
		CreateMoon();
		GameObject gameObject = new GameObject("UniStorm Time of Day Sounds");
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.AddComponent<AudioSource>();
		TimeOfDayAudioSource = gameObject.GetComponent<AudioSource>();
		TimeOfDayAudioSource.outputAudioMixerGroup = UniStormAudioMixer.FindMatchingGroups("Master/Ambience")[0];
		m_TimeOfDaySoundsSeconds = UnityEngine.Random.Range(TimeOfDaySoundsSecondsMin, TimeOfDaySoundsSecondsMax + 1);
		GameObject gameObject2 = new GameObject("UniStorm Time of Day Music");
		gameObject2.transform.SetParent(base.transform);
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.AddComponent<AudioSource>();
		TimeOfDayMusicAudioSource = gameObject2.GetComponent<AudioSource>();
		TimeOfDayMusicAudioSource.outputAudioMixerGroup = UniStormAudioMixer.FindMatchingGroups("Master/Music")[0];
		UniStormWindZone = GameObject.Find("UniStorm Windzone").GetComponent<WindZone>();
		m_StarsRenderer = GameObject.Find("UniStorm Stars").GetComponent<Renderer>();
		m_StarsMaterial = m_StarsRenderer.material;
		m_StarsMaterial.SetFloat("_StarSpeed", StarSpeed);
		m_StarsRenderer.gameObject.AddComponent<MeshFilter>();
		m_StarsRenderer.GetComponent<MeshFilter>().sharedMesh = ProceduralHemispherePolarUVs.hemisphere;
		if (StarmapType == StarmapTypeEnum.LightConstellations)
		{
			m_StarsMaterial.SetTexture("_Starmap", Resources.Load("Starmap (Light Constellations)") as Texture);
		}
		else if (StarmapType == StarmapTypeEnum.StrongConstellations)
		{
			m_StarsMaterial.SetTexture("_Starmap", Resources.Load("Starmap (Strong Constellations)") as Texture);
		}
		else if (StarmapType == StarmapTypeEnum.VeryStrongConstellations)
		{
			m_StarsMaterial.SetTexture("_Starmap", Resources.Load("Starmap (Very Strong Constellations)") as Texture);
		}
		m_StarsMaterial.SetFloat("_LoY", -2200f);
		m_StarsMaterial.SetFloat("_HiY", -60f);
		m_CloudDomeMaterial = UnityEngine.Object.FindObjectOfType<UniStormClouds>().skyMaterial;
		GameObject original = Resources.Load("UniStorm Auroras") as GameObject;
		m_AuroraParent = UnityEngine.Object.Instantiate(original, base.transform.position, Quaternion.identity);
		m_AuroraParent.transform.SetParent(UnityEngine.Object.FindObjectOfType<UniStormClouds>().gameObject.transform);
		m_AuroraParent.transform.localPosition = Vector3.zero;
		m_AuroraParent.transform.localScale = Vector3.one * 0.001f;
		m_AuroraParent.name = "UniStorm Auroras";
		float num = Minute;
		if (RealWorldTime == EnableFeature.Disabled)
		{
			m_TimeFloat = (float)Hour / 24f + num / 1440f;
		}
		else if (RealWorldTime == EnableFeature.Enabled)
		{
			m_TimeFloat = (float)DateTime.Now.Hour / 24f + (float)DateTime.Now.Minute / 1440f;
		}
		m_LastHour = Hour;
		m_SunLight.intensity = SunIntensityCurve.Evaluate(Hour) * SunIntensity;
		m_MoonLight.intensity = MoonIntensityCurve.Evaluate(Hour) * MoonIntensity * MoonPhaseIntensity;
		if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.MostlyCloudy)
		{
			MostlyCloudyFadeValue = 1f;
		}
		else if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.Cloudy && CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.No)
		{
			MostlyCloudyFadeValue = 1f;
		}
		if (CloudRenderType == CloudRenderTypeEnum.Transparent)
		{
			if (Hour <= 5 || Hour >= 19)
			{
				m_CloudDomeMaterial.SetFloat("_MaskMoon", 1f);
			}
			if (Hour >= 6 && Hour < 19)
			{
				m_CloudDomeMaterial.SetFloat("_MaskMoon", 0f);
			}
		}
		else if (CloudRenderType == CloudRenderTypeEnum.Opaque)
		{
			m_CloudDomeMaterial.SetFloat("_MaskMoon", 1f);
		}
		if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.DontChange)
		{
			m_CloudDomeMaterial.SetFloat("_uCloudsCoverage", UnityEngine.Random.Range(0.4f, 0.55f));
		}
		m_SkyBoxMaterial = (Material)Resources.Load("UniStorm Skybox");
		RenderSettings.skybox = m_SkyBoxMaterial;
		m_SkyBoxMaterial.SetFloat("_AtmosphereThickness", AtmosphereThickness.Evaluate(Hour));
		m_SkyBoxMaterial.SetColor("_NightSkyTint", SkyTintColor.Evaluate(Hour));
		RenderSettings.reflectionIntensity = EnvironmentReflections.Evaluate(Hour);
		Temperature = (int)TemperatureCurve.Evaluate(m_PreciseCurveTime) + (int)TemperatureFluctuation.Evaluate(StartingHour);
		if (TemperatureType == TemperatureTypeEnum.Fahrenheit)
		{
			m_FreezingTemperature = 32;
		}
		else if (TemperatureType == TemperatureTypeEnum.Celsius)
		{
			m_FreezingTemperature = 0;
		}
		base.transform.position = new Vector3(PlayerTransform.position.x, base.transform.position.y, PlayerTransform.position.z);
		GenerateWeather();
		CreateLightning();
		CreateUniStormFog();
		UpdateColors();
		CalculateMoonPhase();
		InitializeWeather(UseWeatherConditions: true);
		CalculateTimeOfDay();
		CalculateSeason();
		UpdateCelestialLightShafts();
		StartCoroutine(InitializeCloudShadows());
		if (CurrentWeatherType.UseAuroras == WeatherType.Yes_No.Yes)
		{
			if (CurrentTimeOfDay == CurrentTimeOfDayEnum.Night)
			{
				m_AuroraParent.SetActive(value: true);
				Shader.SetGlobalFloat("_LightIntensity", CurrentWeatherType.AuroraIntensity);
				Shader.SetGlobalColor("_InnerColor", CurrentWeatherType.AuroraInnerColor);
				Shader.SetGlobalColor("_OuterColor", CurrentWeatherType.AuroraOuterColor);
			}
		}
		else if (CurrentWeatherType.UseAuroras == WeatherType.Yes_No.No)
		{
			m_AuroraParent.SetActive(value: false);
		}
		if (WeatherGenerationMethod == WeatherGenerationMethodEnum.Hourly)
		{
			WeatherForecast[Hour] = CurrentWeatherType;
		}
		if (UseUniStormMenu == EnableFeature.Enabled)
		{
			CreateUniStormMenu();
		}
		Material skyMaterial = UnityEngine.Object.FindObjectOfType<UniStormClouds>().skyMaterial;
		if (UseHighConvergenceSpeed == EnableFeature.Enabled && CloudType == CloudTypeEnum.Volumetric)
		{
			if (CloudQuality == CloudQualityEnum.High || CloudQuality == CloudQualityEnum.Ultra)
			{
				skyMaterial.SetFloat("_UseHighConvergenceSpeed", 1f);
				Shader.SetGlobalFloat("DISTANT_CLOUD_MARCH_STEPS", 2.25f);
			}
			else
			{
				skyMaterial.SetFloat("_UseHighConvergenceSpeed", 0f);
				Shader.SetGlobalFloat("DISTANT_CLOUD_MARCH_STEPS", 1f);
			}
		}
		else
		{
			skyMaterial.SetFloat("_UseHighConvergenceSpeed", 0f);
			Shader.SetGlobalFloat("DISTANT_CLOUD_MARCH_STEPS", 1f);
		}
		UniStormInitialized = true;
	}

	private IEnumerator InitializeCloudShadows()
	{
		if (CloudShadows == EnableFeature.Enabled)
		{
			UniStormClouds m_UniStormClouds = UnityEngine.Object.FindObjectOfType<UniStormClouds>();
			if (PlayerCamera.gameObject.GetComponent<ScreenSpaceCloudShadows>() == null)
			{
				m_CloudShadows = PlayerCamera.gameObject.AddComponent<ScreenSpaceCloudShadows>();
			}
			else
			{
				m_CloudShadows = PlayerCamera.gameObject.GetComponent<ScreenSpaceCloudShadows>();
				m_CloudShadows.enabled = true;
			}
			yield return new WaitUntil(() => m_UniStormClouds.PublicCloudShadowTexture != null);
			m_CloudShadows.CloudShadowTexture = m_UniStormClouds.PublicCloudShadowTexture;
			m_CloudShadows.BottomThreshold = 0.5f;
			m_CloudShadows.TopThreshold = 1f;
			m_CloudShadows.CloudTextureScale = 0.001f;
			m_CloudShadows.ShadowIntensity = CurrentWeatherType.CloudShadowIntensity;
			PlayerCamera.clearFlags = CameraClearFlags.Skybox;
		}
		else if (PlayerCamera.gameObject.GetComponent<ScreenSpaceCloudShadows>() != null)
		{
			PlayerCamera.gameObject.GetComponent<ScreenSpaceCloudShadows>().enabled = false;
		}
	}

	private void InitializeCloudSettings()
	{
		UniStormClouds uniStormClouds = UnityEngine.Object.FindObjectOfType<UniStormClouds>();
		Material skyMaterial = UnityEngine.Object.FindObjectOfType<UniStormClouds>().skyMaterial;
		uniStormClouds.performance = (UniStormClouds.CloudPerformance)CloudQuality;
		skyMaterial.SetFloat("_uCloudsMovementSpeed", CloudSpeed);
		skyMaterial.SetFloat("_uCloudsTurbulenceSpeed", CloudTurbulence);
		skyMaterial.SetColor("_uMoonColor", MoonlightColor);
		if (ForceLowClouds == EnableFeature.Enabled)
		{
			Shader.SetGlobalFloat("_uCloudNoiseScale", 1.8f);
		}
		else
		{
			Shader.SetGlobalFloat("_uCloudNoiseScale", 0.7f);
		}
		if (CloudShadows == EnableFeature.Enabled)
		{
			uniStormClouds.CloudShadowsTypeRef = UniStormClouds.CloudShadowsType.Simulated;
			if (CloudShadowResolution == CloudShadowResolutionEnum._256x256)
			{
				uniStormClouds.CloudShadowResolutionValue = 256;
			}
			else if (CloudShadowResolution == CloudShadowResolutionEnum._512x512)
			{
				uniStormClouds.CloudShadowResolutionValue = 512;
			}
			else if (CloudShadowResolution == CloudShadowResolutionEnum._1024x1024)
			{
				uniStormClouds.CloudShadowResolutionValue = 1024;
			}
		}
		if (CloudType == CloudTypeEnum.Volumetric)
		{
			uniStormClouds.cloudType = UniStormClouds.CloudType.Volumetric;
			CloudProfile cloudProfileComponent = CurrentWeatherType.CloudProfileComponent;
			skyMaterial.SetFloat("_uCloudsBaseEdgeSoftness", cloudProfileComponent.EdgeSoftness);
			skyMaterial.SetFloat("_uCloudsBottomSoftness", cloudProfileComponent.BaseSoftness);
			skyMaterial.SetFloat("_uCloudsDetailStrength", cloudProfileComponent.DetailStrength);
			skyMaterial.SetFloat("_uCloudsDensity", cloudProfileComponent.Density);
			skyMaterial.SetFloat("_uCloudsCoverageBias", 0.082f);
			skyMaterial.SetFloat("_uCloudsDetailStrength", 0.082f);
			skyMaterial.SetFloat("_uCloudsBaseScale", 1.72f);
		}
		else if (CloudType == CloudTypeEnum._2D)
		{
			uniStormClouds.cloudType = UniStormClouds.CloudType.TwoD;
			skyMaterial.SetFloat("_uCloudsBaseEdgeSoftness", 0.2f);
			skyMaterial.SetFloat("_uCloudsBottomSoftness", 0.3f);
			skyMaterial.SetFloat("_uCloudsDetailStrength", 0.1f);
			skyMaterial.SetFloat("_uCloudsDensity", 0.3f);
			skyMaterial.SetFloat("_uCloudsBaseScale", 1f);
		}
	}

	public void InitializeWeather(bool UseWeatherConditions)
	{
		if (CloudCoroutine != null)
		{
			StopCoroutine(CloudCoroutine);
		}
		if (FogCoroutine != null)
		{
			StopCoroutine(FogCoroutine);
		}
		if (WeatherEffectCoroutine != null)
		{
			StopCoroutine(WeatherEffectCoroutine);
		}
		if (AdditionalWeatherEffectCoroutine != null)
		{
			StopCoroutine(AdditionalWeatherEffectCoroutine);
		}
		if (ParticleFadeCoroutine != null)
		{
			StopCoroutine(ParticleFadeCoroutine);
		}
		if (AdditionalParticleFadeCoroutine != null)
		{
			StopCoroutine(AdditionalParticleFadeCoroutine);
		}
		if (SunCoroutine != null)
		{
			StopCoroutine(SunCoroutine);
		}
		if (MoonCoroutine != null)
		{
			StopCoroutine(MoonCoroutine);
		}
		if (SoundInCoroutine != null)
		{
			StopCoroutine(SoundInCoroutine);
		}
		if (SoundOutCoroutine != null)
		{
			StopCoroutine(SoundOutCoroutine);
		}
		if (ColorCoroutine != null)
		{
			StopCoroutine(ColorCoroutine);
		}
		if (SunColorCoroutine != null)
		{
			StopCoroutine(SunColorCoroutine);
		}
		if (CloudHeightCoroutine != null)
		{
			StopCoroutine(CloudHeightCoroutine);
		}
		if (WindCoroutine != null)
		{
			StopCoroutine(WindCoroutine);
		}
		if (RainShaderCoroutine != null)
		{
			StopCoroutine(RainShaderCoroutine);
		}
		if (SnowShaderCoroutine != null)
		{
			StopCoroutine(SnowShaderCoroutine);
		}
		if (StormyCloudsCoroutine != null)
		{
			StopCoroutine(StormyCloudsCoroutine);
		}
		if (CloudProfileCoroutine != null)
		{
			StopCoroutine(CloudProfileCoroutine);
		}
		if (CloudShadowIntensityCoroutine != null)
		{
			StopCoroutine(CloudShadowIntensityCoroutine);
		}
		if (SunAttenuationIntensityCoroutine != null)
		{
			StopCoroutine(SunAttenuationIntensityCoroutine);
		}
		if (AuroraCoroutine != null)
		{
			StopCoroutine(AuroraCoroutine);
		}
		TempWeatherType = CurrentWeatherType;
		if (UseWeatherConditions)
		{
			while ((TempWeatherType.TemperatureType == WeatherType.TemperatureTypeEnum.AboveFreezing && Temperature <= m_FreezingTemperature) || (TempWeatherType.Season != 0 && TempWeatherType.Season != (WeatherType.SeasonEnum)CurrentSeason) || (TempWeatherType.TemperatureType == WeatherType.TemperatureTypeEnum.BelowFreezing && Temperature > m_FreezingTemperature))
			{
				if (TempWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.No)
				{
					TempWeatherType = NonPrecipiationWeatherTypes[UnityEngine.Random.Range(0, NonPrecipiationWeatherTypes.Count)];
					continue;
				}
				if (TempWeatherType.PrecipitationWeatherType != 0)
				{
					break;
				}
				TempWeatherType = PrecipiationWeatherTypes[UnityEngine.Random.Range(0, PrecipiationWeatherTypes.Count)];
			}
		}
		CurrentWeatherType = TempWeatherType;
		m_ReceivedCloudValue = GetCloudLevel(InstantFade: true);
		m_CloudDomeMaterial.SetFloat("_uCloudsCoverage", m_ReceivedCloudValue);
		RenderSettings.fogDensity = CurrentWeatherType.FogDensity;
		CurrentFogAmount = RenderSettings.fogDensity;
		UniStormWindZone.windMain = CurrentWeatherType.WindIntensity;
		CurrentWindIntensity = CurrentWeatherType.WindIntensity;
		SunIntensity = CurrentWeatherType.SunIntensity;
		MoonIntensity = CurrentWeatherType.MoonIntensity;
		if (ForceLowClouds == EnableFeature.Disabled)
		{
			m_CloudDomeMaterial.SetFloat("_uCloudsBottom", CurrentWeatherType.CloudHeight);
			m_CurrentCloudHeight = CurrentWeatherType.CloudHeight;
		}
		else
		{
			m_CloudDomeMaterial.SetFloat("_uCloudsBottom", LowCloudHeight);
			m_CurrentCloudHeight = LowCloudHeight;
		}
		if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.Cloudy || CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.MostlyCloudy)
		{
			m_CloudDomeMaterial.SetFloat("_uCloudsHeight", 1000f);
		}
		else if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.MostlyClear || CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.Clear || CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.PartyCloudy)
		{
			m_CloudDomeMaterial.SetFloat("_uCloudsHeight", 1000f);
		}
		if (FogMode == FogModeEnum.Exponential)
		{
			RenderSettings.fogMode = UnityEngine.FogMode.Exponential;
		}
		else if (FogMode == FogModeEnum.ExponentialSquared)
		{
			RenderSettings.fogMode = UnityEngine.FogMode.ExponentialSquared;
		}
		if (FogType == FogTypeEnum.UnistormFog)
		{
			RenderSettings.fog = false;
			if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
			{
				if (CurrentWeatherType.OverrideCameraFogHeight == WeatherType.Yes_No.No)
				{
					m_UniStormAtmosphericFog.BlendHeight = 0f;
				}
				else if (CurrentWeatherType.OverrideCameraFogHeight == WeatherType.Yes_No.Yes)
				{
					m_UniStormAtmosphericFog.BlendHeight = (1f - CurrentWeatherType.CameraFogHeight) / 10f;
				}
				m_CloudDomeMaterial.SetFloat("_FogBlendHeight", 1f - CurrentWeatherType.FogHeight);
				SunObjectMaterial.SetFloat("_OpaqueY", -600f);
				SunObjectMaterial.SetFloat("_TransparentY", -400f);
			}
			else if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.No)
			{
				if (CurrentWeatherType.OverrideCameraFogHeight == WeatherType.Yes_No.No)
				{
					m_UniStormAtmosphericFog.BlendHeight = (1f - CameraFogHeight) / 10f;
				}
				else if (CurrentWeatherType.OverrideCameraFogHeight == WeatherType.Yes_No.Yes)
				{
					m_UniStormAtmosphericFog.BlendHeight = (1f - CurrentWeatherType.CameraFogHeight) / 10f;
				}
				m_CloudDomeMaterial.SetFloat("_FogBlendHeight", 1f - CurrentWeatherType.FogHeight);
				SunObjectMaterial.SetFloat("_OpaqueY", -50f);
				SunObjectMaterial.SetFloat("_TransparentY", -10f);
			}
			if (UseRadialDistantFog == EnableFeature.Enabled)
			{
				m_UniStormAtmosphericFog.useRadialDistance = true;
			}
			FogLightFalloff = CurrentWeatherType.FogLightFalloff;
		}
		if (CurrentWeatherType.ShaderControl == WeatherType.ShaderControlEnum.Rain)
		{
			Shader.SetGlobalFloat("_WetnessStrength", 1f);
			Shader.SetGlobalFloat("_SnowStrength", 0f);
		}
		else if (CurrentWeatherType.ShaderControl == WeatherType.ShaderControlEnum.Snow)
		{
			Shader.SetGlobalFloat("_SnowStrength", 1f);
			Shader.SetGlobalFloat("_WetnessStrength", 0f);
		}
		else
		{
			Shader.SetGlobalFloat("_WetnessStrength", 0f);
			Shader.SetGlobalFloat("_SnowStrength", 0f);
		}
		for (int i = 0; i < WeatherEffectsList.Count; i++)
		{
			ParticleSystem.EmissionModule emission = WeatherEffectsList[i].emission;
			emission.rateOverTime = new ParticleSystem.MinMaxCurve(0f);
		}
		for (int j = 0; j < AdditionalWeatherEffectsList.Count; j++)
		{
			ParticleSystem.EmissionModule emission2 = AdditionalWeatherEffectsList[j].emission;
			emission2.rateOverTime = new ParticleSystem.MinMaxCurve(0f);
		}
		if (CurrentWeatherType.UseWeatherEffect == WeatherType.Yes_No.Yes)
		{
			for (int k = 0; k < WeatherEffectsList.Count; k++)
			{
				if (WeatherEffectsList[k].name == CurrentWeatherType.WeatherEffect.name + " (UniStorm)")
				{
					CurrentParticleSystem = WeatherEffectsList[k];
					ParticleSystem.EmissionModule emission3 = CurrentParticleSystem.emission;
					emission3.rateOverTime = new ParticleSystem.MinMaxCurve(CurrentWeatherType.ParticleEffectAmount);
				}
			}
			CurrentParticleSystem.transform.localPosition = CurrentWeatherType.ParticleEffectVector;
		}
		if (CurrentWeatherType.UseAdditionalWeatherEffect == WeatherType.Yes_No.Yes)
		{
			for (int l = 0; l < AdditionalWeatherEffectsList.Count; l++)
			{
				if (AdditionalWeatherEffectsList[l].name == CurrentWeatherType.AdditionalWeatherEffect.name + " (UniStorm)")
				{
					AdditionalCurrentParticleSystem = AdditionalWeatherEffectsList[l];
					ParticleSystem.EmissionModule emission4 = AdditionalCurrentParticleSystem.emission;
					emission4.rateOverTime = new ParticleSystem.MinMaxCurve(CurrentWeatherType.AdditionalParticleEffectAmount);
				}
			}
			AdditionalCurrentParticleSystem.transform.localPosition = CurrentWeatherType.AdditionalParticleEffectVector;
		}
		if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
		{
			m_CloudDomeMaterial.SetFloat("_uHorizonColorFadeStart", -0.2f);
			m_CloudDomeMaterial.SetFloat("_uHorizonColorFadeEnd", 0.32f);
			m_CloudDomeMaterial.SetFloat("_uHorizonFadeStart", 0f);
			if (FogType == FogTypeEnum.UnistormFog)
			{
				m_CloudDomeMaterial.SetFloat("_uHorizonFadeEnd", 0.22f);
				m_CloudDomeMaterial.SetFloat("_uSunFadeEnd", 0.18f);
			}
			else if (FogType == FogTypeEnum.UnityFog)
			{
				m_CloudDomeMaterial.SetFloat("_uHorizonFadeEnd", 0f);
			}
			m_CloudDomeMaterial.SetFloat("_uCloudAlpha", StormyHorizonBrightness);
			SunAttenuationMultipler = 0.2f;
			for (int m = 0; m < CloudBaseColor.colorKeys.Length; m++)
			{
				if (CurrentWeatherType.OverrideCloudColor == WeatherType.Yes_No.No)
				{
					CloudColorKeySwitcher[m].color = Color.Lerp(CloudColorKeySwitcher[m].color, CloudStormyBaseColor.colorKeys[m].color, 1f);
				}
				else if (CurrentWeatherType.OverrideCloudColor == WeatherType.Yes_No.Yes)
				{
					CloudColorKeySwitcher[m].color = Color.Lerp(CloudColorKeySwitcher[m].color, CurrentWeatherType.CloudColor.colorKeys[m].color, 1f);
				}
			}
			for (int n = 0; n < FogColor.colorKeys.Length; n++)
			{
				if (CurrentWeatherType.OverrideFogColor == WeatherType.Yes_No.No)
				{
					FogColorKeySwitcher[n].color = Color.Lerp(FogColorKeySwitcher[n].color, FogStormyColor.colorKeys[n].color, 1f);
				}
				else if (CurrentWeatherType.OverrideFogColor == WeatherType.Yes_No.Yes)
				{
					FogColorKeySwitcher[n].color = Color.Lerp(FogColorKeySwitcher[n].color, CurrentWeatherType.FogColor.colorKeys[n].color, 1f);
				}
			}
			for (int num = 0; num < CloudLightColor.colorKeys.Length; num++)
			{
				CloudLightColorKeySwitcher[num].color = Color.Lerp(CloudLightColorKeySwitcher[num].color, StormyCloudLightColor.colorKeys[num].color, 1f);
			}
			for (int num2 = 0; num2 < FogLightColor.colorKeys.Length; num2++)
			{
				FogLightColorKeySwitcher[num2].color = Color.Lerp(FogLightColorKeySwitcher[num2].color, StormyFogLightColor.colorKeys[num2].color, 1f);
			}
			for (int num3 = 0; num3 < AmbientSkyLightColor.colorKeys.Length; num3++)
			{
				AmbientSkyLightColorKeySwitcher[num3].color = Color.Lerp(AmbientSkyLightColorKeySwitcher[num3].color, StormyAmbientSkyLightColor.colorKeys[num3].color, 1f);
			}
			for (int num4 = 0; num4 < AmbientEquatorLightColor.colorKeys.Length; num4++)
			{
				AmbientEquatorLightColorKeySwitcher[num4].color = Color.Lerp(AmbientEquatorLightColorKeySwitcher[num4].color, StormyAmbientEquatorLightColor.colorKeys[num4].color, 1f);
			}
			for (int num5 = 0; num5 < AmbientGroundLightColor.colorKeys.Length; num5++)
			{
				AmbientGroundLightColorKeySwitcher[num5].color = Color.Lerp(AmbientGroundLightColorKeySwitcher[num5].color, StormyAmbientGroundLightColor.colorKeys[num5].color, 1f);
			}
			for (int num6 = 0; num6 < SunColor.colorKeys.Length; num6++)
			{
				SunLightColorKeySwitcher[num6].color = Color.Lerp(SunLightColorKeySwitcher[num6].color, StormySunColor.colorKeys[num6].color, 1f);
			}
			FogLightColor.SetKeys(FogLightColorKeySwitcher, FogLightColor.alphaKeys);
			CloudLightColor.SetKeys(CloudLightColorKeySwitcher, CloudLightColor.alphaKeys);
			FogColor.SetKeys(FogColorKeySwitcher, FogColor.alphaKeys);
			CloudBaseColor.SetKeys(CloudColorKeySwitcher, CloudBaseColor.alphaKeys);
			AmbientSkyLightColor.SetKeys(AmbientSkyLightColorKeySwitcher, AmbientSkyLightColor.alphaKeys);
			AmbientEquatorLightColor.SetKeys(AmbientEquatorLightColorKeySwitcher, AmbientEquatorLightColor.alphaKeys);
			AmbientGroundLightColor.SetKeys(AmbientGroundLightColorKeySwitcher, AmbientGroundLightColor.alphaKeys);
			SunColor.SetKeys(SunLightColorKeySwitcher, SunColor.alphaKeys);
		}
		else if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.No)
		{
			m_CloudDomeMaterial.SetFloat("_uHorizonColorFadeStart", 0f);
			m_CloudDomeMaterial.SetFloat("_uHorizonColorFadeEnd", 0f);
			m_CloudDomeMaterial.SetFloat("_uHorizonFadeStart", m_CloudFadeLevelStart);
			m_CloudDomeMaterial.SetFloat("_uHorizonFadeEnd", m_CloudFadeLevelEnd);
			m_CloudDomeMaterial.SetFloat("_uSunFadeEnd", 0.045f);
			m_CloudDomeMaterial.SetFloat("_uCloudAlpha", 1f);
			SunAttenuationMultipler = 1f;
			for (int num7 = 0; num7 < CloudBaseColor.colorKeys.Length; num7++)
			{
				if (CurrentWeatherType.OverrideCloudColor == WeatherType.Yes_No.No)
				{
					CloudColorKeySwitcher[num7].color = Color.Lerp(CloudColorKeySwitcher[num7].color, DefaultCloudBaseColor.colorKeys[num7].color, 1f);
				}
				else if (CurrentWeatherType.OverrideCloudColor == WeatherType.Yes_No.Yes)
				{
					CloudColorKeySwitcher[num7].color = Color.Lerp(CloudColorKeySwitcher[num7].color, CurrentWeatherType.CloudColor.colorKeys[num7].color, 1f);
				}
			}
			for (int num8 = 0; num8 < FogColor.colorKeys.Length; num8++)
			{
				if (CurrentWeatherType.OverrideFogColor == WeatherType.Yes_No.No)
				{
					FogColorKeySwitcher[num8].color = Color.Lerp(FogColorKeySwitcher[num8].color, DefaultFogBaseColor.colorKeys[num8].color, 1f);
				}
				else if (CurrentWeatherType.OverrideFogColor == WeatherType.Yes_No.Yes)
				{
					FogColorKeySwitcher[num8].color = Color.Lerp(FogColorKeySwitcher[num8].color, CurrentWeatherType.FogColor.colorKeys[num8].color, 1f);
				}
			}
			for (int num9 = 0; num9 < AmbientSkyLightColor.colorKeys.Length; num9++)
			{
				AmbientSkyLightColorKeySwitcher[num9].color = Color.Lerp(AmbientSkyLightColorKeySwitcher[num9].color, DefaultAmbientSkyLightBaseColor.colorKeys[num9].color, 1f);
			}
			for (int num10 = 0; num10 < CloudLightColor.colorKeys.Length; num10++)
			{
				CloudLightColorKeySwitcher[num10].color = Color.Lerp(CloudLightColorKeySwitcher[num10].color, DefaultCloudLightColor.colorKeys[num10].color, 1f);
			}
			for (int num11 = 0; num11 < FogLightColor.colorKeys.Length; num11++)
			{
				FogLightColorKeySwitcher[num11].color = Color.Lerp(FogLightColorKeySwitcher[num11].color, DefaultFogLightColor.colorKeys[num11].color, 1f);
			}
			for (int num12 = 0; num12 < AmbientEquatorLightColor.colorKeys.Length; num12++)
			{
				AmbientEquatorLightColorKeySwitcher[num12].color = Color.Lerp(AmbientEquatorLightColorKeySwitcher[num12].color, DefaultAmbientEquatorLightBaseColor.colorKeys[num12].color, 1f);
			}
			for (int num13 = 0; num13 < AmbientGroundLightColor.colorKeys.Length; num13++)
			{
				AmbientGroundLightColorKeySwitcher[num13].color = Color.Lerp(AmbientGroundLightColorKeySwitcher[num13].color, DefaultAmbientGroundLightBaseColor.colorKeys[num13].color, 1f);
			}
			for (int num14 = 0; num14 < SunColor.colorKeys.Length; num14++)
			{
				SunLightColorKeySwitcher[num14].color = Color.Lerp(SunLightColorKeySwitcher[num14].color, DefaultSunLightBaseColor.colorKeys[num14].color, 1f);
			}
			FogLightColor.SetKeys(FogLightColorKeySwitcher, FogLightColor.alphaKeys);
			CloudLightColor.SetKeys(CloudLightColorKeySwitcher, CloudLightColor.alphaKeys);
			FogColor.SetKeys(FogColorKeySwitcher, FogColor.alphaKeys);
			CloudBaseColor.SetKeys(CloudColorKeySwitcher, CloudBaseColor.alphaKeys);
			AmbientSkyLightColor.SetKeys(AmbientSkyLightColorKeySwitcher, AmbientSkyLightColor.alphaKeys);
			AmbientEquatorLightColor.SetKeys(AmbientEquatorLightColorKeySwitcher, AmbientEquatorLightColor.alphaKeys);
			AmbientGroundLightColor.SetKeys(AmbientGroundLightColorKeySwitcher, AmbientGroundLightColor.alphaKeys);
			SunColor.SetKeys(SunLightColorKeySwitcher, SunColor.alphaKeys);
		}
		foreach (AudioSource weatherSounds in WeatherSoundsList)
		{
			weatherSounds.volume = 0f;
		}
		if (CurrentWeatherType.UseWeatherSound != 0)
		{
			return;
		}
		foreach (AudioSource weatherSounds2 in WeatherSoundsList)
		{
			if (weatherSounds2.gameObject.name == CurrentWeatherType.WeatherTypeName + " (UniStorm)")
			{
				weatherSounds2.Play();
				weatherSounds2.volume = CurrentWeatherType.WeatherVolume;
			}
		}
	}

	private void FollowPlayer()
	{
		m_MoonLight.transform.position = PlayerTransform.position;
		m_SunLight.transform.position = PlayerTransform.position;
	}

	private void CalculatePrecipiation()
	{
		CalculateMonths();
		GetDate();
		m_roundingCorrection = (float)UniStormDate.DayOfYear * 0.002739726f;
		m_PreciseCurveTime = (float)UniStormDate.DayOfYear / 28.0769234f + 1f - m_roundingCorrection;
		m_PreciseCurveTime = Mathf.Round(m_PreciseCurveTime * 10f) / 10f;
		m_CurrentPrecipitationAmountFloat = PrecipitationGraph.Evaluate(m_PreciseCurveTime);
		m_CurrentPrecipitationAmountInt = (int)Mathf.Round(m_CurrentPrecipitationAmountFloat);
		m_PrecipitationOdds = m_CurrentPrecipitationAmountInt;
	}

	private void CreateUniStormFog()
	{
		if (FogType == FogTypeEnum.UnistormFog)
		{
			m_UniStormAtmosphericFog = PlayerCamera.gameObject.AddComponent<UniStormAtmosphericFog>();
			m_UniStormAtmosphericFog.fogShader = Shader.Find("Hidden/UniStorm Atmospheric Fog");
			m_UniStormAtmosphericFog.SunSource = m_SunLight;
			m_UniStormAtmosphericFog.MoonSource = m_MoonLight;
			m_UniStormAtmosphericFog.MoonColor = MoonlightColor;
			m_CloudDomeMaterial.SetColor("_MoonColor", MoonlightColor);
			m_CloudDomeMaterial.SetFloat("_UseUniStormFog", 1f);
			m_CloudFadeLevelStart = 0f;
			m_CloudFadeLevelEnd = 0.18f;
			if (UseDithering == EnableFeature.Enabled)
			{
				m_UniStormAtmosphericFog.Dither = UniStormAtmosphericFog.DitheringControl.Enabled;
				m_UniStormAtmosphericFog.NoiseTexture = (Texture2D)Resources.Load("Clouds/baseNoise");
				m_CloudDomeMaterial.SetFloat("_EnableDithering", 1f);
				m_CloudDomeMaterial.SetTexture("_NoiseTex", (Texture2D)Resources.Load("Clouds/baseNoise"));
			}
		}
	}

	private void CreateMoon()
	{
		m_MoonLight = GameObject.Find("UniStorm Moon").GetComponent<Light>();
		m_MoonLight.transform.localEulerAngles = new Vector3(-180f, MoonAngle, 0f);
		UnityEngine.Object.Instantiate((GameObject)Resources.Load("UniStorm Moon Object"), base.transform.position, Quaternion.identity).name = "UniStorm Moon Object";
		m_MoonRenderer = GameObject.Find("UniStorm Moon Object").GetComponent<Renderer>();
		m_MoonTransform = m_MoonRenderer.transform;
		m_MoonPhaseMaterial = m_MoonRenderer.material;
		m_MoonPhaseMaterial.SetColor("_MoonColor", MoonPhaseColor);
		m_MoonTransform.parent = m_MoonLight.transform;
		m_MoonLight.shadowResolution = MoonShadowResolution;
		m_MoonLight.shadows = MoonShadowType;
		m_MoonLight.shadowStrength = MoonShadowStrength;
		if (PlayerCamera.farClipPlane < 2000f)
		{
			m_MoonTransform.localPosition = new Vector3(0f, 0f, PlayerCamera.farClipPlane * -1f);
			m_MoonTransform.localEulerAngles = new Vector3(270f, 0f, 0f);
			float num = PlayerCamera.farClipPlane / 2000f;
			m_MoonStartingSize = m_MoonTransform.localScale * num;
			m_MoonTransform.localScale = new Vector3(m_MoonTransform.localScale.x, m_MoonTransform.localScale.y, m_MoonTransform.localScale.z);
		}
		else
		{
			m_MoonTransform.localPosition = new Vector3(0f, 0f, -2000f);
			m_MoonTransform.localEulerAngles = new Vector3(270f, 0f, 0f);
			m_MoonStartingSize = m_MoonTransform.localScale;
			m_MoonTransform.localScale = new Vector3(m_MoonTransform.localScale.x, m_MoonTransform.localScale.y, m_MoonTransform.localScale.z);
		}
		if (MoonShaftsEffect == EnableFeature.Enabled)
		{
			CreateMoonShafts();
		}
	}

	private void CreateSun()
	{
		m_SunLight = GameObject.Find("UniStorm Sun").GetComponent<Light>();
		m_SunLight.transform.localEulerAngles = new Vector3(0f, SunAngle, 0f);
		m_CelestialAxisTransform = GameObject.Find("Celestial Axis").transform;
		RenderSettings.sun = m_SunLight;
		m_SkyBoxMaterial = RenderSettings.skybox;
		m_SunLight.shadowResolution = SunShadowResolution;
		m_SunLight.shadows = SunShadowType;
		m_SunLight.shadowStrength = SunShadowStrength;
		SunObject = UnityEngine.Object.Instantiate((GameObject)Resources.Load("UniStorm Sun Object"), base.transform.position, Quaternion.identity);
		SunObject.name = "UniStorm Sun Object";
		SunObjectMaterial = SunObject.GetComponent<Renderer>().material;
		m_SunRenderer = GameObject.Find("UniStorm Sun Object").GetComponent<Renderer>();
		m_SunTransform = m_SunRenderer.transform;
		m_SunTransform.parent = m_SunLight.transform;
		if (PlayerCamera.farClipPlane < 2000f)
		{
			m_SunTransform.localPosition = new Vector3(0f, 0f, PlayerCamera.farClipPlane * -1f);
			m_SunTransform.localEulerAngles = new Vector3(270f, 0f, 0f);
			_ = PlayerCamera.farClipPlane / 2000f;
		}
		else
		{
			m_SunTransform.localPosition = new Vector3(0f, 0f, -2000f);
			m_SunTransform.localEulerAngles = new Vector3(270f, 0f, 0f);
		}
		if (SunShaftsEffect == EnableFeature.Enabled)
		{
			CreatSunShafts();
		}
	}

	private void CreatSunShafts()
	{
		m_SunShafts = PlayerCamera.gameObject.AddComponent<UniStormSunShafts>();
		m_SunShafts.sunShaftsShader = Shader.Find("Hidden/UniStormSunShafts");
		m_SunShafts.simpleClearShader = Shader.Find("Hidden/UniStormSimpleClear");
		m_SunShafts.useDepthTexture = true;
		m_SunShafts.maxRadius = 0.5f;
		m_SunShafts.sunShaftBlurRadius = 4.86f;
		m_SunShafts.radialBlurIterations = 2;
		m_SunShafts.sunShaftIntensity = 1f;
		m_SunShafts.sunTransform = GameObject.Find("Sun Transform").transform;
		ColorUtility.TryParseHtmlString("#C8A763", out var color);
		m_SunShafts.sunColor = color;
		ColorUtility.TryParseHtmlString("#8E897B", out var color2);
		m_SunShafts.sunThreshold = color2;
	}

	private void CreateMoonShafts()
	{
		m_MoonShafts = PlayerCamera.gameObject.AddComponent<UniStormSunShafts>();
		m_MoonShafts.sunShaftsShader = Shader.Find("Hidden/UniStormSunShafts");
		m_MoonShafts.simpleClearShader = Shader.Find("Hidden/UniStormSimpleClear");
		m_MoonShafts.useDepthTexture = true;
		m_MoonShafts.maxRadius = 0.3f;
		m_MoonShafts.sunShaftBlurRadius = 3.32f;
		m_MoonShafts.radialBlurIterations = 3;
		m_MoonShafts.sunShaftIntensity = 1f;
		GameObject gameObject = new GameObject("Moon Transform");
		gameObject.transform.SetParent(m_MoonLight.transform);
		gameObject.transform.localPosition = new Vector3(0f, 0f, -20000f);
		m_MoonShafts.sunTransform = gameObject.transform;
		ColorUtility.TryParseHtmlString("#515252FF", out var color);
		m_MoonShafts.sunColor = color;
		ColorUtility.TryParseHtmlString("#222222FF", out var color2);
		m_MoonShafts.sunThreshold = color2;
	}

	private void CreateLightning()
	{
		GameObject gameObject = new GameObject("UniStorm Lightning System");
		gameObject.AddComponent<LightningSystem>();
		m_UniStormLightningSystem = gameObject.GetComponent<LightningSystem>();
		m_UniStormLightningSystem.transform.SetParent(base.transform);
		for (int i = 0; i < ThunderSounds.Count; i++)
		{
			m_UniStormLightningSystem.ThunderSounds.Add(ThunderSounds[i]);
		}
		GameObject gameObject2 = new GameObject("UniStorm Lightning Light");
		gameObject2.AddComponent<Light>();
		m_LightningLight = gameObject2.GetComponent<Light>();
		m_LightningLight.type = LightType.Directional;
		m_LightningLight.transform.SetParent(base.transform);
		m_LightningLight.transform.localPosition = Vector3.zero;
		m_LightningLight.intensity = 0f;
		m_LightningLight.shadowResolution = LightningShadowResolution;
		m_LightningLight.shadows = LightningShadowType;
		m_LightningLight.shadowStrength = LightningShadowStrength;
		m_UniStormLightningSystem.LightningLightSource = m_LightningLight;
		m_UniStormLightningSystem.PlayerTransform = PlayerTransform;
		m_UniStormLightningSystem.LightningGenerationDistance = LightningGenerationDistance;
		m_LightningSeconds = UnityEngine.Random.Range(LightningSecondsMin, LightningSecondsMax);
		m_UniStormLightningSystem.LightningLightIntensityMin = LightningLightIntensityMin;
		m_UniStormLightningSystem.LightningLightIntensityMax = LightningLightIntensityMax;
	}

	public void ChangeWeatherUI()
	{
		CurrentWeatherType = AllWeatherTypes[WeatherDropdown.value];
		TransitionWeather();
	}

	private void CreateUniStormMenu()
	{
		UniStormCanvas = UnityEngine.Object.Instantiate((GameObject)Resources.Load("UniStorm Canvas"), base.transform.position, Quaternion.identity);
		UniStormCanvas.name = "UniStorm Canvas";
		TimeSlider = GameObject.Find("Time Slider").GetComponent<Slider>();
		TimeSliderGameObject = TimeSlider.gameObject;
		TimeSlider.onValueChanged.AddListener(delegate
		{
			CalculateTimeSlider();
		});
		OnHourChangeEvent.AddListener(delegate
		{
			UpdateTimeSlider();
		});
		TimeSlider.maxValue = 0.995f;
		WeatherButtonGameObject = GameObject.Find("Change Weather Button");
		WeatherDropdown = GameObject.Find("Weather Dropdown").GetComponent<Dropdown>();
		GameObject.Find("Change Weather Button").GetComponent<Button>().onClick.AddListener(delegate
		{
			ChangeWeatherUI();
		});
		List<string> list = new List<string>();
		for (int i = 0; i < AllWeatherTypes.Count; i++)
		{
			list.Add(AllWeatherTypes[i].WeatherTypeName);
		}
		WeatherDropdown.AddOptions(list);
		TimeSlider.value = m_TimeFloat;
		WeatherDropdown.value = AllWeatherTypes.IndexOf(CurrentWeatherType);
		if (UnityEngine.Object.FindObjectOfType<EventSystem>() == null)
		{
			GameObject obj = new GameObject();
			obj.name = "EventSystem";
			obj.AddComponent<EventSystem>();
			obj.AddComponent<StandaloneInputModule>();
		}
		m_MenuToggle = false;
		ToggleUniStormMenu();
	}

	public DateTime GetDate()
	{
		if (RealWorldTime == EnableFeature.Disabled)
		{
			UniStormDate = new DateTime(Year, Month, Day, Hour, Minute, 0);
		}
		else if (RealWorldTime == EnableFeature.Enabled)
		{
			UniStormDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Hour, Minute, 0);
			Year = UniStormDate.Year;
			Month = UniStormDate.Month;
			Day = UniStormDate.Day;
		}
		return UniStormDate;
	}

	public void MoveSun()
	{
		if (UseTimeOfDayUpdateControl == UseTimeOfDayUpdateSeconds.Yes)
		{
			TimeOfDayUpdateTimer += Time.deltaTime;
			if (TimeOfDayUpdateTimer >= (float)TimeOfDayUpdateSeconds)
			{
				m_CelestialAxisTransform.eulerAngles = new Vector3(m_TimeFloat * 360f - 100f, SunRevolution, 180f);
				if (CloudShadows == EnableFeature.Enabled)
				{
					m_CloudShadows.ShadowDirection = m_SunLight.transform.forward;
				}
				TimeOfDayUpdateTimer = 0f;
			}
		}
		else if (UseTimeOfDayUpdateControl == UseTimeOfDayUpdateSeconds.No)
		{
			m_CelestialAxisTransform.eulerAngles = new Vector3(m_TimeFloat * 360f - 100f, SunRevolution, 180f);
			if (CloudShadows == EnableFeature.Enabled)
			{
				m_CloudShadows.ShadowDirection = m_SunLight.transform.forward;
			}
		}
	}

	private void UpdateCelestialLightShafts()
	{
		if (SunShaftsEffect == EnableFeature.Enabled)
		{
			if (m_SunLight.intensity <= 0f)
			{
				m_SunShafts.enabled = false;
			}
			else
			{
				m_SunShafts.enabled = true;
			}
		}
		if (MoonShaftsEffect == EnableFeature.Enabled)
		{
			if (m_MoonLight.intensity <= 0f)
			{
				m_MoonShafts.enabled = false;
			}
			else
			{
				m_MoonShafts.enabled = true;
			}
		}
	}

	public void ToggleUniStormMenu()
	{
		WeatherButtonGameObject.SetActive(m_MenuToggle);
		TimeSliderGameObject.SetActive(m_MenuToggle);
		WeatherDropdown.gameObject.SetActive(m_MenuToggle);
		m_MenuToggle = !m_MenuToggle;
	}

	private void Update()
	{
		if (UniStormInitialized)
		{
			if (UseUniStormMenu == EnableFeature.Enabled && Input.GetKeyDown(UniStormMenuKey))
			{
				ToggleUniStormMenu();
			}
			if (TimeFlow == EnableFeature.Enabled)
			{
				if (RealWorldTime == EnableFeature.Disabled)
				{
					if (Hour > 6 && Hour <= 18)
					{
						m_TimeFloat += Time.deltaTime / (float)DayLength / 120f;
					}
					if (Hour > 18 || Hour <= 6)
					{
						m_TimeFloat += Time.deltaTime / (float)NightLength / 120f;
					}
				}
				else if (RealWorldTime == EnableFeature.Enabled)
				{
					m_TimeFloat = (float)DateTime.Now.Hour / 24f + (float)DateTime.Now.Minute / 1440f;
				}
				if (m_TimeFloat >= 1f)
				{
					m_TimeFloat = 0f;
					CalculateDays();
				}
			}
			float num = m_TimeFloat * 24f;
			Hour = (int)num;
			float num2 = num * 60f;
			Minute = (int)num2 % 60;
			if (m_LastHour != Hour)
			{
				m_LastHour = Hour;
				HourlyUpdate();
			}
			MoveSun();
			UpdateColors();
			PlayTimeOfDaySound();
			PlayTimeOfDayMusic();
			CalculateTimeOfDay();
			if (CurrentWeatherType.UseLightning == WeatherType.Yes_No.Yes && (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.MostlyCloudy || CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.Cloudy))
			{
				m_LightningTimer += Time.deltaTime;
				if (m_LightningTimer >= (float)m_LightningSeconds && m_CloudDomeMaterial.GetFloat("_uCloudsCoverage") >= 0.5f)
				{
					m_UniStormLightningSystem.LightningCurve = LightningFlashPatterns[UnityEngine.Random.Range(0, LightningFlashPatterns.Count)];
					m_UniStormLightningSystem.GenerateLightning();
					m_LightningSeconds = UnityEngine.Random.Range(LightningSecondsMin, LightningSecondsMax);
					m_LightningTimer = 0f;
				}
			}
			FollowPlayer();
		}
		else if (GetPlayerAtRuntime == EnableFeature.Enabled && !UniStormInitialized)
		{
			try
			{
				PlayerTransform = GameObject.FindWithTag(PlayerTag).transform;
				m_PlayerFound = true;
			}
			catch
			{
				m_PlayerFound = false;
			}
		}
	}

	private float GetCloudLevel(bool InstantFade)
	{
		UnityEngine.Random.InitState(DateTime.Now.Millisecond);
		float num = 0f;
		if (MostlyCloudyCoroutine != null)
		{
			StopCoroutine(MostlyCloudyCoroutine);
		}
		if (CloudTallnessCoroutine != null)
		{
			StopCoroutine(CloudTallnessCoroutine);
		}
		if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.Clear)
		{
			num = 0.25f;
			if (!InstantFade)
			{
				MostlyCloudyCoroutine = StartCoroutine(MostlyCloudyAdjustment(10 * TransitionSpeed, 1f, FadeOut: true));
				CloudTallnessCoroutine = StartCoroutine(CloudTallnessSequence(5 * TransitionSpeed, 800f));
			}
		}
		else if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.MostlyClear)
		{
			num = UnityEngine.Random.Range(0.36f, 0.39f);
			if (!InstantFade)
			{
				MostlyCloudyCoroutine = StartCoroutine(MostlyCloudyAdjustment(10 * TransitionSpeed, 1f, FadeOut: true));
				CloudTallnessCoroutine = StartCoroutine(CloudTallnessSequence(5 * TransitionSpeed, 800f));
			}
		}
		else if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.PartyCloudy)
		{
			num = UnityEngine.Random.Range(0.44f, 0.47f);
			if (!InstantFade)
			{
				MostlyCloudyCoroutine = StartCoroutine(MostlyCloudyAdjustment(10 * TransitionSpeed, 1f, FadeOut: true));
				CloudTallnessCoroutine = StartCoroutine(CloudTallnessSequence(5 * TransitionSpeed, 1000f));
			}
		}
		else if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.MostlyCloudy)
		{
			num = UnityEngine.Random.Range(0.48f, 0.5f);
			if (!InstantFade)
			{
				MostlyCloudyCoroutine = StartCoroutine(MostlyCloudyAdjustment(10 * TransitionSpeed, 1f, FadeOut: false));
				CloudTallnessCoroutine = StartCoroutine(CloudTallnessSequence(5 * TransitionSpeed, 1000f));
			}
		}
		else if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.Cloudy)
		{
			if (!InstantFade)
			{
				CloudTallnessCoroutine = StartCoroutine(CloudTallnessSequence(5 * TransitionSpeed, 1000f));
			}
			if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.No)
			{
				num = UnityEngine.Random.Range(0.51f, 0.54f);
				if (!InstantFade)
				{
					MostlyCloudyCoroutine = StartCoroutine(MostlyCloudyAdjustment(10 * TransitionSpeed, 1f, FadeOut: false));
				}
			}
			else if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
			{
				num = UnityEngine.Random.Range(0.51f, 0.54f);
				if (!InstantFade)
				{
					MostlyCloudyCoroutine = StartCoroutine(MostlyCloudyAdjustment(5 * TransitionSpeed, 1f, FadeOut: true));
				}
			}
		}
		else if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.DontChange)
		{
			num = m_CloudDomeMaterial.GetFloat("_uCloudsCoverage");
		}
		return Mathf.Round(num * 1000f) / 1000f;
	}

	public void CalculateTimeSlider()
	{
		m_TimeFloat = TimeSlider.value;
		TimeOfDayUpdateTimer = TimeOfDayUpdateSeconds;
	}

	public void UpdateTimeSlider()
	{
		TimeSlider.value = m_TimeFloat;
	}

	private void HourlyUpdate()
	{
		OnHourChangeEvent.Invoke();
		MoveSun();
		if (CloudRenderType == CloudRenderTypeEnum.Transparent)
		{
			if (Hour <= 5 || Hour >= 19)
			{
				m_CloudDomeMaterial.SetFloat("_MaskMoon", 1f);
			}
			if (Hour >= 6 && Hour < 19)
			{
				m_CloudDomeMaterial.SetFloat("_MaskMoon", 0f);
			}
		}
		UpdateCelestialLightShafts();
		Temperature = (int)TemperatureCurve.Evaluate(m_PreciseCurveTime) + (int)TemperatureFluctuation.Evaluate(Hour);
		if (WeatherGenerationMethod == WeatherGenerationMethodEnum.Hourly)
		{
			if (Hour < 23)
			{
				CurrentWeatherType = WeatherForecast[Hour];
				NextWeatherType = WeatherForecast[Hour + 1];
			}
			else
			{
				CurrentWeatherType = WeatherForecast[Hour];
				NextWeatherType = WeatherForecast[0];
			}
		}
		CheckWeather();
		if (Hour == 12)
		{
			MoonPhaseIndex++;
			CalculateMoonPhase();
		}
		if (CurrentWeatherType.UseAuroras != 0)
		{
			return;
		}
		if ((Hour >= 20 && Hour <= 23) || (Hour >= 0 && Hour <= 5))
		{
			if (AuroraCoroutine != null)
			{
				StopCoroutine(AuroraCoroutine);
			}
			AuroraCoroutine = StartCoroutine(AuroraShaderFadeSequence(3f, CurrentWeatherType.AuroraIntensity, CurrentWeatherType.AuroraInnerColor, CurrentWeatherType.AuroraOuterColor));
		}
		else if (Hour >= 6 && Hour <= 7)
		{
			if (AuroraCoroutine != null)
			{
				StopCoroutine(AuroraCoroutine);
			}
			AuroraCoroutine = StartCoroutine(AuroraShaderFadeSequence(3f, 0f, CurrentWeatherType.AuroraInnerColor, CurrentWeatherType.AuroraOuterColor));
		}
	}

	private void CalculateTimeOfDay()
	{
		if (Hour >= 6 && Hour <= 7)
		{
			if (CurrentTimeOfDay != 0)
			{
				m_UpdateTimeOfDayMusic = true;
			}
			CurrentTimeOfDay = CurrentTimeOfDayEnum.Morning;
		}
		else if (Hour >= 8 && Hour <= 16)
		{
			if (CurrentTimeOfDay != CurrentTimeOfDayEnum.Day)
			{
				m_UpdateTimeOfDayMusic = true;
			}
			CurrentTimeOfDay = CurrentTimeOfDayEnum.Day;
		}
		else if (Hour >= 17 && Hour <= 18)
		{
			if (CurrentTimeOfDay != CurrentTimeOfDayEnum.Evening)
			{
				m_UpdateTimeOfDayMusic = true;
			}
			CurrentTimeOfDay = CurrentTimeOfDayEnum.Evening;
		}
		else if ((Hour >= 19 && Hour <= 23) || (Hour >= 0 && Hour <= 5))
		{
			if (CurrentTimeOfDay != CurrentTimeOfDayEnum.Night)
			{
				m_UpdateTimeOfDayMusic = true;
			}
			CurrentTimeOfDay = CurrentTimeOfDayEnum.Night;
		}
	}

	public void CalculateSeason()
	{
		if ((Month == 3 && Day >= 20) || Month == 4 || Month == 5 || (Month == 6 && Day <= 20))
		{
			if (Hemisphere == HemisphereEnum.Northern)
			{
				CurrentSeason = CurrentSeasonEnum.Spring;
			}
			else if (Hemisphere == HemisphereEnum.Southern)
			{
				CurrentSeason = CurrentSeasonEnum.Fall;
			}
		}
		else if ((Month == 6 && Day >= 21) || Month == 7 || Month == 8 || (Month == 9 && Day <= 21))
		{
			if (Hemisphere == HemisphereEnum.Northern)
			{
				CurrentSeason = CurrentSeasonEnum.Summer;
			}
			else if (Hemisphere == HemisphereEnum.Southern)
			{
				CurrentSeason = CurrentSeasonEnum.Winter;
			}
		}
		else if ((Month == 9 && Day >= 22) || Month == 10 || Month == 11 || (Month == 12 && Day <= 20))
		{
			if (Hemisphere == HemisphereEnum.Northern)
			{
				CurrentSeason = CurrentSeasonEnum.Fall;
			}
			else if (Hemisphere == HemisphereEnum.Southern)
			{
				CurrentSeason = CurrentSeasonEnum.Spring;
			}
		}
		else if ((Month == 12 && Day >= 21) || Month == 1 || Month == 2 || (Month == 3 && Day <= 19))
		{
			if (Hemisphere == HemisphereEnum.Northern)
			{
				CurrentSeason = CurrentSeasonEnum.Winter;
			}
			else if (Hemisphere == HemisphereEnum.Southern)
			{
				CurrentSeason = CurrentSeasonEnum.Summer;
			}
		}
	}

	private void PlayTimeOfDaySound()
	{
		m_TimeOfDaySoundsTimer += Time.deltaTime;
		if (!(m_TimeOfDaySoundsTimer >= (float)m_TimeOfDaySoundsSeconds + m_CurrentClipLength) || ((!(CurrentWeatherType != null) || CurrentWeatherType.PrecipitationWeatherType != 0 || TimeOfDaySoundsDuringPrecipitationWeather != 0) && (!(CurrentWeatherType != null) || CurrentWeatherType.PrecipitationWeatherType != WeatherType.Yes_No.No || TimeOfDaySoundsDuringPrecipitationWeather != EnableFeature.Disabled)))
		{
			return;
		}
		if (CurrentTimeOfDay == CurrentTimeOfDayEnum.Morning)
		{
			if (MorningSounds.Count != 0)
			{
				TimeOfDayAudioSource.clip = MorningSounds[UnityEngine.Random.Range(0, MorningSounds.Count)];
				if (TimeOfDayAudioSource.clip != null)
				{
					TimeOfDayAudioSource.Play();
					m_CurrentClipLength = TimeOfDayAudioSource.clip.length;
				}
			}
		}
		else if (CurrentTimeOfDay == CurrentTimeOfDayEnum.Day)
		{
			if (DaySounds.Count != 0)
			{
				TimeOfDayAudioSource.clip = DaySounds[UnityEngine.Random.Range(0, DaySounds.Count)];
				if (TimeOfDayAudioSource.clip != null)
				{
					TimeOfDayAudioSource.Play();
					m_CurrentClipLength = TimeOfDayAudioSource.clip.length;
				}
			}
		}
		else if (CurrentTimeOfDay == CurrentTimeOfDayEnum.Evening)
		{
			if (EveningSounds.Count != 0)
			{
				TimeOfDayAudioSource.clip = EveningSounds[UnityEngine.Random.Range(0, EveningSounds.Count)];
				if (TimeOfDayAudioSource.clip != null)
				{
					TimeOfDayAudioSource.Play();
					m_CurrentClipLength = TimeOfDayAudioSource.clip.length;
				}
			}
		}
		else if (CurrentTimeOfDay == CurrentTimeOfDayEnum.Night && NightSounds.Count != 0)
		{
			TimeOfDayAudioSource.clip = NightSounds[UnityEngine.Random.Range(0, NightSounds.Count)];
			if (TimeOfDayAudioSource.clip != null)
			{
				TimeOfDayAudioSource.Play();
				m_CurrentClipLength = TimeOfDayAudioSource.clip.length;
			}
		}
		m_TimeOfDaySoundsTimer = 0f;
	}

	private void PlayTimeOfDayMusic()
	{
		m_TimeOfDayMusicTimer += Time.deltaTime;
		if (!(m_TimeOfDayMusicTimer >= m_CurrentMusicClipLength + (float)TimeOfDayMusicDelay) && (!m_UpdateTimeOfDayMusic || TransitionMusicOnTimeOfDayChange != 0) && !m_UpdateBiomeTimeOfDayMusic)
		{
			return;
		}
		if (CurrentTimeOfDay == CurrentTimeOfDayEnum.Morning)
		{
			if (MorningMusic.Count != 0)
			{
				if (MusicVolumeCoroutine != null)
				{
					StopCoroutine(MusicVolumeCoroutine);
				}
				AudioClip audioClip = MorningMusic[UnityEngine.Random.Range(0, MorningMusic.Count)];
				if (audioClip != null)
				{
					MusicVolumeCoroutine = StartCoroutine(MusicFadeSequence(MusicTransitionLength, audioClip));
					m_CurrentMusicClipLength = audioClip.length;
				}
			}
		}
		else if (CurrentTimeOfDay == CurrentTimeOfDayEnum.Day)
		{
			if (DayMusic.Count != 0)
			{
				if (MusicVolumeCoroutine != null)
				{
					StopCoroutine(MusicVolumeCoroutine);
				}
				AudioClip audioClip2 = DayMusic[UnityEngine.Random.Range(0, DayMusic.Count)];
				if (audioClip2 != null)
				{
					MusicVolumeCoroutine = StartCoroutine(MusicFadeSequence(MusicTransitionLength, audioClip2));
					m_CurrentMusicClipLength = audioClip2.length;
				}
			}
		}
		else if (CurrentTimeOfDay == CurrentTimeOfDayEnum.Evening)
		{
			if (EveningMusic.Count != 0)
			{
				if (MusicVolumeCoroutine != null)
				{
					StopCoroutine(MusicVolumeCoroutine);
				}
				AudioClip audioClip3 = EveningMusic[UnityEngine.Random.Range(0, EveningMusic.Count)];
				if (audioClip3 != null)
				{
					MusicVolumeCoroutine = StartCoroutine(MusicFadeSequence(MusicTransitionLength, audioClip3));
					m_CurrentMusicClipLength = audioClip3.length;
				}
			}
		}
		else if (CurrentTimeOfDay == CurrentTimeOfDayEnum.Night && NightMusic.Count != 0)
		{
			if (MusicVolumeCoroutine != null)
			{
				StopCoroutine(MusicVolumeCoroutine);
			}
			AudioClip audioClip4 = NightMusic[UnityEngine.Random.Range(0, NightMusic.Count)];
			if (audioClip4 != null)
			{
				MusicVolumeCoroutine = StartCoroutine(MusicFadeSequence(MusicTransitionLength, audioClip4));
				m_CurrentMusicClipLength = audioClip4.length;
			}
		}
		m_TimeOfDayMusicTimer = 0f;
		m_UpdateTimeOfDayMusic = false;
		m_UpdateBiomeTimeOfDayMusic = false;
	}

	private void CheckWeather()
	{
		if (!m_WeatherGenerated || WeatherGeneration != 0 || (Hour != HourToChangeWeather && WeatherGenerationMethod != 0))
		{
			return;
		}
		if (CurrentWeatherType != NextWeatherType)
		{
			if (WeatherGenerationMethod == WeatherGenerationMethodEnum.Daily)
			{
				CurrentWeatherType = NextWeatherType;
			}
			TransitionWeather();
		}
		if (WeatherGenerationMethod == WeatherGenerationMethodEnum.Daily)
		{
			GenerateWeather();
		}
	}

	public void ChangeWeather(WeatherType Weather)
	{
		CurrentWeatherType = Weather;
		TransitionWeather();
	}

	private void TransitionWeather()
	{
		OnWeatherChangeEvent.Invoke();
		if (CloudCoroutine != null)
		{
			StopCoroutine(CloudCoroutine);
		}
		if (FogCoroutine != null)
		{
			StopCoroutine(FogCoroutine);
		}
		if (WeatherEffectCoroutine != null)
		{
			StopCoroutine(WeatherEffectCoroutine);
		}
		if (AdditionalWeatherEffectCoroutine != null)
		{
			StopCoroutine(AdditionalWeatherEffectCoroutine);
		}
		if (ParticleFadeCoroutine != null)
		{
			StopCoroutine(ParticleFadeCoroutine);
		}
		if (AdditionalParticleFadeCoroutine != null)
		{
			StopCoroutine(AdditionalParticleFadeCoroutine);
		}
		if (SunCoroutine != null)
		{
			StopCoroutine(SunCoroutine);
		}
		if (MoonCoroutine != null)
		{
			StopCoroutine(MoonCoroutine);
		}
		if (SoundInCoroutine != null)
		{
			StopCoroutine(SoundInCoroutine);
		}
		if (SoundOutCoroutine != null)
		{
			StopCoroutine(SoundOutCoroutine);
		}
		if (ColorCoroutine != null)
		{
			StopCoroutine(ColorCoroutine);
		}
		if (SunColorCoroutine != null)
		{
			StopCoroutine(SunColorCoroutine);
		}
		if (CloudHeightCoroutine != null)
		{
			StopCoroutine(CloudHeightCoroutine);
		}
		if (WindCoroutine != null)
		{
			StopCoroutine(WindCoroutine);
		}
		if (RainShaderCoroutine != null)
		{
			StopCoroutine(RainShaderCoroutine);
		}
		if (SnowShaderCoroutine != null)
		{
			StopCoroutine(SnowShaderCoroutine);
		}
		if (StormyCloudsCoroutine != null)
		{
			StopCoroutine(StormyCloudsCoroutine);
		}
		if (CloudProfileCoroutine != null)
		{
			StopCoroutine(CloudProfileCoroutine);
		}
		if (CloudShadowIntensityCoroutine != null)
		{
			StopCoroutine(CloudShadowIntensityCoroutine);
		}
		if (SunAttenuationIntensityCoroutine != null)
		{
			StopCoroutine(SunAttenuationIntensityCoroutine);
		}
		if (AuroraCoroutine != null)
		{
			StopCoroutine(AuroraCoroutine);
		}
		if (AtmosphericFogCoroutine != null)
		{
			StopCoroutine(AtmosphericFogCoroutine);
		}
		if (FogLightFalloffCoroutine != null)
		{
			StopCoroutine(FogLightFalloffCoroutine);
		}
		if (SunHeightCoroutine != null)
		{
			StopCoroutine(SunHeightCoroutine);
		}
		m_TimeOfDaySoundsTimer = 0f;
		if (CurrentWeatherType.CloudLevel != WeatherType.CloudLevelEnum.DontChange)
		{
			m_ReceivedCloudValue = GetCloudLevel(InstantFade: false);
		}
		if (m_CloudDomeMaterial.GetFloat("_uCloudsCoverage") < m_ReceivedCloudValue)
		{
			CloudCoroutine = StartCoroutine(CloudFadeSequence(10 * TransitionSpeed, m_ReceivedCloudValue, FadeOut: false));
		}
		else
		{
			CloudCoroutine = StartCoroutine(CloudFadeSequence(10 * TransitionSpeed, m_ReceivedCloudValue, FadeOut: true));
		}
		if (ForceLowClouds == EnableFeature.Disabled)
		{
			CloudHeightCoroutine = StartCoroutine(CloudHeightSequence(10 * TransitionSpeed, CurrentWeatherType.CloudHeight));
		}
		if (CloudType == CloudTypeEnum.Volumetric && CurrentWeatherType.CloudLevel != WeatherType.CloudLevelEnum.DontChange)
		{
			CloudProfile cloudProfileComponent = CurrentWeatherType.CloudProfileComponent;
			CloudProfileCoroutine = StartCoroutine(CloudProfileSequence(10 * TransitionSpeed, cloudProfileComponent.EdgeSoftness, cloudProfileComponent.BaseSoftness, 0.082f, cloudProfileComponent.Density, 0.082f, cloudProfileComponent.DetailScale));
		}
		if (CurrentWeatherType.CloudLevel != WeatherType.CloudLevelEnum.DontChange && CloudShadows == EnableFeature.Enabled)
		{
			CloudShadowIntensityCoroutine = StartCoroutine(CloudShadowIntensitySequence(10 * TransitionSpeed, CurrentWeatherType.CloudShadowIntensity));
		}
		if (CurrentWindIntensity < CurrentWeatherType.WindIntensity)
		{
			WindCoroutine = StartCoroutine(WindFadeSequence(10 * TransitionSpeed, CurrentWeatherType.WindIntensity, FadeOut: false));
		}
		else
		{
			WindCoroutine = StartCoroutine(WindFadeSequence(10 * TransitionSpeed, CurrentWeatherType.WindIntensity, FadeOut: true));
		}
		if (FogType == FogTypeEnum.UnistormFog)
		{
			FogLightFalloffCoroutine = StartCoroutine(FogLightFalloffSequence(10 * TransitionSpeed, CurrentWeatherType.FogLightFalloff));
		}
		if (RenderSettings.fogDensity < CurrentWeatherType.FogDensity)
		{
			FogCoroutine = StartCoroutine(FogFadeSequence(5 * TransitionSpeed, CurrentWeatherType.FogDensity, FadeOut: false));
		}
		else
		{
			FogCoroutine = StartCoroutine(FogFadeSequence(5 * TransitionSpeed, CurrentWeatherType.FogDensity, FadeOut: true));
		}
		if (CurrentWeatherType.UseAuroras == WeatherType.Yes_No.Yes)
		{
			if ((Hour >= 20 && Hour <= 23) || (Hour >= 0 && Hour <= 5))
			{
				AuroraCoroutine = StartCoroutine(AuroraShaderFadeSequence(5 * TransitionSpeed, CurrentWeatherType.AuroraIntensity, CurrentWeatherType.AuroraInnerColor, CurrentWeatherType.AuroraOuterColor));
			}
		}
		else
		{
			AuroraCoroutine = StartCoroutine(AuroraShaderFadeSequence(5 * TransitionSpeed, 0f, CurrentWeatherType.AuroraInnerColor, CurrentWeatherType.AuroraOuterColor));
		}
		if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
		{
			SunCoroutine = StartCoroutine(SunFadeSequence(10 * TransitionSpeed, CurrentWeatherType.SunIntensity, FadeOut: false));
			MoonCoroutine = StartCoroutine(MoonFadeSequence(10 * TransitionSpeed, CurrentWeatherType.MoonIntensity, FadeOut: false));
			ColorCoroutine = StartCoroutine(ColorFadeSequence(10 * TransitionSpeed, 1f, CurrentWeatherType.FogColor, CurrentWeatherType.CloudColor));
			SunColorCoroutine = StartCoroutine(SunColorFadeSequence(10 * TransitionSpeed, 1f));
			StormyCloudsCoroutine = StartCoroutine(StormyCloudsSequence(10 * TransitionSpeed, FadeOut: false));
			SunAttenuationIntensityCoroutine = StartCoroutine(SunAttenuationIntensitySequence(10 * TransitionSpeed, 0.2f));
			SunHeightCoroutine = StartCoroutine(SunHeightSequence(10 * TransitionSpeed, -600f, -400f));
			if (FogType == FogTypeEnum.UnistormFog)
			{
				if (CurrentWeatherType.OverrideCameraFogHeight == WeatherType.Yes_No.No)
				{
					AtmosphericFogCoroutine = StartCoroutine(AtmosphericFogFadeSequence(10 * TransitionSpeed, 0f, 1f - CurrentWeatherType.FogHeight));
				}
				else if (CurrentWeatherType.OverrideCameraFogHeight == WeatherType.Yes_No.Yes)
				{
					AtmosphericFogCoroutine = StartCoroutine(AtmosphericFogFadeSequence(10 * TransitionSpeed, (1f - CurrentWeatherType.CameraFogHeight) / 10f, 1f - CurrentWeatherType.FogHeight));
				}
			}
			if (CurrentWeatherType.ShaderControl == WeatherType.ShaderControlEnum.Rain)
			{
				RainShaderCoroutine = StartCoroutine(RainShaderFadeSequence(20 * TransitionSpeed, 1f, FadeOut: false));
				SnowShaderCoroutine = StartCoroutine(SnowShaderFadeSequence(20 * TransitionSpeed, 0f, FadeOut: true));
			}
			else if (CurrentWeatherType.ShaderControl == WeatherType.ShaderControlEnum.Snow)
			{
				SnowShaderCoroutine = StartCoroutine(SnowShaderFadeSequence(20 * TransitionSpeed, 1f, FadeOut: false));
				RainShaderCoroutine = StartCoroutine(RainShaderFadeSequence(20 * TransitionSpeed, 0f, FadeOut: true));
			}
			else if (CurrentWeatherType.ShaderControl == WeatherType.ShaderControlEnum.None)
			{
				SnowShaderCoroutine = StartCoroutine(SnowShaderFadeSequence(20 * TransitionSpeed, 0f, FadeOut: true));
				RainShaderCoroutine = StartCoroutine(RainShaderFadeSequence(20 * TransitionSpeed, 0f, FadeOut: true));
			}
		}
		else if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.No)
		{
			SunCoroutine = StartCoroutine(SunFadeSequence(10 * TransitionSpeed, CurrentWeatherType.SunIntensity, FadeOut: false));
			MoonCoroutine = StartCoroutine(MoonFadeSequence(10 * TransitionSpeed, CurrentWeatherType.MoonIntensity, FadeOut: false));
			ColorCoroutine = StartCoroutine(ColorFadeSequence(30 * TransitionSpeed, 1f, CurrentWeatherType.FogColor, CurrentWeatherType.CloudColor));
			SunColorCoroutine = StartCoroutine(SunColorFadeSequence(10 * TransitionSpeed, 1f));
			StormyCloudsCoroutine = StartCoroutine(StormyCloudsSequence(10 * TransitionSpeed, FadeOut: true));
			SunAttenuationIntensityCoroutine = StartCoroutine(SunAttenuationIntensitySequence(10 * TransitionSpeed, 1f));
			SunHeightCoroutine = StartCoroutine(SunHeightSequence(10 * TransitionSpeed, -50f, -10f));
			if (FogType == FogTypeEnum.UnistormFog)
			{
				if (CurrentWeatherType.OverrideCameraFogHeight == WeatherType.Yes_No.No)
				{
					AtmosphericFogCoroutine = StartCoroutine(AtmosphericFogFadeSequence(10 * TransitionSpeed, (1f - CameraFogHeight) / 10f, 1f - CurrentWeatherType.FogHeight));
				}
				else if (CurrentWeatherType.OverrideCameraFogHeight == WeatherType.Yes_No.Yes)
				{
					AtmosphericFogCoroutine = StartCoroutine(AtmosphericFogFadeSequence(10 * TransitionSpeed, (1f - CurrentWeatherType.CameraFogHeight) / 10f, 1f - CurrentWeatherType.FogHeight));
				}
			}
			if (CurrentWeatherType.ShaderControl == WeatherType.ShaderControlEnum.None)
			{
				SnowShaderCoroutine = StartCoroutine(SnowShaderFadeSequence(20 * TransitionSpeed, 0f, FadeOut: true));
				RainShaderCoroutine = StartCoroutine(RainShaderFadeSequence(20 * TransitionSpeed, 0f, FadeOut: true));
			}
		}
		if (CurrentWeatherType.UseWeatherEffect == WeatherType.Yes_No.Yes)
		{
			ParticleSystem currentParticleSystem = CurrentParticleSystem;
			for (int i = 0; i < WeatherEffectsList.Count; i++)
			{
				if (WeatherEffectsList[i].name == CurrentWeatherType.WeatherEffect.name + " (UniStorm)")
				{
					CurrentParticleSystem = WeatherEffectsList[i];
					CurrentParticleSystem.transform.localPosition = CurrentWeatherType.ParticleEffectVector;
				}
			}
			if (CurrentParticleSystem.emission.rateOverTime.constant < (float)CurrentWeatherType.ParticleEffectAmount)
			{
				WeatherEffectCoroutine = StartCoroutine(ParticleFadeSequence(10 * TransitionSpeed, CurrentWeatherType.ParticleEffectAmount, null, FadeOut: false));
			}
			else if (currentParticleSystem != CurrentParticleSystem)
			{
				ParticleFadeCoroutine = StartCoroutine(ParticleFadeSequence(5 * TransitionSpeed, 0f, CurrentParticleSystem, FadeOut: true));
			}
			else
			{
				ParticleFadeCoroutine = StartCoroutine(ParticleFadeSequence(5 * TransitionSpeed, CurrentWeatherType.ParticleEffectAmount, CurrentParticleSystem, FadeOut: true));
			}
		}
		if (CurrentWeatherType.UseAdditionalWeatherEffect == WeatherType.Yes_No.Yes)
		{
			for (int j = 0; j < AdditionalWeatherEffectsList.Count; j++)
			{
				if (AdditionalWeatherEffectsList[j].name == CurrentWeatherType.AdditionalWeatherEffect.name + " (UniStorm)")
				{
					AdditionalCurrentParticleSystem = AdditionalWeatherEffectsList[j];
					AdditionalCurrentParticleSystem.transform.localPosition = CurrentWeatherType.AdditionalParticleEffectVector;
				}
			}
			if (AdditionalCurrentParticleSystem.emission.rateOverTime.constant < (float)CurrentWeatherType.AdditionalParticleEffectAmount)
			{
				AdditionalWeatherEffectCoroutine = StartCoroutine(AdditionalParticleFadeSequence(10 * TransitionSpeed, CurrentWeatherType.AdditionalParticleEffectAmount, null, FadeOut: false));
			}
			else
			{
				AdditionalParticleFadeCoroutine = StartCoroutine(AdditionalParticleFadeSequence(5 * TransitionSpeed, 0f, AdditionalCurrentParticleSystem, FadeOut: true));
			}
		}
		if (CurrentWeatherType.UseWeatherSound == WeatherType.Yes_No.Yes)
		{
			foreach (AudioSource weatherSounds in WeatherSoundsList)
			{
				if (weatherSounds.gameObject.name == CurrentWeatherType.WeatherTypeName + " (UniStorm)")
				{
					weatherSounds.Play();
					SoundInCoroutine = StartCoroutine(SoundFadeSequence(10 * TransitionSpeed, CurrentWeatherType.WeatherVolume, weatherSounds, FadeOut: false));
				}
			}
		}
		if (CurrentWeatherType.UseWeatherEffect == WeatherType.Yes_No.No)
		{
			CurrentParticleSystem = null;
			if (CurrentWeatherType.UseAdditionalWeatherEffect == WeatherType.Yes_No.No)
			{
				AdditionalCurrentParticleSystem = null;
			}
		}
		foreach (ParticleSystem weatherEffects in WeatherEffectsList)
		{
			if ((weatherEffects != CurrentParticleSystem && weatherEffects.emission.rateOverTime.constant > 0f) || (CurrentWeatherType.UseWeatherEffect == WeatherType.Yes_No.No && weatherEffects.emission.rateOverTime.constant > 0f))
			{
				ParticleFadeCoroutine = StartCoroutine(ParticleFadeSequence(5 * TransitionSpeed, 0f, weatherEffects, FadeOut: true));
			}
		}
		foreach (ParticleSystem additionalWeatherEffects in AdditionalWeatherEffectsList)
		{
			if ((additionalWeatherEffects != AdditionalCurrentParticleSystem && additionalWeatherEffects.emission.rateOverTime.constant > 0f) || (CurrentWeatherType.UseAdditionalWeatherEffect == WeatherType.Yes_No.No && additionalWeatherEffects.emission.rateOverTime.constant > 0f))
			{
				AdditionalParticleFadeCoroutine = StartCoroutine(AdditionalParticleFadeSequence(5 * TransitionSpeed, 0f, additionalWeatherEffects, FadeOut: true));
			}
		}
		foreach (AudioSource weatherSounds2 in WeatherSoundsList)
		{
			if ((weatherSounds2.gameObject.name != CurrentWeatherType.WeatherTypeName + " (UniStorm)" && weatherSounds2.volume > 0f) || (CurrentWeatherType.UseWeatherSound == WeatherType.Yes_No.No && weatherSounds2.volume > 0f))
			{
				SoundOutCoroutine = StartCoroutine(SoundFadeSequence(5 * TransitionSpeed, 0f, weatherSounds2, FadeOut: true));
			}
		}
	}

	private void CalculateMoonPhase()
	{
		if (MoonPhaseList.Count > 0)
		{
			if (MoonPhaseIndex == MoonPhaseList.Count)
			{
				MoonPhaseIndex = 0;
			}
			m_MoonPhaseMaterial.SetTexture("_MainTex", MoonPhaseList[MoonPhaseIndex].MoonPhaseTexture);
			m_MoonRenderer.material = m_MoonPhaseMaterial;
			m_MoonPhaseMaterial.SetFloat("_MoonBrightness", MoonBrightness);
			MoonPhaseIntensity = MoonPhaseList[MoonPhaseIndex].MoonPhaseIntensity;
			m_MoonPhaseMaterial.SetColor("_MoonColor", MoonPhaseColor);
		}
	}

	private void UpdateColors()
	{
		m_SunLight.color = SunColor.Evaluate(m_TimeFloat);
		m_MoonLight.color = MoonColor.Evaluate(m_TimeFloat);
		m_StarsMaterial.color = StarLightColor.Evaluate(m_TimeFloat);
		m_StarsRenderer.transform.position = PlayerTransform.position;
		m_StarsMaterial.SetVector("_uWorldSpaceCameraPos", PlayerCamera.transform.position);
		m_SkyBoxMaterial.SetColor("_SkyTint", SkyColor.Evaluate(m_TimeFloat));
		m_SkyBoxMaterial.SetFloat("_AtmosphereThickness", AtmosphereThickness.Evaluate(m_TimeFloat * 24f));
		m_SkyBoxMaterial.SetColor("_NightSkyTint", SkyTintColor.Evaluate(m_TimeFloat));
		m_CloudDomeMaterial.SetColor("_uCloudsAmbientColorTop", CloudLightColor.Evaluate(m_TimeFloat));
		m_CloudDomeMaterial.SetColor("_uCloudsAmbientColorBottom", CloudBaseColor.Evaluate(m_TimeFloat));
		m_CloudDomeMaterial.SetColor("_uSunColor", SunColor.Evaluate(m_TimeFloat));
		Color color = FogColor.Evaluate(m_TimeFloat);
		m_CloudDomeMaterial.SetVector("_uFogColor", new Vector4(color.r, color.g, color.b, CurrentFogAmount));
		m_CloudDomeMaterial.SetFloat("_uAttenuation", SunAttenuationCurve.Evaluate(m_TimeFloat * 24f) * SunAttenuationMultipler);
		RenderSettings.ambientIntensity = AmbientIntensityCurve.Evaluate(m_TimeFloat * 24f);
		RenderSettings.ambientSkyColor = AmbientSkyLightColor.Evaluate(m_TimeFloat);
		RenderSettings.ambientEquatorColor = AmbientEquatorLightColor.Evaluate(m_TimeFloat);
		RenderSettings.ambientGroundColor = AmbientGroundLightColor.Evaluate(m_TimeFloat);
		RenderSettings.reflectionIntensity = EnvironmentReflections.Evaluate(m_TimeFloat * 24f);
		SunObjectMaterial.SetVector("_uWorldSpaceCameraPos", PlayerCamera.transform.position);
		SunObjectMaterial.SetColor("_SunColor", SunSpotColor.Evaluate(m_TimeFloat));
		SunObject.transform.localScale = Vector3.one * SunSize.Evaluate(m_TimeFloat * 24f) * 3f;
		m_SunLight.intensity = SunIntensityCurve.Evaluate(m_TimeFloat * 24f) * SunIntensity;
		m_MoonLight.intensity = MoonIntensityCurve.Evaluate(m_TimeFloat * 24f) * MoonIntensity * MoonPhaseIntensity;
		m_MoonTransform.localScale = MoonSize.Evaluate(m_TimeFloat * 24f) * m_MoonStartingSize;
		m_MoonPhaseMaterial.SetFloat("_MoonBrightness", MoonObjectFade.Evaluate(m_TimeFloat * 24f) * MoonBrightness);
		if (SunShaftsEffect == EnableFeature.Enabled && m_SunLight.intensity > 0f)
		{
			m_SunShafts.sunShaftIntensity = SunLightShaftIntensity.Evaluate(m_TimeFloat * 24f);
			m_SunShafts.radialBlurIterations = SunLightShaftsBlurIterations;
			m_SunShafts.sunShaftBlurRadius = SunLightShaftsBlurSize;
			m_SunShafts.sunColor = SunLightShaftsColor.Evaluate(m_TimeFloat);
		}
		else if (MoonShaftsEffect == EnableFeature.Enabled && m_MoonLight.intensity > 0f)
		{
			m_MoonShafts.sunShaftIntensity = MoonLightShaftIntensity.Evaluate(m_TimeFloat * 24f);
			m_MoonShafts.radialBlurIterations = MoonLightShaftsBlurIterations;
			m_MoonShafts.sunShaftBlurRadius = MoonLightShaftsBlurSize;
			m_MoonShafts.sunColor = MoonLightShaftsColor.Evaluate(m_TimeFloat);
		}
		if (FogType == FogTypeEnum.UnityFog)
		{
			CurrentFogColor = FogColor.Evaluate(m_TimeFloat);
			RenderSettings.fogColor = CurrentFogColor;
			m_CloudDomeMaterial.SetFloat("_UseUniStormFog", 0f);
		}
		else if (FogType == FogTypeEnum.UnistormFog)
		{
			CurrentFogColor = FogColor.Evaluate(m_TimeFloat);
			m_UniStormAtmosphericFog.BottomColor = CurrentFogColor;
			m_UniStormAtmosphericFog.SunColor = FogLightColor.Evaluate(m_TimeFloat);
			m_UniStormAtmosphericFog.SunControl = SunControlCurve.Evaluate(m_TimeFloat * 24f);
			m_UniStormAtmosphericFog.MoonControl = m_MoonLight.intensity;
			m_UniStormAtmosphericFog.SunIntensity = SunAtmosphericFogIntensity.Evaluate(m_TimeFloat * 24f) * FogLightFalloff;
			m_UniStormAtmosphericFog.MoonIntensity = MoonAtmosphericFogIntensity.Evaluate(m_TimeFloat * 24f) * FogLightFalloff;
			m_CloudDomeMaterial.SetColor("_FogColor", CurrentFogColor);
			m_CloudDomeMaterial.SetColor("_SunColor", FogLightColor.Evaluate(m_TimeFloat));
			m_CloudDomeMaterial.SetFloat("_SunControl", SunControlCurve.Evaluate(m_TimeFloat * 24f));
			m_CloudDomeMaterial.SetFloat("_MoonControl", m_MoonLight.intensity);
			m_CloudDomeMaterial.SetVector("_SunVector", m_SunLight.transform.rotation * -Vector3.forward);
			m_CloudDomeMaterial.SetVector("_MoonVector", m_MoonLight.transform.rotation * -Vector3.forward);
			m_CloudDomeMaterial.SetFloat("_SunIntensity", SunAtmosphericFogIntensity.Evaluate(m_TimeFloat * 24f) * FogLightFalloff);
			m_CloudDomeMaterial.SetFloat("_MoonIntensity", MoonAtmosphericFogIntensity.Evaluate(m_TimeFloat * 24f) * FogLightFalloff);
		}
	}

	private void CalculateDays()
	{
		CalculatePrecipiation();
		TemperatureCurve.Evaluate(m_PreciseCurveTime);
		Day++;
		CalculateMonths();
		CalculateSeason();
		OnDayChangeEvent.Invoke();
		GetDate();
		if (WeatherGenerationMethod == WeatherGenerationMethodEnum.Hourly)
		{
			WeatherForecast.Clear();
			GenerateWeather();
		}
	}

	private void CalculateMonths()
	{
		if ((Day >= 32 && Month == 1) || (Day >= 32 && Month == 3) || (Day >= 32 && Month == 5) || (Day >= 32 && Month == 7) || (Day >= 32 && Month == 8) || (Day >= 32 && Month == 10) || (Day >= 32 && Month == 12))
		{
			Day %= 32;
			Day++;
			Month++;
			OnMonthChangeEvent.Invoke();
		}
		if ((Day >= 31 && Month == 4) || (Day >= 31 && Month == 6) || (Day >= 31 && Month == 9) || (Day >= 31 && Month == 11))
		{
			Day %= 31;
			Day++;
			Month++;
			OnMonthChangeEvent.Invoke();
		}
		if ((Day >= 30 && Month == 2 && Year % 4 == 0 && Year % 100 != 0) || Year % 400 == 0)
		{
			Day %= 30;
			Day++;
			Month++;
			OnMonthChangeEvent.Invoke();
		}
		else if (Day >= 29 && Month == 2 && Year % 4 != 0)
		{
			Day %= 29;
			Day++;
			Month++;
			OnMonthChangeEvent.Invoke();
		}
		if (Month > 12)
		{
			Month %= 13;
			Year++;
			Month++;
			OnYearChangeEvent.Invoke();
			m_roundingCorrection = 0f;
		}
	}

	public void GenerateWeather()
	{
		if (WeatherGeneration != 0)
		{
			return;
		}
		if (WeatherGenerationMethod == WeatherGenerationMethodEnum.Daily)
		{
			m_GeneratedOdds = UnityEngine.Random.Range(1, 101);
			HourToChangeWeather = UnityEngine.Random.Range(0, 23);
			if (HourToChangeWeather == Hour)
			{
				HourToChangeWeather = Hour - 1;
			}
			CheckGeneratedWeather();
		}
		else if (WeatherGenerationMethod == WeatherGenerationMethodEnum.Hourly)
		{
			for (int i = 0; i < 24; i++)
			{
				m_GeneratedOdds = UnityEngine.Random.Range(1, 101);
				CheckGeneratedWeather();
			}
		}
	}

	public void CheckGeneratedWeather()
	{
		CalculatePrecipiation();
		if (m_GeneratedOdds <= m_PrecipitationOdds && PrecipiationWeatherTypes.Count != 0)
		{
			TempWeatherType = PrecipiationWeatherTypes[UnityEngine.Random.Range(0, PrecipiationWeatherTypes.Count)];
		}
		else if (m_GeneratedOdds > m_PrecipitationOdds && NonPrecipiationWeatherTypes.Count != 0)
		{
			TempWeatherType = NonPrecipiationWeatherTypes[UnityEngine.Random.Range(0, NonPrecipiationWeatherTypes.Count)];
		}
		if (!IgnoreConditions)
		{
			while ((TempWeatherType.TemperatureType == WeatherType.TemperatureTypeEnum.AboveFreezing && Temperature <= m_FreezingTemperature) || (TempWeatherType.Season != 0 && TempWeatherType.Season != (WeatherType.SeasonEnum)CurrentSeason) || (TempWeatherType.TemperatureType == WeatherType.TemperatureTypeEnum.BelowFreezing && Temperature > m_FreezingTemperature) || TempWeatherType.SpecialWeatherType == WeatherType.Yes_No.Yes)
			{
				if (TempWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.No)
				{
					TempWeatherType = NonPrecipiationWeatherTypes[UnityEngine.Random.Range(0, NonPrecipiationWeatherTypes.Count)];
					continue;
				}
				if (TempWeatherType.PrecipitationWeatherType != 0)
				{
					break;
				}
				TempWeatherType = PrecipiationWeatherTypes[UnityEngine.Random.Range(0, PrecipiationWeatherTypes.Count)];
			}
		}
		if (WeatherGenerationMethod == WeatherGenerationMethodEnum.Daily)
		{
			NextWeatherType = TempWeatherType;
			OnWeatherGenerationEvent.Invoke();
		}
		else if (WeatherGenerationMethod == WeatherGenerationMethodEnum.Hourly)
		{
			WeatherForecast.Add(TempWeatherType);
			OnWeatherGenerationEvent.Invoke();
		}
		m_WeatherGenerated = true;
	}

	private IEnumerator SunColorFadeSequence(float TransitionTime, float MaxValue)
	{
		float LerpValue = 0f;
		float t = 0f;
		while (LerpValue < MaxValue)
		{
			if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
			{
				if (FogType == FogTypeEnum.UnistormFog)
				{
					yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_uCloudsCoverage") >= 0.45f);
				}
				else if (FogType == FogTypeEnum.UnityFog)
				{
					yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_uCloudsCoverage") >= m_ReceivedCloudValue);
				}
				t += Time.deltaTime / TransitionTime * 0.01f;
				LerpValue += Time.deltaTime / TransitionTime;
				for (int i = 0; i < SunColor.colorKeys.Length; i++)
				{
					SunLightColorKeySwitcher[i].color = Color.Lerp(SunLightColorKeySwitcher[i].color, StormySunColor.colorKeys[i].color, t);
				}
				SunColor.SetKeys(SunLightColorKeySwitcher, SunColor.alphaKeys);
			}
			else
			{
				t += Time.deltaTime / TransitionTime * 0.01f;
				LerpValue += Time.deltaTime / TransitionTime;
				for (int j = 0; j < SunColor.colorKeys.Length; j++)
				{
					SunLightColorKeySwitcher[j].color = Color.Lerp(SunLightColorKeySwitcher[j].color, DefaultSunLightBaseColor.colorKeys[j].color, t);
				}
				SunColor.SetKeys(SunLightColorKeySwitcher, SunColor.alphaKeys);
			}
			yield return null;
		}
	}

	private IEnumerator ColorFadeSequence(float TransitionTime, float MaxValue, Gradient FogGradientColor, Gradient CloudGradientColor)
	{
		float LerpValue = 0f;
		float t = 0f;
		if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes && CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.Cloudy)
		{
			yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_uCloudsCoverage") >= 0.48f);
		}
		while (LerpValue < MaxValue)
		{
			if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
			{
				t += Time.deltaTime / TransitionTime * 0.01f;
				LerpValue += Time.deltaTime / TransitionTime;
				for (int i = 0; i < CloudBaseColor.colorKeys.Length; i++)
				{
					if (CurrentWeatherType.OverrideCloudColor == WeatherType.Yes_No.No)
					{
						CloudColorKeySwitcher[i].color = Color.Lerp(CloudColorKeySwitcher[i].color, CloudStormyBaseColor.colorKeys[i].color, t);
					}
					else if (CurrentWeatherType.OverrideCloudColor == WeatherType.Yes_No.Yes)
					{
						CloudColorKeySwitcher[i].color = Color.Lerp(CloudColorKeySwitcher[i].color, CloudGradientColor.colorKeys[i].color, t);
					}
				}
				for (int j = 0; j < FogColor.colorKeys.Length; j++)
				{
					if (CurrentWeatherType.OverrideFogColor == WeatherType.Yes_No.No)
					{
						FogColorKeySwitcher[j].color = Color.Lerp(FogColorKeySwitcher[j].color, FogStormyColor.colorKeys[j].color, t);
					}
					else if (CurrentWeatherType.OverrideFogColor == WeatherType.Yes_No.Yes)
					{
						FogColorKeySwitcher[j].color = Color.Lerp(FogColorKeySwitcher[j].color, FogGradientColor.colorKeys[j].color, t);
					}
				}
				for (int k = 0; k < AmbientSkyLightColor.colorKeys.Length; k++)
				{
					AmbientSkyLightColorKeySwitcher[k].color = Color.Lerp(AmbientSkyLightColorKeySwitcher[k].color, StormyAmbientSkyLightColor.colorKeys[k].color, t);
				}
				for (int l = 0; l < CloudLightColor.colorKeys.Length; l++)
				{
					CloudLightColorKeySwitcher[l].color = Color.Lerp(CloudLightColorKeySwitcher[l].color, StormyCloudLightColor.colorKeys[l].color, t);
				}
				for (int m = 0; m < FogLightColor.colorKeys.Length; m++)
				{
					FogLightColorKeySwitcher[m].color = Color.Lerp(FogLightColorKeySwitcher[m].color, StormyFogLightColor.colorKeys[m].color, t);
				}
				for (int n = 0; n < AmbientEquatorLightColor.colorKeys.Length; n++)
				{
					AmbientEquatorLightColorKeySwitcher[n].color = Color.Lerp(AmbientEquatorLightColorKeySwitcher[n].color, StormyAmbientEquatorLightColor.colorKeys[n].color, t);
				}
				for (int num = 0; num < AmbientGroundLightColor.colorKeys.Length; num++)
				{
					AmbientGroundLightColorKeySwitcher[num].color = Color.Lerp(AmbientGroundLightColorKeySwitcher[num].color, StormyAmbientGroundLightColor.colorKeys[num].color, t);
				}
				FogLightColor.SetKeys(FogLightColorKeySwitcher, FogLightColor.alphaKeys);
				CloudLightColor.SetKeys(CloudLightColorKeySwitcher, CloudLightColor.alphaKeys);
				FogColor.SetKeys(FogColorKeySwitcher, FogColor.alphaKeys);
				CloudBaseColor.SetKeys(CloudColorKeySwitcher, CloudBaseColor.alphaKeys);
				AmbientSkyLightColor.SetKeys(AmbientSkyLightColorKeySwitcher, AmbientSkyLightColor.alphaKeys);
				AmbientEquatorLightColor.SetKeys(AmbientEquatorLightColorKeySwitcher, AmbientEquatorLightColor.alphaKeys);
				AmbientGroundLightColor.SetKeys(AmbientGroundLightColorKeySwitcher, AmbientGroundLightColor.alphaKeys);
			}
			else if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.No)
			{
				t += Time.deltaTime / TransitionTime * 0.01f;
				LerpValue += Time.deltaTime / TransitionTime;
				for (int num2 = 0; num2 < CloudBaseColor.colorKeys.Length; num2++)
				{
					if (CurrentWeatherType.OverrideCloudColor == WeatherType.Yes_No.No)
					{
						CloudColorKeySwitcher[num2].color = Color.Lerp(CloudColorKeySwitcher[num2].color, DefaultCloudBaseColor.colorKeys[num2].color, t);
					}
					else if (CurrentWeatherType.OverrideCloudColor == WeatherType.Yes_No.Yes)
					{
						CloudColorKeySwitcher[num2].color = Color.Lerp(CloudColorKeySwitcher[num2].color, CloudGradientColor.colorKeys[num2].color, t);
					}
				}
				for (int num3 = 0; num3 < FogColor.colorKeys.Length; num3++)
				{
					if (CurrentWeatherType.OverrideFogColor == WeatherType.Yes_No.No)
					{
						FogColorKeySwitcher[num3].color = Color.Lerp(FogColorKeySwitcher[num3].color, DefaultFogBaseColor.colorKeys[num3].color, t);
					}
					else if (CurrentWeatherType.OverrideFogColor == WeatherType.Yes_No.Yes)
					{
						FogColorKeySwitcher[num3].color = Color.Lerp(FogColorKeySwitcher[num3].color, FogGradientColor.colorKeys[num3].color, t);
					}
				}
				for (int num4 = 0; num4 < AmbientSkyLightColor.colorKeys.Length; num4++)
				{
					AmbientSkyLightColorKeySwitcher[num4].color = Color.Lerp(AmbientSkyLightColorKeySwitcher[num4].color, DefaultAmbientSkyLightBaseColor.colorKeys[num4].color, t);
				}
				for (int num5 = 0; num5 < CloudLightColor.colorKeys.Length; num5++)
				{
					CloudLightColorKeySwitcher[num5].color = Color.Lerp(CloudLightColorKeySwitcher[num5].color, DefaultCloudLightColor.colorKeys[num5].color, t);
				}
				for (int num6 = 0; num6 < FogLightColor.colorKeys.Length; num6++)
				{
					FogLightColorKeySwitcher[num6].color = Color.Lerp(FogLightColorKeySwitcher[num6].color, DefaultFogLightColor.colorKeys[num6].color, t);
				}
				for (int num7 = 0; num7 < AmbientEquatorLightColor.colorKeys.Length; num7++)
				{
					AmbientEquatorLightColorKeySwitcher[num7].color = Color.Lerp(AmbientEquatorLightColorKeySwitcher[num7].color, DefaultAmbientEquatorLightBaseColor.colorKeys[num7].color, t);
				}
				for (int num8 = 0; num8 < AmbientGroundLightColor.colorKeys.Length; num8++)
				{
					AmbientGroundLightColorKeySwitcher[num8].color = Color.Lerp(AmbientGroundLightColorKeySwitcher[num8].color, DefaultAmbientGroundLightBaseColor.colorKeys[num8].color, t);
				}
				FogLightColor.SetKeys(FogLightColorKeySwitcher, FogLightColor.alphaKeys);
				CloudLightColor.SetKeys(CloudLightColorKeySwitcher, CloudLightColor.alphaKeys);
				FogColor.SetKeys(FogColorKeySwitcher, FogColor.alphaKeys);
				CloudBaseColor.SetKeys(CloudColorKeySwitcher, CloudBaseColor.alphaKeys);
				AmbientSkyLightColor.SetKeys(AmbientSkyLightColorKeySwitcher, AmbientSkyLightColor.alphaKeys);
				AmbientEquatorLightColor.SetKeys(AmbientEquatorLightColorKeySwitcher, AmbientEquatorLightColor.alphaKeys);
				AmbientGroundLightColor.SetKeys(AmbientGroundLightColorKeySwitcher, AmbientGroundLightColor.alphaKeys);
			}
			yield return null;
		}
	}

	private IEnumerator CloudFadeSequence(float TransitionTime, float MaxValue, bool FadeOut)
	{
		float CurrentValue = m_CloudDomeMaterial.GetFloat("_uCloudsCoverage");
		float LerpValue = CurrentValue;
		float t = 0f;
		while ((LerpValue > MaxValue && FadeOut) || (LerpValue < MaxValue && !FadeOut))
		{
			t += Time.deltaTime;
			LerpValue = Mathf.Lerp(CurrentValue, MaxValue, t / TransitionTime);
			m_CloudDomeMaterial.SetFloat("_uCloudsCoverage", LerpValue);
			yield return null;
		}
	}

	private IEnumerator StormyCloudsSequence(float TransitionTime, bool FadeOut)
	{
		float HorizonColorFadeStart = m_CloudDomeMaterial.GetFloat("_uHorizonColorFadeStart");
		float HorizonColorFadeEnd = m_CloudDomeMaterial.GetFloat("_uHorizonColorFadeEnd");
		float LerpValueColorEnd2 = HorizonColorFadeEnd;
		float HorizonFadeStart = m_CloudDomeMaterial.GetFloat("_uHorizonFadeStart");
		float HorizonFadeEnd = m_CloudDomeMaterial.GetFloat("_uHorizonFadeEnd");
		float HorizonSunFadeEnd = m_CloudDomeMaterial.GetFloat("_uSunFadeEnd");
		float LerpValueStart2 = HorizonFadeStart;
		float LerpValueEnd = HorizonFadeEnd;
		float LerpSunValueEnd2 = HorizonSunFadeEnd;
		float HorizonBrightness = m_CloudDomeMaterial.GetFloat("_uCloudAlpha");
		float t = 0f;
		if (!FadeOut)
		{
			yield return new WaitUntil(() => MostlyCloudyFadeValue <= 0f);
			while ((double)LerpValueColorEnd2 < 0.32)
			{
				yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_uCloudsCoverage") >= m_ReceivedCloudValue - 0.05f);
				t += Time.deltaTime * 1.5f;
				float value = Mathf.Lerp(HorizonColorFadeStart, -0.2f, t * 15f / TransitionTime);
				LerpValueColorEnd2 = Mathf.Lerp(HorizonColorFadeEnd, 0.32f, t * 1.5f / TransitionTime);
				m_CloudDomeMaterial.SetFloat("_uHorizonColorFadeStart", value);
				m_CloudDomeMaterial.SetFloat("_uHorizonColorFadeEnd", LerpValueColorEnd2);
				if (FogType == FogTypeEnum.UnistormFog)
				{
					LerpValueStart2 = Mathf.Lerp(HorizonFadeStart, 0f, t * 4f / TransitionTime);
					LerpValueEnd = Mathf.Lerp(HorizonFadeEnd, 0.22f, t * 4f / TransitionTime);
					LerpSunValueEnd2 = Mathf.Lerp(HorizonSunFadeEnd, 0.18f, t * 4f / TransitionTime);
				}
				else if (FogType == FogTypeEnum.UnityFog)
				{
					LerpValueStart2 = Mathf.Lerp(HorizonFadeStart, 0f, t * 4f / TransitionTime);
					LerpValueEnd = Mathf.Lerp(HorizonFadeEnd, 0.015f, t * 4f / TransitionTime);
					LerpSunValueEnd2 = Mathf.Lerp(HorizonSunFadeEnd, 0.18f, t * 4f / TransitionTime);
				}
				m_CloudDomeMaterial.SetFloat("_uHorizonFadeStart", LerpValueStart2);
				m_CloudDomeMaterial.SetFloat("_uHorizonFadeEnd", LerpValueEnd);
				m_CloudDomeMaterial.SetFloat("_uSunFadeEnd", LerpSunValueEnd2);
				float value2 = Mathf.Lerp(HorizonBrightness, StormyHorizonBrightness, t * 5f / TransitionTime);
				m_CloudDomeMaterial.SetFloat("_uCloudAlpha", value2);
				if (!(LerpValueColorEnd2 >= 0.32f))
				{
					yield return null;
					continue;
				}
				break;
			}
		}
		else if (FadeOut)
		{
			yield return new WaitUntil(() => MostlyCloudyFadeValue <= 0f);
			while (LerpValueEnd > m_CloudFadeLevelEnd)
			{
				t += Time.deltaTime;
				float value = Mathf.Lerp(HorizonColorFadeStart, 0f, t * 2f / TransitionTime);
				LerpValueColorEnd2 = Mathf.Lerp(HorizonColorFadeEnd, 0f, t * 5f / TransitionTime);
				m_CloudDomeMaterial.SetFloat("_uHorizonColorFadeStart", value);
				m_CloudDomeMaterial.SetFloat("_uHorizonColorFadeEnd", LerpValueColorEnd2);
				LerpValueStart2 = Mathf.Lerp(HorizonFadeStart, m_CloudFadeLevelStart, t * 10f / TransitionTime);
				LerpValueEnd = Mathf.Lerp(HorizonFadeEnd, m_CloudFadeLevelEnd, t * 1f / TransitionTime);
				LerpSunValueEnd2 = Mathf.Lerp(HorizonSunFadeEnd, 0.045f, t * 1f / TransitionTime);
				m_CloudDomeMaterial.SetFloat("_uHorizonFadeStart", LerpValueStart2);
				m_CloudDomeMaterial.SetFloat("_uHorizonFadeEnd", LerpValueEnd);
				m_CloudDomeMaterial.SetFloat("_uSunFadeEnd", LerpSunValueEnd2);
				float value2 = Mathf.Lerp(HorizonBrightness, 1f, t * 1f / TransitionTime);
				m_CloudDomeMaterial.SetFloat("_uCloudAlpha", value2);
				yield return null;
			}
		}
	}

	private IEnumerator FogFadeSequence(float TransitionTime, float MaxValue, bool FadeOut)
	{
		if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
		{
			yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_uCloudsCoverage") >= 0.5f);
		}
		float CurrentValue = RenderSettings.fogDensity;
		float LerpValue = CurrentValue;
		float t = 0f;
		while ((LerpValue > MaxValue && FadeOut) || (LerpValue < MaxValue && !FadeOut))
		{
			t += Time.deltaTime;
			LerpValue = (RenderSettings.fogDensity = Mathf.Lerp(CurrentValue, MaxValue, t / TransitionTime));
			yield return null;
		}
	}

	private IEnumerator WindFadeSequence(float TransitionTime, float MaxValue, bool FadeOut)
	{
		float CurrentValue = UniStormWindZone.windMain;
		float LerpValue = CurrentValue;
		float t = 0f;
		while ((LerpValue > MaxValue && FadeOut) || (LerpValue < MaxValue && !FadeOut))
		{
			t += Time.deltaTime;
			LerpValue = Mathf.Lerp(CurrentValue, MaxValue, t / TransitionTime);
			UniStormWindZone.windMain = LerpValue;
			yield return null;
		}
	}

	private IEnumerator SunFadeSequence(float TransitionTime, float MaxValue, bool FadeOut)
	{
		if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.DontChange)
		{
			yield break;
		}
		if (SunIntensity > CurrentWeatherType.SunIntensity)
		{
			FadeOut = true;
		}
		if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
		{
			if (FogType == FogTypeEnum.UnistormFog)
			{
				yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_uCloudsCoverage") >= 0.45f);
			}
			else if (FogType == FogTypeEnum.UnityFog)
			{
				yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_uCloudsCoverage") >= m_ReceivedCloudValue);
			}
		}
		float CurrentValue = SunIntensity;
		float LerpValue = CurrentValue;
		float t = 0f;
		while ((LerpValue > MaxValue && FadeOut) || (LerpValue < MaxValue && !FadeOut))
		{
			t += Time.deltaTime;
			LerpValue = (SunIntensity = Mathf.Lerp(CurrentValue, MaxValue, t / TransitionTime));
			yield return null;
		}
	}

	private IEnumerator MoonFadeSequence(float TransitionTime, float MaxValue, bool FadeOut)
	{
		if (CurrentWeatherType.CloudLevel == WeatherType.CloudLevelEnum.DontChange)
		{
			yield break;
		}
		if (MoonIntensity > CurrentWeatherType.MoonIntensity)
		{
			FadeOut = true;
		}
		if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
		{
			if (FogType == FogTypeEnum.UnistormFog)
			{
				yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_uCloudsCoverage") >= 0.45f);
			}
			else if (FogType == FogTypeEnum.UnityFog)
			{
				yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_uCloudsCoverage") >= m_ReceivedCloudValue);
			}
		}
		float CurrentValue = MoonIntensity;
		float LerpValue = CurrentValue;
		float t = 0f;
		while ((LerpValue > MaxValue && FadeOut) || (LerpValue < MaxValue && !FadeOut))
		{
			t += Time.deltaTime;
			LerpValue = (MoonIntensity = Mathf.Lerp(CurrentValue, MaxValue, t / TransitionTime));
			yield return null;
		}
	}

	private IEnumerator MostlyCloudyAdjustment(float TransitionTime, float MaxValue, bool FadeOut)
	{
		yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_uHorizonColorFadeStart") >= 0f);
		float CurrentValue = MostlyCloudyFadeValue;
		float LerpValue = CurrentValue;
		float t = 0f;
		while ((LerpValue > 0f && FadeOut) || (LerpValue < 1f && !FadeOut))
		{
			t += Time.deltaTime;
			if (!FadeOut)
			{
				LerpValue = Mathf.Lerp(CurrentValue, 1f, t / TransitionTime);
			}
			else if (FadeOut)
			{
				LerpValue = Mathf.Lerp(CurrentValue, 0f, t / TransitionTime);
			}
			MostlyCloudyFadeValue = LerpValue;
			yield return null;
		}
	}

	private IEnumerator CloudHeightSequence(float TransitionTime, float MaxValue)
	{
		float CurrentValue = (m_CurrentCloudHeight = m_CloudDomeMaterial.GetFloat("_uCloudsBottom"));
		float t = 0f;
		while (t / TransitionTime < 1f)
		{
			t += Time.deltaTime;
			float value = (m_CurrentCloudHeight = Mathf.Lerp(CurrentValue, MaxValue, t / TransitionTime));
			m_CloudDomeMaterial.SetFloat("_uCloudsBottom", value);
			yield return null;
		}
	}

	private IEnumerator CloudTallnessSequence(float TransitionTime, float MaxValue)
	{
		if (UniStormInitialized && ForceLowClouds == EnableFeature.Disabled)
		{
			float CurrentValue = m_CloudDomeMaterial.GetFloat("_uCloudsHeight");
			float t = 0f;
			while (t / TransitionTime < 1f)
			{
				t += Time.deltaTime;
				float value = Mathf.Lerp(CurrentValue, MaxValue, t / TransitionTime);
				m_CloudDomeMaterial.SetFloat("_uCloudsHeight", value);
				yield return null;
			}
		}
	}

	private IEnumerator ParticleFadeSequence(float TransitionTime, float MaxValue, ParticleSystem EffectToFade, bool FadeOut)
	{
		if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
		{
			yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_uCloudsCoverage") >= m_ReceivedCloudValue);
		}
		else if (CurrentWeatherType.WaitForCloudLevel == WeatherType.Yes_No.Yes)
		{
			yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_uCloudsCoverage") >= m_ReceivedCloudValue - 0.01f);
		}
		float t2;
		float LerpValue2;
		float CurrentValue2;
		if (EffectToFade == null)
		{
			CurrentValue2 = CurrentParticleSystem.emission.rateOverTime.constant;
			LerpValue2 = CurrentValue2;
			t2 = 0f;
			while ((LerpValue2 > MaxValue && FadeOut) || (LerpValue2 < MaxValue && !FadeOut))
			{
				t2 += Time.deltaTime;
				LerpValue2 = Mathf.Lerp(CurrentValue2, MaxValue, t2 / TransitionTime);
				ParticleSystem.EmissionModule emission = CurrentParticleSystem.emission;
				emission.rateOverTime = new ParticleSystem.MinMaxCurve(LerpValue2);
				yield return null;
			}
			yield break;
		}
		t2 = EffectToFade.emission.rateOverTime.constant;
		LerpValue2 = t2;
		CurrentValue2 = 0f;
		while ((LerpValue2 > MaxValue && FadeOut) || (LerpValue2 < MaxValue && !FadeOut))
		{
			CurrentValue2 += Time.deltaTime;
			LerpValue2 = Mathf.Lerp(t2, MaxValue, CurrentValue2 / TransitionTime);
			ParticleSystem.EmissionModule emission2 = EffectToFade.emission;
			emission2.rateOverTime = new ParticleSystem.MinMaxCurve(LerpValue2);
			m_ParticleAmount = LerpValue2;
			yield return null;
		}
	}

	private IEnumerator AdditionalParticleFadeSequence(float TransitionTime, float MaxValue, ParticleSystem AdditionalEffectToFade, bool FadeOut)
	{
		if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
		{
			yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_uCloudsCoverage") >= m_ReceivedCloudValue);
		}
		else if (CurrentWeatherType.WaitForCloudLevel == WeatherType.Yes_No.Yes)
		{
			yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_uCloudsCoverage") >= m_ReceivedCloudValue - 0.01f);
		}
		if (AdditionalEffectToFade == null)
		{
			float CurrentValue2 = AdditionalCurrentParticleSystem.emission.rateOverTime.constant;
			float LerpValue2 = CurrentValue2;
			float t2 = 0f;
			while ((LerpValue2 > MaxValue && FadeOut) || (LerpValue2 < MaxValue && !FadeOut))
			{
				t2 += Time.deltaTime;
				LerpValue2 = Mathf.Lerp(CurrentValue2, MaxValue, t2 / TransitionTime);
				ParticleSystem.EmissionModule emission = AdditionalCurrentParticleSystem.emission;
				emission.rateOverTime = new ParticleSystem.MinMaxCurve(LerpValue2);
				yield return null;
			}
		}
		else
		{
			float t2 = AdditionalEffectToFade.emission.rateOverTime.constant;
			float LerpValue2 = t2;
			float CurrentValue2 = 0f;
			while ((LerpValue2 > MaxValue && FadeOut) || (LerpValue2 < MaxValue && !FadeOut))
			{
				CurrentValue2 += Time.deltaTime;
				LerpValue2 = Mathf.Lerp(t2, MaxValue, CurrentValue2 / TransitionTime);
				ParticleSystem.EmissionModule emission2 = AdditionalEffectToFade.emission;
				emission2.rateOverTime = new ParticleSystem.MinMaxCurve(LerpValue2);
				yield return null;
			}
		}
	}

	private IEnumerator SoundFadeSequence(float TransitionTime, float MaxValue, AudioSource SourceToFade, bool FadeOut)
	{
		if (CurrentWeatherType.PrecipitationWeatherType == WeatherType.Yes_No.Yes)
		{
			yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_uCloudsCoverage") >= m_ReceivedCloudValue);
		}
		else if (CurrentWeatherType.WaitForCloudLevel == WeatherType.Yes_No.Yes)
		{
			yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_uCloudsCoverage") >= m_ReceivedCloudValue - 0.01f);
		}
		float CurrentValue = SourceToFade.volume;
		float LerpValue = CurrentValue;
		float t = 0f;
		while ((LerpValue > MaxValue && FadeOut) || (LerpValue < MaxValue && !FadeOut))
		{
			t += Time.deltaTime;
			LerpValue = (SourceToFade.volume = Mathf.Lerp(CurrentValue, MaxValue, t / TransitionTime));
			yield return null;
		}
	}

	private IEnumerator RainShaderFadeSequence(float TransitionTime, float MaxValue, bool FadeOut)
	{
		if (!FadeOut)
		{
			yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_uCloudsCoverage") >= m_ReceivedCloudValue);
		}
		else
		{
			yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_uCloudsCoverage") <= m_ReceivedCloudValue + 0.01f);
		}
		float CurrentValue = Shader.GetGlobalFloat("_WetnessStrength");
		float LerpValue = CurrentValue;
		float t = 0f;
		while ((LerpValue > MaxValue && FadeOut) || (LerpValue < MaxValue && !FadeOut))
		{
			t += Time.deltaTime;
			LerpValue = Mathf.Lerp(CurrentValue, MaxValue, t / TransitionTime);
			Shader.SetGlobalFloat("_WetnessStrength", LerpValue);
			yield return null;
		}
	}

	private IEnumerator SnowShaderFadeSequence(float TransitionTime, float MaxValue, bool FadeOut)
	{
		if (!FadeOut)
		{
			yield return new WaitUntil(() => m_CloudDomeMaterial.GetFloat("_uCloudsCoverage") >= m_ReceivedCloudValue);
			TransitionTime *= 2f;
		}
		float CurrentValue = Shader.GetGlobalFloat("_SnowStrength");
		float LerpValue = CurrentValue;
		float t = 0f;
		while ((LerpValue > MaxValue && FadeOut) || (LerpValue < MaxValue && !FadeOut))
		{
			t += Time.deltaTime;
			LerpValue = Mathf.Lerp(CurrentValue, MaxValue, t / TransitionTime);
			Shader.SetGlobalFloat("_SnowStrength", LerpValue);
			yield return null;
		}
	}

	private IEnumerator AuroraShaderFadeSequence(float TransitionTime, float MaxValue, Color InnerColor, Color OuterColor)
	{
		float CurrentLightIntensity = Shader.GetGlobalFloat("_LightIntensity");
		float LerpLightIntensity = CurrentLightIntensity;
		Color CurrentInnerColor = Shader.GetGlobalColor("_InnerColor");
		Color CurrentOuterColor = Shader.GetGlobalColor("_OuterColor");
		float t = 0f;
		if (CurrentLightIntensity <= 0f && CurrentWeatherType.UseAuroras == WeatherType.Yes_No.Yes)
		{
			m_AuroraParent.SetActive(value: true);
		}
		while (t / TransitionTime < 1f)
		{
			t += Time.deltaTime;
			LerpLightIntensity = Mathf.Lerp(CurrentLightIntensity, MaxValue, t / TransitionTime);
			Shader.SetGlobalFloat("_LightIntensity", LerpLightIntensity);
			if (CurrentWeatherType.UseAuroras == WeatherType.Yes_No.Yes)
			{
				Color value = Color.Lerp(CurrentInnerColor, InnerColor, t / TransitionTime);
				Shader.SetGlobalColor("_InnerColor", value);
				Color value2 = Color.Lerp(CurrentOuterColor, OuterColor, t / TransitionTime);
				Shader.SetGlobalColor("_OuterColor", value2);
			}
			yield return null;
		}
		if (LerpLightIntensity <= 0f)
		{
			m_AuroraParent.SetActive(value: false);
		}
	}

	private IEnumerator CloudProfileSequence(float TransitionTime, float MaxEdgeSoftness, float MaxBaseSoftness, float MaxDetailStrength, float MaxDensity, float MaxCoverageBias, float MaxDetailScale)
	{
		float EdgeSoftnessValue = m_CloudDomeMaterial.GetFloat("_uCloudsBaseEdgeSoftness");
		float BaseSoftnessValue = m_CloudDomeMaterial.GetFloat("_uCloudsBottomSoftness");
		float DetailStrengthValue = m_CloudDomeMaterial.GetFloat("_uCloudsDetailStrength");
		float DensityValue = m_CloudDomeMaterial.GetFloat("_uCloudsDensity");
		float CoverageBiasValue = m_CloudDomeMaterial.GetFloat("_uCloudsCoverageBias");
		float t = 0f;
		while (t / TransitionTime < 1f)
		{
			t += Time.deltaTime;
			float value = Mathf.Lerp(EdgeSoftnessValue, MaxEdgeSoftness, t / TransitionTime);
			m_CloudDomeMaterial.SetFloat("_uCloudsBaseEdgeSoftness", value);
			float value2 = Mathf.Lerp(BaseSoftnessValue, MaxBaseSoftness, t / TransitionTime);
			m_CloudDomeMaterial.SetFloat("_uCloudsBottomSoftness", value2);
			float value3 = Mathf.Lerp(DetailStrengthValue, MaxDetailStrength, t / TransitionTime);
			m_CloudDomeMaterial.SetFloat("_uCloudsDetailStrength", value3);
			float value4 = Mathf.Lerp(DensityValue, MaxDensity, t / TransitionTime);
			m_CloudDomeMaterial.SetFloat("_uCloudsDensity", value4);
			float value5 = Mathf.Lerp(CoverageBiasValue, MaxCoverageBias, t / TransitionTime);
			m_CloudDomeMaterial.SetFloat("_uCloudsCoverageBias", value5);
			yield return null;
		}
	}

	private IEnumerator CloudShadowIntensitySequence(float TransitionTime, float MaxValue)
	{
		if (UniStormInitialized)
		{
			float CurrentValue = m_CloudShadows.ShadowIntensity;
			float t = 0f;
			while (t / TransitionTime < 1f)
			{
				t += Time.deltaTime;
				float shadowIntensity = Mathf.Lerp(CurrentValue, MaxValue, t / TransitionTime);
				m_CloudShadows.ShadowIntensity = shadowIntensity;
				yield return null;
			}
		}
	}

	private IEnumerator SunAttenuationIntensitySequence(float TransitionTime, float MaxValue)
	{
		if (UniStormInitialized)
		{
			float CurrentValue = SunAttenuationMultipler;
			float t = 0f;
			while (t / TransitionTime < 1f)
			{
				t += Time.deltaTime;
				float num = (SunAttenuationMultipler = Mathf.Lerp(CurrentValue, MaxValue, t / TransitionTime));
				yield return null;
			}
		}
	}

	private IEnumerator MusicFadeSequence(float TransitionTime, AudioClip NewMusicClip)
	{
		if (!UniStormInitialized)
		{
			yield break;
		}
		float CurrentValue2 = TimeOfDayMusicAudioSource.volume;
		float t2 = 0f;
		if (TimeOfDayMusicAudioSource.clip != null)
		{
			while (t2 / TransitionTime < 1f)
			{
				t2 += Time.deltaTime;
				float volume = Mathf.Lerp(CurrentValue2, 0f, t2 / TransitionTime);
				TimeOfDayMusicAudioSource.volume = volume;
				yield return null;
			}
		}
		else
		{
			TimeOfDayMusicAudioSource.volume = 0f;
		}
		TimeOfDayMusicAudioSource.clip = NewMusicClip;
		TimeOfDayMusicAudioSource.Play();
		CurrentValue2 = TimeOfDayMusicAudioSource.volume;
		t2 = 0f;
		while (t2 / TransitionTime < 1f)
		{
			t2 += Time.deltaTime;
			float volume = Mathf.Lerp(CurrentValue2, MusicVolume, t2 / TransitionTime);
			TimeOfDayMusicAudioSource.volume = volume;
			yield return null;
		}
		m_TimeOfDayMusicTimer = 0f;
	}

	private IEnumerator AtmosphericFogFadeSequence(float TransitionTime, float ShaderMaxValue, float CloudMaxValue)
	{
		float ShaderCurrentValue = m_UniStormAtmosphericFog.BlendHeight;
		float CloudsCurrentValue = m_CloudDomeMaterial.GetFloat("_FogBlendHeight");
		float t = 0f;
		while (t / TransitionTime < 1f)
		{
			t += Time.deltaTime;
			float blendHeight = Mathf.Lerp(ShaderCurrentValue, ShaderMaxValue, t / TransitionTime);
			m_UniStormAtmosphericFog.BlendHeight = blendHeight;
			float value = Mathf.Lerp(CloudsCurrentValue, CloudMaxValue, t / TransitionTime);
			m_CloudDomeMaterial.SetFloat("_FogBlendHeight", value);
			yield return null;
		}
	}

	private IEnumerator FogLightFalloffSequence(float TransitionTime, float MaxValue)
	{
		float CurrentValue = FogLightFalloff;
		float t = 0f;
		while (t / TransitionTime < 1f)
		{
			t += Time.deltaTime;
			float num = (FogLightFalloff = Mathf.Lerp(CurrentValue, MaxValue, t / TransitionTime));
			yield return null;
		}
	}

	private IEnumerator SunHeightSequence(float TransitionTime, float OpaqueValue, float TransparentValue)
	{
		float CurrentOpaqueValue = SunObjectMaterial.GetFloat("_OpaqueY");
		float CurrentTransparentValue = SunObjectMaterial.GetFloat("_TransparentY");
		float t = 0f;
		while (t / TransitionTime < 1f)
		{
			t += Time.deltaTime * 0.85f;
			float value = Mathf.Lerp(CurrentOpaqueValue, OpaqueValue, t / TransitionTime);
			SunObjectMaterial.SetFloat("_OpaqueY", value);
			float value2 = Mathf.Lerp(CurrentTransparentValue, TransparentValue, t / TransitionTime);
			SunObjectMaterial.SetFloat("_TransparentY", value2);
			yield return null;
		}
	}

	private void OnApplicationQuit()
	{
		Shader.SetGlobalFloat("_WetnessStrength", 0f);
		Shader.SetGlobalFloat("_SnowStrength", 0f);
		Shader.SetGlobalFloat("_LightIntensity", 0f);
		m_CloudDomeMaterial.SetFloat("_uHorizonColorFadeStart", 0f);
		m_CloudDomeMaterial.SetFloat("_uHorizonColorFadeEnd", 0f);
		m_CloudDomeMaterial.SetFloat("_uHorizonFadeStart", m_CloudFadeLevelStart);
		m_CloudDomeMaterial.SetFloat("_uHorizonFadeEnd", 0.18f);
		m_CloudDomeMaterial.SetFloat("_uSunFadeEnd", 0.045f);
		m_CloudDomeMaterial.SetFloat("_uCloudAlpha", 1f);
		m_CloudDomeMaterial.SetFloat("_FogBlendHeight", 0.3f);
		if (CloudShadows == EnableFeature.Enabled)
		{
			m_CloudShadows.ShadowIntensity = 0f;
		}
	}
}
