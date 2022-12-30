using System.Collections.Generic;
using UnityEngine;

namespace AsImpL;

public class MultiObjectImporter : ObjectImporter
{
	[Tooltip("Load models in the list on start")]
	public bool autoLoadOnStart;

	[Tooltip("Models to load on startup")]
	public List<ModelImportInfo> objectsList = new List<ModelImportInfo>();

	[Tooltip("Default import options")]
	public ImportOptions defaultImportOptions = new ImportOptions();

	[SerializeField]
	private PathSettings pathSettings;

	public string RootPath
	{
		get
		{
			if (!(pathSettings != null))
			{
				return "";
			}
			return pathSettings.RootPath;
		}
	}

	public void ImportModelListAsync(ModelImportInfo[] modelsInfo)
	{
		if (modelsInfo == null)
		{
			return;
		}
		for (int i = 0; i < modelsInfo.Length; i++)
		{
			if (modelsInfo[i].skip)
			{
				continue;
			}
			string objName = modelsInfo[i].name;
			string path = modelsInfo[i].path;
			if (string.IsNullOrEmpty(path))
			{
				Debug.LogErrorFormat("File path missing for the model at position {0} in the list.", i);
				continue;
			}
			path = RootPath + path;
			ImportOptions loaderOptions = modelsInfo[i].loaderOptions;
			if (loaderOptions == null || loaderOptions.modelScaling == 0f)
			{
				loaderOptions = defaultImportOptions;
			}
			ImportModelAsync(objName, path, base.transform, loaderOptions);
		}
	}

	protected virtual void Start()
	{
		if (autoLoadOnStart)
		{
			ImportModelListAsync(objectsList.ToArray());
		}
	}
}
