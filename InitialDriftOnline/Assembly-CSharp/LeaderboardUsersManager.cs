using System.Collections;
using System.IO;
using CodeStage.AntiCheat.Storage;
using HeathenEngineering.SteamApi.PlayerServices;
using Steamworks;
using UnityEngine;

public class LeaderboardUsersManager : MonoBehaviour
{
	private int PPIroRC;

	private int PPIroBT;

	private int PPAkagiRC;

	private int PPAkagiBT;

	private int PPHarunaRC;

	private int PPHarunaBT;

	private int MyLvl;

	private int BattleWin;

	private int RVPPHarunaBT;

	private int RVPPAkagiBT;

	private int RVPPIroBT;

	private int PPUSUIBT;

	private int PPUSUIBTREVERSE;

	private int PPUSUIRC;

	public SteamworksLeaderboardData IroRC;

	public SteamworksLeaderboardData IroBT;

	public SteamworksLeaderboardData AkagiRC;

	public SteamworksLeaderboardData AkagiBT;

	public SteamworksLeaderboardData HarunaRC;

	public SteamworksLeaderboardData HarunaBT;

	public SteamworksLeaderboardData RVIroBT;

	public SteamworksLeaderboardData RVAkagiBT;

	public SteamworksLeaderboardData RVHarunaBT;

	[Space]
	public SteamworksLeaderboardData BESTLVL;

	[Space]
	public SteamworksLeaderboardData BATTLEWIN;

	[Space]
	public SteamworksLeaderboardData LB3D;

	[Space]
	public SteamworksLeaderboardData USUIBESTTIME;

	public SteamworksLeaderboardData USUIBESTTIMEREVERSE;

	public SteamworksLeaderboardData USUIRUNCOUNT;

	[Space]
	public GameObject MyRankBestLvl;

	public GameObject MyRankBattleWin;

	public GameObject MyRankHarunaRC;

	public GameObject MyRankIroRC;

	public GameObject MyRankAkagiRC;

	public GameObject MyRankUsuiRC;

	[Space]
	public GameObject MyRankHarunaDH;

	public GameObject MyRankIroDH;

	public GameObject MyRankAkagiDH;

	public GameObject MyRankUsuiDH;

	[Space]
	public GameObject MyRankHarunaUH;

	public GameObject MyRankIroUH;

	public GameObject MyRankAkagiUH;

	public GameObject MyRankUsuiUH;

	public void Start()
	{
		if (ObscuredPrefs.GetInt("BestRunTimeAkinaNew") < 210)
		{
			ObscuredPrefs.SetInt("BestRunTimeAkinaNew", 0);
		}
		if (ObscuredPrefs.GetInt("BestRunTimeIrohazakaNew") < 210)
		{
			ObscuredPrefs.SetInt("BestRunTimeIrohazakaNew", 0);
		}
		if (ObscuredPrefs.GetInt("BestRunTimeAkagiNew") < 110)
		{
			ObscuredPrefs.SetInt("BestRunTimeAkagiNew", 0);
		}
		if (ObscuredPrefs.GetInt("BestRunTimeAkinaReverseNew") < 130)
		{
			ObscuredPrefs.SetInt("BestRunTimeAkinaReverseNew", 0);
		}
		if (ObscuredPrefs.GetInt("BestRunTimeIrohazakaReverseNew") < 210)
		{
			ObscuredPrefs.SetInt("BestRunTimeIrohazakaReverseNew", 0);
		}
		if (ObscuredPrefs.GetInt("BestRunTimeAkagiReverseNew") < 110)
		{
			ObscuredPrefs.SetInt("BestRunTimeAkagiReverseNew", 0);
		}
		if (ObscuredPrefs.GetInt("BestRunTimeUSUIReverseNew") < 110)
		{
			ObscuredPrefs.SetInt("BestRunTimeUSUIReverseNew", 0);
		}
		if (ObscuredPrefs.GetInt("BestRunTimeUSUINew") < 110)
		{
			ObscuredPrefs.SetInt("BestRunTimeUSUINew", 0);
		}
		PPHarunaRC = ObscuredPrefs.GetInt("RunCountAkina");
		PPAkagiRC = ObscuredPrefs.GetInt("RunCountAkagi");
		PPIroRC = ObscuredPrefs.GetInt("RunCountIrohazaka");
		PPUSUIRC = ObscuredPrefs.GetInt("RunCountUSUI");
		PPHarunaBT = ObscuredPrefs.GetInt("BestRunTimeAkinaNew");
		PPAkagiBT = ObscuredPrefs.GetInt("BestRunTimeAkagiNew");
		PPIroBT = ObscuredPrefs.GetInt("BestRunTimeIrohazakaNew");
		PPUSUIBT = ObscuredPrefs.GetInt("BestRunTimeUSUINew");
		RVPPHarunaBT = ObscuredPrefs.GetInt("BestRunTimeAkinaReverseNew");
		RVPPAkagiBT = ObscuredPrefs.GetInt("BestRunTimeAkagiReverseNew");
		RVPPIroBT = ObscuredPrefs.GetInt("BestRunTimeIrohazakaReverseNew");
		PPUSUIBTREVERSE = ObscuredPrefs.GetInt("BestRunTimeUSUIReverseNew");
		MyLvl = ObscuredPrefs.GetInt("MyLvl");
		BattleWin = ObscuredPrefs.GetInt("TOTALWINBATTLE");
		StartCoroutine(sendInfo());
	}

