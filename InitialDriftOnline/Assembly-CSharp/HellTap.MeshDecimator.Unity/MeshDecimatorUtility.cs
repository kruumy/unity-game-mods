using System;
using System.Collections.Generic;
using HellTap.MeshDecimator.Algorithms;
using HellTap.MeshDecimator.Loggers;
using HellTap.MeshDecimator.Math;
using HellTap.MeshDecimator.Unity.Loggers;
using UnityEngine;

namespace HellTap.MeshDecimator.Unity;

public static class MeshDecimatorUtility
{
	static MeshDecimatorUtility()
	{
		if (Logging.Logger == null || Logging.Logger is ConsoleLogger)
		{
			Logging.Logger = new UnityLogger();
		}
	}

	private static Vector3d[] ToSimplifyVertices(UnityEngine.Vector3[] vertices)
	{
		Vector3d[] array = new Vector3d[vertices.Length];
		for (int i = 0; i < vertices.Length; i++)
		{
			UnityEngine.Vector3 vector = vertices[i];
			array[i] = new Vector3d(vector.x, vector.y, vector.z);
		}
		return array;
	}

	private static HellTap.MeshDecimator.Math.Vector2[] ToSimplifyVec(UnityEngine.Vector2[] vectors)
	{
		if (vectors == null)
		{
			return null;
		}
		HellTap.MeshDecimator.Math.Vector2[] array = new HellTap.MeshDecimator.Math.Vector2[vectors.Length];
		for (int i = 0; i < vectors.Length; i++)
		{
			UnityEngine.Vector2 vector = vectors[i];
			array[i] = new HellTap.MeshDecimator.Math.Vector2(vector.x, vector.y);
		}
		return array;
	}

	private static HellTap.MeshDecimator.Math.Vector3[] ToSimplifyVec(UnityEngine.Vector3[] vectors)
	{
		if (vectors == null)
		{
			return null;
		}
		HellTap.MeshDecimator.Math.Vector3[] array = new HellTap.MeshDecimator.Math.Vector3[vectors.Length];
		for (int i = 0; i < vectors.Length; i++)
		{
			UnityEngine.Vector3 vector = vectors[i];
			array[i] = new HellTap.MeshDecimator.Math.Vector3(vector.x, vector.y, vector.z);
		}
		return array;
	}

	private static HellTap.MeshDecimator.Math.Vector4[] ToSimplifyVec(UnityEngine.Vector4[] vectors)
	{
		if (vectors == null)
		{
			return null;
		}
		HellTap.MeshDecimator.Math.Vector4[] array = new HellTap.MeshDecimator.Math.Vector4[vectors.Length];
		for (int i = 0; i < vectors.Length; i++)
		{
			UnityEngine.Vector4 vector = vectors[i];
			array[i] = new HellTap.MeshDecimator.Math.Vector4(vector.x, vector.y, vector.z, vector.w);
		}
		return array;
	}

	private static HellTap.MeshDecimator.Math.Vector4[] ToSimplifyVec(Color[] colors)
	{
		if (colors == null)
		{
			return null;
		}
		HellTap.MeshDecimator.Math.Vector4[] array = new HellTap.MeshDecimator.Math.Vector4[colors.Length];
		for (int i = 0; i < colors.Length; i++)
		{
			Color color = colors[i];
			array[i] = new HellTap.MeshDecimator.Math.Vector4(color.r, color.g, color.b, color.a);
		}
		return array;
	}

	private static BoneWeight[] ToSimplifyBoneWeights(UnityEngine.BoneWeight[] boneWeights)
	{
		if (boneWeights == null)
		{
			return null;
		}
		BoneWeight[] array = new BoneWeight[boneWeights.Length];
		for (int i = 0; i < boneWeights.Length; i++)
		{
			UnityEngine.BoneWeight boneWeight = boneWeights[i];
			array[i] = new BoneWeight(boneWeight.boneIndex0, boneWeight.boneIndex1, boneWeight.boneIndex2, boneWeight.boneIndex3, boneWeight.weight0, boneWeight.weight1, boneWeight.weight2, boneWeight.weight3);
		}
		return array;
	}

