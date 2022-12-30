using UnityEngine;
using UnityEngine.UI;

namespace UniStorm.Example;

public class UniStormUIDisplay : MonoBehaviour
{
	public Text UniStormTime;

	public Text UniStormTemperature;

	public RawImage UniStormWeatherIcon;

	private void Update()
	{
		UniStormTime.text = UniStormSystem.Instance.Hour + ":" + UniStormSystem.Instance.Minute.ToString("00");
		UniStormTemperature.text = UniStormSystem.Instance.Temperature + "°";
		UniStormWeatherIcon.texture = UniStormSystem.Instance.CurrentWeatherType.WeatherIcon;
	}
}
