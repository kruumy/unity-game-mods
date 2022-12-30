using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace AsImpL;

public class ObjectBuilder
{
	public class ProgressInfo
	{
		public int materialsLoaded;

		public int objectsLoaded;

		public int groupsLoaded;

		public int numGroups;
	}

	private class BuildStatus
	{
		public bool newObject = true;

		public int objCount;

		public int subObjCount;

		public int idxCount;

		public int grpIdx;

		public int numGroups;

		public int grpFaceIdx;

		public int meshPartIdx;

		public int totFaceIdxCount;

		public GameObject currObjGameObject;

		internal GameObject subObjParent;
	}

	public ImportOptions buildOptions;

	private BuildStatus buildStatus = new BuildStatus();

	private DataSet currDataSet;

	private GameObject currParentObj;

	private Dictionary<string, Material> currMaterials;

	private List<MaterialData> materialData;

	private static int MAX_VERTICES_LIMIT_FOR_A_MESH = 65000;

	private static int MAX_INDICES_LIMIT_FOR_A_MESH = 65000;

	private static int MAX_VERT_COUNT = (MAX_VERTICES_LIMIT_FOR_A_MESH - 2) / 3 * 3;

	public Dictionary<string, Material> ImportedMaterials => currMaterials;

	public int NumImportedMaterials
	{
		get
		{
			if (currMaterials == null)
			{
				return 0;
			}
			return currMaterials.Count;
		}
	}

	public void InitBuildMaterials(List<MaterialData> materialData, bool hasColors)
	{
		this.materialData = materialData;
		currMaterials = new Dictionary<string, Material>();
		if (materialData != null && materialData.Count != 0)
		{
			return;
		}
		string name = "VertexLit";
		if (hasColors)
		{
			name = "Unlit/Simple Vertex Colors Shader";
			if (Shader.Find(name) == null)
			{
				name = "Mobile/Particles/Alpha Blended";
			}
			Debug.Log("No material library defined. Using vertex colors.");
		}
		else
		{
			Debug.LogWarning("No material library defined. Using a default material.");
		}
		currMaterials.Add("default", new Material(Shader.Find(name)));
	}

	public bool BuildMaterials(ProgressInfo info)
	{
		if (this.materialData == null)
		{
			Debug.LogWarning("No material library defined.");
			return false;
		}
		if (info.materialsLoaded >= this.materialData.Count)
		{
			return false;
		}
		MaterialData materialData = this.materialData[info.materialsLoaded];
		info.materialsLoaded++;
		if (currMaterials.ContainsKey(materialData.materialName))
		{
			Debug.LogWarning("Duplicate material found: " + materialData.materialName + ". Repeated occurence ignored");
		}
		else
		{
			currMaterials.Add(materialData.materialName, BuildMaterial(materialData));
		}
		return info.materialsLoaded < this.materialData.Count;
	}

	public void StartBuildObjectAsync(DataSet dataSet, GameObject parentObj, Dictionary<string, Material> materials = null)
	{
		currDataSet = dataSet;
		currParentObj = parentObj;
		if (materials != null)
		{
			currMaterials = materials;
		}
	}

	public bool BuildObjectAsync(ref ProgressInfo info)
	{
		bool result = BuildNextObject(currParentObj, currMaterials);
		info.objectsLoaded = buildStatus.objCount;
		info.groupsLoaded = buildStatus.subObjCount;
		info.numGroups = buildStatus.numGroups;
		return result;
	}

