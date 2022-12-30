using System;
using System.Text;
using HeathenEngineering.SteamApi.PlayerServices;
using UnityEngine;
using UnityEngine.UI;

public class TestCloud : MonoBehaviour
{
	[Serializable]
	public class MyStatsSaving
	{
		public int XP;

		public float PLAYINGTIME;
	}

	public MyStatsSaving myData;

	public InputField XPtxt;

	public InputField PTIMEtxt;

	private void Start()
	{
		SteamworksRemoteStorageManager.RefreshFileList();
	}

	private void Update()
	{
	}

	public void loadcloud()
	{
	}

	public void SaveData()
	{
		myData.XP = int.Parse(XPtxt.text);
		myData.PLAYINGTIME = int.Parse(PTIMEtxt.text);
		SteamworksRemoteStorageManager.FileWrite("MyStatsSavingFile.dat", myData, Encoding.UTF8);
	}
}