	private static UnityEngine.Vector3[] FromSimplifyVertices(Vector3d[] vertices)
	{
		UnityEngine.Vector3[] array = new UnityEngine.Vector3[vertices.Length];
		for (int i = 0; i < vertices.Length; i++)
		{
			Vector3d vector3d = vertices[i];
			array[i] = new UnityEngine.Vector3((float)vector3d.x, (float)vector3d.y, (float)vector3d.z);
		}
		return array;
	}

	private static UnityEngine.Vector2[] FromSimplifyVec(HellTap.MeshDecimator.Math.Vector2[] vectors)
	{
		if (vectors == null)
		{
			return null;
		}
		UnityEngine.Vector2[] array = new UnityEngine.Vector2[vectors.Length];
		for (int i = 0; i < vectors.Length; i++)
		{
			HellTap.MeshDecimator.Math.Vector2 vector = vectors[i];
			array[i] = new UnityEngine.Vector2(vector.x, vector.y);
		}
		return array;
	}

	private static UnityEngine.Vector3[] FromSimplifyVec(HellTap.MeshDecimator.Math.Vector3[] vectors)
	{
		if (vectors == null)
		{
			return null;
		}
		UnityEngine.Vector3[] array = new UnityEngine.Vector3[vectors.Length];
		for (int i = 0; i < vectors.Length; i++)
		{
			HellTap.MeshDecimator.Math.Vector3 vector = vectors[i];
			array[i] = new UnityEngine.Vector3(vector.x, vector.y, vector.z);
		}
		return array;
	}

	private static UnityEngine.Vector4[] FromSimplifyVec(HellTap.MeshDecimator.Math.Vector4[] vectors)
	{
		if (vectors == null)
		{
			return null;
		}
		UnityEngine.Vector4[] array = new UnityEngine.Vector4[vectors.Length];
		for (int i = 0; i < vectors.Length; i++)
		{
			HellTap.MeshDecimator.Math.Vector4 vector = vectors[i];
			array[i] = new UnityEngine.Vector4(vector.x, vector.y, vector.z, vector.w);
		}
		return array;
	}

	private static Color[] FromSimplifyColor(HellTap.MeshDecimator.Math.Vector4[] vectors)
	{
		if (vectors == null)
		{
			return null;
		}
		Color[] array = new Color[vectors.Length];
		for (int i = 0; i < vectors.Length; i++)
		{
			HellTap.MeshDecimator.Math.Vector4 vector = vectors[i];
			array[i] = new Color(vector.x, vector.y, vector.z, vector.w);
		}
		return array;
	}

	private static UnityEngine.BoneWeight[] FromSimplifyBoneWeights(BoneWeight[] boneWeights)
	{
		if (boneWeights == null)
		{
			return null;
		}
		UnityEngine.BoneWeight[] array = new UnityEngine.BoneWeight[boneWeights.Length];
		for (int i = 0; i < boneWeights.Length; i++)
		{
			BoneWeight boneWeight = boneWeights[i];
			array[i] = new UnityEngine.BoneWeight
			{
				boneIndex0 = boneWeight.boneIndex0,
				boneIndex1 = boneWeight.boneIndex1,
				boneIndex2 = boneWeight.boneIndex2,
				boneIndex3 = boneWeight.boneIndex3,
				weight0 = boneWeight.boneWeight0,
				weight1 = boneWeight.boneWeight1,
				weight2 = boneWeight.boneWeight2,
				weight3 = boneWeight.boneWeight3
			};
		}
		return array;
	}

	private static void AddToList(List<Vector3d> list, UnityEngine.Vector3[] arr, int previousVertexCount, int totalVertexCount)
	{
		if (arr != null && arr.Length != 0)
		{
			for (int i = 0; i < arr.Length; i++)
			{
				UnityEngine.Vector3 vector = arr[i];
				list.Add(new Vector3d(vector.x, vector.y, vector.z));
			}
		}
	}