	public static void Solve(Mesh origMesh)
	{
		if (origMesh.uv == null || origMesh.uv.Length == 0)
		{
			Debug.LogWarning("Unable to compute tangent space vectors - texture coordinates not defined.");
			return;
		}
		if (origMesh.vertices == null || origMesh.vertices.Length == 0)
		{
			Debug.LogWarning("Unable to compute tangent space vectors - vertices not defined.");
			return;
		}
		if (origMesh.normals == null || origMesh.normals.Length == 0)
		{
			Debug.LogWarning("Unable to compute tangent space vectors - normals not defined.");
			return;
		}
		if (origMesh.triangles == null || origMesh.triangles.Length == 0)
		{
			Debug.LogWarning("Unable to compute tangent space vectors - triangles not defined.");
			return;
		}
		Vector3[] vertices = origMesh.vertices;
		Vector3[] normals = origMesh.normals;
		Vector2[] uv = origMesh.uv;
		int[] triangles = origMesh.triangles;
		_ = origMesh.triangles.Length;
		int num = -1;
		for (int i = 0; i < triangles.Length; i++)
		{
			if (num < triangles[i])
			{
				num = triangles[i];
			}
		}
		if (vertices.Length <= num)
		{
			Debug.LogWarning("Unable to compute tangent space vectors - not enough vertices: " + vertices.Length);
			return;
		}
		if (normals.Length <= num)
		{
			Debug.LogWarning("Unable to compute tangent space vectors - not enough normals.");
			return;
		}
		if (uv.Length <= num)
		{
			Debug.LogWarning("Unable to compute tangent space vectors - not enough UVs.");
			return;
		}
		int vertexCount = origMesh.vertexCount;
		Vector4[] array = new Vector4[vertexCount];
		Vector3[] array2 = new Vector3[vertexCount];
		Vector3[] array3 = new Vector3[vertexCount];
		int num2 = triangles.Length / 3;
		int num3 = 0;
		for (int j = 0; j < num2; j++)
		{
			int num4 = triangles[num3];
			int num5 = triangles[num3 + 1];
			int num6 = triangles[num3 + 2];
			Vector3 vector = vertices[num4];
			Vector3 vector2 = vertices[num5];
			Vector3 vector3 = vertices[num6];
			Vector2 vector4 = uv[num4];
			Vector2 vector5 = uv[num5];
			Vector2 vector6 = uv[num6];
			float num7 = vector2.x - vector.x;
			float num8 = vector3.x - vector.x;
			float num9 = vector2.y - vector.y;
			float num10 = vector3.y - vector.y;
			float num11 = vector2.z - vector.z;
			float num12 = vector3.z - vector.z;
			float num13 = vector5.x - vector4.x;
			float num14 = vector6.x - vector4.x;
			float num15 = vector5.y - vector4.y;
			float num16 = vector6.y - vector4.y;
			float num17 = 1f / (num13 * num16 - num14 * num15);
			Vector3 vector7 = new Vector3((num16 * num7 - num15 * num8) * num17, (num16 * num9 - num15 * num10) * num17, (num16 * num11 - num15 * num12) * num17);
			Vector3 vector8 = new Vector3((num13 * num8 - num14 * num7) * num17, (num13 * num10 - num14 * num9) * num17, (num13 * num12 - num14 * num11) * num17);
			array2[num4] += vector7;
			array2[num5] += vector7;
			array2[num6] += vector7;
			array3[num4] += vector8;
			array3[num5] += vector8;
			array3[num6] += vector8;
			num3 += 3;
		}
		for (int k = 0; k < vertexCount; k++)
		{
			Vector3 normal = normals[k];
			Vector3 tangent = array2[k];
			Vector3.OrthoNormalize(ref normal, ref tangent);
			array[k].x = tangent.x;
			array[k].y = tangent.y;
			array[k].z = tangent.z;
			array[k].w = ((Vector3.Dot(Vector3.Cross(normal, tangent), array3[k]) < 0f) ? (-1f) : 1f);
		}
		origMesh.tangents = array;
	}

	public static void BuildMeshCollider(GameObject targetObject, bool convex = false, bool isTrigger = false, bool inflateMesh = false, float skinWidth = 0.01f)
	{
		MeshFilter component = targetObject.GetComponent<MeshFilter>();
		if (component != null && component.sharedMesh != null)
		{
			Mesh sharedMesh = component.sharedMesh;
			MeshCollider meshCollider = targetObject.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = sharedMesh;
			if (convex)
			{
				meshCollider.convex = convex;
				meshCollider.isTrigger = isTrigger;
			}
		}
	}

