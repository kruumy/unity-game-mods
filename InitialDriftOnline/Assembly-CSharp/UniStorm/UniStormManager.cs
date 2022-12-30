using System;
using UniStorm.Effects;
using UniStorm.Utility;
using UnityEngine;
using UnityEngine.Rendering;

namespace UniStorm;

public class UniStormManager : MonoBehaviour
{
	public static UniStormManager Instance;

	private void Start()
	{
		Instance = this;
	}

	public void SetTime(int Hour, int Minute)
	{
		UniStormSystem.Instance.m_TimeFloat = (float)Hour / 24f + (float)Minute / 1440f;
	}

	public void SetDate(int Month, int Day, int Year)
	{
		UniStormSystem.Instance.UniStormDate = new DateTime(Year, Month, Day);
		UniStormSystem.Instance.Month = Month;
		UniStormSystem.Instance.Day = Day;
		UniStormSystem.Instance.Year = Year;
	}

	public DateTime GetDate()
	{
		return UniStormSystem.Instance.UniStormDate.Date;
	}

	public void ChangeWeatherWithTransition(WeatherType weatherType)
	{
		if (UniStormSystem.Instance.UniStormInitialized)
		{
			if (UniStormSystem.Instance.AllWeatherTypes.Contains(weatherType))
			{
				UniStormSystem.Instance.ChangeWeather(weatherType);
			}
			else
			{
				Debug.LogError("The weather type was not found in UniStorm Editor's All Weather Types list. The weather type must be added to the UniStorm Editor's All Weather Type list before it can be used.");
			}
		}
	}

	public void ChangeWeatherInstantly(WeatherType weatherType)
	{
		if (UniStormSystem.Instance.UniStormInitialized)
		{
			if (UniStormSystem.Instance.AllWeatherTypes.Contains(weatherType))
			{
				UniStormSystem.Instance.CurrentWeatherType = weatherType;
				UniStormSystem.Instance.InitializeWeather(UseWeatherConditions: false);
			}
			else
			{
				Debug.LogError("The weather type was not found in UniStorm Editor's All Weather Types list. The weather type must be added to the UniStorm Editor's All Weather Type list before it can be used.");
			}
		}
	}

	public void RandomWeather()
	{
		if (UniStormSystem.Instance.UniStormInitialized)
		{
			int index = UnityEngine.Random.Range(0, UniStormSystem.Instance.AllWeatherTypes.Count);
			UniStormSystem.Instance.ChangeWeather(UniStormSystem.Instance.AllWeatherTypes[index]);
		}
	}

	public void RegenerateForecast()
	{
		if (UniStormSystem.Instance.UniStormInitialized)
		{
			UniStormSystem.Instance.WeatherForecast.Clear();
			UniStormSystem.Instance.GenerateWeather();
		}
	}

	public void ChangeWeatherSoundsState(bool ActiveState)
	{
		if (UniStormSystem.Instance.UniStormInitialized)
		{
			UniStormSystem.Instance.m_SoundTransform.SetActive(ActiveState);
		}
	}

	public void ChangeWeatherEffectsState(bool ActiveState)
	{
		if (UniStormSystem.Instance.UniStormInitialized)
		{
			UniStormSystem.Instance.m_EffectsTransform.SetActive(ActiveState);
		}
	}

