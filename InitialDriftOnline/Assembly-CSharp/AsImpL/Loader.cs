using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BrainFailProductions.PolyFewRuntime;
using UnityEngine;
using UnityEngine.Networking;

namespace AsImpL;

public abstract class Loader : MonoBehaviour
{
	protected struct BuildStats
	{
		public float texturesTime;

		public float materialsTime;

		public float objectsTime;
	}

	protected struct Stats
	{
		public float modelParseTime;

		public float materialsParseTime;

		public float buildTime;

		public BuildStats buildStats;

		public float totalTime;
	}

	public static LoadingProgress totalProgress = new LoadingProgress();

	public ImportOptions buildOptions;

	public PolyfewRuntime.ReferencedNumeric<float> individualProgress = new PolyfewRuntime.ReferencedNumeric<float>(0f);

	protected static float LOAD_PHASE_PERC = 8f;

	protected static float TEXTURE_PHASE_PERC = 1f;

	protected static float MATERIAL_PHASE_PERC = 1f;

	protected static float BUILD_PHASE_PERC = 90f;

	protected static Dictionary<string, GameObject> loadedModels = new Dictionary<string, GameObject>();

	protected static Dictionary<string, int> instanceCount = new Dictionary<string, int>();

	protected DataSet dataSet = new DataSet();

	protected ObjectBuilder objectBuilder = new ObjectBuilder();

	protected List<MaterialData> materialData;

	protected SingleLoadingProgress objLoadingProgress = new SingleLoadingProgress();

	protected Stats loadStats;

	private Texture2D loadedTexture;

	public bool ConvertVertAxis
	{
		get
		{
			if (buildOptions == null)
			{
				return false;
			}
			return buildOptions.zUp;
		}
		set
		{
			if (buildOptions == null)
			{
				buildOptions = new ImportOptions();
			}
			buildOptions.zUp = value;
		}
	}

	public float Scaling
	{
		get
		{
			if (buildOptions == null)
			{
				return 1f;
			}
			return buildOptions.modelScaling;
		}
		set
		{
			if (buildOptions == null)
			{
				buildOptions = new ImportOptions();
			}
			buildOptions.modelScaling = value;
		}
	}

	protected abstract bool HasMaterialLibrary { get; }

	public event Action<GameObject, string> ModelCreated;

	public event Action<GameObject, string> ModelLoaded;

	public event Action<string> ModelError;

	public static GameObject GetModelByPath(string absolutePath)
	{
		if (loadedModels.ContainsKey(absolutePath))
		{
			return loadedModels[absolutePath];
		}
		return null;
	}

	public async Task<GameObject> Load(string objName, string objAbsolutePath, Transform parentObj, string texturesFolderPath = "", string materialsFolderPath = "")
	{
		string fileName = Path.GetFileName(objAbsolutePath);
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(objAbsolutePath);
		string text = objName;
		if (text == null || text == "")
		{
			objName = fileNameWithoutExtension;
		}
		totalProgress.singleProgress.Add(objLoadingProgress);
		objLoadingProgress.fileName = fileName;
		objLoadingProgress.error = false;
		objLoadingProgress.message = "Loading " + fileName + "...";
		await Task.Yield();
		loadedModels[objAbsolutePath] = null;
		instanceCount[objAbsolutePath] = 0;
		float lastTime3 = Time.realtimeSinceStartup;
		float startTime = lastTime3;
		await LoadModelFile(objAbsolutePath, texturesFolderPath, materialsFolderPath);
		loadStats.modelParseTime = Time.realtimeSinceStartup - lastTime3;
		if (objLoadingProgress.error)
		{
			OnLoadFailed(objAbsolutePath);
			return null;
		}
		lastTime3 = Time.realtimeSinceStartup;
		if (HasMaterialLibrary)
		{
			await LoadMaterialLibrary(objAbsolutePath, materialsFolderPath);
		}
		loadStats.materialsParseTime = Time.realtimeSinceStartup - lastTime3;
		lastTime3 = Time.realtimeSinceStartup;
		await Build(objAbsolutePath, objName, parentObj, texturesFolderPath);
		loadStats.buildTime = Time.realtimeSinceStartup - lastTime3;
		loadStats.totalTime = Time.realtimeSinceStartup - startTime;
		totalProgress.singleProgress.Remove(objLoadingProgress);
		OnLoaded(loadedModels[objAbsolutePath], objAbsolutePath);
		return loadedModels[objAbsolutePath];
	}