	protected bool BuildNextObject(GameObject parentObj, Dictionary<string, Material> mats)
	{
		if (buildStatus.objCount >= currDataSet.objectList.Count)
		{
			return false;
		}
		DataSet.ObjectData objectData = currDataSet.objectList[buildStatus.objCount];
		if (buildStatus.newObject)
		{
			if (buildStatus.objCount == 0 && objectData.name == "default")
			{
				buildStatus.currObjGameObject = parentObj;
			}
			else
			{
				buildStatus.currObjGameObject = new GameObject();
				buildStatus.currObjGameObject.transform.parent = parentObj.transform;
				buildStatus.currObjGameObject.name = objectData.name;
				buildStatus.currObjGameObject.transform.localScale = Vector3.one;
			}
			buildStatus.subObjParent = buildStatus.currObjGameObject;
			buildStatus.newObject = false;
			buildStatus.subObjCount = 0;
			buildStatus.idxCount = 0;
			buildStatus.grpIdx = 0;
			buildStatus.grpFaceIdx = 0;
			buildStatus.meshPartIdx = 0;
			buildStatus.totFaceIdxCount = 0;
			buildStatus.numGroups = Mathf.Max(1, objectData.faceGroups.Count);
		}
		bool flag = true;
		if (Using32bitIndices())
		{
			flag = false;
		}
		bool flag2 = false;
		DataSet.FaceGroupData faceGroupData = new DataSet.FaceGroupData();
		faceGroupData.name = objectData.faceGroups[buildStatus.grpIdx].name;
		faceGroupData.materialName = objectData.faceGroups[buildStatus.grpIdx].materialName;
		DataSet.ObjectData objectData2 = new DataSet.ObjectData();
		objectData2.hasNormals = objectData.hasNormals;
		objectData2.hasColors = objectData.hasColors;
		HashSet<int> hashSet = new HashSet<int>();
		int num = ((buildOptions != null && buildOptions.convertToDoubleSided) ? (MAX_INDICES_LIMIT_FOR_A_MESH / 2) : MAX_INDICES_LIMIT_FOR_A_MESH);
		for (int i = buildStatus.grpFaceIdx; i < objectData.faceGroups[buildStatus.grpIdx].faces.Count; i++)
		{
			if (flag && (hashSet.Count / 3 > MAX_VERT_COUNT / 3 || objectData2.allFaces.Count / 3 > num / 3))
			{
				flag2 = true;
				buildStatus.grpFaceIdx = i;
				Debug.LogWarningFormat("Maximum vertex number for a mesh exceeded.\nSplitting object {0} (group {1}, starting from index {2})...", faceGroupData.name, buildStatus.grpIdx, i);
				break;
			}
			DataSet.FaceIndices item = objectData.faceGroups[buildStatus.grpIdx].faces[i];
			objectData2.allFaces.Add(item);
			faceGroupData.faces.Add(item);
			hashSet.Add(item.vertIdx);
		}
		if (flag2 || buildStatus.meshPartIdx > 0)
		{
			buildStatus.meshPartIdx++;
		}
		if (buildStatus.meshPartIdx == 1)
		{
			GameObject gameObject = new GameObject();
			gameObject.transform.SetParent(buildStatus.currObjGameObject.transform, worldPositionStays: false);
			gameObject.name = faceGroupData.name;
			buildStatus.subObjParent = gameObject;
		}
		if (buildStatus.meshPartIdx > 0)
		{
			faceGroupData.name = buildStatus.subObjParent.name + "_MeshPart" + buildStatus.meshPartIdx;
		}
		objectData2.name = faceGroupData.name;
		objectData2.faceGroups.Add(faceGroupData);
		buildStatus.idxCount += objectData2.allFaces.Count;
		if (!flag2)
		{
			buildStatus.grpFaceIdx = 0;
			buildStatus.grpIdx++;
		}
		buildStatus.totFaceIdxCount += objectData2.allFaces.Count;
		if (ImportSubObject(buildStatus.subObjParent, objectData2, mats) == null)
		{
			Debug.LogWarningFormat("Error loading sub object n.{0}.", buildStatus.subObjCount);
		}
		buildStatus.subObjCount++;
		if (buildStatus.totFaceIdxCount >= objectData.allFaces.Count || buildStatus.grpIdx >= objectData.faceGroups.Count)
		{
			if (buildStatus.totFaceIdxCount != objectData.allFaces.Count)
			{
				Debug.LogWarningFormat("Imported face indices: {0} of {1}", buildStatus.totFaceIdxCount, objectData.allFaces.Count);
				return false;
			}
			buildStatus.objCount++;
			buildStatus.newObject = true;
		}
		return true;
	}

