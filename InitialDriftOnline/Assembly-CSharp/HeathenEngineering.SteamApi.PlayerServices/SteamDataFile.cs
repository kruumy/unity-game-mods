using System;
using System.Text;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamApi.PlayerServices;

[Serializable]
public class SteamDataFile
{
	public SteamworksRemoteStorageManager.FileAddress address;

	[HideInInspector]
	public byte[] binaryData;

	[HideInInspector]
	public SteamAPICall_t? apiCall;

	[HideInInspector]
	public EResult result = EResult.k_EResultPending;

	[HideInInspector]
	public SteamDataLibrary linkedLibrary;

	public Action<SteamDataFile> Complete;

	public void ReadFromLibrary(SteamDataLibrary dataLibrary)
	{
		linkedLibrary = dataLibrary;
		dataLibrary.SyncToBuffer(out binaryData);
	}

	public void WriteToLibrary(SteamDataLibrary dataLibrary)
	{
		linkedLibrary = dataLibrary;
		dataLibrary.SyncFromBuffer(binaryData);
	}

	public void SetDataFromObject(object jsonObject, Encoding encoding)
	{
		binaryData = encoding.GetBytes(JsonUtility.ToJson(jsonObject));
	}

	public string FromUTF8()
	{
		if (binaryData.Length != 0)
		{
			return Encoding.UTF8.GetString(binaryData);
		}
		return string.Empty;
	}

	public string FromUTF32()
	{
		if (binaryData.Length != 0)
		{
			return Encoding.UTF32.GetString(binaryData);
		}
		return string.Empty;
	}

	public string FromUnicode()
	{
		if (binaryData.Length != 0)
		{
			return Encoding.Unicode.GetString(binaryData);
		}
		return string.Empty;
	}

	public string FromDefaultEncoding()
	{
		if (binaryData.Length != 0)
		{
			return Encoding.Default.GetString(binaryData);
		}
		return string.Empty;
	}

	public string FromASCII()
	{
		if (binaryData.Length != 0)
		{
			return Encoding.ASCII.GetString(binaryData);
		}
		return string.Empty;
	}

	public string FromEncoding(Encoding encoding)
	{
		return encoding.GetString(binaryData);
	}

	public T FromJson<T>(Encoding encoding)
	{
		return JsonUtility.FromJson<T>(encoding.GetString(binaryData));
	}

	public void HandleFileReadAsyncComplete(RemoteStorageFileReadAsyncComplete_t param, bool bIOFailure)
	{
		result = param.m_eResult;
		if (result == EResult.k_EResultOK)
		{
			binaryData = new byte[address.fileSize];
			if (!SteamRemoteStorage.FileReadAsyncComplete(param.m_hFileReadAsync, binaryData, (uint)binaryData.Length))
			{
				result = EResult.k_EResultFail;
			}
			else if (linkedLibrary != null)
			{
				linkedLibrary.activeFile = this;
				WriteToLibrary(linkedLibrary);
			}
		}
		if (Complete != null)
		{
			Complete(this);
		}
	}

	public void HandleFileWriteAsyncComplete(RemoteStorageFileWriteAsyncComplete_t param, bool bIOFailure)
	{
		result = param.m_eResult;
		if (result == EResult.k_EResultOK && linkedLibrary != null)
		{
			linkedLibrary.activeFile = this;
			WriteToLibrary(linkedLibrary);
		}
		if (Complete != null)
		{
			Complete(this);
		}
	}
}
