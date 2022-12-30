using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CarComponents : MonoBehaviour
{
	public bool blink;

	[Header("Lights")]
	public bool frontLightsOn;

	public bool brakeEffectsOn;

	public bool indicatorLightOn;

	[Space(5f)]
	public GameObject brakeEffects;

	public GameObject frontLightEffects;

	public GameObject reverseEffect;

	public GameObject indicatorR;

	public GameObject indicatorL;

	[Header("Needles")]
	public Transform SpeedNeedle;

	public Vector2 SpeedNeedleRotateRange = Vector3.zero;

	private Vector3 SpeedEulers = Vector3.zero;

	public Transform RpmNeedle;

	public Vector2 RpmNeedleRotateRange = Vector3.zero;

	private Vector3 RpmdEulers = Vector3.zero;

	public float _NeedleSmoothing = 1f;

	public Transform steeringWheel;

	private float rotateNeedles;

	[Header("Wheels")]
	public Transform wheel_FR;

	public Transform wheel_FL;

	[Header("Panel Texts")]
	public Text txtSpeed;

	[Header("Panel Texts")]
	public Text txtRPM;

	[Header("Panel Texts")]
	public Text txtSpeed2;

	public Slider sliderRPM;

	private IEnumerator coroutine;

	private IEnumerator coroutineIndicator;

	private void Start()
	{
		blink = true;
		frontLightsOn = true;
		brakeEffectsOn = true;
		indicatorLightOn = true;
		if ((bool)SpeedNeedle)
		{
			SpeedEulers = SpeedNeedle.localEulerAngles;
		}
		if ((bool)RpmNeedle)
		{
			RpmdEulers = RpmNeedle.localEulerAngles;
		}
		coroutine = WaitLights(2f);
		StartCoroutine(coroutine);
		coroutineIndicator = WaitIndicatorLights(0.5f);
		StartCoroutine(coroutineIndicator);
	}

	private void Update()
	{
		if (blink)
		{
			TurnOnFrontLights();
			TurnOnBackLights();
			TurnOnIndicatorLights();
		}
		if ((bool)SpeedNeedle)
		{
			Vector3 b = new Vector3(SpeedEulers.x, SpeedEulers.y, Mathf.Lerp(SpeedNeedleRotateRange.x, SpeedNeedleRotateRange.y, rotateNeedles));
			SpeedNeedle.localEulerAngles = Vector3.Lerp(SpeedNeedle.localEulerAngles, b, Time.deltaTime * _NeedleSmoothing);
		}
		if ((bool)RpmNeedle)
		{
			Vector3 b2 = new Vector3(RpmdEulers.x, RpmdEulers.y, Mathf.Lerp(RpmNeedleRotateRange.x, RpmNeedleRotateRange.y, rotateNeedles));
			RpmNeedle.localEulerAngles = Vector3.Lerp(RpmNeedle.localEulerAngles, b2, Time.deltaTime * _NeedleSmoothing);
		}
		if (steeringWheel != null)
		{
			Vector3 eulerAngles = steeringWheel.localRotation.eulerAngles;
			Vector3 eulerAngles2 = wheel_FL.localRotation.eulerAngles;
			eulerAngles.z = rotateNeedles * 15f;
			eulerAngles2.y = (0f - rotateNeedles) * 15f;
			steeringWheel.localRotation = Quaternion.Slerp(steeringWheel.localRotation, Quaternion.Euler(eulerAngles), Time.deltaTime * 2.5f);
			wheel_FL.localRotation = Quaternion.Slerp(wheel_FL.localRotation, Quaternion.Euler(eulerAngles2), Time.deltaTime * 2.5f);
			wheel_FR.localRotation = Quaternion.Slerp(wheel_FR.localRotation, Quaternion.Euler(eulerAngles2), Time.deltaTime * 2.5f);
		}
		if ((bool)txtSpeed && txtSpeed != null)
		{
			txtSpeed.text = ((int)(rotateNeedles * 100f)).ToString();
		}
		if (txtSpeed2 != null)
		{
			txtSpeed2.text = ((int)(rotateNeedles * 100f)).ToString();
		}
		if ((bool)txtRPM)
		{
			txtRPM.text = ((int)(rotateNeedles * 1000f)).ToString();
		}
		if ((bool)sliderRPM)
		{
			sliderRPM.value = rotateNeedles * 1000f;
		}
	}

	public void TurnOnFrontLights()
	{
		if (frontLightsOn)
		{
			frontLightEffects.SetActive(value: true);
			rotateNeedles += Time.deltaTime;
		}
		else
		{
			frontLightEffects.SetActive(value: false);
			rotateNeedles -= Time.deltaTime;
		}
	}

	public void TurnOnBackLights()
	{
		if (brakeEffectsOn)
		{
			brakeEffects.SetActive(value: true);
		}
		else
		{
			brakeEffects.SetActive(value: false);
		}
	}

	private IEnumerator WaitLights(float waitTime)
	{
		while (true)
		{
			yield return new WaitForSeconds(waitTime);
			frontLightsOn = !frontLightsOn;
			brakeEffectsOn = !brakeEffectsOn;
		}
	}

	private IEnumerator WaitIndicatorLights(float waitTime)
	{
		while (true)
		{
			yield return new WaitForSeconds(waitTime);
			indicatorLightOn = !indicatorLightOn;
		}
	}

	public void TurnOnIndicatorLights()
	{
		if (indicatorLightOn)
		{
			indicatorR.SetActive(value: true);
			indicatorL.SetActive(value: true);
		}
		else
		{
			indicatorR.SetActive(value: false);
			indicatorL.SetActive(value: false);
		}
	}
}