	private static void AddToList(ref List<HellTap.MeshDecimator.Math.Vector2> list, UnityEngine.Vector2[] arr, int previousVertexCount, int currentVertexCount, int totalVertexCount, HellTap.MeshDecimator.Math.Vector2 defaultValue)
	{
		if (arr == null || arr.Length == 0)
		{
			if (list != null)
			{
				for (int i = 0; i < currentVertexCount; i++)
				{
					list.Add(defaultValue);
				}
			}
			return;
		}
		if (list == null)
		{
			list = new List<HellTap.MeshDecimator.Math.Vector2>(totalVertexCount);
			for (int j = 0; j < previousVertexCount; j++)
			{
				list.Add(defaultValue);
			}
		}
		for (int k = 0; k < arr.Length; k++)
		{
			UnityEngine.Vector2 vector = arr[k];
			list.Add(new HellTap.MeshDecimator.Math.Vector2(vector.x, vector.y));
		}
	}

	private static void AddToList(ref List<HellTap.MeshDecimator.Math.Vector3> list, UnityEngine.Vector3[] arr, int previousVertexCount, int currentVertexCount, int totalVertexCount, HellTap.MeshDecimator.Math.Vector3 defaultValue)
	{
		if (arr == null || arr.Length == 0)
		{
			if (list != null)
			{
				for (int i = 0; i < currentVertexCount; i++)
				{
					list.Add(defaultValue);
				}
			}
			return;
		}
		if (list == null)
		{
			list = new List<HellTap.MeshDecimator.Math.Vector3>(totalVertexCount);
			for (int j = 0; j < previousVertexCount; j++)
			{
				list.Add(defaultValue);
			}
		}
		for (int k = 0; k < arr.Length; k++)
		{
			UnityEngine.Vector3 vector = arr[k];
			list.Add(new HellTap.MeshDecimator.Math.Vector3(vector.x, vector.y, vector.z));
		}
	}

	private static void AddToList(ref List<HellTap.MeshDecimator.Math.Vector4> list, UnityEngine.Vector4[] arr, int previousVertexCount, int currentVertexCount, int totalVertexCount, HellTap.MeshDecimator.Math.Vector4 defaultValue)
	{
		if (arr == null || arr.Length == 0)
		{
			if (list != null)
			{
				for (int i = 0; i < currentVertexCount; i++)
				{
					list.Add(defaultValue);
				}
			}
			return;
		}
		if (list == null)
		{
			list = new List<HellTap.MeshDecimator.Math.Vector4>(totalVertexCount);
			for (int j = 0; j < previousVertexCount; j++)
			{
				list.Add(defaultValue);
			}
		}
		for (int k = 0; k < arr.Length; k++)
		{
			UnityEngine.Vector4 vector = arr[k];
			list.Add(new HellTap.MeshDecimator.Math.Vector4(vector.x, vector.y, vector.z, vector.w));
		}
	}

	private static void AddToList(ref List<HellTap.MeshDecimator.Math.Vector4> list, Color[] arr, int previousVertexCount, int currentVertexCount, int totalVertexCount)
	{
		if (arr == null || arr.Length == 0)
		{
			if (list != null)
			{
				for (int i = 0; i < currentVertexCount; i++)
				{
					list.Add(default(HellTap.MeshDecimator.Math.Vector4));
				}
			}
			return;
		}
		if (list == null)
		{
			list = new List<HellTap.MeshDecimator.Math.Vector4>(totalVertexCount);
			for (int j = 0; j < previousVertexCount; j++)
			{
				list.Add(default(HellTap.MeshDecimator.Math.Vector4));
			}
		}
		for (int k = 0; k < arr.Length; k++)
		{
			Color color = arr[k];
			list.Add(new HellTap.MeshDecimator.Math.Vector4(color.r, color.g, color.b, color.a));
		}
	}

