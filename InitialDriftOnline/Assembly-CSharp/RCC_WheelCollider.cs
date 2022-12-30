using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Main/RCC Wheel Collider")]
public class RCC_WheelCollider : MonoBehaviour
{
	private RCC_Settings RCCSettingsInstance;

	private RCC_GroundMaterials RCCGroundMaterialsInstance;

	private WheelCollider _wheelCollider;

	private RCC_CarControllerV3 carController;

	private Rigidbody rigid;

	private List<RCC_WheelCollider> allWheelColliders = new List<RCC_WheelCollider>();

	public Transform wheelModel;

	private float wheelRotation;

	public float camber;

	public WheelHit wheelHit;

	internal float wheelRPMToSpeed;

	private RCC_SkidmarksManager skidmarksManager;

	private int lastSkidmark = -1;

	private float wheelSlipAmountForward;

	private float wheelSlipAmountSideways;

	internal float totalSlip;

	public WheelFrictionCurve forwardFrictionCurve;

	public WheelFrictionCurve sidewaysFrictionCurve;

	public bool isGrounded;

	private AudioSource audioSource;

	private AudioClip audioClip;

	private float audioVolume = 1f;

	public int groundIndex;

	internal List<ParticleSystem> allWheelParticles = new List<ParticleSystem>();

	internal ParticleSystem.EmissionModule emission;

	internal float tractionHelpedSidewaysStiffness = 1f;

	private float minForwardStiffness = 0.75f;

	private float maxForwardStiffness = 1f;

	private float minSidewaysStiffness = 0.75f;

	private float maxSidewaysStiffness = 1f;

	private TerrainData mTerrainData;

	private int alphamapWidth;

	private int alphamapHeight;

	private float[,,] mSplatmapData;

	private float mNumTextures;

	public float compression;

	public bool canSteer;

	[Range(-1f, 1f)]
	public float steeringMultiplier = 1f;

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

	private RCC_GroundMaterials RCCGroundMaterials
	{
		get
		{
			if (RCCGroundMaterialsInstance == null)
			{
				RCCGroundMaterialsInstance = RCC_GroundMaterials.Instance;
			}
			return RCCGroundMaterialsInstance;
		}
	}

	public WheelCollider wheelCollider
	{
		get
		{
			if (_wheelCollider == null)
			{
				_wheelCollider = GetComponent<WheelCollider>();
			}
			return _wheelCollider;
		}
	}

	private RCC_GroundMaterials physicsMaterials => RCCGroundMaterials;

	private RCC_GroundMaterials.GroundMaterialFrictions[] physicsFrictions => RCCGroundMaterials.frictions;

	private float steeringSmoother => carController.steeringSensitivity;

