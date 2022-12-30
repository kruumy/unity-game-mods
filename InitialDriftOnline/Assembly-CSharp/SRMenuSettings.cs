using System;
using UnityEngine;
using UnityEngine.UI;

public class SRMenuSettings : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetScrollPosVerti()
	{
		int num = Convert.ToInt32(base.gameObject.transform.name.Split(':')[0].Replace("Item ", ""));
		float verticalNormalizedPosition = 1f / 24f * (24f - (float)num);
		if (24 - num <= 4)
		{
			verticalNormalizedPosition = 0f;
		}
		GetComponentInParent<ScrollRect>().verticalNormalizedPosition = verticalNormalizedPosition;
	}

	public void SetScrollPosVertiLauncher()
	{
		int num = Convert.ToInt32(base.gameObject.transform.name.Split(':')[0].Replace("Item ", ""));
		float verticalNormalizedPosition = 0.125f * (8f - (float)num);
		if (8 - num <= 4)
		{
			verticalNormalizedPosition = 0f;
		}
		else if (8 - num > 4)
		{
			verticalNormalizedPosition = 1f;
		}
		GetComponentInParent<ScrollRect>().verticalNormalizedPosition = verticalNormalizedPosition;
	}

	public void SetScrollPosVertiLauncherResolution()
	{
	}
}