	private static void AddToList(ref List<BoneWeight> list, UnityEngine.BoneWeight[] arr, int previousVertexCount, int currentVertexCount, int totalVertexCount)
	{
		if (arr == null || arr.Length == 0)
		{
			if (list != null)
			{
				for (int i = 0; i < currentVertexCount; i++)
				{
					list.Add(default(BoneWeight));
				}
			}
			return;
		}
		if (list == null)
		{
			list = new List<BoneWeight>(totalVertexCount);
			for (int j = 0; j < previousVertexCount; j++)
			{
				list.Add(default(BoneWeight));
			}
		}
		for (int k = 0; k < arr.Length; k++)
		{
			UnityEngine.BoneWeight boneWeight = arr[k];
			list.Add(new BoneWeight(boneWeight.boneIndex0, boneWeight.boneIndex1, boneWeight.boneIndex2, boneWeight.boneIndex3, boneWeight.weight0, boneWeight.weight1, boneWeight.weight2, boneWeight.weight3));
		}
	}

	private static void TransformVertices(UnityEngine.Vector3[] vertices, ref Matrix4x4 transform)
	{
		for (int i = 0; i < vertices.Length; i++)
		{
			vertices[i] = transform.MultiplyPoint3x4(vertices[i]);
		}
	}

	private static void TransformVertices(UnityEngine.Vector3[] vertices, UnityEngine.BoneWeight[] boneWeights, Matrix4x4[] oldBindposes, Matrix4x4[] newBindposes)
	{
		for (int i = 0; i < oldBindposes.Length; i++)
		{
			oldBindposes[i] = oldBindposes[i].inverse;
		}
		for (int j = 0; j < vertices.Length; j++)
		{
			UnityEngine.Vector3 vector = vertices[j];
			UnityEngine.BoneWeight boneWeight = boneWeights[j];
			if (boneWeight.weight0 > 0f)
			{
				int boneIndex = boneWeight.boneIndex0;
				float weight = boneWeight.weight0;
				vector = ScaleMatrix(ref newBindposes[boneIndex], weight) * (ScaleMatrix(ref oldBindposes[boneIndex], weight) * vector);
			}
			if (boneWeight.weight1 > 0f)
			{
				int boneIndex2 = boneWeight.boneIndex1;
				float weight2 = boneWeight.weight1;
				vector = ScaleMatrix(ref newBindposes[boneIndex2], weight2) * (ScaleMatrix(ref oldBindposes[boneIndex2], weight2) * vector);
			}
			if (boneWeight.weight2 > 0f)
			{
				int boneIndex3 = boneWeight.boneIndex2;
				float weight3 = boneWeight.weight2;
				vector = ScaleMatrix(ref newBindposes[boneIndex3], weight3) * (ScaleMatrix(ref oldBindposes[boneIndex3], weight3) * vector);
			}
			if (boneWeight.weight3 > 0f)
			{
				int boneIndex4 = boneWeight.boneIndex3;
				float weight4 = boneWeight.weight3;
				vector = ScaleMatrix(ref newBindposes[boneIndex4], weight4) * (ScaleMatrix(ref oldBindposes[boneIndex4], weight4) * vector);
			}
			vertices[j] = vector;
		}
	}

