using System.Collections;
using CodeStage.AntiCheat.Storage;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;
using ZionBandwidthOptimizer.Examples;

public class EnterAreaGarage : MonoBehaviour
{
	public GameObject CarDealer;

	public GameObject Menu;

	public GameObject ListButton;

	public Button FirstCarsButton;

	public GameObject UIMessage;

	public GameObject CommandeInfo;

	public GameObject ControllerBtnImg;

	public GameObject LastPartTextOpen;

	public GameObject firstparttextopen;

	public Sprite Keyboard;

	public Sprite Xbox;

	public Sprite PS4;

	private int JACK;

	public string VendeurName;

	public GarageManager GarageMana;

	public CarsPreview DisableCamUI;

	public AudioClip Survol;

	public GameObject TutoManager;

	private string usedctrl;

	public string LangWelcome;

	public string ToOpenCarGarage;

	public string LangSeeYouSoon;

	private void Start()
	{
		ObscuredPrefs.SetInt("NoReopenCarsDealer", 0);
	}

	private void Update()
	{
		if (JACK == 1)
		{
			usedctrl = PlayerPrefs.GetString("ControllerTypeChoose");
			if (Menu.activeSelf || CarDealer.activeSelf)
			{
				CommandeInfo.SetActive(value: false);
			}
			else if (PlayerPrefs.GetString("ControllerTypeChoose") == "Xbox360One")
			{
				firstparttextopen.GetComponent<Text>().fontSize = LastPartTextOpen.GetComponent<Text>().fontSize;
				LastPartTextOpen.GetComponent<Text>().text = ToOpenCarGarage;
				CommandeInfo.SetActive(value: true);
				ControllerBtnImg.GetComponent<Image>().sprite = Xbox;
			}
			else if (PlayerPrefs.GetString("ControllerTypeChoose") == "PS4")
			{
				firstparttextopen.GetComponent<Text>().fontSize = LastPartTextOpen.GetComponent<Text>().fontSize;
				LastPartTextOpen.GetComponent<Text>().text = ToOpenCarGarage;
				CommandeInfo.SetActive(value: true);
				ControllerBtnImg.GetComponent<Image>().sprite = PS4;
			}
			else
			{
				LastPartTextOpen.GetComponent<Text>().text = ToOpenCarGarage;
				CommandeInfo.SetActive(value: true);
				ControllerBtnImg.GetComponent<Image>().sprite = Keyboard;
			}
			if (!CarDealer.activeSelf && PlayerPrefs.GetInt("ImInRun") == 0 && ((Input.GetKeyDown(KeyCode.Joystick1Button0) && !Menu.activeSelf && usedctrl == "Xbox360One") || (Input.GetKeyDown(KeyCode.Joystick1Button0) && !Menu.activeSelf && usedctrl == "LogitechSteeringWheel") || (Input.GetKeyDown(KeyCode.E) && !Menu.activeSelf && ObscuredPrefs.GetInt("ONTYPING") == 0) || Input.GetButtonDown("PS4_X")))
			{
				CarDealer.SetActive(value: true);
				FirstCarsButton.Select();
				GarageMana.HGomeGarage();
				SteamUserStats.SetAchievement("USEGARAGE");
				SteamUserStats.StoreStats();
			}
		}
		else if (JACK == 0)
		{
			CarDealer.SetActive(value: false);
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.GetComponentInParent<RCC_PhotonNetwork>().isMine && PlayerPrefs.GetInt("ImInRun") == 0 && !ObscuredPrefs.GetBool("TOFU RUN"))
		{
			JACK = 1;
			if (Menu.activeSelf)
			{
				Menu.SetActive(value: false);
			}
			UIMessage.GetComponent<Text>().text = VendeurName + LangWelcome;
			UIMessage.GetComponent<Animator>().Play("UIMessage");
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().AppelRPCSetGhostModeV2(10);
			other.gameObject.tag = "PlayerCollider";
		}
		if (other.GetComponentInParent<RCC_PhotonNetwork>().isMine && ObscuredPrefs.GetInt("TutoGaragRuning") == 0)
		{
			TutoManager.GetComponent<SRTutoManager>().InGarage();
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.GetComponentInParent<RCC_PhotonNetwork>().isMine && PlayerPrefs.GetInt("ImInRun") == 0 && !ObscuredPrefs.GetBool("TOFU RUN"))
		{
			JACK = 0;
			CarDealer.SetActive(value: false);
			CommandeInfo.SetActive(value: false);
			UIMessage.GetComponent<Text>().text = VendeurName + LangSeeYouSoon;
			UIMessage.GetComponent<Animator>().Play("UIMessage");
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().AppelRPCSetGhostModeV2(8);
			other.gameObject.tag = "Player";
			Object.FindObjectOfType<RCC_Camera>().ResetCamera();
			StartCoroutine(SetCamRot2());
		}
	}

	private IEnumerator SetCamRot2()
	{
		yield return new WaitForSeconds(0.1f);
		PlayerPrefs.SetInt("WANTROT", 0);
	}
}
