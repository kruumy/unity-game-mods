using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniStorm.Example;

public class DemoUIController : MonoBehaviour
{
	public Dropdown QualityDropdown;

	public Dropdown CloudTypeDropdown;

	public Slider TimeSlider;

	public GameObject WeatherButtonGameObject;

	public GameObject TimeSliderGameObject;

	public Dropdown WeatherDropdown;

	public GameObject DemoMenu;

	public Toggle SunShaftsToggle;

	public Toggle ShadowsToggle;

	public Toggle CloudShadowsToggle;

	public Toggle TimeFlowToggle;

	public Text Temperature;

	public Text Time;

	private Text CloudLevelText;

	private GameObject SliderMenu;

	private void Start()
	{
		QualityDropdown = GameObject.Find("Cloud Quality Dropdown").GetComponent<Dropdown>();
		QualityDropdown.onValueChanged.AddListener(delegate
		{
			UpdateCloudQuality();
		});
		CloudTypeDropdown = GameObject.Find("Cloud Type Dropdown").GetComponent<Dropdown>();
		CloudTypeDropdown.onValueChanged.AddListener(delegate
		{
			UpdateCloudType();
		});
		SunShaftsToggle = GameObject.Find("Sun Shafts Toggle").GetComponent<Toggle>();
		SunShaftsToggle.onValueChanged.AddListener(delegate
		{
			ControlSunShaftsState();
		});
		ShadowsToggle = GameObject.Find("Shadows Toggle").GetComponent<Toggle>();
		ShadowsToggle.onValueChanged.AddListener(delegate
		{
			ControlShadowsState();
		});
		CloudShadowsToggle = GameObject.Find("Cloud Shadows Toggle").GetComponent<Toggle>();
		CloudShadowsToggle.onValueChanged.AddListener(delegate
		{
			ControlCloudShadowsState();
		});
		TimeFlowToggle = GameObject.Find("Time Flow Toggle").GetComponent<Toggle>();
		TimeFlowToggle.onValueChanged.AddListener(delegate
		{
			ControlTimeFlowState();
		});
		Temperature = GameObject.Find("Temperature").GetComponent<Text>();
		Time = GameObject.Find("Time").GetComponent<Text>();
		StartCoroutine(WaitForInilialization());
	}

	private IEnumerator WaitForInilialization()
	{
		yield return new WaitUntil(() => UniStormSystem.Instance.UniStormInitialized);
		CreateUniStormMenu();
	}

	public void ControlSunShaftsState()
	{
		UniStormManager.Instance.SetSunShaftsState(SunShaftsToggle.isOn);
	}

	public void ControlTimeFlowState()
	{
		if (TimeFlowToggle.isOn)
		{
			UniStormSystem.Instance.TimeFlow = UniStormSystem.EnableFeature.Enabled;
		}
		else if (!TimeFlowToggle.isOn)
		{
			UniStormSystem.Instance.TimeFlow = UniStormSystem.EnableFeature.Disabled;
		}
	}

	public void UpdateTime()
	{
		Time.text = UniStormSystem.Instance.Hour + ":" + UniStormSystem.Instance.Minute.ToString("00");
	}

	public void UpdateTemperature()
	{
		Temperature.text = UniStormSystem.Instance.Temperature + "°";
	}

	public void UpdateTimeSlider()
	{
		TimeSlider.value = UniStormSystem.Instance.m_TimeFloat;
	}

	public void ControlShadowsState()
	{
		if (!ShadowsToggle.isOn)
		{
			UniStormSystem.Instance.m_SunLight.shadows = LightShadows.None;
			UniStormSystem.Instance.m_MoonLight.shadows = LightShadows.None;
			UniStormSystem.Instance.m_LightningLight.shadows = LightShadows.None;
		}
		else if (ShadowsToggle.isOn)
		{
			UniStormSystem.Instance.m_SunLight.shadows = LightShadows.Soft;
			UniStormSystem.Instance.m_MoonLight.shadows = LightShadows.Soft;
			UniStormSystem.Instance.m_LightningLight.shadows = LightShadows.Soft;
		}
	}

