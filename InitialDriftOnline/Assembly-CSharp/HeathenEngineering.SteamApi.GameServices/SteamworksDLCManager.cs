using System.Collections.Generic;
using System.Linq;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamApi.GameServices;

public class SteamworksDLCManager : MonoBehaviour
{
	public List<SteamDLCData> DLC = new List<SteamDLCData>();

	private Callback<DlcInstalled_t> m_DlcInstalled;

	private void Start()
	{
		m_DlcInstalled = Callback<DlcInstalled_t>.Create(HandleDlcInstalled);
		UpdateAll();
	}

	private void HandleDlcInstalled(DlcInstalled_t param)
	{
		SteamDLCData steamDLCData = DLC.FirstOrDefault((SteamDLCData p) => p.AppId == param.m_nAppID);
		if (steamDLCData != null)
		{
			steamDLCData.UpdateStatus();
		}
	}

	public void UpdateAll()
	{
		foreach (SteamDLCData item in DLC)
		{
			item.UpdateStatus();
		}
	}

	public SteamDLCData GetDLC(AppId_t AppId)
	{
		return DLC.FirstOrDefault((SteamDLCData p) => p.AppId == AppId);
	}

	public SteamDLCData GetDLC(string name)
	{
		return DLC.FirstOrDefault((SteamDLCData p) => p.name == name);
	}
}