	public async Task<GameObject> LoadFromNetwork(string objURL, string diffuseTexURL, string bumpTexURL, string specularTexURL, string opacityTexURL, string materialURL, string objName)
	{
		string text = objName + ".obj";
		totalProgress.singleProgress.Add(objLoadingProgress);
		objLoadingProgress.fileName = text;
		objLoadingProgress.error = false;
		objLoadingProgress.message = "Loading " + text + "...";
		await Task.Yield();
		loadedModels[objURL] = null;
		instanceCount[objURL] = 0;
		float lastTime3 = Time.realtimeSinceStartup;
		float startTime = lastTime3;
		try
		{
			await LoadModelFileNetworked(objURL);
		}
		catch (Exception ex)
		{
			throw ex;
		}
		loadStats.modelParseTime = Time.realtimeSinceStartup - lastTime3;
		if (objLoadingProgress.error)
		{
			OnLoadFailed(objURL);
			return null;
		}
		lastTime3 = Time.realtimeSinceStartup;
		if (HasMaterialLibrary)
		{
			try
			{
				await LoadMaterialLibrary(materialURL);
			}
			catch (Exception ex2)
			{
				throw ex2;
			}
		}
		else
		{
			ObjectImporter.activeDownloads--;
		}
		loadStats.materialsParseTime = Time.realtimeSinceStartup - lastTime3;
		lastTime3 = Time.realtimeSinceStartup;
		try
		{
			await NetworkedBuild(null, objName, objURL, diffuseTexURL, bumpTexURL, specularTexURL, opacityTexURL);
		}
		catch (Exception ex3)
		{
			throw ex3;
		}
		loadStats.buildTime = Time.realtimeSinceStartup - lastTime3;
		loadStats.totalTime = Time.realtimeSinceStartup - startTime;
		totalProgress.singleProgress.Remove(objLoadingProgress);
		OnLoaded(loadedModels[objURL], objURL);
		return loadedModels[objURL];
	}

	public IEnumerator LoadFromNetworkWebGL(string objURL, string diffuseTexURL, string bumpTexURL, string specularTexURL, string opacityTexURL, string materialURL, string objName, Action<GameObject> OnSuccess, Action<Exception> OnError)
	{
		string text = objName + ".obj";
		_ = objName;
		totalProgress.singleProgress.Add(objLoadingProgress);
		objLoadingProgress.fileName = text;
		objLoadingProgress.error = false;
		objLoadingProgress.message = "Loading " + text + "...";
		loadedModels[objURL] = null;
		instanceCount[objURL] = 0;
		float lastTime3 = Time.realtimeSinceStartup;
		float startTime = lastTime3;
		yield return StartCoroutine(LoadModelFileNetworkedWebGL(objURL, OnError));
		if (ObjectImporter.isException)
		{
			yield return null;
		}
		loadStats.modelParseTime = Time.realtimeSinceStartup - lastTime3;
		if (objLoadingProgress.error)
		{
			OnLoadFailed(objURL);
			OnError(new Exception("Load failed due to unknown reasons."));
			yield return null;
		}
		lastTime3 = Time.realtimeSinceStartup;
		if (HasMaterialLibrary)
		{
			yield return StartCoroutine(LoadMaterialLibraryWebGL(materialURL));
		}
		else
		{
			ObjectImporter.activeDownloads--;
		}
		if (ObjectImporter.isException)
		{
			yield return null;
		}
		loadStats.materialsParseTime = Time.realtimeSinceStartup - lastTime3;
		lastTime3 = Time.realtimeSinceStartup;
		yield return StartCoroutine(NetworkedBuildWebGL(null, objName, objURL, diffuseTexURL, bumpTexURL, specularTexURL, opacityTexURL));
		if (ObjectImporter.isException)
		{
			yield return null;
		}
		loadStats.buildTime = Time.realtimeSinceStartup - lastTime3;
		loadStats.totalTime = Time.realtimeSinceStartup - startTime;
		totalProgress.singleProgress.Remove(objLoadingProgress);
		OnLoaded(loadedModels[objURL], objURL);
		OnSuccess(loadedModels[objURL]);
	}