	private GameObject ImportSubObject(GameObject parentObj, DataSet.ObjectData objData, Dictionary<string, Material> mats)
	{
		bool flag = buildOptions != null && buildOptions.convertToDoubleSided;
		GameObject gameObject = new GameObject();
		gameObject.name = objData.name;
		int num = 0;
		if ((bool)parentObj.transform)
		{
			while ((bool)parentObj.transform.Find(gameObject.name))
			{
				num++;
				gameObject.name = objData.name + num;
			}
		}
		gameObject.transform.SetParent(parentObj.transform, worldPositionStays: false);
		if (objData.allFaces.Count == 0)
		{
			throw new InvalidOperationException("Failed to parse vertex and uv data. It might be that the file is corrupt or is not a valid wavefront OBJ file.");
		}
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		int num2 = 0;
		foreach (DataSet.FaceIndices allFace in objData.allFaces)
		{
			string faceIndicesKey = DataSet.GetFaceIndicesKey(allFace);
			if (!dictionary.TryGetValue(faceIndicesKey, out var _))
			{
				dictionary.Add(faceIndicesKey, num2);
				num2++;
			}
		}
		int num3 = (flag ? (num2 * 2) : num2);
		Vector3[] array = new Vector3[num3];
		Vector2[] array2 = new Vector2[num3];
		Vector3[] array3 = new Vector3[num3];
		Color32[] array4 = new Color32[num3];
		bool flag2 = currDataSet.colorList.Count > 0;
		foreach (DataSet.FaceIndices allFace2 in objData.allFaces)
		{
			string faceIndicesKey2 = DataSet.GetFaceIndicesKey(allFace2);
			int num4 = dictionary[faceIndicesKey2];
			array[num4] = currDataSet.vertList[allFace2.vertIdx];
			if (flag)
			{
				array[num2 + num4] = array[num4];
			}
			if (flag2)
			{
				array4[num4] = currDataSet.colorList[allFace2.vertIdx];
				if (flag)
				{
					array4[num2 + num4] = array4[num4];
				}
			}
			if (currDataSet.uvList.Count > 0)
			{
				array2[num4] = currDataSet.uvList[allFace2.uvIdx];
				if (flag)
				{
					array2[num2 + num4] = array2[num4];
				}
			}
			if (currDataSet.normalList.Count > 0 && allFace2.normIdx >= 0)
			{
				array3[num4] = currDataSet.normalList[allFace2.normIdx];
				if (flag)
				{
					array3[num2 + num4] = -array3[num4];
				}
			}
		}
		bool flag3 = currDataSet.normalList.Count > 0 && objData.hasNormals;
		bool num5 = currDataSet.colorList.Count > 0 && objData.hasColors;
		bool flag4 = currDataSet.uvList.Count > 0;
		int count = objData.faceGroups[0].faces.Count;
		int num6 = (flag ? (count * 2) : count);
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshRenderer>();
		Mesh mesh = new Mesh();
		if (Using32bitIndices() && (num3 > MAX_VERT_COUNT || num6 > MAX_INDICES_LIMIT_FOR_A_MESH))
		{
			mesh.indexFormat = IndexFormat.UInt32;
		}
		mesh.name = gameObject.name;
		meshFilter.sharedMesh = mesh;
		mesh.vertices = array;
		if (flag4)
		{
			mesh.uv = array2;
		}
		if (flag3)
		{
			mesh.normals = array3;
		}
		if (num5)
		{
			mesh.colors32 = array4;
		}
		string text = ((objData.faceGroups[0].materialName != null) ? objData.faceGroups[0].materialName : "default");
		Renderer component = gameObject.GetComponent<Renderer>();
		if (mats.ContainsKey(text))
		{
			Material material2 = (component.sharedMaterial = mats[text]);
			component.UpdateGIMaterials();
		}
		else if (mats.ContainsKey("default"))
		{
			Material material2 = (component.sharedMaterial = mats["default"]);
			Debug.LogWarning("Material: " + text + " not found. Using the default material.");
		}
		else
		{
			Debug.LogError("Material: " + text + " not found.");
		}
		int[] array5 = new int[num6];
		for (int i = 0; i < count; i++)
		{
			string faceIndicesKey3 = DataSet.GetFaceIndicesKey(objData.faceGroups[0].faces[i]);
			array5[i] = dictionary[faceIndicesKey3];
		}
		if (flag)
		{
			for (int j = 0; j < count; j++)
			{
				array5[j + count] = num2 + array5[j / 3 * 3 + 2 - j % 3];
			}
		}
		mesh.SetTriangles(array5, 0);
		if (!flag3)
		{
			mesh.RecalculateNormals();
		}
		if (flag4)
		{
			Solve(mesh);
		}
		if (buildOptions != null && buildOptions.buildColliders)
		{
			BuildMeshCollider(gameObject, buildOptions.colliderConvex, buildOptions.colliderTrigger);
		}
		return gameObject;
	}

