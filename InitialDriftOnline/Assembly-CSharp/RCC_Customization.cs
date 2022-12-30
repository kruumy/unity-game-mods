using CodeStage.AntiCheat.Storage;
using UnityEngine;

public class RCC_Customization : MonoBehaviour
{
	public static void SetCustomizationMode(RCC_CarControllerV3 vehicle, bool state)
	{
		if (!vehicle)
		{
			Debug.LogError("Player vehicle is not selected for customization! Use RCC_Customization.SetCustomizationMode(playerVehicle, true/false); for enabling / disabling customization mode for player vehicle.");
			return;
		}
		RCC_Camera activePlayerCamera = RCC_SceneManager.Instance.activePlayerCamera;
		RCC_UIDashboardDisplay activePlayerCanvas = RCC_SceneManager.Instance.activePlayerCanvas;
		if (state)
		{
			vehicle.SetCanControl(state: false);
			if ((bool)activePlayerCamera)
			{
				activePlayerCamera.ChangeCamera(RCC_Camera.CameraMode.TPS);
			}
			if ((bool)activePlayerCanvas)
			{
				activePlayerCanvas.SetDisplayType(RCC_UIDashboardDisplay.DisplayType.Customization);
			}
			return;
		}
		SetSmokeParticle(vehicle, state: false);
		SetExhaustFlame(vehicle, state: false);
		vehicle.SetCanControl(state: true);
		if ((bool)activePlayerCamera)
		{
			activePlayerCamera.ChangeCamera(RCC_Camera.CameraMode.TPS);
		}
		if ((bool)activePlayerCanvas)
		{
			activePlayerCanvas.SetDisplayType(RCC_UIDashboardDisplay.DisplayType.Full);
		}
	}

	public static void OverrideRCC(RCC_CarControllerV3 vehicle)
	{
		if (CheckVehicle(vehicle))
		{
			vehicle.isSleeping = false;
		}
	}

	public static void SetSmokeParticle(RCC_CarControllerV3 vehicle, bool state)
	{
		if (CheckVehicle(vehicle))
		{
			vehicle.PreviewSmokeParticle(state);
		}
	}

	public static void SetSmokeColor(RCC_CarControllerV3 vehicle, int indexOfGroundMaterial, Color color)
	{
		if (!CheckVehicle(vehicle))
		{
			return;
		}
		RCC_WheelCollider[] componentsInChildren = vehicle.GetComponentsInChildren<RCC_WheelCollider>();
		foreach (RCC_WheelCollider rCC_WheelCollider in componentsInChildren)
		{
			for (int j = 0; j < rCC_WheelCollider.allWheelParticles.Count; j++)
			{
				ParticleSystem.MainModule main = rCC_WheelCollider.allWheelParticles[j].main;
				color.a = main.startColor.color.a;
				main.startColor = color;
			}
		}
	}

	public static void SetHeadlightsColor(RCC_CarControllerV3 vehicle, Color color)
	{
		if (!CheckVehicle(vehicle))
		{
			return;
		}
		RCC_Light[] componentsInChildren = vehicle.GetComponentsInChildren<RCC_Light>();
		foreach (RCC_Light rCC_Light in componentsInChildren)
		{
			if (rCC_Light.lightType == RCC_Light.LightType.HeadLight || rCC_Light.lightType == RCC_Light.LightType.HighBeamHeadLight)
			{
				rCC_Light.GetComponent<Light>().color = color;
			}
		}
	}

