using UnityEngine;
using UnityEngine.Audio;

namespace UniStorm;

[CreateAssetMenu(fileName = "New Weather Type", menuName = "UniStorm/New Weather Type")]
public class WeatherType : ScriptableObject
{
	public enum SeasonEnum
	{
		All,
		Spring,
		Summer,
		Fall,
		Winter
	}

	public enum CloudLevelEnum
	{
		Clear,
		MostlyClear,
		PartyCloudy,
		MostlyCloudy,
		Cloudy,
		DontChange
	}

	public enum TemperatureTypeEnum
	{
		BelowFreezing,
		AboveFreezing,
		Both
	}

	public enum ShaderControlEnum
	{
		Rain,
		Snow,
		None
	}

	public enum Yes_No
	{
		Yes,
		No
	}

	public string WeatherTypeName = "New Weather Type";

	public CloudProfile CloudProfileComponent;

	public float SunIntensity = 1f;

	public float CloudShadowIntensity = 1f;

	public float MoonIntensity = 1f;

	public Yes_No UseAuroras = Yes_No.No;

	public float AuroraIntensity = 0.4f;

	public Color AuroraOuterColor = Color.cyan;

	public Color AuroraInnerColor = Color.green;

	public int MinimumFogLevel = 300;

	public int MaximumFogLevel = 600;

	public float FogDensity = 0.0015f;

	public float FogLightFalloff = 0.65f;

	public float FogHeight = 0.73f;

	public float CameraFogHeight = 0.85f;

	public Yes_No OverrideFogColor = Yes_No.No;

	public Yes_No OverrideCameraFogHeight = Yes_No.No;

	public Gradient FogColor;

	public float WindSpeedLevel = 0.85f;

	public float WindBendingLevel = 0.2f;

	public float WindIntensity = 0.25f;

	public ParticleSystem WeatherEffect;

	public int ParticleEffectAmount = 200;

	public Vector3 ParticleEffectVector = new Vector3(0f, 28f, 0f);

	public ParticleSystem AdditionalWeatherEffect;

	public int AdditionalParticleEffectAmount = 200;

	public Vector3 AdditionalParticleEffectVector = new Vector3(0f, 28f, 0f);

	public float WeatherVolume = 1f;

	public AudioClip WeatherSound;

	public Yes_No PrecipitationWeatherType = Yes_No.No;

	public Yes_No UseWeatherSound = Yes_No.No;

	public Yes_No UseWeatherEffect = Yes_No.No;

	public Yes_No UseAdditionalWeatherEffect = Yes_No.No;

	public Yes_No SpecialWeatherType = Yes_No.No;

	public SeasonEnum Season;

	public int CloudHeight = 1000;

	public CloudLevelEnum CloudLevel;

	public TemperatureTypeEnum TemperatureType = TemperatureTypeEnum.AboveFreezing;

	public ShaderControlEnum ShaderControl = ShaderControlEnum.None;

	public Yes_No OverrideCloudColor = Yes_No.No;

	public Gradient CloudColor;

	public Yes_No UseLightning = Yes_No.No;

	public Yes_No CustomizeWeatherIcon = Yes_No.No;

	public Yes_No WaitForCloudLevel = Yes_No.No;

	public Texture WeatherIcon;

	public void CreateWeatherSound()
	{
		UniStormSystem uniStormSystem = Object.FindObjectOfType<UniStormSystem>();
		if (uniStormSystem.enabled)
		{
			GameObject gameObject = new GameObject();
			gameObject.AddComponent<AudioSource>();
			AudioSource component = gameObject.GetComponent<AudioSource>();
			component.clip = WeatherSound;
			component.volume = 0f;
			component.loop = true;
			AudioMixer audioMixer = Resources.Load("UniStorm Audio Mixer") as AudioMixer;
			component.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master/Weather")[0];
			gameObject.name = WeatherTypeName + " (UniStorm)";
			gameObject.transform.SetParent(GameObject.Find("UniStorm Sounds").transform);
			gameObject.transform.position = new Vector3(gameObject.transform.parent.position.x, gameObject.transform.parent.position.y, gameObject.transform.parent.position.z);
			uniStormSystem.WeatherSoundsList.Add(component);
		}
	}

	public void CreateWeatherEffect()
	{
		UniStormSystem uniStormSystem = Object.FindObjectOfType<UniStormSystem>();
		ParticleSystem particleSystem = Object.Instantiate(WeatherEffect, Vector3.zero, Quaternion.AngleAxis(-90f, Vector3.right));
		particleSystem.transform.SetParent(GameObject.Find("UniStorm Effects").transform);
		particleSystem.name = particleSystem.name.Replace("(Clone)", " (UniStorm)");
		ParticleSystem.EmissionModule emission = particleSystem.emission;
		emission.enabled = true;
		emission.rateOverTime = new ParticleSystem.MinMaxCurve(0f);
		emission.SetBursts(new ParticleSystem.Burst[0]);
		uniStormSystem.WeatherEffectsList.Add(particleSystem);
	}

	public void CreateAdditionalWeatherEffect()
	{
		UniStormSystem uniStormSystem = Object.FindObjectOfType<UniStormSystem>();
		ParticleSystem particleSystem = Object.Instantiate(AdditionalWeatherEffect, Vector3.zero, Quaternion.AngleAxis(-90f, Vector3.right));
		particleSystem.transform.SetParent(GameObject.Find("UniStorm Effects").transform);
		particleSystem.name = particleSystem.name.Replace("(Clone)", " (UniStorm)");
		ParticleSystem.EmissionModule emission = particleSystem.emission;
		emission.enabled = true;
		emission.rateOverTime = new ParticleSystem.MinMaxCurve(0f);
		uniStormSystem.AdditionalWeatherEffectsList.Add(particleSystem);
	}
}