	public abstract string[] ParseTexturePaths(string absolutePath);

	protected abstract Task LoadModelFile(string absolutePath, string texturesFolderPath = "", string materialsFolderPath = "");

	protected abstract Task LoadModelFileNetworked(string objURL);

	protected abstract IEnumerator LoadModelFileNetworkedWebGL(string objURL, Action<Exception> OnError);

	protected abstract Task LoadMaterialLibrary(string absolutePath, string materialsFolderPath = "");

	protected abstract Task LoadMaterialLibrary(string materialURL);

	protected abstract IEnumerator LoadMaterialLibraryWebGL(string materialURL);

	protected async Task Build(string absolutePath, string objName, Transform parentTransform, string texturesFolderPath = "")
	{
		float prevTime = Time.realtimeSinceStartup;
		if (materialData != null)
		{
			string basePath = Path.GetDirectoryName(absolutePath);
			objLoadingProgress.message = "Loading textures...";
			int count = 0;
			foreach (MaterialData mtl in materialData)
			{
				objLoadingProgress.percentage = LOAD_PHASE_PERC + TEXTURE_PHASE_PERC * (float)count / (float)materialData.Count;
				count++;
				if (mtl.diffuseTexPath != null)
				{
					await LoadMaterialTexture(basePath, mtl.diffuseTexPath, texturesFolderPath);
					mtl.diffuseTex = loadedTexture;
				}
				if (mtl.bumpTexPath != null)
				{
					await LoadMaterialTexture(basePath, mtl.bumpTexPath, texturesFolderPath);
					mtl.bumpTex = loadedTexture;
				}
				if (mtl.specularTexPath != null)
				{
					await LoadMaterialTexture(basePath, mtl.specularTexPath, texturesFolderPath);
					mtl.specularTex = loadedTexture;
				}
				if (mtl.opacityTexPath != null)
				{
					await LoadMaterialTexture(basePath, mtl.opacityTexPath, texturesFolderPath);
					mtl.opacityTex = loadedTexture;
				}
			}
		}
		loadStats.buildStats.texturesTime = Time.realtimeSinceStartup - prevTime;
		prevTime = Time.realtimeSinceStartup;
		ObjectBuilder.ProgressInfo info = new ObjectBuilder.ProgressInfo();
		objLoadingProgress.message = "Loading materials...";
		objectBuilder.buildOptions = buildOptions;
		bool hasColors = dataSet.colorList.Count > 0;
		bool num = materialData != null;
		objectBuilder.InitBuildMaterials(materialData, hasColors);
		float percentage = objLoadingProgress.percentage;
		if (num)
		{
			while (objectBuilder.BuildMaterials(info))
			{
				objLoadingProgress.percentage = percentage + MATERIAL_PHASE_PERC * (float)objectBuilder.NumImportedMaterials / (float)materialData.Count;
			}
			loadStats.buildStats.materialsTime = Time.realtimeSinceStartup - prevTime;
			prevTime = Time.realtimeSinceStartup;
		}
		objLoadingProgress.message = "Building scene objects...";
		GameObject gameObject = new GameObject(objName);
		if (buildOptions.hideWhileLoading)
		{
			gameObject.SetActive(value: false);
		}
		if (parentTransform != null)
		{
			gameObject.transform.SetParent(parentTransform.transform, worldPositionStays: false);
		}
		OnCreated(gameObject, absolutePath);
		float percentage2 = objLoadingProgress.percentage;
		objectBuilder.StartBuildObjectAsync(dataSet, gameObject);
		while (objectBuilder.BuildObjectAsync(ref info))
		{
			objLoadingProgress.message = "Building scene objects... " + (info.objectsLoaded + info.groupsLoaded) + "/" + (dataSet.objectList.Count + info.numGroups);
			objLoadingProgress.percentage = percentage2 + BUILD_PHASE_PERC * ((float)(info.objectsLoaded / dataSet.objectList.Count) + (float)info.groupsLoaded / (float)info.numGroups);
		}
		objLoadingProgress.percentage = 100f;
		loadedModels[absolutePath] = gameObject;
		loadStats.buildStats.objectsTime = Time.realtimeSinceStartup - prevTime;
	}

