using System;
using System.Collections;
using System.IO;
using CodeStage.AntiCheat.Storage;
using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/RCC Camera")]
public class RCC_Camera : MonoBehaviour
{
	public enum CameraMode
	{
		TPS,
		TPS2,
		FPS,
		WHEEL,
		FIXED,
		CINEMATIC
	}

	public delegate void onBCGCameraSpawned(GameObject BCGCamera);

	private RCC_Settings RCCSettingsInstance;

	public bool isRendering = true;

	public RCC_CarControllerV3 playerCar;

	private Rigidbody playerRigid;

	private float playerSpeed;

	private Vector3 playerVelocity = new Vector3(0f, 0f, 0f);

	public Camera thisCam;

	public GameObject pivot;

	[Space]
	public CameraMode cameraMode;

	public CameraMode lastCameraMode;

	public bool useTopCameraMode;

	public bool useHoodCameraMode = true;

	public bool useOrbitInTPSCameraMode = true;

	public bool useOrbitInHoodCameraMode = true;

	public bool useWheelCameraMode = true;

	public bool useFixedCameraMode = true;

	public bool useCinematicCameraMode = true;

	public bool useOrthoForTopCamera;

	public bool useOcclusion = true;

	public LayerMask occlusionLayerMask = -1;

	public bool useAutoChangeCamera;

	private float autoChangeCameraTimer;

	public Vector3 topCameraAngle = new Vector3(45f, 45f, 0f);

	private float distanceOffset;

	public float maximumZDistanceOffset = 10f;

	public float topCameraDistance = 100f;

	private Vector3 targetPosition = Vector3.zero;

	public int RearRotationValue;

	private int direction = 1;

	private int lastDirection = 1;

	// OLD: public float TPSDistance = 6f;
	public float TPSDistance = 7f;

	// OLD: public float TPSHeight = 2f;
	public float TPSHeight = 3f;

	public float TPSHeightDamping = 10f;

	public float TPSRotationDamping = 5f;

	public float TPSRotationDamping2 = 10f;

	public float TPSTiltMaximum = 15f;

	public float TPSTiltMultiplier = 2f;

	private float TPSTiltAngle;

	public float TPSYawAngle;

	public float TPSPitchAngle = 7f;

	public float TPSOffsetX;

	// OLD: public float TPSOffsetY = 0.5f;
	public float TPSOffsetY = 0.75f;

	public bool TPSAutoFocus = true;

	private int BackBackSpeed;

	private int fixeinfo;

	// OLD: blank
	public static float SettingsFOV = (float)Convert.ToDouble(File.ReadAllLines("Settings.txt")[1].Split('=')[1]);

	// OLD: internal float targetFieldOfView = 60f;
	internal float targetFieldOfView = SettingsFOV + 15f;

	// OLD: public float TPSMinimumFOV = 50f;
	public readonly float TPSMinimumFOV = SettingsFOV;

	// OLD: public float TPSMaximumFOV = 70f;
	public readonly float TPSMaximumFOV = SettingsFOV + 30f;

	public float hoodCameraFOV = 60f;

	public float wheelCameraFOV = 60f;

	public float minimumOrtSize = 10f;

	public float maximumOrtSize = 20f;

	internal int cameraSwitchCount;

	private RCC_HoodCamera hoodCam;

	private RCC_WheelCamera wheelCam;

	private RCC_FixedCamera fixedCam;

	private RCC_CinematicCamera cinematicCam;

	private Vector3 collisionVector = Vector3.zero;

	private Vector3 collisionPos = Vector3.zero;

	private Quaternion collisionRot = Quaternion.identity;

	private float index;

	private Quaternion orbitRotation = Quaternion.identity;

	internal float orbitX;

	internal float orbitY;

	public float minOrbitY = -20f;

	public float maxOrbitY = 80f;

	public float orbitXSpeed = 50f;

	public float orbitYSpeed = 50f;

	public float orbitSmooth = 10f;

	private float orbitResetTimer;

	private float oldOrbitX;

	public float oldOrbitY;

	private Quaternion currentRotation = Quaternion.identity;

	public Quaternion wantedRotation = Quaternion.identity;

	private float currentHeight;

	private float wantedHeight;

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

	public static event onBCGCameraSpawned OnBCGCameraSpawned;

	private void Awake()
	{
		thisCam = GetComponentInChildren<Camera>();
		RearRotationValue = 180;
	}

