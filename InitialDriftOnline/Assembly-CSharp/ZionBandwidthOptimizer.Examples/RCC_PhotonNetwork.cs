using Photon.Pun;
using UnityEngine;

namespace ZionBandwidthOptimizer.Examples;

[RequireComponent(typeof(PhotonView))]
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Network/Photon/RCC Photon Network")]
public class RCC_PhotonNetwork : MonoBehaviourPunCallbacks, IPunObservable
{
	public bool isMine;

	private RCC_CarControllerV3 carController;

	private RCC_WheelCollider[] wheelColliders;

	private Rigidbody rigid;

	private Vector3 correctPlayerPos;

	private Quaternion correctPlayerRot;

	private Vector3 currentVelocity;

	private float updateTime;

	private float gasInput;

	private float brakeInput;

	private float steerInput;

	private float handbrakeInput;

	private float boostInput;

	private float clutchInput;

	private float idleInput;

	private int gear;

	private int direction = 1;

	private bool changingGear;

	private bool semiAutomaticGear;

	private float fuelInput = 1f;

	private bool engineRunning;

	private float[] cambers;

	private bool applyEngineTorqueToExtraRearWheelColliders;

	private RCC_CarControllerV3.WheelType _wheelTypeChoise;

	private float biasedWheelTorque;

	private bool canGoReverseNow;

	private float engineTorque;

	private float brakeTorque;

	private float minEngineRPM;

	private float maxEngineRPM;

	private float engineInertia;

	private bool useRevLimiter;

	private bool useExhaustFlame;

	private bool useClutchMarginAtFirstGear;

	private float highspeedsteerAngle;

	private float highspeedsteerAngleAtspeed;

	private float antiRollFrontHorizontal;

	private float antiRollRearHorizontal;

	private float antiRollVertical;

	private float maxspeed;

	private float engineHeat;

	private float engineHeatMultiplier;

	private int totalGears;

	private float gearShiftingDelay;

	private float gearShiftingThreshold;

	private float clutchInertia;

	private bool NGear;

	private float launched;

	private bool ABS;

	private bool TCS;

	private bool ESP;

	private bool steeringHelper;

	private bool tractionHelper;

	private bool applyCounterSteering;

	private bool useNOS;

	private bool useTurbo;

	private bool lowBeamHeadLightsOn;

	private bool highBeamHeadLightsOn;

	private RCC_CarControllerV3.IndicatorsOn indicatorsOn;

	private object bitPacker;

	private int packedSize;

	private byte[] buffer;

	private Vector3 Jack;

	private Quaternion Jack2;

	private int IndiSend;

	private int IndiSend2;

	private int IndiSend0;

	private int IndiRecep;

	private int IndiRecep2;

	private int IndiRecep0;

	private void Start()
	{
		IndiSend = 0;
		IndiRecep = 0;
		carController = GetComponent<RCC_CarControllerV3>();
		wheelColliders = GetComponentsInChildren<RCC_WheelCollider>();
		cambers = new float[wheelColliders.Length];
		rigid = GetComponent<Rigidbody>();
		if (!base.gameObject.GetComponent<PhotonView>().ObservedComponents.Contains(this))
		{
			base.gameObject.GetComponent<PhotonView>().ObservedComponents.Add(this);
		}
		base.gameObject.GetComponent<PhotonView>().Synchronization = ViewSynchronization.Unreliable;
		GetValues();
		if (base.photonView.IsMine)
		{
			carController.externalController = false;
			carController.canControl = true;
		}
		else
		{
			carController.externalController = true;
			carController.canControl = false;
		}
		base.gameObject.name = base.gameObject.name + base.photonView.ViewID;
		PhotonNetwork.SendRate = 20;
		PhotonNetwork.SerializationRate = 20;
	}