	protected async Task NetworkedBuild(Transform parentTransform, string objName, string objURL, string diffuseTexURL, string bumpTexURL, string specularTexURL, string opacityTexURL)
	{
		float prevTime2 = Time.realtimeSinceStartup;
		if (materialData != null)
		{
			objLoadingProgress.message = "Loading textures...";
			int count = 0;
			foreach (MaterialData mtl in materialData)
			{
				objLoadingProgress.percentage = LOAD_PHASE_PERC + TEXTURE_PHASE_PERC * (float)count / (float)materialData.Count;
				count++;
				if (mtl.diffuseTexPath != null)
				{
					if (!string.IsNullOrWhiteSpace(diffuseTexURL))
					{
						try
						{
							await LoadMaterialTexture(diffuseTexURL);
						}
						catch (Exception ex)
						{
							throw ex;
						}
					}
					else
					{
						ObjectImporter.activeDownloads--;
					}
					mtl.diffuseTex = loadedTexture;
				}
				else
				{
					ObjectImporter.activeDownloads--;
				}
				ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
				if (mtl.bumpTexPath != null)
				{
					if (!string.IsNullOrWhiteSpace(bumpTexURL))
					{
						try
						{
							await LoadMaterialTexture(bumpTexURL);
						}
						catch (Exception ex2)
						{
							throw ex2;
						}
					}
					else
					{
						ObjectImporter.activeDownloads--;
					}
					mtl.bumpTex = loadedTexture;
				}
				else
				{
					ObjectImporter.activeDownloads--;
				}
				ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
				if (mtl.specularTexPath != null)
				{
					if (!string.IsNullOrWhiteSpace(specularTexURL))
					{
						try
						{
							await LoadMaterialTexture(specularTexURL);
						}
						catch (Exception ex3)
						{
							throw ex3;
						}
					}
					else
					{
						ObjectImporter.activeDownloads--;
					}
					mtl.specularTex = loadedTexture;
				}
				else
				{
					ObjectImporter.activeDownloads--;
				}
				ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
				if (mtl.opacityTexPath != null)
				{
					if (!string.IsNullOrWhiteSpace(opacityTexURL))
					{
						try
						{
							await LoadMaterialTexture(opacityTexURL);
						}
						catch (Exception ex4)
						{
							throw ex4;
						}
					}
					else
					{
						ObjectImporter.activeDownloads--;
					}
					mtl.opacityTex = loadedTexture;
				}
				else
				{
					ObjectImporter.activeDownloads--;
				}
				ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
			}
		}
		loadStats.buildStats.texturesTime = Time.realtimeSinceStartup - prevTime2;
		prevTime2 = Time.realtimeSinceStartup;
		ObjectBuilder.ProgressInfo info = new ObjectBuilder.ProgressInfo();
		objLoadingProgress.message = "Loading materials...";
		objectBuilder.buildOptions = buildOptions;
		bool hasColors = dataSet.colorList.Count > 0;
		bool num = materialData != null;
		objectBuilder.InitBuildMaterials(materialData, hasColors);
		float objInitPerc = objLoadingProgress.percentage;
		if (num)
		{
			while (objectBuilder.BuildMaterials(info))
			{
				objLoadingProgress.percentage = objInitPerc + MATERIAL_PHASE_PERC * (float)objectBuilder.NumImportedMaterials / (float)materialData.Count;
				await Task.Delay(0);
			}
			loadStats.buildStats.materialsTime = Time.realtimeSinceStartup - prevTime2;
			prevTime2 = Time.realtimeSinceStartup;
		}
		objLoadingProgress.message = "Building scene objects...";
		GameObject newObj = new GameObject(objName);
		if (buildOptions.hideWhileLoading)
		{
			newObj.SetActive(value: false);
		}
		if (parentTransform != null)
		{
			newObj.transform.SetParent(parentTransform.transform, worldPositionStays: false);
		}
		OnCreated(newObj, objURL);
		float initProgress = objLoadingProgress.percentage;
		objectBuilder.StartBuildObjectAsync(dataSet, newObj);
		while (objectBuilder.BuildObjectAsync(ref info))
		{
			objLoadingProgress.message = "Building scene objects... " + (info.objectsLoaded + info.groupsLoaded) + "/" + (dataSet.objectList.Count + info.numGroups);
			objLoadingProgress.percentage = initProgress + BUILD_PHASE_PERC * ((float)(info.objectsLoaded / dataSet.objectList.Count) + (float)info.groupsLoaded / (float)info.numGroups);
			await Task.Delay(0);
		}
		objLoadingProgress.percentage = 100f;
		loadedModels[objURL] = newObj;
		loadStats.buildStats.objectsTime = Time.realtimeSinceStartup - prevTime2;
	}

