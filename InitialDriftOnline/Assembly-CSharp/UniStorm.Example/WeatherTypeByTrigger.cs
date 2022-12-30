using UnityEngine;

namespace UniStorm.Example;

public class WeatherTypeByTrigger : MonoBehaviour
{
	public WeatherType m_WeatherType;

	public string TriggerTag = "Player";

	private void OnTriggerEnter(Collider C)
	{
		if (C.tag == TriggerTag)
		{
			UniStormSystem.Instance.ChangeWeather(m_WeatherType);
		}
	}
}
