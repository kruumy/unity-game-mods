using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BrainFailProductions.PolyFewRuntime;
using UnityEngine;

namespace AsImpL;

public class ObjectImporter : MonoBehaviour
{
	private enum ImportPhase
	{
		Idle,
		TextureImport,
		ObjLoad,
		AssetBuild,
		Done
	}

	public static PolyfewRuntime.ReferencedNumeric<float> downloadProgress;

	public static int activeDownloads;

	private static float objDownloadProgress;

	private static float textureDownloadProgress;

	private static float materialDownloadProgress;

	public static bool isException;

	protected int numTotalImports;

	protected bool allLoaded;

	protected ImportOptions buildOptions;

	protected List<Loader> loaderList;

	private ImportPhase importPhase;

	public int NumImportRequests => numTotalImports;

	public event Action ImportingStart;

	public event Action ImportingComplete;

	public event Action<GameObject, string> CreatedModel;

	public event Action<GameObject, string> ImportedModel;

	public event Action<string> ImportError;

	public ObjectImporter()
	{
		isException = false;
		downloadProgress = new PolyfewRuntime.ReferencedNumeric<float>(0f);
		objDownloadProgress = 0f;
		textureDownloadProgress = 0f;
		materialDownloadProgress = 0f;
		activeDownloads = 6;
	}

	private Loader CreateLoader(string absolutePath, bool isNetwork = false)
	{
		if (isNetwork)
		{
			LoaderObj loaderObj = base.gameObject.AddComponent<LoaderObj>();
			loaderObj.ModelCreated += OnModelCreated;
			loaderObj.ModelLoaded += OnImported;
			loaderObj.ModelError += OnImportError;
			return loaderObj;
		}
		string extension = Path.GetExtension(absolutePath);
		if (string.IsNullOrEmpty(extension))
		{
			throw new InvalidOperationException("No extension defined, unable to detect file format. Please provide a full path to the file that ends with the file name including its extension.");
		}
		Loader loader = null;
		extension = extension.ToLower();
		if (extension.StartsWith(".php"))
		{
			if (!extension.EndsWith(".obj"))
			{
				throw new InvalidOperationException("Unable to detect file format in " + extension);
			}
			loader = base.gameObject.AddComponent<LoaderObj>();
		}
		else
		{
			if (!(extension == ".obj"))
			{
				throw new InvalidOperationException("File format not supported (" + extension + ")");
			}
			loader = base.gameObject.AddComponent<LoaderObj>();
		}
		loader.ModelCreated += OnModelCreated;
		loader.ModelLoaded += OnImported;
		loader.ModelError += OnImportError;
		return loader;
	}

	public async Task<GameObject> ImportModelAsync(string objName, string filePath, Transform parentObj, ImportOptions options, string texturesFolderPath = "", string materialsFolderPath = "")
	{
		if (loaderList == null)
		{
			loaderList = new List<Loader>();
		}
		if (loaderList.Count == 0)
		{
			numTotalImports = 0;
			this.ImportingStart?.Invoke();
		}
		string text = (filePath.Contains("//") ? filePath : Path.GetFullPath(filePath));
		text = text.Replace('\\', '/');
		Loader loader = CreateLoader(text);
		if (loader == null)
		{
			throw new SystemException("Failed to import obj.");
		}
		numTotalImports++;
		loaderList.Add(loader);
		loader.buildOptions = options;
		if (string.IsNullOrEmpty(objName))
		{
			objName = Path.GetFileNameWithoutExtension(text);
		}
		allLoaded = false;
		return await loader.Load(objName, text, parentObj, texturesFolderPath, materialsFolderPath);
	}

	public async Task<GameObject> ImportModelFromNetwork(string objURL, string objName, string diffuseTexURL, string bumpTexURL, string specularTexURL, string opacityTexURL, string materialURL, PolyfewRuntime.ReferencedNumeric<float> downloadProgress, ImportOptions options)
	{
		if (loaderList == null)
		{
			loaderList = new List<Loader>();
		}
		if (loaderList.Count == 0)
		{
			numTotalImports = 0;
			this.ImportingStart?.Invoke();
		}
		Loader loader = CreateLoader("", isNetwork: true);
		if (loader == null)
		{
			throw new SystemException("Failed to import obj.");
		}
		numTotalImports++;
		loaderList.Add(loader);
		loader.buildOptions = options;
		allLoaded = false;
		if (string.IsNullOrWhiteSpace(objName))
		{
			objName = "";
		}
		ObjectImporter.downloadProgress = downloadProgress;
		try
		{
			return await loader.LoadFromNetwork(objURL, diffuseTexURL, bumpTexURL, specularTexURL, opacityTexURL, materialURL, objName);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public void ImportModelFromNetworkWebGL(string objURL, string objName, string diffuseTexURL, string bumpTexURL, string specularTexURL, string opacityTexURL, string materialURL, PolyfewRuntime.ReferencedNumeric<float> downloadProgress, ImportOptions options, Action<GameObject> OnSuccess, Action<Exception> OnError)
	{
		if (loaderList == null)
		{
			loaderList = new List<Loader>();
		}
		if (loaderList.Count == 0)
		{
			numTotalImports = 0;
			this.ImportingStart?.Invoke();
		}
		Loader loader = CreateLoader("", isNetwork: true);
		if (loader == null)
		{
			OnError(new SystemException("Loader initialization failed due to unknown reasons."));
		}
		numTotalImports++;
		loaderList.Add(loader);
		loader.buildOptions = options;
		allLoaded = false;
		if (string.IsNullOrWhiteSpace(objName))
		{
			objName = "";
		}
		ObjectImporter.downloadProgress = downloadProgress;
		StartCoroutine(loader.LoadFromNetworkWebGL(objURL, diffuseTexURL, bumpTexURL, specularTexURL, opacityTexURL, materialURL, objName, OnSuccess, OnError));
	}

	public virtual void UpdateStatus()
	{
		if (allLoaded || numTotalImports - Loader.totalProgress.singleProgress.Count < numTotalImports)
		{
			return;
		}
		allLoaded = true;
		if (loaderList != null)
		{
			foreach (Loader loader in loaderList)
			{
				UnityEngine.Object.Destroy(loader);
			}
			loaderList.Clear();
		}
		OnImportingComplete();
	}

	protected virtual void Update()
	{
		UpdateStatus();
	}

	protected virtual void OnImportingComplete()
	{
		if (this.ImportingComplete != null)
		{
			this.ImportingComplete();
		}
	}

	protected virtual void OnModelCreated(GameObject obj, string absolutePath)
	{
		if (this.CreatedModel != null)
		{
			this.CreatedModel(obj, absolutePath);
		}
	}

	protected virtual void OnImported(GameObject obj, string absolutePath)
	{
		if (this.ImportedModel != null)
		{
			this.ImportedModel(obj, absolutePath);
		}
	}

	protected virtual void OnImportError(string absolutePath)
	{
		if (this.ImportError != null)
		{
			this.ImportError(absolutePath);
		}
	}
}
