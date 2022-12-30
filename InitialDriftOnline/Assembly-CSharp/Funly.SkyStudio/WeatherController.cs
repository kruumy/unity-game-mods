using System;
using UnityEngine;

namespace Funly.SkyStudio;

public class WeatherController : MonoBehaviour
{
	private WeatherEnclosure m_Enclosure;

	private MeshRenderer m_EnclosureMeshRenderer;

	private WeatherEnclosureDetector detector;

	private SkyProfile m_Profile;

	private float m_TimeOfDay;

	public RainDownfallController rainDownfallController { get; protected set; }

	public RainSplashController rainSplashController { get; protected set; }

	public LightningController lightningController { get; protected set; }

	public WeatherDepthCamera weatherDepthCamera { get; protected set; }

	private void Awake()
	{
		DiscoverWeatherControllers();
	}

	private void Start()
	{
		DiscoverWeatherControllers();
	}

	private void OnEnable()
	{
		DiscoverWeatherControllers();
		if (detector == null)
		{
			Debug.LogError("Can't register for enclosure callbacks since there's no WeatherEnclosureDetector on any children");
			return;
		}
		WeatherEnclosureDetector weatherEnclosureDetector = detector;
		weatherEnclosureDetector.enclosureChangedCallback = (Action<WeatherEnclosure>)Delegate.Combine(weatherEnclosureDetector.enclosureChangedCallback, new Action<WeatherEnclosure>(OnEnclosureDidChange));
	}

	private void DiscoverWeatherControllers()
	{
		rainDownfallController = GetComponentInChildren<RainDownfallController>();
		rainSplashController = GetComponentInChildren<RainSplashController>();
		lightningController = GetComponentInChildren<LightningController>();
		weatherDepthCamera = GetComponentInChildren<WeatherDepthCamera>();
		detector = GetComponentInChildren<WeatherEnclosureDetector>();
	}

	private void OnDisable()
	{
		if (!(detector == null))
		{
			WeatherEnclosureDetector weatherEnclosureDetector = detector;
			weatherEnclosureDetector.enclosureChangedCallback = (Action<WeatherEnclosure>)Delegate.Remove(weatherEnclosureDetector.enclosureChangedCallback, new Action<WeatherEnclosure>(OnEnclosureDidChange));
		}
	}

	public void UpdateForTimeOfDay(SkyProfile skyProfile, float timeOfDay)
	{
		if ((bool)skyProfile)
		{
			m_Profile = skyProfile;
			m_TimeOfDay = timeOfDay;
			if (weatherDepthCamera != null)
			{
				weatherDepthCamera.enabled = skyProfile.IsFeatureEnabled("RainSplashFeature");
			}
			if (rainDownfallController != null)
			{
				rainDownfallController.UpdateForTimeOfDay(skyProfile, timeOfDay);
			}
			if (rainSplashController != null)
			{
				rainSplashController.UpdateForTimeOfDay(skyProfile, timeOfDay);
			}
			if (lightningController != null)
			{
				lightningController.UpdateForTimeOfDay(skyProfile, timeOfDay);
			}
		}
	}

	private void LateUpdate()
	{
		if (!(m_Profile == null))
		{
			if ((bool)m_EnclosureMeshRenderer && (bool)rainDownfallController && m_Profile.IsFeatureEnabled("RainFeature"))
			{
				m_EnclosureMeshRenderer.enabled = true;
			}
			else
			{
				m_EnclosureMeshRenderer.enabled = false;
			}
		}
	}

	private void OnEnclosureDidChange(WeatherEnclosure enclosure)
	{
		m_Enclosure = enclosure;
		if (m_Enclosure != null)
		{
			m_EnclosureMeshRenderer = m_Enclosure.GetComponentInChildren<MeshRenderer>();
		}
		rainDownfallController.SetWeatherEnclosure(m_Enclosure);
		UpdateForTimeOfDay(m_Profile, m_TimeOfDay);
	}
}
