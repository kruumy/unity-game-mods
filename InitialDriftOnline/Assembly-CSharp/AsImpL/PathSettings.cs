using System.IO;
using UnityEngine;

namespace AsImpL;

public class PathSettings : MonoBehaviour
{
	[Tooltip("Default root path for models")]
	public RootPathEnum defaultRootPath;

	[Tooltip("Root path for models on mobile devices")]
	public RootPathEnum mobileRootPath;

	public string RootPath => defaultRootPath switch
	{
		RootPathEnum.DataPath => Application.dataPath + "/", 
		RootPathEnum.DataPathParent => Application.dataPath + "/../", 
		RootPathEnum.PersistentDataPath => Application.persistentDataPath + "/", 
		_ => "", 
	};

	public static PathSettings FindPathComponent(GameObject obj)
	{
		PathSettings pathSettings = obj.GetComponent<PathSettings>();
		if (pathSettings == null)
		{
			pathSettings = Object.FindObjectOfType<PathSettings>();
		}
		if (pathSettings == null)
		{
			pathSettings = obj.AddComponent<PathSettings>();
		}
		return pathSettings;
	}

	public string FullPath(string path)
	{
		string result = path;
		if (!Path.IsPathRooted(path))
		{
			result = RootPath + path;
		}
		return result;
	}
}
