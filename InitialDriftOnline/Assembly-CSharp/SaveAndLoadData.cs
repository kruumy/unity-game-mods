using System.IO;
using HeathenEngineering.Scriptable;
using UnityEngine;

public class SaveAndLoadData : MonoBehaviour
{
	public string filePath = "./ Data Library Example/library.dat";

	public DataLibraryVariable library;

	[ContextMenu("Save")]
	public void Save()
	{
		library.SyncToFile(filePath, createDirectory: true);
		FileInfo fileInfo = new FileInfo(filePath);
		Debug.Log("Saved file to [" + fileInfo.FullName + "]");
	}

	[ContextMenu("Load")]
	public void Load()
	{
		FileInfo fileInfo = new FileInfo(filePath);
		if (fileInfo.Exists)
		{
			library.SyncFromFile(filePath);
			Debug.Log("Loaded file from [" + fileInfo.FullName + "]");
		}
		else
		{
			Debug.Log("No file found.");
		}
	}
}
