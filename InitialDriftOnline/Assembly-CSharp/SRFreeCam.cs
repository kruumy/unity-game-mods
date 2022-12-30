using System.Collections;
using CodeStage.AntiCheat.Storage;
using UnityEngine;

public class SRFreeCam : MonoBehaviour
{
	private RCC_Settings RCCSettingsInstance;

	public GameObject CameraRCC;

	public GameObject camcolider;

	public GameObject pivotorigcam;

	public int camrotspeedxbox;

	public float Speed;

	public bool FreeCamEnabled;

	private float dragsave;

	private float masssave;

	private int hudstate;

	public float distanceMax;

	public float downdistance;

	public GameObject FreeCamUI;

	public float camSens = 0.25f;

	private Vector3 lastMouse = new Vector3(255f, 255f, 255f);

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
		FreeCamEnabled = false;
		camcolider.GetComponent<BoxCollider>().enabled = false;
		camcolider.SetActive(value: false);
	}

	private void Update()
	{
		if (FreeCamEnabled)
		{
			float axis = Input.GetAxis("Vertical");
			float axis2 = Input.GetAxis("Horizontal");
			camcolider.transform.Translate(Vector3.forward * axis * Speed * Time.deltaTime);
			camcolider.transform.Translate(Vector3.right * axis2 * Speed * Time.deltaTime);
			Input.GetAxis("Empty_Axis");
			switch (PlayerPrefs.GetString("ControllerTypeChoose"))
			{
			case "LogitechSteeringWheel":
			case "Keyboard":
				lastMouse = Input.mousePosition - lastMouse;
				lastMouse = new Vector3((0f - lastMouse.y) * camSens, lastMouse.x * camSens, 0f);
				lastMouse = new Vector3(camcolider.transform.eulerAngles.x + lastMouse.x, camcolider.transform.eulerAngles.y + lastMouse.y, 0f);
				camcolider.transform.eulerAngles = lastMouse;
				lastMouse = Input.mousePosition;
				if ((Input.GetKeyDown(KeyCode.LeftShift) && ObscuredPrefs.GetInt("ONTYPING") == 0) || Input.GetKeyDown(KeyCode.Joystick1Button1))
				{
					Speed = 30f;
				}
				else if ((Input.GetKeyUp(KeyCode.LeftShift) && ObscuredPrefs.GetInt("ONTYPING") == 0) || Input.GetKeyUp(KeyCode.Joystick1Button1))
				{
					Speed = 15f;
				}
				MoveCondition();
				break;
			case "Xbox360One":
			case "PS4":
			{
				float num = Input.GetAxis("Xbox_MouseX") * (float)camrotspeedxbox * 20f * Time.deltaTime;
				float num2 = Input.GetAxis("Xbox_MouseY_invert") * (float)camrotspeedxbox * 20f * Time.deltaTime;
				float num3 = Input.GetAxis("Xbox_RB") * (float)camrotspeedxbox * 20f * Time.deltaTime;
				float num4 = Input.GetAxis("Xbox_LB") * (float)camrotspeedxbox * 20f * Time.deltaTime;
				camcolider.transform.Rotate(Vector3.right * num2);
				camcolider.transform.Rotate(Vector3.up * num, Space.World);
				camcolider.transform.Rotate(-Vector3.forward * num3);
				camcolider.transform.Rotate(Vector3.forward * num4);
				float axis3 = Input.GetAxis("Xbox_TriggerRight");
				Speed = 15f + 15f * axis3;
				MoveCondition();
				break;
			}
			}
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Rigidbody>().drag = 1000f;
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Rigidbody>().mass = 100000f;
			if ((Input.GetKeyDown(KeyCode.R) && ObscuredPrefs.GetInt("ONTYPING") == 0) || Input.GetKeyDown(KeyCode.Joystick1Button2))
			{
				SetFreemCam(jack: false);
			}
		}
		if (PlayerPrefs.GetInt("MenuOpen") == 0 && Input.GetKeyDown(KeyCode.F) && ObscuredPrefs.GetInt("ONTYPING") == 0 && PlayerPrefs.GetInt("ImInRun") == 0 && !FreeCamEnabled && RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().speed < 10f && Object.FindObjectOfType<RCC_Camera>().playerCar == RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>())
		{
			SetFreemCam(jack: true);
		}
		else if (Input.GetKeyDown(KeyCode.F) && ObscuredPrefs.GetInt("ONTYPING") == 0 && PlayerPrefs.GetInt("ImInRun") == 0 && FreeCamEnabled)
		{
			SetFreemCam(jack: false);
		}
	}

	public void MoveCondition()
	{
		float num = Vector2.Distance(RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.position, camcolider.transform.position);
		if (num >= distanceMax)
		{
			FreeCamUI.GetComponent<Animator>().Play("FreecamUIIdle");
			camcolider.transform.position = pivotorigcam.transform.position;
			camcolider.transform.rotation = pivotorigcam.transform.rotation;
		}
		else if (RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.position.y - downdistance > camcolider.transform.position.y)
		{
			FreeCamUI.GetComponent<Animator>().Play("FreecamUIIdle");
			camcolider.transform.position = pivotorigcam.transform.position;
			camcolider.transform.rotation = pivotorigcam.transform.rotation;
		}
		else if (num >= distanceMax - 3f)
		{
			FreeCamUI.GetComponent<Animator>().Play("FreecamAlert");
		}
	}

	public void SetFreemCam(bool jack)
	{
		if (jack)
		{
			FreeCamUI.GetComponent<Animator>().Play("FreecamUI");
			camcolider.SetActive(value: true);
			camcolider.transform.position = pivotorigcam.transform.position;
			camcolider.transform.rotation = pivotorigcam.transform.rotation;
			hudstate = Object.FindObjectOfType<Buttonkey>().state;
			Object.FindObjectOfType<Buttonkey>().state = 1;
			Object.FindObjectOfType<Buttonkey>().SetHud();
			Object.FindObjectOfType<SRUIManager>().CloseMenu();
			pivotorigcam.SetActive(value: false);
			masssave = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Rigidbody>().mass;
			dragsave = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Rigidbody>().drag;
			FreeCamEnabled = true;
			camcolider.GetComponent<BoxCollider>().enabled = true;
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().engineRunning = false;
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().PreviewSmokeParticle(state: false);
			StartCoroutine(disablercc(i: false));
			CameraRCC.GetComponent<RCC_Camera>().enabled = false;
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInParent<SRPlayerCollider>().AppelRPCSetGhostModeV2(10);
		}
		else if (!jack)
		{
			FreeCamUI.GetComponent<Animator>().Play("FreecamUIIdle");
			camcolider.SetActive(value: false);
			if (hudstate == 0)
			{
				hudstate = 1;
			}
			else
			{
				hudstate = 0;
			}
			pivotorigcam.SetActive(value: true);
			Object.FindObjectOfType<Buttonkey>().state = hudstate;
			Object.FindObjectOfType<Buttonkey>().SetHud();
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().gasInput = 0f;
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().engineRunning = true;
			camcolider.GetComponent<BoxCollider>().enabled = false;
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Rigidbody>().drag = dragsave;
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Rigidbody>().mass = masssave;
			StartCoroutine(disablercc(i: true));
			FreeCamEnabled = false;
			CameraRCC.GetComponent<RCC_Camera>().enabled = true;
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInParent<SRPlayerCollider>().AppelRPCSetGhostModeV2(8);
		}
	}

	private IEnumerator disablercc(bool i)
	{
		yield return new WaitForSeconds(1.2f);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().enabled = i;
	}
}