	private void Start()
	{
		carController = GetComponentInParent<RCC_CarControllerV3>();
		rigid = carController.GetComponent<Rigidbody>();
		allWheelColliders = carController.GetComponentsInChildren<RCC_WheelCollider>().ToList();
		GetTerrainData();
		CheckBehavior();
		if (!RCCSettings.dontUseSkidmarks)
		{
			if ((bool)Object.FindObjectOfType<RCC_SkidmarksManager>())
			{
				skidmarksManager = Object.FindObjectOfType<RCC_SkidmarksManager>();
			}
			else
			{
				skidmarksManager = Object.Instantiate(RCCSettings.skidmarksManager, Vector3.zero, Quaternion.identity);
			}
		}
		if (RCCSettings.useFixedWheelColliders)
		{
			wheelCollider.mass = rigid.mass / 20f;
		}
		if (RCCSettings.useSharedAudioSources)
		{
			if (!carController.transform.Find("All Audio Sources/Skid Sound AudioSource"))
			{
				audioSource = RCC_CreateAudioSource.NewAudioSource(carController.audioMixer, carController.gameObject, "Skid Sound AudioSource", 5f, 50f, 0f, audioClip, loop: true, playNow: true, destroyAfterFinished: false);
			}
			else
			{
				audioSource = carController.transform.Find("All Audio Sources/Skid Sound AudioSource").GetComponent<AudioSource>();
			}
		}
		else
		{
			audioSource = RCC_CreateAudioSource.NewAudioSource(carController.audioMixer, carController.gameObject, "Skid Sound AudioSource", 5f, 50f, 0f, audioClip, loop: true, playNow: true, destroyAfterFinished: false);
			audioSource.transform.position = base.transform.position;
		}
		if (!RCCSettings.dontUseAnyParticleEffects)
		{
			for (int i = 0; i < RCCGroundMaterials.frictions.Length; i++)
			{
				GameObject gameObject = Object.Instantiate(RCCGroundMaterials.frictions[i].groundParticles, base.transform.position, base.transform.rotation);
				emission = gameObject.GetComponent<ParticleSystem>().emission;
				emission.enabled = false;
				gameObject.transform.SetParent(base.transform, worldPositionStays: false);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				allWheelParticles.Add(gameObject.GetComponent<ParticleSystem>());
			}
		}
		GameObject gameObject2 = new GameObject("Pivot_" + wheelModel.transform.name);
		gameObject2.transform.position = RCC_GetBounds.GetBoundsCenter(wheelModel.transform);
		gameObject2.transform.rotation = base.transform.rotation;
		gameObject2.transform.SetParent(wheelModel.transform.parent, worldPositionStays: true);
		wheelModel.SetParent(gameObject2.transform, worldPositionStays: true);
		wheelModel = gameObject2.transform;
	}

	private void OnEnable()
	{
		RCC_SceneManager.OnBehaviorChanged += CheckBehavior;
	}

	private void CheckBehavior()
	{
		forwardFrictionCurve = wheelCollider.forwardFriction;
		sidewaysFrictionCurve = wheelCollider.sidewaysFriction;
		RCC_Settings.BehaviorType selectedBehaviorType = RCCSettings.selectedBehaviorType;
		if (selectedBehaviorType != null)
		{
			forwardFrictionCurve = SetFrictionCurves(forwardFrictionCurve, selectedBehaviorType.forwardExtremumSlip, selectedBehaviorType.forwardExtremumValue, selectedBehaviorType.forwardAsymptoteSlip, selectedBehaviorType.forwardAsymptoteValue);
			sidewaysFrictionCurve = SetFrictionCurves(sidewaysFrictionCurve, selectedBehaviorType.sidewaysExtremumSlip, selectedBehaviorType.sidewaysExtremumValue, selectedBehaviorType.sidewaysAsymptoteSlip, selectedBehaviorType.sidewaysAsymptoteValue);
		}
		wheelCollider.forwardFriction = forwardFrictionCurve;
		wheelCollider.sidewaysFriction = sidewaysFrictionCurve;
	}

	private void Update()
	{
		if (carController.enabled && !carController.isSleeping)
		{
			WheelAlign();
		}
	}

