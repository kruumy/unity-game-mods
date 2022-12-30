using System.Collections;
using System.IO;
using CodeStage.AntiCheat.Storage;
using HSVPicker;
using UnityEngine;
using UnityEngine.UI;

public class SRLightTunerUI : MonoBehaviour
{
	public GameObject LightManager;

	public GameObject LockTuner;

	public GameObject ColorSelector;

	public GameObject CameraPreview;

	public Text AccessPrice;

	private bool lightstate;

	public AudioClip NoMoneyClip;

	public AudioClip UnLockClip;

	public Image UnLockBtnImage;

	public Sprite XboxA;

	public Sprite KeyboardE;

	public ColorPicker oui;

	private int hudstate;

	public Color DefaultColor;

	private void Start()
	{
		LightManager.SetActive(value: false);
		CameraPreview.SetActive(value: false);
		StartCoroutine(SetCurrentColor());
	}

	private IEnumerator SetCurrentColor()
	{
		yield return new WaitForSeconds(2f);
		string text = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split('(')[0];
		if (PlayerPrefs.GetInt("FirstLightLoad_" + text) == 0)
		{
			ObscuredPrefs.SetColor("MyLightColor_" + text, DefaultColor);
			PlayerPrefs.SetInt("FirstLightLoad_" + text, 1);
			oui.CurrentColor = DefaultColor;
			Debug.Log("PREMIER SPAWN LIGHT POUR LA " + text);
		}
		else
		{
			oui.CurrentColor = ObscuredPrefs.GetColor("MyLightColor_" + text);
		}
		SendMyLightColor();
	}

	private void Update()
	{
	}

	public void SendMyLightColor()
	{
		string text = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split('(')[0];
		Color32 color = ObscuredPrefs.GetColor("MyLightColor_" + text);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().SendMyLightColor(color.r, color.g, color.b, color.a);
	}

	public void UpdateAfterSpawn()
	{
		StartCoroutine(SetCurrentColor());
	}

	public void SetColor(ColorPicker color)
	{
		string text = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split('(')[0];
		Color currentColor = color.CurrentColor;
		RCC_Light[] componentsInChildren = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentsInChildren<RCC_Light>();
		foreach (RCC_Light rCC_Light in componentsInChildren)
		{
			if (rCC_Light.lightType == RCC_Light.LightType.HeadLight || rCC_Light.lightType == RCC_Light.LightType.HighBeamHeadLight)
			{
				rCC_Light.GetComponent<Light>().color = currentColor;
			}
			ObscuredPrefs.SetColor("MyLightColor_" + text, currentColor);
		}
	}

	public void SetUI(bool jack)
	{
		LightManager.SetActive(jack);
		CameraPreview.SetActive(value: false);
		if (PlayerPrefs.GetString("ControllerTypeChoose") == "Xbox360One")
		{
			UnLockBtnImage.sprite = XboxA;
		}
		else
		{
			UnLockBtnImage.sprite = null;
		}
		if (jack && ObscuredPrefs.GetInt("LightTunerBuy") == 10)
		{
			lightstate = RCC_SceneManager.Instance.activePlayerVehicle.lowBeamHeadLightsOn;
			RCC_SceneManager.Instance.activePlayerVehicle.lowBeamHeadLightsOn = true;
			LockTuner.SetActive(value: false);
			ColorSelector.SetActive(value: true);
			CameraPreview.SetActive(value: true);
			hudstate = Object.FindObjectOfType<Buttonkey>().state;
			Object.FindObjectOfType<Buttonkey>().state = 1;
			Object.FindObjectOfType<Buttonkey>().SetHud();
		}
		else if (jack)
		{
			lightstate = RCC_SceneManager.Instance.activePlayerVehicle.lowBeamHeadLightsOn;
			LockTuner.SetActive(value: true);
			ColorSelector.SetActive(value: false);
			LockTuner.GetComponent<Button>().Select();
			LockTuner.GetComponent<Button>().interactable = false;
			LockTuner.GetComponent<Button>().interactable = true;
			LockTuner.GetComponent<Button>().Select();
			hudstate = Object.FindObjectOfType<Buttonkey>().state;
			Object.FindObjectOfType<Buttonkey>().state = 1;
			Object.FindObjectOfType<Buttonkey>().SetHud();
		}
		else if (!jack)
		{
			RCC_SceneManager.Instance.activePlayerVehicle.lowBeamHeadLightsOn = lightstate;
			LockTuner.SetActive(value: true);
			ColorSelector.SetActive(value: false);
			if (hudstate == 0)
			{
				hudstate = 1;
			}
			else
			{
				hudstate = 0;
			}
			Object.FindObjectOfType<Buttonkey>().state = hudstate;
			Object.FindObjectOfType<Buttonkey>().SetHud();
		}
	}

	public void BuyLightAccess()
	{
		// OLD: if (ObscuredPrefs.GetInt("MyBalance") >= 300)
		if ((ObscuredPrefs.GetInt("MyBalance") >= 300) || File.ReadAllLines("Settings.txt")[2].Split('=')[1] == "true")
		{
			RCC_SceneManager.Instance.activePlayerVehicle.lowBeamHeadLightsOn = true;
			ObscuredPrefs.SetInt("LightTunerBuy", 10);
			// OLD: ObscuredPrefs.SetInt("MyBalance", ObscuredPrefs.GetInt("MyBalance") - 300);
			if(File.ReadAllLines("Settings.txt")[2].Split('=')[1] != "true")
				ObscuredPrefs.SetInt("MyBalance", ObscuredPrefs.GetInt("MyBalance") - 300);
			GetComponent<AudioSource>().PlayOneShot(UnLockClip);
			LockTuner.SetActive(value: false);
			ColorSelector.SetActive(value: true);
			CameraPreview.SetActive(value: true);
		}
		else
		{
			AccessPrice.color = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);
			StartCoroutine(NoMoney());
			GetComponent<AudioSource>().PlayOneShot(NoMoneyClip);
		}
	}

	private IEnumerator NoMoney()
	{
		yield return new WaitForSeconds(0.5f);
		AccessPrice.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
	}
}
