using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio;

public class WeatherEnclosureDetector : MonoBehaviour
{
	[Tooltip("Default enclosure used when not inside the trigger of another enclosure area.")]
	public WeatherEnclosure mainEnclosure;

	private List<WeatherEnclosure> triggeredEnclosures = new List<WeatherEnclosure>();

	public RainDownfallController rainController;

	public Action<WeatherEnclosure> enclosureChangedCallback;

	private void Start()
	{
		ApplyEnclosure();
	}

	private void OnEnable()
	{
		ApplyEnclosure();
	}

	private void OnTriggerEnter(Collider other)
	{
		WeatherEnclosure componentInChildren = other.gameObject.GetComponentInChildren<WeatherEnclosure>();
		if ((bool)componentInChildren)
		{
			if (triggeredEnclosures.Contains(componentInChildren))
			{
				triggeredEnclosures.Remove(componentInChildren);
			}
			triggeredEnclosures.Add(componentInChildren);
			ApplyEnclosure();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		WeatherEnclosure componentInChildren = other.gameObject.GetComponentInChildren<WeatherEnclosure>();
		if ((bool)componentInChildren && triggeredEnclosures.Contains(componentInChildren))
		{
			triggeredEnclosures.Remove(componentInChildren);
			ApplyEnclosure();
		}
	}

	public void ApplyEnclosure()
	{
		WeatherEnclosure weatherEnclosure;
		if (triggeredEnclosures.Count > 0)
		{
			weatherEnclosure = triggeredEnclosures[triggeredEnclosures.Count - 1];
			if (!weatherEnclosure)
			{
				Debug.LogError("Failed to find mesh renderer on weather enclosure, using main enclosure instead.");
				weatherEnclosure = mainEnclosure;
			}
		}
		else
		{
			weatherEnclosure = mainEnclosure;
		}
		if (enclosureChangedCallback != null)
		{
			enclosureChangedCallback(weatherEnclosure);
		}
	}
}