	private void FixedUpdate()
	{
		isGrounded = wheelCollider.GetGroundHit(out wheelHit);
		groundIndex = GetGroundMaterialIndex();
		wheelRPMToSpeed = wheelCollider.rpm * wheelCollider.radius / 2.9f * rigid.transform.lossyScale.y;
		Frictions();
		SkidMarks();
		Audio();
		Smoke();
		if (!carController.enabled || carController.isSleeping)
		{
			return;
		}
		switch (carController.wheelTypeChoise)
		{
		case RCC_CarControllerV3.WheelType.FWD:
			if (this == carController.FrontLeftWheelCollider || this == carController.FrontRightWheelCollider)
			{
				ApplyMotorTorque(carController.engineTorque);
			}
			break;
		case RCC_CarControllerV3.WheelType.RWD:
			if (this == carController.RearLeftWheelCollider || this == carController.RearRightWheelCollider)
			{
				ApplyMotorTorque(carController.engineTorque);
			}
			break;
		case RCC_CarControllerV3.WheelType.AWD:
			ApplyMotorTorque(carController.engineTorque);
			break;
		case RCC_CarControllerV3.WheelType.BIASED:
			if (this == carController.FrontLeftWheelCollider || this == carController.FrontRightWheelCollider)
			{
				ApplyMotorTorque(carController.engineTorque * (100f - carController.biasedWheelTorque) / 100f);
			}
			if (this == carController.RearLeftWheelCollider || this == carController.RearRightWheelCollider)
			{
				ApplyMotorTorque(carController.engineTorque * carController.biasedWheelTorque / 100f);
			}
			break;
		}
		if (carController.ExtraRearWheelsCollider.Length != 0 && carController.applyEngineTorqueToExtraRearWheelColliders)
		{
			for (int i = 0; i < carController.ExtraRearWheelsCollider.Length; i++)
			{
				if (this == carController.ExtraRearWheelsCollider[i])
				{
					ApplyMotorTorque(carController.engineTorque);
				}
			}
		}
		if (this == carController.FrontLeftWheelCollider || this == carController.FrontRightWheelCollider || canSteer)
		{
			ApplySteering();
		}
		if (carController.handbrakeInput > 0.5f)
		{
			if (this == carController.RearLeftWheelCollider || this == carController.RearRightWheelCollider)
			{
				ApplyBrakeTorque(carController.brakeTorque * 1f * carController.handbrakeInput);
			}
		}
		else if (this == carController.FrontLeftWheelCollider || this == carController.FrontRightWheelCollider)
		{
			ApplyBrakeTorque(carController.brakeTorque * Mathf.Clamp(carController._brakeInput, 0f, 1f));
		}
		else
		{
			ApplyBrakeTorque(carController.brakeTorque * (Mathf.Clamp(carController._brakeInput, 0f, 1f) / 5f));
		}
		if (!carController.ESP || !(carController.handbrakeInput < 0.5f))
		{
			return;
		}
		if (carController.underSteering)
		{
			if (this == carController.FrontLeftWheelCollider)
			{
				ApplyBrakeTorque(carController.brakeTorque * carController.ESPStrength * Mathf.Clamp(0f - carController.rearSlip, 0f, float.PositiveInfinity));
			}
			if (this == carController.FrontRightWheelCollider)
			{
				ApplyBrakeTorque(carController.brakeTorque * carController.ESPStrength * Mathf.Clamp(carController.rearSlip, 0f, float.PositiveInfinity));
			}
		}
		if (carController.overSteering)
		{
			if (this == carController.RearLeftWheelCollider)
			{
				ApplyBrakeTorque(carController.brakeTorque * carController.ESPStrength * Mathf.Clamp(0f - carController.frontSlip, 0f, float.PositiveInfinity));
			}
			if (this == carController.RearRightWheelCollider)
			{
				ApplyBrakeTorque(carController.brakeTorque * carController.ESPStrength * Mathf.Clamp(carController.frontSlip, 0f, float.PositiveInfinity));
			}
		}
	}

