using System.Collections;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Light/RCC Light")]
public class RCC_Light : MonoBehaviour
{
	public enum LightType
	{
		HeadLight,
		BrakeLight,
		ReverseLight,
		Indicator,
		ParkLight,
		HighBeamHeadLight,
		External
	}

	private RCC_Settings RCCSettingsInstance;

	private RCC_CarControllerV3 carController;

	private Light _light;

	private Projector projector;

	private LensFlare lensFlare;

	private Camera mainCamera;

	public float flareBrightness = 1.5f;

	private float finalFlareBrightness;

	public LightType lightType;

	public float inertia = 1f;

	public Flare flare;

	public int refreshRate = 30;

	private float refreshTimer;

	private bool parkLightFound;

	private bool highBeamLightFound;

	private RCC_CarControllerV3.IndicatorsOn indicatorsOn;

	private AudioSource indicatorSound;

	private int IndicatorState;

	private int XboxCanPush;

	private RCC_Settings RCCSettings
	{
		get
		{
			if (RCCSettingsInstance == null)
			{
				RCCSettingsInstance = RCC_Settings.Instance;
				return RCCSettingsInstance;
			}
			return RCCSettingsInstance;
		}
	}

	public AudioClip indicatorClip => RCCSettings.indicatorClip;

	private void Start()
	{
		IndicatorState = 0;
		XboxCanPush = 0;
		carController = GetComponentInParent<RCC_CarControllerV3>();
		_light = GetComponent<Light>();
		_light.enabled = true;
		lensFlare = GetComponent<LensFlare>();
		if ((bool)lensFlare && _light.flare != null)
		{
			_light.flare = null;
		}
		if (RCCSettings.useLightProjectorForLightingEffect)
		{
			projector = GetComponent<Projector>();
			if (projector == null)
			{
				projector = Object.Instantiate(RCCSettings.projector, base.transform.position, base.transform.rotation).GetComponent<Projector>();
				projector.transform.SetParent(base.transform, worldPositionStays: true);
			}
			projector.ignoreLayers = RCCSettings.projectorIgnoreLayer;
			if (lightType != 0)
			{
				projector.transform.localRotation = Quaternion.Euler(20f, (base.transform.localPosition.z > 0f) ? 0f : 180f, 0f);
			}
			Material material = new Material(projector.material);
			projector.material = material;
		}
		if (RCCSettings.useLightsAsVertexLights)
		{
			_light.renderMode = LightRenderMode.ForceVertex;
			_light.cullingMask = 0;
		}
		else
		{
			_light.renderMode = LightRenderMode.ForcePixel;
		}
		if (lightType == LightType.Indicator)
		{
			if (!carController.transform.Find("All Audio Sources/Indicator Sound AudioSource"))
			{
				indicatorSound = RCC_CreateAudioSource.NewAudioSource(carController.audioMixer, carController.gameObject, "Indicator Sound AudioSource", 1f, 3f, 1f, indicatorClip, loop: false, playNow: false, destroyAfterFinished: false);
			}
			else
			{
				indicatorSound = carController.transform.Find("All Audio Sources/Indicator Sound AudioSource").GetComponent<AudioSource>();
			}
		}
		RCC_Light[] componentsInChildren = carController.GetComponentsInChildren<RCC_Light>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].lightType == LightType.ParkLight)
			{
				parkLightFound = true;
			}
			if (componentsInChildren[i].lightType == LightType.HighBeamHeadLight)
			{
				highBeamLightFound = true;
			}
		}
		CheckRotation();
		CheckLensFlare();
	}

	private void OnEnable()
	{
		if (!_light)
		{
			_light = GetComponent<Light>();
		}
		_light.intensity = 0f;
	}

	private void Update()
	{
		if (RCCSettings.useLightProjectorForLightingEffect)
		{
			Projectors();
		}
		if ((bool)lensFlare)
		{
			LensFlare();
		}
		switch (lightType)
		{
		case LightType.HeadLight:
			if (highBeamLightFound)
			{
				Lighting(carController.lowBeamHeadLightsOn ? 0.5f : 0f, 50f, 90f);
				break;
			}
			Lighting(carController.lowBeamHeadLightsOn ? 0.5f : 0f, 50f, 90f);
			if (!carController.lowBeamHeadLightsOn && !carController.highBeamHeadLightsOn)
			{
				Lighting(0f);
			}
			if (carController.lowBeamHeadLightsOn && !carController.highBeamHeadLightsOn)
			{
				Lighting(0.6f, 100f, 90f);
				base.transform.localEulerAngles = new Vector3(10f, 0f, 0f);
			}
			else if (carController.highBeamHeadLightsOn)
			{
				Lighting(1.1f, 100f, 45f);
				base.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			}
			break;
		case LightType.BrakeLight:
			if (parkLightFound)
			{
				Lighting((carController._brakeInput >= 0.1f) ? 1f : 0f);
			}
			else
			{
				Lighting((carController._brakeInput >= 0.1f) ? 1f : ((!carController.lowBeamHeadLightsOn) ? 0f : 0.25f));
			}
			break;
		case LightType.ReverseLight:
			Lighting((carController.direction == -1) ? 1f : 0f);
			break;
		case LightType.ParkLight:
			Lighting((!carController.lowBeamHeadLightsOn) ? 0f : 0.5f);
			break;
		case LightType.Indicator:
			indicatorsOn = carController.indicatorsOn;
			Indicators();
			break;
		case LightType.HighBeamHeadLight:
			Lighting(carController.highBeamHeadLightsOn ? 1f : 0f, 200f, 45f);
			break;
		}
	}

	private void Lighting(float input)
	{
		_light.intensity = Mathf.Lerp(_light.intensity, input, Time.deltaTime * inertia * 20f);
	}

	private void Lighting(float input, float range, float spotAngle)
	{
		_light.intensity = Mathf.Lerp(_light.intensity, input, Time.deltaTime * inertia * 20f);
		_light.range = range;
		_light.spotAngle = spotAngle;
	}

	private void Indicators()
	{
		Vector3 vector = carController.transform.InverseTransformPoint(base.transform.position);
		switch (indicatorsOn)
		{
		case RCC_CarControllerV3.IndicatorsOn.Left:
			if (vector.x > 0f)
			{
				Lighting(0f);
				break;
			}
			if (carController.indicatorTimer >= 0.5f)
			{
				Lighting(0f);
				if (indicatorSound.isPlaying)
				{
					indicatorSound.Stop();
				}
			}
			else
			{
				Lighting(1f);
				if (!indicatorSound.isPlaying && carController.indicatorTimer <= 0.05f)
				{
					indicatorSound.Play();
				}
			}
			if (carController.indicatorTimer >= 1f)
			{
				carController.indicatorTimer = 0f;
			}
			break;
		case RCC_CarControllerV3.IndicatorsOn.Right:
			if (vector.x < 0f)
			{
				Lighting(0f);
				break;
			}
			if (carController.indicatorTimer >= 0.5f)
			{
				Lighting(0f);
				if (indicatorSound.isPlaying)
				{
					indicatorSound.Stop();
				}
			}
			else
			{
				Lighting(1f);
				if (!indicatorSound.isPlaying && carController.indicatorTimer <= 0.05f)
				{
					indicatorSound.Play();
				}
			}
			if (carController.indicatorTimer >= 1f)
			{
				carController.indicatorTimer = 0f;
			}
			break;
		case RCC_CarControllerV3.IndicatorsOn.All:
			if (carController.indicatorTimer >= 0.5f)
			{
				Lighting(0f);
				if (indicatorSound.isPlaying)
				{
					indicatorSound.Stop();
				}
			}
			else
			{
				Lighting(1f);
				if (!indicatorSound.isPlaying && carController.indicatorTimer <= 0.05f)
				{
					indicatorSound.Play();
				}
			}
			if (carController.indicatorTimer >= 1f)
			{
				carController.indicatorTimer = 0f;
			}
			break;
		case RCC_CarControllerV3.IndicatorsOn.Off:
			Lighting(0f);
			carController.indicatorTimer = 0f;
			break;
		}
	}

	private IEnumerator IndicatorImpulse()
	{
		yield return new WaitForSeconds(0.1f);
		XboxCanPush = 0;
	}

	private void Projectors()
	{
		if (!_light.enabled)
		{
			projector.enabled = false;
			return;
		}
		projector.enabled = true;
		projector.material.color = _light.color * (_light.intensity / 5f);
		projector.farClipPlane = Mathf.Lerp(10f, 40f, (_light.range - 50f) / 150f);
		projector.fieldOfView = Mathf.Lerp(40f, 30f, (_light.range - 50f) / 150f);
	}

	private void LensFlare()
	{
		if (refreshTimer > 1f / (float)refreshRate)
		{
			refreshTimer = 0f;
			if (!mainCamera)
			{
				mainCamera = RCC_SceneManager.Instance.activeMainCamera;
			}
			if (!mainCamera)
			{
				return;
			}
			float num = Vector3.Distance(base.transform.position, mainCamera.transform.position);
			float num2 = 1f;
			if (lightType != LightType.External)
			{
				num2 = Vector3.Angle(base.transform.forward, mainCamera.transform.position - base.transform.position);
			}
			if (num2 != 0f)
			{
				finalFlareBrightness = flareBrightness * (4f / num) * ((300f - 3f * num2) / 300f) / 3f;
			}
			lensFlare.brightness = finalFlareBrightness * _light.intensity;
			lensFlare.color = _light.color;
		}
		refreshTimer += Time.deltaTime;
	}

	private void CheckRotation()
	{
		if (base.transform.GetComponentInParent<RCC_CarControllerV3>().transform.InverseTransformPoint(base.transform.position).z > 0f)
		{
			if (Mathf.Abs(base.transform.localRotation.y) > 0.5f)
			{
				base.transform.localRotation = Quaternion.identity;
			}
		}
		else if (Mathf.Abs(base.transform.localRotation.y) < 0.5f)
		{
			base.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
		}
	}

	private void CheckLensFlare()
	{
		if (base.transform.GetComponent<LensFlare>() == null)
		{
			base.gameObject.AddComponent<LensFlare>();
			LensFlare component = base.gameObject.GetComponent<LensFlare>();
			component.brightness = 0f;
			component.color = Color.white;
			component.fadeSpeed = 20f;
		}
		if (base.gameObject.GetComponent<LensFlare>().flare == null)
		{
			base.gameObject.GetComponent<LensFlare>().flare = flare;
		}
		base.gameObject.GetComponent<Light>().flare = null;
	}

	private void Reset()
	{
		CheckRotation();
		CheckLensFlare();
	}
}
