using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/RCC UI Dashboard Displayer")]
[RequireComponent(typeof(RCC_DashboardInputs))]
public class RCC_UIDashboardDisplay : MonoBehaviour
{
	public enum DisplayType
	{
		Full,
		Customization,
		TopButtonsOnly,
		Off
	}

	private RCC_Settings RCCSettingsInstance;

	private RCC_DashboardInputs inputs;

	public DisplayType displayType;

	public GameObject topButtons;

	public GameObject controllerButtons;

	public GameObject gauges;

	public GameObject customizationMenu;

	public Text RPMLabel;

	public Text KMHLabel;

	public Text GearLabel;

	public Text recordingLabel;

	public Image ABS;

	public Image ESP;

	public Image Park;

	public Image Headlights;

	public Image leftIndicator;

	public Image rightIndicator;

	public Image heatIndicator;

	public Image fuelIndicator;

	public Image rpmIndicator;

	public Dropdown mobileControllers;

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

	private void Awake()
	{
		inputs = GetComponent<RCC_DashboardInputs>();
		if (!inputs)
		{
			base.enabled = false;
		}
	}

	private void Start()
	{
		CheckController();
	}

	private void OnEnable()
	{
		RCC_SceneManager.OnMainControllerChanged += CheckController;
	}

	private void CheckController()
	{
		if ((RCCSettings.controllerType == RCC_Settings.ControllerType.Keyboard || RCCSettings.controllerType == RCC_Settings.ControllerType.XBox360One) && (bool)mobileControllers)
		{
			mobileControllers.interactable = false;
		}
		if (RCCSettings.controllerType == RCC_Settings.ControllerType.Mobile && (bool)mobileControllers)
		{
			mobileControllers.interactable = true;
		}
	}

	private void Update()
	{
		switch (displayType)
		{
		case DisplayType.Full:
			if ((bool)topButtons && !topButtons.activeInHierarchy)
			{
				topButtons.SetActive(value: true);
			}
			if ((bool)controllerButtons && !controllerButtons.activeInHierarchy)
			{
				controllerButtons.SetActive(value: true);
			}
			if ((bool)gauges && !gauges.activeInHierarchy)
			{
				gauges.SetActive(value: true);
			}
			if ((bool)customizationMenu && customizationMenu.activeInHierarchy)
			{
				customizationMenu.SetActive(value: false);
			}
			break;
		case DisplayType.Customization:
			if ((bool)topButtons && topButtons.activeInHierarchy)
			{
				topButtons.SetActive(value: false);
			}
			if ((bool)controllerButtons && controllerButtons.activeInHierarchy)
			{
				controllerButtons.SetActive(value: false);
			}
			if ((bool)gauges && gauges.activeInHierarchy)
			{
				gauges.SetActive(value: false);
			}
			if ((bool)customizationMenu && !customizationMenu.activeInHierarchy)
			{
				customizationMenu.SetActive(value: true);
			}
			break;
		case DisplayType.TopButtonsOnly:
			if (!topButtons.activeInHierarchy)
			{
				topButtons.SetActive(value: true);
			}
			if (controllerButtons.activeInHierarchy)
			{
				controllerButtons.SetActive(value: false);
			}
			if (gauges.activeInHierarchy)
			{
				gauges.SetActive(value: false);
			}
			if (customizationMenu.activeInHierarchy)
			{
				customizationMenu.SetActive(value: false);
			}
			break;
		case DisplayType.Off:
			if ((bool)topButtons && topButtons.activeInHierarchy)
			{
				topButtons.SetActive(value: false);
			}
			if ((bool)controllerButtons && controllerButtons.activeInHierarchy)
			{
				controllerButtons.SetActive(value: false);
			}
			if ((bool)gauges && gauges.activeInHierarchy)
			{
				gauges.SetActive(value: false);
			}
			if ((bool)customizationMenu && customizationMenu.activeInHierarchy)
			{
				customizationMenu.SetActive(value: false);
			}
			break;
		}
	}

