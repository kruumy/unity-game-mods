using System.Collections;
using System.IO;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class UnlockItem : MonoBehaviour
{
	public ObscuredInt Price;

	public Text PriceText;

	public string PlayerPrefName;

	public GameObject Lock;

	private int MyMoney;

	private Color BaseText;

	public AudioClip NoMoneyClip;

	public AudioClip Unlock;

	private PlayerIndex playerIndex;

	private GamePadState state;

	private GamePadState prevState;

	private void Start()
	{
		if (ObscuredPrefs.GetInt(RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0] + PlayerPrefName + "Lock") == 5)
		{
			Lock.SetActive(value: false);
			GetComponent<Button>().enabled = true;
		}
		else
		{
			Lock.SetActive(value: true);
			GetComponent<Button>().enabled = false;
		}
		BaseText = PriceText.color;
		PriceText.text = string.Concat(Price, "¥");
	}

	private void Update()
	{
		if (ObscuredPrefs.GetInt(RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0] + PlayerPrefName + "Lock") == 5)
		{
			Lock.SetActive(value: false);
			GetComponent<Button>().enabled = true;
		}
		else
		{
			Lock.SetActive(value: true);
			GetComponent<Button>().enabled = false;
		}
		MyMoney = ObscuredPrefs.GetInt("MyBalance");
	}

	public void BuyThisItem()
	{
		string text = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0];
		if (MyMoney >= (int)Price)
		{
			GamePad.SetVibration(playerIndex, 0.3f, 0.3f);
			StartCoroutine(UnlockViration());
			GetComponent<AudioSource>().PlayOneShot(Unlock);
			// OLD: ObscuredPrefs.SetInt("MyBalance", MyMoney - (int)Price);
			if(File.ReadAllLines("Settings.txt")[2].Split('=')[1] != "true")
				ObscuredPrefs.SetInt("MyBalance", MyMoney - (int)Price);
			ObscuredPrefs.SetInt(text + PlayerPrefName + "Lock", 5);
			Lock.SetActive(value: false);
			GetComponent<Button>().enabled = true;
			GetComponent<Button>().Select();
		}
		else
		{
			GetComponent<AudioSource>().PlayOneShot(NoMoneyClip);
			PriceText.color = new Color32(byte.MaxValue, 45, 0, byte.MaxValue);
			StartCoroutine(NoMoney());
		}
	}

	private IEnumerator UnlockViration()
	{
		yield return new WaitForSeconds(0.2f);
		GamePad.SetVibration(playerIndex, 0f, 0f);
	}

	private IEnumerator NoMoney()
	{
		yield return new WaitForSeconds(0.5f);
		PriceText.color = BaseText;
	}
}
