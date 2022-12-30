using System.Collections;
using CodeStage.AntiCheat.Storage;
using Photon.Pun;
using SickscoreGames.HUDNavigationSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XInputDotNetPure;
using ZionBandwidthOptimizer.Examples;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Network/Photon/RCC Photon Demo Manager")]
public class RCC_PhotonDemo : MonoBehaviourPun
{
	public RCC_CarControllerV3[] selectableVehicles;

	public int selectedCarIndex;

	public int selectedBehaviorIndex;

	public Transform FromOtherMap;

	public Transform ComeFromAkagi;

	public Transform ComeFromUsui;

	public GameObject[] LobbySpawnList;

	public GameObject JoinCam;

	public GameObject CarsCam;

	private int aleTarget;

	public RCC_CarControllerV3 playerCar;

	public GameObject[] CubeDeTP;

	public RCC_CustomizerExample RCCCustom;

	public Dropdown CarsSelector;

	private GameObject MyCarr;

	private GameObject TopCam;

	private int NoGhost;

	[Space]
	public float waitingTimeToSpawn = 0.41f;

	private PlayerIndex playerIndex;

	private GamePadState state;

	private GamePadState prevState;

	public void LauncherSpawn()
	{
		StartCoroutine(WaitToSpawnVehicule());
	}

	private IEnumerator WaitToSpawnVehicule()
	{
		GamePad.SetVibration(playerIndex, 0f, 0f);
		yield return new WaitForSeconds(waitingTimeToSpawn);
		Spawn();
	}

	public void Spawn()
	{
		Vector3 vector = Vector3.zero;
		Quaternion rotation = Quaternion.identity;
		if ((bool)RCC_SceneManager.Instance.activePlayerVehicle)
		{
			vector = RCC_SceneManager.Instance.activePlayerVehicle.transform.position;
			rotation = RCC_SceneManager.Instance.activePlayerVehicle.transform.rotation;
			NoGhost = 10;
		}
		PlayerPrefs.SetInt("WAITTPLOBBYBTN", 0);
		PlayerPrefs.SetInt("WANTROT", 0);
		StartCoroutine(WaitTP());
		if (vector == Vector3.zero && PlayerPrefs.GetInt("IcomeFromOtherMap") != 50)
		{
			LobbySpawnList = GameObject.FindGameObjectsWithTag("LobbyRespawn");
			aleTarget = Random.Range(0, LobbySpawnList.Length);
			vector = LobbySpawnList[aleTarget].gameObject.transform.position;
			rotation = LobbySpawnList[aleTarget].gameObject.transform.rotation;
		}
		else if (vector == Vector3.zero)
		{
			if (PlayerPrefs.GetString("WhereYouFrom") == "AKAGI")
			{
				vector = ComeFromAkagi.position;
				rotation = ComeFromAkagi.rotation;
			}
			else if (PlayerPrefs.GetString("WhereYouFrom") == "AKINA")
			{
				vector = FromOtherMap.position;
				rotation = FromOtherMap.rotation;
			}
			else if (PlayerPrefs.GetString("WhereYouFrom") == "USUI")
			{
				vector = ComeFromUsui.position;
				rotation = ComeFromUsui.rotation;
			}
			else if (PlayerPrefs.GetString("WhereYouFrom") == "")
			{
				vector = ComeFromUsui.position;
				rotation = ComeFromUsui.rotation;
			}
		}
		rotation.x = 0f;
		rotation.z = 0f;
		if ((bool)RCC_SceneManager.Instance.activePlayerVehicle)
		{
			PhotonNetwork.Destroy(RCC_SceneManager.Instance.activePlayerVehicle.gameObject);
		}
		JoinCam.SetActive(value: false);
		CarsCam.SetActive(value: true);
		int @int = PlayerPrefs.GetInt("DROPDOWNSELECTOR");
		RCC_CarControllerV3 component = PhotonNetwork.Instantiate("Photon Vehicles/" + selectableVehicles[@int].gameObject.name, vector + Vector3.up, rotation, 0).GetComponent<RCC_CarControllerV3>();
		MyCarr = component.gameObject;
		RCC.RegisterPlayerVehicle(component);
		RCC.SetControl(component, isControllable: true);
		if (PlayerPrefs.GetInt("IcomeFromOtherMap") == 50)
		{
			PlayerPrefs.SetInt("IcomeFromOtherMap", 0);
			StartCoroutine(FromTPBefore(component.gameObject));
		}
		if ((bool)RCC_SceneManager.Instance.activePlayerCamera)
		{
			RCC_SceneManager.Instance.activePlayerCamera.SetTarget(component.gameObject);
			CubeDeTP = GameObject.FindGameObjectsWithTag("CubeZone");
			GameObject[] cubeDeTP = CubeDeTP;
			for (int i = 0; i < cubeDeTP.Length; i++)
			{
				cubeDeTP[i].GetComponent<RespawnCube>().SetTarget(component.gameObject);
				SetTarget(component.gameObject);
			}
		}
		StartCoroutine(LoadLastStat());
		StartCoroutine(NicknameHead2());
		StartCoroutine(SkinDefined2(ObscuredPrefs.GetInt("SkinNumberInUse")));
		if (NoGhost != 10)
		{
			StartCoroutine(GhostMode(component.gameObject));
			NoGhost = 0;
		}
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().enabled = true;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerFonction>().enabled = true;
	}

