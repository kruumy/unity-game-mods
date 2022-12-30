using CodeStage.AntiCheat.Detectors;
using CodeStage.AntiCheat.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using XInputDotNetPure;
using ZionBandwidthOptimizer.Examples;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Main/RCC Realistic Car Controller V3")]
[RequireComponent(typeof(Rigidbody))]
public class RCC_CarControllerV3 : MonoBehaviour
{
    public enum SteeringWheelRotateAround
    {
        XAxis,
        YAxis,
        ZAxis
    }

    public enum WheelType
    {
        FWD,
        RWD,
        AWD,
        BIASED
    }

    [Serializable]
    public class Gear
    {
        public float maxRatio;

        public int maxSpeed;

        public int targetSpeedForNextGear;

        public AnimationCurve torqueCurve;

        public void SetGear(float ratio, int speed, int targetSpeed)
        {
            maxRatio = ratio;
            maxSpeed = speed;
            targetSpeedForNextGear = targetSpeed;
        }
    }

    public enum AudioType
    {
        OneSource,
        TwoSource,
        ThreeSource,
        Off
    }

    public enum IndicatorsOn
    {
        Off,
        Right,
        Left,
        All
    }

    private struct originalMeshVerts
    {
        public Vector3[] meshVerts;
    }

    [Serializable]
    public class ConfigureVehicleSubsteps
    {
        public float speedThreshold = 10f;

        public int stepsBelowThreshold = 5;

        public int stepsAboveThreshold = 5;
    }

    public delegate void onRCCPlayerSpawned(RCC_CarControllerV3 RCC);

    public delegate void onRCCPlayerDestroyed(RCC_CarControllerV3 RCC);

    public delegate void onRCCPlayerCollision(RCC_CarControllerV3 RCC, Collision collision);

    private RCC_Settings RCCSettingsInstance;

    internal Rigidbody rigid;

    internal bool isSleeping;

    public bool externalController;

    public Transform FrontLeftWheelTransform;

    public Transform FrontRightWheelTransform;

    public Transform RearLeftWheelTransform;

    public Transform RearRightWheelTransform;

    public RCC_WheelCollider FrontLeftWheelCollider;

    public RCC_WheelCollider FrontRightWheelCollider;

    public RCC_WheelCollider RearLeftWheelCollider;

    public RCC_WheelCollider RearRightWheelCollider;

    internal RCC_WheelCollider[] allWheelColliders;

    public Transform[] ExtraRearWheelsTransform;

    public RCC_WheelCollider[] ExtraRearWheelsCollider;

    public bool applyEngineTorqueToExtraRearWheelColliders = true;

    public Transform SteeringWheel;

    private Quaternion orgSteeringWheelRot;

    public SteeringWheelRotateAround steeringWheelRotateAround;

    public float steeringWheelAngleMultiplier = 3f;

    public WheelType wheelTypeChoise = WheelType.RWD;

    [Range(0f, 100f)]
    public float biasedWheelTorque = 100f;

    public Transform COM;

    public bool canControl = true;

    public bool engineRunning;

    internal bool semiAutomaticGear;

    internal bool canGoReverseNow;

    public AnimationCurve[] engineTorqueCurve;

    public float finalRatio = 3.23f;

    public float engineTorque = 150f;

    public float brakeTorque = 2000f;

    public float minEngineRPM = 1000f;

    public float maxEngineRPM = 7000f;

    [Range(0.75f, 2f)]
    public float engineInertia = 1f;

    public bool useRevLimiter = true;

    public bool useExhaustFlame = true;

    public bool useClutchMarginAtFirstGear = true;

    public float steerAngle = 40f;

    public float highspeedsteerAngle = 3f;

    public float highspeedsteerAngleAtspeed = 200f;

    [Range(1f, 15f)]
    public float steeringSensitivity = 5f;

    public float antiRollFrontHorizontal = 1000f;

    public float antiRollRearHorizontal = 1000f;

    public float antiRollVertical;

    public float downForce = 25f;

    public bool useCounterSteering = true;

    public float speed;

    public float orgMaxSpeed;

    public float maxspeed = 220f;

    private float resetTime;

    // OLD: private float orgSteerAngle;
    private readonly float orgSteerAngle = 90f;

    public bool useFuelConsumption;

    public float fuelTankCapacity = 62f;

    public float fuelTank = 62f;

    public float fuelConsumptionRate = 0.1f;

    public bool useEngineHeat;

    public float engineHeat = 15f;

    public float engineCoolingWaterThreshold = 60f;

    public float engineHeatRate = 1f;

    public float engineCoolRate = 1f;

    private int WaitClignoInt;

    private int WaitBoostInt;

    private int WaitFamInt;

    private bool highbeamstatesub;

    private bool cangoreversetemp = true;

    private bool TempoForFam;

    private int speedhack;

    private PlayerIndex playerIndex;

    private GamePadState state;

    private GamePadState prevState;

    public Gear[] gears;

    public int totalGears = 6;

    public int currentGear;

    [Range(0f, 0.5f)]
    public float gearShiftingDelay = 0.35f;

    [Range(0.25f, 0.8f)]
    public float gearShiftingThreshold = 0.75f;

    [Range(0.1f, 0.9f)]
    public float clutchInertia = 0.25f;

    private float orgGearShiftingThreshold;

    public bool changingGear;

    public bool NGear;

    public int direction = 1;

    public float launched;

    public bool autoGenerateGearCurves = true;

    public bool autoGenerateTargetSpeedsForChangingGear = true;

    public AudioType audioType;

    public bool autoCreateEngineOffSounds = true;

    private AudioSource engineStartSound;

    public AudioClip engineStartClip;

    internal AudioSource engineSoundHigh;

    public AudioClip engineClipHigh;

    private AudioSource engineSoundMed;

    public AudioClip engineClipMed;

    private AudioSource engineSoundLow;

    public AudioClip engineClipLow;

    private AudioSource engineSoundIdle;

    public AudioClip engineClipIdle;

    private AudioSource gearShiftingSound;

    internal AudioSource engineSoundHighOff;

    public AudioClip engineClipHighOff;

    internal AudioSource engineSoundMedOff;

    public AudioClip engineClipMedOff;

    internal AudioSource engineSoundLowOff;

    public AudioClip engineClipLowOff;

    private AudioSource crashSound;

    private AudioSource reversingSound;

    private AudioSource windSound;

    private AudioSource brakeSound;

    private AudioSource NOSSound;

    private AudioSource turboSound;

    private AudioSource blowSound;

    public AudioMixerGroup audioMixer;

    [Range(0f, 1f)]
    public float minEngineSoundPitch = 0.75f;

    [Range(1f, 2f)]
    public float maxEngineSoundPitch = 1.75f;

    [Range(0f, 1f)]
    public float minEngineSoundVolume = 0.05f;

    [Range(0f, 1f)]
    public float maxEngineSoundVolume = 0.85f;

    [Range(0f, 1f)]
    public float idleEngineSoundVolume = 0.85f;

    public Vector3 engineSoundPosition = new Vector3(0f, 0f, 1.5f);

    public Vector3 gearSoundPosition = new Vector3(0f, -0.5f, 0.5f);

    public Vector3 turboSoundPosition = new Vector3(0f, 0f, 1.5f);

    public Vector3 exhaustSoundPosition = new Vector3(0f, -0.5f, 2f);

    public Vector3 windSoundPosition = new Vector3(0f, 0f, 2f);

    private GameObject allContactParticles;

    [HideInInspector]
    public float gasInput;

    [HideInInspector]
    public float brakeInput;

    [HideInInspector]
    public float steerInput;

    [HideInInspector]
    public float clutchInput;

    [HideInInspector]
    public float handbrakeInput;

    [HideInInspector]
    public float boostInput;

    [HideInInspector]
    public float idleInput;

    [HideInInspector]
    public float fuelInput;

    [HideInInspector]
    public bool cutGas;

    private bool cheater;

    private bool permanentGas;

    internal float rawEngineRPM;

    internal float engineRPM;

    public GameObject chassis;

    public float chassisVerticalLean = 4f;

    public float chassisHorizontalLean = 4f;

    public bool lowBeamHeadLightsOn;

    public bool highBeamHeadLightsOn;

    public IndicatorsOn indicatorsOn;

    public float indicatorTimer;

    public bool useDamage = true;

    private originalMeshVerts[] originalMeshData;

    public MeshFilter[] deformableMeshFilters;

    public LayerMask damageFilter = -1;

    public float randomizeVertices = 1f;

    public float damageRadius = 0.5f;

    private float minimumVertDistanceForDamagedMesh = 0.002f;

    public bool repairNow;

    public bool repaired = true;

    public float maximumDamage = 0.5f;

    private float minimumCollisionForce = 5f;

    public float damageMultiplier = 1f;

    public int maximumContactSparkle = 5;

    private List<ParticleSystem> contactSparkeList = new List<ParticleSystem>();

    private Vector3 localVector;

    private Quaternion rot = Quaternion.identity;

    private float oldRotation;

    public Transform velocityDirection;

    public Transform steeringDirection;

    public float velocityAngle;

    private float angle;

    private float angularVelo;

    public bool ABS = true;

    public bool TCS = true;

    public bool ESP = true;

    public bool steeringHelper = true;

    public bool tractionHelper = true;

    public bool angularDragHelper;

    [Range(0.05f, 0.5f)]
    public float ABSThreshold = 0.35f;

    [Range(0.05f, 0.5f)]
    public float TCSThreshold = 0.5f;

    [Range(0.05f, 1f)]
    public float TCSStrength = 1f;

    [Range(0.05f, 0.5f)]
    public float ESPThreshold = 0.25f;

    [Range(0.05f, 1f)]
    public float ESPStrength = 0.5f;

    [Range(0f, 1f)]
    public float steerHelperLinearVelStrength = 0.1f;

    [Range(0f, 1f)]
    public float steerHelperAngularVelStrength = 0.1f;

    [Range(0f, 1f)]
    public float tractionHelperStrength = 0.1f;

    [Range(0f, 1f)]
    public float angularDragHelperStrength = 0.1f;

    public bool ABSAct;

    public bool TCSAct;

    public bool ESPAct;

    public bool underSteering;

    public bool overSteering;

    internal bool driftingNow;

    internal float driftAngle;

    public bool applyCounterSteering = true;

    [Range(0f, 1f)]
    public float counterSteeringFactor = 0.5f;

    public float frontSlip;

    public float rearSlip;

    public float turboBoost;

    public float NoS = 100f;

    private float NoSConsumption = 25f;

    private float NoSRegenerateTime = 10f;

    private int NoBrakeAtStop;

    private bool waitingForbip;

    private int temporpcfeux;

    public bool useNOS;

    public bool useTurbo;

    public RCC_TruckTrailer attachedTrailer;

    private float VibTime;

    public ConfigureVehicleSubsteps configureVehicleSubsteps = new ConfigureVehicleSubsteps();

    private int lightprio;

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

    [Obsolete("Warning 'AIController' is obsolete: 'Please use externalController.")]
    public bool AIController
    {
        get
        {
            return externalController;
        }
        set
        {
            externalController = value;
        }
    }

    public bool runEngineAtAwake => RCCSettings.runEngineAtAwake;

    public bool autoReverse
    {
        get
        {
            if (!externalController)
            {
                return RCCSettings.autoReverse;
            }
            return true;
        }
    }

    public bool automaticGear
    {
        get
        {
            if (!externalController)
            {
                return RCCSettings.useAutomaticGear;
            }
            return true;
        }
    }

    private AudioClip[] gearShiftingClips => RCCSettings.gearShiftingClips;

    private AudioClip[] crashClips => RCCSettings.crashClips;

    private AudioClip reversingClip => RCCSettings.reversingClip;

    private AudioClip windClip => RCCSettings.windClip;

    private AudioClip brakeClip => RCCSettings.brakeClip;

    private AudioClip NOSClip => RCCSettings.NOSClip;

    private AudioClip turboClip => RCCSettings.turboClip;

    private AudioClip blowClip => RCCSettings.turboClip;

    internal float _gasInput
    {
        get
        {
            if (_fuelInput <= 0f)
            {
                return 0f;
            }
            if (!automaticGear || semiAutomaticGear)
            {
                if (!changingGear && !cutGas)
                {
                    return Mathf.Clamp01(gasInput);
                }
                return 0f;
            }
            if (!changingGear && !cutGas)
            {
                if (direction != 1)
                {
                    return Mathf.Clamp01(brakeInput);
                }
                return Mathf.Clamp01(gasInput);
            }
            return 0f;
        }
        set
        {
            gasInput = value;
        }
    }

