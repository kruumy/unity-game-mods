using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Mobile/RCC UI Mobile Buttons")]
public class RCC_MobileButtons : MonoBehaviour
{
	private RCC_Settings RCCSettingsInstance;

	public RCC_UIController gasButton;

	public RCC_UIController gradualGasButton;

	public RCC_UIController brakeButton;

	public RCC_UIController leftButton;

	public RCC_UIController rightButton;

	public RCC_UISteeringWheelController steeringWheel;

	public RCC_UIController handbrakeButton;

	public RCC_UIController NOSButton;

	public RCC_UIController NOSButtonSteeringWheel;

	public GameObject gearButton;

	public RCC_UIJoystick joystick;

	private float gasInput;

	private float brakeInput;

	private float leftInput;

	private float rightInput;

	private float steeringWheelInput;

	private float handbrakeInput;

	private float NOSInput = 1f;

	private float gyroInput;

	private float joystickInput;

	private bool canUseNos;

	private Vector3 orgBrakeButtonPos;

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

	private void Start()
	{
		if ((bool)brakeButton)
		{
			orgBrakeButtonPos = brakeButton.transform.position;
		}
		CheckController();
	}

	private void OnEnable()
	{
		RCC_SceneManager.OnMainControllerChanged += CheckController;
		RCC_SceneManager.OnVehicleChanged += CheckController;
	}

	private void CheckController()
	{
		if ((bool)RCC_SceneManager.Instance.activePlayerVehicle)
		{
			if (RCCSettings.controllerType == RCC_Settings.ControllerType.Mobile)
			{
				EnableButtons();
			}
			else
			{
				DisableButtons();
			}
		}
	}

	private void DisableButtons()
	{
		if ((bool)gasButton)
		{
			gasButton.gameObject.SetActive(value: false);
		}
		if ((bool)gradualGasButton)
		{
			gradualGasButton.gameObject.SetActive(value: false);
		}
		if ((bool)leftButton)
		{
			leftButton.gameObject.SetActive(value: false);
		}
		if ((bool)rightButton)
		{
			rightButton.gameObject.SetActive(value: false);
		}
		if ((bool)brakeButton)
		{
			brakeButton.gameObject.SetActive(value: false);
		}
		if ((bool)steeringWheel)
		{
			steeringWheel.gameObject.SetActive(value: false);
		}
		if ((bool)handbrakeButton)
		{
			handbrakeButton.gameObject.SetActive(value: false);
		}
		if ((bool)NOSButton)
		{
			NOSButton.gameObject.SetActive(value: false);
		}
		if ((bool)NOSButtonSteeringWheel)
		{
			NOSButtonSteeringWheel.gameObject.SetActive(value: false);
		}
		if ((bool)gearButton)
		{
			gearButton.gameObject.SetActive(value: false);
		}
		if ((bool)joystick)
		{
			joystick.gameObject.SetActive(value: false);
		}
	}

	private void EnableButtons()
	{
		if ((bool)gasButton)
		{
			gasButton.gameObject.SetActive(value: true);
		}
		if ((bool)leftButton)
		{
			leftButton.gameObject.SetActive(value: true);
		}
		if ((bool)rightButton)
		{
			rightButton.gameObject.SetActive(value: true);
		}
		if ((bool)brakeButton)
		{
			brakeButton.gameObject.SetActive(value: true);
		}
		if ((bool)steeringWheel)
		{
			steeringWheel.gameObject.SetActive(value: true);
		}
		if ((bool)handbrakeButton)
		{
			handbrakeButton.gameObject.SetActive(value: true);
		}
		if (canUseNos)
		{
			if ((bool)NOSButton)
			{
				NOSButton.gameObject.SetActive(value: true);
			}
			if ((bool)NOSButtonSteeringWheel)
			{
				NOSButtonSteeringWheel.gameObject.SetActive(value: true);
			}
		}
		if ((bool)joystick)
		{
			joystick.gameObject.SetActive(value: true);
		}
	}

	private void Update()
	{
		RCCSettings.controllerType = RCC_Settings.ControllerType.Custom;
		Debug.Log("CONTROLLER = " + RCCSettings.controllerType);
	}

