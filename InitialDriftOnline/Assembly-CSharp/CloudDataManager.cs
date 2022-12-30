using System;
using System.Collections;
using System.Text;
using CodeStage.AntiCheat.Storage;
using HeathenEngineering.SteamApi.PlayerServices;
using UnityEngine;
using UnityEngine.UI;

public class CloudDataManager : MonoBehaviour
{
	[Serializable]
	public class PlayerData
	{
		public float PlayingTime;

		public int XP;

		[Space]
		public int TOTALWINMONEY;

		public int TOTALWINBATTLE;

		[Space]
		public int RunCountAkina;

		public int RunCountAkagi;

		public int RunCountIrohazaka;

		public int RunCountUSUI;

		[Space]
		public bool DLCThunePurchased;
	}

	public PlayerData myData;

	public GameObject RestoringButton;

	public Text PlayingTimetxt;

	public Text XPtxt;

	[Space]
	public Text TOTALWINMONEYtxt;

	public Text TOTALWINBATTLEtxt;

	[Space]
	public Text RunCountAkinatxt;

	public Text RunCountAkagitxt;

	public Text RunCountIrohazakatxt;

	public Text RunCountUSUItxt;

	[Space]
	public Text compteur;

	private void Start()
	{
		SteamworksRemoteStorageManager.RefreshFileList();
		StartCoroutine(ShowBtn());
	}

	private IEnumerator ShowBtn()
	{
		yield return new WaitForSeconds(1f);
		if (SteamworksRemoteStorageManager.FileReadJson<PlayerData>("CloudDataPlayer.dat", Encoding.UTF8).TOTALWINMONEY > ObscuredPrefs.GetInt("TOTALWINMONEY"))
		{
			RestoringButton.SetActive(value: true);
			yield break;
		}
		RestoringButton.SetActive(value: false);
		Debug.Log("[CLOUD] Pas de sauvegarde supérieur trouvée");
		Debug.Log("     -> Check for Cash DLC");
		UnityEngine.Object.FindObjectOfType<SRDLCManager>().Setupdlctune();
	}

	private void Update()
	{
	}

	public void SaveData()
	{
		StartCoroutine(Saveend());
		PlayerData playerData = SteamworksRemoteStorageManager.FileReadJson<PlayerData>("CloudDataPlayer.dat", Encoding.UTF8);
		if (ObscuredPrefs.GetInt("XP") > playerData.XP)
		{
			myData.XP = ObscuredPrefs.GetInt("XP");
		}
		else
		{
			myData.XP = playerData.XP;
		}
		if ((float)ObscuredPrefs.GetInt("TOTALTIME") > playerData.PlayingTime)
		{
			myData.PlayingTime = ObscuredPrefs.GetFloat("TOTALTIME");
		}
		else
		{
			myData.PlayingTime = playerData.PlayingTime;
		}
		if (ObscuredPrefs.GetInt("TOTALWINMONEY") > playerData.TOTALWINMONEY)
		{
			myData.TOTALWINMONEY = ObscuredPrefs.GetInt("TOTALWINMONEY");
		}
		else
		{
			myData.TOTALWINMONEY = playerData.TOTALWINMONEY;
		}
		if (ObscuredPrefs.GetInt("TOTALWINBATTLE") > playerData.TOTALWINBATTLE)
		{
			myData.TOTALWINBATTLE = ObscuredPrefs.GetInt("TOTALWINBATTLE");
		}
		else
		{
			myData.TOTALWINBATTLE = playerData.TOTALWINBATTLE;
		}
		if (ObscuredPrefs.GetInt("RunCountAkina") > playerData.RunCountAkina)
		{
			myData.RunCountAkina = ObscuredPrefs.GetInt("RunCountAkina");
		}
		else
		{
			myData.RunCountAkina = playerData.RunCountAkina;
		}
		if (ObscuredPrefs.GetInt("RunCountAkagi") > playerData.RunCountAkagi)
		{
			myData.RunCountAkagi = ObscuredPrefs.GetInt("RunCountAkagi");
		}
		else
		{
			myData.RunCountAkagi = playerData.RunCountAkagi;
		}
		if (ObscuredPrefs.GetInt("RunCountIrohazaka") > playerData.RunCountIrohazaka)
		{
			myData.RunCountIrohazaka = ObscuredPrefs.GetInt("RunCountIrohazaka");
		}
		else
		{
			myData.RunCountIrohazaka = playerData.RunCountIrohazaka;
		}
		if (ObscuredPrefs.GetInt("RunCountUSUI") > playerData.RunCountUSUI)
		{
			myData.RunCountUSUI = ObscuredPrefs.GetInt("RunCountUSUI");
		}
		else
		{
			myData.RunCountUSUI = playerData.RunCountUSUI;
		}
		Debug.Log("[CLOUD] Save des données");
		if (ObscuredPrefs.GetBool("TakedDLCGOld") && !playerData.DLCThunePurchased)
		{
			myData.DLCThunePurchased = ObscuredPrefs.GetBool("TakedDLCGOld");
		}
	}

