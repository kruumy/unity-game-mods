using UnityEngine;

public class RCC_LogitechSteeringWheel : MonoBehaviour
{
	private static RCC_LogitechSteeringWheel instance;

	private LogitechGSDK.LogiControllerPropertiesData properties;

	internal LogitechGSDK.DIJOYSTATE2ENGINES rec;

	public RCC_Inputs inputs = new RCC_Inputs();

	public bool useForceFeedback = true;

	public bool useHShifter = true;

	public bool atNGear = true;

	public static RCC_LogitechSteeringWheel Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Object.FindObjectOfType<RCC_LogitechSteeringWheel>();
				if (instance == null)
				{
					instance = new GameObject("_RCCLogitechSteeringWheelManager").AddComponent<RCC_LogitechSteeringWheel>();
				}
			}
			return instance;
		}
	}

	private void Start()
	{
		LogitechGSDK.LogiSteeringInitialize(ignoreXInputControllers: false);
	}

	private void OnEnable()
	{
		RCC_CarControllerV3.OnRCCPlayerCollision += RCC_CarControllerV3_OnRCCPlayerCollision;
	}

	private void RCC_CarControllerV3_OnRCCPlayerCollision(RCC_CarControllerV3 RCC, Collision collision)
	{
		if (RCC == RCC_SceneManager.Instance.activePlayerVehicle)
		{
			LogitechGSDK.LogiPlayFrontalCollisionForce(0, Mathf.CeilToInt(collision.relativeVelocity.magnitude * 3f));
		}
	}

	private void Update()
	{
		if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
		{
			rec = LogitechGSDK.LogiGetStateUnity(0);
			if (useHShifter)
			{
				HShifter(rec);
			}
			if (useForceFeedback)
			{
				ForceFeedback();
			}
			inputs.steerInput = Mathf.Clamp((float)rec.lX / 32768f, -1f, 1f);
			inputs.throttleInput = Mathf.Clamp01((float)rec.lY / -32768f);
			inputs.brakeInput = Mathf.Clamp01(1f - Mathf.Abs((float)rec.lRz / -32768f));
			inputs.clutchInput = Mathf.Clamp01((float)rec.rglSlider[0] / -32768f);
		}
	}

	private void HShifter(LogitechGSDK.DIJOYSTATE2ENGINES shifter)
	{
		bool flag = false;
		for (int i = 0; i < 128; i++)
		{
			if (shifter.rgbButtons[i] == 128)
			{
				switch (i)
				{
				case 12:
					inputs.gearInput = 0;
					flag = true;
					break;
				case 13:
					inputs.gearInput = 1;
					flag = true;
					break;
				case 14:
					inputs.gearInput = 2;
					flag = true;
					break;
				case 15:
					inputs.gearInput = 3;
					flag = true;
					break;
				case 16:
					inputs.gearInput = 4;
					flag = true;
					break;
				case 17:
					inputs.gearInput = 5;
					flag = true;
					break;
				case 18:
					inputs.gearInput = -1;
					flag = true;
					break;
				}
			}
		}
		atNGear = !flag;
	}

	private void ForceFeedback()
	{
		RCC_CarControllerV3 activePlayerVehicle = RCC_SceneManager.Instance.activePlayerVehicle;
		if ((bool)activePlayerVehicle)
		{
			LogitechGSDK.LogiStopConstantForce(0);
			LogitechGSDK.LogiPlayConstantForce(0, (int)((0f - activePlayerVehicle.FrontLeftWheelCollider.wheelHit.sidewaysSlip) * 200f));
		}
	}

	public static bool GetKeyTriggered(int controllerIndex, int keycode)
	{
		return LogitechGSDK.LogiButtonTriggered(controllerIndex, keycode);
	}

	public static bool GetKeyPressed(int controllerIndex, int keycode)
	{
		return LogitechGSDK.LogiButtonIsPressed(controllerIndex, keycode);
	}

	public static bool GetKeyReleased(int controllerIndex, int keycode)
	{
		return LogitechGSDK.LogiButtonReleased(controllerIndex, keycode);
	}

	private void OnDisable()
	{
		RCC_CarControllerV3.OnRCCPlayerCollision -= RCC_CarControllerV3_OnRCCPlayerCollision;
	}
}
