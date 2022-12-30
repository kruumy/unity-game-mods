using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AsImpL;
using UnityEngine;

namespace BrainFailProductions.PolyFewRuntime;

public class UtilityServicesRuntime : MonoBehaviour
{
	public class OBJExporterImporter
	{
		private bool applyPosition = true;

		private bool applyRotation = true;

		private bool applyScale = true;

		private bool generateMaterials = true;

		private bool exportTextures = true;

		private string exportPath;

		private MeshFilter meshFilter;

		private Mesh meshToExport;

		private MeshRenderer meshRenderer;

		private void InitializeExporter(GameObject toExport, string exportPath, PolyfewRuntime.OBJExportOptions exportOptions)
		{
			this.exportPath = exportPath;
			if (string.IsNullOrWhiteSpace(exportPath))
			{
				throw new DirectoryNotFoundException("The path provided is non-existant.");
			}
			exportPath = Path.GetFullPath(exportPath);
			if (exportPath[exportPath.Length - 1] == '\\')
			{
				exportPath = exportPath.Remove(exportPath.Length - 1);
			}
			else if (exportPath[exportPath.Length - 1] == '/')
			{
				exportPath = exportPath.Remove(exportPath.Length - 1);
			}
			if (!Directory.Exists(exportPath))
			{
				throw new DirectoryNotFoundException("The path provided is non-existant.");
			}
			if (toExport == null)
			{
				throw new ArgumentNullException("toExport", "Please provide a GameObject to export as OBJ file.");
			}
			meshRenderer = toExport.GetComponent<MeshRenderer>();
			meshFilter = toExport.GetComponent<MeshFilter>();
			if (!(meshRenderer == null) && meshRenderer.isPartOfStaticBatch)
			{
				throw new InvalidOperationException("The provided object is static batched. Static batched object cannot be exported. Please disable it before trying to export the object.");
			}
			if (meshFilter == null)
			{
				throw new InvalidOperationException("There is no MeshFilter attached to the provided GameObject.");
			}
			meshToExport = meshFilter.sharedMesh;
			if (meshToExport == null || meshToExport.triangles == null || meshToExport.triangles.Length == 0)
			{
				throw new InvalidOperationException("The MeshFilter on the provided GameObject has invalid or no mesh at all.");
			}
			if (exportOptions != null)
			{
				applyPosition = exportOptions.applyPosition;
				applyRotation = exportOptions.applyRotation;
				applyScale = exportOptions.applyScale;
				generateMaterials = exportOptions.generateMaterials;
				exportTextures = exportOptions.exportTextures;
			}
		}

		private void InitializeExporter(Mesh toExport, string exportPath)
		{
			this.exportPath = exportPath;
			if (string.IsNullOrWhiteSpace(exportPath))
			{
				throw new DirectoryNotFoundException("The path provided is non-existant.");
			}
			if (!Directory.Exists(exportPath))
			{
				throw new DirectoryNotFoundException("The path provided is non-existant.");
			}
			if (toExport == null)
			{
				throw new ArgumentNullException("toExport", "Please provide a Mesh to export as OBJ file.");
			}
			meshToExport = toExport;
			if (meshToExport == null || meshToExport.triangles == null || meshToExport.triangles.Length == 0)
			{
				throw new InvalidOperationException("The MeshFilter on the provided GameObject has invalid or no mesh at all.");
			}
		}

		private Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, Quaternion angle)
		{
			return angle * (point - pivot) + pivot;
		}

		private Vector3 MultiplyVec3s(Vector3 v1, Vector3 v2)
		{
			return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
		}

