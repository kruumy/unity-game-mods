using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace AsImpL;

public class LoaderObj : Loader
{
	private class BumpParamDef
	{
		public string optionName;

		public string valueType;

		public int valueNumMin;

		public int valueNumMax;

		public BumpParamDef(string name, string type, int numMin, int numMax)
		{
			optionName = name;
			valueType = type;
			valueNumMin = numMin;
			valueNumMax = numMax;
		}
	}

	private string mtlLib;

	private string loadedText;

	protected override bool HasMaterialLibrary => mtlLib != null;

	public override string[] ParseTexturePaths(string absolutePath)
	{
		List<string> list = new List<string>();
		string dirName = GetDirName(absolutePath);
		string text = ParseMaterialLibName(absolutePath);
		if (!string.IsNullOrEmpty(text))
		{
			string[] lines = File.ReadAllLines(dirName + text);
			List<MaterialData> list2 = new List<MaterialData>();
			ParseMaterialData(lines, list2);
			foreach (MaterialData item in list2)
			{
				if (!string.IsNullOrEmpty(item.diffuseTexPath))
				{
					list.Add(item.diffuseTexPath);
				}
				if (!string.IsNullOrEmpty(item.specularTexPath))
				{
					list.Add(item.specularTexPath);
				}
				if (!string.IsNullOrEmpty(item.bumpTexPath))
				{
					list.Add(item.bumpTexPath);
				}
				if (!string.IsNullOrEmpty(item.opacityTexPath))
				{
					list.Add(item.opacityTexPath);
				}
			}
		}
		return list.ToArray();
	}

	protected override async Task LoadModelFile(string absolutePath, string texturesFolderPath = "", string materialsFolderPath = "")
	{
		if (!absolutePath.Contains("//"))
		{
			_ = "file:///" + absolutePath;
		}
		using (StreamReader sr = new StreamReader(absolutePath))
		{
			loadedText = await sr.ReadToEndAsync();
		}
		if (string.IsNullOrEmpty(loadedText))
		{
			Loader.totalProgress.singleProgress.Remove(objLoadingProgress);
			throw new InvalidOperationException("Failed to load data from file. The file might be empty or non readable.");
		}
		ParseGeometryData(loadedText);
	}

	protected override async Task LoadModelFileNetworked(string objURL)
	{
		bool isWorking = true;
		byte[] downloadedBytes = null;
		Exception ex = null;
		float value = individualProgress.Value;
		try
		{
			StartCoroutine(DownloadFile(objURL, individualProgress, delegate(byte[] bytes)
			{
				isWorking = false;
				downloadedBytes = bytes;
			}, delegate(string error)
			{
				ObjectImporter.activeDownloads--;
				ex = new InvalidOperationException("Failed to download base model." + error);
				isWorking = false;
			}));
		}
		catch (Exception ex2)
		{
			ObjectImporter.activeDownloads--;
			individualProgress.Value = value;
			ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
			isWorking = false;
			throw ex2;
		}
		while (isWorking)
		{
			ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
			await Task.Delay(1);
		}
		if (ex != null)
		{
			throw ex;
		}
		ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
		if (downloadedBytes != null && downloadedBytes.Length != 0)
		{
			using StreamReader sr = new StreamReader(new MemoryStream(downloadedBytes));
			loadedText = await sr.ReadToEndAsync();
		}
		if (string.IsNullOrEmpty(loadedText))
		{
			Loader.totalProgress.singleProgress.Remove(objLoadingProgress);
			throw new InvalidOperationException("Failed to load data from the downloaded obj file. The file might be empty or non readable.");
		}
		try
		{
			ParseGeometryData(loadedText);
		}
		catch (Exception ex3)
		{
			throw ex3;
		}
	}

