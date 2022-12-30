using UnityEngine;

namespace UniStorm.Example;

public class TurnOffLight : MonoBehaviour
{
	private Light LightSource;

	private void Start()
	{
		LightSource = GetComponent<Light>();
		if (UniStormSystem.Instance.CurrentTimeOfDay == UniStormSystem.CurrentTimeOfDayEnum.Night)
		{
			LightSource.enabled = true;
		}
		else
		{
			LightSource.enabled = false;
		}
	}

	private void Update()
	{
		if (UniStormSystem.Instance.CurrentTimeOfDay == UniStormSystem.CurrentTimeOfDayEnum.Night)
		{
			LightSource.enabled = true;
		}
		else
		{
			LightSource.enabled = false;
		}
	}
}