	public void WheelAlign()
	{
		if (!wheelModel)
		{
			Debug.LogError(base.transform.name + " wheel of the " + carController.transform.name + " is missing wheel model. This wheel is disabled");
			base.enabled = false;
			return;
		}
		WheelHit hit;
		bool groundHit = wheelCollider.GetGroundHit(out hit);
		float num = compression;
		compression = Mathf.Lerp(b: (!groundHit) ? wheelCollider.suspensionDistance : (1f - (Vector3.Dot(base.transform.position - hit.point, base.transform.up) - wheelCollider.radius * base.transform.lossyScale.y) / wheelCollider.suspensionDistance), a: compression, t: Time.deltaTime * 50f);
		wheelModel.position = base.transform.position;
		wheelModel.position += base.transform.up * (compression - 1f) * wheelCollider.suspensionDistance;
		wheelRotation += wheelCollider.rpm * 6f * Time.deltaTime;
		wheelModel.rotation = base.transform.rotation * Quaternion.Euler(wheelRotation, wheelCollider.steerAngle, base.transform.rotation.z);
		if (base.transform.localPosition.x > 0f)
		{
			base.transform.localRotation = Quaternion.identity * Quaternion.AngleAxis(camber, Vector3.forward);
		}
		else
		{
			base.transform.localRotation = Quaternion.identity * Quaternion.AngleAxis(0f - camber, Vector3.forward);
		}
		float num2 = (0f - wheelCollider.transform.InverseTransformPoint(hit.point).y - wheelCollider.radius * base.transform.lossyScale.y) / wheelCollider.suspensionDistance;
		Debug.DrawLine(hit.point, hit.point + base.transform.up * (hit.force / rigid.mass), ((double)num2 <= 0.0) ? Color.magenta : Color.white);
		Debug.DrawLine(hit.point, hit.point - base.transform.forward * hit.forwardSlip * 2f, Color.green);
		Debug.DrawLine(hit.point, hit.point - base.transform.right * hit.sidewaysSlip * 2f, Color.red);
	}

	private void SkidMarks()
	{
		if (isGrounded)
		{
			wheelSlipAmountForward = Mathf.Abs(wheelHit.forwardSlip);
			wheelSlipAmountSideways = Mathf.Abs(wheelHit.sidewaysSlip);
		}
		else
		{
			wheelSlipAmountForward = 0f;
			wheelSlipAmountSideways = 0f;
		}
		totalSlip = Mathf.Lerp(totalSlip, (wheelSlipAmountSideways + wheelSlipAmountForward) / 2f, Time.fixedDeltaTime * 5f);
		if (RCCSettings.dontUseSkidmarks)
		{
			return;
		}
		if (totalSlip > physicsFrictions[groundIndex].slip)
		{
			Vector3 pos = wheelHit.point + 2f * rigid.velocity * Time.deltaTime;
			if (rigid.velocity.magnitude > 1f)
			{
				lastSkidmark = skidmarksManager.AddSkidMark(pos, wheelHit.normal, totalSlip - physicsFrictions[groundIndex].slip, lastSkidmark, groundIndex);
			}
			else
			{
				lastSkidmark = -1;
			}
		}
		else
		{
			lastSkidmark = -1;
		}
	}

	private void Frictions()
	{
		float handbrakeInput = carController.handbrakeInput;
		handbrakeInput = (((!(this == carController.RearLeftWheelCollider) && !(this == carController.RearRightWheelCollider)) || !(handbrakeInput > 0.75f)) ? 1f : 0.75f);
		forwardFrictionCurve.stiffness = physicsFrictions[groundIndex].forwardStiffness;
		sidewaysFrictionCurve.stiffness = physicsFrictions[groundIndex].sidewaysStiffness * handbrakeInput * tractionHelpedSidewaysStiffness;
		if (RCCSettings.selectedBehaviorType != null && RCCSettings.selectedBehaviorType.applyExternalWheelFrictions)
		{
			Drift();
		}
		wheelCollider.forwardFriction = forwardFrictionCurve;
		wheelCollider.sidewaysFriction = sidewaysFrictionCurve;
		wheelCollider.wheelDampingRate = physicsFrictions[groundIndex].damp;
		audioClip = physicsFrictions[groundIndex].groundSound;
		audioVolume = physicsFrictions[groundIndex].volume;
	}

	private void Smoke()
	{
		if (RCCSettings.dontUseAnyParticleEffects)
		{
			return;
		}
		for (int i = 0; i < allWheelParticles.Count; i++)
		{
			if (totalSlip > physicsFrictions[groundIndex].slip)
			{
				if (i != groundIndex)
				{
					ParticleSystem.EmissionModule emissionModule = allWheelParticles[i].emission;
					emissionModule.enabled = false;
				}
				else
				{
					ParticleSystem.EmissionModule emissionModule2 = allWheelParticles[i].emission;
					emissionModule2.enabled = true;
				}
			}
			else
			{
				ParticleSystem.EmissionModule emissionModule3 = allWheelParticles[i].emission;
				emissionModule3.enabled = false;
			}
		}
	}

