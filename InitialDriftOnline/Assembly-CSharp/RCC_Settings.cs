using System;
using UnityEngine;

[Serializable]
public class RCC_Settings : ScriptableObject
{
	[Serializable]
	public class BehaviorType
	{
		public string behaviorName = "New Behavior";

		[Header("Steering Helpers")]
		public bool steeringHelper = true;

		public bool tractionHelper = true;

		public bool ABS;

		public bool ESP;

		public bool TCS;

		public bool applyExternalWheelFrictions;

		public bool applyRelativeTorque;

		public float highSpeedSteerAngleMinimum = 40f;

		public float highSpeedSteerAngleMaximum = 40f;

		public float highSpeedSteerAngleAtspeedMinimum = 100f;

		public float highSpeedSteerAngleAtspeedMaximum = 100f;

		[Space]
		[Range(0f, 1f)]
		public float steerHelperAngularVelStrengthMinimum = 0.1f;

		[Range(0f, 1f)]
		public float steerHelperAngularVelStrengthMaximum = 0.1f;

		[Range(0f, 1f)]
		public float steerHelperLinearVelStrengthMinimum = 0.1f;

		[Range(0f, 1f)]
		public float steerHelperLinearVelStrengthMaximum = 0.1f;

		[Range(0f, 1f)]
		public float tractionHelperStrengthMinimum = 0.1f;

		[Range(0f, 1f)]
		public float tractionHelperStrengthMaximum = 0.1f;

		[Space]
		public float antiRollFrontHorizontalMinimum = 1000f;

		public float antiRollRearHorizontalMinimum = 1000f;

		[Space]
		[Range(0f, 1f)]
		public float gearShiftingDelayMaximum = 0.15f;

		[Range(0f, 10f)]
		public float angularDrag = 0.1f;

		[Header("Wheel Frictions Forward")]
		public float forwardExtremumSlip = 0.4f;

		public float forwardExtremumValue = 1f;

		public float forwardAsymptoteSlip = 0.8f;

		public float forwardAsymptoteValue = 0.5f;

		[Header("Wheel Frictions Sideways")]
		public float sidewaysExtremumSlip = 0.2f;

		public float sidewaysExtremumValue = 1f;

		public float sidewaysAsymptoteSlip = 0.5f;

		public float sidewaysAsymptoteValue = 0.75f;
	}

	public enum ControllerType
	{
		Keyboard,
		Mobile,
		XBox360One,
		PS4,
		LogitechSteeringWheel,
		Custom
	}

	public enum Units
	{
		KMH,
		MPH
	}

	public enum UIType
	{
		UI,
		NGUI,
		None
	}

	public enum MobileController
	{
		TouchScreen,
		Gyro,
		SteeringWheel,
		Joystick
	}

	public const string RCCVersion = "V3.4";

	private static RCC_Settings instance;

	public int controllerSelectedIndex;

	public int behaviorSelectedIndex;

	public bool overrideFixedTimeStep = true;

	[Range(0.005f, 0.06f)]
	public float fixedTimeStep = 0.02f;

	[Range(0.5f, 20f)]
	public float maxAngularVelocity = 6f;

	public bool overrideBehavior = true;

	public BehaviorType[] behaviorTypes;

	public bool useFixedWheelColliders = true;

	public bool lockAndUnlockCursor = true;

	public ControllerType controllerType;

	public string verticalInput = "Vertical";

	public string horizontalInput = "Horizontal";

	public string mouseXInput = "Mouse X";

	public string mouseYInput = "Mouse Y";

	public KeyCode handbrakeKB = KeyCode.Space;

	public KeyCode startEngineKB = KeyCode.I;

	public KeyCode lowBeamHeadlightsKB = KeyCode.L;

	public KeyCode highBeamHeadlightsKB = KeyCode.K;

	public KeyCode rightIndicatorKB = KeyCode.E;

	public KeyCode leftIndicatorKB = KeyCode.Q;

	public KeyCode hazardIndicatorKB = KeyCode.Z;

	public KeyCode shiftGearUp = KeyCode.LeftShift;

	public KeyCode shiftGearDown = KeyCode.LeftControl;

	public KeyCode NGear = KeyCode.N;

	public KeyCode boostKB = KeyCode.F;

	public KeyCode slowMotionKB = KeyCode.G;

	public KeyCode changeCameraKB = KeyCode.C;

	public KeyCode lookBackKB = KeyCode.B;

	public KeyCode trailerAttachDetach = KeyCode.T;

	public string Xbox_verticalInput = "Xbox_Vertical";

	public string Xbox_horizontalInput = "Xbox_Horizontal";

	public string Xbox_triggerLeftInput = "Xbox_TriggerLeft";

	public string Xbox_triggerRightInput = "Xbox_TriggerRight";

	public string Xbox_mouseXInput = "Xbox_MouseX";

	public string Xbox_mouseYInput = "Xbox_MouseY";

	public string Xbox_handbrakeKB = "Xbox_B";

	public string Xbox_startEngineKB = "Xbox_Y";

	public string Xbox_lowBeamHeadlightsKB = "Xbox_LB";

	public string Xbox_highBeamHeadlightsKB = "Xbox_RB";

