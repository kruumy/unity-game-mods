using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using UnityEngine;
using UnityEngine.UI;

public class SRShadowManager : MonoBehaviour
{
	public Toggle Shadowss;

	public Toggle BackCam;

	public Toggle vhstog;

	public Toggle PopUpChat;

	public Toggle RearCamRot;

	public Toggle ToggleVibration;

	public Toggle ToggleTrail;

	public Slider SensivitySlider;

	public Slider steeringHelperSlider;

	public GameObject TogglePlayerlist;

	public GameObject CameraForVhs;

	public GameObject directionallight;

	public Text SSensi;

	public Text SHelper;

	[Space]
	public Slider TopCam;

	private float topcamy;

	public Text SliderMinimapHighValue;

	public Toggle TooglePlayerListXbox;

	public GameObject TC;

	public RenderTexture jack;

	public GameObject b;

	public GameObject d;

	public ObscuredString a;

	public ObscuredString c;

	private int setuphigh;

	public void Start()
	{
		setuphigh = 0;
		CameraForVhs.GetComponent<SuperVHSFilter>().enabled = false;
		if (PlayerPrefs.GetInt("shadowsfirst") == 0)
		{
			PlayerPrefs.SetInt("shadowsfirst", 1);
			ObscuredPrefs.SetBool("shadowsbool", value: true);
			ObscuredPrefs.SetBool("BackCamToogle", value: false);
		}
		if (PlayerPrefs.GetInt("Playerlistfirst") == 0)
		{
			PlayerPrefs.SetInt("Playerlistfirst", 1);
			ObscuredPrefs.SetBool("TooglePlayerListXbox", value: true);
			TooglePlayerListXbox.isOn = ObscuredPrefs.GetBool("TooglePlayerListXbox");
		}
		if (ObscuredPrefs.GetBool("shadowsbool"))
		{
			QualitySettings.shadows = ShadowQuality.All;
			Shadowss.isOn = true;
		}
		else
		{
			QualitySettings.shadows = ShadowQuality.Disable;
			Shadowss.isOn = false;
		}
		if (ObscuredPrefs.GetInt("TogglePopUpChat_First") == 0)
		{
			ObscuredPrefs.SetInt("TogglePopUpChat_First", 1);
			ObscuredPrefs.SetBool("TogglePopUpChat", value: true);
			PopUpChat.isOn = true;
		}
		StartCoroutine(CamWait());
	}

