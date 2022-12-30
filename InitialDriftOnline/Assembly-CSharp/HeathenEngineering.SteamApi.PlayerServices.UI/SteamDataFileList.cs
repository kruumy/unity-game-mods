using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HeathenEngineering.Scriptable;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HeathenEngineering.SteamApi.PlayerServices.UI;

public class SteamDataFileList : MonoBehaviour
{
	public SteamDataLibrary Library;

	public SteamDataFileRecord RecordPrefab;

	public RectTransform Container;

	public BoolReference RemovePrefix = new BoolReference(value: true);

	public StringReference DateDisplayFormat = new StringReference("F");

	[Header("Events")]
	public UnityEvent SelectionChanged;

	private SteamworksRemoteStorageManager.FileAddress? s_SelectedFile;

	public SteamDataFile Active => Library.activeFile;

	public SteamworksRemoteStorageManager.FileAddress? SelectedFile
	{
		get
		{
			return s_SelectedFile;
		}
		set
		{
			if (s_SelectedFile.HasValue && value.HasValue)
			{
				if (s_SelectedFile.Value != value.Value)
				{
					s_SelectedFile = value;
					SelectionChanged.Invoke();
				}
			}
			else if (s_SelectedFile.HasValue != value.HasValue)
			{
				s_SelectedFile = value;
				SelectionChanged.Invoke();
			}
		}
	}

	public void Refresh()
	{
		SteamworksRemoteStorageManager.RefreshFileList();
		List<GameObject> list = new List<GameObject>();
		foreach (Transform item in Container)
		{
			list.Add(item.gameObject);
		}
		while (list.Count > 0)
		{
			GameObject gameObject = list[0];
			list.Remove(gameObject);
			Object.Destroy(gameObject);
		}
		Library.availableFiles.Sort((SteamworksRemoteStorageManager.FileAddress p1, SteamworksRemoteStorageManager.FileAddress p2) => p1.UtcTimestamp.CompareTo(p2.UtcTimestamp));
		Library.availableFiles.Reverse();
		foreach (SteamworksRemoteStorageManager.FileAddress availableFile in Library.availableFiles)
		{
			SteamDataFileRecord component = Object.Instantiate(RecordPrefab.gameObject, Container).GetComponent<SteamDataFileRecord>();
			component.parentList = this;
			component.Address = availableFile;
			if (RemovePrefix.Value && availableFile.fileName.StartsWith(Library.filePrefix))
			{
				component.FileName.text = availableFile.fileName.Substring(Library.filePrefix.Length);
			}
			else
			{
				component.FileName.text = availableFile.fileName;
			}
			component.Timestamp.text = availableFile.LocalTimestamp.ToString(DateDisplayFormat, Thread.CurrentThread.CurrentCulture);
		}
	}

	public SteamworksRemoteStorageManager.FileAddress? GetLatest()
	{
		if (Library.availableFiles.Count > 0)
		{
			return Library.availableFiles[0];
		}
		return null;
	}

	public void ClearSelected()
	{
		SelectedFile = null;
	}

	public void Select(SteamworksRemoteStorageManager.FileAddress address)
	{
		SelectedFile = address;
	}

	public void SelectLatest()
	{
		SelectedFile = GetLatest();
	}

	public void LoadSelected()
	{
		if (SelectedFile.HasValue)
		{
			Library.Load(SelectedFile.Value);
		}
	}

	public void LoadSelectedAsync()
	{
		if (SelectedFile.HasValue)
		{
			Library.LoadAsync(SelectedFile.Value);
		}
	}

	public void DeleteSelected()
	{
		if (SelectedFile.HasValue)
		{
			SteamworksRemoteStorageManager.FileDelete(SelectedFile.Value);
		}
		Refresh();
	}

	public void ForgetSelected()
	{
		if (SelectedFile.HasValue)
		{
			SteamRemoteStorage.FileForget(SelectedFile.Value.fileName);
		}
	}

	public void SaveActive()
	{
		if (SelectedFile.HasValue)
		{
			Library.Save();
			Refresh();
		}
		else
		{
			Debug.LogWarning("[SteamDataFileList.SaveActive] Attempted to save the active file but no file is active.");
		}
	}

	public void SaveAs(string fileName)
	{
		Library.SaveAs(fileName);
		Refresh();
		SelectLatest();
	}

	public void SaveAs(InputField fileName)
	{
		if (fileName == null || string.IsNullOrEmpty(fileName.text))
		{
			Debug.LogWarning("[SteamDataFileList.SaveAs] Attempted to SaveAs but was not provided with a file name ... will attempt to save the active file instead.");
			SaveActive();
			return;
		}
		Library.SaveAs(fileName.text);
		Refresh();
		Debug.Log("SAVING");
		SelectLatest();
	}

	public void SaveActiveAsync()
	{
		if (SelectedFile.HasValue)
		{
			Library.SaveAsync();
		}
	}

	public void SaveAsAsync(string fileName)
	{
		Library.SaveAsAsync(fileName);
		string fName = (fileName.StartsWith(Library.filePrefix) ? fileName : (Library.filePrefix + fileName));
		if (Library.availableFiles.Exists((SteamworksRemoteStorageManager.FileAddress p) => p.fileName == fName))
		{
			SelectedFile = Library.availableFiles.First((SteamworksRemoteStorageManager.FileAddress p) => p.fileName == fName);
		}
	}

	public void SaveAsAsync(InputField fileName)
	{
		Library.SaveAsAsync(fileName.text);
		string fName = (fileName.text.StartsWith(Library.filePrefix) ? fileName.text : (Library.filePrefix + fileName.text));
		if (Library.availableFiles.Exists((SteamworksRemoteStorageManager.FileAddress p) => p.fileName == fName))
		{
			SelectedFile = Library.availableFiles.First((SteamworksRemoteStorageManager.FileAddress p) => p.fileName == fName);
		}
	}
}