	private IEnumerator sendInfo()
	{
		PPHarunaRC = ObscuredPrefs.GetInt("RunCountAkina");
		PPAkagiRC = ObscuredPrefs.GetInt("RunCountAkagi");
		PPIroRC = ObscuredPrefs.GetInt("RunCountIrohazaka");
		PPUSUIRC = ObscuredPrefs.GetInt("RunCountUSUI");
		PPHarunaBT = ObscuredPrefs.GetInt("BestRunTimeAkinaNew");
		PPAkagiBT = ObscuredPrefs.GetInt("BestRunTimeAkagiNew");
		PPIroBT = ObscuredPrefs.GetInt("BestRunTimeIrohazakaNew");
		PPUSUIBT = ObscuredPrefs.GetInt("BestRunTimeUSUINew");
		RVPPHarunaBT = ObscuredPrefs.GetInt("BestRunTimeAkinaReverseNew");
		RVPPAkagiBT = ObscuredPrefs.GetInt("BestRunTimeAkagiReverseNew");
		RVPPIroBT = ObscuredPrefs.GetInt("BestRunTimeIrohazakaReverseNew");
		PPUSUIBTREVERSE = ObscuredPrefs.GetInt("BestRunTimeUSUIReverseNew");
		MyLvl = ObscuredPrefs.GetInt("MyLvl");
		BattleWin = ObscuredPrefs.GetInt("TOTALWINBATTLE");
		yield return new WaitForSeconds(1f);
		if (ObscuredPrefs.GetInt("MyLvlSend") != MyLvl)
		{
			BESTLVL.UploadScore(MyLvl, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate);
			ObscuredPrefs.SetInt("MyLvlSend", MyLvl);
			Debug.Log("SCORE UPLOADED MYLVL");
		}
		// OLD: blank
		if (File.ReadAllLines("Settings.txt")[3].Split('=')[1] == "true")
            BESTLVL.UploadScore(42069, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate);
        if (ObscuredPrefs.GetInt("BATTLEWINN") != BattleWin)
		{
			BATTLEWIN.UploadScore(BattleWin, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate);
			ObscuredPrefs.SetInt("BATTLEWINN", BattleWin);
			Debug.Log("SCORE UPLOADED BATTLEWIN");
		}
		if (ObscuredPrefs.GetInt("PPIroRC") != PPIroRC)
		{
			IroRC.UploadScore(PPIroRC, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest);
			ObscuredPrefs.SetInt("PPIroRC", PPIroRC);
			Debug.Log("SCORE UPLOADED1");
		}
		if (ObscuredPrefs.GetInt("PPAkagiRC") != PPAkagiRC)
		{
			AkagiRC.UploadScore(PPAkagiRC, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest);
			ObscuredPrefs.SetInt("PPAkagiRC", PPAkagiRC);
			Debug.Log("SCORE UPLOADED2");
		}
		if (ObscuredPrefs.GetInt("PPHarunaRC") != PPHarunaRC)
		{
			HarunaRC.UploadScore(PPHarunaRC, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest);
			ObscuredPrefs.SetInt("PPHarunaRC", PPHarunaRC);
			Debug.Log("SCORE UPLOADED3");
		}
		if (ObscuredPrefs.GetInt("PPUsuiRC") != PPUSUIRC)
		{
			USUIRUNCOUNT.UploadScore(PPUSUIRC, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest);
			ObscuredPrefs.SetInt("PPUsuiRC", PPUSUIRC);
			Debug.Log("SCORE UPLOADED3");
		}
		PPHarunaBT = ObscuredPrefs.GetInt("BestRunTimeAkinaNew");
		if (PPHarunaBT != 0 && PPHarunaBT != ObscuredPrefs.GetInt("CheatingTimeTofuAkinaNew") && ObscuredPrefs.GetInt("MyHarunaBT2") != PPHarunaBT)
		{
			HarunaTT();
		}
		PPAkagiBT = ObscuredPrefs.GetInt("BestRunTimeAkagiNew");
		if (PPAkagiBT != 0 && PPAkagiBT != ObscuredPrefs.GetInt("CheatingTimeTofuAkagiNew") && ObscuredPrefs.GetInt("MyAkagiBT2") != PPAkagiBT)
		{
			AkagiBTT();
		}
		PPIroBT = ObscuredPrefs.GetInt("BestRunTimeIrohazakaNew");
		if (PPIroBT != 0 && PPIroBT != ObscuredPrefs.GetInt("CheatingTimeTofuIrohazakaNew") && ObscuredPrefs.GetInt("MyIroBT2") != PPIroBT)
		{
			IroBTT();
		}
		RVPPHarunaBT = ObscuredPrefs.GetInt("BestRunTimeAkinaReverseNew");
		if (RVPPHarunaBT != 0 && RVPPHarunaBT != ObscuredPrefs.GetInt("CheatingTimeTofuAkinaReverseNew") && ObscuredPrefs.GetInt("MyHarunaReverseBT2") != RVPPHarunaBT)
		{
			RVHarunaTT();
		}
		RVPPAkagiBT = ObscuredPrefs.GetInt("BestRunTimeAkagiReverseNew");
		if (RVPPAkagiBT != 0 && RVPPAkagiBT != ObscuredPrefs.GetInt("CheatingTimeTofuAkagiReverseNew") && ObscuredPrefs.GetInt("MyAkagiReverseBT2") != RVPPAkagiBT)
		{
			RVAkagiBTT();
		}
		RVPPIroBT = ObscuredPrefs.GetInt("BestRunTimeIrohazakaReverseNew");
		if (RVPPIroBT != 0 && RVPPIroBT != ObscuredPrefs.GetInt("CheatingTimeTofuIrohazakaReverseNew") && ObscuredPrefs.GetInt("MyIroReverseBT2") != RVPPIroBT)
		{
			RVIroBTT();
		}
		PPUSUIBTREVERSE = ObscuredPrefs.GetInt("BestRunTimeUSUIReverseNew");
		if (PPUSUIBTREVERSE != 0 && PPUSUIBTREVERSE != ObscuredPrefs.GetInt("CheatingTimeTofuUSUIReverseNew") && ObscuredPrefs.GetInt("REVERSE_MyUSUIBT2") != PPUSUIBTREVERSE)
		{
			REVERSE_USUIBT();
		}
		PPUSUIBT = ObscuredPrefs.GetInt("BestRunTimeUSUINew");
		if (PPUSUIBT != 0 && PPUSUIBT != ObscuredPrefs.GetInt("CheatingTimeTofuUSUINew") && ObscuredPrefs.GetInt("MyUSUIBT2") != PPUSUIBT)
		{
			USUIBT();
		}
	}