	private void GetValues()
	{
		correctPlayerPos = base.transform.position;
		correctPlayerRot = base.transform.rotation;
		gasInput = carController.gasInput;
		brakeInput = carController.brakeInput;
		steerInput = carController.steerInput;
		handbrakeInput = carController.handbrakeInput;
		boostInput = carController.boostInput;
		clutchInput = carController.clutchInput;
		idleInput = carController.idleInput;
		gear = carController.currentGear;
		direction = carController.direction;
		changingGear = carController.changingGear;
		semiAutomaticGear = carController.semiAutomaticGear;
		fuelInput = carController.fuelInput;
		engineRunning = carController.engineRunning;
		lowBeamHeadLightsOn = carController.lowBeamHeadLightsOn;
		highBeamHeadLightsOn = carController.highBeamHeadLightsOn;
		indicatorsOn = carController.indicatorsOn;
		for (int i = 0; i < wheelColliders.Length; i++)
		{
			cambers[i] = wheelColliders[i].camber;
		}
		applyEngineTorqueToExtraRearWheelColliders = carController.applyEngineTorqueToExtraRearWheelColliders;
		_wheelTypeChoise = carController.wheelTypeChoise;
		biasedWheelTorque = carController.biasedWheelTorque;
		canGoReverseNow = carController.canGoReverseNow;
		engineTorque = carController.engineTorque;
		brakeTorque = carController.brakeTorque;
		minEngineRPM = carController.minEngineRPM;
		maxEngineRPM = carController.maxEngineRPM;
		engineInertia = carController.engineInertia;
		useRevLimiter = carController.useRevLimiter;
		useExhaustFlame = carController.useExhaustFlame;
		useClutchMarginAtFirstGear = carController.useClutchMarginAtFirstGear;
		highspeedsteerAngle = carController.highspeedsteerAngle;
		highspeedsteerAngleAtspeed = carController.highspeedsteerAngleAtspeed;
		antiRollFrontHorizontal = carController.antiRollFrontHorizontal;
		antiRollRearHorizontal = carController.antiRollRearHorizontal;
		antiRollVertical = carController.antiRollVertical;
		maxspeed = carController.maxspeed;
		engineHeat = carController.engineHeat;
		engineHeatMultiplier = carController.engineHeatRate;
		totalGears = carController.totalGears;
		gearShiftingDelay = carController.gearShiftingDelay;
		gearShiftingThreshold = carController.gearShiftingThreshold;
		clutchInertia = carController.clutchInertia;
		NGear = carController.NGear;
		launched = carController.launched;
		ABS = carController.ABS;
		TCS = carController.TCS;
		ESP = carController.ESP;
		steeringHelper = carController.steeringHelper;
		tractionHelper = carController.tractionHelper;
		applyCounterSteering = carController.applyCounterSteering;
		useNOS = carController.useNOS;
		useTurbo = carController.useTurbo;
	}