	private void OnEnable()
	{
		if (RCC_Camera.OnBCGCameraSpawned != null)
		{
			RCC_Camera.OnBCGCameraSpawned(base.gameObject);
		}
		RCC_CarControllerV3.OnRCCPlayerCollision += RCC_CarControllerV3_OnRCCPlayerCollision;
	}

	private void RCC_CarControllerV3_OnRCCPlayerCollision(RCC_CarControllerV3 RCC, Collision collision)
	{
		Collision(collision);
	}

	private void GetTarget()
	{
		if ((bool)playerCar)
		{
			if (TPSAutoFocus)
			{
				StartCoroutine(AutoFocus());
			}
			playerRigid = playerCar.GetComponent<Rigidbody>();
			hoodCam = playerCar.GetComponentInChildren<RCC_HoodCamera>();
			wheelCam = playerCar.GetComponentInChildren<RCC_WheelCamera>();
			fixedCam = UnityEngine.Object.FindObjectOfType<RCC_FixedCamera>();
			cinematicCam = UnityEngine.Object.FindObjectOfType<RCC_CinematicCamera>();
			ResetCamera();
		}
	}

	public void SetTarget(GameObject player)
	{
		playerCar = player.GetComponent<RCC_CarControllerV3>();
		GetTarget();
	}

	public void RemoveTarget()
	{
		base.transform.SetParent(null);
		playerCar = null;
		playerRigid = null;
	}

	private void Update()
	{
		if (!isRendering)
		{
			if (thisCam.gameObject.activeInHierarchy)
			{
				thisCam.gameObject.SetActive(value: false);
			}
			return;
		}
		if (!thisCam.gameObject.activeInHierarchy)
		{
			thisCam.gameObject.SetActive(value: true);
		}
		if (!playerCar || !playerRigid)
		{
			GetTarget();
			return;
		}
		Inputs();
		playerSpeed = Mathf.Lerp(playerSpeed, playerCar.speed, Time.deltaTime * 5f);
		playerVelocity = playerCar.transform.InverseTransformDirection(playerRigid.velocity);
		if (index > 0f)
		{
			index -= Time.deltaTime * 5f;
		}
		thisCam.fieldOfView = Mathf.Lerp(thisCam.fieldOfView, targetFieldOfView, Time.deltaTime * 5f);
	}

	private void LateUpdate()
	{
		if (!playerCar || !playerRigid || !playerCar.gameObject.activeSelf)
		{
			return;
		}
		switch (cameraMode)
		{
		case CameraMode.TPS:
			if (ObscuredPrefs.GetBool("BackCamToogle"))
			{
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID3>().gameObject.GetComponent<Camera>().enabled = true;
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID3>().gameObject.GetComponent<Camera>().targetTexture = null;
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID3>().gameObject.GetComponent<Camera>().rect = new Rect(0.4f, 0.8659f, 0.2f, 0.15f);
				if ((bool)RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID5>())
				{
					RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID5>().gameObject.GetComponent<MeshRenderer>().enabled = false;
				}
			}
			TPS();
			if (useOrbitInTPSCameraMode && PlayerPrefs.GetInt("MenuOpen") == 0)
			{
				ORBIT();
			}
			break;
		case CameraMode.TPS2:
			if (ObscuredPrefs.GetBool("BackCamToogle"))
			{
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID3>().gameObject.GetComponent<Camera>().enabled = true;
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID3>().gameObject.GetComponent<Camera>().targetTexture = null;
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID3>().gameObject.GetComponent<Camera>().rect = new Rect(0.4f, 0.8659f, 0.2f, 0.15f);
				if ((bool)RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID5>())
				{
					RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID5>().gameObject.GetComponent<MeshRenderer>().enabled = false;
				}
			}
			TPS2();
			if (useOrbitInTPSCameraMode && PlayerPrefs.GetInt("MenuOpen") == 0)
			{
				ORBIT();
			}
			break;
		case CameraMode.FPS:
			if (ObscuredPrefs.GetBool("BackCamToogle"))
			{
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID3>().gameObject.GetComponent<Camera>().enabled = true;
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID3>().gameObject.GetComponent<Camera>().targetTexture = UnityEngine.Object.FindObjectOfType<SRShadowManager>().jack;
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID3>().gameObject.GetComponent<Camera>().rect = new Rect(0f, 0f, 1f, 1f);
				if ((bool)RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID5>())
				{
					RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID5>().gameObject.GetComponent<MeshRenderer>().enabled = true;
				}
			}
			FPS();
			if (useOrbitInHoodCameraMode && PlayerPrefs.GetInt("MenuOpen") == 0)
			{
				ORBIT();
			}
			break;
		case CameraMode.WHEEL:
			if (ObscuredPrefs.GetBool("BackCamToogle"))
			{
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID3>().gameObject.GetComponent<Camera>().enabled = false;
				if ((bool)RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID5>())
				{
					RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID5>().gameObject.GetComponent<MeshRenderer>().enabled = false;
				}
			}
			WHEEL();
			break;
		case CameraMode.FIXED:
			if (ObscuredPrefs.GetBool("BackCamToogle"))
			{
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID3>().gameObject.GetComponent<Camera>().enabled = false;
				if ((bool)RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID5>())
				{
					RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID5>().gameObject.GetComponent<MeshRenderer>().enabled = false;
				}
			}
			FIXED();
			break;
		case CameraMode.CINEMATIC:
			if (ObscuredPrefs.GetBool("BackCamToogle"))
			{
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID3>().gameObject.GetComponent<Camera>().enabled = false;
				if ((bool)RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID5>())
				{
					RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID5>().gameObject.GetComponent<MeshRenderer>().enabled = false;
				}
			}
			CINEMATIC();
			break;
		}
		if (lastCameraMode != cameraMode)
		{
			ResetCamera();
		}
		lastCameraMode = cameraMode;
		autoChangeCameraTimer += Time.deltaTime;
		if (useAutoChangeCamera && autoChangeCameraTimer > 10f)
		{
			autoChangeCameraTimer = 0f;
			ChangeCamera();
		}
	}

