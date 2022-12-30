using System;
using System.Collections;
using System.IO;
using CodeStage.AntiCheat.Storage;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ItemPhotonSystem : MonoBehaviour
{
	public GameObject MyAilerons;

	public GameObject MyAileronsDorig;

	private GameObject AileronsLocationOK;

	public GameObject NoSpoilerGo;

	public AudioClip Unlock;

	public AudioClip NoMoney;

	public Text Money;

	[SerializeField]
	public bool EnableUI;

	public GameObject UICommand;

	public GameObject Menu;

	public GameObject MenuGeneral;

	public GameObject ControllerBtnImg;

	public GameObject UiText;

	public string TradOpenMenu;

	public string TradSpoilerShop;

	public Sprite Xbox;

	public Sprite Keyboard;

	public Text SpoilerShopTitle;

	public GameObject SelectedButton;

	[Space]
	public GameObject[] Becquets;

	public GameObject[] AllBecquetsButton;

	public int[] AileronsPrice;

	private void Start()
	{
	}

	private void Update()
	{
		if ((Input.GetKeyDown(KeyCode.Joystick1Button0) && !MenuGeneral.activeSelf && EnableUI && !Menu.activeSelf) || (Input.GetKeyDown(KeyCode.E) && !MenuGeneral.activeSelf && EnableUI && !Menu.activeSelf && ObscuredPrefs.GetInt("ONTYPING") == 0))
		{
			SetPrice();
			Menu.SetActive(value: true);
		}
	}

	public void UICMD(bool jack)
	{
		SpoilerShopTitle.text = TradSpoilerShop;
		MenuGeneral.SetActive(value: false);
		UICommand.SetActive(jack);
		EnableUI = jack;
		if (!jack && Menu.activeSelf)
		{
			Menu.SetActive(value: false);
		}
		if (jack)
		{
			Money.text = ObscuredPrefs.GetInt("MyBalance") + "¥";
			if (PlayerPrefs.GetString("ControllerTypeChoose") == "Xbox360One")
			{
				UiText.GetComponent<Text>().text = TradOpenMenu;
				ControllerBtnImg.GetComponent<Image>().sprite = Xbox;
			}
			else
			{
				UiText.GetComponent<Text>().text = TradOpenMenu;
				ControllerBtnImg.GetComponent<Image>().sprite = Keyboard;
			}
		}
	}

	public void SpawnAilerons(int NumDuBecquet)
	{
		if ((bool)MyAilerons)
		{
			PhotonNetwork.Destroy(MyAilerons);
		}
		AileronsLocationOK = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerFonction>().AileronsLocations;
		Vector3 zero = Vector3.zero;
		Quaternion identity = Quaternion.identity;
		Vector3 zero2 = Vector3.zero;
		zero = AileronsLocationOK.transform.position;
		identity = AileronsLocationOK.transform.rotation;
		zero2 = AileronsLocationOK.transform.localScale;
		MyAilerons = PhotonNetwork.Instantiate("Ailerons/" + Becquets[NumDuBecquet].gameObject.name, zero, identity, 0);
		MyAilerons.transform.parent = AileronsLocationOK.transform;
		MyAilerons.transform.localScale = zero2;
		Debug.Log("SCALE : " + zero2);
		_ = MyAilerons.GetPhotonView().ViewID;
	}

	public void NoSpoiler(GameObject btn)
	{
		string text = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0];
		if ((bool)MyAilerons)
		{
			PhotonNetwork.Destroy(MyAilerons);
		}
		ObscuredPrefs.SetInt("UsedAileronsForFutureSpawn" + text, 100);
		Menu.SetActive(value: false);
		SelectedButton = btn;
		EnableUI = false;
		StartCoroutine(EnUI());
		ManageOrigineSpoiler(state: false);
	}

	public void OrigineSpoiler(GameObject btn)
	{
		string text = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0];
		if ((bool)MyAilerons)
		{
			PhotonNetwork.Destroy(MyAilerons);
		}
		ObscuredPrefs.SetInt("UsedAileronsForFutureSpawn" + text, 333);
		Menu.SetActive(value: false);
		SelectedButton = btn;
		EnableUI = false;
		StartCoroutine(EnUI());
		ManageOrigineSpoiler(state: true);
	}

	public void SetPrice()
	{
		int num = 0;
		string text = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0];
		if ((bool)SelectedButton)
		{
			StartCoroutine(SelectBtn());
		}
		else
		{
			SelectedButton = NoSpoilerGo;
			StartCoroutine(SelectBtn());
		}
		GameObject[] allBecquetsButton = AllBecquetsButton;
		foreach (GameObject gameObject in allBecquetsButton)
		{
			if (ObscuredPrefs.GetInt("BuyAilerons" + text + gameObject.name) == 0)
			{
				gameObject.GetComponentInChildren<Text>().text = AileronsPrice[num] + "¥";
				num++;
			}
			else
			{
				gameObject.GetComponentInChildren<Text>().text = "";
			}
		}
	}

	private IEnumerator SelectBtn()
	{
		SelectedButton.GetComponent<Button>().Select();
		SelectedButton.GetComponent<Button>().interactable = false;
		yield return new WaitForSeconds(0.05f);
		SelectedButton.GetComponent<Button>().interactable = true;
		SelectedButton.GetComponent<Button>().Select();
	}

	public void BuyAilerons(GameObject btn)
	{
		string text = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0];
		int num = Convert.ToInt32(btn.name.Split('(')[1].Replace(")", ""));
		int num2 = 100000;
		if (btn.GetComponentInChildren<Text>().text.Contains("¥"))
		{
			num2 = Convert.ToInt32(btn.GetComponentInChildren<Text>().text.Replace("¥", ""));
		}
		Money.text = ObscuredPrefs.GetInt("MyBalance") + "¥";
        
        // OLD: if (ObscuredPrefs.GetInt("BuyAilerons" + text + btn.name) == 0 && ObscuredPrefs.GetInt("MyBalance") >= num2)
        if (ObscuredPrefs.GetInt("BuyAilerons" + text + btn.name) == 0 && (ObscuredPrefs.GetInt("MyBalance") >= num2 || File.ReadAllLines("Settings.txt")[2].Split('=')[1] == "true"))
		{
			btn.GetComponentInChildren<Text>().text = "";
			ObscuredPrefs.SetInt("BuyAilerons" + text + btn.name, 10);
			ObscuredPrefs.SetInt("BuyAileronsNumber" + text + num, 10);
			GetComponent<AudioSource>().PlayOneShot(Unlock);
			// OLD: ObscuredPrefs.SetInt("MyBalance", ObscuredPrefs.GetInt("MyBalance") - num2);
			if(File.ReadAllLines("Settings.txt")[2].Split('=')[1] != "true")
				ObscuredPrefs.SetInt("MyBalance", ObscuredPrefs.GetInt("MyBalance") - num2);
		}
		// OLD: else if (ObscuredPrefs.GetInt("BuyAilerons" + text + btn.name) == 0 && ObscuredPrefs.GetInt("MyBalance") < num2)
		else if (ObscuredPrefs.GetInt("BuyAilerons" + text + btn.name) == 0 && (ObscuredPrefs.GetInt("MyBalance") < num2 || File.ReadAllLines("Settings.txt")[2].Split('=')[1] == "true"))
		{
			StartCoroutine(NoMoneyColor(btn));
			btn.GetComponentInChildren<Text>().color = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);
			GetComponent<AudioSource>().PlayOneShot(NoMoney);
		}
        else if (ObscuredPrefs.GetInt("BuyAilerons" + text + btn.name) == 10)
        {
            EnableUI = false;
            StartCoroutine(EnUI());
            SelectedButton = btn;
            Menu.SetActive(value: false);
            ObscuredPrefs.SetInt("UsedAileronsForFutureSpawn" + text, num);
            SpawnAilerons(num);
            ManageOrigineSpoiler(state: false);
        }
        Money.text = ObscuredPrefs.GetInt("MyBalance") + "¥";
	}

	private IEnumerator NoMoneyColor(GameObject btn)
	{
		yield return new WaitForSeconds(1f);
		btn.GetComponentInChildren<Text>().color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
	}

	private IEnumerator EnUI()
	{
		yield return new WaitForSeconds(0.3f);
		EnableUI = true;
	}

	public void SetAfterSpawn()
	{
		string text = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0];
		int @int = ObscuredPrefs.GetInt("UsedAileronsForFutureSpawn" + text);
		SelectedButton = NoSpoilerGo;
		if (ObscuredPrefs.GetInt("BuyAileronsNumber" + text + @int) == 10)
		{
			ManageOrigineSpoiler(state: false);
			SpawnAilerons(@int);
		}
		switch (@int)
		{
		case 0:
			if (ObscuredPrefs.GetInt("BuyAileronsNumber" + text + @int) != 0)
			{
				break;
			}
			goto case 333;
		case 333:
			ManageOrigineSpoiler(state: true);
			break;
		}
	}

	public void ManageOrigineSpoiler(bool state)
	{
	}
}
