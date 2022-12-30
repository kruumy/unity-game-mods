using System;
using System.Collections;
using System.IO;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class BuySkinModel : MonoBehaviour
{
	[Header("!! IMPORTANT !!")]
	public string SkinNameForPlayerpref;

	public ObscuredInt NumeroDuSkinDansLeChassisDeLaVoiture;

	[Space]
	[Space]
	private string CarsName;

	private ObscuredInt SkinNumber;

	private ObscuredInt SkinPrice;

	[Space]
	public GameObject Cadena;

	public Text PriceGameObject;

	public ScrollRect SRR;

	public GameObject MainCars;

	private int SelectedVehiculeNumber;

	private ObscuredInt BuyState = 0;

	private ObscuredInt MyMoney;

	private ObscuredInt ModelBuying;

	private Color ActualColor;

	private Color MainCarsColor;

	public AudioClip Notune;

	public AudioClip Unlock;

	private ObscuredInt SelectIfSelect;

	private ObscuredInt OldSelect;

	private string NumberInString;

	private PlayerIndex playerIndex;

	private GamePadState state;

	private GamePadState prevState;

	private void Start()
	{
		if (SkinNameForPlayerpref == "")
		{
			SkinNameForPlayerpref = base.gameObject.name;
		}
		CarsName = GetComponentInParent<MyPlace>().CarsName;
		SelectedVehiculeNumber = GetComponentInParent<MyPlace>().NumeroDeSpawnPhoton;
		string[] array = base.gameObject.transform.name.Split('(');
		NumberInString = array[1].Replace(")", "");
		SkinNumber = Convert.ToInt32(NumberInString);
		SkinPrice = GetComponentInParent<MyPlace>().PriceSkin[(int)SkinNumber - 1];
		OldSelect = 0;
		ActualColor = PriceGameObject.color;
		MainCarsColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		BuyState = ObscuredPrefs.GetInt(CarsName + "Skin" + SkinNameForPlayerpref);
		PriceGameObject.text = string.Concat(SkinPrice, "¥");
		if ((int)BuyState == 1)
		{
			Cadena.SetActive(value: false);
			PriceGameObject.text = "";
		}
		MyMoney = ObscuredPrefs.GetInt("MyBalance");
		BuyState = ObscuredPrefs.GetInt(CarsName + "Skin" + SkinNameForPlayerpref);
		ModelBuying = ObscuredPrefs.GetInt(CarsName + "Buy");
	}

	private void Update()
	{
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
			CarsName = GetComponentInParent<MyPlace>().CarsName;
			SelectedVehiculeNumber = GetComponentInParent<MyPlace>().NumeroDeSpawnPhoton;
			string value = base.gameObject.transform.name.Split('(')[1].Replace(")", "");
			SkinNumber = Convert.ToInt32(value);
			BuyState = ObscuredPrefs.GetInt(CarsName + "Skin" + SkinNameForPlayerpref);
			SkinPrice = GetComponentInParent<MyPlace>().PriceSkin[(int)SkinNumber - 1];
			if ((int)BuyState == 1)
			{
				Cadena.SetActive(value: false);
				PriceGameObject.text = "";
			}
			OldSelect = 1;
			GetComponent<Image>().color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}
	}

	public void BuyThisSkin()
	{
		ModelBuying = ObscuredPrefs.GetInt(CarsName + "Buy");
		BuyState = ObscuredPrefs.GetInt(CarsName + "Skin" + SkinNameForPlayerpref);
		MyMoney = ObscuredPrefs.GetInt("MyBalance");
		if ((int)ModelBuying == 0)
		{
			MainCars.GetComponent<Button>().interactable = false;
			MainCars.GetComponent<Button>().interactable = true;
			MainCars.GetComponent<Button>().Select();
			MainCars.GetComponent<Image>().color = new Color32(126, 0, 0, 230);
			StartCoroutine(ChooseTheCar());
		}
        else if ((int)BuyState == 1 && SelectedVehiculeNumber != PlayerPrefs.GetInt("DROPDOWNSELECTOR"))
        {
            ObscuredPrefs.SetString("SelectedBtn", CarsName + base.gameObject.transform.name);
            ObscuredPrefs.SetInt("SkinNumberInUse", NumeroDuSkinDansLeChassisDeLaVoiture);
            ObscuredPrefs.SetInt("SkinUse" + CarsName, NumeroDuSkinDansLeChassisDeLaVoiture);
            GameObject.FindGameObjectWithTag("RCCCanvasPhoton").GetComponent<RCC_PhotonDemo>().SelectVehicle(SelectedVehiculeNumber);
            ObscuredPrefs.SetInt("NoReopenCarsDealer", 1);
            UnityEngine.Object.FindObjectOfType<EnterArea>().SetOP();
            UnityEngine.Object.FindObjectOfType<SRAdminTools>().Start();
            StartCoroutine(CloseMenu());
        }
        else if ((int)BuyState == 1)
        {
            ObscuredPrefs.SetString("SelectedBtn", CarsName + base.gameObject.transform.name);
            ObscuredPrefs.SetInt("SkinNumberInUse", NumeroDuSkinDansLeChassisDeLaVoiture);
            ObscuredPrefs.SetInt("SkinUse" + CarsName, NumeroDuSkinDansLeChassisDeLaVoiture);
            RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().SendMySkinModel(SkinNumber);
            StartCoroutine(CloseMenu());
        }
        // OLD: else if ((int)BuyState == 0 && ObscuredPrefs.GetInt("MyBalance") >= (int)SkinPrice)
        else if ((int)BuyState == 0 && (ObscuredPrefs.GetInt("MyBalance") >= (int)SkinPrice || File.ReadAllLines("Settings.txt")[2].Split('=')[1] == "true"))
		{
			GamePad.SetVibration(playerIndex, 0.3f, 0.3f);
			StartCoroutine(UnlockViration());
			GetComponent<AudioSource>().PlayOneShot(Unlock);
			ObscuredPrefs.SetInt(CarsName + "Skin" + SkinNameForPlayerpref, 1);
			Cadena.SetActive(value: false);
			PriceGameObject.text = "";
			ObscuredPrefs.SetInt("XP", ObscuredPrefs.GetInt("XP") + 30);
			if(File.ReadAllLines("Settings.txt")[2].Split('=')[1] != "true")
				ObscuredPrefs.SetInt("MyBalance", (int)MyMoney - (int)SkinPrice);
			// OLD: ObscuredPrefs.SetInt("MyBalance", (int)MyMoney - (int)SkinPrice);
			UnityEngine.Object.FindObjectOfType<CloudDataManager>().SaveData();
		}
		// OLD: else if ((int)BuyState == 0 && (int)MyMoney < (int)SkinPrice)
		else if ((int)BuyState == 0 && ((int)MyMoney < (int)SkinPrice || File.ReadAllLines("Settings.txt")[2].Split('=')[1] == "true"))
		{
			GamePad.SetVibration(playerIndex, 0.5f, 0.5f);
			StartCoroutine(UnlockViration());
			GetComponent<AudioSource>().PlayOneShot(Notune);
			PriceGameObject.color = new Color32(242, 0, 0, byte.MaxValue);
			StartCoroutine(NoMoney());
		}
		MyMoney = ObscuredPrefs.GetInt("MyBalance");
		BuyState = ObscuredPrefs.GetInt(CarsName + "Skin" + SkinNameForPlayerpref);
		ModelBuying = ObscuredPrefs.GetInt(CarsName + "Buy");
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

	private IEnumerator ChooseTheCar()
	{
		yield return new WaitForSeconds(0.5f);
		MainCarsColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		MainCars.GetComponent<Image>().color = MainCarsColor;
	}
}