	private void Inputs()
	{
		switch (RCCSettings.controllerType)
		{
		case RCC_Settings.ControllerType.Keyboard:
			if (Input.GetButtonDown(RCCSettings.Xbox_changeCameraKB) && playerCar == RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>())
			{
				ChangeCamera();
			}
			if (Input.GetKeyDown(RCCSettings.changeCameraKB) && ObscuredPrefs.GetInt("ONTYPING") == 0 && playerCar == RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>())
			{
				ChangeCamera();
			}
			orbitX += Input.GetAxis(RCCSettings.mouseXInput) * orbitXSpeed * 0.02f;
			orbitY -= Input.GetAxis(RCCSettings.mouseYInput) * orbitYSpeed * 0.02f;
			orbitX += Input.GetAxis(RCCSettings.Xbox_mouseXInput) * orbitXSpeed * 0.01f;
			orbitY -= Input.GetAxis(RCCSettings.Xbox_mouseYInput) * orbitYSpeed * 0.01f;
			break;
		case RCC_Settings.ControllerType.XBox360One:
			if (Input.GetButtonDown(RCCSettings.Xbox_changeCameraKB) && playerCar == RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>())
			{
				ChangeCamera();
			}
			if (Input.GetKeyDown(RCCSettings.changeCameraKB) && ObscuredPrefs.GetInt("ONTYPING") == 0 && playerCar == RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>())
			{
				ChangeCamera();
			}
			orbitX += Input.GetAxis(RCCSettings.Xbox_mouseXInput) * orbitXSpeed * 0.01f;
			orbitY -= Input.GetAxis(RCCSettings.Xbox_mouseYInput) * orbitYSpeed * 0.01f;
			break;
		case RCC_Settings.ControllerType.PS4:
			if (Input.GetButtonDown(RCCSettings.PS4_changeCameraKB) && playerCar == RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>())
			{
				ChangeCamera();
			}
			orbitX += Input.GetAxis(RCCSettings.PS4_mouseXInput) * orbitXSpeed * 0.01f;
			orbitY -= Input.GetAxis(RCCSettings.PS4_mouseYInput) * orbitYSpeed * 0.01f;
			break;
		case RCC_Settings.ControllerType.LogitechSteeringWheel:
			if (RCC_LogitechSteeringWheel.GetKeyTriggered(0, RCCSettings.LogiSteeringWheel_changeCameraKB) && playerCar == RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>())
			{
				ChangeCamera();
			}
			break;
		case RCC_Settings.ControllerType.Mobile:
			break;
		}
	}