	protected override IEnumerator LoadModelFileNetworkedWebGL(string objURL, Action<Exception> OnError)
	{
		bool isWorking = true;
		Exception ex = null;
		float value = individualProgress.Value;
		try
		{
			StartCoroutine(DownloadFileWebGL(objURL, individualProgress, delegate(string text)
			{
				isWorking = false;
				loadedText = text;
			}, delegate(string error)
			{
				ObjectImporter.activeDownloads--;
				ex = new InvalidOperationException("Base model download unsuccessful." + error);
				ObjectImporter.isException = true;
				OnError(ex);
				isWorking = false;
			}));
		}
		catch (Exception obj)
		{
			ObjectImporter.activeDownloads--;
			individualProgress.Value = value;
			ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
			isWorking = false;
			isWorking = false;
			OnError(obj);
			ObjectImporter.isException = true;
		}
		while (isWorking)
		{
			yield return new WaitForSeconds(0.1f);
			ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
		}
		if (ObjectImporter.isException)
		{
			yield return null;
		}
		ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
		if (string.IsNullOrEmpty(loadedText))
		{
			Loader.totalProgress.singleProgress.Remove(objLoadingProgress);
			throw new InvalidOperationException("Failed to load data from the downloaded obj file. The file might be empty or non readable.");
		}
		try
		{
			ParseGeometryData(loadedText);
		}
		catch (Exception obj2)
		{
			OnError(obj2);
			ObjectImporter.isException = true;
		}
	}

	protected override async Task LoadMaterialLibrary(string absolutePath, string materialsFolderPath = "")
	{
		string dirName = GetDirName(absolutePath);
		if (absolutePath.Contains("//"))
		{
			int num = ((!absolutePath.Contains("?")) ? absolutePath.LastIndexOf('/') : absolutePath.LastIndexOf('='));
			_ = absolutePath.Remove(num + 1) + mtlLib;
		}
		else if (Path.IsPathRooted(mtlLib))
		{
			_ = "file:///" + mtlLib;
		}
		else
		{
			_ = "file:///" + dirName + mtlLib;
		}
		string path = (string.IsNullOrWhiteSpace(materialsFolderPath) ? (dirName + mtlLib) : (materialsFolderPath + mtlLib));
		if (File.Exists(path))
		{
			using StreamReader sr = new StreamReader(path);
			loadedText = await sr.ReadToEndAsync();
		}
		else
		{
			Debug.LogWarning("Cannot find the associated material file at the path   " + dirName + mtlLib);
		}
		if (!string.IsNullOrWhiteSpace(loadedText))
		{
			objLoadingProgress.message = "Parsing material library...";
			ParseMaterialData(loadedText);
		}
	}

