using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Main/RCC Scene Manager")]
public class RCC_SceneManager : MonoBehaviour
{
	public enum RecordMode
	{
		Neutral,
		Play,
		Record
	}

	public delegate void onMainControllerChanged();

	public delegate void onBehaviorChanged();

	public delegate void onVehicleChanged();

	private static RCC_SceneManager instance;

	public RCC_CarControllerV3 activePlayerVehicle;

	private RCC_CarControllerV3 lastActivePlayerVehicle;

	public RCC_Camera activePlayerCamera;

	public RCC_UIDashboardDisplay activePlayerCanvas;

	public Camera activeMainCamera;

	public bool registerFirstVehicleAsPlayer = true;

	public bool disableUIWhenNoPlayerVehicle;

	public bool loadCustomizationAtFirst = true;

	internal RCC_Recorder recorder;

	public RecordMode recordMode;

	private float orgTimeScale = 1f;

	public List<RCC_CarControllerV3> allVehicles = new List<RCC_CarControllerV3>();

	public static RCC_SceneManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Object.FindObjectOfType<RCC_SceneManager>();
				if (instance == null)
				{
					instance = new GameObject("_RCCSceneManager").AddComponent<RCC_SceneManager>();
				}
			}
			return instance;
		}
	}

	public static event onMainControllerChanged OnMainControllerChanged;

	public static event onBehaviorChanged OnBehaviorChanged;

	public static event onVehicleChanged OnVehicleChanged;

	private void Awake()
	{
		RCC_Camera.OnBCGCameraSpawned += RCC_Camera_OnBCGCameraSpawned;
		RCC_CarControllerV3.OnRCCPlayerSpawned += RCC_CarControllerV3_OnRCCSpawned;
		RCC_AICarController.OnRCCAISpawned += RCC_AICarController_OnRCCAISpawned;
		RCC_CarControllerV3.OnRCCPlayerDestroyed += RCC_CarControllerV3_OnRCCPlayerDestroyed;
		RCC_AICarController.OnRCCAIDestroyed += RCC_AICarController_OnRCCAIDestroyed;
		activePlayerCanvas = Object.FindObjectOfType<RCC_UIDashboardDisplay>();
		orgTimeScale = Time.timeScale;
		recorder = base.gameObject.GetComponent<RCC_Recorder>();
		if (!recorder)
		{
			recorder = base.gameObject.AddComponent<RCC_Recorder>();
		}
		if (RCC_Settings.Instance.lockAndUnlockCursor)
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
		XRSettings.enabled = RCC_Settings.Instance.useVR;
	}

	private void RCC_CarControllerV3_OnRCCSpawned(RCC_CarControllerV3 RCC)
	{
		if (!allVehicles.Contains(RCC))
		{
			allVehicles.Add(RCC);
		}
		if (registerFirstVehicleAsPlayer)
		{
			RegisterPlayer(RCC);
		}
	}

	private void RCC_AICarController_OnRCCAISpawned(RCC_AICarController RCCAI)
	{
		if (!allVehicles.Contains(RCCAI.carController))
		{
			allVehicles.Add(RCCAI.carController);
		}
	}

	private void RCC_Camera_OnBCGCameraSpawned(GameObject BCGCamera)
	{
		activePlayerCamera = BCGCamera.GetComponent<RCC_Camera>();
	}

	private void RCC_CarControllerV3_OnRCCPlayerDestroyed(RCC_CarControllerV3 RCC)
	{
		if (allVehicles.Contains(RCC))
		{
			allVehicles.Remove(RCC);
		}
	}

	private void RCC_AICarController_OnRCCAIDestroyed(RCC_AICarController RCCAI)
	{
		if (allVehicles.Contains(RCCAI.carController))
		{
			allVehicles.Remove(RCCAI.carController);
		}
	}

	private void Update()
	{
		if ((bool)activePlayerVehicle)
		{
			if (activePlayerVehicle != lastActivePlayerVehicle && RCC_SceneManager.OnVehicleChanged != null)
			{
				RCC_SceneManager.OnVehicleChanged();
			}
			lastActivePlayerVehicle = activePlayerVehicle;
		}
		if (disableUIWhenNoPlayerVehicle && (bool)activePlayerCanvas)
		{
			CheckCanvas();
		}
		if (Input.GetKey(RCC_Settings.Instance.slowMotionKB))
		{
			Time.timeScale = 0.2f;
		}
		if (Input.GetKeyUp(RCC_Settings.Instance.slowMotionKB))
		{
			Time.timeScale = orgTimeScale;
		}
		if (Input.GetButtonDown("Cancel"))
		{
			Cursor.lockState = CursorLockMode.None;
		}
		activeMainCamera = Camera.main;
		switch (recorder.mode)
		{
		case RCC_Recorder.Mode.Neutral:
			recordMode = RecordMode.Neutral;
			break;
		case RCC_Recorder.Mode.Play:
			recordMode = RecordMode.Play;
			break;
		case RCC_Recorder.Mode.Record:
			recordMode = RecordMode.Record;
			break;
		}
	}

	public void RegisterPlayer(RCC_CarControllerV3 playerVehicle)
	{
		activePlayerVehicle = playerVehicle;
		if ((bool)activePlayerCamera)
		{
			activePlayerCamera.SetTarget(activePlayerVehicle.gameObject);
		}
		if ((bool)Object.FindObjectOfType<RCC_CustomizerExample>())
		{
			Object.FindObjectOfType<RCC_CustomizerExample>().CheckUIs();
		}
	}

	public void RegisterPlayer(RCC_CarControllerV3 playerVehicle, bool isControllable)
	{
		activePlayerVehicle = playerVehicle;
		activePlayerVehicle.SetCanControl(isControllable);
		if ((bool)activePlayerCamera)
		{
			activePlayerCamera.SetTarget(activePlayerVehicle.gameObject);
		}
		if ((bool)Object.FindObjectOfType<RCC_CustomizerExample>())
		{
			Object.FindObjectOfType<RCC_CustomizerExample>().CheckUIs();
		}
	}

	public void RegisterPlayer(RCC_CarControllerV3 playerVehicle, bool isControllable, bool engineState)
	{
		activePlayerVehicle = playerVehicle;
		activePlayerVehicle.SetCanControl(isControllable);
		activePlayerVehicle.SetEngine(engineState);
		if ((bool)activePlayerCamera)
		{
			activePlayerCamera.SetTarget(activePlayerVehicle.gameObject);
		}
		if ((bool)Object.FindObjectOfType<RCC_CustomizerExample>())
		{
			Object.FindObjectOfType<RCC_CustomizerExample>().CheckUIs();
		}
	}

	public void DeRegisterPlayer()
	{
		if ((bool)activePlayerVehicle)
		{
			activePlayerVehicle.SetCanControl(state: false);
		}
		activePlayerVehicle = null;
		if ((bool)activePlayerCamera)
		{
			activePlayerCamera.RemoveTarget();
		}
	}

	public void CheckCanvas()
	{
		if (!activePlayerVehicle || !activePlayerVehicle.gameObject.activeInHierarchy || !activePlayerVehicle.enabled)
		{
			activePlayerCanvas.SetDisplayType(RCC_UIDashboardDisplay.DisplayType.Off);
		}
		else if (activePlayerCanvas.displayType != RCC_UIDashboardDisplay.DisplayType.Customization)
		{
			activePlayerCanvas.displayType = RCC_UIDashboardDisplay.DisplayType.Full;
		}
	}

	public static void SetBehavior(int behaviorIndex)
	{
		RCC_Settings.Instance.overrideBehavior = true;
		RCC_Settings.Instance.behaviorSelectedIndex = behaviorIndex;
		if (RCC_SceneManager.OnBehaviorChanged != null)
		{
			RCC_SceneManager.OnBehaviorChanged();
		}
	}

	public static void SetController(int controllerIndex)
	{
		RCC_Settings.Instance.controllerSelectedIndex = controllerIndex;
		switch (controllerIndex)
		{
		case 0:
			RCC_Settings.Instance.controllerType = RCC_Settings.ControllerType.Keyboard;
			break;
		case 1:
			RCC_Settings.Instance.controllerType = RCC_Settings.ControllerType.Mobile;
			break;
		case 2:
			RCC_Settings.Instance.controllerType = RCC_Settings.ControllerType.XBox360One;
			break;
		case 3:
			RCC_Settings.Instance.controllerType = RCC_Settings.ControllerType.Custom;
			break;
		}
		if (RCC_SceneManager.OnMainControllerChanged != null)
		{
			RCC_SceneManager.OnMainControllerChanged();
		}
	}

	public void ChangeCamera()
	{
		if ((bool)Instance.activePlayerCamera)
		{
			Instance.activePlayerCamera.ChangeCamera();
		}
	}

	private void OnDisable()
	{
		RCC_Camera.OnBCGCameraSpawned -= RCC_Camera_OnBCGCameraSpawned;
		RCC_CarControllerV3.OnRCCPlayerSpawned -= RCC_CarControllerV3_OnRCCSpawned;
		RCC_AICarController.OnRCCAISpawned -= RCC_AICarController_OnRCCAISpawned;
		RCC_CarControllerV3.OnRCCPlayerDestroyed -= RCC_CarControllerV3_OnRCCPlayerDestroyed;
		RCC_AICarController.OnRCCAIDestroyed -= RCC_AICarController_OnRCCAIDestroyed;
	}
}
