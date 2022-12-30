using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/RCC UI Dashboard Inputs")]
public class RCC_DashboardInputs : MonoBehaviour
{
	private RCC_Settings RCCSettingsInstance;

	public GameObject RPMNeedle;

	public GameObject KMHNeedle;

	public GameObject turboGauge;

	public GameObject turboNeedle;

	public GameObject NOSGauge;

	public GameObject NoSNeedle;

	public GameObject heatGauge;

	public GameObject heatNeedle;

	public GameObject fuelGauge;

	public GameObject fuelNeedle;

	private float RPMNeedleRotation;

	private float KMHNeedleRotation;

	private float BoostNeedleRotation;

	private float NoSNeedleRotation;

	private float heatNeedleRotation;

	private float fuelNeedleRotation;

	internal float RPM;

	internal float KMH;

	internal int direction = 1;

	internal float Gear;

	internal bool changingGear;

	internal bool NGear;

	internal bool ABS;

	internal bool ESP;

	internal bool Park;

	internal bool Headlights;

	internal RCC_CarControllerV3.IndicatorsOn indicators;

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

	private void Update()
	{
		GetValues();
	}

	private void GetValues()
	{
		if (!RCC_SceneManager.Instance.activePlayerVehicle || !RCC_SceneManager.Instance.activePlayerVehicle.canControl || RCC_SceneManager.Instance.activePlayerVehicle.externalController)
		{
			return;
		}
		if ((bool)NOSGauge)
		{
			if (RCC_SceneManager.Instance.activePlayerVehicle.useNOS)
			{
				if (!NOSGauge.activeSelf)
				{
					NOSGauge.SetActive(value: true);
				}
			}
			else if (NOSGauge.activeSelf)
			{
				NOSGauge.SetActive(value: false);
			}
		}
		if ((bool)turboGauge)
		{
			if (RCC_SceneManager.Instance.activePlayerVehicle.useTurbo)
			{
				if (!turboGauge.activeSelf)
				{
					turboGauge.SetActive(value: true);
				}
			}
			else if (turboGauge.activeSelf)
			{
				turboGauge.SetActive(value: false);
			}
		}
		if ((bool)heatGauge)
		{
			if (RCC_SceneManager.Instance.activePlayerVehicle.useEngineHeat)
			{
				if (!heatGauge.activeSelf)
				{
					heatGauge.SetActive(value: true);
				}
			}
			else if (heatGauge.activeSelf)
			{
				heatGauge.SetActive(value: false);
			}
		}
		if ((bool)fuelGauge)
		{
			if (RCC_SceneManager.Instance.activePlayerVehicle.useFuelConsumption)
			{
				if (!fuelGauge.activeSelf)
				{
					fuelGauge.SetActive(value: true);
				}
			}
			else if (fuelGauge.activeSelf)
			{
				fuelGauge.SetActive(value: false);
			}
		}
		RPM = RCC_SceneManager.Instance.activePlayerVehicle.engineRPM;
		KMH = RCC_SceneManager.Instance.activePlayerVehicle.speed;
		direction = RCC_SceneManager.Instance.activePlayerVehicle.direction;
		Gear = RCC_SceneManager.Instance.activePlayerVehicle.currentGear;
		changingGear = RCC_SceneManager.Instance.activePlayerVehicle.changingGear;
		NGear = RCC_SceneManager.Instance.activePlayerVehicle.NGear;
		ABS = RCC_SceneManager.Instance.activePlayerVehicle.ABSAct;
		ESP = RCC_SceneManager.Instance.activePlayerVehicle.ESPAct;
		Park = RCC_SceneManager.Instance.activePlayerVehicle.handbrakeInput > 0.1f;
		Headlights = RCC_SceneManager.Instance.activePlayerVehicle.lowBeamHeadLightsOn || RCC_SceneManager.Instance.activePlayerVehicle.highBeamHeadLightsOn;
		indicators = RCC_SceneManager.Instance.activePlayerVehicle.indicatorsOn;
		if ((bool)RPMNeedle)
		{
			RPMNeedleRotation = RCC_SceneManager.Instance.activePlayerVehicle.engineRPM / 30f;
			RPMNeedle.transform.eulerAngles = new Vector3(RPMNeedle.transform.eulerAngles.x, RPMNeedle.transform.eulerAngles.y, RPMNeedleRotation);
		}
		if ((bool)KMHNeedle)
		{
			if (RCCSettings.units == RCC_Settings.Units.KMH)
			{
				KMHNeedleRotation = RCC_SceneManager.Instance.activePlayerVehicle.speed;
			}
			else
			{
				KMHNeedleRotation = RCC_SceneManager.Instance.activePlayerVehicle.speed * 0.62f;
			}
			KMHNeedle.transform.eulerAngles = new Vector3(KMHNeedle.transform.eulerAngles.x, KMHNeedle.transform.eulerAngles.y, 0f - KMHNeedleRotation);
		}
		if ((bool)turboNeedle)
		{
			BoostNeedleRotation = RCC_SceneManager.Instance.activePlayerVehicle.turboBoost / 30f * 270f;
			turboNeedle.transform.eulerAngles = new Vector3(turboNeedle.transform.eulerAngles.x, turboNeedle.transform.eulerAngles.y, 0f - BoostNeedleRotation);
		}
		if ((bool)NoSNeedle)
		{
			NoSNeedleRotation = RCC_SceneManager.Instance.activePlayerVehicle.NoS / 100f * 270f;
			NoSNeedle.transform.eulerAngles = new Vector3(NoSNeedle.transform.eulerAngles.x, NoSNeedle.transform.eulerAngles.y, 0f - NoSNeedleRotation);
		}
		if ((bool)heatNeedle)
		{
			heatNeedleRotation = RCC_SceneManager.Instance.activePlayerVehicle.engineHeat / 110f * 270f;
			heatNeedle.transform.eulerAngles = new Vector3(heatNeedle.transform.eulerAngles.x, heatNeedle.transform.eulerAngles.y, 0f - heatNeedleRotation);
		}
		if ((bool)fuelNeedle)
		{
			fuelNeedleRotation = RCC_SceneManager.Instance.activePlayerVehicle.fuelTank / RCC_SceneManager.Instance.activePlayerVehicle.fuelTankCapacity * 270f;
			fuelNeedle.transform.eulerAngles = new Vector3(fuelNeedle.transform.eulerAngles.x, fuelNeedle.transform.eulerAngles.y, 0f - fuelNeedleRotation);
		}
	}
}