    internal float _brakeInput
    {
        get
        {
            if (!automaticGear || semiAutomaticGear)
            {
                return Mathf.Clamp01(brakeInput);
            }
            if (!cutGas)
            {
                if (direction != 1)
                {
                    return Mathf.Clamp01(gasInput);
                }
                return Mathf.Clamp01(brakeInput);
            }
            return 0f;
        }
        set
        {
            brakeInput = value;
        }
    }

    internal float _boostInput
    {
        get
        {
            if (useNOS && NoS > 5f && _gasInput >= 0.5f)
            {
                return boostInput * 1f;
            }
            return 0f;
        }
        set
        {
            boostInput = value;
        }
    }

    internal float _steerInput => steerInput + _counterSteerInput;

    internal float _counterSteerInput
    {
        get
        {
            if (applyCounterSteering)
            {
                return driftAngle * counterSteeringFactor;
            }
            return 0f;
        }
    }

    internal float _fuelInput
    {
        get
        {
            if (fuelTank > 0f)
            {
                return fuelInput;
            }
            if (engineRunning)
            {
                KillEngine();
            }
            return 0f;
        }
        set
        {
            fuelInput = value;
        }
    }

    public GameObject contactSparkle => RCCSettings.contactParticles;

    public static event onRCCPlayerSpawned OnRCCPlayerSpawned;

    public static event onRCCPlayerDestroyed OnRCCPlayerDestroyed;

    public static event onRCCPlayerCollision OnRCCPlayerCollision;

    private void Awake()
    {
        SpeedHackDetector.StartDetection(OnSpeedHackDetected);
        waitingForbip = false;
        NoBrakeAtStop = 0;
        TempoForFam = false;
        StartCoroutine(TempoForFamm());
        if (RCCSettings.overrideFixedTimeStep)
        {
            Time.fixedDeltaTime = RCCSettings.fixedTimeStep;
        }
        rigid = GetComponent<Rigidbody>();
        rigid.maxAngularVelocity = RCCSettings.maxAngularVelocity;
        gearShiftingThreshold = Mathf.Clamp(gearShiftingThreshold, 0.25f, 0.8f);
        allWheelColliders = GetComponentsInChildren<RCC_WheelCollider>();
        GetComponentInChildren<WheelCollider>().ConfigureVehicleSubsteps(configureVehicleSubsteps.speedThreshold, configureVehicleSubsteps.stepsBelowThreshold, configureVehicleSubsteps.stepsAboveThreshold);
        FrontLeftWheelCollider.wheelModel = FrontLeftWheelTransform;
        FrontRightWheelCollider.wheelModel = FrontRightWheelTransform;
        RearLeftWheelCollider.wheelModel = RearLeftWheelTransform;
        RearRightWheelCollider.wheelModel = RearRightWheelTransform;
        for (int i = 0; i < ExtraRearWheelsCollider.Length; i++)
        {
            ExtraRearWheelsCollider[i].wheelModel = ExtraRearWheelsTransform[i];
        }
        // OLD: orgSteerAngle = steerAngle;
        allContactParticles = new GameObject("All Contact Particles");
        allContactParticles.transform.SetParent(base.transform, worldPositionStays: false);
        CreateGearCurves();
        CreateAudios();
        if (useDamage)
        {
            InitDamage();
        }
        CheckBehavior();
        if (runEngineAtAwake || externalController)
        {
            engineRunning = true;
            fuelInput = 1f;
            if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
            {
                GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
            }
        }
        if ((bool)chassis && !chassis.GetComponent<RCC_Chassis>())
        {
            chassis.AddComponent<RCC_Chassis>();
        }
    }

    private void OnSpeedHackDetected()
    {
        cheater = true;
    }

    private void OnGUI()
    {
        if (GetComponentInParent<RCC_PhotonNetwork>().isMine && cheater)
        {
            Debug.Log("Cheating detect");
        }
    }

    private void OnEnable()
    {
        StartCoroutine(RCCPlayerSpawned());
        RCC_SceneManager.OnBehaviorChanged += CheckBehavior;
    }

    private IEnumerator RCCPlayerSpawned()
    {
        yield return new WaitForEndOfFrame();
        if (!externalController && RCC_CarControllerV3.OnRCCPlayerSpawned != null)
        {
            RCC_CarControllerV3.OnRCCPlayerSpawned(this);
        }
    }

    private IEnumerator TempoForFamm()
    {
        yield return new WaitForSeconds(5f);
        TempoForFam = true;
    }

