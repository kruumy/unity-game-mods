using System.Collections;
using CodeStage.AntiCheat.Storage;
using UnityEngine;
using UnityEngine.UI;
using ZionBandwidthOptimizer.Examples;

public class GarageManager : MonoBehaviour
{
	public GameObject ButtonList;

	public GameObject MenuCamber;

	public GameObject MenuSuspension;

	public GameObject MenuTorque;

	public GameObject Freinage;

	public GameObject Speed;

	public GameObject SettinsEngine;

	public GameObject CarsPreview;

	public Slider frontCamber;

	public Slider rearCamber;

	public Slider frontSuspensionDistances;

	public Slider rearSuspensionDistances;

	public Text Money;

	public Text FrontSusTxt;

	public Text RearSustxt;

	public Button FirstButton;

	public Toggle turbo;

	public Toggle exhaustFlame;

	public Toggle revLimiter;

	public Toggle clutchMargin;

	private void Start()
	{
		string text = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0];
		frontCamber.value = PlayerPrefs.GetFloat(text + "_FrontCamberTemp");
		rearCamber.value = PlayerPrefs.GetFloat(text + "_RearCamberTemp");
		frontSuspensionDistances.value = PlayerPrefs.GetFloat(text + "_FrontSuspensionsDistanceTemp");
		rearSuspensionDistances.value = PlayerPrefs.GetFloat(text + "_RearSuspensionsDistanceTemp");
		turbo.isOn = RCC_PlayerPrefsX.GetBool(text + "TurboTemp");
		exhaustFlame.isOn = RCC_PlayerPrefsX.GetBool(text + "ExhaustFlameTemp");
		revLimiter.isOn = RCC_PlayerPrefsX.GetBool(text + "RevLimiterTemp");
		clutchMargin.isOn = RCC_PlayerPrefsX.GetBool(text + "ClutchMarginTemp");
		UpdateToglleValue();
		HGomeGarage();
	}

	private void Update()
	{
		Money.text = ObscuredPrefs.GetInt("MyBalance") + "¥";
	}

	public void SendInfoRPC()
	{
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_PhotonNetwork>().Sendinfo();
	}

	public void SendInfo2RPC()
	{
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_PhotonNetwork>().Sendinfo2();
	}

	public void SendInfo3RPC()
	{
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_PhotonNetwork>().Sendinfo3();
	}

	public void HGomeGarage()
	{
		Object.FindObjectOfType<RCC_Camera>().ResetCamera();
		CarsPreview.SetActive(value: true);
		MenuCamber.SetActive(value: false);
		MenuSuspension.SetActive(value: false);
		MenuTorque.SetActive(value: false);
		Freinage.SetActive(value: false);
		Speed.SetActive(value: false);
		SettinsEngine.SetActive(value: false);
		ButtonList.SetActive(value: true);
		StartCoroutine(FirstSelect());
	}

	private IEnumerator FirstSelect()
	{
		yield return new WaitForSeconds(0.1f);
		PlayerPrefs.SetInt("WANTROT", 0);
		FirstButton.interactable = false;
		FirstButton.interactable = true;
		FirstButton.Select();
	}

	public void UpTxtSus()
	{
		FrontSusTxt.text = (frontSuspensionDistances.value * 10f).ToString("0.00") ?? "";
		RearSustxt.text = (rearSuspensionDistances.value * 10f).ToString("0.00") ?? "";
	}

	public void UpdateToglleValue()
	{
		string text = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0];
		frontCamber.value = PlayerPrefs.GetFloat(text + "_FrontCamberTemp");
		rearCamber.value = PlayerPrefs.GetFloat(text + "_RearCamberTemp");
		frontSuspensionDistances.value = PlayerPrefs.GetFloat(text + "_FrontSuspensionsDistanceTemp");
		rearSuspensionDistances.value = PlayerPrefs.GetFloat(text + "_RearSuspensionsDistanceTemp");
		turbo.isOn = RCC_PlayerPrefsX.GetBool(text + "TurboTemp");
		exhaustFlame.isOn = RCC_PlayerPrefsX.GetBool(text + "ExhaustFlameTemp");
		revLimiter.isOn = RCC_PlayerPrefsX.GetBool(text + "RevLimiterTemp");
		clutchMargin.isOn = RCC_PlayerPrefsX.GetBool(text + "ClutchMarginTemp");
		Debug.Log("TempoCarsName_FrontSuspensionsDistanceTemp = " + text + "_FrontSuspensionsDistanceTemp = " + PlayerPrefs.GetFloat(text + "_FrontSuspensionsDistanceTemp"));
		Debug.Log("TempoCarsName_RearSuspensionsDistanceTemp = " + text + "_RearSuspensionsDistanceTemp = " + PlayerPrefs.GetFloat(text + "_RearSuspensionsDistanceTemp"));
	}

	public void rotationCam(int rotval)
	{
		Object.FindObjectOfType<RCC_Camera>().ChangeCamera(RCC_Camera.CameraMode.TPS);
		Object.FindObjectOfType<RCC_Camera>().ResetCamera();
		StartCoroutine(SetCamRot(rotval));
	}

	private IEnumerator SetCamRot(int rotval)
	{
		yield return new WaitForSeconds(0.3f);
		PlayerPrefs.SetInt("WANTROT", rotval);
	}
}
