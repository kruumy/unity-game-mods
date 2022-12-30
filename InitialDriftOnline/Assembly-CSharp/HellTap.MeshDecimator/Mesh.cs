using System;
using System.Collections.Generic;
using HellTap.MeshDecimator.Math;

namespace HellTap.MeshDecimator;

public sealed class Mesh
{
	public const int UVChannelCount = 4;

	private Vector3d[] vertices;

	private int[][] indices;

	private Vector3[] normals;

	private Vector4[] tangents;

	private Vector2[][] uvs2D;

	private Vector3[][] uvs3D;

	private Vector4[][] uvs4D;

	private Vector4[] colors;

	private BoneWeight[] boneWeights;

	private static readonly int[] emptyIndices = new int[0];

	public int VertexCount => vertices.Length;

	public int SubMeshCount
	{
		get
		{
			return indices.Length;
		}
		set
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			int[][] array = new int[value][];
			Array.Copy(indices, 0, array, 0, MathHelper.Min(indices.Length, array.Length));
			indices = array;
		}
	}

	public int TriangleCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < indices.Length; i++)
			{
				if (indices[i] != null)
				{
					num += indices[i].Length / 3;
				}
			}
			return num;
		}
	}

	public Vector3d[] Vertices
	{
		get
		{
			return vertices;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			vertices = value;
			ClearVertexAttributes();
		}
	}

	public int[] Indices
	{
		get
		{
			if (indices.Length == 1)
			{
				return indices[0] ?? emptyIndices;
			}
			List<int> list = new List<int>(TriangleCount * 3);
			for (int i = 0; i < indices.Length; i++)
			{
				if (indices[i] != null)
				{
					list.AddRange(indices[i]);
				}
			}
			return list.ToArray();
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (value.Length % 3 != 0)
			{
				throw new ArgumentException("The index count must be multiple by 3.", "value");
			}
			SubMeshCount = 1;
			SetIndices(0, value);
		}
	}

	public Vector3[] Normals
	{
		get
		{
			return normals;
		}
		set
		{
			if (value != null && value.Length != vertices.Length)
			{
				throw new ArgumentException($"The vertex normals must be as many as the vertices. Assigned: {value.Length}  Require: {vertices.Length}");
			}
			normals = value;
		}
	}

	public Vector4[] Tangents
	{
		get
		{
			return tangents;
		}
		set
		{
			if (value != null && value.Length != vertices.Length)
			{
				throw new ArgumentException($"The vertex tangents must be as many as the vertices. Assigned: {value.Length}  Require: {vertices.Length}");
			}
			tangents = value;
		}
	}

	public Vector2[] UV1
	{
		get
		{
			return GetUVs2D(0);
		}
		set
		{
			SetUVs(0, value);
		}
	}

	public Vector2[] UV2
	{
		get
		{
			return GetUVs2D(1);
		}
		set
		{
			SetUVs(1, value);
		}
	}

	public Vector2[] UV3
	{
		get
		{
			return GetUVs2D(2);
		}
		set
		{
			SetUVs(2, value);
		}
	}

	public Vector2[] UV4
	{
		get
		{
			return GetUVs2D(3);
		}
		set
		{
			SetUVs(3, value);
		}
	}

	public Vector4[] Colors
	{
		get
		{
			return colors;
		}
		set
		{
			if (value != null && value.Length != vertices.Length)
			{
				throw new ArgumentException($"The vertex colors must be as many as the vertices. Assigned: {value.Length}  Require: {vertices.Length}");
			}
			colors = value;
		}
	}

	public BoneWeight[] BoneWeights
	{
		get
		{
			return boneWeights;
		}
		set
		{
			if (value != null && value.Length != vertices.Length)
			{
				throw new ArgumentException($"The vertex bone weights must be as many as the vertices. Assigned: {value.Length}  Require: {vertices.Length}");
			}
			boneWeights = value;
		}
	}

	public Mesh(Vector3d[] vertices, int[] indices)
	{
		if (vertices == null)
		{
			throw new ArgumentNullException("vertices");
		}
		if (indices == null)
		{
			throw new ArgumentNullException("indices");
		}
		if (indices.Length % 3 != 0)
		{
			throw new ArgumentException("The index count must be multiple by 3.", "indices");
		}
		this.vertices = vertices;
		this.indices = new int[1][];
		this.indices[0] = indices;
	}

	public Mesh(Vector3d[] vertices, int[][] indices)
	{
		if (vertices == null)
		{
			throw new ArgumentNullException("vertices");
		}
		if (indices == null)
		{
			throw new ArgumentNullException("indices");
		}
		for (int i = 0; i < indices.Length; i++)
		{
			if (indices[i] != null && indices[i].Length % 3 != 0)
			{
				throw new ArgumentException($"The index count must be multiple by 3 at sub-mesh index {i}.", "indices");
			}
		}
		this.vertices = vertices;
		this.indices = indices;
	}

	private void ClearVertexAttributes()
	{
		normals = null;
		tangents = null;
		uvs2D = null;
		uvs3D = null;
		uvs4D = null;
		colors = null;
		boneWeights = null;
	}

	public void RecalculateNormals()
	{
		int num = vertices.Length;
		Vector3[] array = new Vector3[num];
		int num2 = indices.Length;
		for (int i = 0; i < num2; i++)
		{
			int[] array2 = indices[i];
			if (array2 != null)
			{
				int num3 = array2.Length;
				for (int j = 0; j < num3; j += 3)
				{
					int num4 = array2[j];
					int num5 = array2[j + 1];
					int num6 = array2[j + 2];
					Vector3 vector = (Vector3)vertices[num4];
					Vector3 vector2 = (Vector3)vertices[num5];
					Vector3 vector3 = (Vector3)vertices[num6];
					Vector3 lhs = vector2 - vector;
					Vector3 rhs = vector3 - vector;
					Vector3.Cross(ref lhs, ref rhs, out var result);
					result.Normalize();
					array[num4] += result;
					array[num5] += result;
					array[num6] += result;
				}
			}
		}
		for (int k = 0; k < num; k++)
		{
			array[k].Normalize();
		}
		normals = array;
	}

	public void RecalculateTangents()
	{
		if (normals == null)
		{
			return;
		}
		bool flag = uvs2D != null && uvs2D[0] != null;
		bool flag2 = uvs3D != null && uvs3D[0] != null;
		bool flag3 = uvs4D != null && uvs4D[0] != null;
		if (!flag && !flag2 && !flag3)
		{
			return;
		}
		int num = vertices.Length;
		Vector4[] array = new Vector4[num];
		Vector3[] array2 = new Vector3[num];
		Vector3[] array3 = new Vector3[num];
		Vector2[] array4 = (flag ? uvs2D[0] : null);
		Vector3[] array5 = (flag2 ? uvs3D[0] : null);
		Vector4[] array6 = (flag3 ? uvs4D[0] : null);
		int num2 = indices.Length;
		for (int i = 0; i < num2; i++)
		{
			int[] array7 = indices[i];
			if (array7 == null)
			{
				continue;
			}
			int num3 = array7.Length;
			for (int j = 0; j < num3; j += 3)
			{
				int num4 = array7[j];
				int num5 = array7[j + 1];
				int num6 = array7[j + 2];
				Vector3d vector3d = vertices[num4];
				Vector3d vector3d2 = vertices[num5];
				Vector3d vector3d3 = vertices[num6];
				float num7;
				float num8;
				float num9;
				float num10;
				if (flag)
				{
					Vector2 vector = array4[num4];
					Vector2 vector2 = array4[num5];
					Vector2 vector3 = array4[num6];
					num7 = vector2.x - vector.x;
					num8 = vector3.x - vector.x;
					num9 = vector2.y - vector.y;
					num10 = vector3.y - vector.y;
				}
				else if (flag2)
				{
					Vector3 vector4 = array5[num4];
					Vector3 vector5 = array5[num5];
					Vector3 vector6 = array5[num6];
					num7 = vector5.x - vector4.x;
					num8 = vector6.x - vector4.x;
					num9 = vector5.y - vector4.y;
					num10 = vector6.y - vector4.y;
				}
				else
				{
					Vector4 vector7 = array6[num4];
					Vector4 vector8 = array6[num5];
					Vector4 vector9 = array6[num6];
					num7 = vector8.x - vector7.x;
					num8 = vector9.x - vector7.x;
					num9 = vector8.y - vector7.y;
					num10 = vector9.y - vector7.y;
				}
				float num11 = (float)(vector3d2.x - vector3d.x);
				float num12 = (float)(vector3d3.x - vector3d.x);
				float num13 = (float)(vector3d2.y - vector3d.y);
				float num14 = (float)(vector3d3.y - vector3d.y);
				float num15 = (float)(vector3d2.z - vector3d.z);
				float num16 = (float)(vector3d3.z - vector3d.z);
				float num17 = 1f / (num7 * num10 - num8 * num9);
				Vector3 vector10 = new Vector3((num10 * num11 - num9 * num12) * num17, (num10 * num13 - num9 * num14) * num17, (num10 * num15 - num9 * num16) * num17);
				Vector3 vector11 = new Vector3((num7 * num12 - num8 * num11) * num17, (num7 * num14 - num8 * num13) * num17, (num7 * num16 - num8 * num15) * num17);
				array2[num4] += vector10;
				array2[num5] += vector10;
				array2[num6] += vector10;
				array3[num4] += vector11;
				array3[num5] += vector11;
				array3[num6] += vector11;
			}
		}
		for (int k = 0; k < num; k++)
		{
			Vector3 lhs = normals[k];
			Vector3 rhs = array2[k];
			Vector3 vector12 = rhs - lhs * Vector3.Dot(ref lhs, ref rhs);
			vector12.Normalize();
			Vector3.Cross(ref lhs, ref rhs, out var result);
			float w = ((Vector3.Dot(ref result, ref array3[k]) < 0f) ? (-1f) : 1f);
			array[k] = new Vector4(vector12.x, vector12.y, vector12.z, w);
		}
		tangents = array;
	}

	public int GetTriangleCount(int subMeshIndex)
	{
		if (subMeshIndex < 0 || subMeshIndex >= indices.Length)
		{
			throw new IndexOutOfRangeException();
		}
		return indices[subMeshIndex].Length / 3;
	}

	public int[] GetIndices(int subMeshIndex)
	{
		if (subMeshIndex < 0 || subMeshIndex >= indices.Length)
		{
			throw new IndexOutOfRangeException();
		}
		return indices[subMeshIndex] ?? emptyIndices;
	}

	public void SetIndices(int subMeshIndex, int[] indices)
	{
		if (subMeshIndex < 0 || subMeshIndex >= this.indices.Length)
		{
			throw new IndexOutOfRangeException();
		}
		if (indices == null)
		{
			throw new ArgumentNullException("indices");
		}
		if (indices.Length % 3 != 0)
		{
			throw new ArgumentException("The index count must be multiple by 3.", "indices");
		}
		this.indices[subMeshIndex] = indices;
	}

	public int GetUVDimension(int channel)
	{
		if (channel < 0 || channel >= 4)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs2D != null && uvs2D[channel] != null)
		{
			return 2;
		}
		if (uvs3D != null && uvs3D[channel] != null)
		{
			return 3;
		}
		if (uvs4D != null && uvs4D[channel] != null)
		{
			return 4;
		}
		return 0;
	}

	public Vector2[] GetUVs2D(int channel)
	{
		if (channel < 0 || channel >= 4)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs2D != null && uvs2D[channel] != null)
		{
			return uvs2D[channel];
		}
		return null;
	}

	public Vector3[] GetUVs3D(int channel)
	{
		if (channel < 0 || channel >= 4)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs3D != null && uvs3D[channel] != null)
		{
			return uvs3D[channel];
		}
		return null;
	}

	public Vector4[] GetUVs4D(int channel)
	{
		if (channel < 0 || channel >= 4)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs4D != null && uvs4D[channel] != null)
		{
			return uvs4D[channel];
		}
		return null;
	}

	public void GetUVs(int channel, List<Vector2> uvs)
	{
		if (channel < 0 || channel >= 4)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs == null)
		{
			throw new ArgumentNullException("uvs");
		}
		uvs.Clear();
		if (uvs2D != null && uvs2D[channel] != null)
		{
			Vector2[] array = uvs2D[channel];
			if (array != null)
			{
				uvs.AddRange(array);
			}
		}
	}

	public void GetUVs(int channel, List<Vector3> uvs)
	{
		if (channel < 0 || channel >= 4)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs == null)
		{
			throw new ArgumentNullException("uvs");
		}
		uvs.Clear();
		if (uvs3D != null && uvs3D[channel] != null)
		{
			Vector3[] array = uvs3D[channel];
			if (array != null)
			{
				uvs.AddRange(array);
			}
		}
	}

	public void GetUVs(int channel, List<Vector4> uvs)
	{
		if (channel < 0 || channel >= 4)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs == null)
		{
			throw new ArgumentNullException("uvs");
		}
		uvs.Clear();
		if (uvs4D != null && uvs4D[channel] != null)
		{
			Vector4[] array = uvs4D[channel];
			if (array != null)
			{
				uvs.AddRange(array);
			}
		}
	}

	public void SetUVs(int channel, Vector2[] uvs)
	{
		if (channel < 0 || channel >= 4)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs != null && uvs.Length != 0)
		{
			if (uvs.Length != vertices.Length)
			{
				throw new ArgumentException($"The vertex UVs must be as many as the vertices. Assigned: {uvs.Length}  Require: {vertices.Length}");
			}
			if (uvs2D == null)
			{
				uvs2D = new Vector2[4][];
			}
			Vector2[] array = new Vector2[uvs.Length];
			uvs2D[channel] = array;
			uvs.CopyTo(array, 0);
		}
		else if (uvs2D != null)
		{
			uvs2D[channel] = null;
		}
		if (uvs3D != null)
		{
			uvs3D[channel] = null;
		}
		if (uvs4D != null)
		{
			uvs4D[channel] = null;
		}
	}

	public void SetUVs(int channel, Vector3[] uvs)
	{
		if (channel < 0 || channel >= 4)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs != null && uvs.Length != 0)
		{
			int num = uvs.Length;
			if (num != vertices.Length)
			{
				throw new ArgumentException($"The vertex UVs must be as many as the vertices. Assigned: {num}  Require: {vertices.Length}", "uvs");
			}
			if (uvs3D == null)
			{
				uvs3D = new Vector3[4][];
			}
			Vector3[] array = new Vector3[num];
			uvs3D[channel] = array;
			uvs.CopyTo(array, 0);
		}
		else if (uvs3D != null)
		{
			uvs3D[channel] = null;
		}
		if (uvs2D != null)
		{
			uvs2D[channel] = null;
		}
		if (uvs4D != null)
		{
			uvs4D[channel] = null;
		}
	}

	public void SetUVs(int channel, Vector4[] uvs)
	{
		if (channel < 0 || channel >= 4)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs != null && uvs.Length != 0)
		{
			int num = uvs.Length;
			if (num != vertices.Length)
			{
				throw new ArgumentException($"The vertex UVs must be as many as the vertices. Assigned: {num}  Require: {vertices.Length}", "uvs");
			}
			if (uvs4D == null)
			{
				uvs4D = new Vector4[4][];
			}
			Vector4[] array = new Vector4[num];
			uvs4D[channel] = array;
			uvs.CopyTo(array, 0);
		}
		else if (uvs4D != null)
		{
			uvs4D[channel] = null;
		}
		if (uvs2D != null)
		{
			uvs2D[channel] = null;
		}
		if (uvs3D != null)
		{
			uvs3D[channel] = null;
		}
	}

	public void SetUVs(int channel, List<Vector2> uvs)
	{
		if (channel < 0 || channel >= 4)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs != null && uvs.Count > 0)
		{
			int count = uvs.Count;
			if (count != vertices.Length)
			{
				throw new ArgumentException($"The vertex UVs must be as many as the vertices. Assigned: {count}  Require: {vertices.Length}", "uvs");
			}
			if (uvs2D == null)
			{
				uvs2D = new Vector2[4][];
			}
			Vector2[] array = new Vector2[count];
			uvs2D[channel] = array;
			uvs.CopyTo(array, 0);
		}
		else if (uvs2D != null)
		{
			uvs2D[channel] = null;
		}
		if (uvs3D != null)
		{
			uvs3D[channel] = null;
		}
		if (uvs4D != null)
		{
			uvs4D[channel] = null;
		}
	}

	public void SetUVs(int channel, List<Vector3> uvs)
	{
		if (channel < 0 || channel >= 4)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs != null && uvs.Count > 0)
		{
			int count = uvs.Count;
			if (count != vertices.Length)
			{
				throw new ArgumentException($"The vertex UVs must be as many as the vertices. Assigned: {count}  Require: {vertices.Length}", "uvs");
			}
			if (uvs3D == null)
			{
				uvs3D = new Vector3[4][];
			}
			Vector3[] array = new Vector3[count];
			uvs3D[channel] = array;
			uvs.CopyTo(array, 0);
		}
		else if (uvs3D != null)
		{
			uvs3D[channel] = null;
		}
		if (uvs2D != null)
		{
			uvs2D[channel] = null;
		}
		if (uvs4D != null)
		{
			uvs4D[channel] = null;
		}
	}

	public void SetUVs(int channel, List<Vector4> uvs)
	{
		if (channel < 0 || channel >= 4)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs != null && uvs.Count > 0)
		{
			int count = uvs.Count;
			if (count != vertices.Length)
			{
				throw new ArgumentException($"The vertex UVs must be as many as the vertices. Assigned: {count}  Require: {vertices.Length}", "uvs");
			}
			if (uvs4D == null)
			{
				uvs4D = new Vector4[4][];
			}
			Vector4[] array = new Vector4[count];
			uvs4D[channel] = array;
			uvs.CopyTo(array, 0);
		}
		else if (uvs4D != null)
		{
			uvs4D[channel] = null;
		}
		if (uvs2D != null)
		{
			uvs2D[channel] = null;
		}
		if (uvs3D != null)
		{
			uvs3D[channel] = null;
		}
	}

	public override string ToString()
	{
		return $"Vertices: {vertices.Length}";
	}
}