	public void ChangeCamera()
	{
		cameraSwitchCount++;
		if (cameraSwitchCount >= 6)
		{
			cameraSwitchCount = 0;
		}
		switch (cameraSwitchCount)
		{
		case 0:
			cameraMode = CameraMode.TPS;
			break;
		case 1:
			cameraMode = CameraMode.TPS2;
			break;
		case 2:
			if (useHoodCameraMode && (bool)hoodCam)
			{
				cameraMode = CameraMode.FPS;
			}
			else
			{
				ChangeCamera();
			}
			break;
		case 3:
			if (useWheelCameraMode && (bool)wheelCam)
			{
				cameraMode = CameraMode.WHEEL;
			}
			else
			{
				ChangeCamera();
			}
			break;
		case 4:
			if (useFixedCameraMode && (bool)fixedCam)
			{
				cameraMode = CameraMode.FIXED;
			}
			else
			{
				ChangeCamera();
			}
			break;
		case 5:
			if (useCinematicCameraMode && (bool)cinematicCam)
			{
				cameraMode = CameraMode.CINEMATIC;
			}
			else
			{
				ChangeCamera();
			}
			break;
		}
	}

	public void ChangeCamera(CameraMode mode)
	{
		cameraMode = mode;
	}

	private void FPS()
	{
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, playerCar.transform.rotation * orbitRotation, Time.deltaTime * 50f);
	}

	private void WHEEL()
	{
		if (useOcclusion && Occluding(playerCar.transform.position))
		{
			ChangeCamera(CameraMode.TPS);
		}
	}

	private IEnumerator Jack()
	{
		yield return new WaitForSeconds(0.5f);
		TPSRotationDamping = 3f;
	}

	private IEnumerator Jack2()
	{
		yield return new WaitForSeconds(0.5f);
		TPSRotationDamping2 = 10f;
	}

	private void TPS()
	{
		RearRotationValue = PlayerPrefs.GetInt("DisableRearRotation_Value");
		fixeinfo = 0;
		if (lastDirection != playerCar.direction)
		{
			direction = playerCar.direction;
			orbitX = 0f;
			orbitY = 0f;
		}
		lastDirection = playerCar.direction;
		wantedRotation = playerCar.transform.rotation * Quaternion.AngleAxis((direction != 1) ? RearRotationValue : 0, Vector3.up);
		switch (RCCSettings.controllerType)
		{
		case RCC_Settings.ControllerType.Keyboard:
			if (Input.GetKey(RCCSettings.lookBackKB) || Input.GetButton(RCCSettings.Xbox_lookBackKB))
			{
				wantedRotation *= Quaternion.AngleAxis(180f, Vector3.up);
				TPSRotationDamping = 15f;
			}
			if (Input.GetKeyUp(RCCSettings.lookBackKB) || Input.GetButtonUp(RCCSettings.Xbox_lookBackKB))
			{
				wantedRotation *= Quaternion.AngleAxis(180f, Vector3.up);
				TPSRotationDamping = 10f;
			}
			break;
		case RCC_Settings.ControllerType.XBox360One:
			if (Input.GetButton(RCCSettings.Xbox_lookBackKB))
			{
				wantedRotation *= Quaternion.AngleAxis(180f, Vector3.up);
				TPSRotationDamping = 13f;
			}
			if (Input.GetButtonUp(RCCSettings.Xbox_lookBackKB))
			{
				wantedRotation *= Quaternion.AngleAxis(180f, Vector3.up);
				TPSRotationDamping = 17f;
				StartCoroutine(Jack());
			}
			break;
		case RCC_Settings.ControllerType.PS4:
			if (Input.GetButton(RCCSettings.PS4_lookBackKB))
			{
				wantedRotation *= Quaternion.AngleAxis(180f, Vector3.up);
				TPSRotationDamping = 13f;
			}
			if (Input.GetButtonUp(RCCSettings.PS4_lookBackKB))
			{
				wantedRotation *= Quaternion.AngleAxis(180f, Vector3.up);
				TPSRotationDamping = 17f;
				StartCoroutine(Jack());
			}
			break;
		case RCC_Settings.ControllerType.LogitechSteeringWheel:
			RCC_LogitechSteeringWheel.GetKeyPressed(0, RCCSettings.LogiSteeringWheel_lookBackKB);
			break;
		}
		if (PlayerPrefs.GetInt("WANTROT") > 0)
		{
			wantedRotation *= Quaternion.AngleAxis(PlayerPrefs.GetInt("WANTROT"), Vector3.up);
			TPSRotationDamping2 = 13f;
		}
		wantedHeight = playerCar.transform.position.y + TPSHeight + TPSOffsetY;
		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, TPSHeightDamping * Time.fixedDeltaTime);
		if (Time.time > 1f)
		{
			currentRotation = Quaternion.Lerp(currentRotation, wantedRotation, 1f - Mathf.Exp((0f - TPSRotationDamping) * Time.deltaTime));
		}
		else
		{
			currentRotation = wantedRotation;
		}
		TPSTiltAngle = Mathf.Lerp(0f, TPSTiltMaximum * Mathf.Clamp(0f - playerVelocity.x, -1f, 1f), Mathf.Abs(playerVelocity.x) / 50f);
		TPSTiltAngle *= TPSTiltMultiplier;
		targetPosition = playerCar.transform.position;
		targetPosition -= currentRotation * orbitRotation * Vector3.forward * (TPSDistance * Mathf.Lerp(1f, 0.75f, playerRigid.velocity.magnitude * 3.6f / 100f));
		targetPosition += Vector3.up * (TPSHeight * Mathf.Lerp(1f, 0.75f, playerRigid.velocity.magnitude * 3.6f / 100f));
		base.transform.position = targetPosition;
		Vector3 worldPosition = playerCar.transform.position + playerCar.transform.rotation * Vector3.forward * Vector3.Distance(playerCar.transform.position, playerCar.FrontLeftWheelTransform.position);
		base.transform.LookAt(worldPosition);
		base.transform.position = base.transform.position + base.transform.right * TPSOffsetX + base.transform.up * TPSOffsetY;
		base.transform.rotation *= Quaternion.Euler(TPSPitchAngle * Mathf.Lerp(1f, 0.75f, playerSpeed / 100f), 0f, Mathf.Clamp(0f - TPSTiltAngle, 0f - TPSTiltMaximum, TPSTiltMaximum) + TPSYawAngle);
		collisionPos = Vector3.Lerp(new Vector3(collisionPos.x, collisionPos.y, collisionPos.z), Vector3.zero, Time.deltaTime * 5f);
		if (Time.deltaTime != 0f)
		{
			collisionRot = Quaternion.Lerp(collisionRot, Quaternion.identity, Time.deltaTime * 5f);
		}
		pivot.transform.localPosition = Vector3.Lerp(pivot.transform.localPosition, collisionPos, Time.deltaTime * 10f);
		pivot.transform.localRotation = Quaternion.Lerp(pivot.transform.localRotation, collisionRot, Time.deltaTime * 10f);
		targetFieldOfView = Mathf.Lerp(TPSMinimumFOV, TPSMaximumFOV, Mathf.Abs(playerSpeed) / 150f);
		targetFieldOfView += 5f * Mathf.Cos(index);
		if (useOcclusion)
		{
			OccludeRay(playerCar.transform.position);
		}
	}

	private void TPS2()
	{
		RearRotationValue = PlayerPrefs.GetInt("DisableRearRotation_Value");
		if (fixeinfo == 0)
		{
			fixeinfo = 1;
			UnityEngine.Object.FindObjectOfType<SRID6>().gameObject.GetComponent<Animator>().Play("Camfix");
		}
		if (lastDirection != playerCar.direction)
		{
			direction = playerCar.direction;
			orbitX = 0f;
			orbitY = 0f;
		}
		lastDirection = playerCar.direction;
		TPSRotationDamping2 = 10f;
		wantedRotation = playerCar.transform.rotation * Quaternion.AngleAxis((direction != 1) ? RearRotationValue : 0, Vector3.up);
		switch (RCCSettings.controllerType)
		{
		case RCC_Settings.ControllerType.Keyboard:
			if (Input.GetKey(RCCSettings.lookBackKB) || Input.GetButton(RCCSettings.Xbox_lookBackKB))
			{
				wantedRotation *= Quaternion.AngleAxis(180f, Vector3.up);
				TPSRotationDamping2 = 15f;
			}
			if (Input.GetKeyUp(RCCSettings.lookBackKB) || Input.GetButtonUp(RCCSettings.Xbox_lookBackKB))
			{
				wantedRotation *= Quaternion.AngleAxis(180f, Vector3.up);
				TPSRotationDamping2 = 10f;
			}
			break;
		case RCC_Settings.ControllerType.XBox360One:
			if (Input.GetButton(RCCSettings.Xbox_lookBackKB))
			{
				wantedRotation *= Quaternion.AngleAxis(180f, Vector3.up);
				TPSRotationDamping2 = 13f;
			}
			if (Input.GetButtonUp(RCCSettings.Xbox_lookBackKB))
			{
				wantedRotation *= Quaternion.AngleAxis(180f, Vector3.up);
				TPSRotationDamping2 = 17f;
				StartCoroutine(Jack2());
			}
			break;
		case RCC_Settings.ControllerType.LogitechSteeringWheel:
			if (RCC_LogitechSteeringWheel.GetKeyPressed(0, RCCSettings.LogiSteeringWheel_lookBackKB))
			{
				wantedRotation *= Quaternion.AngleAxis(180f, Vector3.up);
				TPSRotationDamping2 = 17f;
				StartCoroutine(Jack());
			}
			break;
		}
		if (PlayerPrefs.GetInt("WANTROT") > 0)
		{
			wantedRotation *= Quaternion.AngleAxis(PlayerPrefs.GetInt("WANTROT"), Vector3.up);
			TPSRotationDamping2 = 13f;
		}
		wantedHeight = playerCar.transform.position.y + TPSHeight + TPSOffsetY;
		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, TPSHeightDamping * Time.fixedDeltaTime);
		if (Time.time > 1f)
		{
			currentRotation = Quaternion.Lerp(currentRotation, wantedRotation, 1f - Mathf.Exp((0f - TPSRotationDamping2) * Time.deltaTime));
		}
		else
		{
			currentRotation = wantedRotation;
		}
		TPSTiltAngle = Mathf.Lerp(0f, TPSTiltMaximum * Mathf.Clamp(0f - playerVelocity.x, -1f, 1f), Mathf.Abs(playerVelocity.x) / 50f);
		TPSTiltAngle *= TPSTiltMultiplier;
		targetPosition = playerCar.transform.position;
		targetPosition -= currentRotation * orbitRotation * Vector3.forward * (TPSDistance * Mathf.Lerp(1f, 0.75f, playerRigid.velocity.magnitude * 3.6f / 100f));
		targetPosition += Vector3.up * (TPSHeight * Mathf.Lerp(1f, 0.75f, playerRigid.velocity.magnitude * 3.6f / 100f));
		base.transform.position = targetPosition;
		Vector3 worldPosition = playerCar.transform.position + playerCar.transform.rotation * Vector3.forward * Vector3.Distance(playerCar.transform.position, playerCar.FrontLeftWheelTransform.position);
		base.transform.LookAt(worldPosition);
		base.transform.position = base.transform.position + base.transform.right * TPSOffsetX + base.transform.up * TPSOffsetY;
		base.transform.rotation *= Quaternion.Euler(TPSPitchAngle * Mathf.Lerp(1f, 0.75f, playerSpeed / 100f), 0f, Mathf.Clamp(0f - TPSTiltAngle, 0f - TPSTiltMaximum, TPSTiltMaximum) + TPSYawAngle);
		collisionPos = Vector3.Lerp(new Vector3(collisionPos.x, collisionPos.y, collisionPos.z), Vector3.zero, Time.deltaTime * 5f);
		if (Time.deltaTime != 0f)
		{
			collisionRot = Quaternion.Lerp(collisionRot, Quaternion.identity, Time.deltaTime * 5f);
		}
		pivot.transform.localPosition = Vector3.Lerp(pivot.transform.localPosition, collisionPos, Time.deltaTime * 10f);
		pivot.transform.localRotation = Quaternion.Lerp(pivot.transform.localRotation, collisionRot, Time.deltaTime * 10f);
		targetFieldOfView = Mathf.Lerp(TPSMinimumFOV, TPSMaximumFOV, Mathf.Abs(playerSpeed) / 150f);
		targetFieldOfView += 5f * Mathf.Cos(index);
		if (useOcclusion)
		{
			OccludeRay(playerCar.transform.position);
		}
	}

	private void FIXED()
	{
		if (fixedCam.transform.parent != null)
		{
			fixedCam.transform.SetParent(null);
		}
		if (useOcclusion && Occluding(playerCar.transform.position))
		{
			fixedCam.ChangePosition();
		}
	}

	private void TOP()
	{
		if ((bool)playerCar && (bool)playerRigid)
		{
			thisCam.orthographic = useOrthoForTopCamera;
			distanceOffset = Mathf.Lerp(0f, maximumZDistanceOffset, Mathf.Abs(playerSpeed) / 100f);
			targetFieldOfView = Mathf.Lerp(minimumOrtSize, maximumOrtSize, Mathf.Abs(playerSpeed) / 100f);
			thisCam.orthographicSize = targetFieldOfView;
			targetPosition = playerCar.transform.position;
			targetPosition += playerCar.transform.rotation * Vector3.forward * distanceOffset;
			base.transform.position = targetPosition;
			base.transform.rotation = Quaternion.Euler(topCameraAngle);
			pivot.transform.localPosition = new Vector3(0f, 0f, 0f - topCameraDistance);
		}
	}

	private void ORBIT()
	{
		orbitY = Mathf.Clamp(orbitY, minOrbitY, maxOrbitY);
		if (orbitX < -360f)
		{
			orbitX += 360f;
		}
		if (orbitX > 360f)
		{
			orbitX -= 360f;
		}
		orbitRotation = Quaternion.Lerp(orbitRotation, Quaternion.Euler(orbitY, orbitX, 0f), orbitSmooth * Time.deltaTime);
		if (oldOrbitX != orbitX)
		{
			oldOrbitX = orbitX;
			orbitResetTimer = 2f;
		}
		if (oldOrbitY != orbitY)
		{
			oldOrbitY = orbitY;
			orbitResetTimer = 2f;
		}
		if (orbitResetTimer > 0f)
		{
			orbitResetTimer -= Time.deltaTime;
		}
		Mathf.Clamp(orbitResetTimer, 0f, 2f);
		if (playerSpeed >= 25f && orbitResetTimer <= 0f)
		{
			orbitX = 0f;
			orbitY = 0f;
		}
	}

	public void OnDrag(PointerEventData pointerData)
	{
		orbitX += pointerData.delta.x * orbitXSpeed / 1000f;
		orbitY -= pointerData.delta.y * orbitYSpeed / 1000f;
		orbitResetTimer = 0f;
	}

	private void CINEMATIC()
	{
		if (cinematicCam.transform.parent != null)
		{
			cinematicCam.transform.SetParent(null);
		}
		targetFieldOfView = cinematicCam.targetFOV;
		if (useOcclusion && Occluding(playerCar.transform.position))
		{
			ChangeCamera(CameraMode.TPS);
		}
	}

	public void Collision(Collision collision)
	{
		if (base.enabled && isRendering && cameraMode == CameraMode.TPS)
		{
			Vector3 relativeVelocity = collision.relativeVelocity;
			relativeVelocity *= 1f - Mathf.Abs(Vector3.Dot(base.transform.up, collision.contacts[0].normal));
			float num = Mathf.Abs(Vector3.Dot(collision.contacts[0].normal, relativeVelocity.normalized));
			if (relativeVelocity.magnitude * num >= 5f)
			{
				collisionVector = base.transform.InverseTransformDirection(relativeVelocity) / 30f;
				collisionPos -= collisionVector * 5f;
				collisionRot = Quaternion.Euler(new Vector3((0f - collisionVector.z) * 10f, (0f - collisionVector.y) * 10f, (0f - collisionVector.x) * 10f));
				targetFieldOfView = thisCam.fieldOfView - Mathf.Clamp(collision.relativeVelocity.magnitude, 0f, 15f);
				index = Mathf.Clamp(relativeVelocity.magnitude * num * 50f, 0f, 10f);
			}
		}
	}

	public void ResetCamera()
	{
		if ((bool)fixedCam)
		{
			fixedCam.canTrackNow = false;
		}
		TPSTiltAngle = 0f;
		collisionPos = Vector3.zero;
		collisionRot = Quaternion.identity;
		thisCam.transform.localPosition = Vector3.zero;
		thisCam.transform.localRotation = Quaternion.identity;
		pivot.transform.localPosition = collisionPos;
		pivot.transform.localRotation = collisionRot;
		orbitX = 0f;
		orbitY = 0f;
		thisCam.orthographic = false;
		switch (cameraMode)
		{
		case CameraMode.TPS:
			base.transform.SetParent(null);
			targetFieldOfView = TPSMinimumFOV;
			break;
		case CameraMode.FPS:
			base.transform.SetParent(hoodCam.transform, worldPositionStays: false);
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
			targetFieldOfView = hoodCameraFOV;
			hoodCam.FixShake();
			break;
		case CameraMode.WHEEL:
			base.transform.SetParent(wheelCam.transform, worldPositionStays: false);
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
			targetFieldOfView = wheelCameraFOV;
			break;
		case CameraMode.FIXED:
			base.transform.SetParent(fixedCam.transform, worldPositionStays: false);
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
			targetFieldOfView = 60f;
			fixedCam.canTrackNow = true;
			break;
		case CameraMode.CINEMATIC:
			base.transform.SetParent(cinematicCam.pivot.transform, worldPositionStays: false);
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
			targetFieldOfView = 30f;
			break;
		case CameraMode.TPS2:
			break;
		}
	}

	public void ToggleCamera(bool state)
	{
		isRendering = state;
	}

	private void OccludeRay(Vector3 targetFollow)
	{
		RaycastHit hitInfo = default(RaycastHit);
		if (Physics.Linecast(targetFollow, base.transform.position, out hitInfo, occlusionLayerMask) && !hitInfo.collider.isTrigger && !hitInfo.transform.IsChildOf(playerCar.transform))
		{
			Vector3 position = new Vector3(hitInfo.point.x + hitInfo.normal.x * 0.2f, hitInfo.point.y + hitInfo.normal.y * 0.2f, hitInfo.point.z + hitInfo.normal.z * 0.2f);
			base.transform.position = position;
		}
	}

	private bool Occluding(Vector3 targetFollow)
	{
		RaycastHit hitInfo = default(RaycastHit);
		if (Physics.Linecast(targetFollow, base.transform.position, out hitInfo, ~(1 << LayerMask.NameToLayer(RCCSettings.RCCLayer))) && !hitInfo.collider.isTrigger && !hitInfo.transform.IsChildOf(playerCar.transform))
		{
			return true;
		}
		return false;
	}

	public IEnumerator AutoFocus()
	{
		float timer = 5f;
		while (timer > 0f)
		{
			timer -= Time.deltaTime;
			TPSDistance = Mathf.Lerp(TPSDistance, RCC_GetBounds.MaxBoundsExtent(playerCar.transform) * 2.75f, Time.deltaTime);
			TPSHeight = Mathf.Lerp(TPSHeight, RCC_GetBounds.MaxBoundsExtent(playerCar.transform) * 0.6f, Time.deltaTime);
			yield return null;
		}
	}

	public IEnumerator AutoFocus(Transform bounds)
	{
		float timer = 5f;
		while (timer > 0f)
		{
			timer -= Time.deltaTime;
			TPSDistance = Mathf.Lerp(TPSDistance, RCC_GetBounds.MaxBoundsExtent(bounds) * 2.75f, Time.deltaTime);
			TPSHeight = Mathf.Lerp(TPSHeight, RCC_GetBounds.MaxBoundsExtent(bounds) * 0.6f, Time.deltaTime);
			yield return null;
		}
	}

	public IEnumerator AutoFocus(Transform bounds1, Transform bounds2)
	{
		float timer = 5f;
		while (timer > 0f)
		{
			timer -= Time.deltaTime;
			TPSDistance = Mathf.Lerp(TPSDistance, RCC_GetBounds.MaxBoundsExtent(bounds1) * 2.75f + RCC_GetBounds.MaxBoundsExtent(bounds2) * 2.75f, Time.deltaTime);
			TPSHeight = Mathf.Lerp(TPSHeight, RCC_GetBounds.MaxBoundsExtent(bounds1) * 0.6f + RCC_GetBounds.MaxBoundsExtent(bounds2) * 0.6f, Time.deltaTime);
			yield return null;
		}
	}

	public IEnumerator AutoFocus(Transform bounds1, Transform bounds2, Transform bounds3)
	{
		float timer = 5f;
		while (timer > 0f)
		{
			timer -= Time.deltaTime;
			TPSDistance = Mathf.Lerp(TPSDistance, RCC_GetBounds.MaxBoundsExtent(bounds1) * 2.75f + RCC_GetBounds.MaxBoundsExtent(bounds2) * 2.75f + RCC_GetBounds.MaxBoundsExtent(bounds3) * 2.75f, Time.deltaTime);
			TPSHeight = Mathf.Lerp(TPSHeight, RCC_GetBounds.MaxBoundsExtent(bounds1) * 0.6f + RCC_GetBounds.MaxBoundsExtent(bounds2) * 0.6f + RCC_GetBounds.MaxBoundsExtent(bounds3) * 0.6f, Time.deltaTime);
			yield return null;
		}
	}

	private void OnDisable()
	{
		RCC_CarControllerV3.OnRCCPlayerCollision -= RCC_CarControllerV3_OnRCCPlayerCollision;
	}
}
