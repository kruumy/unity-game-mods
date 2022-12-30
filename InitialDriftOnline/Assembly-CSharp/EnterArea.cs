using System.Collections;
using CodeStage.AntiCheat.Storage;
using UnityEngine;
using UnityEngine.UI;
using ZionBandwidthOptimizer.Examples;

public class EnterArea : MonoBehaviour
{
	public GameObject CarDealer;

	public GameObject Menu;

	public ScrollRect SRR;

	public Button FirstCarsButton;

	public GameObject UIMessage;

	public GameObject CommandeInfo;

	public GameObject ControllerBtnImg;

	public GameObject LastPartTextOpen;

	public GameObject firstparttextopen;

	private int JACK;

	public string VendeurName;

	public Sprite Keyboard;

	public Sprite Xbox;

	public Sprite PS4;

	public GameObject TutoManager;

	public string LangWelcome;

	public string ToOpenCarDealer;

	public string LangSeeYouSoon;

	private string usedctrl;

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
				LastPartTextOpen.GetComponent<Text>().text = ToOpenCarDealer;
				CommandeInfo.SetActive(value: true);
				ControllerBtnImg.GetComponent<Image>().sprite = Xbox;
			}
			else if (PlayerPrefs.GetString("ControllerTypeChoose") == "PS4")
			{
				firstparttextopen.GetComponent<Text>().fontSize = LastPartTextOpen.GetComponent<Text>().fontSize;
				LastPartTextOpen.GetComponent<Text>().text = ToOpenCarDealer;
				CommandeInfo.SetActive(value: true);
				ControllerBtnImg.GetComponent<Image>().sprite = PS4;
			}
			else
			{
				LastPartTextOpen.GetComponent<Text>().text = ToOpenCarDealer;
				CommandeInfo.SetActive(value: true);
				ControllerBtnImg.GetComponent<Image>().sprite = Keyboard;
			}
			if (!CarDealer.activeSelf && ObscuredPrefs.GetInt("NoReopenCarsDealer") == 0 && PlayerPrefs.GetInt("ImInRun") == 0 && RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerFonction>().TopCamera.GetComponent<Camera>().enabled)
			{
				if ((Input.GetKeyDown(KeyCode.Joystick1Button0) && !Menu.activeSelf && usedctrl == "Xbox360One") || (Input.GetKeyDown(KeyCode.Joystick1Button0) && !Menu.activeSelf && usedctrl == "LogitechSteeringWheel") || (Input.GetKeyDown(KeyCode.E) && !Menu.activeSelf && ObscuredPrefs.GetInt("ONTYPING") == 0) || Input.GetButtonDown("PS4_X"))
				{
					PlayerPrefs.SetInt("ForSelectTheCarsTavu", 0);
					SRR.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
					CarDealer.SetActive(value: true);
					BuyCarsModel[] array = Object.FindObjectsOfType<BuyCarsModel>();
					for (int i = 0; i < array.Length; i++)
					{
						array[i].EstCeCestBo();
					}
					if (ObscuredPrefs.GetString("SelectedBtn") == "")
					{
						StartCoroutine(SelectFirstCarsBtn());
					}
				}
			}
			else if (CarDealer.activeSelf)
			{
				ObscuredPrefs.SetInt("NoReopenCarsDealer", 0);
				if (Input.GetKeyDown(KeyCode.Joystick1Button1))
				{
					CarDealer.SetActive(value: false);
				}
			}
		}
		else if (JACK == 0)
		{
			CarDealer.SetActive(value: false);
		}
	}

	private IEnumerator SelectFirstCarsBtn()
	{
		FirstCarsButton.Select();
		FirstCarsButton.interactable = false;
		yield return new WaitForSeconds(0.05f);
		FirstCarsButton.interactable = true;
		FirstCarsButton.Select();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.GetComponentInParent<RCC_PhotonNetwork>().isMine && ObscuredPrefs.GetInt("NoReopenCarsDealer") == 0 && PlayerPrefs.GetInt("ImInRun") == 0 && !ObscuredPrefs.GetBool("TOFU RUN"))
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
		else if (other.GetComponentInParent<RCC_PhotonNetwork>().isMine)
		{
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().AppelRPCSetGhostModeV2(10);
			other.gameObject.tag = "PlayerCollider";
		}
		if (other.GetComponentInParent<RCC_PhotonNetwork>().isMine && ObscuredPrefs.GetInt("TutoConcessRuning") == 0)
		{
			TutoManager.GetComponent<SRTutoManager>().InConcess();
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.GetComponentInParent<RCC_PhotonNetwork>().isMine && PlayerPrefs.GetInt("ImInRun") == 0 && !ObscuredPrefs.GetBool("TOFU RUN"))
		{
			ObscuredPrefs.SetInt("NoReopenCarsDealer", 0);
			JACK = 0;
			CarDealer.SetActive(value: false);
			CommandeInfo.SetActive(value: false);
			UIMessage.GetComponent<Text>().text = VendeurName + LangSeeYouSoon;
			UIMessage.GetComponent<Animator>().Play("UIMessage");
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().AppelRPCSetGhostModeV2(8);
			other.gameObject.tag = "Player";
			GameObject.FindGameObjectWithTag("ShadowManager").GetComponent<SRShadowManager>().Start();
		}
	}

	public void SetOP()
	{
		StartCoroutine(NoMoney2());
	}

	private IEnumerator NoMoney2()
	{
		yield return new WaitForSeconds(0.5f);
		ObscuredPrefs.SetInt("NoReopenCarsDealer", 0);
	}
}