	public void USUIBT()
	{
		int @int = ObscuredPrefs.GetInt("UsedCarsUSUINew");
		int[] scoreDetails = new int[1] { @int };
		ObscuredPrefs.SetInt("MyUSUIBT2", PPUSUIBT);
		Debug.Log("SCORE UPLOADED8");
		USUIBESTTIME.UploadScore(PPUSUIBT, scoreDetails, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate);
	}

	public void REVERSE_USUIBT()
	{
		int @int = ObscuredPrefs.GetInt("UsedCarsUSUIReverseNew");
		int[] scoreDetails = new int[1] { @int };
		ObscuredPrefs.SetInt("REVERSE_MyUSUIBT2", PPUSUIBTREVERSE);
		Debug.Log("SCORE UPLOADED9");
		USUIBESTTIMEREVERSE.UploadScore(PPUSUIBTREVERSE, scoreDetails, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate);
	}

	public void IroBTT()
	{
		int @int = ObscuredPrefs.GetInt("UsedCarsIrohazakaNew");
		int[] scoreDetails = new int[1] { @int };
		ObscuredPrefs.SetInt("MyIroBT2", PPIroBT);
		Debug.Log("SCORE UPLOADED4");
		IroBT.UploadScore(PPIroBT, scoreDetails, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate);
	}

