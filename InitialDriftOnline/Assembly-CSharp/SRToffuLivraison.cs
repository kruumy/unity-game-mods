using System.Collections;
using CodeStage.AntiCheat.Storage;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using ZionBandwidthOptimizer.Examples;

public class SRToffuLivraison : MonoBehaviour
{
	private int LayerNumber;

	private int startuistate;

	public GameObject UIStart;

	public GameObject UIMessage;

	public GameObject Menu;

	public GameObject TutoManager;

	public GameObject ZoneDarriveEnable;

	public GameObject ZoneDarriveFalse;

	public string location;

	public string LangWelcome;

	public string LangDeliveryInProgress;

	public string LangSeeYouSoon;

	private string usedctrl;

	private void Start()
	{
		startuistate = 0;
		ObscuredPrefs.SetBool("TOFU RUN", value: false);
		UIStart.SetActive(value: false);
	}

	private void Update()
	{
		usedctrl = PlayerPrefs.GetString("ControllerTypeChoose");
		if (((Input.GetKeyDown(KeyCode.Joystick1Button0) && usedctrl == "Xbox360One") || (Input.GetKeyDown(KeyCode.Joystick1Button0) && usedctrl == "Keyboard") || (Input.GetKeyDown(KeyCode.Joystick1Button0) && usedctrl == "LogitechSteeringWheel") || (Input.GetButtonDown("PS4_X") && usedctrl == "PS4")) && startuistate == 1 && PlayerPrefs.GetInt("ImInRun") == 0 && PlayerPrefs.GetInt("MenuOpen") == 0 && UIStart.activeSelf)
		{
			YesBtn2();
			Object.FindObjectOfType<SRToffuManager>().YesBTN();
		}
		if (((Input.GetKeyDown(KeyCode.Joystick1Button1) && usedctrl == "Xbox360One") || (Input.GetKeyDown(KeyCode.Joystick1Button1) && usedctrl == "Keyboard") || (Input.GetKeyDown(KeyCode.Joystick1Button1) && usedctrl == "LogitechSteeringWheel") || (Input.GetButtonDown("PS4_Circle") && usedctrl == "PS4")) && startuistate == 1 && UIStart.activeSelf)
		{
			NoBtn();
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.GetComponentInParent<RCC_PhotonNetwork>().isMine && !ObscuredPrefs.GetBool("TOFU RUN") && PlayerPrefs.GetInt("ImInRun") == 0 && other.gameObject.tag != "ASKBATTLE")
		{
			if (other.gameObject.name != "AskBattleArea")
			{
				ObscuredPrefs.SetString("TOFULOCATION", location);
				Debug.Log("LOCATIOn :  " + ObscuredPrefs.GetString("TOFULOCATION"));
				other.gameObject.GetComponentInParent<SRPlayerCollider>().AppelRPCSetGhostModeV2(10);
				other.gameObject.tag = "PlayerCollider";
				UIStart.SetActive(value: true);
				StartCoroutine(CursorEn());
				Menu.SetActive(value: false);
				UIMessage.GetComponent<Text>().text = LangWelcome;
				UIMessage.GetComponent<Animator>().Play("UIMessage");
				ZoneDarriveEnable.SetActive(value: true);
				ZoneDarriveFalse.SetActive(value: false);
			}
		}
		else if (other.gameObject.GetComponentInParent<PhotonView>().IsMine)
		{
			UIMessage.GetComponent<Text>().text = LangDeliveryInProgress;
			UIMessage.GetComponent<Animator>().Play("UIMessage");
			startuistate = 0;
		}
		if (other.GetComponentInParent<RCC_PhotonNetwork>().isMine && ObscuredPrefs.GetInt("TutoTofuRuing") == 0 && (bool)TutoManager)
		{
			TutoManager.GetComponent<SRTutoManager>().InTofu();
		}
	}

	private IEnumerator CursorEn()
	{
		yield return new WaitForSeconds(2f);
		startuistate = 1;
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.GetComponentInParent<RCC_PhotonNetwork>().isMine && !ObscuredPrefs.GetBool("TOFU RUN") && other.gameObject.tag != "ASKBATTLE" && other.gameObject.name != "AskBattleArea")
		{
			ObscuredPrefs.SetString("TOFULOCATION", location);
			Debug.Log("LOCATIOn :  " + ObscuredPrefs.GetString("TOFULOCATION"));
			StopAllCoroutines();
			other.gameObject.GetComponentInParent<SRPlayerCollider>().AppelRPCSetGhostModeV2(8);
			other.gameObject.tag = "Player";
			UIStart.SetActive(value: false);
			startuistate = 0;
			UIMessage.GetComponent<Text>().text = LangSeeYouSoon;
			UIMessage.GetComponent<Animator>().Play("UIMessage");
			ZoneDarriveEnable.SetActive(value: false);
			ZoneDarriveFalse.SetActive(value: false);
			Debug.Log("EXIT DU SHOP : " + other.gameObject.name);
		}
	}

	private IEnumerator CursorDi()
	{
		yield return new WaitForSeconds(2f);
		startuistate = 0;
	}

	public void NoBtn()
	{
		UIStart.SetActive(value: false);
		UIMessage.GetComponent<Text>().text = LangSeeYouSoon;
		UIMessage.GetComponent<Animator>().Play("UIMessage");
		startuistate = 0;
	}

	public void YesBtn2()
	{
		ObscuredPrefs.SetBool("RespawnInTofu", value: false);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Animator>().Play("CagetteApear");
		UIStart.SetActive(value: false);
		startuistate = 0;
		Debug.Log("LOCATIOn :  " + ObscuredPrefs.GetString("TOFULOCATION"));
	}

	public void StopRunWithDeco()
	{
		StartCoroutine(StopRunWithDecoIE());
	}

	private IEnumerator StopRunWithDecoIE()
	{
		yield return new WaitForSeconds(7f);
		Object.FindObjectOfType<SRToffuManager>().StopDelivery();
	}
}