	private static Matrix4x4 ScaleMatrix(ref Matrix4x4 m, float scale)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.m00 = m.m00 * scale;
		result.m01 = m.m01 * scale;
		result.m02 = m.m02 * scale;
		result.m03 = m.m03 * scale;
		result.m10 = m.m10 * scale;
		result.m11 = m.m11 * scale;
		result.m12 = m.m12 * scale;
		result.m13 = m.m13 * scale;
		result.m20 = m.m20 * scale;
		result.m21 = m.m21 * scale;
		result.m22 = m.m22 * scale;
		result.m23 = m.m23 * scale;
		result.m30 = m.m30 * scale;
		result.m31 = m.m31 * scale;
		result.m32 = m.m32 * scale;
		result.m33 = m.m33 * scale;
		return result;
	}

	private static T[] MergeArrays<T>(T[] arr1, T[] arr2)
	{
		T[] array = new T[arr1.Length + arr2.Length];
		Array.Copy(arr1, 0, array, 0, arr1.Length);
		Array.Copy(arr2, 0, array, arr1.Length, arr2.Length);
		return array;
	}

	private static void RemapBones(UnityEngine.BoneWeight[] boneWeights, int[] boneIndices)
	{
		for (int i = 0; i < boneWeights.Length; i++)
		{
			UnityEngine.BoneWeight boneWeight = boneWeights[i];
			if (boneWeight.weight0 > 0f)
			{
				boneWeight.boneIndex0 = boneIndices[boneWeight.boneIndex0];
			}
			if (boneWeight.weight1 > 0f)
			{
				boneWeight.boneIndex1 = boneIndices[boneWeight.boneIndex1];
			}
			if (boneWeight.weight2 > 0f)
			{
				boneWeight.boneIndex2 = boneIndices[boneWeight.boneIndex2];
			}
			if (boneWeight.weight3 > 0f)
			{
				boneWeight.boneIndex3 = boneIndices[boneWeight.boneIndex3];
			}
			boneWeights[i] = boneWeight;
		}
	}

	private static UnityEngine.Mesh CreateMesh(Matrix4x4[] bindposes, UnityEngine.Vector3[] vertices, Mesh destMesh, bool recalculateNormals)
	{
		if (recalculateNormals)
		{
			destMesh.RecalculateNormals();
			destMesh.RecalculateTangents();
		}
		int subMeshCount = destMesh.SubMeshCount;
		UnityEngine.Vector3[] array = FromSimplifyVec(destMesh.Normals);
		UnityEngine.Vector4[] array2 = FromSimplifyVec(destMesh.Tangents);
		UnityEngine.Vector2[] array3 = FromSimplifyVec(destMesh.UV1);
		UnityEngine.Vector2[] array4 = FromSimplifyVec(destMesh.UV2);
		UnityEngine.Vector2[] array5 = FromSimplifyVec(destMesh.UV3);
		UnityEngine.Vector2[] array6 = FromSimplifyVec(destMesh.UV4);
		Color[] array7 = FromSimplifyColor(destMesh.Colors);
		UnityEngine.BoneWeight[] array8 = FromSimplifyBoneWeights(destMesh.BoneWeights);
		int num = 0;
		for (int i = 0; i < subMeshCount; i++)
		{
			int[] indices = destMesh.GetIndices(i);
			for (int j = 0; j < indices.Length; j++)
			{
				if (indices[j] > num)
				{
					num = indices[j];
				}
			}
		}
		UnityEngine.Mesh mesh = new UnityEngine.Mesh();
		if (bindposes != null)
		{
			mesh.bindposes = bindposes;
		}
		mesh.subMeshCount = subMeshCount;
		mesh.vertices = vertices;
		if (array != null)
		{
			mesh.normals = array;
		}
		if (array2 != null)
		{
			mesh.tangents = array2;
		}
		if (array3 != null)
		{
			mesh.uv = array3;
		}
		if (array4 != null)
		{
			mesh.uv2 = array4;
		}
		if (array5 != null)
		{
			mesh.uv3 = array5;
		}
		if (array6 != null)
		{
			mesh.uv4 = array6;
		}
		if (array7 != null)
		{
			mesh.colors = array7;
		}
		if (array8 != null)
		{
			mesh.boneWeights = array8;
		}
		for (int k = 0; k < subMeshCount; k++)
		{
			int[] indices2 = destMesh.GetIndices(k);
			mesh.SetTriangles(indices2, k);
		}
		mesh.RecalculateBounds();
		return mesh;
	}

	public static UnityEngine.Mesh DecimateMesh(UnityEngine.Mesh mesh, Matrix4x4 transform, float quality, bool recalculateNormals, DecimationAlgorithm.StatusReportCallback statusCallback = null, bool preserveBorders = false, bool preserveSeams = false, bool preserveFoldovers = false)
	{
		if (mesh == null)
		{
			throw new ArgumentNullException("mesh");
		}
		int subMeshCount = mesh.subMeshCount;
		UnityEngine.Vector3[] vertices = mesh.vertices;
		UnityEngine.Vector3[] normals = mesh.normals;
		UnityEngine.Vector4[] tangents = mesh.tangents;
		UnityEngine.Vector2[] uv = mesh.uv;
		UnityEngine.Vector2[] uv2 = mesh.uv2;
		UnityEngine.Vector2[] uv3 = mesh.uv3;
		UnityEngine.Vector2[] uv4 = mesh.uv4;
		Color[] colors = mesh.colors;
		UnityEngine.BoneWeight[] boneWeights = mesh.boneWeights;
		Matrix4x4[] bindposes = mesh.bindposes;
		int num = 0;
		int[][] array = new int[subMeshCount][];
		for (int i = 0; i < subMeshCount; i++)
		{
			array[i] = mesh.GetTriangles(i);
			num += array[i].Length / 3;
		}
		TransformVertices(vertices, ref transform);
		Vector3d[] vertices2 = ToSimplifyVertices(vertices);
		quality = Mathf.Clamp01(quality);
		int targetTriangleCount = Mathf.CeilToInt((float)num * quality);
		Mesh mesh2 = new Mesh(vertices2, array);
		if (normals != null && normals.Length != 0)
		{
			mesh2.Normals = ToSimplifyVec(normals);
		}
		if (tangents != null && tangents.Length != 0)
		{
			mesh2.Tangents = ToSimplifyVec(tangents);
		}
		if (uv != null && uv.Length != 0)
		{
			mesh2.UV1 = ToSimplifyVec(uv);
		}
		if (uv2 != null && uv2.Length != 0)
		{
			mesh2.UV2 = ToSimplifyVec(uv2);
		}
		if (uv3 != null && uv3.Length != 0)
		{
			mesh2.UV3 = ToSimplifyVec(uv3);
		}
		if (uv4 != null && uv4.Length != 0)
		{
			mesh2.UV4 = ToSimplifyVec(uv4);
		}
		if (colors != null && colors.Length != 0)
		{
			mesh2.Colors = ToSimplifyVec(colors);
		}
		if (boneWeights != null && boneWeights.Length != 0)
		{
			mesh2.BoneWeights = ToSimplifyBoneWeights(boneWeights);
		}
		DecimationAlgorithm decimationAlgorithm = MeshDecimation.CreateAlgorithm(Algorithm.Default, preserveBorders, preserveSeams, preserveFoldovers);
		decimationAlgorithm.MaxVertexCount = 65535;
		if (statusCallback != null)
		{
			decimationAlgorithm.StatusReport += statusCallback;
		}
		Mesh mesh3 = MeshDecimation.DecimateMesh(decimationAlgorithm, mesh2, targetTriangleCount);
		UnityEngine.Vector3[] vertices3 = FromSimplifyVertices(mesh3.Vertices);
		if (statusCallback != null)
		{
			decimationAlgorithm.StatusReport -= statusCallback;
		}
		return CreateMesh(bindposes, vertices3, mesh3, recalculateNormals);
	}

	public static UnityEngine.Mesh DecimateMeshes(UnityEngine.Mesh[] meshes, Matrix4x4[] transforms, Material[][] materials, float quality, bool recalculateNormals, out Material[] resultMaterials, DecimationAlgorithm.StatusReportCallback statusCallback = null, bool preserveBorders = false, bool preserveSeams = false, bool preserveFoldovers = false)
	{
		Transform[] mergedBones;
		return DecimateMeshes(meshes, transforms, materials, null, quality, recalculateNormals, out resultMaterials, out mergedBones, statusCallback, preserveBorders, preserveSeams, preserveFoldovers);
	}

	public static UnityEngine.Mesh DecimateMeshes(UnityEngine.Mesh[] meshes, Matrix4x4[] transforms, Material[][] materials, Transform[][] meshBones, float quality, bool recalculateNormals, out Material[] resultMaterials, out Transform[] mergedBones, DecimationAlgorithm.StatusReportCallback statusCallback = null, bool preserveBorders = false, bool preserveSeams = false, bool preserveFoldovers = false)
	{
		if (meshes == null)
		{
			throw new ArgumentNullException("meshes");
		}
		if (meshes.Length == 0)
		{
			throw new ArgumentException("You have to simplify at least one mesh.", "meshes");
		}
		if (transforms == null)
		{
			throw new ArgumentNullException("transforms");
		}
		if (transforms.Length != meshes.Length)
		{
			throw new ArgumentException("The array of transforms must match the length of the meshes array.", "transforms");
		}
		if (materials == null)
		{
			throw new ArgumentNullException("materials");
		}
		if (materials.Length != meshes.Length)
		{
			throw new ArgumentException("If materials are provided, the length of the array must match the length of the meshes array.", "materials");
		}
		if (meshBones != null && meshBones.Length != meshes.Length)
		{
			throw new ArgumentException("If mesh bones are provided, the length of the array must match the length of the meshes array.", "meshBones");
		}
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < meshes.Length; i++)
		{
			UnityEngine.Mesh mesh = meshes[i];
			num += mesh.vertexCount;
			num2 += mesh.subMeshCount;
			Material[] array = materials[i];
			if (array == null)
			{
				throw new ArgumentException($"The mesh materials for index {i} is null!", "materials");
			}
			if (array.Length != mesh.subMeshCount)
			{
				throw new ArgumentException($"The mesh materials at index {i} don't match the submesh count! ({array.Length} != {mesh.subMeshCount})", "materials");
			}
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] == null)
				{
					throw new ArgumentException($"The mesh material index {j} at material array index {i} is null!", "materials");
				}
			}
		}
		int num3 = 0;
		List<Vector3d> list = new List<Vector3d>(num);
		List<int[]> list2 = new List<int[]>(num2);
		List<HellTap.MeshDecimator.Math.Vector3> list3 = null;
		List<HellTap.MeshDecimator.Math.Vector4> list4 = null;
		List<HellTap.MeshDecimator.Math.Vector2> list5 = null;
		List<HellTap.MeshDecimator.Math.Vector2> list6 = null;
		List<HellTap.MeshDecimator.Math.Vector2> list7 = null;
		List<HellTap.MeshDecimator.Math.Vector2> list8 = null;
		List<HellTap.MeshDecimator.Math.Vector4> list9 = null;
		List<BoneWeight> list10 = null;
		List<Matrix4x4> list11 = null;
		List<Transform> list12 = null;
		List<Material> list13 = new List<Material>(num2);
		Dictionary<Material, int> dictionary = new Dictionary<Material, int>();
		int num4 = 0;
		for (int k = 0; k < meshes.Length; k++)
		{
			UnityEngine.Mesh mesh2 = meshes[k];
			Matrix4x4 transform = transforms[k];
			Material[] array2 = materials[k];
			Transform[] array3 = ((meshBones != null) ? meshBones[k] : null);
			int vertexCount = mesh2.vertexCount;
			UnityEngine.Vector3[] vertices = mesh2.vertices;
			UnityEngine.Vector3[] normals = mesh2.normals;
			UnityEngine.Vector4[] tangents = mesh2.tangents;
			UnityEngine.Vector2[] uv = mesh2.uv;
			UnityEngine.Vector2[] uv2 = mesh2.uv2;
			UnityEngine.Vector2[] uv3 = mesh2.uv3;
			UnityEngine.Vector2[] uv4 = mesh2.uv4;
			Color[] colors = mesh2.colors;
			UnityEngine.BoneWeight[] boneWeights = mesh2.boneWeights;
			Matrix4x4[] bindposes = mesh2.bindposes;
			if (array3 != null && boneWeights != null && boneWeights.Length != 0 && bindposes != null && bindposes.Length != 0 && array3.Length == bindposes.Length)
			{
				if (list11 == null)
				{
					list11 = new List<Matrix4x4>(bindposes);
					list12 = new List<Transform>(array3);
				}
				else
				{
					bool flag = false;
					int[] array4 = new int[array3.Length];
					for (int l = 0; l < array3.Length; l++)
					{
						int num5 = list12.IndexOf(array3[l]);
						if (num5 == -1)
						{
							num5 = list12.Count;
							list12.Add(array3[l]);
							list11.Add(bindposes[l]);
						}
						else if (bindposes[l] != list11[num5])
						{
							flag = true;
						}
						array4[l] = num5;
					}
					if (flag)
					{
						Matrix4x4[] array5 = new Matrix4x4[bindposes.Length];
						for (int m = 0; m < bindposes.Length; m++)
						{
							int index = array4[m];
							array5[m] = list11[index];
						}
						TransformVertices(vertices, boneWeights, bindposes, array5);
					}
					RemapBones(boneWeights, array4);
				}
			}
			TransformVertices(vertices, ref transform);
			AddToList(list, vertices, num4, num);
			AddToList(ref list3, normals, num4, vertexCount, num, new HellTap.MeshDecimator.Math.Vector3(1f, 0f, 0f));
			AddToList(ref list4, tangents, num4, vertexCount, num, new HellTap.MeshDecimator.Math.Vector4(0f, 0f, 1f, 1f));
			AddToList(ref list5, uv, num4, vertexCount, num, default(HellTap.MeshDecimator.Math.Vector2));
			AddToList(ref list6, uv2, num4, vertexCount, num, default(HellTap.MeshDecimator.Math.Vector2));
			AddToList(ref list7, uv3, num4, vertexCount, num, default(HellTap.MeshDecimator.Math.Vector2));
			AddToList(ref list8, uv4, num4, vertexCount, num, default(HellTap.MeshDecimator.Math.Vector2));
			AddToList(ref list9, colors, num4, vertexCount, num);
			AddToList(ref list10, boneWeights, num4, vertexCount, num);
			int subMeshCount = mesh2.subMeshCount;
			for (int n = 0; n < subMeshCount; n++)
			{
				Material material = array2[n];
				int[] triangles = mesh2.GetTriangles(n);
				num3 += triangles.Length / 3;
				if (num4 > 0)
				{
					for (int num6 = 0; num6 < triangles.Length; num6++)
					{
						triangles[num6] += num4;
					}
				}
				if (dictionary.TryGetValue(material, out var value))
				{
					int[] arr = list2[value];
					list2[value] = MergeArrays(arr, triangles);
					continue;
				}
				int count = list2.Count;
				dictionary.Add(material, count);
				list13.Add(material);
				list2.Add(triangles);
			}
			num4 += vertexCount;
		}
		quality = Mathf.Clamp01(quality);
		int targetTriangleCount = Mathf.CeilToInt((float)num3 * quality);
		Mesh mesh3 = new Mesh(list.ToArray(), list2.ToArray());
		if (list3 != null)
		{
			mesh3.Normals = list3.ToArray();
		}
		if (list4 != null)
		{
			mesh3.Tangents = list4.ToArray();
		}
		if (list5 != null)
		{
			mesh3.UV1 = list5.ToArray();
		}
		if (list6 != null)
		{
			mesh3.UV2 = list6.ToArray();
		}
		if (list7 != null)
		{
			mesh3.UV3 = list7.ToArray();
		}
		if (list8 != null)
		{
			mesh3.UV4 = list8.ToArray();
		}
		if (list9 != null)
		{
			mesh3.Colors = list9.ToArray();
		}
		if (list10 != null)
		{
			mesh3.BoneWeights = list10.ToArray();
		}
		DecimationAlgorithm decimationAlgorithm = MeshDecimation.CreateAlgorithm(Algorithm.Default);
		decimationAlgorithm.MaxVertexCount = 65535;
		if (statusCallback != null)
		{
			decimationAlgorithm.StatusReport += statusCallback;
		}
		Mesh mesh4 = MeshDecimation.DecimateMesh(decimationAlgorithm, mesh3, targetTriangleCount);
		UnityEngine.Vector3[] vertices2 = FromSimplifyVertices(mesh4.Vertices);
		if (statusCallback != null)
		{
			decimationAlgorithm.StatusReport -= statusCallback;
		}
		Matrix4x4[] bindposes2 = list11?.ToArray();
		resultMaterials = list13.ToArray();
		mergedBones = list12?.ToArray();
		return CreateMesh(bindposes2, vertices2, mesh4, recalculateNormals);
	}
}