    public void CreateWheelColliders()
    {
        List<Transform> list = new List<Transform>();
        list.Add(FrontLeftWheelTransform);
        list.Add(FrontRightWheelTransform);
        list.Add(RearLeftWheelTransform);
        list.Add(RearRightWheelTransform);
        if (ExtraRearWheelsTransform.Length != 0 && (bool)ExtraRearWheelsTransform[0])
        {
            Transform[] extraRearWheelsTransform = ExtraRearWheelsTransform;
            foreach (Transform item in extraRearWheelsTransform)
            {
                list.Add(item);
            }
        }
        if (list != null && list[0] == null)
        {
            Debug.LogError("You haven't choosen your Wheel Models. Please select all of your Wheel Models before creating Wheel Colliders. Script needs to know their sizes and positions, aye?");
            return;
        }
        Quaternion rotation = base.transform.rotation;
        base.transform.rotation = Quaternion.identity;
        GameObject gameObject = new GameObject("Wheel Colliders");
        gameObject.transform.SetParent(base.transform, worldPositionStays: false);
        gameObject.transform.localRotation = Quaternion.identity;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localScale = Vector3.one;
        foreach (Transform item2 in list)
        {
            GameObject gameObject2 = new GameObject(item2.transform.name);
            gameObject2.transform.position = RCC_GetBounds.GetBoundsCenter(item2.transform);
            gameObject2.transform.rotation = base.transform.rotation;
            gameObject2.transform.name = item2.transform.name;
            gameObject2.transform.SetParent(gameObject.transform);
            gameObject2.transform.localScale = Vector3.one;
            gameObject2.AddComponent<WheelCollider>();
            Bounds bounds = default(Bounds);
            Renderer[] componentsInChildren = item2.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in componentsInChildren)
            {
                if (renderer != GetComponent<Renderer>() && renderer.bounds.size.z > bounds.size.z)
                {
                    bounds = renderer.bounds;
                }
            }
            gameObject2.GetComponent<WheelCollider>().radius = bounds.extents.y / base.transform.localScale.y;
            gameObject2.AddComponent<RCC_WheelCollider>();
            JointSpring suspensionSpring = gameObject2.GetComponent<WheelCollider>().suspensionSpring;
            suspensionSpring.spring = 40000f;
            suspensionSpring.damper = 1500f;
            suspensionSpring.targetPosition = 0.5f;
            gameObject2.GetComponent<WheelCollider>().suspensionSpring = suspensionSpring;
            gameObject2.GetComponent<WheelCollider>().suspensionDistance = 0.2f;
            gameObject2.GetComponent<WheelCollider>().forceAppPointDistance = 0f;
            gameObject2.GetComponent<WheelCollider>().mass = 40f;
            gameObject2.GetComponent<WheelCollider>().wheelDampingRate = 1f;
            WheelFrictionCurve sidewaysFriction = gameObject2.GetComponent<WheelCollider>().sidewaysFriction;
            WheelFrictionCurve forwardFriction = gameObject2.GetComponent<WheelCollider>().forwardFriction;
            forwardFriction.extremumSlip = 0.3f;
            forwardFriction.extremumValue = 1f;
            forwardFriction.asymptoteSlip = 0.8f;
            forwardFriction.asymptoteValue = 0.6f;
            forwardFriction.stiffness = 1.5f;
            sidewaysFriction.extremumSlip = 0.3f;
            sidewaysFriction.extremumValue = 1f;
            sidewaysFriction.asymptoteSlip = 0.5f;
            sidewaysFriction.asymptoteValue = 0.8f;
            sidewaysFriction.stiffness = 1.5f;
            gameObject2.GetComponent<WheelCollider>().sidewaysFriction = sidewaysFriction;
            gameObject2.GetComponent<WheelCollider>().forwardFriction = forwardFriction;
        }
        RCC_WheelCollider[] array = new RCC_WheelCollider[list.Count];
        array = GetComponentsInChildren<RCC_WheelCollider>();
        FrontLeftWheelCollider = array[0];
        FrontRightWheelCollider = array[1];
        RearLeftWheelCollider = array[2];
        RearRightWheelCollider = array[3];
        ExtraRearWheelsCollider = new RCC_WheelCollider[ExtraRearWheelsTransform.Length];
        for (int j = 0; j < ExtraRearWheelsTransform.Length; j++)
        {
            ExtraRearWheelsCollider[j] = array[j + 4];
        }
        base.transform.rotation = rotation;
    }

    private void CreateAudios()
    {
        switch (audioType)
        {
            case AudioType.OneSource:
                engineSoundHigh = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, engineSoundPosition, "Engine Sound High AudioSource", 5f, 50f, 0f, engineClipHigh, loop: true, playNow: true, destroyAfterFinished: false);
                if (autoCreateEngineOffSounds)
                {
                    engineSoundHighOff = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, engineSoundPosition, "Engine Sound High Off AudioSource", 5f, 50f, 0f, engineClipHigh, loop: true, playNow: true, destroyAfterFinished: false);
                    RCC_CreateAudioSource.NewLowPassFilter(engineSoundHighOff, 3000f);
                }
                else
                {
                    engineSoundHighOff = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, engineSoundPosition, "Engine Sound High Off AudioSource", 5f, 50f, 0f, engineClipHighOff, loop: true, playNow: true, destroyAfterFinished: false);
                }
                break;
            case AudioType.TwoSource:
                engineSoundHigh = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, engineSoundPosition, "Engine Sound High AudioSource", 5f, 50f, 0f, engineClipHigh, loop: true, playNow: true, destroyAfterFinished: false);
                engineSoundLow = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, engineSoundPosition, "Engine Sound Low AudioSource", 5f, 25f, 0f, engineClipLow, loop: true, playNow: true, destroyAfterFinished: false);
                if (autoCreateEngineOffSounds)
                {
                    engineSoundHighOff = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, engineSoundPosition, "Engine Sound High Off AudioSource", 5f, 50f, 0f, engineClipHigh, loop: true, playNow: true, destroyAfterFinished: false);
                    engineSoundLowOff = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, engineSoundPosition, "Engine Sound Low Off AudioSource", 5f, 25f, 0f, engineClipLow, loop: true, playNow: true, destroyAfterFinished: false);
                    RCC_CreateAudioSource.NewLowPassFilter(engineSoundHighOff, 3000f);
                    RCC_CreateAudioSource.NewLowPassFilter(engineSoundLowOff, 3000f);
                }
                else
                {
                    engineSoundHighOff = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, engineSoundPosition, "Engine Sound High Off AudioSource", 5f, 50f, 0f, engineClipHighOff, loop: true, playNow: true, destroyAfterFinished: false);
                    engineSoundLowOff = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, engineSoundPosition, "Engine Sound Low Off AudioSource", 5f, 25f, 0f, engineClipLowOff, loop: true, playNow: true, destroyAfterFinished: false);
                }
                break;
            case AudioType.ThreeSource:
                engineSoundHigh = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, engineSoundPosition, "Engine Sound High AudioSource", 5f, 50f, 0f, engineClipHigh, loop: true, playNow: true, destroyAfterFinished: false);
                engineSoundMed = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, engineSoundPosition, "Engine Sound Medium AudioSource", 5f, 50f, 0f, engineClipMed, loop: true, playNow: true, destroyAfterFinished: false);
                engineSoundLow = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, engineSoundPosition, "Engine Sound Low AudioSource", 5f, 25f, 0f, engineClipLow, loop: true, playNow: true, destroyAfterFinished: false);
                if (autoCreateEngineOffSounds)
                {
                    engineSoundHighOff = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, engineSoundPosition, "Engine Sound High Off AudioSource", 5f, 50f, 0f, engineClipHigh, loop: true, playNow: true, destroyAfterFinished: false);
                    engineSoundMedOff = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, engineSoundPosition, "Engine Sound Medium Off AudioSource", 5f, 50f, 0f, engineClipMed, loop: true, playNow: true, destroyAfterFinished: false);
                    engineSoundLowOff = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, engineSoundPosition, "Engine Sound Low Off AudioSource", 5f, 25f, 0f, engineClipLow, loop: true, playNow: true, destroyAfterFinished: false);
                    if ((bool)engineSoundHighOff)
                    {
                        RCC_CreateAudioSource.NewLowPassFilter(engineSoundHighOff, 3000f);
                    }
                    if ((bool)engineSoundMedOff)
                    {
                        RCC_CreateAudioSource.NewLowPassFilter(engineSoundMedOff, 3000f);
                    }
                    if ((bool)engineSoundLowOff)
                    {
                        RCC_CreateAudioSource.NewLowPassFilter(engineSoundLowOff, 3000f);
                    }
                }
                else
                {
                    engineSoundHighOff = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, engineSoundPosition, "Engine Sound High Off AudioSource", 5f, 50f, 0f, engineClipHighOff, loop: true, playNow: true, destroyAfterFinished: false);
                    engineSoundMedOff = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, engineSoundPosition, "Engine Sound Medium Off AudioSource", 5f, 50f, 0f, engineClipMedOff, loop: true, playNow: true, destroyAfterFinished: false);
                    engineSoundLowOff = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, engineSoundPosition, "Engine Sound Low Off AudioSource", 5f, 25f, 0f, engineClipLowOff, loop: true, playNow: true, destroyAfterFinished: false);
                }
                break;
        }
        engineSoundIdle = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, engineSoundPosition, "Engine Sound Idle AudioSource", 5f, 25f, 0f, engineClipIdle, loop: true, playNow: true, destroyAfterFinished: false);
        reversingSound = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, gearSoundPosition, "Reverse Sound AudioSource", 1f, 10f, 0f, reversingClip, loop: true, playNow: false, destroyAfterFinished: false);
        windSound = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, windSoundPosition, "Wind Sound AudioSource", 1f, 10f, 0f, windClip, loop: true, playNow: true, destroyAfterFinished: false);
        brakeSound = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, "Brake Sound AudioSource", 1f, 10f, 0f, brakeClip, loop: true, playNow: true, destroyAfterFinished: false);
        if (useNOS)
        {
            NOSSound = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, exhaustSoundPosition, "NOS Sound AudioSource", 5f, 10f, 1f, NOSClip, loop: true, playNow: false, destroyAfterFinished: false);
        }
        if (useNOS || useTurbo)
        {
            blowSound = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, exhaustSoundPosition, "NOS Blow", 1f, 10f, 1f, null, loop: false, playNow: false, destroyAfterFinished: false);
        }
        if (useTurbo)
        {
            turboSound = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, turboSoundPosition, "Turbo Sound AudioSource", 0.1f, 0.5f, 0f, turboClip, loop: true, playNow: true, destroyAfterFinished: false);
            RCC_CreateAudioSource.NewHighPassFilter(turboSound, 10000f, 10);
        }
    }

    private void CheckBehavior()
    {
        if (RCCSettings.selectedBehaviorType != null)
        {
            RCC_Settings.BehaviorType selectedBehaviorType = RCCSettings.selectedBehaviorType;
            steeringHelper = selectedBehaviorType.steeringHelper;
            tractionHelper = selectedBehaviorType.tractionHelper;
            ABS = selectedBehaviorType.ABS;
            ESP = selectedBehaviorType.ESP;
            TCS = selectedBehaviorType.TCS;
            highspeedsteerAngle = Mathf.Clamp(highspeedsteerAngle, selectedBehaviorType.highSpeedSteerAngleMinimum, selectedBehaviorType.highSpeedSteerAngleMaximum);
            highspeedsteerAngleAtspeed = Mathf.Clamp(highspeedsteerAngleAtspeed, selectedBehaviorType.highSpeedSteerAngleAtspeedMinimum, selectedBehaviorType.highSpeedSteerAngleAtspeedMaximum);
            steerHelperAngularVelStrength = Mathf.Clamp(steerHelperAngularVelStrength, selectedBehaviorType.steerHelperAngularVelStrengthMinimum, selectedBehaviorType.steerHelperAngularVelStrengthMaximum);
            steerHelperLinearVelStrength = Mathf.Clamp(steerHelperLinearVelStrength, selectedBehaviorType.steerHelperLinearVelStrengthMinimum, selectedBehaviorType.steerHelperLinearVelStrengthMaximum);
            tractionHelperStrength = Mathf.Clamp(tractionHelperStrength, selectedBehaviorType.tractionHelperStrengthMinimum, selectedBehaviorType.tractionHelperStrengthMaximum);
            antiRollFrontHorizontal = Mathf.Clamp(antiRollFrontHorizontal, selectedBehaviorType.antiRollFrontHorizontalMinimum, float.PositiveInfinity);
            antiRollRearHorizontal = Mathf.Clamp(antiRollRearHorizontal, selectedBehaviorType.antiRollRearHorizontalMinimum, float.PositiveInfinity);
            gearShiftingDelay = Mathf.Clamp(gearShiftingDelay, 0f, selectedBehaviorType.gearShiftingDelayMaximum);
            rigid.angularDrag = Mathf.Clamp(rigid.angularDrag, selectedBehaviorType.angularDrag, 1f);
            if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
            {
                GetComponentInParent<RCC_PhotonNetwork>().Sendinfo4();
            }
        }
    }

    public void CreateGearCurves()
    {
        gears = new Gear[totalGears];
        float[] array = new float[gears.Length];
        int[] array2 = new int[gears.Length];
        if (gears.Length == 3)
        {
            array = new float[3] { 2f, 1.5f, 1f };
        }
        if (gears.Length == 4)
        {
            array = new float[4] { 2.86f, 1.62f, 1f, 0.72f };
        }
        if (gears.Length == 5)
        {
            array = new float[5] { 4.23f, 2.52f, 1.66f, 1.22f, 1f };
        }
        if (gears.Length == 6)
        {
            array = new float[6] { 4.35f, 2.5f, 1.66f, 1.23f, 1f, 0.85f };
        }
        if (gears.Length == 7)
        {
            array = new float[7] { 4.5f, 2.5f, 1.66f, 1.23f, 1f, 0.9f, 0.8f };
        }
        if (gears.Length == 8)
        {
            array = new float[8] { 4.6f, 2.5f, 1.86f, 1.43f, 1.23f, 1.05f, 0.9f, 0.72f };
        }
        for (int i = 0; i < array2.Length; i++)
        {
            array2[i] = 0;
            array2[i] = (int)(maxspeed / (float)gears.Length * (float)(i + 1));
        }
        for (int j = 0; j < gears.Length; j++)
        {
            gears[j] = new Gear();
            gears[j].SetGear(array[j], array2[j], (int)Mathf.Lerp(0f, maxspeed * Mathf.Lerp(0f, 1f, gearShiftingThreshold), (float)(j + 1) / (float)gears.Length));
        }
        if (autoGenerateGearCurves)
        {
            engineTorqueCurve = new AnimationCurve[gears.Length];
            currentGear = 0;
            for (int k = 0; k < engineTorqueCurve.Length; k++)
            {
                engineTorqueCurve[k] = new AnimationCurve(new Keyframe(0f, 1f));
            }
            for (int l = 0; l < gears.Length; l++)
            {
                if (l != 0)
                {
                    engineTorqueCurve[l].MoveKey(0, new Keyframe(0f, Mathf.Lerp(1f, 0.05f, (float)(l + 1) / (float)gears.Length)));
                    engineTorqueCurve[l].AddKey((float)gears[l].targetSpeedForNextGear / 3f, gears[l].maxRatio / 1.1f);
                    engineTorqueCurve[l].AddKey(gears[l].targetSpeedForNextGear, gears[l].maxRatio);
                    engineTorqueCurve[l].AddKey(gears[l].maxSpeed, Mathf.Lerp(1f, 0.05f, (float)(l + 1) / (float)gears.Length));
                    engineTorqueCurve[l].postWrapMode = WrapMode.Once;
                    engineTorqueCurve[l].preWrapMode = WrapMode.Once;
                    engineTorqueCurve[l].SmoothTangents(1, -0.5f);
                    engineTorqueCurve[l].SmoothTangents(2, 0.75f);
                }
                else
                {
                    engineTorqueCurve[l].MoveKey(0, new Keyframe(0f, gears[0].maxRatio));
                    engineTorqueCurve[l].AddKey(gears[0].targetSpeedForNextGear, gears[0].maxRatio / 1.25f);
                    engineTorqueCurve[l].AddKey(gears[0].maxSpeed, Mathf.Lerp(1f, 0.05f, 1f / (float)gears.Length));
                    engineTorqueCurve[l].postWrapMode = WrapMode.Once;
                    engineTorqueCurve[l].preWrapMode = WrapMode.Once;
                    engineTorqueCurve[l].SmoothTangents(1, 0.5f);
                }
                orgMaxSpeed = maxspeed;
                orgGearShiftingThreshold = gearShiftingThreshold;
            }
        }
        for (int m = 0; m < engineTorqueCurve.Length; m++)
        {
            gears[m].torqueCurve = engineTorqueCurve[m];
        }
    }

    private void InitDamage()
    {
        if (deformableMeshFilters.Length == 0)
        {
            MeshFilter[] componentsInChildren = GetComponentsInChildren<MeshFilter>();
            List<MeshFilter> list = new List<MeshFilter>();
            MeshFilter[] array = componentsInChildren;
            foreach (MeshFilter meshFilter in array)
            {
                if (!meshFilter.transform.IsChildOf(FrontLeftWheelTransform) && !meshFilter.transform.IsChildOf(FrontRightWheelTransform) && !meshFilter.transform.IsChildOf(RearLeftWheelTransform) && !meshFilter.transform.IsChildOf(RearRightWheelTransform))
                {
                    list.Add(meshFilter);
                }
            }
            deformableMeshFilters = list.ToArray();
        }
        LoadOriginalMeshData();
        if ((bool)contactSparkle)
        {
            for (int j = 0; j < maximumContactSparkle; j++)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate(contactSparkle, base.transform.position, Quaternion.identity);
                gameObject.transform.SetParent(allContactParticles.transform);
                contactSparkeList.Add(gameObject.GetComponent<ParticleSystem>());
                ParticleSystem.EmissionModule emission = gameObject.GetComponent<ParticleSystem>().emission;
                emission.enabled = false;
            }
        }
    }

    public void KillOrStartEngine()
    {
        if (engineRunning)
        {
            KillEngine();
        }
        else
        {
            StartEngine();
        }
    }

    public void StartEngine()
    {
        if (!engineRunning)
        {
            StartCoroutine(StartEngineDelayed());
        }
    }

    public void StartEngine(bool instantStart)
    {
        NoBrakeAtStop = 0;
        if (instantStart)
        {
            fuelInput = 1f;
            engineRunning = true;
            if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
            {
                GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
            }
        }
        else
        {
            StartCoroutine(StartEngineDelayed());
        }
    }

    public IEnumerator StartEngineDelayed()
    {
        if (!engineRunning)
        {
            engineStartSound = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, engineSoundPosition, "Engine Start AudioSource", 1f, 10f, 1f, engineStartClip, loop: false, playNow: true, destroyAfterFinished: true);
            if (engineStartSound.isPlaying)
            {
                engineStartSound.Play();
            }
            yield return new WaitForSeconds(1f);
            engineRunning = true;
            if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
            {
                GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
            }
            fuelInput = 1f;
        }
        yield return new WaitForSeconds(1f);
    }

    public void KillEngine()
    {
        fuelInput = 0f;
        engineRunning = false;
        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
        {
            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
        }
    }

    private void LoadOriginalMeshData()
    {
        originalMeshData = new originalMeshVerts[deformableMeshFilters.Length];
        for (int i = 0; i < deformableMeshFilters.Length; i++)
        {
            originalMeshData[i].meshVerts = deformableMeshFilters[i].mesh.vertices;
        }
    }

    public void Repair()
    {
        if (repaired || !repairNow)
        {
            return;
        }
        repaired = true;
        for (int i = 0; i < deformableMeshFilters.Length; i++)
        {
            Vector3[] vertices = deformableMeshFilters[i].mesh.vertices;
            if (originalMeshData == null)
            {
                LoadOriginalMeshData();
            }
            for (int j = 0; j < vertices.Length; j++)
            {
                vertices[j] += (originalMeshData[i].meshVerts[j] - vertices[j]) * (Time.deltaTime * 2f);
                if ((originalMeshData[i].meshVerts[j] - vertices[j]).magnitude >= minimumVertDistanceForDamagedMesh)
                {
                    repaired = false;
                }
            }
            deformableMeshFilters[i].mesh.vertices = vertices;
            deformableMeshFilters[i].mesh.RecalculateNormals();
            deformableMeshFilters[i].mesh.RecalculateBounds();
        }
        if (repaired)
        {
            repairNow = false;
        }
    }

    private void DeformMesh(Mesh mesh, Vector3[] originalMesh, Collision collision, float cos, Transform meshTransform, Quaternion rot)
    {
        Vector3[] vertices = mesh.vertices;
        ContactPoint[] contacts = collision.contacts;
        foreach (ContactPoint contactPoint in contacts)
        {
            Vector3 vector = meshTransform.InverseTransformPoint(contactPoint.point);
            for (int j = 0; j < vertices.Length; j++)
            {
                if ((vector - vertices[j]).magnitude < damageRadius)
                {
                    vertices[j] += rot * (localVector * (damageRadius - (vector - vertices[j]).magnitude) / damageRadius * cos + new Vector3(Mathf.Sin(vertices[j].y * 1000f), Mathf.Sin(vertices[j].z * 1000f), Mathf.Sin(vertices[j].x * 100f)).normalized * (randomizeVertices / 500f));
                    if (maximumDamage > 0f && (vertices[j] - originalMesh[j]).magnitude > maximumDamage)
                    {
                        vertices[j] = originalMesh[j] + (vertices[j] - originalMesh[j]).normalized * maximumDamage;
                    }
                }
            }
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    private void CollisionParticles(Vector3 contactPoint)
    {
        for (int i = 0; i < contactSparkeList.Count && !contactSparkeList[i].isPlaying; i++)
        {
            contactSparkeList[i].transform.position = contactPoint;
            ParticleSystem.EmissionModule emission = contactSparkeList[i].emission;
            emission.enabled = true;
            contactSparkeList[i].Play();
        }
    }

    private void OtherVisuals()
    {
        if ((bool)SteeringWheel)
        {
            if (orgSteeringWheelRot.eulerAngles == Vector3.zero)
            {
                orgSteeringWheelRot = SteeringWheel.transform.localRotation;
            }
            switch (steeringWheelRotateAround)
            {
                case SteeringWheelRotateAround.XAxis:
                    SteeringWheel.transform.localRotation = orgSteeringWheelRot * Quaternion.AngleAxis(FrontLeftWheelCollider.wheelCollider.steerAngle * (0f - steeringWheelAngleMultiplier), Vector3.right);
                    break;
                case SteeringWheelRotateAround.YAxis:
                    SteeringWheel.transform.localRotation = orgSteeringWheelRot * Quaternion.AngleAxis(FrontLeftWheelCollider.wheelCollider.steerAngle * (0f - steeringWheelAngleMultiplier), Vector3.up);
                    break;
                case SteeringWheelRotateAround.ZAxis:
                    SteeringWheel.transform.localRotation = orgSteeringWheelRot * Quaternion.AngleAxis(FrontLeftWheelCollider.wheelCollider.steerAngle * (0f - steeringWheelAngleMultiplier), Vector3.forward);
                    break;
            }
        }
    }

    private void Update()
    {
        if (useCounterSteering)
        {
            steerInput += driftAngle * counterSteeringFactor;
        }
        if (canControl)
        {
            if (!externalController)
            {
                Inputs();
            }
        }
        else if (!externalController)
        {
            _gasInput = 0f;
            brakeInput = 0f;
            boostInput = 0f;
            handbrakeInput = 1f;
            if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
            {
                GetComponentInParent<RCC_PhotonNetwork>().Sendinfo0();
            }
        }
        Audio();
        ResetCar();
        if (useDamage)
        {
            Repair();
        }
        OtherVisuals();
        indicatorTimer += Time.deltaTime;
        if (_gasInput >= 0.1f)
        {
            launched += _gasInput * Time.deltaTime;
        }
        else
        {
            launched -= Time.deltaTime;
        }
        launched = Mathf.Clamp01(launched);
        if (!GetComponentInParent<RCC_PhotonNetwork>().isMine)
        {
            return;
        }
        if (engineRPM < 1000f && speed <= 2f && NoBrakeAtStop == 1 && TempoForFam)
        {
            NoBrakeAtStop = 0;
            if (PlayerPrefs.GetInt("PS4enable") == 0)
            {
                GetComponent<Rigidbody>().drag = 100f;
            }
            handbrakeInput = 1f;
            StartCoroutine(StopFam());
            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo0();
        }
        else if ((engineRPM > 1200f && NoBrakeAtStop == 0) || (speed >= 3.5f && speed <= 4f && NoBrakeAtStop == 0))
        {
            NoBrakeAtStop = 1;
            if (PlayerPrefs.GetInt("PS4enable") == 0)
            {
                GetComponent<Rigidbody>().drag = 0.01f;
            }
            handbrakeInput = 0f;
            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo0();
        }
        if (speed >= 100f && GetComponentInChildren<SkinManager>().CarsPlayerPrefName == "AE86_STOCK" && waitingForbip)
        {
            waitingForbip = false;
            GetComponent<SRPlayerFonction>().BipAt100(yesno: true);
        }
        else if (speed < 100f && GetComponentInChildren<SkinManager>().CarsPlayerPrefName == "AE86_STOCK" && !waitingForbip)
        {
            waitingForbip = true;
            GetComponent<SRPlayerFonction>().BipAt100(yesno: false);
        }
        if (speed >= 100f)
        {
            GetComponent<SRPlayerFonction>().More100kmh();
        }
        if (speed >= 200f)
        {
            GetComponent<SRPlayerFonction>().More200kmh();
        }
        if (speed < 25f && speedhack == 0)
        {
            speedhack = 1;
            StartCoroutine(SHACK());
        }
        else
        {
            speedhack = 0;
        }
    }

    private IEnumerator SHACK()
    {
        yield return new WaitForSeconds(3f);
        if (speed >= 200f)
        {
            Debug.Log("CHeaaaaaat");
            GetComponent<Rigidbody>().drag = 10000f;
            GameObject obj = GameObject.FindWithTag("RCCCanvasPhoton");
            obj.GetComponent<RCC_PhotonDemo>().GoLobbyFromIrohazaka();
            obj.GetComponent<SRUIManager>().LobbyBtn();
        }
    }

    private IEnumerator StopFam()
    {
        yield return new WaitForSeconds(1.5f);
        if (NoBrakeAtStop == 0)
        {
            if (PlayerPrefs.GetInt("PS4enable") == 0)
            {
                GetComponent<Rigidbody>().drag = 0.01f;
            }
            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo0();
            handbrakeInput = 0f;
        }
        yield return new WaitForSeconds(3f);
        if (NoBrakeAtStop == 0)
        {
            handbrakeInput = 1f;
            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo0();
            StartCoroutine(StopFam());
        }
    }

    private void Inputs()
    {
        switch (RCCSettings.controllerType)
        {
            case RCC_Settings.ControllerType.Keyboard:
                {
                    if (Input.GetAxis(RCCSettings.verticalInput) != 0f && PlayerPrefs.GetInt("MenuOpen") == 0)
                    {
                        gasInput = Input.GetAxis(RCCSettings.verticalInput);
                    }
                    if (Input.GetAxis(RCCSettings.Xbox_triggerRightInput) != 0f)
                    {
                        gasInput = Input.GetAxis(RCCSettings.Xbox_triggerRightInput);
                    }
                    if (Input.GetAxis(RCCSettings.Xbox_triggerLeftInput) != 0f)
                    {
                        brakeInput = Input.GetAxis(RCCSettings.Xbox_triggerLeftInput);
                    }
                    else
                    {
                        brakeInput = Mathf.Clamp01(0f - Input.GetAxis(RCCSettings.verticalInput));
                    }
                    if ((double)brakeInput > 0.2)
                    {
                        string text2 = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0];
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text2) == "Stage (0)" || PlayerPrefs.GetString("BreakBtnUsed" + text2) == "")
                        {
                            GetComponent<Rigidbody>().drag = 0.1f;
                        }
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text2) == "Stage (1)")
                        {
                            GetComponent<Rigidbody>().drag = 0.15f;
                        }
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text2) == "Stage (2)")
                        {
                            GetComponent<Rigidbody>().drag = 0.2f;
                        }
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text2) == "Stage (3)")
                        {
                            GetComponent<Rigidbody>().drag = 0.25f;
                        }
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text2) == "Stage (4)")
                        {
                            GetComponent<Rigidbody>().drag = 0.3f;
                        }
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text2) == "Stage (5)")
                        {
                            GetComponent<Rigidbody>().drag = 0.35f;
                        }
                    }
                    else if ((double)gasInput > 0.5 && (double)brakeInput < 0.2 && brakeInput > 0f)
                    {
                        GetComponent<Rigidbody>().drag = 0.1f;
                    }
                    else if (brakeInput == 0f && GetComponent<Rigidbody>().drag != 0.01f && GetComponent<Rigidbody>().drag < 50f)
                    {
                        GetComponent<Rigidbody>().drag = 0.01f;
                    }
                    handbrakeInput = ((Input.GetKey(RCCSettings.handbrakeKB) || Input.GetButton(RCCSettings.Xbox_handbrakeKB)) ? 1f : 0f);
                    if (PlayerPrefs.GetInt("MenuOpen") == 0)
                    {
                        steerInput = Input.GetAxis(RCCSettings.horizontalInput);
                    }
                    else
                    {
                        steerInput = Input.GetAxis("emptyaxis");
                    }
                    if (ObscuredPrefs.GetInt("BoostQuantity") > 0)
                    {
                        boostInput = ((Input.GetKey(RCCSettings.boostKB) || Input.GetButton(RCCSettings.Xbox_boostKB)) ? 1f : 0f);
                        useNOS = true;
                    }
                    else
                    {
                        boostInput = ((Input.GetKey(RCCSettings.boostKB) || Input.GetButton(RCCSettings.Xbox_boostKB)) ? 1f : 0f);
                        useNOS = false;
                        useNOS = true;
                        useNOS = false;
                    }
                    if (Input.GetKeyDown(RCCSettings.boostKB) || Input.GetButtonDown(RCCSettings.Xbox_boostKB))
                    {
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine && WaitBoostInt == 0)
                        {
                            WaitBoostInt = 1;
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo0();
                        }
                    }
                    else if ((Input.GetKeyUp(RCCSettings.boostKB) || Input.GetButtonUp(RCCSettings.Xbox_boostKB)) && GetComponentInParent<RCC_PhotonNetwork>().isMine && WaitBoostInt == 1)
                    {
                        WaitBoostInt = 0;
                        GetComponentInParent<RCC_PhotonNetwork>().Sendinfo0();
                    }
                    if (Input.GetKeyDown(RCCSettings.handbrakeKB) || Input.GetButtonDown(RCCSettings.Xbox_handbrakeKB))
                    {
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine && WaitFamInt == 0)
                        {
                            WaitFamInt = 1;
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo0();
                        }
                    }
                    else if ((Input.GetKeyUp(RCCSettings.handbrakeKB) || Input.GetButtonUp(RCCSettings.Xbox_handbrakeKB)) && GetComponentInParent<RCC_PhotonNetwork>().isMine && WaitFamInt == 1)
                    {
                        WaitFamInt = 0;
                        GetComponentInParent<RCC_PhotonNetwork>().Sendinfo0();
                    }
                    if (Input.GetKeyDown(RCCSettings.lowBeamHeadlightsKB) || Input.GetButtonDown(RCCSettings.Xbox_lowBeamHeadlightsKB))
                    {
                        lowBeamHeadLightsOn = !lowBeamHeadLightsOn;
                        GetComponentInChildren<SRPlayerFonction>().TrailStateBrake(lowBeamHeadLightsOn);
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
                        {
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
                        }
                    }
                    if (Input.GetKeyDown(RCCSettings.highBeamHeadlightsKB) || Input.GetButtonDown(RCCSettings.Xbox_highBeamHeadlightsKB))
                    {
                        highBeamHeadLightsOn = true;
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
                        {
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
                        }
                    }
                    else if (Input.GetKeyUp(RCCSettings.highBeamHeadlightsKB) || Input.GetButtonUp(RCCSettings.Xbox_highBeamHeadlightsKB))
                    {
                        highBeamHeadLightsOn = false;
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
                        {
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
                        }
                    }
                    if (Input.GetKeyDown(RCCSettings.trailerAttachDetach) || Input.GetButtonDown(RCCSettings.Xbox_trailerAttachDetach))
                    {
                        DetachTrailer();
                    }
                    if (Input.GetKeyDown(RCCSettings.rightIndicatorKB))
                    {
                        if (indicatorsOn != IndicatorsOn.Right)
                        {
                            indicatorsOn = IndicatorsOn.Right;
                        }
                        else
                        {
                            indicatorsOn = IndicatorsOn.Off;
                        }
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
                        {
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
                        }
                    }
                    if (Input.GetKeyDown(RCCSettings.leftIndicatorKB))
                    {
                        if (indicatorsOn != IndicatorsOn.Left)
                        {
                            indicatorsOn = IndicatorsOn.Left;
                        }
                        else
                        {
                            indicatorsOn = IndicatorsOn.Off;
                        }
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
                        {
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
                        }
                    }
                    if (Input.GetKeyDown(RCCSettings.hazardIndicatorKB))
                    {
                        if (indicatorsOn != IndicatorsOn.All)
                        {
                            indicatorsOn = IndicatorsOn.Off;
                            indicatorsOn = IndicatorsOn.All;
                        }
                        else
                        {
                            indicatorsOn = IndicatorsOn.Off;
                        }
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
                        {
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
                        }
                    }
                    float axis = Input.GetAxis(RCCSettings.Xbox_indicatorKB);
                    float num = Input.GetAxis(RCCSettings.Xbox_hazardIndicatorKB);
                    if (num >= 0.95f && WaitClignoInt == 0)
                    {
                        StartCoroutine(waitClignoXbox());
                        if (indicatorsOn != IndicatorsOn.All)
                        {
                            indicatorsOn = IndicatorsOn.All;
                        }
                        else
                        {
                            indicatorsOn = IndicatorsOn.Off;
                        }
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
                        {
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
                        }
                    }
                    if (axis >= 1f && WaitClignoInt == 0)
                    {
                        StartCoroutine(waitClignoXbox());
                        if (indicatorsOn != IndicatorsOn.Right)
                        {
                            indicatorsOn = IndicatorsOn.Off;
                            indicatorsOn = IndicatorsOn.Right;
                        }
                        else
                        {
                            indicatorsOn = IndicatorsOn.Off;
                        }
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
                        {
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
                        }
                    }
                    if (axis <= -1f && WaitClignoInt == 0)
                    {
                        StartCoroutine(waitClignoXbox());
                        if (indicatorsOn != IndicatorsOn.Left)
                        {
                            indicatorsOn = IndicatorsOn.Off;
                            indicatorsOn = IndicatorsOn.Left;
                        }
                        else
                        {
                            indicatorsOn = IndicatorsOn.Off;
                        }
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
                        {
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
                        }
                    }
                    if (Input.GetKeyDown(RCCSettings.NGear))
                    {
                        NGear = true;
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
                        {
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
                        }
                    }
                    if (Input.GetKeyUp(RCCSettings.NGear))
                    {
                        NGear = false;
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
                        {
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
                        }
                    }
                    if (!automaticGear)
                    {
                        if (Input.GetKeyDown(RCCSettings.shiftGearUp))
                        {
                            GearShiftUp();
                        }
                        if (Input.GetKeyDown(RCCSettings.shiftGearDown))
                        {
                            GearShiftDown();
                        }
                    }
                    break;
                }
            case RCC_Settings.ControllerType.XBox360One:
                {
                    gasInput = Input.GetAxis(RCCSettings.Xbox_triggerRightInput);
                    brakeInput = Input.GetAxis(RCCSettings.Xbox_triggerLeftInput);
                    if (PlayerPrefs.GetInt("MenuOpen") == 0)
                    {
                        steerInput = Input.GetAxis(RCCSettings.Xbox_horizontalInput);
                    }
                    else
                    {
                        steerInput = Input.GetAxis("emptyaxis");
                    }
                    handbrakeInput = (Input.GetButton(RCCSettings.Xbox_handbrakeKB) ? 1f : 0f);
                    if (ObscuredPrefs.GetInt("BoostQuantity") > 0)
                    {
                        boostInput = ((Input.GetButton(RCCSettings.Xbox_boostKB) || Input.GetKey(RCCSettings.boostKB)) ? 1f : 0f);
                        useNOS = true;
                    }
                    else
                    {
                        boostInput = ((Input.GetKey(RCCSettings.boostKB) || Input.GetButton(RCCSettings.Xbox_boostKB)) ? 1f : 0f);
                        useNOS = false;
                        NOSSound.Stop();
                    }
                    if ((double)brakeInput > 0.2)
                    {
                        string text4 = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0];
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text4) == "Stage (0)" || PlayerPrefs.GetString("BreakBtnUsed" + text4) == "")
                        {
                            GetComponent<Rigidbody>().drag = 0.1f;
                        }
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text4) == "Stage (1)")
                        {
                            GetComponent<Rigidbody>().drag = 0.15f;
                        }
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text4) == "Stage (2)")
                        {
                            GetComponent<Rigidbody>().drag = 0.2f;
                        }
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text4) == "Stage (3)")
                        {
                            GetComponent<Rigidbody>().drag = 0.25f;
                        }
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text4) == "Stage (4)")
                        {
                            GetComponent<Rigidbody>().drag = 0.3f;
                        }
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text4) == "Stage (5)")
                        {
                            GetComponent<Rigidbody>().drag = 0.35f;
                        }
                    }
                    else if ((double)gasInput > 0.5 && (double)brakeInput < 0.2 && brakeInput > 0f)
                    {
                        GetComponent<Rigidbody>().drag = 0.1f;
                    }
                    else if (brakeInput == 0f && GetComponent<Rigidbody>().drag != 0.01f && GetComponent<Rigidbody>().drag < 50f)
                    {
                        GetComponent<Rigidbody>().drag = 0.01f;
                    }
                    if (Input.GetButtonDown(RCCSettings.Xbox_boostKB))
                    {
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine && WaitBoostInt == 0)
                        {
                            WaitBoostInt = 1;
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo0();
                        }
                    }
                    else if (Input.GetButtonUp(RCCSettings.Xbox_boostKB) && GetComponentInParent<RCC_PhotonNetwork>().isMine && WaitBoostInt == 1)
                    {
                        WaitBoostInt = 0;
                        GetComponentInParent<RCC_PhotonNetwork>().Sendinfo0();
                    }
                    if (Input.GetButtonDown(RCCSettings.Xbox_handbrakeKB))
                    {
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine && WaitFamInt == 0)
                        {
                            WaitFamInt = 1;
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo0();
                        }
                    }
                    else if (Input.GetButtonUp(RCCSettings.Xbox_handbrakeKB) && GetComponentInParent<RCC_PhotonNetwork>().isMine && WaitFamInt == 1)
                    {
                        WaitFamInt = 0;
                        GetComponentInParent<RCC_PhotonNetwork>().Sendinfo0();
                    }
                    if (Input.GetButtonDown(RCCSettings.Xbox_trailerAttachDetach))
                    {
                        DetachTrailer();
                    }
                    float axis3 = Input.GetAxis(RCCSettings.Xbox_indicatorKB);
                    float num = Input.GetAxis(RCCSettings.Xbox_hazardIndicatorKB);
                    if (axis3 >= 1f)
                    {
                        highBeamHeadLightsOn = true;
                    }
                    else
                    {
                        highBeamHeadLightsOn = false;
                    }
                    if (highbeamstatesub != highBeamHeadLightsOn)
                    {
                        highbeamstatesub = highBeamHeadLightsOn;
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
                        {
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
                        }
                    }
                    if (axis3 <= -1f && WaitClignoInt == 0)
                    {
                        lowBeamHeadLightsOn = !lowBeamHeadLightsOn;
                        GetComponentInChildren<SRPlayerFonction>().TrailStateBrake(lowBeamHeadLightsOn);
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
                        {
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
                            StartCoroutine(waitClignoXbox());
                        }
                    }
                    if (num >= 0.95f && WaitClignoInt == 0)
                    {
                        StartCoroutine(waitClignoXbox());
                        if (indicatorsOn != IndicatorsOn.All)
                        {
                            indicatorsOn = IndicatorsOn.All;
                        }
                        else
                        {
                            indicatorsOn = IndicatorsOn.Off;
                        }
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
                        {
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
                        }
                    }
                    if (!automaticGear)
                    {
                        if (Input.GetButtonDown(RCCSettings.Xbox_shiftGearUp))
                        {
                            GearShiftUp();
                        }
                        if (Input.GetButtonDown(RCCSettings.Xbox_shiftGearDown))
                        {
                            GearShiftDown();
                        }
                    }
                    break;
                }
            case RCC_Settings.ControllerType.PS4:
                {
                    gasInput = Input.GetAxis("PS4_TriggerRight");
                    if (!string.IsNullOrEmpty(RCC_Settings.Instance.PS4_triggerLeftInput))
                    {
                        brakeInput = Input.GetAxis(RCC_Settings.Instance.PS4_triggerLeftInput);
                    }
                    if (!string.IsNullOrEmpty(RCC_Settings.Instance.PS4_horizontalInput))
                    {
                        steerInput = Input.GetAxis(RCC_Settings.Instance.PS4_horizontalInput);
                    }
                    if (!string.IsNullOrEmpty(RCC_Settings.Instance.PS4_handbrakeKB))
                    {
                        handbrakeInput = (Input.GetButton(RCC_Settings.Instance.PS4_handbrakeKB) ? 1f : 0f);
                    }
                    if (!string.IsNullOrEmpty(RCC_Settings.Instance.PS4_boostKB))
                    {
                        boostInput = (Input.GetButton(RCC_Settings.Instance.PS4_boostKB) ? 1f : 0f);
                    }
                    if ((double)brakeInput > 0.2)
                    {
                        string text3 = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0];
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text3) == "Stage (0)" || PlayerPrefs.GetString("BreakBtnUsed" + text3) == "")
                        {
                            GetComponent<Rigidbody>().drag = 0.1f;
                        }
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text3) == "Stage (1)")
                        {
                            GetComponent<Rigidbody>().drag = 0.15f;
                        }
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text3) == "Stage (2)")
                        {
                            GetComponent<Rigidbody>().drag = 0.2f;
                        }
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text3) == "Stage (3)")
                        {
                            GetComponent<Rigidbody>().drag = 0.25f;
                        }
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text3) == "Stage (4)")
                        {
                            GetComponent<Rigidbody>().drag = 0.3f;
                        }
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text3) == "Stage (5)")
                        {
                            GetComponent<Rigidbody>().drag = 0.35f;
                        }
                    }
                    else if ((double)gasInput > 0.5 && (double)brakeInput < 0.2 && brakeInput > 0f)
                    {
                        GetComponent<Rigidbody>().drag = 0.1f;
                    }
                    else if (brakeInput == 0f && GetComponent<Rigidbody>().drag != 0.01f && GetComponent<Rigidbody>().drag < 50f)
                    {
                        GetComponent<Rigidbody>().drag = 0.01f;
                    }
                    float axis2 = Input.GetAxis(RCCSettingsInstance.PS4_indicatorKB);
                    float num = Input.GetAxis(RCCSettings.PS4_hazardIndicatorKB);
                    if (num >= 1f)
                    {
                        highBeamHeadLightsOn = true;
                    }
                    else
                    {
                        highBeamHeadLightsOn = false;
                    }
                    if (highbeamstatesub != highBeamHeadLightsOn)
                    {
                        highbeamstatesub = highBeamHeadLightsOn;
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
                        {
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
                        }
                    }
                    if (num <= -1f && WaitClignoInt == 0)
                    {
                        lowBeamHeadLightsOn = !lowBeamHeadLightsOn;
                        GetComponentInChildren<SRPlayerFonction>().TrailStateBrake(lowBeamHeadLightsOn);
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
                        {
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
                            StartCoroutine(waitClignoXbox());
                        }
                    }
                    if (axis2 <= -0.95f && WaitClignoInt == 0)
                    {
                        StartCoroutine(waitClignoXbox());
                        if (indicatorsOn != IndicatorsOn.All)
                        {
                            indicatorsOn = IndicatorsOn.All;
                        }
                        else
                        {
                            indicatorsOn = IndicatorsOn.Off;
                        }
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
                        {
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
                        }
                    }
                    if (!automaticGear)
                    {
                        if (!string.IsNullOrEmpty(RCCSettingsInstance.PS4_shiftGearUp) && RCC_InputManager.GetButtonDown(RCCSettings.PS4_shiftGearUp))
                        {
                            GearShiftUp();
                        }
                        if (!string.IsNullOrEmpty(RCCSettingsInstance.PS4_shiftGearDown) && RCC_InputManager.GetButtonDown(RCCSettings.PS4_shiftGearDown))
                        {
                            GearShiftDown();
                        }
                    }
                    break;
                }
            case RCC_Settings.ControllerType.LogitechSteeringWheel:
                {
                    RCC_LogitechSteeringWheel instance = RCC_LogitechSteeringWheel.Instance;
                    useCounterSteering = false;
                    gasInput = Input.GetAxis(RCCSettings.verticalInput);
                    brakeInput = Mathf.Clamp01(0f - Input.GetAxis(RCCSettings.verticalInput));
                    if ((double)brakeInput > 0.2)
                    {
                        string text = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0];
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text) == "Stage (0)" || PlayerPrefs.GetString("BreakBtnUsed" + text) == "")
                        {
                            GetComponent<Rigidbody>().drag = 0.1f;
                        }
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text) == "Stage (1)")
                        {
                            GetComponent<Rigidbody>().drag = 0.15f;
                        }
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text) == "Stage (2)")
                        {
                            GetComponent<Rigidbody>().drag = 0.2f;
                        }
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text) == "Stage (3)")
                        {
                            GetComponent<Rigidbody>().drag = 0.25f;
                        }
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text) == "Stage (4)")
                        {
                            GetComponent<Rigidbody>().drag = 0.3f;
                        }
                        if (PlayerPrefs.GetString("BreakBtnUsed" + text) == "Stage (5)")
                        {
                            GetComponent<Rigidbody>().drag = 0.35f;
                        }
                    }
                    else if ((double)gasInput > 0.5 && (double)brakeInput < 0.2 && brakeInput > 0f)
                    {
                        GetComponent<Rigidbody>().drag = 0.1f;
                    }
                    else if (brakeInput == 0f && GetComponent<Rigidbody>().drag != 0.01f && GetComponent<Rigidbody>().drag < 50f)
                    {
                        GetComponent<Rigidbody>().drag = 0.01f;
                    }
                    boostInput = (RCC_LogitechSteeringWheel.GetKeyPressed(0, RCCSettings.LogiSteeringWheel_boostKB) ? 1f : 0f);
                    handbrakeInput = (RCC_LogitechSteeringWheel.GetKeyPressed(0, RCCSettings.LogiSteeringWheel_handbrakeKB) ? 1f : 0f);
                    if (!instance)
                    {
                        break;
                    }
                    steerInput = instance.inputs.steerInput;
                    clutchInput = instance.inputs.clutchInput;
                    if (RCC_LogitechSteeringWheel.GetKeyTriggered(0, RCCSettingsInstance.LogiSteeringWheel_lowBeamHeadlightsKB))
                    {
                        lowBeamHeadLightsOn = !lowBeamHeadLightsOn;
                    }
                    if (RCC_LogitechSteeringWheel.GetKeyPressed(0, RCCSettings.LogiSteeringWheel_highBeamHeadlightsKB))
                    {
                        highBeamHeadLightsOn = true;
                    }
                    else if (RCC_LogitechSteeringWheel.GetKeyReleased(0, RCCSettings.LogiSteeringWheel_highBeamHeadlightsKB))
                    {
                        highBeamHeadLightsOn = false;
                    }
                    if (RCC_LogitechSteeringWheel.GetKeyTriggered(0, RCCSettings.LogiSteeringWheel_startEngineKB))
                    {
                        KillOrStartEngine();
                    }
                    if (Input.GetAxis("Logitech_DPadHorizontal") > 0f && PlayerPrefs.GetString("ControllerTypeChoose") == "LogitechSteeringWheel")
                    {
                        Debug.Log("CLIIICK");
                    }
                    else if (Input.GetAxis("Logitech_DPadHorizontal") < 0f && PlayerPrefs.GetString("ControllerTypeChoose") == "LogitechSteeringWheel")
                    {
                        Debug.Log("SOUS CLIIICK");
                    }
                    float num = (RCC_LogitechSteeringWheel.GetKeyPressed(0, RCCSettings.LogiSteeringWheel_hazardIndicatorKB) ? 1f : 0f);
                    if (num >= 0.95f && WaitClignoInt == 0)
                    {
                        StartCoroutine(waitClignoXbox());
                        if (indicatorsOn != IndicatorsOn.All)
                        {
                            indicatorsOn = IndicatorsOn.All;
                        }
                        else
                        {
                            indicatorsOn = IndicatorsOn.Off;
                        }
                        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
                        {
                            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
                        }
                    }
                    if (!automaticGear)
                    {
                        if (RCC_LogitechSteeringWheel.GetKeyTriggered(0, RCCSettings.LogiSteeringWheel_shiftGearUp))
                        {
                            GearShiftUp();
                        }
                        if (RCC_LogitechSteeringWheel.GetKeyTriggered(0, RCCSettings.LogiSteeringWheel_shiftGearDown))
                        {
                            GearShiftDown();
                        }
                    }
                    if (Input.GetKeyDown(RCCSettings.NGear))
                    {
                        NGear = true;
                    }
                    if (Input.GetKeyUp(RCCSettings.NGear))
                    {
                        NGear = false;
                    }
                    break;
                }
        }
        if (permanentGas)
        {
            gasInput = 1f;
        }
    }

    private IEnumerator waitClignoXbox()
    {
        WaitClignoInt = 1;
        yield return new WaitForSeconds(0.5f);
        WaitClignoInt = 0;
    }

    private IEnumerator WaitHighbeamclose()
    {
        yield return new WaitForSeconds(1f);
        lightprio = 0;
    }

    private IEnumerator WaitFeuxtempo()
    {
        yield return new WaitForSeconds(1f);
        temporpcfeux = 0;
    }

    private void FixedUpdate()
    {
        if (rigid.velocity.magnitude < 0.01f && Mathf.Abs(_steerInput) < 0.01f && Mathf.Abs(_gasInput) < 0.01f && Mathf.Abs(rigid.angularVelocity.magnitude) < 0.01f)
        {
            isSleeping = true;
        }
        else
        {
            isSleeping = false;
        }
        if (orgMaxSpeed != maxspeed || orgGearShiftingThreshold != gearShiftingThreshold)
        {
            CreateGearCurves();
        }
        if (gears == null || gears.Length == 0)
        {
            MonoBehaviour.print("Gear can not be 0!");
            gears = new Gear[totalGears];
            CreateGearCurves();
        }
        Engine();
        EngineSounds();
        if (canControl)
        {
            GearBox();
            Clutch();
        }
        AntiRollBars();
        DriftVariables();
        RevLimiter();
        Turbo();
        NOS();
        if (useFuelConsumption)
        {
            Fuel();
        }
        if (useEngineHeat)
        {
            EngineHeat();
        }
        if (steeringHelper)
        {
            SteerHelper();
        }
        if (tractionHelper)
        {
            TractionHelper();
        }
        if (angularDragHelper)
        {
            AngularDragHelper();
        }
        if (ESP)
        {
            ESPCheck(FrontLeftWheelCollider.wheelCollider.steerAngle);
        }
        if (RCCSettings.selectedBehaviorType != null && RCCSettings.selectedBehaviorType.applyRelativeTorque && RearLeftWheelCollider.wheelCollider.isGrounded)
        {
            rigid.AddRelativeTorque(Vector3.up * (steerInput * _gasInput * (float)direction) / 1f, ForceMode.Acceleration);
        }
        rigid.centerOfMass = base.transform.InverseTransformPoint(COM.transform.position);
        rigid.AddRelativeForce(Vector3.down * (speed * downForce), ForceMode.Force);
    }

    private void Engine()
    {
        speed = rigid.velocity.magnitude * 3.6f;
        steerAngle = Mathf.Lerp(orgSteerAngle, highspeedsteerAngle, speed / highspeedsteerAngleAtspeed);
        float num = ((wheelTypeChoise == WheelType.FWD) ? (FrontLeftWheelCollider.wheelRPMToSpeed + FrontRightWheelCollider.wheelRPMToSpeed) : (RearLeftWheelCollider.wheelRPMToSpeed + RearRightWheelCollider.wheelRPMToSpeed));
        rawEngineRPM = Mathf.Clamp(Mathf.MoveTowards(rawEngineRPM, maxEngineRPM * 1.1f * Mathf.Clamp01(Mathf.Lerp(0f, 1f, (1f - clutchInput) * (num * (float)direction / 2f / (float)gears[currentGear].maxSpeed)) + (_gasInput * clutchInput + idleInput)), engineInertia * 100f), 0f, maxEngineRPM * 1.1f);
        rawEngineRPM *= _fuelInput;
        engineRPM = Mathf.Lerp(engineRPM, rawEngineRPM, Mathf.Lerp(Time.fixedDeltaTime * 5f, Time.fixedDeltaTime * 50f, rawEngineRPM / maxEngineRPM));
        if (autoReverse)
        {
            canGoReverseNow = true;
        }
        else if (_brakeInput < 0.5f && speed < 5f)
        {
            canGoReverseNow = true;
        }
        else if (_brakeInput > 0f && base.transform.InverseTransformDirection(rigid.velocity).z > 1f)
        {
            canGoReverseNow = false;
        }
        if (GetComponentInParent<RCC_PhotonNetwork>().isMine && cangoreversetemp != canGoReverseNow)
        {
            cangoreversetemp = canGoReverseNow;
            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
            Debug.Log("CAN GO REVERSE");
        }
    }

    private void Audio()
    {
        windSound.volume = Mathf.Lerp(0f, RCCSettings.maxWindSoundVolume, speed / 300f);
        windSound.pitch = UnityEngine.Random.Range(0.9f, 1f);
        if (direction == 1)
        {
            brakeSound.volume = Mathf.Lerp(0f, RCCSettings.maxBrakeSoundVolume, Mathf.Clamp01((FrontLeftWheelCollider.wheelCollider.brakeTorque + FrontRightWheelCollider.wheelCollider.brakeTorque) / (brakeTorque * 2f)) * Mathf.Lerp(0f, 1f, FrontLeftWheelCollider.wheelCollider.rpm / 50f));
        }
        else
        {
            brakeSound.volume = 0f;
        }
    }

    private void ESPCheck(float steering)
    {
        FrontLeftWheelCollider.wheelCollider.GetGroundHit(out WheelHit hit);
        FrontRightWheelCollider.wheelCollider.GetGroundHit(out WheelHit hit2);
        frontSlip = hit.sidewaysSlip + hit2.sidewaysSlip;
        RearLeftWheelCollider.wheelCollider.GetGroundHit(out WheelHit hit3);
        RearRightWheelCollider.wheelCollider.GetGroundHit(out WheelHit hit4);
        rearSlip = hit3.sidewaysSlip + hit4.sidewaysSlip;
        if (Mathf.Abs(frontSlip) >= ESPThreshold)
        {
            underSteering = true;
        }
        else
        {
            underSteering = false;
        }
        if (Mathf.Abs(rearSlip) >= ESPThreshold)
        {
            overSteering = true;
        }
        else
        {
            overSteering = false;
        }
        if (overSteering || underSteering)
        {
            ESPAct = true;
        }
        else
        {
            ESPAct = false;
        }
    }

    private void EngineSounds()
    {
        float num = 0f;
        float num2 = 0f;
        float num3 = 0f;
        num = ((!(engineRPM < maxEngineRPM / 2f)) ? Mathf.Lerp(1f, 0.25f, engineRPM / maxEngineRPM) : Mathf.Lerp(0f, 1f, engineRPM / (maxEngineRPM / 2f)));
        num2 = ((!(engineRPM < maxEngineRPM / 2f)) ? Mathf.Lerp(1f, 0.5f, engineRPM / maxEngineRPM) : Mathf.Lerp(-0.5f, 1f, engineRPM / (maxEngineRPM / 2f)));
        num3 = Mathf.Lerp(-1f, 1f, engineRPM / maxEngineRPM);
        num = Mathf.Clamp01(num) * maxEngineSoundVolume;
        num2 = Mathf.Clamp01(num2) * maxEngineSoundVolume;
        num3 = Mathf.Clamp01(num3) * maxEngineSoundVolume;
        float num4 = Mathf.Clamp(_gasInput, 0f, 1f);
        float pitch = Mathf.Lerp(minEngineSoundPitch, maxEngineSoundPitch, engineRPM / maxEngineRPM) * (engineRunning ? 1f : 0f);
        switch (audioType)
        {
            case AudioType.OneSource:
                engineSoundHigh.volume = num4 * maxEngineSoundVolume;
                engineSoundHigh.pitch = pitch;
                engineSoundHighOff.volume = (1f - num4) * maxEngineSoundVolume;
                engineSoundHighOff.pitch = pitch;
                if ((bool)engineSoundIdle)
                {
                    engineSoundIdle.volume = Mathf.Lerp(engineRunning ? idleEngineSoundVolume : 0f, 0f, engineRPM / maxEngineRPM);
                    engineSoundIdle.pitch = pitch;
                }
                if (!engineSoundHigh.isPlaying)
                {
                    engineSoundHigh.Play();
                }
                if (!engineSoundIdle.isPlaying)
                {
                    engineSoundIdle.Play();
                }
                break;
            case AudioType.TwoSource:
                engineSoundHigh.volume = num3 * num4;
                engineSoundHigh.pitch = pitch;
                engineSoundLow.volume = num * num4;
                engineSoundLow.pitch = pitch;
                engineSoundHighOff.volume = num3 * (1f - num4);
                engineSoundHighOff.pitch = pitch;
                engineSoundLowOff.volume = num * (1f - num4);
                engineSoundLowOff.pitch = pitch;
                if ((bool)engineSoundIdle)
                {
                    engineSoundIdle.volume = Mathf.Lerp(engineRunning ? idleEngineSoundVolume : 0f, 0f, engineRPM / maxEngineRPM);
                    engineSoundIdle.pitch = pitch;
                }
                if (!engineSoundLow.isPlaying)
                {
                    engineSoundLow.Play();
                }
                if (!engineSoundHigh.isPlaying)
                {
                    engineSoundHigh.Play();
                }
                if (!engineSoundIdle.isPlaying)
                {
                    engineSoundIdle.Play();
                }
                break;
            case AudioType.ThreeSource:
                engineSoundHigh.volume = num3 * num4;
                engineSoundHigh.pitch = pitch;
                engineSoundMed.volume = num2 * num4;
                engineSoundMed.pitch = pitch;
                engineSoundLow.volume = num * num4;
                engineSoundLow.pitch = pitch;
                engineSoundHighOff.volume = num3 * (1f - num4);
                engineSoundHighOff.pitch = pitch;
                engineSoundMedOff.volume = num2 * (1f - num4);
                engineSoundMedOff.pitch = pitch;
                engineSoundLowOff.volume = num * (1f - num4);
                engineSoundLowOff.pitch = pitch;
                if ((bool)engineSoundIdle)
                {
                    engineSoundIdle.volume = Mathf.Lerp(engineRunning ? idleEngineSoundVolume : 0f, 0f, engineRPM / maxEngineRPM);
                    engineSoundIdle.pitch = pitch;
                }
                if (!engineSoundLow.isPlaying)
                {
                    engineSoundLow.Play();
                }
                if (!engineSoundMed.isPlaying)
                {
                    engineSoundMed.Play();
                }
                if (!engineSoundHigh.isPlaying)
                {
                    engineSoundHigh.Play();
                }
                if (!engineSoundIdle.isPlaying)
                {
                    engineSoundIdle.Play();
                }
                break;
        }
    }

    private void AntiRollBars()
    {
        float num = 1f;
        float num2 = 1f;
        bool isGrounded = FrontLeftWheelCollider.isGrounded;
        if (isGrounded)
        {
            num = (0f - FrontLeftWheelCollider.transform.InverseTransformPoint(FrontLeftWheelCollider.wheelHit.point).y - FrontLeftWheelCollider.wheelCollider.radius) / FrontLeftWheelCollider.wheelCollider.suspensionDistance;
        }
        bool isGrounded2 = FrontRightWheelCollider.isGrounded;
        if (isGrounded2)
        {
            num2 = (0f - FrontRightWheelCollider.transform.InverseTransformPoint(FrontRightWheelCollider.wheelHit.point).y - FrontRightWheelCollider.wheelCollider.radius) / FrontRightWheelCollider.wheelCollider.suspensionDistance;
        }
        float num3 = (num - num2) * antiRollFrontHorizontal;
        if (isGrounded)
        {
            rigid.AddForceAtPosition(FrontLeftWheelCollider.transform.up * (0f - num3), FrontLeftWheelCollider.transform.position);
        }
        if (isGrounded2)
        {
            rigid.AddForceAtPosition(FrontRightWheelCollider.transform.up * num3, FrontRightWheelCollider.transform.position);
        }
        float num4 = 1f;
        float num5 = 1f;
        bool isGrounded3 = RearLeftWheelCollider.isGrounded;
        if (isGrounded3)
        {
            num4 = (0f - RearLeftWheelCollider.transform.InverseTransformPoint(RearLeftWheelCollider.wheelHit.point).y - RearLeftWheelCollider.wheelCollider.radius) / RearLeftWheelCollider.wheelCollider.suspensionDistance;
        }
        bool isGrounded4 = RearRightWheelCollider.isGrounded;
        if (isGrounded4)
        {
            num5 = (0f - RearRightWheelCollider.transform.InverseTransformPoint(RearRightWheelCollider.wheelHit.point).y - RearRightWheelCollider.wheelCollider.radius) / RearRightWheelCollider.wheelCollider.suspensionDistance;
        }
        float num6 = (num4 - num5) * antiRollRearHorizontal;
        if (isGrounded3)
        {
            rigid.AddForceAtPosition(RearLeftWheelCollider.transform.up * (0f - num6), RearLeftWheelCollider.transform.position);
        }
        if (isGrounded4)
        {
            rigid.AddForceAtPosition(RearRightWheelCollider.transform.up * num6, RearRightWheelCollider.transform.position);
        }
        float num7 = (num - num4) * antiRollVertical;
        if (isGrounded)
        {
            rigid.AddForceAtPosition(FrontLeftWheelCollider.transform.up * (0f - num7), FrontLeftWheelCollider.transform.position);
        }
        if (isGrounded3)
        {
            rigid.AddForceAtPosition(RearLeftWheelCollider.transform.up * num7, RearLeftWheelCollider.transform.position);
        }
        float num8 = (num2 - num5) * antiRollVertical;
        if (isGrounded2)
        {
            rigid.AddForceAtPosition(FrontRightWheelCollider.transform.up * (0f - num8), FrontRightWheelCollider.transform.position);
        }
        if (isGrounded4)
        {
            rigid.AddForceAtPosition(RearRightWheelCollider.transform.up * num8, RearRightWheelCollider.transform.position);
        }
    }

    private void SteerHelper()
    {
        if (!steeringDirection || !velocityDirection)
        {
            if (!steeringDirection)
            {
                GameObject gameObject = new GameObject("Steering Direction");
                gameObject.transform.SetParent(base.transform, worldPositionStays: false);
                steeringDirection = gameObject.transform;
                gameObject.transform.localPosition = new Vector3(1f, 2f, 0f);
                gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 3f);
            }
            if (!velocityDirection)
            {
                GameObject gameObject2 = new GameObject("Velocity Direction");
                gameObject2.transform.SetParent(base.transform, worldPositionStays: false);
                velocityDirection = gameObject2.transform;
                gameObject2.transform.localPosition = new Vector3(-1f, 2f, 0f);
                gameObject2.transform.localScale = new Vector3(0.1f, 0.1f, 3f);
            }
            return;
        }
        for (int i = 0; i < allWheelColliders.Length; i++)
        {
            allWheelColliders[i].wheelCollider.GetGroundHit(out WheelHit hit);
            if (hit.normal == Vector3.zero)
            {
                return;
            }
        }
        velocityAngle = rigid.angularVelocity.y * Mathf.Clamp(base.transform.InverseTransformDirection(rigid.velocity).z, -1f, 1f) * 57.29578f;
        velocityDirection.localRotation = Quaternion.Lerp(velocityDirection.localRotation, Quaternion.AngleAxis(Mathf.Clamp(velocityAngle / 3f, -45f, 45f), Vector3.up), Time.fixedDeltaTime * 20f);
        steeringDirection.localRotation = Quaternion.Euler(0f, FrontLeftWheelCollider.wheelCollider.steerAngle, 0f);
        int num = 1;
        num = ((steeringDirection.localRotation.y > velocityDirection.localRotation.y) ? 1 : (-1));
        float num2 = Quaternion.Angle(velocityDirection.localRotation, steeringDirection.localRotation) * (float)num;
        rigid.AddRelativeTorque(Vector3.up * (num2 * (Mathf.Clamp(base.transform.InverseTransformDirection(rigid.velocity).z, -10f, 10f) / 1000f) * steerHelperAngularVelStrength), ForceMode.VelocityChange);
        if (Mathf.Abs(oldRotation - base.transform.eulerAngles.y) < 10f)
        {
            Quaternion quaternion = Quaternion.AngleAxis((base.transform.eulerAngles.y - oldRotation) * (steerHelperLinearVelStrength / 2f), Vector3.up);
            rigid.velocity = quaternion * rigid.velocity;
        }
        oldRotation = base.transform.eulerAngles.y;
    }

    private void TractionHelper()
    {
        Vector3 velocity = rigid.velocity;
        velocity -= base.transform.up * Vector3.Dot(velocity, base.transform.up);
        velocity.Normalize();
        angle = 0f - Mathf.Asin(Vector3.Dot(Vector3.Cross(base.transform.forward, velocity), base.transform.up));
        angularVelo = rigid.angularVelocity.y;
        if (angle * FrontLeftWheelCollider.wheelCollider.steerAngle < 0f)
        {
            FrontLeftWheelCollider.tractionHelpedSidewaysStiffness = 1f - Mathf.Clamp01(tractionHelperStrength * Mathf.Abs(angularVelo));
        }
        else
        {
            FrontLeftWheelCollider.tractionHelpedSidewaysStiffness = 1f;
        }
        if (angle * FrontRightWheelCollider.wheelCollider.steerAngle < 0f)
        {
            FrontRightWheelCollider.tractionHelpedSidewaysStiffness = 1f - Mathf.Clamp01(tractionHelperStrength * Mathf.Abs(angularVelo));
        }
        else
        {
            FrontRightWheelCollider.tractionHelpedSidewaysStiffness = 1f;
        }
    }

    private void AngularDragHelper()
    {
        rigid.angularDrag = Mathf.Lerp(0f, 10f, speed * angularDragHelperStrength / 1000f);
    }

    private void Clutch()
    {
        if (engineRunning)
        {
            idleInput = Mathf.Lerp(1f, 0f, engineRPM / minEngineRPM);
        }
        else
        {
            idleInput = 0f;
        }
        if (currentGear == 0)
        {
            if (useClutchMarginAtFirstGear)
            {
                if (launched >= 0.25f)
                {
                    clutchInput = Mathf.Lerp(clutchInput, Mathf.Lerp(1f, Mathf.Lerp(clutchInertia, 0f, (RearLeftWheelCollider.wheelRPMToSpeed + RearRightWheelCollider.wheelRPMToSpeed) / 2f / (float)gears[0].targetSpeedForNextGear), Mathf.Abs(_gasInput)), Time.fixedDeltaTime * 5f);
                }
                else
                {
                    clutchInput = Mathf.Lerp(clutchInput, 1f / speed, Time.fixedDeltaTime * 5f);
                }
            }
            else
            {
                clutchInput = Mathf.Lerp(clutchInput, Mathf.Lerp(1f, Mathf.Lerp(clutchInertia, 0f, (RearLeftWheelCollider.wheelRPMToSpeed + RearRightWheelCollider.wheelRPMToSpeed) / 2f / (float)gears[0].targetSpeedForNextGear), Mathf.Abs(_gasInput)), Time.fixedDeltaTime * 5f);
            }
        }
        else if (changingGear)
        {
            clutchInput = Mathf.Lerp(clutchInput, 1f, Time.fixedDeltaTime * 5f);
        }
        else
        {
            clutchInput = Mathf.Lerp(clutchInput, 0f, Time.fixedDeltaTime * 5f);
        }
        if (cutGas || handbrakeInput >= 0.1f)
        {
            clutchInput = 1f;
        }
        if (NGear)
        {
            clutchInput = 1f;
        }
        clutchInput = Mathf.Clamp01(clutchInput);
    }

    private void GearBox()
    {
        if (!externalController)
        {
            if (brakeInput > 0.9f && base.transform.InverseTransformDirection(rigid.velocity).z < 1f && canGoReverseNow && automaticGear && !semiAutomaticGear && !changingGear && direction != -1)
            {
                StartCoroutine(ChangeGear(-1));
            }
            else if (brakeInput < 0.1f && base.transform.InverseTransformDirection(rigid.velocity).z > -1f && direction == -1 && !changingGear && automaticGear && !semiAutomaticGear)
            {
                StartCoroutine(ChangeGear(0));
            }
        }
        if (automaticGear)
        {
            if (currentGear < gears.Length - 1 && !changingGear && speed >= (float)gears[currentGear].targetSpeedForNextGear * 0.9f && FrontLeftWheelCollider.wheelCollider.rpm > 0f)
            {
                if (!semiAutomaticGear)
                {
                    StartCoroutine(ChangeGear(currentGear + 1));
                }
                else if (semiAutomaticGear && direction != -1)
                {
                    StartCoroutine(ChangeGear(currentGear + 1));
                }
            }
            if (currentGear > 0 && !changingGear && speed < (float)gears[currentGear - 1].targetSpeedForNextGear * 0.7f && direction != -1)
            {
                StartCoroutine(ChangeGear(currentGear - 1));
            }
        }
        if (direction == -1)
        {
            if (!reversingSound.isPlaying)
            {
                reversingSound.Play();
            }
            reversingSound.volume = Mathf.Lerp(0f, 1f, speed / (float)gears[0].targetSpeedForNextGear);
            reversingSound.pitch = reversingSound.volume;
        }
        else
        {
            if (reversingSound.isPlaying)
            {
                reversingSound.Stop();
            }
            reversingSound.volume = 0f;
            reversingSound.pitch = 0f;
        }
    }

    public IEnumerator ChangeGear(int gear)
    {
        changingGear = true;
        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
        {
            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
        }
        if (RCCSettings.useTelemetry)
        {
            MonoBehaviour.print("Shifted to: " + gear);
        }
        if (gearShiftingClips.Length != 0)
        {
            gearShiftingSound = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, gearSoundPosition, "Gear Shifting AudioSource", 1f, 5f, RCCSettings.maxGearShiftingSoundVolume, gearShiftingClips[UnityEngine.Random.Range(0, gearShiftingClips.Length)], loop: false, playNow: true, destroyAfterFinished: true);
            if (!gearShiftingSound.isPlaying)
            {
                gearShiftingSound.Play();
            }
        }
        yield return new WaitForSeconds(gearShiftingDelay);
        if (gear == -1)
        {
            currentGear = 0;
            if (!NGear)
            {
                direction = -1;
            }
            else
            {
                direction = 0;
            }
        }
        else
        {
            currentGear = gear;
            if (!NGear)
            {
                direction = 1;
            }
            else
            {
                direction = 0;
            }
        }
        changingGear = false;
        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
        {
            GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
        }
    }

    public void GearShiftUp()
    {
        if (currentGear < gears.Length - 1 && !changingGear)
        {
            if (direction != -1)
            {
                StartCoroutine(ChangeGear(currentGear + 1));
            }
            else
            {
                StartCoroutine(ChangeGear(0));
            }
        }
    }

    public void GearShiftDown()
    {
        if (currentGear >= 0)
        {
            StartCoroutine(ChangeGear(currentGear - 1));
        }
    }

    private void RevLimiter()
    {
        if (useRevLimiter && engineRPM >= maxEngineRPM)
        {
            cutGas = true;
        }
        else if (engineRPM < maxEngineRPM * 0.95f)
        {
            cutGas = false;
        }
    }

    private void NOS()
    {
        if (!useNOS)
        {
            return;
        }
        if (!NOSSound)
        {
            NOSSound = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, exhaustSoundPosition, "NOS Sound AudioSource", 5f, 10f, 1f, NOSClip, loop: true, playNow: false, destroyAfterFinished: false);
        }
        if (!blowSound)
        {
            blowSound = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, exhaustSoundPosition, "NOS Blow", 1f, 10f, 1f, null, loop: false, playNow: false, destroyAfterFinished: false);
        }
        if (boostInput >= 0.8f && _gasInput >= 0.8f && NoS > 5f)
        {
            NoS -= NoSConsumption * Time.fixedDeltaTime;
            NoSRegenerateTime = 0f;
            if (!NOSSound.isPlaying)
            {
                NOSSound.Play();
            }
            return;
        }
        if (NoS < 100f && NoSRegenerateTime > 3f)
        {
            NoS += NoSConsumption / 1.5f * Time.fixedDeltaTime;
        }
        NoSRegenerateTime += Time.fixedDeltaTime;
        if (NOSSound.isPlaying)
        {
            NOSSound.Stop();
            blowSound.clip = RCCSettings.blowoutClip[UnityEngine.Random.Range(0, RCCSettings.blowoutClip.Length)];
            blowSound.Play();
        }
    }

    private void Turbo()
    {
        if (useTurbo)
        {
            if (!turboSound)
            {
                turboSound = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, turboSoundPosition, "Turbo Sound AudioSource", 0.1f, 0.5f, 0f, turboClip, loop: true, playNow: true, destroyAfterFinished: false);
                RCC_CreateAudioSource.NewHighPassFilter(turboSound, 10000f, 10);
            }
            turboBoost = Mathf.Lerp(turboBoost, Mathf.Clamp(Mathf.Pow(_gasInput, 10f) * 30f + Mathf.Pow(engineRPM / maxEngineRPM, 10f) * 30f, 0f, 30f), Time.fixedDeltaTime * 10f);
            if (turboBoost >= 25f && turboBoost < turboSound.volume * 30f && !blowSound.isPlaying)
            {
                blowSound.clip = RCCSettings.blowoutClip[UnityEngine.Random.Range(0, RCCSettings.blowoutClip.Length)];
                blowSound.Play();
            }
            turboSound.volume = Mathf.Lerp(turboSound.volume, turboBoost / 30f, Time.fixedDeltaTime * 5f);
            turboSound.pitch = Mathf.Lerp(Mathf.Clamp(turboSound.pitch, 2f, 3f), turboBoost / 30f * 2f, Time.fixedDeltaTime * 5f);
        }
    }

    private void Fuel()
    {
        fuelTank -= engineRPM / 10000f * fuelConsumptionRate * Time.fixedDeltaTime;
        fuelTank = Mathf.Clamp(fuelTank, 0f, fuelTankCapacity);
    }

    private void EngineHeat()
    {
        engineHeat += engineRPM / 10000f * engineHeatRate * Time.fixedDeltaTime;
        if (engineHeat > engineCoolingWaterThreshold)
        {
            engineHeat -= engineCoolRate * Time.fixedDeltaTime;
        }
        engineHeat -= engineCoolRate / 10f * Time.fixedDeltaTime;
        engineHeat = Mathf.Clamp(engineHeat, 15f, 120f);
    }

    private void DriftVariables()
    {
        RearRightWheelCollider.wheelCollider.GetGroundHit(out WheelHit hit);
        if (Mathf.Abs(hit.sidewaysSlip) > 0.25f)
        {
            driftingNow = true;
        }
        else
        {
            driftingNow = false;
        }
        if (speed > 10f)
        {
            driftAngle = hit.sidewaysSlip * 0.75f;
        }
        else
        {
            driftAngle = 0f;
        }
    }

    private void ResetCar()
    {
        if (speed < 5f && !rigid.isKinematic && RCCSettings.autoReset && base.transform.eulerAngles.z < 300f && base.transform.eulerAngles.z > 60f)
        {
            resetTime += Time.deltaTime;
            if (resetTime > 3f)
            {
                base.transform.rotation = Quaternion.Euler(0f, base.transform.eulerAngles.y, 0f);
                base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 3f, base.transform.position.z);
                resetTime = 0f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length < 1 || collision.relativeVelocity.magnitude < minimumCollisionForce)
        {
            return;
        }
        if (RCC_CarControllerV3.OnRCCPlayerCollision != null && this == RCC_SceneManager.Instance.activePlayerVehicle)
        {
            RCC_CarControllerV3.OnRCCPlayerCollision(this, collision);
        }
        if (useDamage && ((1 << collision.gameObject.layer) & (int)damageFilter) != 0)
        {
            CollisionParticles(collision.contacts[0].point);
            Vector3 relativeVelocity = collision.relativeVelocity;
            relativeVelocity *= 1f - Mathf.Abs(Vector3.Dot(base.transform.up, collision.contacts[0].normal));
            float num = Mathf.Abs(Vector3.Dot(collision.contacts[0].normal, relativeVelocity.normalized));
            if (relativeVelocity.magnitude * num >= minimumCollisionForce)
            {
                repaired = false;
                localVector = base.transform.InverseTransformDirection(relativeVelocity) * (damageMultiplier / 50f);
                if (originalMeshData == null)
                {
                    LoadOriginalMeshData();
                }
                for (int i = 0; i < deformableMeshFilters.Length; i++)
                {
                    DeformMesh(deformableMeshFilters[i].mesh, originalMeshData[i].meshVerts, collision, num, deformableMeshFilters[i].transform, rot);
                }
            }
        }
        if (crashClips.Length == 0)
        {
            return;
        }
        if (GetComponentInParent<RCC_PhotonNetwork>().isMine)
        {
            if ((speed <= 40f && PlayerPrefs.GetInt("Vibration") == 0 && PlayerPrefs.GetString("ControllerTypeChoose") == "Xbox360One") || (speed <= 40f && PlayerPrefs.GetInt("Vibration") == 0 && PlayerPrefs.GetString("ControllerTypeChoose") == "LogitechSteeringWheel"))
            {
                GamePad.SetVibration(playerIndex, 0.3f, 0.3f);
                VibTime = 0.3f;
                StopCoroutine(UnlockViration());
                StartCoroutine(UnlockViration());
            }
            else if ((speed <= 80f && PlayerPrefs.GetInt("Vibration") == 0 && PlayerPrefs.GetString("ControllerTypeChoose") == "Xbox360One") || (speed <= 80f && PlayerPrefs.GetInt("Vibration") == 0 && PlayerPrefs.GetString("ControllerTypeChoose") == "LogitechSteeringWheel"))
            {
                GamePad.SetVibration(playerIndex, 0.5f, 0.5f);
                VibTime = 0.35f;
                StopCoroutine(UnlockViration());
                StartCoroutine(UnlockViration());
            }
            else if ((speed <= 120f && PlayerPrefs.GetInt("Vibration") == 0 && PlayerPrefs.GetString("ControllerTypeChoose") == "Xbox360One") || (speed <= 120f && PlayerPrefs.GetInt("Vibration") == 0 && PlayerPrefs.GetString("ControllerTypeChoose") == "LogitechSteeringWheel"))
            {
                GamePad.SetVibration(playerIndex, 0.7f, 0.7f);
                VibTime = 0.4f;
                StopCoroutine(UnlockViration());
                StartCoroutine(UnlockViration());
            }
            else if ((speed < 180f && PlayerPrefs.GetInt("Vibration") == 0 && PlayerPrefs.GetString("ControllerTypeChoose") == "Xbox360One") || (speed < 180f && PlayerPrefs.GetInt("Vibration") == 0 && PlayerPrefs.GetString("ControllerTypeChoose") == "LogitechSteeringWheel"))
            {
                GamePad.SetVibration(playerIndex, 0.85f, 0.85f);
                VibTime = 0.5f;
                StopCoroutine(UnlockViration());
                StartCoroutine(UnlockViration());
            }
            else if ((speed >= 180f && PlayerPrefs.GetInt("Vibration") == 0 && PlayerPrefs.GetString("ControllerTypeChoose") == "Xbox360One") || (speed >= 180f && PlayerPrefs.GetInt("Vibration") == 0 && PlayerPrefs.GetString("ControllerTypeChoose") == "LogitechSteeringWheel"))
            {
                GamePad.SetVibration(playerIndex, 0.95f, 0.95f);
                VibTime = 0.6f;
                StopCoroutine(UnlockViration());
                StartCoroutine(UnlockViration());
            }
        }
        if (collision.contacts[0].thisCollider.gameObject.transform != base.transform.parent)
        {
            crashSound = RCC_CreateAudioSource.NewAudioSource(audioMixer, base.gameObject, "Crash Sound AudioSource", 5f, 20f, RCCSettings.maxCrashSoundVolume, crashClips[UnityEngine.Random.Range(0, crashClips.Length)], loop: false, playNow: true, destroyAfterFinished: true);
            if (!crashSound.isPlaying)
            {
                crashSound.Play();
            }
        }
    }

    private IEnumerator UnlockViration()
    {
        yield return new WaitForSeconds(VibTime);
        GamePad.SetVibration(playerIndex, 0f, 0f);
    }

    private void OnDrawGizmos()
    {
    }

    public void PreviewSmokeParticle(bool state)
    {
        canControl = state;
        permanentGas = state;
        rigid.isKinematic = state;
    }

    public void DetachTrailer()
    {
        if ((bool)attachedTrailer)
        {
            attachedTrailer.DetachTrailer();
        }
    }

    private void OnDestroy()
    {
        if (RCC_CarControllerV3.OnRCCPlayerDestroyed != null)
        {
            RCC_CarControllerV3.OnRCCPlayerDestroyed(this);
        }
        if (canControl && (bool)base.gameObject.GetComponentInChildren<RCC_Camera>())
        {
            base.gameObject.GetComponentInChildren<RCC_Camera>().transform.SetParent(null);
        }
    }

    public void SetCanControl(bool state)
    {
        canControl = state;
    }

    public void SetEngine(bool state)
    {
        if (state)
        {
            StartEngine();
        }
        else
        {
            KillEngine();
        }
    }

    private void OnDisable()
    {
        RCC_SceneManager.OnBehaviorChanged -= CheckBehavior;
    }
}