	private IEnumerator CamWait()
	{
		yield return new WaitForSeconds(4f);
		if (PlayerPrefs.GetInt("DisableRearRotation") == 0)
		{
			RearCamRot.isOn = true;
			PlayerPrefs.SetInt("DisableRearRotation_Value", 180);
		}
		else
		{
			RearCamRot.isOn = false;
			PlayerPrefs.SetInt("DisableRearRotation_Value", 0);
		}
		SliderMinimapHighValue.text = "0";
		TooglePlayerListXbox.isOn = ObscuredPrefs.GetBool("TooglePlayerListXbox");
		SSensi.text = PlayerPrefs.GetFloat("SteeringSensivity").ToString("00.00") ?? "";
		SHelper.text = (PlayerPrefs.GetFloat("SteeringhelperValue") * 10f).ToString("00.00") ?? "";
		if (PlayerPrefs.GetInt("ToggleVHS") == 1 && PlayerPrefs.GetInt("LowQualityJack") == 1)
		{
			CameraForVhs.GetComponent<SuperVHSFilter>().enabled = true;
			vhstog.isOn = true;
			vhstog.interactable = true;
		}
		else if (PlayerPrefs.GetInt("ToggleVHS") == 1 && PlayerPrefs.GetInt("LowQualityJack") == 0)
		{
			CameraForVhs.GetComponent<SuperVHSFilter>().enabled = false;
			vhstog.isOn = true;
			vhstog.interactable = false;
		}
		else if (PlayerPrefs.GetInt("ToggleVHS") == 0 && PlayerPrefs.GetInt("LowQualityJack") == 1)
		{
			CameraForVhs.GetComponent<SuperVHSFilter>().enabled = false;
			vhstog.isOn = false;
			vhstog.interactable = true;
		}
		else if (PlayerPrefs.GetInt("ToggleVHS") == 0 && PlayerPrefs.GetInt("LowQualityJack") == 0)
		{
			CameraForVhs.GetComponent<SuperVHSFilter>().enabled = false;
			vhstog.isOn = false;
			vhstog.interactable = false;
		}
		if (ObscuredPrefs.GetBool("TogglePopUpChat"))
		{
			PopUpChat.isOn = true;
		}
		else
		{
			PopUpChat.isOn = false;
		}
		if (PlayerPrefs.GetInt("SterringFirst") == 0)
		{
			PlayerPrefs.SetInt("SterringFirst", 1);
			PlayerPrefs.SetFloat("SteeringSensivity", 15f);
			SensivitySlider.value = 15f;
		}
		else if (PlayerPrefs.GetFloat("SteeringSensivity") < 2.5f)
		{
			PlayerPrefs.SetFloat("SteeringSensivity", 2.5f);
			SensivitySlider.value = 2.5f;
			SSensi.text = PlayerPrefs.GetFloat("SteeringSensivity").ToString("00.00") ?? "";
		}
		else
		{
			SensivitySlider.value = PlayerPrefs.GetFloat("SteeringSensivity");
			SSensi.text = PlayerPrefs.GetFloat("SteeringSensivity").ToString("00.00") ?? "";
		}
		if (PlayerPrefs.GetInt("SteeringHelperFirst") == 0)
		{
			PlayerPrefs.SetInt("SteeringHelperFirst", 1);
			PlayerPrefs.SetFloat("SteeringhelperValue", 0.3f);
			steeringHelperSlider.value = 0.3f;
			SHelper.text = (PlayerPrefs.GetFloat("SteeringhelperValue") * 10f).ToString("00.00") ?? "";
		}
		else
		{
			steeringHelperSlider.value = PlayerPrefs.GetFloat("SteeringhelperValue");
			SHelper.text = (PlayerPrefs.GetFloat("SteeringhelperValue") * 10f).ToString("00.00") ?? "";
		}
		if (PlayerPrefs.GetString("ControllerTypeChoose") == "Xbox360One" || PlayerPrefs.GetString("ControllerTypeChoose") == "LogitechSteeringWheel")
		{
			ToggleVibration.gameObject.SetActive(value: true);
			TogglePlayerlist.SetActive(value: true);
		}
		else
		{
			ToggleVibration.gameObject.SetActive(value: false);
			TogglePlayerlist.SetActive(value: false);
		}
		if (PlayerPrefs.GetInt("Vibration") == 0)
		{
			ToggleVibration.isOn = true;
		}
		else if (PlayerPrefs.GetInt("Vibration") == 1)
		{
			ToggleVibration.isOn = false;
		}
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().steerHelperLinearVelStrength = PlayerPrefs.GetFloat("SteeringhelperValue");
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().steerHelperAngularVelStrength = PlayerPrefs.GetFloat("SteeringhelperValue");
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().steeringSensitivity = PlayerPrefs.GetFloat("SteeringSensivity");
		if (ObscuredPrefs.GetBool("BackCamToogle"))
		{
			if ((bool)RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID3>())
			{
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID3>().gameObject.GetComponent<Camera>().enabled = true;
			}
			if ((bool)RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID5>())
			{
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID5>().gameObject.GetComponent<MeshRenderer>().enabled = false;
			}
			BackCam.isOn = true;
		}
		else
		{
			if ((bool)RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID3>())
			{
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID3>().gameObject.GetComponent<Camera>().enabled = false;
			}
			if ((bool)RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID5>())
			{
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID5>().gameObject.GetComponent<MeshRenderer>().enabled = false;
			}
			BackCam.isOn = false;
		}
		yield return new WaitForSeconds(0.1f);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().steeringSensitivity = PlayerPrefs.GetFloat("SteeringSensivity");
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().steeringHelper = true;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().steerHelperLinearVelStrength = PlayerPrefs.GetFloat("SteeringhelperValue");
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().steerHelperAngularVelStrength = PlayerPrefs.GetFloat("SteeringhelperValue");
		yield return new WaitForSeconds(1f);
		SteeringHelperUpdate();
		SensivityUpdate();
		yield return new WaitForSeconds(2f);
		TC = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID8>().gameObject;
		topcamy = TC.transform.position.y;
		if ((ObscuredString)PlayerPrefs.GetString("PLAYERNAMEE") == a || (ObscuredString)PlayerPrefs.GetString("PLAYERNAMEE") == c)
		{
			b.SetActive(value: true);
			d.SetActive(value: true);
		}
		ToggleTrail.isOn = ObscuredPrefs.GetBool("ToggleTrail");
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRPlayerFonction>().TrailState(ObscuredPrefs.GetBool("ToggleTrail"));
	}

	private void Update()
	{
		SSensi.text = PlayerPrefs.GetFloat("SteeringSensivity").ToString("00.00") ?? "";
		SHelper.text = (PlayerPrefs.GetFloat("SteeringhelperValue") * 10f).ToString("00.00") ?? "";
	}

	public void SetMiniMapHigh()
	{
		if (setuphigh == 0)
		{
			TC = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID8>().gameObject;
			topcamy = TC.transform.position.y;
			setuphigh = 1;
		}
	}

	public void UpdateShadows()
	{
		if (Shadowss.isOn)
		{
			QualitySettings.shadows = ShadowQuality.All;
			ObscuredPrefs.SetBool("shadowsbool", value: true);
		}
		else
		{
			QualitySettings.shadows = ShadowQuality.Disable;
			ObscuredPrefs.SetBool("shadowsbool", value: false);
		}
	}

