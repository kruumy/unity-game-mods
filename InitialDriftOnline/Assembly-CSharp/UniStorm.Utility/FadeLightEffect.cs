using UnityEngine;

namespace UniStorm.Utility;

public class FadeLightEffect : MonoBehaviour
{
	public AnimationCurve LightCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	public float FadeMultipler = 1f;

	private Light LightSource;

	private float Timer;

	private void OnEnable()
	{
		Timer = 0f;
	}

	private void Start()
	{
		if (GetComponent<Light>() != null)
		{
			LightSource = GetComponent<Light>();
		}
		else
		{
			GetComponent<FadeLightEffect>().enabled = false;
		}
	}

	private void Update()
	{
		Timer += Time.deltaTime;
		LightSource.intensity = LightCurve.Evaluate(Timer * FadeMultipler);
	}
}