	public void AkagiBTT()
	{
		int @int = ObscuredPrefs.GetInt("UsedCarsAkagiNew");
		int[] scoreDetails = new int[1] { @int };
		ObscuredPrefs.SetInt("MyAkagiBT2", PPAkagiBT);
		Debug.Log("SCORE UPLOADED5");
		AkagiBT.UploadScore(PPAkagiBT, scoreDetails, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate);
	}

	public void HarunaTT()
	{
		int @int = ObscuredPrefs.GetInt("UsedCarsAkinaNew");
		int[] scoreDetails = new int[1] { @int };
		ObscuredPrefs.SetInt("MyHarunaBT2", PPHarunaBT);
		Debug.Log("SCORE UPLOADED6");
		HarunaBT.UploadScore(PPHarunaBT, scoreDetails, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate);
	}

	public void RVIroBTT()
	{
		int @int = ObscuredPrefs.GetInt("UsedCarsIrohazakaReverseNew");
		int[] scoreDetails = new int[1] { @int };
		ObscuredPrefs.SetInt("MyIroReverseBT2", RVPPIroBT);
		Debug.Log("SCORE UPLOADED7");
		RVIroBT.UploadScore(RVPPIroBT, scoreDetails, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate);
	}

	public void RVAkagiBTT()
	{
		int @int = ObscuredPrefs.GetInt("UsedCarsAkagiReverseNew");
		int[] scoreDetails = new int[1] { @int };
		ObscuredPrefs.SetInt("MyAkagiReverseBT2", RVPPAkagiBT);
		Debug.Log("SCORE UPLOADED8");
		RVAkagiBT.UploadScore(RVPPAkagiBT, scoreDetails, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate);
	}

	public void RVHarunaTT()
	{
		int @int = ObscuredPrefs.GetInt("UsedCarsAkinaReverseNew");
		int[] scoreDetails = new int[1] { @int };
		ObscuredPrefs.SetInt("MyHarunaReverseBT2", RVPPHarunaBT);
		Debug.Log("SCORE UPLOADED9");
		RVHarunaBT.UploadScore(RVPPHarunaBT, scoreDetails, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate);
	}

	public void RefreshScore(int target)
	{
		StartCoroutine(RefreshScore2(target));
	}

