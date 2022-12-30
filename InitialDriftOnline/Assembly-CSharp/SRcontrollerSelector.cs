using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class SRcontrollerSelector : MonoBehaviour
{
	private PlayerIndex playerIndex;

	private GamePadState state;

	private GamePadState prevState;

	public GameObject PS4TUTO;

	public GameObject LogitechTuto;

	public GameObject pslogo;

	private int showtutoint;

	private RCC_Settings RCCSettingsInstance;

	public Dropdown Controller;

	public GameObject ResolutionDp;

	public GameObject VibrationToggle;

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

	public void Awake()
	{
		showtutoint = 0;
		StartCoroutine(ShowTuto());
	}

	private IEnumerator ShowTuto()
	{
		yield return new WaitForSeconds(2f);
		showtutoint = 1;
	}

	private void Start()
	{
		Navigation navigation = Controller.navigation;
		if (PlayerPrefs.GetString("ControllerTypeChoose") == "Keyboard" || PlayerPrefs.GetString("ControllerTypeChoose") == "")
		{
			Controller.value = 0;
			RCCSettings.controllerType = RCC_Settings.ControllerType.Keyboard;
			navigation.selectOnRight = ResolutionDp.GetComponent<Dropdown>();
			pslogo.SetActive(value: false);
		}
		else if (PlayerPrefs.GetString("ControllerTypeChoose") == "Xbox360One")
		{
			Controller.value = 1;
			RCCSettings.controllerType = RCC_Settings.ControllerType.XBox360One;
			navigation.selectOnRight = VibrationToggle.GetComponent<Toggle>();
			pslogo.SetActive(value: true);
		}
		else if (PlayerPrefs.GetString("ControllerTypeChoose") == "PS4")
		{
			Controller.value = 1;
			RCCSettings.controllerType = RCC_Settings.ControllerType.XBox360One;
			navigation.selectOnRight = VibrationToggle.GetComponent<Toggle>();
			pslogo.SetActive(value: true);
		}
		else if (PlayerPrefs.GetString("ControllerTypeChoose") == "LogitechSteeringWheel")
		{
			Controller.value = 2;
			RCCSettings.controllerType = RCC_Settings.ControllerType.LogitechSteeringWheel;
			navigation.selectOnRight = VibrationToggle.GetComponent<Toggle>();
			pslogo.SetActive(value: false);
		}
		Controller.navigation = navigation;
		StartCoroutine(DisableTuto());
	}

	private IEnumerator DisableTuto()
	{
		yield return new WaitForSeconds(1f);
		PS4TUTO.SetActive(value: false);
		LogitechTuto.SetActive(value: false);
	}

	public void SetDropValueInServerList()
	{
		if (PlayerPrefs.GetString("ControllerTypeChoose") == "Keyboard" || PlayerPrefs.GetString("ControllerTypeChoose") == "")
		{
			Controller.value = 0;
		}
		else if (PlayerPrefs.GetString("ControllerTypeChoose") == "Xbox360One")
		{
			Controller.value = 1;
		}
		else if (PlayerPrefs.GetString("ControllerTypeChoose") == "PS4")
		{
			Controller.value = 1;
		}
		else if (PlayerPrefs.GetString("ControllerTypeChoose") == "LogitechSteeringWheel")
		{
			Controller.value = 2;
		}
	}

	public void SetController()
	{
		Navigation navigation = Controller.navigation;
		if (Controller.value == 0)
		{
			RCCSettings.controllerType = RCC_Settings.ControllerType.Keyboard;
			PlayerPrefs.SetString("ControllerTypeChoose", "Keyboard");
			PlayerPrefs.SetInt("UsingPS4Controller", 0);
			navigation.selectOnRight = ResolutionDp.GetComponent<Dropdown>();
			pslogo.SetActive(value: false);
		}
		if (Controller.value == 1)
		{
			RCCSettings.controllerType = RCC_Settings.ControllerType.XBox360One;
			PlayerPrefs.SetString("ControllerTypeChoose", "Xbox360One");
			PlayerPrefs.SetInt("UsingPS4Controller", 0);
			navigation.selectOnRight = VibrationToggle.GetComponent<Toggle>();
			PS4TUTO.SetActive(value: true);
			pslogo.SetActive(value: true);
		}
		if (Controller.value == 2)
		{
			RCCSettings.controllerType = RCC_Settings.ControllerType.LogitechSteeringWheel;
			PlayerPrefs.SetString("ControllerTypeChoose", "LogitechSteeringWheel");
			PlayerPrefs.SetInt("PS4enable", 0);
			PlayerPrefs.SetInt("UsingPS4Controller", 0);
			navigation.selectOnRight = VibrationToggle.GetComponent<Toggle>();
			LogitechTuto.SetActive(value: true);
			pslogo.SetActive(value: false);
		}
		Controller.navigation = navigation;
	}

	public void SetControllerFromServerList()
	{
		if (Controller.value == 0)
		{
			PlayerPrefs.SetString("ControllerTypeChoose", "Keyboard");
		}
		if (Controller.value == 1 && showtutoint == 1)
		{
			PlayerPrefs.SetString("ControllerTypeChoose", "Xbox360One");
			PS4TUTO.SetActive(value: true);
			Debug.Log("PS4 XBOX");
			StartCoroutine(selectitem(PS4TUTO));
		}
		if (Controller.value == 2 && showtutoint == 1)
		{
			PlayerPrefs.SetString("ControllerTypeChoose", "LogitechSteeringWheel");
			LogitechTuto.SetActive(value: true);
			Debug.Log("LOGITECH");
			StartCoroutine(selectitem(LogitechTuto));
		}
	}

	private IEnumerator selectitem(GameObject jack)
	{
		yield return new WaitForSeconds(0.1f);
		jack.GetComponent<Button>().Select();
	}

	public void SetLogi()
	{
		Debug.Log("PASSAGE AVEC LOGITECH STEERING WHEEL");
	}

	public void NoVolant()
	{
		Debug.Log("MDRRRR");
	}

	public void Update()
	{
	}
}