	protected IEnumerator NetworkedBuildWebGL(Transform parentTransform, string objName, string objURL, string diffuseTexURL, string bumpTexURL, string specularTexURL, string opacityTexURL)
	{
		float prevTime2 = Time.realtimeSinceStartup;
		if (materialData != null)
		{
			objLoadingProgress.message = "Loading textures...";
			int count = 0;
			foreach (MaterialData mtl in materialData)
			{
				objLoadingProgress.percentage = LOAD_PHASE_PERC + TEXTURE_PHASE_PERC * (float)count / (float)materialData.Count;
				count++;
				if (mtl.diffuseTexPath != null)
				{
					if (!string.IsNullOrWhiteSpace(diffuseTexURL))
					{
						yield return StartCoroutine(LoadMaterialTextureWebGL(diffuseTexURL));
					}
					else
					{
						ObjectImporter.activeDownloads--;
					}
					mtl.diffuseTex = loadedTexture;
				}
				else
				{
					ObjectImporter.activeDownloads--;
				}
				ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
				if (mtl.bumpTexPath != null)
				{
					if (!string.IsNullOrWhiteSpace(bumpTexURL))
					{
						yield return StartCoroutine(LoadMaterialTextureWebGL(bumpTexURL));
					}
					else
					{
						ObjectImporter.activeDownloads--;
					}
					mtl.bumpTex = loadedTexture;
				}
				else
				{
					ObjectImporter.activeDownloads--;
				}
				ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
				if (mtl.specularTexPath != null)
				{
					if (!string.IsNullOrWhiteSpace(specularTexURL))
					{
						yield return StartCoroutine(LoadMaterialTextureWebGL(specularTexURL));
					}
					else
					{
						ObjectImporter.activeDownloads--;
					}
					mtl.specularTex = loadedTexture;
				}
				else
				{
					ObjectImporter.activeDownloads--;
				}
				ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
				if (mtl.opacityTexPath != null)
				{
					if (!string.IsNullOrWhiteSpace(opacityTexURL))
					{
						yield return StartCoroutine(LoadMaterialTextureWebGL(opacityTexURL));
					}
					else
					{
						ObjectImporter.activeDownloads--;
					}
					mtl.opacityTex = loadedTexture;
				}
				else
				{
					ObjectImporter.activeDownloads--;
				}
				ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
			}
		}
		loadStats.buildStats.texturesTime = Time.realtimeSinceStartup - prevTime2;
		prevTime2 = Time.realtimeSinceStartup;
		ObjectBuilder.ProgressInfo info = new ObjectBuilder.ProgressInfo();
		objLoadingProgress.message = "Loading materials...";
		objectBuilder.buildOptions = buildOptions;
		bool hasColors = dataSet.colorList.Count > 0;
		bool num = materialData != null;
		objectBuilder.InitBuildMaterials(materialData, hasColors);
		float percentage = objLoadingProgress.percentage;
		if (num)
		{
			while (objectBuilder.BuildMaterials(info))
			{
				objLoadingProgress.percentage = percentage + MATERIAL_PHASE_PERC * (float)objectBuilder.NumImportedMaterials / (float)materialData.Count;
			}
			loadStats.buildStats.materialsTime = Time.realtimeSinceStartup - prevTime2;
			prevTime2 = Time.realtimeSinceStartup;
		}
		objLoadingProgress.message = "Building scene objects...";
		GameObject gameObject = new GameObject(objName);
		if (buildOptions.hideWhileLoading)
		{
			gameObject.SetActive(value: false);
		}
		if (parentTransform != null)
		{
			gameObject.transform.SetParent(parentTransform.transform, worldPositionStays: false);
		}
		OnCreated(gameObject, objURL);
		float percentage2 = objLoadingProgress.percentage;
		objectBuilder.StartBuildObjectAsync(dataSet, gameObject);
		while (objectBuilder.BuildObjectAsync(ref info))
		{
			objLoadingProgress.message = "Building scene objects... " + (info.objectsLoaded + info.groupsLoaded) + "/" + (dataSet.objectList.Count + info.numGroups);
			objLoadingProgress.percentage = percentage2 + BUILD_PHASE_PERC * ((float)(info.objectsLoaded / dataSet.objectList.Count) + (float)info.groupsLoaded / (float)info.numGroups);
		}
		objLoadingProgress.percentage = 100f;
		loadedModels[objURL] = gameObject;
		loadStats.buildStats.objectsTime = Time.realtimeSinceStartup - prevTime2;
	}