	private void Drift()
	{
		Vector3 vector = base.transform.InverseTransformDirection(rigid.velocity);
		float num = vector.x * vector.x / 10f;
		num += Mathf.Abs(wheelHit.forwardSlip * wheelHit.forwardSlip) * 1f;
		if (wheelCollider == carController.FrontLeftWheelCollider.wheelCollider || wheelCollider == carController.FrontRightWheelCollider.wheelCollider)
		{
			forwardFrictionCurve.extremumValue = Mathf.Clamp(1f - num, 0.5f, maxForwardStiffness);
			forwardFrictionCurve.asymptoteValue = Mathf.Clamp(0.75f - num / 2f, 0.5f, minForwardStiffness);
		}
		else
		{
			forwardFrictionCurve.extremumValue = Mathf.Clamp(1f - num, 1f, maxForwardStiffness);
			forwardFrictionCurve.asymptoteValue = Mathf.Clamp(0.75f - num / 2f, 1.2f, minForwardStiffness);
		}
		if (wheelCollider == carController.FrontLeftWheelCollider.wheelCollider || wheelCollider == carController.FrontRightWheelCollider.wheelCollider)
		{
			sidewaysFrictionCurve.extremumValue = Mathf.Clamp(1f - num, 0.4f, maxSidewaysStiffness);
			sidewaysFrictionCurve.asymptoteValue = Mathf.Clamp(0.75f - num / 2f, 0.4f, minSidewaysStiffness);
		}
		else
		{
			sidewaysFrictionCurve.extremumValue = Mathf.Clamp(1f - num, 0.375f, maxSidewaysStiffness);
			sidewaysFrictionCurve.asymptoteValue = Mathf.Clamp(0.75f - num / 2f, 0.375f, minSidewaysStiffness);
		}
	}

	private void Audio()
	{
		if (RCCSettings.useSharedAudioSources && isSkidding())
		{
			return;
		}
		if (totalSlip > physicsFrictions[groundIndex].slip)
		{
			if (audioSource.clip != audioClip)
			{
				audioSource.clip = audioClip;
			}
			if (!audioSource.isPlaying)
			{
				audioSource.Play();
			}
			if (rigid.velocity.magnitude > 1f)
			{
				audioSource.volume = Mathf.Lerp(0f, audioVolume, totalSlip - 0f);
				audioSource.pitch = Mathf.Lerp(1f, 0.8f, audioSource.volume);
			}
			else
			{
				audioSource.volume = 0f;
			}
		}
		else
		{
			audioSource.volume = 0f;
			if (audioSource.volume <= 0.05f && audioSource.isPlaying)
			{
				audioSource.Stop();
			}
		}
	}

	private bool isSkidding()
	{
		for (int i = 0; i < allWheelColliders.Count; i++)
		{
			if (allWheelColliders[i].totalSlip > physicsFrictions[groundIndex].slip)
			{
				return true;
			}
		}
		return false;
	}

	private void ApplyMotorTorque(float torque)
	{
		if (carController.TCS)
		{
			wheelCollider.GetGroundHit(out var hit);
			if (Mathf.Abs(wheelCollider.rpm) >= 100f)
			{
				if (hit.forwardSlip > physicsFrictions[groundIndex].slip)
				{
					carController.TCSAct = true;
					torque -= Mathf.Clamp(torque * hit.forwardSlip * carController.TCSStrength, 0f, carController.engineTorque);
				}
				else
				{
					carController.TCSAct = false;
					torque += Mathf.Clamp(torque * hit.forwardSlip * carController.TCSStrength, 0f - carController.engineTorque, 0f);
				}
			}
			else
			{
				carController.TCSAct = false;
			}
		}
		if (OverTorque())
		{
			torque = 0f;
		}
		wheelCollider.motorTorque = torque * (1f - carController.clutchInput) * carController._gasInput * (1f + carController._boostInput) * (carController.engineTorqueCurve[carController.currentGear].Evaluate(wheelRPMToSpeed * (float)carController.direction) * (float)carController.direction) * carController.finalRatio;
	}

