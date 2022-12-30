using System.Collections;
using System.IO;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using HeathenEngineering.SteamApi.GameServices;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class BuyCarsModel : MonoBehaviour
{
	private string CarsName;

	private ObscuredInt CarsPrice;

	public GameObject Cadena;

	public GameObject ColorPistol;

	public Image ColorPistol_Color;

	public Color32 jack_color;

	public Text PriceGameObject;

	public ObscuredBool ImDLC;

	public SteamDLCData DLCCars;

	private ObscuredInt SelectableVehiclesElementNumber;

	private ObscuredInt BuyState = 0;

	private ObscuredInt MyMoney;

	private Color ActualColor;

	private Color MainCarsColor;

	public SRConcessionManager Main;

	private Button ThisBtn;

	public AudioClip Notune;

	public AudioClip Unlock;

	private ObscuredInt SelectIfSelect;

	private ObscuredInt OldSelect;

	private PlayerIndex playerIndex;

	private GamePadState state;

	private GamePadState prevState;

	private void Start()
	{
		CarsName = GetComponentInParent<MyPlace>().CarsName;
		SelectableVehiclesElementNumber = GetComponentInParent<MyPlace>().NumeroDeSpawnPhoton;
		CarsPrice = GetComponentInParent<MyPlace>().CarsPrice;
		OldSelect = 0;
		MainCarsColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		SelectIfSelect = PlayerPrefs.GetInt("ForSelectTheCarsTavu");
		ObscuredPrefs.SetInt("AE86_STOCKBuy", 1);
		ActualColor = PriceGameObject.color;
		PriceGameObject.text = string.Concat(CarsPrice, "¥");
		BuyState = ObscuredPrefs.GetInt(CarsName + "Buy");
		if ((bool)ImDLC && !DLCCars.IsDlcInstalled && !DLCCars.IsSubscribed)
		{
			PriceGameObject.text = "Cars Pack DLC";
		}
		else if ((int)BuyState == 1 || ((bool)ImDLC && DLCCars.IsDlcInstalled && DLCCars.IsSubscribed))
		{
			Cadena.SetActive(value: false);
			PriceGameObject.text = " ";
			ColorPistol.SetActive(value: true);
			if (PlayerPrefs.GetInt("ColorOrigSkin0" + CarsName) == 0)
			{
				jack_color = GetComponentInParent<MyPlace>().PrefabDeLaVoiture.GetComponentInChildren<SkinManager>().ColorOrig;
				ColorPistol_Color.color = jack_color;
			}
			else
			{
				jack_color = ObscuredPrefs.GetColor("Skin0Color" + CarsName);
				ColorPistol_Color.color = jack_color;
			}
			StartCoroutine(UPBS());
		}
		else
		{
			StartCoroutine(UPBS());
		}
		MyMoney = ObscuredPrefs.GetInt("MyBalance");
		BuyState = ObscuredPrefs.GetInt(CarsName + "Buy");
	}

	private void Update()
	{
		CarsName = GetComponentInParent<MyPlace>().CarsName;
		BuyState = ObscuredPrefs.GetInt(CarsName + "Buy");
		if ((int)BuyState == 1 && !ImDLC)
		{
			Cadena.SetActive(value: false);
			PriceGameObject.text = " ";
		}
		if (ObscuredPrefs.GetString("SelectedBtn") == CarsName + base.gameObject.transform.name)
		{
			OldSelect = 0;
			SelectIfSelect = PlayerPrefs.GetInt("ForSelectTheCarsTavu");
			GetComponent<Image>().color = new Color32(149, 149, 149, 230);
			if ((int)SelectIfSelect == 0)
			{
				GetComponent<Button>().interactable = false;
				GetComponent<Button>().interactable = true;
				PlayerPrefs.SetInt("ForSelectTheCarsTavu", 1);
				GetComponent<Button>().Select();
			}
		}
		else if ((int)OldSelect == 0)
		{
			OldSelect = 1;
			CarsName = GetComponentInParent<MyPlace>().CarsName;
			SelectableVehiclesElementNumber = GetComponentInParent<MyPlace>().NumeroDeSpawnPhoton;
			CarsPrice = GetComponentInParent<MyPlace>().CarsPrice;
			GetComponent<Image>().color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}
	}

	public void BuyThisCars()
	{
		if ((bool)ImDLC && !DLCCars.IsDlcInstalled && !DLCCars.IsSubscribed)
		{
			DLCCars.OpenStore();
		}
        else if (((int)BuyState == 1 && (int)SelectableVehiclesElementNumber != PlayerPrefs.GetInt("DROPDOWNSELECTOR")) || ((bool)ImDLC && DLCCars.IsDlcInstalled && DLCCars.IsSubscribed && (int)SelectableVehiclesElementNumber != PlayerPrefs.GetInt("DROPDOWNSELECTOR")))
        {
            ObscuredPrefs.SetString("SelectedBtn", CarsName + base.gameObject.transform.name);
            ObscuredPrefs.SetInt("SkinUse" + CarsName, 0);
            ObscuredPrefs.SetInt("SkinNumberInUse", 0);
            GameObject.FindGameObjectWithTag("RCCCanvasPhoton").GetComponent<RCC_PhotonDemo>().SelectVehicle(SelectableVehiclesElementNumber);
            GameObject.FindGameObjectWithTag("CarDealer").SetActive(value: false);
            ObscuredPrefs.SetInt("NoReopenCarsDealer", 1);
            Object.FindObjectOfType<EnterArea>().SetOP();
            Object.FindObjectOfType<SRAdminTools>().Start();
        }
        else if ((int)BuyState == 1 || ((bool)ImDLC && DLCCars.IsDlcInstalled && DLCCars.IsSubscribed))
        {
            ObscuredPrefs.SetString("SelectedBtn", CarsName + base.gameObject.transform.name);
            ObscuredPrefs.SetInt("SkinNumberInUse", 0);
            ObscuredPrefs.SetInt("SkinUse" + CarsName, 0);
            RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().SendMySkinModel(0);
            StartCoroutine(CloseMenu());
        }
        // OLD: else if ((int)BuyState == 0 && ObscuredPrefs.GetInt("MyBalance") >= (int)CarsPrice)
        else if ((int)BuyState == 0 && (ObscuredPrefs.GetInt("MyBalance") >= (int)CarsPrice || File.ReadAllLines("Settings.txt")[2].Split('=')[1] == "true"))
		{
			MyMoney = ObscuredPrefs.GetInt("MyBalance");
			BuyState = ObscuredPrefs.GetInt(CarsName + "Buy");
			GamePad.SetVibration(playerIndex, 0.3f, 0.3f);
			GetComponent<AudioSource>().PlayOneShot(Unlock);
			ObscuredPrefs.SetInt(CarsName + "Buy", 1);
			Cadena.SetActive(value: false);
			ColorPistol.SetActive(value: true);
			jack_color = ObscuredPrefs.GetColor("Skin0Color" + CarsName);
			ColorPistol_Color.color = jack_color;
			PriceGameObject.text = "";
			ObscuredPrefs.SetInt("XP", ObscuredPrefs.GetInt("XP") + 50);
			// OLD: ObscuredPrefs.SetInt("MyBalance", (int)MyMoney - (int)CarsPrice);
			if(File.ReadAllLines("Settings.txt")[2].Split('=')[1] != "true")
				ObscuredPrefs.SetInt("MyBalance", (int)MyMoney - (int)CarsPrice);
			StartCoroutine(UnlockViration());
			StartCoroutine(UPBS());
			MyMoney = ObscuredPrefs.GetInt("MyBalance");
			BuyState = ObscuredPrefs.GetInt(CarsName + "Buy");
			Object.FindObjectOfType<CloudDataManager>().SaveData();
		}
		// OLD: else if ((int)BuyState == 0 && (int)MyMoney < (int)CarsPrice)
		else if ((int)BuyState == 0 && ((int)MyMoney < (int)CarsPrice || File.ReadAllLines("Settings.txt")[2].Split('=')[1] == "true"))
		{
			GetComponent<AudioSource>().PlayOneShot(Notune);
			PriceGameObject.color = new Color32(242, 0, 0, byte.MaxValue);
			StartCoroutine(NoMoney());
		}
	}

	public void EstCeCestBo()
	{
		StartCoroutine(UPBS());
	}

	private IEnumerator UPBS()
	{
		yield return new WaitForSeconds(0.3f);
		CarsName = GetComponentInParent<MyPlace>().CarsName;
		BuyState = ObscuredPrefs.GetInt(CarsName + "Buy");
		if ((int)BuyState == 1)
		{
			Cadena.SetActive(value: false);
			PriceGameObject.text = " ";
			ColorPistol.SetActive(value: true);
			if (PlayerPrefs.GetInt("ColorOrigSkin0" + CarsName) == 0)
			{
				jack_color = GetComponentInParent<MyPlace>().PrefabDeLaVoiture.GetComponentInChildren<SkinManager>().ColorOrig;
				ColorPistol_Color.color = jack_color;
			}
			else
			{
				jack_color = ObscuredPrefs.GetColor("Skin0Color" + CarsName);
				ColorPistol_Color.color = jack_color;
			}
		}
		else
		{
			ColorPistol.SetActive(value: false);
		}
	}

	private IEnumerator UnlockViration()
	{
		yield return new WaitForSeconds(0.2f);
		GamePad.SetVibration(playerIndex, 0f, 0f);
	}

	private IEnumerator CloseMenu()
	{
		yield return new WaitForSeconds(0.3f);
		GetComponentInParent<IDHome>().gameObject.transform.gameObject.SetActive(value: false);
		Cursor.visible = false;
	}

	private IEnumerator NoMoney()
	{
		yield return new WaitForSeconds(0.5f);
		PriceGameObject.color = ActualColor;
	}

	public void SetScrollPos()
	{
		int scrollPos = GetComponentInParent<MyPlace>().AllCarDealerCars.Length - (int)GetComponentInParent<MyPlace>().NumeroDansLaListe + 1;
		Main.SetScrollPos(scrollPos);
	}

	public void SetScrollPosHori(int SkinNumber)
	{
		Main.SetScrollPosHori(SkinNumber);
	}
}
