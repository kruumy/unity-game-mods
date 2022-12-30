using UnityEngine;

public class RCC_PoliceSiren : MonoBehaviour
{
	public enum SirenMode
	{
		Off,
		On
	}

	private RCC_AICarController AI;

	public SirenMode sirenMode;

	public Light[] redLights;

	public Light[] blueLights;

	private void Start()
	{
		AI = GetComponentInParent<RCC_AICarController>();
	}

	private void Update()
	{
		switch (sirenMode)
		{
		case SirenMode.Off:
		{
			for (int m = 0; m < redLights.Length; m++)
			{
				redLights[m].intensity = Mathf.Lerp(redLights[m].intensity, 0f, Time.deltaTime * 50f);
			}
			for (int n = 0; n < blueLights.Length; n++)
			{
				blueLights[n].intensity = Mathf.Lerp(blueLights[n].intensity, 0f, Time.deltaTime * 50f);
			}
			break;
		}
		case SirenMode.On:
		{
			if (Mathf.Approximately((int)Time.time % 2, 0f) && Mathf.Approximately((int)(Time.time * 20f) % 3, 0f))
			{
				for (int i = 0; i < redLights.Length; i++)
				{
					redLights[i].intensity = Mathf.Lerp(redLights[i].intensity, 1f, Time.deltaTime * 50f);
				}
				break;
			}
			for (int j = 0; j < redLights.Length; j++)
			{
				redLights[j].intensity = Mathf.Lerp(redLights[j].intensity, 0f, Time.deltaTime * 10f);
			}
			if (Mathf.Approximately((int)(Time.time * 20f) % 3, 0f))
			{
				for (int k = 0; k < blueLights.Length; k++)
				{
					blueLights[k].intensity = Mathf.Lerp(blueLights[k].intensity, 1f, Time.deltaTime * 50f);
				}
			}
			else
			{
				for (int l = 0; l < blueLights.Length; l++)
				{
					blueLights[l].intensity = Mathf.Lerp(blueLights[l].intensity, 0f, Time.deltaTime * 10f);
				}
			}
			break;
		}
		}
		if ((bool)AI)
		{
			if (AI.targetChase != null)
			{
				sirenMode = SirenMode.On;
			}
			else
			{
				sirenMode = SirenMode.Off;
			}
		}
	}

	public void SetSiren(bool state)
	{
		if (state)
		{
			sirenMode = SirenMode.On;
		}
		else
		{
			sirenMode = SirenMode.Off;
		}
	}
}