	public string Xbox_indicatorKB = "Xbox_DPadHorizontal";

	public string Xbox_hazardIndicatorKB = "Xbox_DPadVertical";

	public string Xbox_shiftGearUp = "Xbox_RB";

	public string Xbox_shiftGearDown = "Xbox_LB";

	public string Xbox_boostKB = "Xbox_A";

	public string Xbox_changeCameraKB = "Xbox_Back";

	public KeyCode Xbox_recordKB = KeyCode.R;

	public KeyCode Xbox_playbackKB = KeyCode.P;

	public string Xbox_lookBackKB = "Xbox_ClickRight";

	public string Xbox_trailerAttachDetach = "Xbox_ClickLeft";

	public string PS4_verticalInput = "PS4_Vertical";

	public string PS4_horizontalInput = "PS4_Horizontal";

	public string PS4_triggerLeftInput = "PS4_TriggerLeft";

	public string PS4_triggerRightInput = "PS4_TriggerRight";

	public string PS4_mouseXInput = "PS4_MouseX";

	public string PS4_mouseYInput = "PS4_MouseY";

	public string PS4_handbrakeKB = "PS4_B";

	public string PS4_startEngineKB = "PS4_Y";

	public string PS4_lowBeamHeadlightsKB = "PS4_LB";

	public string PS4_highBeamHeadlightsKB = "PS4_RB";

	public string PS4_indicatorKB = "PS4_DPadHorizontal";

	public string PS4_hazardIndicatorKB = "PS4_DPadVertical";

	public string PS4_shiftGearUp = "PS4_RB";

	public string PS4_shiftGearDown = "PS4_LB";

	public string PS4_boostKB = "PS4_A";

	public string PS4_changeCameraKB = "PS4_Back";

	public string PS4_lookBackKB = "PS4_ClickRight";

	public string PS4_trailerAttachDetach = "PS4_ClickLeft";

	public int LogiSteeringWheel_handbrakeKB;

	public int LogiSteeringWheel_startEngineKB;

	public int LogiSteeringWheel_lowBeamHeadlightsKB;

	public int LogiSteeringWheel_highBeamHeadlightsKB;

	public int LogiSteeringWheel_hazardIndicatorKB;

	public int LogiSteeringWheel_shiftGearUp;

	public int LogiSteeringWheel_shiftGearDown;

	public int LogiSteeringWheel_boostKB;

	public int LogiSteeringWheel_changeCameraKB;

	public int LogiSteeringWheel_lookBackKB;

	public bool useVR;

	public bool useAutomaticGear = true;

	public bool runEngineAtAwake = true;

	public bool autoReverse = true;

	public bool autoReset = true;

	public GameObject contactParticles;

	public Units units;

	public UIType uiType;

	public bool useTelemetry;

	public MobileController mobileController;

	public float UIButtonSensitivity = 3f;

	public float UIButtonGravity = 5f;

	public float gyroSensitivity = 2f;

	public bool useLightsAsVertexLights = true;

	public bool useLightProjectorForLightingEffect;

	public bool setTagsAndLayers;

	public string RCCLayer;

	public string RCCTag;

	public bool tagAllChildrenGameobjects;

	public GameObject chassisJoint;

	public GameObject exhaustGas;

	public RCC_SkidmarksManager skidmarksManager;

	public GameObject projector;

	public LayerMask projectorIgnoreLayer;

	public GameObject headLights;

	public GameObject brakeLights;

	public GameObject reverseLights;

	public GameObject indicatorLights;

	public GameObject mirrors;

	public RCC_Camera RCCMainCamera;

	public GameObject hoodCamera;

	public GameObject cinematicCamera;

	public GameObject RCCCanvas;

	public bool dontUseAnyParticleEffects;

	public bool dontUseChassisJoint;

	public bool dontUseSkidmarks;

	public AudioClip[] gearShiftingClips;

	public AudioClip[] crashClips;

	public AudioClip reversingClip;

	public AudioClip windClip;

	public AudioClip brakeClip;

	public AudioClip indicatorClip;

	public AudioClip NOSClip;

	public AudioClip turboClip;

	public AudioClip[] blowoutClip;

	public AudioClip[] exhaustFlameClips;

	public bool useSharedAudioSources = true;

	[Range(0f, 1f)]
	public float maxGearShiftingSoundVolume = 0.25f;

	[Range(0f, 1f)]
	public float maxCrashSoundVolume = 1f;

	[Range(0f, 1f)]
	public float maxWindSoundVolume = 0.1f;

	[Range(0f, 1f)]
	public float maxBrakeSoundVolume = 0.1f;

	public bool foldGeneralSettings;

	public bool foldBehaviorSettings;

	public bool foldControllerSettings;

	public bool foldUISettings;

	public bool foldWheelPhysics;

	public bool foldSFX;

	public bool foldOptimization;

	public bool foldTagsAndLayers;

	public static RCC_Settings Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Resources.Load("RCC Assets/RCC_Settings") as RCC_Settings;
			}
			return instance;
		}
	}

	public BehaviorType selectedBehaviorType
	{
		get
		{
			if (overrideBehavior)
			{
				return behaviorTypes[behaviorSelectedIndex];
			}
			return null;
		}
	}
}