	private IEnumerator LoadLastStat()
	{
		Object.FindObjectOfType<SRToffuLivraison>().UIStart.SetActive(value: false);
		ObscuredPrefs.SetString("CurrentCubee", "");
		yield return new WaitForSeconds(0.5f);
		Object.FindObjectOfType<SRToffuManager>().StopDelivery2();
		Object.FindObjectOfType<HUDNavigationSystem>().PlayerController = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform;
		Object.FindObjectOfType<Becquet>().SetAfterSpawn();
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().enabled = true;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerFonction>().enabled = true;
		GamePad.SetVibration(playerIndex, 0f, 0f);
		TopCam = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<Mask>().gameObject;
		TopCam.GetComponent<Camera>().enabled = true;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<HUDNavigationElement>().enabled = false;
		RCCCustom.LoadStatsTemp();
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<TextMeshPro>().gameObject.SetActive(value: false);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInParent<RCC_PhotonNetwork>().Sendinfo0();
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInParent<RCC_PhotonNetwork>().Sendinfo();
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInParent<RCC_PhotonNetwork>().Sendinfo2();
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInParent<RCC_PhotonNetwork>().Sendinfo3();
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInParent<RCC_PhotonNetwork>().Sendinfo4();
	}

	public void NickHead()
	{
		StartCoroutine(NicknameHead2());
	}

	private IEnumerator NicknameHead2()
	{
		yield return new WaitForSeconds(1.3f);
		Object.FindObjectOfType<SRLightTunerUI>().UpdateAfterSpawn();
		Object.FindObjectOfType<HUDNavigationSystem>().PlayerController = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform;
		Object.FindObjectOfType<SRAdminTools>().UserID();
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().PlayernameOnCars2();
		Object.FindObjectOfType<SkinManager>().SendMySkinColorToOther();
		if ((bool)Object.FindObjectOfType<SRSkinColorManager>())
		{
			Object.FindObjectOfType<SRSkinColorManager>().Setcolor();
		}
	}

	public void SkinDefined(int SkinNumber)
	{
		StartCoroutine(SkinDefined2(SkinNumber));
	}

	private IEnumerator SkinDefined2(int SkinNumber)
	{
		yield return new WaitForSeconds(1.2f);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SkinManager>().enabled = true;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SkinManager>().enabled = true;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().enabled = true;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerFonction>().enabled = true;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().SendMySkinModel(SkinNumber);
	}

	private IEnumerator GhostMode(GameObject voiture)
	{
		yield return new WaitForSeconds(0.1f);
		GamePad.SetVibration(playerIndex, 0f, 0f);
		voiture.GetComponentInChildren<SRPlayerCollider>().AppelRPCSetGhostModeV2(10);
		PlayerPrefs.SetInt("PlayerSpawned", 1);
		yield return new WaitForSeconds(5f);
		voiture.GetComponentInChildren<SRPlayerCollider>().AppelRPCSetGhostModeV2(8);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerFonction>().EnableBattleCube();
	}

	private IEnumerator FromTPBefore(GameObject newVehicle)
	{
		yield return new WaitForSeconds(0.4f);
		FromTeleporteur(newVehicle.gameObject);
	}