	public void ChangeCameraSource(Transform PlayerTransform, Camera CameraSource)
	{
		if (!UniStormSystem.Instance.UniStormInitialized)
		{
			return;
		}
		UniStormSystem.Instance.PlayerCamera.enabled = false;
		if (UniStormSystem.Instance.PlayerCamera.GetComponent<ScreenSpaceCloudShadows>() != null && UniStormSystem.Instance.CloudShadows == UniStormSystem.EnableFeature.Enabled)
		{
			UniStormSystem.Instance.PlayerCamera.GetComponent<ScreenSpaceCloudShadows>().enabled = false;
		}
		UniStormSystem.Instance.PlayerTransform = PlayerTransform;
		UniStormSystem.Instance.m_EffectsTransform.transform.SetParent(PlayerTransform);
		UniStormSystem.Instance.m_EffectsTransform.transform.localPosition = Vector3.zero;
		UniStormSystem.Instance.m_SoundTransform.transform.SetParent(PlayerTransform);
		UniStormSystem.Instance.m_SoundTransform.transform.localPosition = Vector3.zero;
		UniStormSystem.Instance.PlayerCamera = CameraSource;
		CommandBuffer[] commandBuffers = CameraSource.GetCommandBuffers(CameraEvent.AfterSkybox);
		if (commandBuffers.Length != 0)
		{
			for (int i = 0; i < commandBuffers.Length; i++)
			{
				if (commandBuffers[i].name != "Render Clouds")
				{
					CommandBuffer cloudsCommBuff = UnityEngine.Object.FindObjectOfType<UniStormClouds>().cloudsCommBuff;
					CameraSource.AddCommandBuffer(CameraEvent.AfterSkybox, cloudsCommBuff);
					cloudsCommBuff.name = "Render Clouds";
				}
			}
		}
		else if (commandBuffers.Length == 0)
		{
			CommandBuffer cloudsCommBuff2 = UnityEngine.Object.FindObjectOfType<UniStormClouds>().cloudsCommBuff;
			CameraSource.AddCommandBuffer(CameraEvent.AfterSkybox, cloudsCommBuff2);
			cloudsCommBuff2.name = "Render Clouds";
		}
		if (UniStormSystem.Instance.PlayerCamera.GetComponent<ScreenSpaceCloudShadows>() == null && UniStormSystem.Instance.CloudShadows == UniStormSystem.EnableFeature.Enabled)
		{
			ScreenSpaceCloudShadows screenSpaceCloudShadows = UniStormSystem.Instance.PlayerCamera.gameObject.AddComponent<ScreenSpaceCloudShadows>();
			screenSpaceCloudShadows.CloudShadowTexture = UnityEngine.Object.FindObjectOfType<UniStormClouds>().PublicCloudShadowTexture;
			screenSpaceCloudShadows.BottomThreshold = 0.5f;
			screenSpaceCloudShadows.TopThreshold = 1f;
			screenSpaceCloudShadows.CloudTextureScale = 0.001f;
			screenSpaceCloudShadows.ShadowIntensity = UniStormSystem.Instance.CurrentWeatherType.CloudShadowIntensity;
		}
		else if (UniStormSystem.Instance.CloudShadows == UniStormSystem.EnableFeature.Enabled)
		{
			UniStormSystem.Instance.PlayerCamera.GetComponent<ScreenSpaceCloudShadows>().enabled = true;
		}
		UniStormSystem.Instance.PlayerCamera.enabled = true;
	}

	public string GetWeatherForecastName()
	{
		return UniStormSystem.Instance.NextWeatherType.WeatherTypeName;
	}

	public int GetWeatherForecastHour()
	{
		return UniStormSystem.Instance.HourToChangeWeather;
	}

	public void SetMusicVolume(float Volume)
	{
		if (UniStormSystem.Instance.UniStormInitialized)
		{
			if (Volume <= 0f)
			{
				Volume = 0.001f;
			}
			else if (Volume > 1f)
			{
				Volume = 1f;
			}
			UniStormSystem.Instance.UniStormAudioMixer.SetFloat("MusicVolume", Mathf.Log(Volume) * 20f);
		}
	}

	public void SetAmbienceVolume(float Volume)
	{
		if (UniStormSystem.Instance.UniStormInitialized)
		{
			if (Volume <= 0f)
			{
				Volume = 0.001f;
			}
			else if (Volume > 1f)
			{
				Volume = 1f;
			}
			UniStormSystem.Instance.UniStormAudioMixer.SetFloat("AmbienceVolume", Mathf.Log(Volume) * 20f);
		}
	}

	public void SetWeatherVolume(float Volume)
	{
		if (UniStormSystem.Instance.UniStormInitialized)
		{
			if (Volume <= 0f)
			{
				Volume = 0.001f;
			}
			else if (Volume > 1f)
			{
				Volume = 1f;
			}
			UniStormSystem.Instance.UniStormAudioMixer.SetFloat("WeatherVolume", Mathf.Log(Volume) * 20f);
		}
	}

	public void SetDayLength(int MinuteLength)
	{
		UniStormSystem.Instance.DayLength = MinuteLength;
	}

	public void SetNightLength(int MinuteLength)
	{
		UniStormSystem.Instance.NightLength = MinuteLength;
	}

	public void ChangeMoonPhaseColor(Color MoonPhaseColor)
	{
		UniStormSystem.Instance.MoonPhaseColor = MoonPhaseColor;
	}

