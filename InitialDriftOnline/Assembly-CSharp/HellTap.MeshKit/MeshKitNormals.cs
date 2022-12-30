using System;
using System.Collections.Generic;
using UnityEngine;

namespace HellTap.MeshKit;

public static class MeshKitNormals
{
	private struct VertexKey
	{
		private readonly long _x;

		private readonly long _y;

		private readonly long _z;

		private const int Tolerance = 100000;

		public VertexKey(Vector3 position)
		{
			_x = (long)Mathf.Round(position.x * 100000f);
			_y = (long)Mathf.Round(position.y * 100000f);
			_z = (long)Mathf.Round(position.z * 100000f);
		}

		public override bool Equals(object obj)
		{
			VertexKey vertexKey = (VertexKey)obj;
			if (_x == vertexKey._x && _y == vertexKey._y)
			{
				return _z == vertexKey._z;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return ((_x * 7) ^ (_y * 13) ^ (_z * 27)).GetHashCode();
		}
	}

	private sealed class VertexEntry
	{
		public int[] TriangleIndex = new int[4];

		public int[] VertexIndex = new int[4];

		private int _reserved = 4;

		private int _count;

		public int Count => _count;

		public void Add(int vertIndex, int triIndex)
		{
			if (_reserved == _count)
			{
				_reserved *= 2;
				Array.Resize(ref TriangleIndex, _reserved);
				Array.Resize(ref VertexIndex, _reserved);
			}
			TriangleIndex[_count] = triIndex;
			VertexIndex[_count] = vertIndex;
			_count++;
		}
	}

	public static void RecalculateNormalsBasedOnAngleThreshold(this Mesh mesh, float angle)
	{
		int[] triangles = mesh.GetTriangles(0);
		Vector3[] vertices = mesh.vertices;
		Vector3[] array = new Vector3[triangles.Length / 3];
		Vector3[] array2 = new Vector3[vertices.Length];
		angle *= (float)Math.PI / 180f;
		Dictionary<VertexKey, VertexEntry> dictionary = new Dictionary<VertexKey, VertexEntry>(vertices.Length);
		for (int i = 0; i < triangles.Length; i += 3)
		{
			int num = triangles[i];
			int num2 = triangles[i + 1];
			int num3 = triangles[i + 2];
			Vector3 lhs = vertices[num2] - vertices[num];
			Vector3 rhs = vertices[num3] - vertices[num];
			Vector3 normalized = Vector3.Cross(lhs, rhs).normalized;
			int num4 = i / 3;
			array[num4] = normalized;
			VertexKey key = new VertexKey(vertices[num]);
			if (!dictionary.TryGetValue(key, out var value))
			{
				value = new VertexEntry();
				dictionary.Add(key, value);
			}
			value.Add(num, num4);
			key = new VertexKey(vertices[num2]);
			if (!dictionary.TryGetValue(key, out value))
			{
				value = new VertexEntry();
				dictionary.Add(key, value);
			}
			value.Add(num2, num4);
			key = new VertexKey(vertices[num3]);
			if (!dictionary.TryGetValue(key, out value))
			{
				value = new VertexEntry();
				dictionary.Add(key, value);
			}
			value.Add(num3, num4);
		}
		foreach (VertexEntry value2 in dictionary.Values)
		{
			for (int j = 0; j < value2.Count; j++)
			{
				Vector3 vector = default(Vector3);
				for (int k = 0; k < value2.Count; k++)
				{
					if (value2.VertexIndex[j] == value2.VertexIndex[k])
					{
						vector += array[value2.TriangleIndex[k]];
					}
					else if (Mathf.Acos(Mathf.Clamp(Vector3.Dot(array[value2.TriangleIndex[j]], array[value2.TriangleIndex[k]]), -0.99999f, 0.99999f)) <= angle)
					{
						vector += array[value2.TriangleIndex[k]];
					}
				}
				array2[value2.VertexIndex[j]] = vector.normalized;
			}
		}
		mesh.normals = array2;
	}
}
