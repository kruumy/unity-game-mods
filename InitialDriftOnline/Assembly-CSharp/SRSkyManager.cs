using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SRSkyManager : MonoBehaviour
{
	public Material DayBox;

	public Material NightBox;

	[Space]
	public Material MidBox;

	[Space]
	public Material MidMidBox;

	public Light DirectionalLight;

	public int AddTime;

	public int ImMaster;

	public Text Minutetxt;

	private float LessTime;

	private float Seconde;

	public int Minute;

	public int NoRepeat;

	[Space]
	[Space]
	[Header("DAY SETTINGS")]
	public float DirectionalLightIntensityDay;

	public float AmbientIntensityDay;

	public float ReflectionIntensityDay;

	[Space]
	[Space]
	[Header("NIGHT SETTINGS")]
	public float DirectionalLightIntensityNight;

	public float AmbientIntensityNight;

	public float ReflectionIntensityNight;

	public bool Autorisation;

	private int PeopleNbr;

	private int ReceidMaster;

	private PhotonView view;

	private GameObject TargetMec;

	private float DLI2;

	private float RSAI2;

	private float RSRI2;

	private int hcycle;

	private void Start()
	{
		hcycle = 0;
		Autorisation = false;
		AddTime = 0;
		NoRepeat = 0;
		LessTime = Time.time;
		StartCoroutine(CheckMec());
	}

	private IEnumerator CheckMec()
	{
		yield return new WaitForSeconds(1f);
		view = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<PhotonView>();
		if (Object.FindObjectsOfType<RCC_CarControllerV3>().Length == 1)
		{
			PeopleNbr = 1;
			Autorisation = true;
			ImMaster = 10;
		}
		else
		{
			Autorisation = false;
			ImMaster = 5;
		}
	}

	private void Update()
	{
		if (Object.FindObjectsOfType<RCC_CarControllerV3>().Length != PeopleNbr && Autorisation)
		{
			PeopleNbr = Object.FindObjectsOfType<RCC_CarControllerV3>().Length;
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().SendTheTimeOfRoom(Minute);
		}
		if (!Autorisation && !TargetMec && ReceidMaster == 1)
		{
			RCC_CarControllerV3[] array = Object.FindObjectsOfType<RCC_CarControllerV3>();
			if (array[0].gameObject.name == RCC_SceneManager.Instance.activePlayerVehicle.gameObject.name)
			{
				Autorisation = true;
				ImMaster = 10;
			}
			else
			{
				TargetMec = array[0].gameObject;
				ImMaster = 5;
			}
		}
		SetSky();
	}

	public void ReceiveTimeByRPC(int Minute, string MasterPlayer)
	{
		if (!Autorisation && Time.time - LessTime < 20f)
		{
			AddTime = Minute;
			if ((Minute >= 0 && Minute <= 5) || (Minute >= 51 && Minute <= 59))
			{
				NoRepeat = 0;
			}
			else if ((Minute >= 6 && Minute <= 14) || (Minute >= 41 && Minute <= 50))
			{
				NoRepeat = 1;
			}
			else if ((Minute >= 15 && Minute <= 20) || (Minute >= 35 && Minute <= 40))
			{
				NoRepeat = 0;
			}
			else if (Minute >= 21 && Minute <= 34)
			{
				NoRepeat = 1;
			}
		}
		ReceidMaster = 1;
		TargetMec = GameObject.Find(MasterPlayer);
	}

	public void SetSky()
	{
		Seconde = Time.time - LessTime;
		Minute = (int)Seconde / 60 + AddTime;
		if ((bool)TargetMec)
		{
			Minutetxt.text = "MINUTE : " + Minute + "\n ReceidMaster : " + ReceidMaster + "\n Autorisation : " + Autorisation.ToString() + "\n MASTERNAME : " + TargetMec.gameObject.name + "\n Dirlight in : " + DirectionalLight.intensity + "\n AmbienInt : " + RenderSettings.ambientIntensity + "\n ReflectionInt : " + RenderSettings.reflectionIntensity;
		}
		else
		{
			Minutetxt.text = "MINUTE : " + Minute + "\n ReceidMaster : " + ReceidMaster + "\n Autorisation : " + Autorisation.ToString() + "\n MASTERNAME : NO MASTER NAME\n Dirlight in : " + DirectionalLight.intensity + "\n AmbienInt : " + RenderSettings.ambientIntensity + "\n ReflectionInt : " + RenderSettings.reflectionIntensity;
		}
		if ((Minute >= 0 && Minute <= 5 && NoRepeat == 0) || (Minute >= 51 && Minute <= 59 && NoRepeat == 0))
		{
			NoRepeat = 1;
			RenderSettings.skybox = DayBox;
			DirectionalLight.color = new Color32(byte.MaxValue, 190, 130, byte.MaxValue);
			DirectionalLight.intensity = DirectionalLightIntensityDay;
			RenderSettings.ambientIntensity = AmbientIntensityDay;
			RenderSettings.reflectionIntensity = ReflectionIntensityDay;
			DirectionalLight.shadowStrength = 0.8f;
		}
		else if ((Minute >= 6 && Minute <= 14 && NoRepeat == 1) || (Minute >= 41 && Minute <= 50 && NoRepeat == 1))
		{
			NoRepeat = 0;
			RenderSettings.skybox = MidBox;
			DirectionalLight.color = new Color32(byte.MaxValue, 190, 130, byte.MaxValue);
			DirectionalLight.intensity = DirectionalLightIntensityDay;
			RenderSettings.ambientIntensity = 0.8f;
			RenderSettings.reflectionIntensity = 0.5f;
			DirectionalLight.shadowStrength = 0.6f;
		}
		else if ((Minute >= 15 && Minute <= 20 && NoRepeat == 0) || (Minute >= 35 && Minute <= 40 && NoRepeat == 0))
		{
			NoRepeat = 1;
			RenderSettings.skybox = MidMidBox;
			DirectionalLight.color = new Color32(byte.MaxValue, 98, 0, byte.MaxValue);
			DirectionalLight.intensity = 0.3f;
			RenderSettings.ambientIntensity = 0.4f;
			RenderSettings.reflectionIntensity = 0.3f;
			DirectionalLight.shadowStrength = 0.3f;
		}
		else if (Minute >= 21 && Minute <= 34 && NoRepeat == 1)
		{
			NoRepeat = 0;
			RenderSettings.skybox = NightBox;
			DirectionalLight.color = new Color32(130, 130, 130, byte.MaxValue);
			DirectionalLight.intensity = DirectionalLightIntensityNight;
			RenderSettings.ambientIntensity = AmbientIntensityNight;
			RenderSettings.reflectionIntensity = ReflectionIntensityNight;
			DirectionalLight.shadowStrength = 0.4f;
		}
		if (Minute == 60)
		{
			LessTime = Time.time;
			AddTime = 0;
			Debug.Log("24H OK");
			hcycle = 1;
		}
	}
}
