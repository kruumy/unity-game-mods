using System;
using System.Text;
using HeathenEngineering.SteamApi.PlayerServices;
using UnityEngine;
using UnityEngine.UI;

public class TestCloud2 : MonoBehaviour
{
	[Serializable]
	public class MyStatsSaving
	{
		public int XP;

		public float PLAYINGTIME;
	}

	public Text XPtxt;

	public Text PTIMEtxt;

	public MyStatsSaving ReadingData;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void ReadData()
	{
		SteamworksRemoteStorageManager.RefreshFileList();
		MyStatsSaving myStatsSaving = SteamworksRemoteStorageManager.FileReadJson<MyStatsSaving>("MyStatsSavingFile.dat", Encoding.UTF8);
		Debug.Log("XP : " + myStatsSaving.XP);
		ReadingData.XP = myStatsSaving.XP;
		Debug.Log("XP : " + myStatsSaving.PLAYINGTIME);
		ReadingData.PLAYINGTIME = myStatsSaving.PLAYINGTIME;
		XPtxt.text = (" XP : " + myStatsSaving.XP) ?? "";
		PTIMEtxt.text = (" PLAYINGTIME : " + myStatsSaving.PLAYINGTIME) ?? "";
	}
}
