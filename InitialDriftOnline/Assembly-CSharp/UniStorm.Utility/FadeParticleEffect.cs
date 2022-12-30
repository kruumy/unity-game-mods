using UnityEngine;

namespace UniStorm.Utility;

public class FadeParticleEffect : MonoBehaviour
{
	public AnimationCurve ParticleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	public float FadeMultipler = 1f;

	private ParticleSystem ParticleSource;

	private float EmissionAmount;

	private float Timer;

	private void OnEnable()
	{
		if (ParticleSource != null)
		{
			ParticleSystem.EmissionModule emission = ParticleSource.emission;
			emission.rateOverTime = new ParticleSystem.MinMaxCurve(EmissionAmount);
			Timer = 0f;
		}
	}

	private void Start()
	{
		if (GetComponent<ParticleSystem>() != null)
		{
			ParticleSource = GetComponent<ParticleSystem>();
			EmissionAmount = ParticleSource.emission.rateOverTime.constant;
		}
		else
		{
			GetComponent<FadeParticleEffect>().enabled = false;
		}
	}

	private void Update()
	{
		Timer += Time.deltaTime;
		ParticleSystem.EmissionModule emission = ParticleSource.emission;
		emission.rateOverTime = new ParticleSystem.MinMaxCurve(ParticleCurve.Evaluate(Timer * FadeMultipler));
	}
}
