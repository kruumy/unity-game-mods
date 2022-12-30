using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamApi.Foundation;

public static class SteamUtilities
{
	public static class Colors
	{
		public static Color SteamBlue = new Color(0.2f, 0.6f, 0.93f, 1f);

		public static Color SteamGreen = new Color(0.2f, 0.42f, 0.2f, 1f);

		public static Color BrightGreen = new Color(0.4f, 0.84f, 0.4f, 1f);

		public static Color HalfAlpha = new Color(1f, 1f, 1f, 0.5f);

		public static Color ErrorRed = new Color(1f, 0.5f, 0.5f, 1f);
	}

	public static bool IsSteamInBigPictureMode => SteamUtils.IsSteamInBigPictureMode();

	public static bool IsSteamRunningInVR => SteamUtils.IsSteamRunningInVR();

	public static bool OverlayNeedsPresent()
	{
		return SteamUtils.BOverlayNeedsPresent();
	}

	public static AppId_t GetAppId()
	{
		return SteamUtils.GetAppID();
	}

	public static float GetCurrentBatteryPower()
	{
		return Mathf.Clamp01((float)(int)SteamUtils.GetCurrentBatteryPower() / 100f);
	}

	public static uint GetServerRealUnixTime()
	{
		return SteamUtils.GetServerRealTime();
	}

	public static DateTime GetServerRealDateTime()
	{
		return ConvertUnixDate(GetServerRealUnixTime());
	}

	public static string GetSteamClientLanguage()
	{
		return SteamUtils.GetSteamUILanguage();
	}

	public static string GetCurrentGameLanguage()
	{
		return SteamApps.GetCurrentGameLanguage();
	}

	public static int GetAppBuildId()
	{
		return SteamApps.GetAppBuildId();
	}

	public static string GetAppInstallDir(AppId_t appId)
	{
		SteamApps.GetAppInstallDir(appId, out var pchFolder, 1024u);
		return pchFolder;
	}

	public static CSteamID GetAppOwner()
	{
		return SteamApps.GetAppOwner();
	}

	public static int GetDLCCount()
	{
		return SteamApps.GetDLCCount();
	}

	public static AppDlcData GetDLCDataByIndex(int index)
	{
		AppDlcData result = default(AppDlcData);
		if (SteamApps.BGetDLCDataByIndex(index, out result.appId, out result.available, out result.name, 2048))
		{
			return result;
		}
		result.appId = AppId_t.Invalid;
		return result;
	}

	public static List<AppDlcData> GetDLCData()
	{
		List<AppDlcData> list = new List<AppDlcData>();
		int dLCCount = GetDLCCount();
		for (int i = 0; i < dLCCount; i++)
		{
			AppDlcData item = default(AppDlcData);
			if (SteamApps.BGetDLCDataByIndex(i, out item.appId, out item.available, out item.name, 2048))
			{
				list.Add(item);
			}
		}
		return list;
	}

	public static uint GetEarliestPurchaseUnixTime(AppId_t appId)
	{
		return SteamApps.GetEarliestPurchaseUnixTime(appId);
	}

	public static DateTime GetEarliestPurchaseDateTime(AppId_t appId)
	{
		return ConvertUnixDate(GetEarliestPurchaseUnixTime(appId));
	}

	public static string GetLaunchCommandLine()
	{
		SteamApps.GetLaunchCommandLine(out var pszCommandLine, 1024);
		return pszCommandLine;
	}

	public static byte[] FlipImageBufferVertical(int width, int height, byte[] buffer)
	{
		byte[] array = new byte[buffer.Length];
		int num = width * 4;
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < num; j++)
			{
				array[j + (height - 1 - i) * num] = buffer[j + num * i];
			}
		}
		return array;
	}

	public static bool LoadImageFromDisk(string path, out Texture2D texture)
	{
		if (File.Exists(path))
		{
			byte[] data = File.ReadAllBytes(path);
			texture = new Texture2D(2, 2);
			return texture.LoadImage(data);
		}
		Debug.LogError("Load Image From Disk called on file [" + path + "] but no such file was found.");
		texture = new Texture2D(2, 2);
		return false;
	}

	public static DateTime ConvertUnixDate(uint nixTime)
	{
		return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(nixTime);
	}

	public static bool WorkshopItemStateHasFlag(EItemState value, EItemState checkflag)
	{
		return (value & checkflag) == checkflag;
	}

	public static bool WorkshopItemStateHasAllFlags(EItemState value, params EItemState[] checkflags)
	{
		foreach (EItemState eItemState in checkflags)
		{
			if ((value & eItemState) != eItemState)
			{
				return false;
			}
		}
		return true;
	}

	public static byte[] IPStringToBytes(string address)
	{
		return IPAddress.Parse(address).GetAddressBytes();
	}

	public static string IPBytesToString(byte[] address)
	{
		return new IPAddress(address).ToString();
	}

	public static uint IPStringToUint(string address)
	{
		byte[] array = IPStringToBytes(address);
		return (uint)((array[0] << 24) + (array[1] << 16) + (array[2] << 8) + array[3]);
	}

	public static string IPUintToString(uint address)
	{
		byte[] bytes = BitConverter.GetBytes(address);
		return new IPAddress(new byte[4]
		{
			bytes[3],
			bytes[2],
			bytes[1],
			bytes[0]
		}).ToString();
	}
}
