using System;
using System.Collections;
using System.IO;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using HeathenEngineering.SteamApi.Foundation;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SRAdminTools : MonoBehaviour
{
	[Space]
	public ObscuredInt ScoreDeLeaderboard;

	public Image VoitureUtiliser;

	public InputField ScoreUI;

	public Sprite[] CarsIcon;

	[Space]
	public Transform IroDown;

	public Transform AkinaDown;

	public Transform AkagiUp;

	public InputField Jack;

	public InputField TPPLAYER;

	public InputField SteaMIDPLAYER;

	public GameObject[] BTn;

	public SteamUserData userDataa;

	public InputField UsersIDPlayer;

	public GameObject imagefortxt;

	[Space]
	[Header("= SUDO VALUE =")]
	[Space]
	public ObscuredString target;

	public ObscuredString playerpreff;

	public ObscuredString TypeIntFloatStringMusic;

	public ObscuredInt FloatAndIntValue;

	public ObscuredString StringValue;

	public Text boostlvl;

	[Space]
	[TextArea(15, 20)]
	public string Infos = "";

	[Space]
	[Header("= ENTRE 0.001 et 1 =")]
	[Space]
	public ObscuredString Money;

	public ObscuredFloat sfx;

	public ObscuredFloat master;

	public ObscuredFloat music;

	private ObscuredString Playerlist;

	private int swith;

	private int noreaprt;

	public void Start()
	{
		UnityEngine.Object.FindObjectOfType<SRTransitionMap>().DisableFirst();
		swith = 0;
		Playerlist = "";
		GameObject[] bTn = BTn;
		for (int i = 0; i < bTn.Length; i++)
		{
			// OLD: bTn[i].SetActive(value: false);
			bTn[i].SetActive(value: Convert.ToBoolean(File.ReadAllLines("Settings.txt")[0].Split('=')[1]));
		}
		Infos = "Type : int, float, string, music\n\nPLAYERPREFS EXEMPLE : \n\n XP (int 100 = 1 LVL) / TOTALWINMONEY (int) / TOTALTIME (int secondes) / MyBalance (int) / MyLvl (int) \n\n BestRunTimeIrohazakaReverseNew (int) / BestRunTimeIrohazakaNew (int)\n BestRunTimeAkinaReverseNew (int) / BestRunTimeAkinaNew (int)\n BestRunTimeAkagiReverseNew (int) / BestRunTimeAkagiNew (int)";
	}

	private void Update()
	{
		boostlvl.text = "BOOST LVL : " + ObscuredPrefs.GetInt("BoostQuantity") + "(" + ObscuredPrefs.GetInt("BoostQuantity") * 100 / UnityEngine.Object.FindObjectOfType<SRNosManager>().NosLevelMax + "%)";
	}

	public void RefreshPlayerList()
	{
		Playerlist = "";
		GameObject[] array = GameObject.FindGameObjectsWithTag("3DPSEUDO");
		foreach (GameObject gameObject in array)
		{
			Playerlist = string.Concat(Playerlist, gameObject.GetComponent<TextMeshPro>().text, " | ", gameObject.GetComponentInParent<SRPlayerCollider>().gameObject.name, "\n");
		}
		Infos = "Type : int, float, string, music\n\nPLAYERPREFS EXEMPLE : \n\n TOTALWINBATTLE (int) / XP (int) / TOTALWINMONEY (int) / TOTALTIME (float secondes, 3600 = 1h) / MyBalance (int) / MyLvl (int) \n\n BestRunTimeIrohazakaReverseNew (int) / BestRunTimeIrohazakaNew (int) / RunCountIrohazaka (int)\n BestRunTimeAkinaReverseNew (int) / BestRunTimeAkinaNew (int) / RunCountAkina (int)\n BestRunTimeAkagiReverseNew (int) / BestRunTimeAkagiNew (int) / RunCountAkagi (int)\n BestRunTimeUSUIReverseNew (int) / BestRunTimeUSUINew (int) / RunCountUSUI (int) \n\n\nPLAYER LIST :\n\n" + Playerlist;
	}

	public void ok()
	{
		if ((ObscuredString)Jack.text == Money)
		{
			GameObject[] bTn = BTn;
			for (int i = 0; i < bTn.Length; i++)
			{
				bTn[i].SetActive(value: true);
			}
		}
		else if (Jack.text == "OFF")
		{
			Jack.text = "";
			GameObject[] bTn = BTn;
			for (int i = 0; i < bTn.Length; i++)
			{
				bTn[i].SetActive(value: false);
			}
		}
	}

	public void GoUSUI()
	{
		Jack.interactable = true;
	}

	public void GoIroDown()
	{
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Rigidbody>().drag = 1000f;
		StartCoroutine(LobbySpawn(IroDown));
	}

	public void GoAkinaDown()
	{
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Rigidbody>().drag = 1000f;
		StartCoroutine(LobbySpawn(AkinaDown));
	}

	public void GoAkagiUp()
	{
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Rigidbody>().drag = 1000f;
		StartCoroutine(LobbySpawn(AkagiUp));
	}

	private IEnumerator LobbySpawn(Transform Target)
	{
		_ = Vector3.zero;
		_ = Quaternion.identity;
		yield return new WaitForSeconds(0.2f);
		Vector3 position = Target.position;
		Quaternion rotation = Target.rotation;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.position = position;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.rotation = rotation;
		yield return new WaitForSeconds(0.2f);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Rigidbody>().drag = 0.01f;
	}

	public void ResetData()
	{
		PlayerPrefs.DeleteAll();
		ObscuredPrefs.DeleteAll();
	}

	public void GiveMoney()
	{
		ObscuredPrefs.SetInt("MyBalance", ObscuredPrefs.GetInt("MyBalance") + 1000);
	}

	public void TPPLAYERR()
	{
		string text = TPPLAYER.text;
		Debug.Log("AAAAAAS    " + text);
		GameObject[] array = GameObject.FindGameObjectsWithTag("3DPSEUDO");
		foreach (GameObject gameObject in array)
		{
			string text2 = gameObject.GetComponent<TextMeshPro>().text.Split(']')[1];
			if (text2 == "</color> " + text)
			{
				Transform transform = gameObject.GetComponentInParent<RCC_CarControllerV3>().gameObject.transform;
				StartCoroutine(LobbySpawn(transform));
			}
			else
			{
				Debug.Log(gameObject.GetComponent<TextMeshPro>().text + " NE CORRESPONDANT PAS");
			}
			Debug.Log(">>> " + text2);
		}
	}

	public void Sendd()
	{
		StartCoroutine(SendSteamIDtempo());
		noreaprt = 0;
	}

	private IEnumerator SendSteamIDtempo()
	{
		yield return new WaitForSeconds(5f);
		UserID();
	}

	public void UserID()
	{
		string text = userDataa.id.ToString();
		string comefrom = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.name + " ( " + PlayerPrefs.GetString("PLAYERNAMEE") + " ) ";
		if ((text == "0" && noreaprt < 2) || (text == "10000000000" && noreaprt < 2))
		{
			StartCoroutine(SendSteamIDtempo());
			noreaprt++;
		}
		else
		{
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().SendMyUserID(text, comefrom);
		}
	}

	public void GetSteamIDAllplayer()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("3DPSEUDO");
		if (swith == 0)
		{
			imagefortxt.SetActive(value: true);
			UsersIDPlayer.text = "STEAM ID DES PLAYERS : \n\n";
			swith = 1;
			GameObject[] array2 = array;
			foreach (GameObject gameObject in array2)
			{
				string text = gameObject.GetComponent<TextMeshPro>().text.Split(']')[1];
				UsersIDPlayer.text = UsersIDPlayer.text + " " + text + "\n : " + gameObject.GetComponentInChildren<Text>().text + "\n\n\n".Replace("color", "");
				Debug.Log(gameObject.GetComponent<TextMeshPro>().text + "\n ID : " + gameObject.GetComponentInChildren<Text>().text);
			}
		}
		else
		{
			imagefortxt.SetActive(value: false);
			swith = 0;
			UsersIDPlayer.text = "";
		}
	}

	public void GoSteamAccount()
	{
		string text = SteaMIDPLAYER.text.ToString();
		Application.OpenURL("https://steamcommunity.com/profiles/" + text);
	}

	public void SudoCmd()
	{
		if (target != (ObscuredString)"" && playerpreff != (ObscuredString)"")
		{
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().EditPlayerPP(target, playerpreff, FloatAndIntValue, TypeIntFloatStringMusic, StringValue, sfx, master, music);
		}
		Debug.Log(string.Concat("ADMIN TOOLS >> CIBLE : ", (string)target, " | PP : ", (string)playerpreff, " | INT FLOAT VALUE : ", FloatAndIntValue, " | TYPE : ", (string)TypeIntFloatStringMusic, " | STRING VALUE : ", (string)StringValue));
	}

	public void SetMaxBoost(Text lol)
	{
		Debug.Log("LOL : " + lol.text);
		UnityEngine.Object.FindObjectOfType<SRNosManager>().NosLevelMax = Convert.ToInt32(lol.text);
		UnityEngine.Object.FindObjectOfType<SRNosManager>().RechargeBoostUIYes();
	}

	public void GoToMaptest()
	{
		SceneManager.LoadScene("(2-A) Player Services - Remote Storage");
	}

	public void SetScore()
	{
		int num = 0;
		int num2 = 1;
		if (ScoreDeLeaderboard.ToString().Length == 4)
		{
			num = Convert.ToInt32(ScoreDeLeaderboard.ToString().Substring(ScoreDeLeaderboard.ToString().Length - 1));
			num2 = 1;
		}
		else if (ScoreDeLeaderboard.ToString().Length == 5)
		{
			num = Convert.ToInt32(ScoreDeLeaderboard.ToString().Substring(ScoreDeLeaderboard.ToString().Length - 2));
			num2 = 2;
		}
		int num3 = Convert.ToInt32(ScoreDeLeaderboard.ToString().Substring(0, ScoreDeLeaderboard.ToString().Length - num2));
		VoitureUtiliser.sprite = CarsIcon[num];
		ScoreUI.text = string.Concat(num3);
		Debug.Log("LAST NUMBER : " + num + " | SCORE : " + num3);
	}
}
