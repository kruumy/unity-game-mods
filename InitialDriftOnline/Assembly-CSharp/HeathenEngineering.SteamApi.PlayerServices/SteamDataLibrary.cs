using System;
using System.Collections.Generic;
using HeathenEngineering.Scriptable;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamApi.PlayerServices;

[Serializable]
[CreateAssetMenu(menuName = "Library Variables/Steam Game Data Library")]
public class SteamDataLibrary : DataLibraryVariable
{
	public string filePrefix;

	[HideInInspector]
	public SteamDataFile activeFile;

	[HideInInspector]
	public List<SteamworksRemoteStorageManager.FileAddress> availableFiles = new List<SteamworksRemoteStorageManager.FileAddress>();

	public void Save()
	{
		if (activeFile == null)
		{
			Debug.Log("");
		}
		activeFile.linkedLibrary = this;
		SteamworksRemoteStorageManager.FileWrite(activeFile);
	}

	public void SaveAs(string fileName)
	{
		if (!string.IsNullOrEmpty(fileName))
		{
			if (!fileName.StartsWith(filePrefix))
			{
				fileName = filePrefix + fileName;
			}
			if (SteamworksRemoteStorageManager.FileWrite(fileName, this))
			{
				Debug.Log("[SteamDataLibrary.SaveAs] Saved '" + fileName + "' successfully.");
			}
			else
			{
				Debug.LogWarning("[SteamDataLibrary.SaveAs] Failed to save '" + fileName + "' to Steam Remote Storage.\nPlease consult https://partner.steamgames.com/doc/api/ISteamRemoteStorage#FileWrite for more information.");
			}
		}
	}

	public void SaveAsync()
	{
		activeFile.linkedLibrary = this;
		SteamDataFile steamDataFile = SteamworksRemoteStorageManager.FileWriteAsync(activeFile);
		if (steamDataFile.result != EResult.k_EResultFail)
		{
			steamDataFile.Complete = delegate(SteamDataFile results)
			{
				activeFile = results;
			};
		}
	}

	public void SaveAsAsync(string fileName)
	{
		if (string.IsNullOrEmpty(fileName))
		{
			return;
		}
		if (!fileName.StartsWith(filePrefix))
		{
			fileName = filePrefix + fileName;
		}
		SteamDataFile steamDataFile = SteamworksRemoteStorageManager.FileWriteAsync(fileName, this);
		if (steamDataFile.result != EResult.k_EResultFail)
		{
			steamDataFile.Complete = delegate(SteamDataFile results)
			{
				activeFile = results;
			};
		}
	}

	public void Load(string fileName)
	{
		if (fileName.StartsWith(filePrefix))
		{
			(activeFile = SteamworksRemoteStorageManager.FileReadSteamDataFile(fileName)).WriteToLibrary(this);
		}
		else
		{
			(activeFile = SteamworksRemoteStorageManager.FileReadSteamDataFile(filePrefix + fileName)).WriteToLibrary(this);
		}
	}

	public void LoadAsync(string fileName)
	{
		if (fileName.StartsWith(filePrefix))
		{
			SteamworksRemoteStorageManager.FileReadAsync(fileName).Complete = delegate(SteamDataFile fileResult)
			{
				activeFile = fileResult;
				fileResult.WriteToLibrary(this);
			};
		}
		else
		{
			SteamworksRemoteStorageManager.FileReadAsync(filePrefix + fileName).Complete = delegate(SteamDataFile fileResult)
			{
				activeFile = fileResult;
				fileResult.WriteToLibrary(this);
			};
		}
	}

	public void Load()
	{
		if (activeFile != null)
		{
			activeFile = SteamworksRemoteStorageManager.FileReadSteamDataFile(activeFile.address);
			activeFile.WriteToLibrary(this);
		}
	}

	public void LoadAsync()
	{
		if (activeFile != null)
		{
			SteamworksRemoteStorageManager.FileReadAsync(activeFile.address).Complete = delegate(SteamDataFile results)
			{
				activeFile = results;
				activeFile.WriteToLibrary(this);
			};
		}
	}

	public void Load(SteamworksRemoteStorageManager.FileAddress address)
	{
		if (!string.IsNullOrEmpty(address.fileName) && address.fileName.StartsWith(filePrefix))
		{
			activeFile = SteamworksRemoteStorageManager.FileReadSteamDataFile(address);
			activeFile.WriteToLibrary(this);
		}
	}

	public void LoadAsync(SteamworksRemoteStorageManager.FileAddress address)
	{
		if (string.IsNullOrEmpty(address.fileName) || !address.fileName.StartsWith(filePrefix))
		{
			return;
		}
		SteamDataFile steamDataFile = SteamworksRemoteStorageManager.FileReadAsync(address);
		if (steamDataFile.result != EResult.k_EResultFail)
		{
			steamDataFile.Complete = delegate(SteamDataFile results)
			{
				activeFile = results;
				activeFile.WriteToLibrary(this);
			};
		}
	}

	public void Load(int availableFileIndex)
	{
		if (availableFileIndex >= 0 && availableFileIndex < availableFiles.Count)
		{
			Load(availableFiles[availableFileIndex]);
		}
	}

	public void LoadAsync(int availableFileIndex)
	{
		if (availableFileIndex >= 0 && availableFileIndex < availableFiles.Count)
		{
			LoadAsync(availableFiles[availableFileIndex]);
		}
	}
}