	private void FixedUpdate()
	{
		if (!carController)
		{
			return;
		}
		isMine = base.photonView.IsMine;
		carController.externalController = !isMine;
		carController.canControl = isMine;
		if (!isMine)
		{
			Vector3 b = correctPlayerPos + currentVelocity * (Time.time - updateTime);
			if (Vector3.Distance(base.transform.position, correctPlayerPos) < 15f)
			{
				base.transform.position = Vector3.Lerp(base.transform.position, b, Time.deltaTime * 5f);
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, correctPlayerRot, Time.deltaTime * 5f);
			}
			else
			{
				base.transform.position = correctPlayerPos;
				base.transform.rotation = correctPlayerRot;
			}
			carController.gasInput = gasInput;
			carController.brakeInput = brakeInput;
			carController.steerInput = steerInput;
			carController.clutchInput = clutchInput;
			carController.idleInput = idleInput;
			carController.currentGear = gear;
			carController.indicatorsOn = indicatorsOn;
			carController.engineTorque = engineTorque;
			carController.highspeedsteerAngle = highspeedsteerAngle;
			carController.highspeedsteerAngleAtspeed = highspeedsteerAngleAtspeed;
			carController.engineHeat = engineHeat;
			carController.engineHeatRate = engineHeatMultiplier;
			carController.launched = launched;
		}
	}

	public void Sendinfo0()
	{
		string text = base.gameObject.name;
		float num = GetComponent<RCC_CarControllerV3>().handbrakeInput;
		float num2 = GetComponent<RCC_CarControllerV3>().boostInput;
		bool flag = GetComponent<RCC_CarControllerV3>().canGoReverseNow;
		GetComponent<PhotonView>().RPC("RecepInfo0", RpcTarget.Others, text, num, num2, flag);
	}

	[PunRPC]
	public void RecepInfo0(string cible, float handbrakeInput, float boostInput, bool canGoReverseNow)
	{
		GameObject gameObject = GameObject.Find(cible);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<RCC_CarControllerV3>().handbrakeInput = handbrakeInput;
			gameObject.GetComponent<RCC_CarControllerV3>().boostInput = boostInput;
			gameObject.GetComponent<RCC_CarControllerV3>().canGoReverseNow = canGoReverseNow;
		}
	}

	public void Sendinfo()
	{
		string text = base.gameObject.name;
		bool flag = GetComponent<RCC_CarControllerV3>().changingGear;
		bool flag2 = GetComponent<RCC_CarControllerV3>().engineRunning;
		bool flag3 = GetComponent<RCC_CarControllerV3>().lowBeamHeadLightsOn;
		bool flag4 = GetComponent<RCC_CarControllerV3>().highBeamHeadLightsOn;
		bool nGear = GetComponent<RCC_CarControllerV3>().NGear;
		_ = GetComponent<RCC_CarControllerV3>().direction;
		GetComponent<PhotonView>().RPC("RecepInfo", RpcTarget.Others, text, flag, flag2, flag3, flag4, nGear, direction);
	}

	[PunRPC]
	public void RecepInfo(string cible, bool changingGear, bool engineRunning, bool lowBeamHeadLightsOn, bool highBeamHeadLightsOn, bool NGear, int direction)
	{
		GameObject gameObject = GameObject.Find(cible);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<RCC_CarControllerV3>().changingGear = changingGear;
			gameObject.GetComponent<RCC_CarControllerV3>().engineRunning = engineRunning;
			gameObject.GetComponent<RCC_CarControllerV3>().lowBeamHeadLightsOn = lowBeamHeadLightsOn;
			gameObject.GetComponent<RCC_CarControllerV3>().highBeamHeadLightsOn = highBeamHeadLightsOn;
			gameObject.GetComponent<RCC_CarControllerV3>().NGear = NGear;
			gameObject.GetComponent<RCC_CarControllerV3>().direction = direction;
		}
	}

	public void Sendinfo2()
	{
		string text = base.gameObject.name;
		float num = GetComponent<RCC_CarControllerV3>().brakeTorque;
		bool flag = GetComponent<RCC_CarControllerV3>().useRevLimiter;
		bool flag2 = GetComponent<RCC_CarControllerV3>().useExhaustFlame;
		bool flag3 = GetComponent<RCC_CarControllerV3>().useClutchMarginAtFirstGear;
		float num2 = GetComponent<RCC_CarControllerV3>().maxspeed;
		bool flag4 = GetComponent<RCC_CarControllerV3>().useNOS;
		bool flag5 = GetComponent<RCC_CarControllerV3>().useTurbo;
		float num3 = GetComponent<RCC_CarControllerV3>().engineTorque;
		bool flag6 = GetComponent<RCC_CarControllerV3>().applyCounterSteering;
		GetComponent<PhotonView>().RPC("RecepInfo2", RpcTarget.Others, text, num, flag, flag2, flag3, num2, flag4, flag5, num3, flag6);
	}

	[PunRPC]
	public void RecepInfo2(string cible, float brakeTorque, bool useRevLimiter, bool useExhaustFlame, bool useClutchMarginAtFirstGear, float maxspeed, bool useNOS, bool useTurbo, float engineTorque, bool applyCounterSteering)
	{
		GameObject gameObject = GameObject.Find(cible);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<RCC_CarControllerV3>().brakeTorque = brakeTorque;
			gameObject.GetComponent<RCC_CarControllerV3>().useRevLimiter = useRevLimiter;
			gameObject.GetComponent<RCC_CarControllerV3>().useExhaustFlame = useExhaustFlame;
			gameObject.GetComponent<RCC_CarControllerV3>().useClutchMarginAtFirstGear = useClutchMarginAtFirstGear;
			gameObject.GetComponent<RCC_CarControllerV3>().maxspeed = maxspeed;
			gameObject.GetComponent<RCC_CarControllerV3>().useNOS = useNOS;
			gameObject.GetComponent<RCC_CarControllerV3>().useTurbo = useTurbo;
			gameObject.GetComponent<RCC_CarControllerV3>().engineTorque = engineTorque;
			gameObject.GetComponent<RCC_CarControllerV3>().applyCounterSteering = applyCounterSteering;
		}
	}

	public void Sendinfo3()
	{
		string text = base.gameObject.name;
		wheelColliders = GetComponentsInChildren<RCC_WheelCollider>();
		float num = 0.2f;
		float num2 = 0.2f;
		float num3 = 0.2f;
		float num4 = 0.2f;
		for (int i = 0; i < wheelColliders.Length; i++)
		{
			num = wheelColliders[0].camber;
			num2 = wheelColliders[1].camber;
			num3 = wheelColliders[2].camber;
			num4 = wheelColliders[3].camber;
		}
		float suspensionDistance = GetComponent<RCC_CarControllerV3>().RearLeftWheelCollider.wheelCollider.suspensionDistance;
		float suspensionDistance2 = GetComponent<RCC_CarControllerV3>().FrontLeftWheelCollider.wheelCollider.suspensionDistance;
		GetComponent<PhotonView>().RPC("RecepInfo3", RpcTarget.Others, text, num, num2, num3, num4, suspensionDistance2, suspensionDistance);
	}

	[PunRPC]
	public void RecepInfo3(string cible, float camberRoue1, float camberRoue2, float camberRoue3, float camberRoue4, float SusAV, float susAR)
	{
		GameObject gameObject = GameObject.Find(cible);
		RCC_WheelCollider[] componentsInChildren = gameObject.GetComponentsInChildren<RCC_WheelCollider>();
		if ((bool)gameObject)
		{
			for (int i = 0; i < wheelColliders.Length; i++)
			{
				componentsInChildren[0].camber = camberRoue1;
				componentsInChildren[1].camber = camberRoue2;
				componentsInChildren[2].camber = camberRoue3;
				componentsInChildren[3].camber = camberRoue4;
			}
			gameObject.GetComponent<RCC_CarControllerV3>().RearLeftWheelCollider.wheelCollider.suspensionDistance = susAR;
			gameObject.GetComponent<RCC_CarControllerV3>().RearRightWheelCollider.wheelCollider.suspensionDistance = susAR;
			gameObject.GetComponent<RCC_CarControllerV3>().FrontLeftWheelCollider.wheelCollider.suspensionDistance = SusAV;
			gameObject.GetComponent<RCC_CarControllerV3>().FrontRightWheelCollider.wheelCollider.suspensionDistance = SusAV;
		}
	}

	public void Sendinfo4()
	{
		string text = base.gameObject.name;
		float num = GetComponent<RCC_CarControllerV3>().antiRollFrontHorizontal;
		float num2 = GetComponent<RCC_CarControllerV3>().antiRollRearHorizontal;
		GetComponent<PhotonView>().RPC("RecepInfo4", RpcTarget.Others, text, num, num2);
	}

	[PunRPC]
	public void RecepInfo4(string cible, float antiRollFrontHorizontal, float antiRollRearHorizontal)
	{
		GameObject gameObject = GameObject.Find(cible);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<RCC_CarControllerV3>().antiRollFrontHorizontal = antiRollFrontHorizontal;
			gameObject.GetComponent<RCC_CarControllerV3>().antiRollRearHorizontal = antiRollRearHorizontal;
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if ((bool)carController)
		{
			if (stream.IsWriting)
			{
				stream.SendNext(carController.gasInput);
				stream.SendNext(carController.brakeInput);
				stream.SendNext(carController.steerInput);
				stream.SendNext(carController.clutchInput);
				stream.SendNext(carController.idleInput);
				stream.SendNext(carController.currentGear);
				stream.SendNext(carController.engineTorque);
				stream.SendNext(carController.indicatorsOn);
				stream.SendNext(carController.highspeedsteerAngle);
				stream.SendNext(carController.highspeedsteerAngleAtspeed);
				stream.SendNext(carController.engineHeat);
				stream.SendNext(carController.engineHeatRate);
				stream.SendNext(carController.launched);
				stream.SendNext(base.transform.position);
				stream.SendNext(base.transform.rotation);
				stream.SendNext(rigid.velocity);
			}
			else
			{
				gasInput = (float)stream.ReceiveNext();
				brakeInput = (float)stream.ReceiveNext();
				steerInput = (float)stream.ReceiveNext();
				clutchInput = (float)stream.ReceiveNext();
				idleInput = (float)stream.ReceiveNext();
				gear = (int)stream.ReceiveNext();
				engineTorque = (float)stream.ReceiveNext();
				indicatorsOn = (RCC_CarControllerV3.IndicatorsOn)stream.ReceiveNext();
				highspeedsteerAngle = (float)stream.ReceiveNext();
				highspeedsteerAngleAtspeed = (float)stream.ReceiveNext();
				engineHeat = (float)stream.ReceiveNext();
				engineHeatMultiplier = (float)stream.ReceiveNext();
				launched = (float)stream.ReceiveNext();
				correctPlayerPos = (Vector3)stream.ReceiveNext();
				correctPlayerRot = (Quaternion)stream.ReceiveNext();
				currentVelocity = (Vector3)stream.ReceiveNext();
				updateTime = Time.time;
			}
		}
	}
}