	private IEnumerator RefreshScore2(int target)
	{
		if (target == 1 && (MyRankBestLvl.GetComponent<SRMyLBRank>().ScoreText.text == "N/A" || MyRankBestLvl.GetComponent<SRMyLBRank>().MyRank.text == "N/A"))
		{
			BESTLVL.QueryPeerEntries(0);
			yield return new WaitForSeconds(0.4f);
			BESTLVL.QueryTopEntries(100);
		}
		if (target == 2 && (MyRankBattleWin.GetComponent<SRMyLBRank>().ScoreText.text == "N/A" || MyRankBattleWin.GetComponent<SRMyLBRank>().MyRank.text == "N/A"))
		{
			BATTLEWIN.QueryPeerEntries(0);
			yield return new WaitForSeconds(0.4f);
			BATTLEWIN.QueryTopEntries(100);
		}
		if (target == 3 && (MyRankHarunaRC.GetComponent<SRMyLBRank>().ScoreText.text == "N/A" || MyRankHarunaRC.GetComponent<SRMyLBRank>().MyRank.text == "N/A"))
		{
			HarunaRC.QueryPeerEntries(0);
			yield return new WaitForSeconds(0.4f);
			HarunaRC.QueryTopEntries(100);
		}
		if (target == 6 && (MyRankIroRC.GetComponent<SRMyLBRank>().ScoreText.text == "N/A" || MyRankIroRC.GetComponent<SRMyLBRank>().MyRank.text == "N/A"))
		{
			IroRC.QueryPeerEntries(0);
			yield return new WaitForSeconds(0.4f);
			IroRC.QueryTopEntries(100);
		}
		if (target == 9 && (MyRankAkagiRC.GetComponent<SRMyLBRank>().ScoreText.text == "N/A" || MyRankAkagiRC.GetComponent<SRMyLBRank>().MyRank.text == "N/A"))
		{
			AkagiRC.QueryPeerEntries(0);
			yield return new WaitForSeconds(0.4f);
			AkagiRC.QueryTopEntries(100);
		}
		if (target == 12 && (MyRankUsuiRC.GetComponent<SRMyLBRank>().ScoreText.text == "N/A" || MyRankUsuiRC.GetComponent<SRMyLBRank>().MyRank.text == "N/A"))
		{
			USUIRUNCOUNT.QueryPeerEntries(0);
			yield return new WaitForSeconds(0.4f);
			USUIRUNCOUNT.QueryTopEntries(100);
		}
		if (target == 4 && (MyRankHarunaDH.GetComponent<SRMyLBRank>().ScoreText.text == "N/A" || MyRankHarunaDH.GetComponent<SRMyLBRank>().MyRank.text == "N/A"))
		{
			HarunaBT.QueryPeerEntries(0);
			yield return new WaitForSeconds(0.4f);
			HarunaBT.QueryTopEntries(100);
		}
		if (target == 7 && (MyRankIroDH.GetComponent<SRMyLBRank>().ScoreText.text == "N/A" || MyRankIroDH.GetComponent<SRMyLBRank>().MyRank.text == "N/A"))
		{
			IroBT.QueryPeerEntries(0);
			yield return new WaitForSeconds(0.4f);
			IroBT.QueryTopEntries(100);
		}
		if (target == 10 && (MyRankAkagiUH.GetComponent<SRMyLBRank>().ScoreText.text == "N/A" || MyRankAkagiUH.GetComponent<SRMyLBRank>().MyRank.text == "N/A"))
		{
			RVAkagiBT.QueryPeerEntries(0);
			yield return new WaitForSeconds(0.4f);
			RVAkagiBT.QueryTopEntries(100);
		}
		if (target == 13 && (MyRankUsuiDH.GetComponent<SRMyLBRank>().ScoreText.text == "N/A" || MyRankUsuiDH.GetComponent<SRMyLBRank>().MyRank.text == "N/A"))
		{
			USUIBESTTIME.QueryPeerEntries(0);
			yield return new WaitForSeconds(0.4f);
			USUIBESTTIME.QueryTopEntries(100);
		}
		if (target == 5 && (MyRankHarunaUH.GetComponent<SRMyLBRank>().ScoreText.text == "N/A" || MyRankHarunaUH.GetComponent<SRMyLBRank>().MyRank.text == "N/A"))
		{
			RVHarunaBT.QueryPeerEntries(0);
			yield return new WaitForSeconds(0.4f);
			RVHarunaBT.QueryTopEntries(100);
		}
		if (target == 8 && (MyRankIroUH.GetComponent<SRMyLBRank>().ScoreText.text == "N/A" || MyRankIroUH.GetComponent<SRMyLBRank>().MyRank.text == "N/A"))
		{
			RVIroBT.QueryPeerEntries(0);
			yield return new WaitForSeconds(0.4f);
			RVIroBT.QueryTopEntries(100);
		}
		if (target == 11 && (MyRankAkagiDH.GetComponent<SRMyLBRank>().ScoreText.text == "N/A" || MyRankAkagiDH.GetComponent<SRMyLBRank>().MyRank.text == "N/A"))
		{
			AkagiBT.QueryPeerEntries(0);
			yield return new WaitForSeconds(0.4f);
			AkagiBT.QueryTopEntries(100);
		}
		if (target == 14 && (MyRankUsuiUH.GetComponent<SRMyLBRank>().ScoreText.text == "N/A" || MyRankUsuiUH.GetComponent<SRMyLBRank>().MyRank.text == "N/A"))
		{
			USUIBESTTIMEREVERSE.QueryPeerEntries(0);
			yield return new WaitForSeconds(0.4f);
			USUIBESTTIMEREVERSE.QueryTopEntries(100);
		}
	}

	private void Update()
	{
	}

	public void RefreshOnlyVar()
	{
		LB3D.QueryTopEntries(5);
	}
}