	public void ApplySteering()
	{
		wheelCollider.steerAngle = Mathf.Lerp(wheelCollider.steerAngle, Mathf.Clamp(carController.steerAngle * carController._steerInput, 0f - carController.steerAngle, carController.steerAngle) * steeringMultiplier, steeringSmoother * Time.fixedDeltaTime);
	}

	private void ApplyBrakeTorque(float brake)
	{
		if (carController.ABS && carController.handbrakeInput <= 0.1f)
		{
			wheelCollider.GetGroundHit(out var hit);
			if (Mathf.Abs(hit.forwardSlip) * Mathf.Clamp01(brake) >= carController.ABSThreshold)
			{
				carController.ABSAct = true;
				brake = 0f;
			}
			else
			{
				carController.ABSAct = false;
			}
		}
		wheelCollider.brakeTorque = brake;
	}

	private bool OverTorque()
	{
		if (carController.speed > carController.maxspeed || !carController.engineRunning)
		{
			return true;
		}
		return false;
	}

	private void GetTerrainData()
	{
		if ((bool)Terrain.activeTerrain)
		{
			mTerrainData = Terrain.activeTerrain.terrainData;
			alphamapWidth = mTerrainData.alphamapWidth;
			alphamapHeight = mTerrainData.alphamapHeight;
			mSplatmapData = mTerrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
			mNumTextures = mSplatmapData.Length / (alphamapWidth * alphamapHeight);
		}
	}

	private Vector3 ConvertToSplatMapCoordinate(Vector3 playerPos)
	{
		Vector3 result = default(Vector3);
		Terrain activeTerrain = Terrain.activeTerrain;
		Vector3 position = activeTerrain.transform.position;
		result.x = (playerPos.x - position.x) / activeTerrain.terrainData.size.x * (float)activeTerrain.terrainData.alphamapWidth;
		result.z = (playerPos.z - position.z) / activeTerrain.terrainData.size.z * (float)activeTerrain.terrainData.alphamapHeight;
		return result;
	}

	private int GetGroundMaterialIndex()
	{
		bool flag = false;
		wheelCollider.GetGroundHit(out var hit);
		if (hit.point == Vector3.zero)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < physicsFrictions.Length; i++)
		{
			if (hit.collider.sharedMaterial == physicsFrictions[i].groundMaterial)
			{
				flag = true;
				num = i;
			}
		}
		if (!flag)
		{
			for (int j = 0; j < RCCGroundMaterials.terrainFrictions.Length; j++)
			{
				if (!(hit.collider.sharedMaterial == RCCGroundMaterials.terrainFrictions[j].groundMaterial))
				{
					continue;
				}
				Vector3 position = base.transform.position;
				Vector3 vector = ConvertToSplatMapCoordinate(position);
				float num2 = 0f;
				for (int k = 0; (float)k < mNumTextures; k++)
				{
					if (num2 < mSplatmapData[(int)vector.z, (int)vector.x, k])
					{
						num = k;
					}
				}
				num = RCCGroundMaterialsInstance.terrainFrictions[j].splatmapIndexes[num].index;
			}
		}
		return num;
	}

	public WheelFrictionCurve SetFrictionCurves(WheelFrictionCurve curve, float extremumSlip, float extremumValue, float asymptoteSlip, float asymptoteValue)
	{
		WheelFrictionCurve result = curve;
		result.extremumSlip = extremumSlip;
		result.extremumValue = extremumValue;
		result.asymptoteSlip = asymptoteSlip;
		result.asymptoteValue = asymptoteValue;
		return result;
	}

	private void OnDisable()
	{
		RCC_SceneManager.OnBehaviorChanged -= CheckBehavior;
	}
}