	public void ControlCloudShadowsState()
	{
		if (UniStormSystem.Instance.CloudShadows == UniStormSystem.EnableFeature.Enabled)
		{
			UniStormSystem.Instance.m_CloudShadows.enabled = !UniStormSystem.Instance.m_CloudShadows.enabled;
		}
	}

	public void EnableObject(GameObject ObjectToEnable)
	{
		ObjectToEnable.SetActive(value: true);
	}

	public void DisableObject(GameObject ObjectToDisable)
	{
		ObjectToDisable.SetActive(value: false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			DemoMenu.SetActive(!DemoMenu.activeSelf);
			SliderMenu.SetActive(DemoMenu.activeSelf);
		}
	}

	public void QuitButton()
	{
		Application.Quit();
	}

	public void ChangeWeatherUI()
	{
		UniStormManager.Instance.ChangeWeatherWithTransition(UniStormSystem.Instance.AllWeatherTypes[WeatherDropdown.value]);
	}

	public void CalculateTimeSlider()
	{
		UniStormSystem.Instance.m_TimeFloat = TimeSlider.value;
	}

	public void UpdateCloudQuality()
	{
		UniStormManager.Instance.UpdateCloudQuality((UniStormSystem.CloudQualityEnum)QualityDropdown.value);
	}

	public void UpdateCloudType()
	{
		UniStormManager.Instance.UpdateCloudType((UniStormSystem.CloudTypeEnum)CloudTypeDropdown.value);
	}

	private void CreateUniStormMenu()
	{
		GameObject gameObject = Object.Instantiate((GameObject)Resources.Load("UniStorm Canvas"), base.transform.position, Quaternion.identity);
		TimeSlider = GameObject.Find("Time Slider").GetComponent<Slider>();
		TimeSliderGameObject = TimeSlider.gameObject;
		TimeSlider.onValueChanged.AddListener(delegate
		{
			CalculateTimeSlider();
		});
		TimeSlider.maxValue = 0.995f;
		WeatherButtonGameObject = GameObject.Find("Change Weather Button");
		WeatherButtonGameObject.SetActive(value: false);
		GameObject.Find("Weather Dropdown").SetActive(value: false);
		WeatherDropdown = GameObject.Find("Weather Dropdown (1)").GetComponent<Dropdown>();
		List<string> list = new List<string>();
		for (int i = 0; i < UniStormSystem.Instance.AllWeatherTypes.Count; i++)
		{
			if (UniStormSystem.Instance.AllWeatherTypes[i] != null)
			{
				list.Add(UniStormSystem.Instance.AllWeatherTypes[i].WeatherTypeName);
			}
		}
		WeatherDropdown.AddOptions(list);
		WeatherDropdown.itemText.fontSize = 8;
		WeatherDropdown.itemText.fontStyle = FontStyle.Bold;
		WeatherDropdown.value = UniStormSystem.Instance.AllWeatherTypes.IndexOf(UniStormSystem.Instance.CurrentWeatherType);
		WeatherDropdown.onValueChanged.AddListener(delegate
		{
			ChangeWeatherUI();
		});
		TimeSlider.value = UniStormSystem.Instance.m_TimeFloat;
		WeatherDropdown.value = UniStormSystem.Instance.AllWeatherTypes.IndexOf(UniStormSystem.Instance.CurrentWeatherType);
		if (Object.FindObjectOfType<EventSystem>() == null)
		{
			GameObject obj = new GameObject();
			obj.name = "EventSystem";
			obj.AddComponent<EventSystem>();
			obj.AddComponent<StandaloneInputModule>();
		}
		TimeSliderGameObject.transform.SetParent(GameObject.Find("Slider Canvas").transform);
		TimeSliderGameObject.transform.localPosition = new Vector3(0f, -185f, 0f);
		DemoMenu.SetActive(value: false);
		gameObject.SetActive(value: false);
		UniStormSystem.Instance.OnHourChangeEvent.AddListener(delegate
		{
			UpdateTimeSlider();
		});
		UniStormSystem.Instance.OnHourChangeEvent.AddListener(delegate
		{
			UpdateTemperature();
		});
		UpdateTemperature();
		InvokeRepeating("UpdateTime", 0.05f, 0.25f);
		SliderMenu = GameObject.Find("Slider Canvas");
	}
}