	private IEnumerator Saveend()
	{
		yield return new WaitForSeconds(0.5f);
		SteamworksRemoteStorageManager.FileWrite("CloudDataPlayer.dat", myData, Encoding.UTF8);
	}

	public void RestoreData()
	{
		ObscuredPrefs.DeleteAll();
		PlayerPrefs.DeleteAll();
		SteamworksRemoteStorageManager.RefreshFileList();
		StartCoroutine(restoring());
	}

	private IEnumerator restoring()
	{
		PlayerData data = SteamworksRemoteStorageManager.FileReadJson<PlayerData>("CloudDataPlayer.dat", Encoding.UTF8);
		compteur.text = "5";
		yield return new WaitForSeconds(1f);
		compteur.text = "4";
		ObscuredPrefs.SetFloat("TOTALTIME", data.PlayingTime);
		ObscuredPrefs.SetInt("XP", data.XP);
		ObscuredPrefs.SetInt("TOTALWINMONEY", data.TOTALWINMONEY);
		ObscuredPrefs.SetInt("MyBalance", data.TOTALWINMONEY);
		ObscuredPrefs.SetInt("TOTALWINBATTLE", data.TOTALWINBATTLE);
		ObscuredPrefs.SetInt("RunCountAkina", data.RunCountAkina);
		ObscuredPrefs.SetInt("RunCountAkagi", data.RunCountAkagi);
		ObscuredPrefs.SetInt("RunCountIrohazaka", data.RunCountIrohazaka);
		ObscuredPrefs.SetInt("RunCountUSUI", data.RunCountUSUI);
		ObscuredPrefs.SetBool("TakedDLCGOld", data.DLCThunePurchased);
		yield return new WaitForSeconds(1f);
		compteur.text = "3";
		SaveData();
		yield return new WaitForSeconds(1f);
		compteur.text = "2";
		yield return new WaitForSeconds(1f);
		compteur.text = "1";
		Application.Quit();
	}

	public void ShowStatsInfo()
	{
		SteamworksRemoteStorageManager.RefreshFileList();
		PlayerData playerData = SteamworksRemoteStorageManager.FileReadJson<PlayerData>("CloudDataPlayer.dat", Encoding.UTF8);
		Debug.Log("[CLOUD] Affichages des données");
		int num = Convert.ToInt32(playerData.PlayingTime);
		int num2 = num / 60;
		Debug.Log("MINUTE : " + num2);
		Debug.Log("Resultat : " + num);
		if (num2 < 60)
		{
			PlayingTimetxt.text = "00:" + num2;
			Debug.Log("sortie  : 00h" + num2);
		}
		else
		{
			int num3 = num2 / 60;
			int num4 = num2 - num3 * 60;
			PlayingTimetxt.text = num3 + "h" + num4;
			Debug.Log("sortie  : 00h" + num3 + ":" + num4);
		}
		int num5 = playerData.XP / 100;
		XPtxt.text = string.Concat(num5);
		TOTALWINMONEYtxt.text = playerData.TOTALWINMONEY + "¥";
		TOTALWINBATTLEtxt.text = string.Concat(playerData.TOTALWINBATTLE);
		RunCountAkinatxt.text = string.Concat(playerData.RunCountAkina);
		RunCountAkagitxt.text = string.Concat(playerData.RunCountAkagi);
		RunCountIrohazakatxt.text = string.Concat(playerData.RunCountIrohazaka);
		RunCountUSUItxt.text = string.Concat(playerData.RunCountUSUI);
		Debug.Log("[CLOUD] DLC USED INFO : (" + playerData.DLCThunePurchased + ")");
	}
}