	private Material BuildMaterial(MaterialData md)
	{
		string name = "Standard";
		bool flag = false;
		ModelUtil.MtlBlendMode mode = ((md.overallAlpha < 1f) ? ModelUtil.MtlBlendMode.TRANSPARENT : ModelUtil.MtlBlendMode.OPAQUE);
		bool num = buildOptions != null && buildOptions.litDiffuse && md.diffuseTex != null && md.bumpTex == null && md.opacityTex == null && md.specularTex == null && !md.hasReflectionTex;
		bool? flag2 = null;
		if (num)
		{
			flag2 = ModelUtil.ScanTransparentPixels(md.diffuseTex, ref mode);
		}
		if (num && !flag2.Value)
		{
			name = "Unlit/Texture";
		}
		else if (flag)
		{
			name = "Standard (Specular setup)";
		}
		Material material = new Material(Shader.Find(name));
		material.name = md.materialName;
		float num2 = Mathf.Log(md.shininess, 2f);
		float num3 = Mathf.Clamp01(num2 / 10f);
		float num4 = Mathf.Clamp01(num2 / 10f);
		if (flag)
		{
			material.SetColor("_SpecColor", md.specularColor);
			material.SetFloat("_Shininess", md.shininess / 1000f);
		}
		else
		{
			material.SetFloat("_Metallic", num3);
		}
		if (md.diffuseTex != null)
		{
			if (md.opacityTex != null)
			{
				int width = md.diffuseTex.width;
				int width2 = md.diffuseTex.width;
				Texture2D texture2D = new Texture2D(width, width2, TextureFormat.ARGB32, mipChain: false);
				Color color = default(Color);
				for (int i = 0; i < texture2D.width; i++)
				{
					for (int j = 0; j < texture2D.height; j++)
					{
						color = md.diffuseTex.GetPixel(i, j);
						color.a *= md.opacityTex.GetPixel(i, j).grayscale;
						texture2D.SetPixel(i, j, color);
					}
				}
				texture2D.name = md.diffuseTexPath;
				texture2D.Apply();
				mode = ModelUtil.MtlBlendMode.FADE;
				material.SetTexture("_MainTex", texture2D);
			}
			else
			{
				if (!flag2.HasValue)
				{
					flag2 = ModelUtil.ScanTransparentPixels(md.diffuseTex, ref mode);
				}
				material.SetTexture("_MainTex", md.diffuseTex);
			}
		}
		else if (md.opacityTex != null)
		{
			mode = ModelUtil.MtlBlendMode.FADE;
			int width3 = md.opacityTex.width;
			int width4 = md.opacityTex.width;
			Texture2D texture2D2 = new Texture2D(width3, width4, TextureFormat.ARGB32, mipChain: false);
			Color color2 = default(Color);
			bool noDoubt = false;
			for (int k = 0; k < texture2D2.width; k++)
			{
				for (int l = 0; l < texture2D2.height; l++)
				{
					color2 = md.diffuseColor;
					color2.a = md.overallAlpha * md.opacityTex.GetPixel(k, l).grayscale;
					ModelUtil.DetectMtlBlendFadeOrCutout(color2.a, ref mode, ref noDoubt);
					texture2D2.SetPixel(k, l, color2);
				}
			}
			texture2D2.name = md.diffuseTexPath;
			texture2D2.Apply();
			material.SetTexture("_MainTex", texture2D2);
		}
		md.diffuseColor.a = md.overallAlpha;
		material.SetColor("_Color", md.diffuseColor);
		md.emissiveColor.a = md.overallAlpha;
		material.SetColor("_EmissionColor", md.emissiveColor);
		if (md.emissiveColor.r > 0f || md.emissiveColor.g > 0f || md.emissiveColor.b > 0f)
		{
			material.EnableKeyword("_EMISSION");
		}
		if (md.bumpTex != null)
		{
			if (md.bumpTexPath.Contains("_normal_map"))
			{
				material.EnableKeyword("_NORMALMAP");
				material.SetFloat("_BumpScale", 0.25f);
				material.SetTexture("_BumpMap", md.bumpTex);
			}
			else
			{
				Texture2D value = ModelUtil.HeightToNormalMap(md.bumpTex);
				material.SetTexture("_BumpMap", value);
				material.EnableKeyword("_NORMALMAP");
				material.SetFloat("_BumpScale", 1f);
			}
		}
		if (md.specularTex != null)
		{
			Texture2D texture2D3 = new Texture2D(md.specularTex.width, md.specularTex.height, TextureFormat.ARGB32, mipChain: false);
			Color color3 = default(Color);
			float num5 = 0f;
			for (int m = 0; m < texture2D3.width; m++)
			{
				for (int n = 0; n < texture2D3.height; n++)
				{
					num5 = md.specularTex.GetPixel(m, n).grayscale;
					color3.r = num3 * num5;
					color3.g = color3.r;
					color3.b = color3.r;
					if (md.hasReflectionTex)
					{
						color3.a = num5;
					}
					else
					{
						color3.a = num5 * num4;
					}
					texture2D3.SetPixel(m, n, color3);
				}
			}
			texture2D3.Apply();
			if (flag)
			{
				material.EnableKeyword("_SPECGLOSSMAP");
				material.SetTexture("_SpecGlossMap", texture2D3);
			}
			else
			{
				material.EnableKeyword("_METALLICGLOSSMAP");
				material.SetTexture("_MetallicGlossMap", texture2D3);
			}
		}
		if (md.hasReflectionTex)
		{
			if (md.overallAlpha < 1f)
			{
				Color white = Color.white;
				white.a = md.overallAlpha;
				material.SetColor("_Color", white);
				mode = ModelUtil.MtlBlendMode.FADE;
			}
			if (md.specularTex != null)
			{
				material.SetFloat("_Metallic", num3);
			}
			material.SetFloat("_Glossiness", 1f);
		}
		ModelUtil.SetupMaterialWithBlendMode(material, mode);
		return material;
	}

	private bool Using32bitIndices()
	{
		if (buildOptions != null && !buildOptions.use32bitIndices)
		{
			return false;
		}
		return true;
	}
}