	private void LateUpdate()
	{
		if (!RCC_SceneManager.Instance.activePlayerVehicle)
		{
			return;
		}
		if ((bool)RPMLabel)
		{
			RPMLabel.text = inputs.RPM.ToString("0");
		}
		if ((bool)KMHLabel)
		{
			if (RCCSettings.units == RCC_Settings.Units.KMH)
			{
				KMHLabel.text = inputs.KMH.ToString("0") + "\nKMH";
			}
			else
			{
				KMHLabel.text = (inputs.KMH * 0.62f).ToString("0") + "\nMPH";
			}
		}
		if ((bool)GearLabel)
		{
			if (!inputs.NGear && !inputs.changingGear)
			{
				GearLabel.text = ((inputs.direction == 1) ? (inputs.Gear + 1f).ToString("0") : "R");
			}
			else
			{
				GearLabel.text = string.Concat(inputs.Gear + 1f);
			}
		}
		if ((bool)recordingLabel)
		{
			switch (RCC_SceneManager.Instance.recordMode)
			{
			case RCC_SceneManager.RecordMode.Neutral:
				if (recordingLabel.gameObject.activeInHierarchy)
				{
					recordingLabel.gameObject.SetActive(value: false);
				}
				recordingLabel.text = "";
				break;
			case RCC_SceneManager.RecordMode.Play:
				if (!recordingLabel.gameObject.activeInHierarchy)
				{
					recordingLabel.gameObject.SetActive(value: true);
				}
				recordingLabel.text = "Playing";
				recordingLabel.color = Color.green;
				break;
			case RCC_SceneManager.RecordMode.Record:
				if (!recordingLabel.gameObject.activeInHierarchy)
				{
					recordingLabel.gameObject.SetActive(value: true);
				}
				recordingLabel.text = "Recording";
				recordingLabel.color = Color.red;
				break;
			}
		}
		if ((bool)ABS)
		{
			ABS.color = (inputs.ABS ? Color.yellow : Color.white);
		}
		if ((bool)ESP)
		{
			ESP.color = (inputs.ESP ? Color.yellow : Color.white);
		}
		if ((bool)Park)
		{
			Park.color = (inputs.Park ? Color.red : Color.white);
		}
		if ((bool)Headlights)
		{
			Headlights.color = (inputs.Headlights ? Color.green : Color.white);
		}
		if ((bool)heatIndicator)
		{
			heatIndicator.color = ((RCC_SceneManager.Instance.activePlayerVehicle.engineHeat >= 100f) ? Color.red : new Color(0.1f, 0f, 0f));
		}
		if ((bool)fuelIndicator)
		{
			fuelIndicator.color = ((RCC_SceneManager.Instance.activePlayerVehicle.fuelTank < 10f) ? Color.red : new Color(0.1f, 0f, 0f));
		}
		if ((bool)rpmIndicator)
		{
			rpmIndicator.color = ((RCC_SceneManager.Instance.activePlayerVehicle.engineRPM >= RCC_SceneManager.Instance.activePlayerVehicle.maxEngineRPM - 500f) ? Color.red : new Color(0.1f, 0f, 0f));
		}
		if ((bool)leftIndicator && (bool)rightIndicator)
		{
			switch (inputs.indicators)
			{
			case RCC_CarControllerV3.IndicatorsOn.Left:
				leftIndicator.color = new Color(1f, 0.5f, 0f);
				rightIndicator.color = new Color(0.5f, 0.25f, 0f);
				break;
			case RCC_CarControllerV3.IndicatorsOn.Right:
				leftIndicator.color = new Color(0.5f, 0.25f, 0f);
				rightIndicator.color = new Color(1f, 0.5f, 0f);
				break;
			case RCC_CarControllerV3.IndicatorsOn.All:
				leftIndicator.color = new Color(1f, 0.5f, 0f);
				rightIndicator.color = new Color(1f, 0.5f, 0f);
				break;
			case RCC_CarControllerV3.IndicatorsOn.Off:
				leftIndicator.color = new Color(0.5f, 0.25f, 0f);
				rightIndicator.color = new Color(0.5f, 0.25f, 0f);
				break;
			}
		}
	}

	public void SetDisplayType(DisplayType _displayType)
	{
		displayType = _displayType;
	}

	private void OnDisable()
	{
		RCC_SceneManager.OnMainControllerChanged -= CheckController;
	}
}