	public void UpdateCloudQuality(UniStormSystem.CloudQualityEnum CloudQuality)
	{
		if (UniStormSystem.Instance.UniStormInitialized)
		{
			UniStormSystem.Instance.CloudQuality = CloudQuality;
			UniStormClouds uniStormClouds = UnityEngine.Object.FindObjectOfType<UniStormClouds>();
			if (UniStormSystem.Instance.CloudShadows == UniStormSystem.EnableFeature.Enabled)
			{
				uniStormClouds.SetCloudDetails((UniStormClouds.CloudPerformance)CloudQuality, (UniStormClouds.CloudType)UniStormSystem.Instance.CloudType, UniStormClouds.CloudShadowsType.Simulated);
			}
			else if (UniStormSystem.Instance.CloudShadows == UniStormSystem.EnableFeature.Disabled)
			{
				uniStormClouds.SetCloudDetails((UniStormClouds.CloudPerformance)CloudQuality, (UniStormClouds.CloudType)UniStormSystem.Instance.CloudType, UniStormClouds.CloudShadowsType.Off);
			}
		}
	}

	public void UpdateCloudType(UniStormSystem.CloudTypeEnum CloudType)
	{
		if (UniStormSystem.Instance.UniStormInitialized)
		{
			UniStormSystem.Instance.CloudType = CloudType;
			UniStormClouds uniStormClouds = UnityEngine.Object.FindObjectOfType<UniStormClouds>();
			Material skyMaterial = UnityEngine.Object.FindObjectOfType<UniStormClouds>().skyMaterial;
			if (UniStormSystem.Instance.CloudType == UniStormSystem.CloudTypeEnum._2D)
			{
				skyMaterial.SetFloat("_uCloudsBaseEdgeSoftness", 0.2f);
				skyMaterial.SetFloat("_uCloudsBottomSoftness", 0.3f);
				skyMaterial.SetFloat("_uCloudsDetailStrength", 0.1f);
				skyMaterial.SetFloat("_uCloudsDensity", 0.3f);
			}
			else if (UniStormSystem.Instance.CloudType == UniStormSystem.CloudTypeEnum.Volumetric)
			{
				CloudProfile cloudProfileComponent = UniStormSystem.Instance.CurrentWeatherType.CloudProfileComponent;
				skyMaterial.SetFloat("_uCloudsBaseEdgeSoftness", cloudProfileComponent.EdgeSoftness);
				skyMaterial.SetFloat("_uCloudsBottomSoftness", cloudProfileComponent.BaseSoftness);
				skyMaterial.SetFloat("_uCloudsDensity", cloudProfileComponent.Density);
				skyMaterial.SetFloat("_uCloudsBaseScale", 1.72f);
				skyMaterial.SetFloat("_uCloudsCoverageBias", 0.082f);
				skyMaterial.SetFloat("_uCloudsDetailStrength", 0.082f);
			}
			if (UniStormSystem.Instance.CloudShadows == UniStormSystem.EnableFeature.Enabled)
			{
				uniStormClouds.SetCloudDetails((UniStormClouds.CloudPerformance)UniStormSystem.Instance.CloudQuality, (UniStormClouds.CloudType)UniStormSystem.Instance.CloudType, UniStormClouds.CloudShadowsType.Simulated);
			}
			else if (UniStormSystem.Instance.CloudShadows == UniStormSystem.EnableFeature.Disabled)
			{
				uniStormClouds.SetCloudDetails((UniStormClouds.CloudPerformance)UniStormSystem.Instance.CloudQuality, (UniStormClouds.CloudType)UniStormSystem.Instance.CloudType, UniStormClouds.CloudShadowsType.Off);
			}
		}
	}

	public string GetCurrentPrecipitationType()
	{
		if (Shader.GetGlobalFloat("_WetnessStrength") > Shader.GetGlobalFloat("_SnowStrength"))
		{
			return "Rain";
		}
		if (Shader.GetGlobalFloat("_SnowStrength") > Shader.GetGlobalFloat("_WetnessStrength"))
		{
			return "Snow";
		}
		return "None";
	}

	public void SetSunShaftsState(bool CurrentState)
	{
		if (UniStormSystem.Instance.UniStormInitialized)
		{
			UniStormSystem.Instance.PlayerCamera.GetComponent<UniStormSunShafts>().enabled = CurrentState;
		}
	}

	public void UpdateSunlightSettings()
	{
		if (UniStormSystem.Instance.UniStormInitialized)
		{
			UniStormSystem.Instance.m_SunLight.shadowResolution = UniStormSystem.Instance.SunShadowResolution;
			UniStormSystem.Instance.m_SunLight.shadows = UniStormSystem.Instance.SunShadowType;
		}
	}
}