	public void tki()
	{
		if (RCCSettings.controllerType != RCC_Settings.ControllerType.Mobile)
		{
			return;
		}
		switch (RCCSettings.mobileController)
		{
		case RCC_Settings.MobileController.TouchScreen:
			gyroInput = 0f;
			if ((bool)steeringWheel && steeringWheel.gameObject.activeInHierarchy)
			{
				steeringWheel.gameObject.SetActive(value: false);
			}
			if ((bool)NOSButton && NOSButton.gameObject.activeInHierarchy != canUseNos)
			{
				NOSButton.gameObject.SetActive(canUseNos);
			}
			if ((bool)joystick && joystick.gameObject.activeInHierarchy)
			{
				joystick.gameObject.SetActive(value: false);
			}
			if (!leftButton.gameObject.activeInHierarchy)
			{
				brakeButton.transform.position = orgBrakeButtonPos;
				leftButton.gameObject.SetActive(value: true);
			}
			if (!rightButton.gameObject.activeInHierarchy)
			{
				rightButton.gameObject.SetActive(value: true);
			}
			break;
		case RCC_Settings.MobileController.Gyro:
			gyroInput = Input.acceleration.x * RCCSettings.gyroSensitivity;
			brakeButton.transform.position = leftButton.transform.position;
			if (steeringWheel.gameObject.activeInHierarchy)
			{
				steeringWheel.gameObject.SetActive(value: false);
			}
			if ((bool)NOSButton && NOSButton.gameObject.activeInHierarchy != canUseNos)
			{
				NOSButton.gameObject.SetActive(canUseNos);
			}
			if ((bool)joystick && joystick.gameObject.activeInHierarchy)
			{
				joystick.gameObject.SetActive(value: false);
			}
			if (leftButton.gameObject.activeInHierarchy)
			{
				leftButton.gameObject.SetActive(value: false);
			}
			if (rightButton.gameObject.activeInHierarchy)
			{
				rightButton.gameObject.SetActive(value: false);
			}
			break;
		case RCC_Settings.MobileController.SteeringWheel:
			gyroInput = 0f;
			if (!steeringWheel.gameObject.activeInHierarchy)
			{
				steeringWheel.gameObject.SetActive(value: true);
				brakeButton.transform.position = orgBrakeButtonPos;
			}
			if ((bool)NOSButton && NOSButton.gameObject.activeInHierarchy)
			{
				NOSButton.gameObject.SetActive(value: false);
			}
			if ((bool)NOSButtonSteeringWheel && NOSButtonSteeringWheel.gameObject.activeInHierarchy != canUseNos)
			{
				NOSButtonSteeringWheel.gameObject.SetActive(canUseNos);
			}
			if ((bool)joystick && joystick.gameObject.activeInHierarchy)
			{
				joystick.gameObject.SetActive(value: false);
			}
			if (leftButton.gameObject.activeInHierarchy)
			{
				leftButton.gameObject.SetActive(value: false);
			}
			if (rightButton.gameObject.activeInHierarchy)
			{
				rightButton.gameObject.SetActive(value: false);
			}
			break;
		case RCC_Settings.MobileController.Joystick:
			gyroInput = 0f;
			if ((bool)steeringWheel && steeringWheel.gameObject.activeInHierarchy)
			{
				steeringWheel.gameObject.SetActive(value: false);
			}
			if ((bool)NOSButton && NOSButton.gameObject.activeInHierarchy != canUseNos)
			{
				NOSButton.gameObject.SetActive(canUseNos);
			}
			if ((bool)joystick && !joystick.gameObject.activeInHierarchy)
			{
				joystick.gameObject.SetActive(value: true);
				brakeButton.transform.position = orgBrakeButtonPos;
			}
			if (leftButton.gameObject.activeInHierarchy)
			{
				leftButton.gameObject.SetActive(value: false);
			}
			if (rightButton.gameObject.activeInHierarchy)
			{
				rightButton.gameObject.SetActive(value: false);
			}
			break;
		}
		gasInput = GetInput(gasButton) + GetInput(gradualGasButton);
		brakeInput = GetInput(brakeButton);
		leftInput = GetInput(leftButton);
		rightInput = GetInput(rightButton);
		handbrakeInput = GetInput(handbrakeButton);
		NOSInput = Mathf.Clamp(GetInput(NOSButton) + GetInput(NOSButtonSteeringWheel), 0f, 1f);
		if ((bool)steeringWheel)
		{
			steeringWheelInput = steeringWheel.input;
		}
		if ((bool)joystick)
		{
			joystickInput = joystick.inputHorizontal;
		}
		FeedRCC();
	}

	private void FeedRCC()
	{
		if ((bool)RCC_SceneManager.Instance.activePlayerVehicle)
		{
			canUseNos = RCC_SceneManager.Instance.activePlayerVehicle.useNOS;
			if (RCC_SceneManager.Instance.activePlayerVehicle.canControl && !RCC_SceneManager.Instance.activePlayerVehicle.externalController)
			{
				RCC_SceneManager.Instance.activePlayerVehicle.gasInput = gasInput;
				RCC_SceneManager.Instance.activePlayerVehicle.brakeInput = brakeInput;
				RCC_SceneManager.Instance.activePlayerVehicle.steerInput = 0f - leftInput + rightInput + steeringWheelInput + gyroInput + joystickInput;
				RCC_SceneManager.Instance.activePlayerVehicle.handbrakeInput = handbrakeInput;
				RCC_SceneManager.Instance.activePlayerVehicle.boostInput = NOSInput;
			}
		}
	}

	private float GetInput(RCC_UIController button)
	{
		if (button == null)
		{
			return 0f;
		}
		return button.input;
	}

	private void OnDisable()
	{
		RCC_SceneManager.OnMainControllerChanged -= CheckController;
		RCC_SceneManager.OnVehicleChanged -= CheckController;
	}
}