	protected override async Task LoadMaterialLibrary(string materialURL)
	{
		bool isWorking = true;
		byte[] downloadedBytes = null;
		float value = individualProgress.Value;
		try
		{
			StartCoroutine(DownloadFile(materialURL, individualProgress, delegate(byte[] bytes)
			{
				isWorking = false;
				downloadedBytes = bytes;
			}, delegate(string error)
			{
				ObjectImporter.activeDownloads--;
				isWorking = false;
				Debug.LogWarning("Failed to load the associated material file." + error);
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
			using StreamReader sr = new StreamReader(new MemoryStream(downloadedBytes));
			loadedText = await sr.ReadToEndAsync();
		}
		if (!string.IsNullOrWhiteSpace(loadedText))
		{
			objLoadingProgress.message = "Parsing material library...";
			ParseMaterialData(loadedText);
		}
	}

	protected override IEnumerator LoadMaterialLibraryWebGL(string materialURL)
	{
		bool isWorking = true;
		_ = individualProgress.Value;
		StartCoroutine(DownloadFileWebGL(materialURL, individualProgress, delegate(string text)
		{
			isWorking = false;
			loadedText = text;
		}, delegate(string error)
		{
			ObjectImporter.activeDownloads--;
			isWorking = false;
			Debug.LogWarning("Failed to load the associated material file." + error);
		}));
		while (isWorking)
		{
			yield return new WaitForSeconds(0.1f);
			ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
		}
		ObjectImporter.downloadProgress.Value = individualProgress.Value / (float)ObjectImporter.activeDownloads * 100f;
		if (!string.IsNullOrWhiteSpace(loadedText))
		{
			objLoadingProgress.message = "Parsing material library...";
			ParseMaterialData(loadedText);
		}
	}

	private void GetFaceIndicesByOneFaceLine(DataSet.FaceIndices[] faces, string[] p, bool isFaceIndexPlus)
	{
		if (isFaceIndexPlus)
		{
			for (int i = 1; i < p.Length; i++)
			{
				string[] array = p[i].Trim().Split("/".ToCharArray());
				DataSet.FaceIndices faceIndices = default(DataSet.FaceIndices);
				int num = int.Parse(array[0]);
				faceIndices.vertIdx = num - 1;
				if (array.Length > 1 && array[1] != "")
				{
					int num2 = int.Parse(array[1]);
					faceIndices.uvIdx = num2 - 1;
				}
				if (array.Length > 2 && array[2] != "")
				{
					int num3 = int.Parse(array[2]);
					faceIndices.normIdx = num3 - 1;
				}
				else
				{
					faceIndices.normIdx = -1;
				}
				faces[i - 1] = faceIndices;
			}
			return;
		}
		int count = dataSet.vertList.Count;
		int count2 = dataSet.uvList.Count;
		for (int j = 1; j < p.Length; j++)
		{
			string[] array2 = p[j].Trim().Split("/".ToCharArray());
			DataSet.FaceIndices faceIndices2 = default(DataSet.FaceIndices);
			int num4 = int.Parse(array2[0]);
			faceIndices2.vertIdx = count + num4;
			if (array2.Length > 1 && array2[1] != "")
			{
				int num5 = int.Parse(array2[1]);
				faceIndices2.uvIdx = count2 + num5;
			}
			if (array2.Length > 2 && array2[2] != "")
			{
				int num6 = int.Parse(array2[2]);
				faceIndices2.normIdx = count + num6;
			}
			else
			{
				faceIndices2.normIdx = -1;
			}
			faces[j - 1] = faceIndices2;
		}
	}

	private Vector3 ConvertVec3(float x, float y, float z)
	{
		if (base.Scaling != 1f)
		{
			x *= base.Scaling;
			y *= base.Scaling;
			z *= base.Scaling;
		}
		if (base.ConvertVertAxis)
		{
			return new Vector3(x, z, y);
		}
		return new Vector3(x, y, 0f - z);
	}

	private float ParseFloat(string floatString)
	{
		return float.Parse(floatString, CultureInfo.InvariantCulture.NumberFormat);
	}

	protected void ParseGeometryData(string objDataText)
	{
		string[] array = objDataText.Split("\n".ToCharArray());
		bool flag = true;
		bool isFaceIndexPlus = true;
		objLoadingProgress.message = "Parsing geometry data...";
		char[] separator = new char[2] { ' ', '\t' };
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].Trim();
			if (text.Length > 0 && text[0] == '#')
			{
				continue;
			}
			string[] array2 = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			if (array2.Length == 0)
			{
				continue;
			}
			string text2 = null;
			if (text.Length > array2[0].Length)
			{
				text2 = text.Substring(array2[0].Length + 1).Trim();
			}
			switch (array2[0])
			{
			case "o":
				dataSet.AddObject(text2);
				flag = true;
				break;
			case "g":
				flag = true;
				dataSet.AddGroup(text2);
				break;
			case "v":
				dataSet.AddVertex(ConvertVec3(ParseFloat(array2[1]), ParseFloat(array2[2]), ParseFloat(array2[3])));
				if (array2.Length >= 7)
				{
					dataSet.AddColor(new Color(ParseFloat(array2[4]), ParseFloat(array2[5]), ParseFloat(array2[6]), 1f));
				}
				break;
			case "vt":
				dataSet.AddUV(new Vector2(ParseFloat(array2[1]), ParseFloat(array2[2])));
				break;
			case "vn":
				dataSet.AddNormal(ConvertVec3(ParseFloat(array2[1]), ParseFloat(array2[2]), ParseFloat(array2[3])));
				break;
			case "f":
			{
				int num = array2.Length - 1;
				DataSet.FaceIndices[] array3 = new DataSet.FaceIndices[num];
				if (flag)
				{
					flag = false;
					isFaceIndexPlus = int.Parse(array2[1].Trim().Split("/".ToCharArray())[0]) >= 0;
				}
				GetFaceIndicesByOneFaceLine(array3, array2, isFaceIndexPlus);
				if (num == 3)
				{
					dataSet.AddFaceIndices(array3[0]);
					dataSet.AddFaceIndices(array3[2]);
					dataSet.AddFaceIndices(array3[1]);
				}
				else
				{
					Triangulator.Triangulate(dataSet, array3);
				}
				break;
			}
			case "mtllib":
				if (!string.IsNullOrEmpty(text2))
				{
					mtlLib = text2;
				}
				break;
			case "usemtl":
				if (!string.IsNullOrEmpty(text2))
				{
					dataSet.AddMaterialName(DataSet.FixMaterialName(text2));
				}
				break;
			}
			if (i % 7000 == 0)
			{
				objLoadingProgress.percentage = Loader.LOAD_PHASE_PERC * (float)i / (float)array.Length;
			}
		}
		objLoadingProgress.percentage = Loader.LOAD_PHASE_PERC;
	}

	private string ParseMaterialLibName(string path)
	{
		string[] array = File.ReadAllLines(path);
		objLoadingProgress.message = "Parsing geometry data...";
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].Trim();
			if (text.StartsWith("mtllib"))
			{
				return text.Substring("mtllib".Length).Trim();
			}
		}
		return null;
	}

	private void ParseMaterialData(string data)
	{
		objLoadingProgress.message = "Parsing material data...";
		string[] lines = data.Split("\n".ToCharArray());
		materialData = new List<MaterialData>();
		ParseMaterialData(lines, materialData);
	}

	private void ParseMaterialData(string[] lines, List<MaterialData> mtlData)
	{
		MaterialData materialData = new MaterialData();
		char[] separator = new char[2] { ' ', '\t' };
		for (int i = 0; i < lines.Length; i++)
		{
			string text = lines[i].Trim();
			if (text.IndexOf("#") != -1)
			{
				text = text.Substring(0, text.IndexOf("#"));
			}
			string[] array = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 0 || string.IsNullOrEmpty(array[0]))
			{
				continue;
			}
			string text2 = null;
			if (text.Length > array[0].Length)
			{
				text2 = text.Substring(array[0].Length + 1).Trim();
			}
			try
			{
				switch (array[0])
				{
				case "newmtl":
					materialData = new MaterialData();
					materialData.materialName = DataSet.FixMaterialName(text2);
					mtlData.Add(materialData);
					break;
				case "Ka":
					materialData.ambientColor = StringsToColor(array);
					break;
				case "Kd":
					materialData.diffuseColor = StringsToColor(array);
					break;
				case "Ks":
					materialData.specularColor = StringsToColor(array);
					break;
				case "Ke":
					materialData.emissiveColor = StringsToColor(array);
					break;
				case "Ns":
					materialData.shininess = ParseFloat(array[1]);
					break;
				case "d":
					materialData.overallAlpha = ((array.Length > 1 && array[1] != "") ? ParseFloat(array[1]) : 1f);
					break;
				case "Tr":
					materialData.overallAlpha = ((array.Length > 1 && array[1] != "") ? (1f - ParseFloat(array[1])) : 1f);
					break;
				case "map_KD":
				case "map_Kd":
					if (!string.IsNullOrEmpty(text2))
					{
						materialData.diffuseTexPath = text2;
					}
					break;
				case "map_Ks":
				case "map_kS":
				case "map_Ns":
					if (!string.IsNullOrEmpty(text2))
					{
						materialData.specularTexPath = text2;
					}
					break;
				case "map_bump":
					if (!string.IsNullOrEmpty(text2))
					{
						materialData.bumpTexPath = text2;
					}
					break;
				case "bump":
					ParseBumpParameters(array, materialData);
					break;
				case "map_opacity":
				case "map_d":
					if (!string.IsNullOrEmpty(text2))
					{
						materialData.opacityTexPath = text2;
					}
					break;
				case "illum":
					materialData.illumType = int.Parse(array[1]);
					break;
				case "refl":
					if (!string.IsNullOrEmpty(text2))
					{
						materialData.hasReflectionTex = true;
					}
					break;
				case "map_Ka":
				case "map_kA":
					if (!string.IsNullOrEmpty(text2))
					{
						Debug.Log("Map not supported:" + text);
					}
					break;
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Error at line {0} in mtl file: {1}", i + 1, ex);
			}
		}
	}

	private void ParseBumpParameters(string[] param, MaterialData mtlData)
	{
		Regex regex = new Regex("^[-+]?[0-9]*\\.?[0-9]+$");
		Dictionary<string, BumpParamDef> dictionary = new Dictionary<string, BumpParamDef>();
		dictionary.Add("bm", new BumpParamDef("bm", "string", 1, 1));
		dictionary.Add("clamp", new BumpParamDef("clamp", "string", 1, 1));
		dictionary.Add("blendu", new BumpParamDef("blendu", "string", 1, 1));
		dictionary.Add("blendv", new BumpParamDef("blendv", "string", 1, 1));
		dictionary.Add("imfchan", new BumpParamDef("imfchan", "string", 1, 1));
		dictionary.Add("mm", new BumpParamDef("mm", "string", 1, 1));
		dictionary.Add("o", new BumpParamDef("o", "number", 1, 3));
		dictionary.Add("s", new BumpParamDef("s", "number", 1, 3));
		dictionary.Add("t", new BumpParamDef("t", "number", 1, 3));
		dictionary.Add("texres", new BumpParamDef("texres", "string", 1, 1));
		int num = 1;
		string text = null;
		while (num < param.Length)
		{
			if (!param[num].StartsWith("-"))
			{
				text = param[num];
				num++;
				continue;
			}
			string text2 = param[num].Substring(1);
			num++;
			if (!dictionary.ContainsKey(text2))
			{
				continue;
			}
			BumpParamDef bumpParamDef = dictionary[text2];
			ArrayList arrayList = new ArrayList();
			int num2 = 0;
			bool flag = false;
			while (num2 < bumpParamDef.valueNumMin)
			{
				if (num >= param.Length)
				{
					flag = true;
					break;
				}
				if (bumpParamDef.valueType == "number" && !regex.Match(param[num]).Success)
				{
					flag = true;
					break;
				}
				arrayList.Add(param[num]);
				num2++;
				num++;
			}
			if (flag)
			{
				Debug.Log("bump variable value not enough for option:" + text2 + " of material:" + mtlData.materialName);
				continue;
			}
			while (num2 < bumpParamDef.valueNumMax && num < param.Length && (!(bumpParamDef.valueType == "number") || regex.Match(param[num]).Success))
			{
				arrayList.Add(param[num]);
				num2++;
				num++;
			}
			Debug.Log(string.Concat("found option: ", text2, " of material: ", mtlData.materialName, " args: ", string.Concat(arrayList.ToArray())));
		}
		if (text != null)
		{
			mtlData.bumpTexPath = text;
		}
	}

	private Color StringsToColor(string[] p)
	{
		return new Color(ParseFloat(p[1]), ParseFloat(p[2]), ParseFloat(p[3]));
	}

	private IEnumerator LoadOrDownloadText(string url, bool notifyErrors = true)
	{
		loadedText = null;
		UnityWebRequest uwr = UnityWebRequest.Get(url);
		yield return uwr.SendWebRequest();
		if (uwr.isNetworkError || uwr.isHttpError)
		{
			if (notifyErrors)
			{
				Debug.LogError(uwr.error);
			}
		}
		else
		{
			loadedText = uwr.downloadHandler.text;
		}
	}
}