	public static void SetExhaustFlame(RCC_CarControllerV3 vehicle, bool state)
	{
		if (CheckVehicle(vehicle))
		{
			RCC_Exhaust[] componentsInChildren = vehicle.GetComponentsInChildren<RCC_Exhaust>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].previewFlames = state;
			}
		}
	}

	public static void SetFrontCambers(RCC_CarControllerV3 vehicle, float camberAngle)
	{
		if (!CheckVehicle(vehicle))
		{
			return;
		}
		RCC_WheelCollider[] componentsInChildren = vehicle.GetComponentsInChildren<RCC_WheelCollider>();
		foreach (RCC_WheelCollider rCC_WheelCollider in componentsInChildren)
		{
			if (rCC_WheelCollider == vehicle.FrontLeftWheelCollider || rCC_WheelCollider == vehicle.FrontRightWheelCollider)
			{
				rCC_WheelCollider.camber = camberAngle;
			}
		}
		OverrideRCC(vehicle);
	}

	public static void SetRearCambers(RCC_CarControllerV3 vehicle, float camberAngle)
	{
		if (!CheckVehicle(vehicle))
		{
			return;
		}
		RCC_WheelCollider[] componentsInChildren = vehicle.GetComponentsInChildren<RCC_WheelCollider>();
		foreach (RCC_WheelCollider rCC_WheelCollider in componentsInChildren)
		{
			if (rCC_WheelCollider != vehicle.FrontLeftWheelCollider && rCC_WheelCollider != vehicle.FrontRightWheelCollider)
			{
				rCC_WheelCollider.camber = camberAngle;
			}
		}
		OverrideRCC(vehicle);
	}

	public static void ChangeWheels(RCC_CarControllerV3 vehicle, GameObject wheel)
	{
		if (!CheckVehicle(vehicle))
		{
			return;
		}
		for (int i = 0; i < vehicle.allWheelColliders.Length; i++)
		{
			if ((bool)vehicle.allWheelColliders[i].wheelModel.GetComponent<MeshRenderer>())
			{
				vehicle.allWheelColliders[i].wheelModel.GetComponent<MeshRenderer>().enabled = false;
			}
			foreach (Transform componentInChild in vehicle.allWheelColliders[i].wheelModel.GetComponentInChildren<Transform>())
			{
				componentInChild.gameObject.SetActive(value: false);
			}
			GameObject gameObject = Object.Instantiate(wheel, vehicle.allWheelColliders[i].wheelModel.position, vehicle.allWheelColliders[i].wheelModel.rotation, vehicle.allWheelColliders[i].wheelModel);
			if (vehicle.allWheelColliders[i].wheelModel.localPosition.x > 0f)
			{
				gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * -1f, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
			}
		}
		OverrideRCC(vehicle);
	}

	public static void SetFrontSuspensionsTargetPos(RCC_CarControllerV3 vehicle, float targetPosition)
	{
		if (CheckVehicle(vehicle))
		{
			targetPosition = Mathf.Clamp01(targetPosition);
			JointSpring suspensionSpring = vehicle.FrontLeftWheelCollider.wheelCollider.suspensionSpring;
			suspensionSpring.targetPosition = 1f - targetPosition;
			vehicle.FrontLeftWheelCollider.wheelCollider.suspensionSpring = suspensionSpring;
			JointSpring suspensionSpring2 = vehicle.FrontRightWheelCollider.wheelCollider.suspensionSpring;
			suspensionSpring2.targetPosition = 1f - targetPosition;
			vehicle.FrontRightWheelCollider.wheelCollider.suspensionSpring = suspensionSpring2;
			OverrideRCC(vehicle);
		}
	}

	public static void SetRearSuspensionsTargetPos(RCC_CarControllerV3 vehicle, float targetPosition)
	{
		if (CheckVehicle(vehicle))
		{
			targetPosition = Mathf.Clamp01(targetPosition);
			JointSpring suspensionSpring = vehicle.RearLeftWheelCollider.wheelCollider.suspensionSpring;
			suspensionSpring.targetPosition = 1f - targetPosition;
			vehicle.RearLeftWheelCollider.wheelCollider.suspensionSpring = suspensionSpring;
			JointSpring suspensionSpring2 = vehicle.RearRightWheelCollider.wheelCollider.suspensionSpring;
			suspensionSpring2.targetPosition = 1f - targetPosition;
			vehicle.RearRightWheelCollider.wheelCollider.suspensionSpring = suspensionSpring2;
			OverrideRCC(vehicle);
		}
	}

	public static void SetAllSuspensionsTargetPos(RCC_CarControllerV3 vehicle, float targetPosition)
	{
		if (CheckVehicle(vehicle))
		{
			targetPosition = Mathf.Clamp01(targetPosition);
			JointSpring suspensionSpring = vehicle.RearLeftWheelCollider.wheelCollider.suspensionSpring;
			suspensionSpring.targetPosition = 1f - targetPosition;
			vehicle.RearLeftWheelCollider.wheelCollider.suspensionSpring = suspensionSpring;
			JointSpring suspensionSpring2 = vehicle.RearRightWheelCollider.wheelCollider.suspensionSpring;
			suspensionSpring2.targetPosition = 1f - targetPosition;
			vehicle.RearRightWheelCollider.wheelCollider.suspensionSpring = suspensionSpring2;
			JointSpring suspensionSpring3 = vehicle.FrontLeftWheelCollider.wheelCollider.suspensionSpring;
			suspensionSpring3.targetPosition = 1f - targetPosition;
			vehicle.FrontLeftWheelCollider.wheelCollider.suspensionSpring = suspensionSpring3;
			JointSpring suspensionSpring4 = vehicle.FrontRightWheelCollider.wheelCollider.suspensionSpring;
			suspensionSpring4.targetPosition = 1f - targetPosition;
			vehicle.FrontRightWheelCollider.wheelCollider.suspensionSpring = suspensionSpring4;
			OverrideRCC(vehicle);
		}
	}

	public static void SetFrontSuspensionsDistances(RCC_CarControllerV3 vehicle, float distance)
	{
		if (CheckVehicle(vehicle))
		{
			if (distance <= 0f)
			{
				distance = 0.05f;
			}
			vehicle.FrontLeftWheelCollider.wheelCollider.suspensionDistance = distance;
			vehicle.FrontRightWheelCollider.wheelCollider.suspensionDistance = distance;
			Debug.Log(" ");
			OverrideRCC(vehicle);
		}
	}

	public static void SetRearSuspensionsDistances(RCC_CarControllerV3 vehicle, float distance)
	{
		if (!CheckVehicle(vehicle))
		{
			return;
		}
		if (distance <= 0f)
		{
			distance = 0.05f;
		}
		vehicle.RearLeftWheelCollider.wheelCollider.suspensionDistance = distance;
		vehicle.RearRightWheelCollider.wheelCollider.suspensionDistance = distance;
		if (vehicle.ExtraRearWheelsCollider != null && vehicle.ExtraRearWheelsCollider.Length != 0)
		{
			RCC_WheelCollider[] extraRearWheelsCollider = vehicle.ExtraRearWheelsCollider;
			for (int i = 0; i < extraRearWheelsCollider.Length; i++)
			{
				extraRearWheelsCollider[i].wheelCollider.suspensionDistance = distance;
			}
		}
		OverrideRCC(vehicle);
	}

	public static void SetDrivetrainMode(RCC_CarControllerV3 vehicle, RCC_CarControllerV3.WheelType mode)
	{
		if (CheckVehicle(vehicle))
		{
			vehicle.wheelTypeChoise = mode;
			OverrideRCC(vehicle);
		}
	}

	public static void SetGearShiftingThreshold(RCC_CarControllerV3 vehicle, float targetValue)
	{
		if (CheckVehicle(vehicle))
		{
			vehicle.gearShiftingThreshold = targetValue;
			OverrideRCC(vehicle);
		}
	}

	public static void SetClutchThreshold(RCC_CarControllerV3 vehicle, float targetValue)
	{
		if (CheckVehicle(vehicle))
		{
			vehicle.clutchInertia = targetValue;
			OverrideRCC(vehicle);
		}
	}

	public static void SetCounterSteering(RCC_CarControllerV3 vehicle, bool state)
	{
		if (CheckVehicle(vehicle))
		{
			vehicle.applyCounterSteering = state;
			OverrideRCC(vehicle);
		}
	}

	public static void SetNOS(RCC_CarControllerV3 vehicle, bool state)
	{
		if (CheckVehicle(vehicle))
		{
			vehicle.useNOS = state;
			OverrideRCC(vehicle);
		}
	}

	public static void SetTurbo(RCC_CarControllerV3 vehicle, bool state)
	{
		if (CheckVehicle(vehicle))
		{
			vehicle.useTurbo = state;
			OverrideRCC(vehicle);
		}
	}

	public static void SetUseExhaustFlame(RCC_CarControllerV3 vehicle, bool state)
	{
		if (CheckVehicle(vehicle))
		{
			vehicle.useExhaustFlame = state;
			OverrideRCC(vehicle);
		}
	}

	public static void SetRevLimiter(RCC_CarControllerV3 vehicle, bool state)
	{
		if (CheckVehicle(vehicle))
		{
			vehicle.useRevLimiter = state;
			OverrideRCC(vehicle);
		}
	}

	public static void SetClutchMargin(RCC_CarControllerV3 vehicle, bool state)
	{
		if (CheckVehicle(vehicle))
		{
			vehicle.useClutchMarginAtFirstGear = state;
			OverrideRCC(vehicle);
		}
	}

	public static void SetFrontSuspensionsSpringForce(RCC_CarControllerV3 vehicle, float targetValue)
	{
		if (CheckVehicle(vehicle))
		{
			JointSpring suspensionSpring = vehicle.FrontLeftWheelCollider.GetComponent<WheelCollider>().suspensionSpring;
			suspensionSpring.spring = targetValue;
			vehicle.FrontLeftWheelCollider.GetComponent<WheelCollider>().suspensionSpring = suspensionSpring;
			vehicle.FrontRightWheelCollider.GetComponent<WheelCollider>().suspensionSpring = suspensionSpring;
			OverrideRCC(vehicle);
		}
	}

	public static void SetRearSuspensionsSpringForce(RCC_CarControllerV3 vehicle, float targetValue)
	{
		if (CheckVehicle(vehicle))
		{
			JointSpring suspensionSpring = vehicle.RearLeftWheelCollider.GetComponent<WheelCollider>().suspensionSpring;
			suspensionSpring.spring = targetValue;
			vehicle.RearLeftWheelCollider.GetComponent<WheelCollider>().suspensionSpring = suspensionSpring;
			vehicle.RearRightWheelCollider.GetComponent<WheelCollider>().suspensionSpring = suspensionSpring;
			OverrideRCC(vehicle);
		}
	}

	public static void SetFrontSuspensionsSpringDamper(RCC_CarControllerV3 vehicle, float targetValue)
	{
		if (CheckVehicle(vehicle))
		{
			JointSpring suspensionSpring = vehicle.FrontLeftWheelCollider.GetComponent<WheelCollider>().suspensionSpring;
			suspensionSpring.damper = targetValue;
			vehicle.FrontLeftWheelCollider.GetComponent<WheelCollider>().suspensionSpring = suspensionSpring;
			vehicle.FrontRightWheelCollider.GetComponent<WheelCollider>().suspensionSpring = suspensionSpring;
			OverrideRCC(vehicle);
		}
	}

	public static void SetRearSuspensionsSpringDamper(RCC_CarControllerV3 vehicle, float targetValue)
	{
		if (CheckVehicle(vehicle))
		{
			JointSpring suspensionSpring = vehicle.RearLeftWheelCollider.GetComponent<WheelCollider>().suspensionSpring;
			suspensionSpring.damper = targetValue;
			vehicle.RearLeftWheelCollider.GetComponent<WheelCollider>().suspensionSpring = suspensionSpring;
			vehicle.RearRightWheelCollider.GetComponent<WheelCollider>().suspensionSpring = suspensionSpring;
			OverrideRCC(vehicle);
		}
	}

	public static void SetMaximumSpeed(RCC_CarControllerV3 vehicle, float targetValue)
	{
		if (CheckVehicle(vehicle))
		{
			vehicle.maxspeed = Mathf.Clamp(targetValue, 10f, 320f);
			OverrideRCC(vehicle);
		}
	}

	public static void SetMaximumTorque(RCC_CarControllerV3 vehicle, float targetValue)
	{
		if (CheckVehicle(vehicle))
		{
			vehicle.engineTorque = Mathf.Clamp(targetValue, 50f, 50000f);
			OverrideRCC(vehicle);
		}
	}

	public static void SetMaximumBrake(RCC_CarControllerV3 vehicle, float targetValue)
	{
		if (CheckVehicle(vehicle))
		{
			vehicle.brakeTorque = Mathf.Clamp(targetValue, 0f, 100000f);
			OverrideRCC(vehicle);
		}
	}

	public static void Repair(RCC_CarControllerV3 vehicle)
	{
		if (CheckVehicle(vehicle))
		{
			vehicle.repairNow = true;
		}
	}

	public static void SetESP(RCC_CarControllerV3 vehicle, bool state)
	{
		if (CheckVehicle(vehicle))
		{
			vehicle.ESP = state;
		}
	}

	public static void SetABS(RCC_CarControllerV3 vehicle, bool state)
	{
		if (CheckVehicle(vehicle))
		{
			vehicle.ABS = state;
		}
	}

	public static void SetTCS(RCC_CarControllerV3 vehicle, bool state)
	{
		if (CheckVehicle(vehicle))
		{
			vehicle.TCS = state;
		}
	}

	public static void SetSH(RCC_CarControllerV3 vehicle, bool state)
	{
		if (CheckVehicle(vehicle))
		{
			vehicle.steeringHelper = state;
		}
	}

	public static void SetSHStrength(RCC_CarControllerV3 vehicle, float value)
	{
		if (CheckVehicle(vehicle))
		{
			vehicle.steeringHelper = true;
			vehicle.steerHelperLinearVelStrength = value;
			vehicle.steerHelperAngularVelStrength = value;
		}
	}

	public static void SetTransmission(bool automatic)
	{
		RCC_Settings.Instance.useAutomaticGear = automatic;
	}

	public static void SaveStats(RCC_CarControllerV3 vehicle)
	{
		if (!CheckVehicle(vehicle))
		{
			return;
		}
		string text = vehicle.transform.name.Split(')')[0];
		PlayerPrefs.SetFloat(text + "_FrontCamber", vehicle.FrontLeftWheelCollider.camber);
		PlayerPrefs.SetFloat(text + "_RearCamber", vehicle.RearLeftWheelCollider.camber);
		PlayerPrefs.SetFloat(text + "_FrontSuspensionsDistance", vehicle.FrontLeftWheelCollider.wheelCollider.suspensionDistance);
		PlayerPrefs.SetFloat(text + "_RearSuspensionsDistance", vehicle.RearLeftWheelCollider.wheelCollider.suspensionDistance);
		PlayerPrefs.SetFloat(text + "_FrontSuspensionsSpring", vehicle.FrontLeftWheelCollider.wheelCollider.suspensionSpring.spring);
		PlayerPrefs.SetFloat(text + "_RearSuspensionsSpring", vehicle.RearLeftWheelCollider.wheelCollider.suspensionSpring.spring);
		PlayerPrefs.SetFloat(text + "_FrontSuspensionsDamper", vehicle.FrontLeftWheelCollider.wheelCollider.suspensionSpring.damper);
		PlayerPrefs.SetFloat(text + "_RearSuspensionsDamper", vehicle.RearLeftWheelCollider.wheelCollider.suspensionSpring.damper);
		ObscuredPrefs.SetFloat(text + "_MaximumSpeed", vehicle.maxspeed);
		ObscuredPrefs.SetFloat(text + "_MaximumBrake", vehicle.brakeTorque);
		ObscuredPrefs.SetFloat(text + "_MaximumTorque", vehicle.engineTorque);
		ObscuredPrefs.SetString(text + "_DrivetrainMode", vehicle.wheelTypeChoise.ToString());
		ObscuredPrefs.SetFloat(text + "_GearShiftingThreshold", vehicle.gearShiftingThreshold);
		ObscuredPrefs.SetFloat(text + "_ClutchingThreshold", vehicle.clutchInertia);
		RCC_PlayerPrefsX.SetBool(text + "_CounterSteering", vehicle.applyCounterSteering);
		RCC_Light[] componentsInChildren = vehicle.GetComponentsInChildren<RCC_Light>();
		foreach (RCC_Light rCC_Light in componentsInChildren)
		{
			if (rCC_Light.lightType == RCC_Light.LightType.HeadLight)
			{
				RCC_PlayerPrefsX.SetColor(text + "_HeadlightsColor", rCC_Light.GetComponentInChildren<Light>().color);
				break;
			}
		}
		RCC_PlayerPrefsX.SetColor(color: vehicle.RearLeftWheelCollider.allWheelParticles[0].main.startColor.color, key: text + "_WheelsSmokeColor");
		RCC_PlayerPrefsX.SetBool(text + "_ABS", vehicle.ABS);
		RCC_PlayerPrefsX.SetBool(text + "_ESP", vehicle.ESP);
		RCC_PlayerPrefsX.SetBool(text + "_TCS", vehicle.TCS);
		RCC_PlayerPrefsX.SetBool(text + "_SH", vehicle.steeringHelper);
		RCC_PlayerPrefsX.SetBool(text + "NOS", vehicle.useNOS);
		RCC_PlayerPrefsX.SetBool(text + "Turbo", vehicle.useTurbo);
		RCC_PlayerPrefsX.SetBool(text + "ExhaustFlame", vehicle.useExhaustFlame);
		RCC_PlayerPrefsX.SetBool(text + "RevLimiter", vehicle.useRevLimiter);
		RCC_PlayerPrefsX.SetBool(text + "ClutchMargin", vehicle.useClutchMarginAtFirstGear);
	}

	public static void LoadStats(RCC_CarControllerV3 vehicle)
	{
		if (CheckVehicle(vehicle))
		{
			string text = vehicle.transform.name.Split(')')[0];
			SetFrontCambers(vehicle, PlayerPrefs.GetFloat(text + "_FrontCamber", vehicle.FrontLeftWheelCollider.camber));
			SetRearCambers(vehicle, PlayerPrefs.GetFloat(text + "_RearCamber", vehicle.RearLeftWheelCollider.camber));
			SetFrontSuspensionsDistances(vehicle, PlayerPrefs.GetFloat(text + "_FrontSuspensionsDistance", vehicle.FrontLeftWheelCollider.wheelCollider.suspensionDistance));
			SetRearSuspensionsDistances(vehicle, PlayerPrefs.GetFloat(text + "_RearSuspensionsDistance", vehicle.RearLeftWheelCollider.wheelCollider.suspensionDistance));
			SetFrontSuspensionsSpringForce(vehicle, PlayerPrefs.GetFloat(text + "_FrontSuspensionsSpring", vehicle.FrontLeftWheelCollider.wheelCollider.suspensionSpring.spring));
			SetRearSuspensionsSpringForce(vehicle, PlayerPrefs.GetFloat(text + "_RearSuspensionsSpring", vehicle.RearLeftWheelCollider.wheelCollider.suspensionSpring.spring));
			SetFrontSuspensionsSpringDamper(vehicle, PlayerPrefs.GetFloat(text + "_FrontSuspensionsDamper", vehicle.FrontLeftWheelCollider.wheelCollider.suspensionSpring.damper));
			SetRearSuspensionsSpringDamper(vehicle, PlayerPrefs.GetFloat(text + "_RearSuspensionsDamper", vehicle.RearLeftWheelCollider.wheelCollider.suspensionSpring.damper));
			SetMaximumSpeed(vehicle, ObscuredPrefs.GetFloat(text + "_MaximumSpeed", vehicle.maxspeed));
			SetMaximumBrake(vehicle, ObscuredPrefs.GetFloat(text + "_MaximumBrake", vehicle.brakeTorque));
			SetMaximumTorque(vehicle, ObscuredPrefs.GetFloat(text + "_MaximumTorque", vehicle.engineTorque));
			switch (ObscuredPrefs.GetString(text + "_DrivetrainMode", vehicle.wheelTypeChoise.ToString()))
			{
			case "FWD":
				vehicle.wheelTypeChoise = RCC_CarControllerV3.WheelType.FWD;
				break;
			case "RWD":
				vehicle.wheelTypeChoise = RCC_CarControllerV3.WheelType.RWD;
				break;
			case "AWD":
				vehicle.wheelTypeChoise = RCC_CarControllerV3.WheelType.AWD;
				break;
			}
			SetGearShiftingThreshold(vehicle, ObscuredPrefs.GetFloat(text + "_GearShiftingThreshold", vehicle.gearShiftingThreshold));
			SetClutchThreshold(vehicle, ObscuredPrefs.GetFloat(text + "_ClutchingThreshold", vehicle.clutchInertia));
			SetCounterSteering(vehicle, RCC_PlayerPrefsX.GetBool(text + "_CounterSteering", vehicle.applyCounterSteering));
			SetABS(vehicle, RCC_PlayerPrefsX.GetBool(text + "_ABS", vehicle.ABS));
			SetESP(vehicle, RCC_PlayerPrefsX.GetBool(text + "_ESP", vehicle.ESP));
			SetTCS(vehicle, RCC_PlayerPrefsX.GetBool(text + "_TCS", vehicle.TCS));
			SetSH(vehicle, RCC_PlayerPrefsX.GetBool(text + "_SH", vehicle.steeringHelper));
			SetNOS(vehicle, RCC_PlayerPrefsX.GetBool(text + "NOS", vehicle.useNOS));
			SetTurbo(vehicle, RCC_PlayerPrefsX.GetBool(text + "Turbo", vehicle.useTurbo));
			SetUseExhaustFlame(vehicle, RCC_PlayerPrefsX.GetBool(text + "ExhaustFlame", vehicle.useExhaustFlame));
			SetRevLimiter(vehicle, RCC_PlayerPrefsX.GetBool(text + "RevLimiter", vehicle.useRevLimiter));
			SetClutchMargin(vehicle, RCC_PlayerPrefsX.GetBool(text + "ClutchMargin", vehicle.useClutchMarginAtFirstGear));
			if (PlayerPrefs.HasKey(text + "_WheelsSmokeColor"))
			{
				SetSmokeColor(vehicle, 0, RCC_PlayerPrefsX.GetColor(text + "_WheelsSmokeColor"));
			}
			if (PlayerPrefs.HasKey(text + "_HeadlightsColor"))
			{
				SetHeadlightsColor(vehicle, RCC_PlayerPrefsX.GetColor(text + "_HeadlightsColor"));
			}
			OverrideRCC(vehicle);
		}
	}

	public static void SaveStatsTemp(RCC_CarControllerV3 vehicle)
	{
		if (!CheckVehicle(vehicle))
		{
			return;
		}
		string text = vehicle.transform.name.Split(')')[0];
		PlayerPrefs.SetFloat(text + "_FrontCamberTemp", vehicle.FrontLeftWheelCollider.camber);
		PlayerPrefs.SetFloat(text + "_RearCamberTemp", vehicle.RearLeftWheelCollider.camber);
		PlayerPrefs.SetFloat(text + "_FrontSuspensionsDistanceTemp", vehicle.FrontLeftWheelCollider.wheelCollider.suspensionDistance);
		PlayerPrefs.SetFloat(text + "_RearSuspensionsDistanceTemp", vehicle.RearLeftWheelCollider.wheelCollider.suspensionDistance);
		Debug.Log("FRONT SUS SAVE: " + PlayerPrefs.GetFloat(text + "_FrontSuspensionsDistanceTemp"));
		PlayerPrefs.SetFloat(text + "_FrontSuspensionsSpringTemp", vehicle.FrontLeftWheelCollider.wheelCollider.suspensionSpring.spring);
		PlayerPrefs.SetFloat(text + "_RearSuspensionsSpringTemp", vehicle.RearLeftWheelCollider.wheelCollider.suspensionSpring.spring);
		PlayerPrefs.SetFloat(text + "_FrontSuspensionsDamperTemp", vehicle.FrontLeftWheelCollider.wheelCollider.suspensionSpring.damper);
		PlayerPrefs.SetFloat(text + "_RearSuspensionsDamperTemp", vehicle.RearLeftWheelCollider.wheelCollider.suspensionSpring.damper);
		ObscuredPrefs.SetFloat(text + "_MaximumSpeedTemp", vehicle.maxspeed);
		ObscuredPrefs.SetFloat(text + "_MaximumBrakeTemp", vehicle.brakeTorque);
		ObscuredPrefs.SetFloat(text + "_MaximumTorqueTemp", vehicle.engineTorque);
		ObscuredPrefs.SetString(text + "_DrivetrainModeTemp", vehicle.wheelTypeChoise.ToString());
		ObscuredPrefs.SetFloat(text + "_GearShiftingThresholdTemp", vehicle.gearShiftingThreshold);
		ObscuredPrefs.SetFloat(text + "_ClutchingThresholdTemp", vehicle.clutchInertia);
		RCC_PlayerPrefsX.SetBool(text + "_CounterSteeringTemp", vehicle.applyCounterSteering);
		RCC_Light[] componentsInChildren = vehicle.GetComponentsInChildren<RCC_Light>();
		foreach (RCC_Light rCC_Light in componentsInChildren)
		{
			if (rCC_Light.lightType == RCC_Light.LightType.HeadLight)
			{
				RCC_PlayerPrefsX.SetColor(text + "_HeadlightsColorTemp", rCC_Light.GetComponentInChildren<Light>().color);
				break;
			}
		}
		RCC_PlayerPrefsX.SetBool(text + "_ABSTemp", vehicle.ABS);
		RCC_PlayerPrefsX.SetBool(text + "_ESPTemp", vehicle.ESP);
		RCC_PlayerPrefsX.SetBool(text + "_TCSTemp", vehicle.TCS);
		RCC_PlayerPrefsX.SetBool(text + "_SHTemp", vehicle.steeringHelper);
		RCC_PlayerPrefsX.SetBool(text + "NOSTemp", vehicle.useNOS);
		RCC_PlayerPrefsX.SetBool(text + "TurboTemp", vehicle.useTurbo);
		RCC_PlayerPrefsX.SetBool(text + "ExhaustFlameTemp", vehicle.useExhaustFlame);
		RCC_PlayerPrefsX.SetBool(text + "RevLimiterTemp", vehicle.useRevLimiter);
		RCC_PlayerPrefsX.SetBool(text + "ClutchMarginTemp", vehicle.useClutchMarginAtFirstGear);
	}

	public static void LoadStatsTemp(RCC_CarControllerV3 vehicle)
	{
		if (CheckVehicle(vehicle))
		{
			string text = vehicle.transform.name.Split(')')[0];
			SetFrontCambers(vehicle, PlayerPrefs.GetFloat(text + "_FrontCamberTemp", vehicle.FrontLeftWheelCollider.camber));
			SetRearCambers(vehicle, PlayerPrefs.GetFloat(text + "_RearCamberTemp", vehicle.RearLeftWheelCollider.camber));
			SetFrontSuspensionsDistances(vehicle, PlayerPrefs.GetFloat(text + "_FrontSuspensionsDistanceTemp", vehicle.FrontLeftWheelCollider.wheelCollider.suspensionDistance));
			SetRearSuspensionsDistances(vehicle, PlayerPrefs.GetFloat(text + "_RearSuspensionsDistanceTemp", vehicle.RearLeftWheelCollider.wheelCollider.suspensionDistance));
			Debug.Log(" ");
			SetFrontSuspensionsSpringForce(vehicle, PlayerPrefs.GetFloat(text + "_FrontSuspensionsSpringTemp", vehicle.FrontLeftWheelCollider.wheelCollider.suspensionSpring.spring));
			SetRearSuspensionsSpringForce(vehicle, PlayerPrefs.GetFloat(text + "_RearSuspensionsSpringTemp", vehicle.RearLeftWheelCollider.wheelCollider.suspensionSpring.spring));
			SetFrontSuspensionsSpringDamper(vehicle, PlayerPrefs.GetFloat(text + "_FrontSuspensionsDamperTemp", vehicle.FrontLeftWheelCollider.wheelCollider.suspensionSpring.damper));
			SetRearSuspensionsSpringDamper(vehicle, PlayerPrefs.GetFloat(text + "_RearSuspensionsDamperTemp", vehicle.RearLeftWheelCollider.wheelCollider.suspensionSpring.damper));
			SetMaximumSpeed(vehicle, ObscuredPrefs.GetFloat(text + "_MaximumSpeedTemp", vehicle.maxspeed));
			SetMaximumBrake(vehicle, ObscuredPrefs.GetFloat(text + "_MaximumBrakeTemp", vehicle.brakeTorque));
			SetMaximumTorque(vehicle, ObscuredPrefs.GetFloat(text + "_MaximumTorqueTemp", vehicle.engineTorque));
			switch (ObscuredPrefs.GetString(text + "_DrivetrainModeTemp", vehicle.wheelTypeChoise.ToString()))
			{
			case "FWD":
				vehicle.wheelTypeChoise = RCC_CarControllerV3.WheelType.FWD;
				break;
			case "RWD":
				vehicle.wheelTypeChoise = RCC_CarControllerV3.WheelType.RWD;
				break;
			case "AWD":
				vehicle.wheelTypeChoise = RCC_CarControllerV3.WheelType.AWD;
				break;
			}
			SetGearShiftingThreshold(vehicle, ObscuredPrefs.GetFloat(text + "_GearShiftingThresholdTemp", vehicle.gearShiftingThreshold));
			SetClutchThreshold(vehicle, ObscuredPrefs.GetFloat(text + "_ClutchingThresholdTemp", vehicle.clutchInertia));
			SetCounterSteering(vehicle, RCC_PlayerPrefsX.GetBool(text + "_CounterSteeringTemp", vehicle.applyCounterSteering));
			SetABS(vehicle, RCC_PlayerPrefsX.GetBool(text + "_ABSTemp", vehicle.ABS));
			SetESP(vehicle, RCC_PlayerPrefsX.GetBool(text + "_ESPTemp", vehicle.ESP));
			SetTCS(vehicle, RCC_PlayerPrefsX.GetBool(text + "_TCSTemp", vehicle.TCS));
			SetSH(vehicle, RCC_PlayerPrefsX.GetBool(text + "_SHTemp", vehicle.steeringHelper));
			SetNOS(vehicle, RCC_PlayerPrefsX.GetBool(text + "NOSTemp", vehicle.useNOS));
			SetTurbo(vehicle, RCC_PlayerPrefsX.GetBool(text + "TurboTemp", vehicle.useTurbo));
			SetUseExhaustFlame(vehicle, RCC_PlayerPrefsX.GetBool(text + "ExhaustFlameTemp", vehicle.useExhaustFlame));
			SetRevLimiter(vehicle, RCC_PlayerPrefsX.GetBool(text + "RevLimiterTemp", vehicle.useRevLimiter));
			SetClutchMargin(vehicle, RCC_PlayerPrefsX.GetBool(text + "ClutchMarginTemp", vehicle.useClutchMarginAtFirstGear));
			if (PlayerPrefs.HasKey(text + "_HeadlightsColorTemp"))
			{
				SetHeadlightsColor(vehicle, RCC_PlayerPrefsX.GetColor(text + "_HeadlightsColorTemp"));
			}
			OverrideRCC(vehicle);
		}
	}

	public static void ResetStats(RCC_CarControllerV3 vehicle, RCC_CarControllerV3 defaultCar)
	{
		if (CheckVehicle(vehicle) && CheckVehicle(defaultCar))
		{
			SetFrontCambers(vehicle, defaultCar.FrontLeftWheelCollider.camber);
			SetRearCambers(vehicle, defaultCar.RearLeftWheelCollider.camber);
			SetFrontSuspensionsDistances(vehicle, defaultCar.FrontLeftWheelCollider.wheelCollider.suspensionDistance);
			SetRearSuspensionsDistances(vehicle, defaultCar.RearLeftWheelCollider.wheelCollider.suspensionDistance);
			SetFrontSuspensionsSpringForce(vehicle, defaultCar.FrontLeftWheelCollider.wheelCollider.suspensionSpring.spring);
			SetRearSuspensionsSpringForce(vehicle, defaultCar.RearLeftWheelCollider.wheelCollider.suspensionSpring.spring);
			SetFrontSuspensionsSpringDamper(vehicle, defaultCar.FrontLeftWheelCollider.wheelCollider.suspensionSpring.damper);
			SetRearSuspensionsSpringDamper(vehicle, defaultCar.RearLeftWheelCollider.wheelCollider.suspensionSpring.damper);
			SetMaximumSpeed(vehicle, defaultCar.maxspeed);
			SetMaximumBrake(vehicle, defaultCar.brakeTorque);
			SetMaximumTorque(vehicle, defaultCar.engineTorque);
			switch (defaultCar.wheelTypeChoise.ToString())
			{
			case "FWD":
				vehicle.wheelTypeChoise = RCC_CarControllerV3.WheelType.FWD;
				break;
			case "RWD":
				vehicle.wheelTypeChoise = RCC_CarControllerV3.WheelType.RWD;
				break;
			case "AWD":
				vehicle.wheelTypeChoise = RCC_CarControllerV3.WheelType.AWD;
				break;
			}
			SetGearShiftingThreshold(vehicle, defaultCar.gearShiftingThreshold);
			SetClutchThreshold(vehicle, defaultCar.clutchInertia);
			SetCounterSteering(vehicle, defaultCar.applyCounterSteering);
			SetABS(vehicle, defaultCar.ABS);
			SetESP(vehicle, defaultCar.ESP);
			SetTCS(vehicle, defaultCar.TCS);
			SetSH(vehicle, defaultCar.steeringHelper);
			SetNOS(vehicle, defaultCar.useNOS);
			SetTurbo(vehicle, defaultCar.useTurbo);
			SetUseExhaustFlame(vehicle, defaultCar.useExhaustFlame);
			SetRevLimiter(vehicle, defaultCar.useRevLimiter);
			SetClutchMargin(vehicle, defaultCar.useClutchMarginAtFirstGear);
			SetSmokeColor(vehicle, 0, Color.white);
			SetHeadlightsColor(vehicle, Color.white);
			SaveStats(vehicle);
			OverrideRCC(vehicle);
		}
	}

	public static bool CheckVehicle(RCC_CarControllerV3 vehicle)
	{
		if (!vehicle)
		{
			Debug.LogError("Vehicle is missing!");
			return false;
		}
		return true;
	}
}