	public void UpdateBackCam()
	{
		if (BackCam.isOn)
		{
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID3>().gameObject.GetComponent<Camera>().enabled = true;
			if ((bool)RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID5>())
			{
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID5>().gameObject.GetComponent<MeshRenderer>().enabled = false;
			}
			ObscuredPrefs.SetBool("BackCamToogle", value: true);
		}
		else
		{
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID3>().gameObject.GetComponent<Camera>().enabled = false;
			if ((bool)RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID5>())
			{
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID5>().gameObject.GetComponent<MeshRenderer>().enabled = false;
			}
			ObscuredPrefs.SetBool("BackCamToogle", value: false);
		}
	}

	public void SensivityUpdate()
	{
		if (SensivitySlider.value < 0.4f)
		{
			PlayerPrefs.SetFloat("SteeringSensivity", 0.4f);
		}
		else
		{
			PlayerPrefs.SetFloat("SteeringSensivity", SensivitySlider.value);
		}
		SSensi.text = PlayerPrefs.GetFloat("SteeringSensivity").ToString("00.00") ?? "";
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().steeringSensitivity = PlayerPrefs.GetFloat("SteeringSensivity");
	}

	public void SteeringHelperUpdate()
	{
		PlayerPrefs.SetFloat("SteeringhelperValue", steeringHelperSlider.value);
		SHelper.text = (PlayerPrefs.GetFloat("SteeringhelperValue") * 10f).ToString("00.00") ?? "";
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().steeringHelper = true;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().steerHelperLinearVelStrength = PlayerPrefs.GetFloat("SteeringhelperValue");
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().steerHelperAngularVelStrength = PlayerPrefs.GetFloat("SteeringhelperValue");
	}

	public void UpdateVibration()
	{
		if (PlayerPrefs.GetString("ControllerTypeChoose") == "Xbox360One" || PlayerPrefs.GetString("ControllerTypeChoose") == "LogitechSteeringWheel" || PlayerPrefs.GetString("ControllerTypeChoose") == "PS4")
		{
			ToggleVibration.gameObject.SetActive(value: true);
			TogglePlayerlist.SetActive(value: true);
		}
		else
		{
			ToggleVibration.gameObject.SetActive(value: false);
			TogglePlayerlist.SetActive(value: false);
		}
	}

	public void UpdateVibrationFromToggle()
	{
		if (ToggleVibration.isOn)
		{
			PlayerPrefs.SetInt("Vibration", 0);
			Debug.Log("VIBRATION ACTIVER 2");
		}
		else if (!ToggleVibration.isOn)
		{
			PlayerPrefs.SetInt("Vibration", 1);
			Debug.Log("VIBRATION DESACTIVER 2");
		}
	}

	public void UpdateVHS()
	{
		if (vhstog.isOn)
		{
			CameraForVhs.GetComponent<SuperVHSFilter>().enabled = true;
			PlayerPrefs.SetInt("ToggleVHS", 1);
		}
		else
		{
			CameraForVhs.GetComponent<SuperVHSFilter>().enabled = false;
			PlayerPrefs.SetInt("ToggleVHS", 0);
		}
	}

	public void TopCamUP()
	{
		TC = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRID8>().gameObject;
		TC.transform.position = new Vector3(TC.transform.position.x, topcamy + TopCam.value, TC.transform.position.z);
		PlayerPrefs.SetFloat("MiniMapHigh", TopCam.value);
		SliderMinimapHighValue.text = TopCam.value.ToString("00") ?? "";
	}

	public void UpdateTabXbix()
	{
		if (TooglePlayerListXbox.isOn)
		{
			ObscuredPrefs.SetBool("TooglePlayerListXbox", value: true);
		}
		else
		{
			ObscuredPrefs.SetBool("TooglePlayerListXbox", value: false);
		}
	}

	public void UpdateTogglePopUp()
	{
		if (PopUpChat.isOn)
		{
			ObscuredPrefs.SetBool("TogglePopUpChat", value: true);
		}
		else
		{
			ObscuredPrefs.SetBool("TogglePopUpChat", value: false);
		}
	}

	public void UpdateRearCamRot()
	{
		if (RearCamRot.isOn)
		{
			PlayerPrefs.SetInt("DisableRearRotation_Value", 180);
			PlayerPrefs.SetInt("DisableRearRotation", 0);
		}
		else
		{
			PlayerPrefs.SetInt("DisableRearRotation_Value", 0);
			PlayerPrefs.SetInt("DisableRearRotation", 1);
		}
	}

	public void UpdateToggleTrail()
	{
		if (ToggleTrail.isOn)
		{
			ObscuredPrefs.SetBool("ToggleTrail", value: true);
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRPlayerFonction>().TrailState(enabled: true);
		}
		else
		{
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SRPlayerFonction>().TrailState(enabled: false);
			ObscuredPrefs.SetBool("ToggleTrail", value: false);
		}
	}
}