	protected string GetDirName(string absolutePath)
	{
		string text;
		if (absolutePath.Contains("//"))
		{
			text = absolutePath.Remove(absolutePath.LastIndexOf('/') + 1);
		}
		else
		{
			string directoryName = Path.GetDirectoryName(absolutePath);
			text = (string.IsNullOrEmpty(directoryName) ? "" : directoryName);
			if (!text.EndsWith("/"))
			{
				text += "/";
			}
		}
		return text;
	}

	protected virtual void OnLoaded(GameObject obj, string absolutePath)
	{
		if (obj == null)
		{
			if (this.ModelError != null)
			{
				this.ModelError(absolutePath);
			}
			return;
		}
		if (buildOptions != null)
		{
			obj.transform.localPosition = buildOptions.localPosition;
			obj.transform.localRotation = Quaternion.Euler(buildOptions.localEulerAngles);
			obj.transform.localScale = buildOptions.localScale;
			if (buildOptions.inheritLayer)
			{
				obj.layer = obj.transform.parent.gameObject.layer;
				MeshRenderer[] componentsInChildren = obj.transform.GetComponentsInChildren<MeshRenderer>(includeInactive: true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].gameObject.layer = obj.transform.parent.gameObject.layer;
				}
			}
		}
		if (buildOptions.hideWhileLoading)
		{
			obj.SetActive(value: true);
		}
		if (this.ModelLoaded != null)
		{
			this.ModelLoaded(obj, absolutePath);
		}
	}

	protected virtual void OnCreated(GameObject obj, string absolutePath)
	{
		if (obj == null)
		{
			if (this.ModelError != null)
			{
				this.ModelError(absolutePath);
			}
		}
		else if (this.ModelCreated != null)
		{
			this.ModelCreated(obj, absolutePath);
		}
	}

	protected virtual void OnLoadFailed(string absolutePath)
	{
		if (this.ModelError != null)
		{
			this.ModelError(absolutePath);
		}
	}

	private string GetTextureUrl(string basePath, string texturePath)
	{
		string text = texturePath.Replace("\\", "/").Replace("//", "/");
		if (!Path.IsPathRooted(text))
		{
			text = basePath + texturePath;
		}
		if (!text.Contains("//"))
		{
			text = "file:///" + text;
		}
		objLoadingProgress.message = "Loading textures...\n" + text;
		return text;
	}

	private async Task LoadMaterialTexture(string basePath, string path, string texturesFolderPath = "")
	{
		loadedTexture = null;
		string path2 = (string.IsNullOrWhiteSpace(texturesFolderPath) ? (basePath + path) : (texturesFolderPath + "\\" + path));
		path2 = Path.GetFullPath(path2);
		if (File.Exists(path2))
		{
			byte[] result;
			using (FileStream stream = File.Open(path2, FileMode.Open))
			{
				result = new byte[stream.Length];
				await stream.ReadAsync(result, 0, (int)stream.Length);
			}
			if (result.Length != 0)
			{
				Texture2D tex = new Texture2D(1, 1);
				tex.LoadImage(result);
				loadedTexture = tex;
			}
		}
		else
		{
			Debug.LogWarning("Failed to load texture at path  " + path2 + "   BasePath  " + basePath + "  path  " + path);
		}
	}

	private async Task LoadMaterialTexture(string textureURL)
	{
		loadedTexture = null;
		bool isWorking = true;
		byte[] downloadedBytes = null;
		float value = individualProgress.Value;
		try
		{
			StartCoroutine(DownloadFile(textureURL, individualProgress, delegate(byte[] bytes)
			{
				isWorking = false;
				downloadedBytes = bytes;
			}, delegate(string error)
			{
				ObjectImporter.activeDownloads--;
				isWorking = false;
				Debug.LogWarning("Failed to load the associated texture file." + error);
			}));
		}
		catch (Exception ex)
		{
			ObjectImporter.activeDownloads--;
			individualProgress.Value = value;
			ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
			isWorking = false;
			throw ex;
		}
		while (isWorking)
		{
			ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
			await Task.Delay(3);
		}
		ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
		if (downloadedBytes != null && downloadedBytes.Length != 0)
		{
			Texture2D tex = new Texture2D(1, 1);
			tex.LoadImage(downloadedBytes);
			loadedTexture = tex;
		}
		else
		{
			Debug.LogWarning("Failed to load texture.");
		}
	}

	private IEnumerator LoadMaterialTextureWebGL(string textureURL)
	{
		loadedTexture = null;
		bool isWorking = true;
		float value = individualProgress.Value;
		try
		{
			StartCoroutine(DownloadTexFileWebGL(textureURL, individualProgress, delegate(Texture2D texture)
			{
				isWorking = false;
				loadedTexture = texture;
			}, delegate(string error)
			{
				ObjectImporter.activeDownloads--;
				isWorking = false;
				Debug.LogWarning("Failed to load the associated texture file." + error);
			}));
		}
		catch (Exception ex)
		{
			ObjectImporter.activeDownloads--;
			individualProgress.Value = value;
			ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
			isWorking = false;
			throw ex;
		}
		while (isWorking)
		{
			yield return new WaitForSeconds(0.1f);
			ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
		}
		ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
		if (loadedTexture == null)
		{
			Debug.LogWarning("Failed to load texture.");
		}
	}

	private Texture2D LoadTexture(UnityWebRequest loader)
	{
		string text = Path.GetExtension(loader.url).ToLower();
		Texture2D texture2D = null;
		switch (text)
		{
		case ".tga":
			texture2D = TextureLoader.LoadTextureFromUrl(loader.url);
			break;
		case ".png":
		case ".jpg":
		case ".jpeg":
			texture2D = DownloadHandlerTexture.GetContent(loader);
			break;
		default:
			Debug.LogWarning("Unsupported texture format: " + text);
			break;
		}
		if (texture2D == null)
		{
			Debug.LogErrorFormat("Failed to load texture {0}", loader.url);
		}
		return texture2D;
	}

	public IEnumerator DownloadFile(string url, PolyfewRuntime.ReferencedNumeric<float> downloadProgress, Action<byte[]> DownloadComplete, Action<string> OnError)
	{
		WWW www = null;
		float oldProgress = downloadProgress.Value;
		try
		{
			www = new WWW(url);
		}
		catch (Exception ex)
		{
			downloadProgress.Value = oldProgress;
			OnError(ex.ToString());
		}
		Coroutine progress = StartCoroutine(GetProgress(www, downloadProgress));
		yield return www;
		if (!string.IsNullOrWhiteSpace(www.error))
		{
			downloadProgress.Value = oldProgress;
			OnError(www.error);
		}
		else if (www.bytes == null || www.bytes.Length == 0)
		{
			if (string.IsNullOrWhiteSpace(www.error))
			{
				downloadProgress.Value = oldProgress;
				OnError("No bytes downloaded. The file might be empty.");
			}
			else
			{
				downloadProgress.Value = oldProgress;
				OnError(www.error);
			}
		}
		else if (string.IsNullOrWhiteSpace(www.error))
		{
			downloadProgress.Value = oldProgress + 1f;
			DownloadComplete(www.bytes);
		}
		else
		{
			downloadProgress.Value = oldProgress;
			OnError(www.error);
		}
		StopCoroutine(progress);
		www.Dispose();
	}

	private IEnumerator GetProgress(WWW www, PolyfewRuntime.ReferencedNumeric<float> downloadProgress)
	{
		float oldProgress = downloadProgress.Value;
		if (www != null && downloadProgress != null)
		{
			while (!www.isDone && string.IsNullOrWhiteSpace(www.error))
			{
				yield return new WaitForSeconds(0.1f);
				downloadProgress.Value = oldProgress + www.progress;
			}
			if (www.isDone && string.IsNullOrWhiteSpace(www.error))
			{
				downloadProgress.Value = oldProgress + www.progress;
				Debug.Log("Progress  " + www.progress);
			}
		}
	}

	public IEnumerator DownloadFileWebGL(string url, PolyfewRuntime.ReferencedNumeric<float> downloadProgress, Action<string> DownloadComplete, Action<string> OnError)
	{
		WWW www = null;
		float oldProgress = downloadProgress.Value;
		try
		{
			www = new WWW(url);
		}
		catch (Exception ex)
		{
			downloadProgress.Value = oldProgress;
			OnError(ex.ToString());
		}
		Coroutine progress = StartCoroutine(GetProgress(www, downloadProgress));
		yield return www;
		if (!string.IsNullOrWhiteSpace(www.error))
		{
			downloadProgress.Value = oldProgress;
			OnError(www.error);
		}
		else if (www.bytes == null || www.bytes.Length == 0)
		{
			if (string.IsNullOrWhiteSpace(www.error))
			{
				downloadProgress.Value = oldProgress;
				OnError("No bytes downloaded. The file might be empty.");
			}
			else
			{
				downloadProgress.Value = oldProgress;
				OnError(www.error);
			}
		}
		else if (string.IsNullOrWhiteSpace(www.error))
		{
			downloadProgress.Value = oldProgress + 1f;
			DownloadComplete(www.text);
		}
		else
		{
			downloadProgress.Value = oldProgress;
			OnError(www.error);
		}
		try
		{
			StopCoroutine(progress);
		}
		catch (Exception)
		{
		}
		www.Dispose();
	}

	public IEnumerator DownloadTexFileWebGL(string url, PolyfewRuntime.ReferencedNumeric<float> downloadProgress, Action<Texture2D> DownloadComplete, Action<string> OnError)
	{
		WWW www = null;
		float oldProgress = downloadProgress.Value;
		try
		{
			www = new WWW(url);
		}
		catch (Exception ex)
		{
			downloadProgress.Value = oldProgress;
			OnError(ex.ToString());
		}
		Coroutine progress = StartCoroutine(GetProgress(www, downloadProgress));
		yield return www;
		if (!string.IsNullOrWhiteSpace(www.error))
		{
			downloadProgress.Value = oldProgress;
			OnError(www.error);
		}
		else if (www.bytes == null || www.bytes.Length == 0)
		{
			if (string.IsNullOrWhiteSpace(www.error))
			{
				downloadProgress.Value = oldProgress;
				OnError("No bytes downloaded. The file might be empty.");
			}
			else
			{
				downloadProgress.Value = oldProgress;
				OnError(www.error);
			}
		}
		else if (string.IsNullOrWhiteSpace(www.error))
		{
			downloadProgress.Value = oldProgress + 1f;
			DownloadComplete(www.texture);
		}
		else
		{
			downloadProgress.Value = oldProgress;
			OnError(www.error);
		}
		StopCoroutine(progress);
		www.Dispose();
	}
}
