using System;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamApi.GameServices;

[CreateAssetMenu(menuName = "Steamworks/Game Services/DLC Data")]
public class SteamDLCData : ScriptableObject
{
	public AppId_t AppId;

	public bool IsSubscribed;

	public bool IsDlcInstalled;

	public bool IsDownloading;

	public void UpdateStatus()
	{
		GetIsSubscribed();
		GetIsInstalled();
		GetDownloadProgress();
	}

	public bool GetIsSubscribed()
	{
		IsSubscribed = SteamApps.BIsSubscribedApp(AppId);
		return IsSubscribed;
	}

	public bool GetIsInstalled()
	{
		IsDlcInstalled = SteamApps.BIsDlcInstalled(AppId);
		return IsDlcInstalled;
	}

	public string GetInstallDirectory()
	{
		if (SteamApps.GetAppInstallDir(AppId, out var pchFolder, 2048u) != 0)
		{
			return pchFolder;
		}
		return string.Empty;
	}

	public float GetDownloadProgress()
	{
		IsDownloading = SteamApps.GetDlcDownloadProgress(AppId, out var punBytesDownloaded, out var punBytesTotal);
		if (IsDownloading)
		{
			return Convert.ToSingle((double)punBytesDownloaded / (double)punBytesTotal);
		}
		return 0f;
	}

	public DateTime GetEarliestPurchaseTime()
	{
		uint earliestPurchaseUnixTime = SteamApps.GetEarliestPurchaseUnixTime(AppId);
		return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(earliestPurchaseUnixTime);
	}

	public void InstallDLC()
	{
		SteamApps.InstallDLC(AppId);
	}

	public void UninstallDLC()
	{
		SteamApps.UninstallDLC(AppId);
	}

	public void OpenStore(EOverlayToStoreFlag flag = EOverlayToStoreFlag.k_EOverlayToStoreFlag_None)
	{
		SteamFriends.ActivateGameOverlayToStore(AppId, flag);
	}
}
