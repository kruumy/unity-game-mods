using UnityEngine;

public class RCC_InputManager : MonoBehaviour
{
	private enum InputState
	{
		None,
		Pressed,
		Held,
		Released
	}

	private RCC_Settings RCCSettingsInstance;

	private static RCC_Inputs inputs = new RCC_Inputs();

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

	public static RCC_Inputs GetInputs()
	{
		switch (RCC_Settings.Instance.controllerType)
		{
		case RCC_Settings.ControllerType.Keyboard:
			inputs.throttleInput = Mathf.Clamp01(Input.GetAxis(RCC_Settings.Instance.verticalInput));
			inputs.brakeInput = Mathf.Abs(Mathf.Clamp(Input.GetAxis(RCC_Settings.Instance.verticalInput), -1f, 0f));
			inputs.steerInput = Mathf.Clamp(Input.GetAxis(RCC_Settings.Instance.horizontalInput), -1f, 1f);
			inputs.handbrakeInput = Mathf.Clamp01(Input.GetKey(RCC_Settings.Instance.handbrakeKB) ? 1f : 0f);
			inputs.boostInput = Mathf.Clamp01(Input.GetKey(RCC_Settings.Instance.boostKB) ? 1f : 0f);
			break;
		case RCC_Settings.ControllerType.XBox360One:
			if (!string.IsNullOrEmpty(RCC_Settings.Instance.Xbox_triggerRightInput))
			{
				inputs.throttleInput = Input.GetAxis(RCC_Settings.Instance.Xbox_triggerRightInput);
			}
			if (!string.IsNullOrEmpty(RCC_Settings.Instance.Xbox_triggerLeftInput))
			{
				inputs.brakeInput = Input.GetAxis(RCC_Settings.Instance.Xbox_triggerLeftInput);
			}
			if (!string.IsNullOrEmpty(RCC_Settings.Instance.Xbox_horizontalInput))
			{
				inputs.steerInput = Input.GetAxis(RCC_Settings.Instance.Xbox_horizontalInput);
			}
			if (!string.IsNullOrEmpty(RCC_Settings.Instance.Xbox_handbrakeKB))
			{
				inputs.handbrakeInput = (Input.GetButton(RCC_Settings.Instance.Xbox_handbrakeKB) ? 1f : 0f);
			}
			if (!string.IsNullOrEmpty(RCC_Settings.Instance.Xbox_boostKB))
			{
				inputs.boostInput = (Input.GetButton(RCC_Settings.Instance.Xbox_boostKB) ? 1f : 0f);
			}
			break;
		case RCC_Settings.ControllerType.PS4:
			if (!string.IsNullOrEmpty(RCC_Settings.Instance.PS4_triggerRightInput))
			{
				inputs.throttleInput = Mathf.Clamp01(Input.GetAxis(RCC_Settings.Instance.PS4_triggerRightInput));
			}
			if (!string.IsNullOrEmpty(RCC_Settings.Instance.PS4_triggerLeftInput))
			{
				inputs.brakeInput = Input.GetAxis(RCC_Settings.Instance.PS4_triggerLeftInput);
			}
			if (!string.IsNullOrEmpty(RCC_Settings.Instance.PS4_horizontalInput))
			{
				inputs.steerInput = Input.GetAxis(RCC_Settings.Instance.PS4_horizontalInput);
			}
			if (!string.IsNullOrEmpty(RCC_Settings.Instance.PS4_handbrakeKB))
			{
				inputs.handbrakeInput = (Input.GetButton(RCC_Settings.Instance.PS4_handbrakeKB) ? 1f : 0f);
			}
			if (!string.IsNullOrEmpty(RCC_Settings.Instance.PS4_boostKB))
			{
				inputs.boostInput = (Input.GetButton(RCC_Settings.Instance.PS4_boostKB) ? 1f : 0f);
			}
			break;
		case RCC_Settings.ControllerType.LogitechSteeringWheel:
		{
			RCC_LogitechSteeringWheel instance = RCC_LogitechSteeringWheel.Instance;
			if ((bool)instance)
			{
				inputs.throttleInput = instance.inputs.throttleInput;
				inputs.brakeInput = instance.inputs.brakeInput;
				inputs.steerInput = instance.inputs.steerInput;
				inputs.clutchInput = instance.inputs.clutchInput;
				inputs.handbrakeInput = instance.inputs.handbrakeInput;
			}
			break;
		}
		}
		return inputs;
	}

	public static bool GetKeyDown(KeyCode keyCode)
	{
		if (Input.GetKeyDown(keyCode))
		{
			return true;
		}
		return false;
	}

	public static bool GetKeyUp(KeyCode keyCode)
	{
		if (Input.GetKeyUp(keyCode))
		{
			return true;
		}
		return false;
	}

	public static bool GetKey(KeyCode keyCode)
	{
		if (Input.GetKey(keyCode))
		{
			return true;
		}
		return false;
	}

	public static bool GetButtonDown(string buttonCode)
	{
		if (Input.GetButtonDown(buttonCode))
		{
			return true;
		}
		return false;
	}

	public static bool GetButtonUp(string buttonCode)
	{
		if (Input.GetButtonUp(buttonCode))
		{
			return true;
		}
		return false;
	}

	public static bool GetButton(string buttonCode)
	{
		if (Input.GetButton(buttonCode))
		{
			return true;
		}
		return false;
	}
}