		public void ExportGameObjectToOBJ(GameObject toExport, string exportPath, PolyfewRuntime.OBJExportOptions exportOptions = null, Action OnSuccess = null)
		{
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				Debug.LogWarning("The function cannot run on WebGL player. As web apps cannot read from or write to local file system.");
				return;
			}
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			InitializeExporter(toExport, exportPath, exportOptions);
			string name = toExport.gameObject.name;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			if (generateMaterials)
			{
				stringBuilder.AppendLine("mtllib " + name + ".mtl");
			}
			int num = 0;
			if (meshRenderer != null && generateMaterials)
			{
				Material[] sharedMaterials = meshRenderer.sharedMaterials;
				foreach (Material material in sharedMaterials)
				{
					if (!dictionary.ContainsKey(material.name))
					{
						dictionary[material.name] = true;
						stringBuilder2.Append(MaterialToString(material));
						stringBuilder2.AppendLine();
					}
				}
			}
			int num2 = (int)Mathf.Clamp(toExport.gameObject.transform.lossyScale.x * toExport.gameObject.transform.lossyScale.z, -1f, 1f);
			Vector3[] vertices = meshToExport.vertices;
			for (int j = 0; j < vertices.Length; j++)
			{
				Vector3 vector = vertices[j];
				if (applyScale)
				{
					vector = MultiplyVec3s(vector, toExport.gameObject.transform.lossyScale);
				}
				if (applyRotation)
				{
					vector = RotateAroundPoint(vector, Vector3.zero, toExport.gameObject.transform.rotation);
				}
				if (applyPosition)
				{
					vector += toExport.gameObject.transform.position;
				}
				vector.x *= -1f;
				stringBuilder.AppendLine("v " + vector.x + " " + vector.y + " " + vector.z);
			}
			vertices = meshToExport.normals;
			for (int j = 0; j < vertices.Length; j++)
			{
				Vector3 vector2 = vertices[j];
				if (applyScale)
				{
					vector2 = MultiplyVec3s(vector2, toExport.gameObject.transform.lossyScale.normalized);
				}
				if (applyRotation)
				{
					vector2 = RotateAroundPoint(vector2, Vector3.zero, toExport.gameObject.transform.rotation);
				}
				vector2.x *= -1f;
				stringBuilder.AppendLine("vn " + vector2.x + " " + vector2.y + " " + vector2.z);
			}
			Vector2[] uv = meshToExport.uv;
			for (int j = 0; j < uv.Length; j++)
			{
				Vector2 vector3 = uv[j];
				stringBuilder.AppendLine("vt " + vector3.x + " " + vector3.y);
			}
			for (int k = 0; k < meshToExport.subMeshCount; k++)
			{
				if (meshRenderer != null && k < meshRenderer.sharedMaterials.Length)
				{
					string name2 = meshRenderer.sharedMaterials[k].name;
					stringBuilder.AppendLine("usemtl " + name2);
				}
				else
				{
					stringBuilder.AppendLine("usemtl " + name + "_sm" + k);
				}
				int[] triangles = meshToExport.GetTriangles(k);
				for (int l = 0; l < triangles.Length; l += 3)
				{
					int index = triangles[l] + 1 + num;
					int index2 = triangles[l + 1] + 1 + num;
					int index3 = triangles[l + 2] + 1 + num;
					if (num2 < 0)
					{
						stringBuilder.AppendLine("f " + ConstructOBJString(index) + " " + ConstructOBJString(index2) + " " + ConstructOBJString(index3));
					}
					else
					{
						stringBuilder.AppendLine("f " + ConstructOBJString(index3) + " " + ConstructOBJString(index2) + " " + ConstructOBJString(index));
					}
				}
			}
			num += meshToExport.vertices.Length;
			File.WriteAllText(Path.Combine(exportPath, name + ".obj"), stringBuilder.ToString());
			if (generateMaterials)
			{
				File.WriteAllText(Path.Combine(exportPath, name + ".mtl"), stringBuilder2.ToString());
			}
			OnSuccess?.Invoke();
		}

		public async Task ExportMeshToOBJ(Mesh mesh, string exportPath)
		{
			InitializeExporter(mesh, exportPath);
			string objectName = meshToExport.name;
			StringBuilder sb = new StringBuilder();
			int lastIndex = 0;
			int faceOrder = 1;
			Vector3[] vertices = meshToExport.vertices;
			foreach (Vector3 vx in vertices)
			{
				await Task.Delay(1);
				Vector3 vector = vx;
				vector.x *= -1f;
				sb.AppendLine("v " + vector.x + " " + vector.y + " " + vector.z);
			}
			vertices = meshToExport.normals;
			foreach (Vector3 vx in vertices)
			{
				await Task.Delay(1);
				Vector3 vector2 = vx;
				vector2.x *= -1f;
				sb.AppendLine("vn " + vector2.x + " " + vector2.y + " " + vector2.z);
			}
			Vector2[] uv = meshToExport.uv;
			for (int i = 0; i < uv.Length; i++)
			{
				Vector2 v = uv[i];
				await Task.Delay(1);
				sb.AppendLine("vt " + v.x + " " + v.y);
			}
			for (int i = 0; i < meshToExport.subMeshCount; i++)
			{
				await Task.Delay(1);
				sb.AppendLine("usemtl " + objectName + "_sm" + i);
				int[] tris = meshToExport.GetTriangles(i);
				for (int t = 0; t < tris.Length; t += 3)
				{
					await Task.Delay(1);
					int index = tris[t] + 1 + lastIndex;
					int index2 = tris[t + 1] + 1 + lastIndex;
					int index3 = tris[t + 2] + 1 + lastIndex;
					if (faceOrder < 0)
					{
						sb.AppendLine("f " + ConstructOBJString(index) + " " + ConstructOBJString(index2) + " " + ConstructOBJString(index3));
					}
					else
					{
						sb.AppendLine("f " + ConstructOBJString(index3) + " " + ConstructOBJString(index2) + " " + ConstructOBJString(index));
					}
				}
			}
			_ = lastIndex + meshToExport.vertices.Length;
			File.WriteAllText(Path.Combine(exportPath, objectName + ".obj"), sb.ToString());
		}

		private string TryExportTexture(string propertyName, Material m, string exportPath)
		{
			if (m.HasProperty(propertyName))
			{
				Texture texture = m.GetTexture(propertyName);
				if (texture != null)
				{
					return ExportTexture((Texture2D)texture, exportPath);
				}
			}
			return "false";
		}

		private string ExportTexture(Texture2D t, string exportPath)
		{
			string name = t.name;
			try
			{
				Color32[] array = null;
				try
				{
					array = t.GetPixels32();
				}
				catch (UnityException)
				{
					t = DuplicateTexture(t);
					array = t.GetPixels32();
				}
				string text = Path.Combine(exportPath, name + ".png");
				Texture2D texture2D = new Texture2D(t.width, t.height, TextureFormat.ARGB32, mipChain: false);
				texture2D.SetPixels32(array);
				File.WriteAllBytes(text, texture2D.EncodeToPNG());
				return text;
			}
			catch (Exception)
			{
				Debug.Log("Could not export texture : " + t.name + ". is it readable?");
				return "null";
			}
		}

		private string ConstructOBJString(int index)
		{
			string text = index.ToString();
			return text + "/" + text + "/" + text;
		}

		private string MaterialToString(Material m)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("newmtl " + m.name);
			if (m.HasProperty("_Color"))
			{
				stringBuilder.AppendLine("Kd " + m.color.r + " " + m.color.g + " " + m.color.b);
				if (m.color.a < 1f)
				{
					stringBuilder.AppendLine("Tr " + (1f - m.color.a));
					stringBuilder.AppendLine("d " + m.color.a);
				}
			}
			if (m.HasProperty("_SpecColor"))
			{
				Color color = m.GetColor("_SpecColor");
				stringBuilder.AppendLine("Ks " + color.r + " " + color.g + " " + color.b);
			}
			if (exportTextures)
			{
				string text = TryExportTexture("_MainTex", m, exportPath);
				if (text != "false")
				{
					stringBuilder.AppendLine("map_Kd " + text);
				}
				text = TryExportTexture("_SpecMap", m, exportPath);
				if (text != "false")
				{
					stringBuilder.AppendLine("map_Ks " + text);
				}
				text = TryExportTexture("_BumpMap", m, exportPath);
				if (text != "false")
				{
					stringBuilder.AppendLine("map_Bump " + text);
				}
			}
			stringBuilder.AppendLine("illum 2");
			return stringBuilder.ToString();
		}

		public async Task ImportFromLocalFileSystem(string objPath, string texturesFolderPath, string materialsFolderPath, Action<GameObject> Callback, PolyfewRuntime.OBJImportOptions importOptions = null)
		{
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				Debug.LogWarning("The function cannot run on WebGL player. As web apps cannot read from or write to local file system.");
				return;
			}
			if (!string.IsNullOrWhiteSpace(objPath))
			{
				objPath = Path.GetFullPath(objPath);
				if (objPath[objPath.Length - 1] == '\\')
				{
					objPath = objPath.Remove(objPath.Length - 1);
				}
				else if (objPath[objPath.Length - 1] == '/')
				{
					objPath = objPath.Remove(objPath.Length - 1);
				}
			}
			if (!string.IsNullOrWhiteSpace(texturesFolderPath))
			{
				texturesFolderPath = Path.GetFullPath(texturesFolderPath);
				if (texturesFolderPath[texturesFolderPath.Length - 1] == '\\')
				{
					texturesFolderPath = texturesFolderPath.Remove(texturesFolderPath.Length - 1);
				}
				else if (texturesFolderPath[texturesFolderPath.Length - 1] == '/')
				{
					texturesFolderPath = texturesFolderPath.Remove(texturesFolderPath.Length - 1);
				}
			}
			if (!string.IsNullOrWhiteSpace(materialsFolderPath))
			{
				materialsFolderPath = Path.GetFullPath(materialsFolderPath);
				if (materialsFolderPath[materialsFolderPath.Length - 1] == '\\')
				{
					materialsFolderPath = materialsFolderPath.Remove(materialsFolderPath.Length - 1);
				}
				else if (materialsFolderPath[materialsFolderPath.Length - 1] == '/')
				{
					materialsFolderPath = materialsFolderPath.Remove(materialsFolderPath.Length - 1);
				}
			}
			if (!File.Exists(objPath))
			{
				throw new FileNotFoundException("The path provided doesn't point to a file. The path might be invalid or the file is non-existant.");
			}
			if (!string.IsNullOrWhiteSpace(texturesFolderPath) && !Directory.Exists(texturesFolderPath))
			{
				Debug.LogWarning("The directory pointed to by the given path for textures is non-existant.");
			}
			if (!string.IsNullOrWhiteSpace(materialsFolderPath) && !Directory.Exists(materialsFolderPath))
			{
				Debug.LogWarning("The directory pointed to by the given path for materials is non-existant.");
			}
			string fileName = Path.GetFileName(objPath);
			string directoryName = Path.GetDirectoryName(objPath);
			string objName = fileName.Split('.')[0];
			GameObject objectToPopulate = new GameObject();
			objectToPopulate.AddComponent<ObjectImporter>();
			ObjectImporter objImporter = objectToPopulate.GetComponent<ObjectImporter>();
			if (directoryName.Contains("/") && !directoryName.EndsWith("/"))
			{
				_ = directoryName + "/";
			}
			else if (!directoryName.EndsWith("\\"))
			{
				_ = directoryName + "\\";
			}
			if (fileName.Split('.')[1].ToLower() != "obj")
			{
				UnityEngine.Object.DestroyImmediate(objectToPopulate);
				throw new InvalidOperationException("The path provided must point to a wavefront obj file.");
			}
			if (importOptions == null)
			{
				importOptions = new PolyfewRuntime.OBJImportOptions();
			}
			try
			{
				GameObject obj = await objImporter.ImportModelAsync(objName, objPath, null, importOptions, texturesFolderPath, materialsFolderPath);
				UnityEngine.Object.Destroy(objImporter);
				Callback(obj);
			}
			catch (Exception ex)
			{
				UnityEngine.Object.DestroyImmediate(objectToPopulate);
				throw ex;
			}
		}

		public async void ImportFromNetwork(string objURL, string objName, string diffuseTexURL, string bumpTexURL, string specularTexURL, string opacityTexURL, string materialURL, PolyfewRuntime.ReferencedNumeric<float> downloadProgress, Action<GameObject> OnSuccess, Action<Exception> OnError, PolyfewRuntime.OBJImportOptions importOptions = null)
		{
			if (string.IsNullOrWhiteSpace(objURL))
			{
				throw new InvalidOperationException("Cannot download from empty URL. Please provide a direct URL to the obj file");
			}
			if (string.IsNullOrWhiteSpace(diffuseTexURL))
			{
				Debug.LogWarning("Cannot download from empty URL. Please provide a direct URL to the accompanying texture file.");
			}
			if (string.IsNullOrWhiteSpace(materialURL))
			{
				Debug.LogWarning("Cannot download from empty URL. Please provide a direct URL to the accompanying material file.");
			}
			if (downloadProgress == null)
			{
				throw new ArgumentNullException("downloadProgress", "You must pass a reference to the Download Progress object.");
			}
			GameObject objectToPopulate = new GameObject();
			objectToPopulate.AddComponent<ObjectImporter>();
			ObjectImporter objImporter = objectToPopulate.GetComponent<ObjectImporter>();
			if (importOptions == null)
			{
				importOptions = new PolyfewRuntime.OBJImportOptions();
			}
			try
			{
				GameObject obj = await objImporter.ImportModelFromNetwork(objURL, objName, diffuseTexURL, bumpTexURL, specularTexURL, opacityTexURL, materialURL, downloadProgress, importOptions);
				UnityEngine.Object.Destroy(objImporter);
				OnSuccess(obj);
			}
			catch (Exception obj2)
			{
				UnityEngine.Object.DestroyImmediate(objectToPopulate);
				OnError(obj2);
			}
		}

		public async void ImportFromNetworkWebGL(string objURL, string objName, string diffuseTexURL, string bumpTexURL, string specularTexURL, string opacityTexURL, string materialURL, PolyfewRuntime.ReferencedNumeric<float> downloadProgress, Action<GameObject> OnSuccess, Action<Exception> OnError, PolyfewRuntime.OBJImportOptions importOptions = null)
		{
			if (string.IsNullOrWhiteSpace(objURL))
			{
				OnError(new InvalidOperationException("Cannot download from empty URL. Please provide a direct URL to the obj file"));
				return;
			}
			if (string.IsNullOrWhiteSpace(diffuseTexURL))
			{
				Debug.LogWarning("Cannot download from empty URL. Please provide a direct URL to the accompanying texture file.");
			}
			if (string.IsNullOrWhiteSpace(materialURL))
			{
				Debug.LogWarning("Cannot download from empty URL. Please provide a direct URL to the accompanying material file.");
			}
			if (downloadProgress == null)
			{
				OnError(new ArgumentNullException("downloadProgress", "You must pass a reference to the Download Progress object."));
				return;
			}
			GameObject objectToPopulate = new GameObject();
			objectToPopulate.AddComponent<ObjectImporter>();
			ObjectImporter objImporter = objectToPopulate.GetComponent<ObjectImporter>();
			if (importOptions == null)
			{
				importOptions = new PolyfewRuntime.OBJImportOptions();
			}
			objImporter.ImportModelFromNetworkWebGL(objURL, objName, diffuseTexURL, bumpTexURL, specularTexURL, opacityTexURL, materialURL, downloadProgress, importOptions, delegate(GameObject imported)
			{
				UnityEngine.Object.Destroy(objImporter);
				OnSuccess(imported);
			}, delegate(Exception exception)
			{
				UnityEngine.Object.DestroyImmediate(objectToPopulate);
				OnError(exception);
			});
		}
	}

	public static Texture2D DuplicateTexture(Texture2D source)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
		Graphics.Blit(source, temporary);
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = temporary;
		Texture2D texture2D = new Texture2D(source.width, source.height);
		texture2D.ReadPixels(new Rect(0f, 0f, temporary.width, temporary.height), 0, 0);
		texture2D.Apply();
		RenderTexture.active = active;
		RenderTexture.ReleaseTemporary(temporary);
		return texture2D;
	}

	public static Renderer[] GetChildRenderersForCombining(GameObject forObject, bool skipInactiveChildObjects)
	{
		List<Renderer> list = new List<Renderer>();
		if (skipInactiveChildObjects && !forObject.gameObject.activeSelf)
		{
			Debug.LogWarning("No Renderers under the GameObject \"" + forObject.name + "\" combined because the object was inactive and was skipped entirely.");
			return null;
		}
		if (forObject.GetComponent<LODGroup>() != null)
		{
			Debug.LogWarning("No Renderers under the GameObject \"" + forObject.name + "\" combined because the object had LOD groups and was skipped entirely.");
			return null;
		}
		CollectChildRenderersForCombining(forObject.transform, list, skipInactiveChildObjects);
		return list.ToArray();
	}

	public static MeshRenderer CreateStaticLevelRenderer(string name, Transform parentTransform, Transform originalTransform, Mesh mesh, Material[] materials)
	{
		GameObject obj = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer));
		Transform transform = obj.transform;
		if (originalTransform != null)
		{
			ParentAndOffsetTransform(transform, parentTransform, originalTransform);
		}
		else
		{
			ParentAndResetTransform(transform, parentTransform);
		}
		obj.GetComponent<MeshFilter>().sharedMesh = mesh;
		MeshRenderer component = obj.GetComponent<MeshRenderer>();
		component.sharedMaterials = materials;
		return component;
	}

	public static SkinnedMeshRenderer CreateSkinnedLevelRenderer(string name, Transform parentTransform, Transform originalTransform, Mesh mesh, Material[] materials, Transform rootBone, Transform[] bones)
	{
		GameObject obj = new GameObject(name, typeof(SkinnedMeshRenderer));
		Transform transform = obj.transform;
		if (originalTransform != null)
		{
			ParentAndOffsetTransform(transform, parentTransform, originalTransform);
		}
		else
		{
			ParentAndResetTransform(transform, parentTransform);
		}
		SkinnedMeshRenderer component = obj.GetComponent<SkinnedMeshRenderer>();
		component.sharedMesh = mesh;
		component.sharedMaterials = materials;
		component.rootBone = rootBone;
		component.bones = bones;
		return component;
	}

	private static void CollectChildRenderersForCombining(Transform transform, List<Renderer> resultRenderers, bool skipInactiveChildObjects)
	{
		Renderer[] components = transform.GetComponents<Renderer>();
		resultRenderers.AddRange(components);
		int childCount = transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (skipInactiveChildObjects && !child.gameObject.activeSelf)
			{
				Debug.LogWarning("No Renderers under the GameObject \"" + transform.name + "\" combined because the object was inactive and was skipped entirely.");
			}
			else if (child.GetComponent<LODGroup>() != null)
			{
				Debug.LogWarning("No Renderers under the GameObject \"" + transform.name + "\" combined because the object had LOD groups and was skipped entirely.");
			}
			else
			{
				CollectChildRenderersForCombining(child, resultRenderers, skipInactiveChildObjects);
			}
		}
	}

	private static void ParentAndResetTransform(Transform transform, Transform parentTransform)
	{
		transform.SetParent(parentTransform);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
	}

	public static void ParentAndOffsetTransform(Transform transform, Transform parentTransform, Transform originalTransform)
	{
		transform.position = originalTransform.position;
		transform.rotation = originalTransform.rotation;
		transform.localScale = originalTransform.lossyScale;
		transform.SetParent(parentTransform, worldPositionStays: true);
	}
}