	public void FromTeleporteur(GameObject newVehicle)
	{
		string text = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0];
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Akina"))
		{
			Debug.Log("WELCOME AKINA");
			float @float = ObscuredPrefs.GetFloat(text + "_MaximumBrakeTemp");
			float steeringSensitivity = newVehicle.GetComponent<RCC_CarControllerV3>().steeringSensitivity;
			newVehicle.GetComponent<RCC_CarControllerV3>().steeringSensitivity = 0f;
			newVehicle.GetComponent<RCC_CarControllerV3>().brakeTorque = 0f;
			int val = -5;
			int val2 = 0;
			int val3 = -20;
			StartCoroutine(WaitEnableRCC(newVehicle.gameObject, steeringSensitivity, @float, val, val2, val3));
		}
		else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Irohazaka") && PlayerPrefs.GetString("WhereYouFrom") == "AKAGI")
		{
			Debug.Log("WELCOME IRO FROM AKAGI");
			PlayerPrefs.SetString("WhereYouFrom", "");
			float float2 = ObscuredPrefs.GetFloat(text + "_MaximumBrakeTemp");
			float steeringSensitivity2 = newVehicle.GetComponent<RCC_CarControllerV3>().steeringSensitivity;
			newVehicle.GetComponent<RCC_CarControllerV3>().steeringSensitivity = 0f;
			newVehicle.GetComponent<RCC_CarControllerV3>().brakeTorque = 0f;
			int val4 = 11;
			int val5 = 0;
			int val6 = -11;
			StartCoroutine(WaitEnableRCC(newVehicle.gameObject, steeringSensitivity2, float2, val4, val5, val6));
		}
		else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Irohazaka") && PlayerPrefs.GetString("WhereYouFrom") == "AKINA")
		{
			Debug.Log("WELCOME IRO FROM AKINA");
			PlayerPrefs.SetString("WhereYouFrom", "");
			float float3 = ObscuredPrefs.GetFloat(text + "_MaximumBrakeTemp");
			float steeringSensitivity3 = newVehicle.GetComponent<RCC_CarControllerV3>().steeringSensitivity;
			newVehicle.GetComponent<RCC_CarControllerV3>().steeringSensitivity = 0f;
			newVehicle.GetComponent<RCC_CarControllerV3>().brakeTorque = 0f;
			int val7 = -20;
			int val8 = -2;
			int val9 = 2;
			StartCoroutine(WaitEnableRCC(newVehicle.gameObject, steeringSensitivity3, float3, val7, val8, val9));
		}
		else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Irohazaka") && PlayerPrefs.GetString("WhereYouFrom") == "USUI")
		{
			Debug.Log("WELCOME IRO FROM USUI");
			PlayerPrefs.SetString("WhereYouFrom", "");
			float float4 = ObscuredPrefs.GetFloat(text + "_MaximumBrakeTemp");
			float steeringSensitivity4 = newVehicle.GetComponent<RCC_CarControllerV3>().steeringSensitivity;
			newVehicle.GetComponent<RCC_CarControllerV3>().steeringSensitivity = 0f;
			newVehicle.GetComponent<RCC_CarControllerV3>().brakeTorque = 0f;
			int val10 = -20;
			int val11 = -2;
			int val12 = -1;
			StartCoroutine(WaitEnableRCC(newVehicle.gameObject, steeringSensitivity4, float4, val10, val11, val12));
		}
		else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Akagi"))
		{
			Debug.Log("WELCOME AKAGI");
			float float5 = ObscuredPrefs.GetFloat(text + "_MaximumBrakeTemp");
			float steeringSensitivity5 = newVehicle.GetComponent<RCC_CarControllerV3>().steeringSensitivity;
			newVehicle.GetComponent<RCC_CarControllerV3>().steeringSensitivity = 0f;
			newVehicle.GetComponent<RCC_CarControllerV3>().brakeTorque = 0f;
			RCC_SceneManager.Instance.activePlayerVehicle.GetComponent<RCC_CarControllerV3>().brakeTorque = 0f;
			int val13 = -14;
			int val14 = -2;
			int val15 = -2;
			StartCoroutine(WaitEnableRCC(newVehicle.gameObject, steeringSensitivity5, float5, val13, val14, val15));
		}
		else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("USUI"))
		{
			Debug.Log("WELCOME USUI");
			float float6 = ObscuredPrefs.GetFloat(text + "_MaximumBrakeTemp");
			float steeringSensitivity6 = newVehicle.GetComponent<RCC_CarControllerV3>().steeringSensitivity;
			newVehicle.GetComponent<RCC_CarControllerV3>().steeringSensitivity = 0f;
			newVehicle.GetComponent<RCC_CarControllerV3>().brakeTorque = 0f;
			RCC_SceneManager.Instance.activePlayerVehicle.GetComponent<RCC_CarControllerV3>().brakeTorque = 0f;
			int val16 = -20;
			int val17 = -2;
			int val18 = -2;
			StartCoroutine(WaitEnableRCC(newVehicle.gameObject, steeringSensitivity6, float6, val16, val17, val18));
		}
	}

	private IEnumerator WaitEnableRCC(GameObject mycars, float ssSteering, float ssMaxbrake, int val1, int val2, int val3)
	{
		string[] array = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')');
		string TempoCarsName = array[0];
		Vector3 velocity = new Vector3(val1, val2, val3);
		RCC_SceneManager.Instance.activePlayerVehicle.GetComponent<Rigidbody>().velocity = velocity;
		yield return new WaitForSeconds(0.1f);
		mycars.GetComponent<RCC_CarControllerV3>().steeringSensitivity = 0f;
		mycars.GetComponent<RCC_CarControllerV3>().brakeTorque = 0f;
		yield return new WaitForSeconds(0.3f);
		RCC_SceneManager.Instance.activePlayerVehicle.GetComponent<RCC_CarControllerV3>().brakeTorque = 0f;
		RCC_SceneManager.Instance.activePlayerVehicle.GetComponent<RCC_CarControllerV3>().steeringSensitivity = 0f;
		yield return new WaitForSeconds(0.4f);
		RCC_SceneManager.Instance.activePlayerVehicle.GetComponent<RCC_CarControllerV3>().brakeTorque = 0f;
		yield return new WaitForSeconds(1.3f);
		if (ObscuredPrefs.GetFloat(TempoCarsName + "_MaximumBrakeTemp") == 0f)
		{
			ObscuredPrefs.SetFloat(TempoCarsName + "_MaximumBrakeTemp", 2500f);
		}
		mycars.GetComponent<RCC_CarControllerV3>().brakeTorque = ObscuredPrefs.GetFloat(TempoCarsName + "_MaximumBrakeTemp");
		mycars.GetComponent<RCC_CarControllerV3>().steeringSensitivity = ssSteering;
	}

	public void SelectVehicle(int index)
	{
		selectedCarIndex = index;
		PlayerPrefs.SetInt("DROPDOWNSELECTOR", index);
		StartCoroutine(WaitToSpawnVehicule());
	}

	public void SetColorLight()
	{
		MyCarr.GetComponentInChildren<SRPlayerCollider>().RPCChangeLightColor();
	}

	public void SetColorSmoke()
	{
		MyCarr.GetComponentInChildren<SRPlayerCollider>().RPCChangeSmokeColor();
	}

	public void SelectBehavior(int index)
	{
		selectedBehaviorIndex = index;
	}

	public void SetTarget(GameObject player)
	{
		playerCar = player.GetComponent<RCC_CarControllerV3>();
	}

	public void GoLobbyFromIrohazaka()
	{
		_ = Vector3.zero;
		_ = Quaternion.identity;
		GamePad.SetVibration(playerIndex, 0f, 0f);
		LobbySpawnList = GameObject.FindGameObjectsWithTag("LobbyRespawn");
		aleTarget = Random.Range(0, LobbySpawnList.Length);
		_ = LobbySpawnList[aleTarget].gameObject.transform.position;
		_ = LobbySpawnList[aleTarget].gameObject.transform.rotation;
		GameObject gameObject = GameObject.Find("Scene Objects/AutoRespawn_Collider");
		if (PlayerPrefs.GetInt("WAITTPLOBBYBTN") == 10)
		{
			PlayerPrefs.SetInt("WAITTPLOBBYBTN", 0);
			playerCar.gameObject.GetComponent<Rigidbody>().drag = 1000f;
			StartCoroutine(LobbySpawn());
			StartCoroutine(WaitTP());
		}
		else
		{
			gameObject.GetComponent<SRautorespawn>().WaitMessage();
		}
	}

	private IEnumerator LobbySpawn()
	{
		yield return new WaitForSeconds(0.2f);
		_ = Vector3.zero;
		_ = Quaternion.identity;
		GamePad.SetVibration(playerIndex, 0f, 0f);
		LobbySpawnList = GameObject.FindGameObjectsWithTag("LobbyRespawn");
		aleTarget = Random.Range(0, LobbySpawnList.Length);
		Vector3 position = LobbySpawnList[aleTarget].gameObject.transform.position;
		Quaternion rotation = LobbySpawnList[aleTarget].gameObject.transform.rotation;
		playerCar.gameObject.transform.position = position;
		playerCar.gameObject.transform.rotation = rotation;
		yield return new WaitForSeconds(0.2f);
		playerCar.gameObject.GetComponent<Rigidbody>().drag = 0.01f;
	}

	private IEnumerator WaitTP()
	{
		yield return new WaitForSeconds(10f);
		PlayerPrefs.SetInt("WAITTPLOBBYBTN", 10);
	}
}
