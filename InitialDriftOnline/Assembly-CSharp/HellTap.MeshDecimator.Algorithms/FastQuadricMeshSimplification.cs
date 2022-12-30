using System;
using System.Collections.Generic;
using HellTap.MeshDecimator.Collections;
using HellTap.MeshDecimator.Math;

namespace HellTap.MeshDecimator.Algorithms;

public sealed class FastQuadricMeshSimplification : DecimationAlgorithm
{
	private struct Triangle
	{
		public int v0;

		public int v1;

		public int v2;

		public int subMeshIndex;

		public int va0;

		public int va1;

		public int va2;

		public double err0;

		public double err1;

		public double err2;

		public double err3;

		public bool deleted;

		public bool dirty;

		public Vector3d n;

		public int this[int index]
		{
			get
			{
				return index switch
				{
					1 => v1, 
					0 => v0, 
					_ => v2, 
				};
			}
			set
			{
				switch (index)
				{
				case 0:
					v0 = value;
					break;
				case 1:
					v1 = value;
					break;
				case 2:
					v2 = value;
					break;
				default:
					throw new IndexOutOfRangeException();
				}
			}
		}

		public Triangle(int v0, int v1, int v2, int subMeshIndex)
		{
			this.v0 = v0;
			this.v1 = v1;
			this.v2 = v2;
			this.subMeshIndex = subMeshIndex;
			va0 = v0;
			va1 = v1;
			va2 = v2;
			err0 = (err1 = (err2 = (err3 = 0.0)));
			deleted = (dirty = false);
			n = default(Vector3d);
		}

		public void GetAttributeIndices(int[] attributeIndices)
		{
			attributeIndices[0] = va0;
			attributeIndices[1] = va1;
			attributeIndices[2] = va2;
		}

		public void SetAttributeIndex(int index, int value)
		{
			switch (index)
			{
			case 0:
				va0 = value;
				break;
			case 1:
				va1 = value;
				break;
			case 2:
				va2 = value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}

		public void GetErrors(double[] err)
		{
			err[0] = err0;
			err[1] = err1;
			err[2] = err2;
		}
	}

	private struct Vertex
	{
		public Vector3d p;

		public int tstart;

		public int tcount;

		public SymmetricMatrix q;

		public bool border;

		public bool seam;

		public bool foldover;

		public Vertex(Vector3d p)
		{
			this.p = p;
			tstart = 0;
			tcount = 0;
			q = default(SymmetricMatrix);
			border = true;
			seam = false;
			foldover = false;
		}
	}

	private struct Ref
	{
		public int tid;

		public int tvertex;

		public void Set(int tid, int tvertex)
		{
			this.tid = tid;
			this.tvertex = tvertex;
		}
	}

	private struct BorderVertex
	{
		public int index;

		public int hash;

		public BorderVertex(int index, int hash)
		{
			this.index = index;
			this.hash = hash;
		}
	}

	private class BorderVertexComparer : IComparer<BorderVertex>
	{
		public static readonly BorderVertexComparer instance = new BorderVertexComparer();

		public int Compare(BorderVertex x, BorderVertex y)
		{
			return x.hash.CompareTo(y.hash);
		}
	}

	private const double DoubleEpsilon = 0.001;

	private bool preserveSeams;

	private bool preserveFoldovers;

	private bool enableSmartLink = true;

	private int maxIterationCount = 100;

	private double agressiveness = 7.0;

	private double vertexLinkDistanceSqr = double.Epsilon;

	private int subMeshCount;

	private ResizableArray<Triangle> triangles;

	private ResizableArray<Vertex> vertices;

	private ResizableArray<Ref> refs;

	private ResizableArray<Vector3> vertNormals;

	private ResizableArray<Vector4> vertTangents;

	private UVChannels<Vector2> vertUV2D;

	private UVChannels<Vector3> vertUV3D;

	private UVChannels<Vector4> vertUV4D;

	private ResizableArray<Vector4> vertColors;

	private ResizableArray<BoneWeight> vertBoneWeights;

	private int remainingVertices;

	private double[] errArr = new double[3];

	private int[] attributeIndexArr = new int[3];

	public bool PreserveSeams
	{
		get
		{
			return preserveSeams;
		}
		set
		{
			preserveSeams = value;
		}
	}

	public bool PreserveFoldovers
	{
		get
		{
			return preserveFoldovers;
		}
		set
		{
			preserveFoldovers = value;
		}
	}

	public bool EnableSmartLink
	{
		get
		{
			return enableSmartLink;
		}
		set
		{
			enableSmartLink = value;
		}
	}

	public int MaxIterationCount
	{
		get
		{
			return maxIterationCount;
		}
		set
		{
			maxIterationCount = value;
		}
	}

	public double Agressiveness
	{
		get
		{
			return agressiveness;
		}
		set
		{
			agressiveness = value;
		}
	}

	public double VertexLinkDistanceSqr
	{
		get
		{
			return vertexLinkDistanceSqr;
		}
		set
		{
			vertexLinkDistanceSqr = value;
		}
	}

	public FastQuadricMeshSimplification(bool preserveBorders = false, bool preserveSeams = false, bool preserveFoldovers = false)
	{
		triangles = new ResizableArray<Triangle>(0);
		vertices = new ResizableArray<Vertex>(0);
		refs = new ResizableArray<Ref>(0);
		base.PreserveBorders = preserveBorders;
		PreserveSeams = preserveSeams;
		PreserveFoldovers = preserveFoldovers;
	}

	private ResizableArray<T> InitializeVertexAttribute<T>(T[] attributeValues, string attributeName)
	{
		if (attributeValues != null && attributeValues.Length == vertices.Length)
		{
			ResizableArray<T> resizableArray = new ResizableArray<T>(attributeValues.Length, attributeValues.Length);
			T[] data = resizableArray.Data;
			Array.Copy(attributeValues, 0, data, 0, attributeValues.Length);
			return resizableArray;
		}
		if (attributeValues != null && attributeValues.Length != 0)
		{
			Logging.LogError("Failed to set vertex attribute '{0}' with {1} length of array, when {2} was needed.", attributeName, attributeValues.Length, vertices.Length);
		}
		return null;
	}

	private double VertexError(ref SymmetricMatrix q, double x, double y, double z)
	{
		return q.m0 * x * x + 2.0 * q.m1 * x * y + 2.0 * q.m2 * x * z + 2.0 * q.m3 * x + q.m4 * y * y + 2.0 * q.m5 * y * z + 2.0 * q.m6 * y + q.m7 * z * z + 2.0 * q.m8 * z + q.m9;
	}

	private double CalculateError(ref Vertex vert0, ref Vertex vert1, out Vector3d result, out int resultIndex)
	{
		SymmetricMatrix q = vert0.q + vert1.q;
		bool flag = vert0.border & vert1.border;
		double num = 0.0;
		double num2 = q.Determinant1();
		if (num2 != 0.0 && !flag)
		{
			result = new Vector3d(-1.0 / num2 * q.Determinant2(), 1.0 / num2 * q.Determinant3(), -1.0 / num2 * q.Determinant4());
			num = VertexError(ref q, result.x, result.y, result.z);
			resultIndex = 2;
		}
		else
		{
			Vector3d p = vert0.p;
			Vector3d p2 = vert1.p;
			Vector3d vector3d = (p + p2) * 0.5;
			double num3 = VertexError(ref q, p.x, p.y, p.z);
			double num4 = VertexError(ref q, p2.x, p2.y, p2.z);
			double num5 = VertexError(ref q, vector3d.x, vector3d.y, vector3d.z);
			num = MathHelper.Min(num3, num4, num5);
			if (num == num5)
			{
				result = vector3d;
				resultIndex = 2;
			}
			else if (num == num4)
			{
				result = p2;
				resultIndex = 1;
			}
			else if (num == num3)
			{
				result = p;
				resultIndex = 0;
			}
			else
			{
				result = vector3d;
				resultIndex = 2;
			}
		}
		return num;
	}

	private bool Flipped(ref Vector3d p, int i0, int i1, ref Vertex v0, bool[] deleted)
	{
		int tcount = v0.tcount;
		Ref[] data = refs.Data;
		Triangle[] data2 = triangles.Data;
		Vertex[] data3 = vertices.Data;
		for (int j = 0; j < tcount; j++)
		{
			Ref @ref = data[v0.tstart + j];
			if (data2[@ref.tid].deleted)
			{
				continue;
			}
			int tvertex = @ref.tvertex;
			int num = data2[@ref.tid][(tvertex + 1) % 3];
			int num2 = data2[@ref.tid][(tvertex + 2) % 3];
			if (num == i1 || num2 == i1)
			{
				deleted[j] = true;
				continue;
			}
			Vector3d lhs = data3[num].p - p;
			lhs.Normalize();
			Vector3d rhs = data3[num2].p - p;
			rhs.Normalize();
			if (System.Math.Abs(Vector3d.Dot(ref lhs, ref rhs)) > 0.999)
			{
				return true;
			}
			Vector3d.Cross(ref lhs, ref rhs, out var result);
			result.Normalize();
			deleted[j] = false;
			if (Vector3d.Dot(ref result, ref data2[@ref.tid].n) < 0.2)
			{
				return true;
			}
		}
		return false;
	}

	private void UpdateTriangles(int i0, int ia0, ref Vertex v, ResizableArray<bool> deleted, ref int deletedTriangles)
	{
		int tcount = v.tcount;
		Triangle[] data = triangles.Data;
		Vertex[] data2 = vertices.Data;
		for (int j = 0; j < tcount; j++)
		{
			Ref item = refs[v.tstart + j];
			int tid = item.tid;
			Triangle triangle = data[tid];
			if (triangle.deleted)
			{
				continue;
			}
			if (deleted[j])
			{
				data[tid].deleted = true;
				deletedTriangles++;
				continue;
			}
			triangle[item.tvertex] = i0;
			if (ia0 != -1)
			{
				triangle.SetAttributeIndex(item.tvertex, ia0);
			}
			triangle.dirty = true;
			triangle.err0 = CalculateError(ref data2[triangle.v0], ref data2[triangle.v1], out var result, out var resultIndex);
			triangle.err1 = CalculateError(ref data2[triangle.v1], ref data2[triangle.v2], out result, out resultIndex);
			triangle.err2 = CalculateError(ref data2[triangle.v2], ref data2[triangle.v0], out result, out resultIndex);
			triangle.err3 = MathHelper.Min(triangle.err0, triangle.err1, triangle.err2);
			data[tid] = triangle;
			refs.Add(item);
		}
	}

	private void MoveVertexAttributes(int i0, int i1)
	{
		if (vertNormals != null)
		{
			vertNormals[i0] = vertNormals[i1];
		}
		if (vertTangents != null)
		{
			vertTangents[i0] = vertTangents[i1];
		}
		if (vertUV2D != null)
		{
			for (int j = 0; j < 4; j++)
			{
				ResizableArray<Vector2> resizableArray = vertUV2D[j];
				if (resizableArray != null)
				{
					resizableArray[i0] = resizableArray[i1];
				}
			}
		}
		if (vertUV3D != null)
		{
			for (int k = 0; k < 4; k++)
			{
				ResizableArray<Vector3> resizableArray2 = vertUV3D[k];
				if (resizableArray2 != null)
				{
					resizableArray2[i0] = resizableArray2[i1];
				}
			}
		}
		if (vertUV4D != null)
		{
			for (int l = 0; l < 4; l++)
			{
				ResizableArray<Vector4> resizableArray3 = vertUV4D[l];
				if (resizableArray3 != null)
				{
					resizableArray3[i0] = resizableArray3[i1];
				}
			}
		}
		if (vertColors != null)
		{
			vertColors[i0] = vertColors[i1];
		}
		if (vertBoneWeights != null)
		{
			vertBoneWeights[i0] = vertBoneWeights[i1];
		}
	}

	private void MergeVertexAttributes(int i0, int i1)
	{
		if (vertNormals != null)
		{
			vertNormals[i0] = (vertNormals[i0] + vertNormals[i1]) * 0.5f;
		}
		if (vertTangents != null)
		{
			vertTangents[i0] = (vertTangents[i0] + vertTangents[i1]) * 0.5f;
		}
		if (vertUV2D != null)
		{
			for (int j = 0; j < 4; j++)
			{
				ResizableArray<Vector2> resizableArray = vertUV2D[j];
				if (resizableArray != null)
				{
					resizableArray[i0] = (resizableArray[i0] + resizableArray[i1]) * 0.5f;
				}
			}
		}
		if (vertUV3D != null)
		{
			for (int k = 0; k < 4; k++)
			{
				ResizableArray<Vector3> resizableArray2 = vertUV3D[k];
				if (resizableArray2 != null)
				{
					resizableArray2[i0] = (resizableArray2[i0] + resizableArray2[i1]) * 0.5f;
				}
			}
		}
		if (vertUV4D != null)
		{
			for (int l = 0; l < 4; l++)
			{
				ResizableArray<Vector4> resizableArray3 = vertUV4D[l];
				if (resizableArray3 != null)
				{
					resizableArray3[i0] = (resizableArray3[i0] + resizableArray3[i1]) * 0.5f;
				}
			}
		}
		if (vertColors != null)
		{
			vertColors[i0] = (vertColors[i0] + vertColors[i1]) * 0.5f;
		}
	}

	private bool AreUVsTheSame(int channel, int indexA, int indexB)
	{
		if (vertUV2D != null)
		{
			ResizableArray<Vector2> resizableArray = vertUV2D[channel];
			if (resizableArray != null)
			{
				Vector2 vector = resizableArray[indexA];
				Vector2 vector2 = resizableArray[indexB];
				return vector == vector2;
			}
		}
		if (vertUV3D != null)
		{
			ResizableArray<Vector3> resizableArray2 = vertUV3D[channel];
			if (resizableArray2 != null)
			{
				Vector3 vector3 = resizableArray2[indexA];
				Vector3 vector4 = resizableArray2[indexB];
				return vector3 == vector4;
			}
		}
		if (vertUV4D != null)
		{
			ResizableArray<Vector4> resizableArray3 = vertUV4D[channel];
			if (resizableArray3 != null)
			{
				Vector4 vector5 = resizableArray3[indexA];
				Vector4 vector6 = resizableArray3[indexB];
				return vector5 == vector6;
			}
		}
		return false;
	}

	private void RemoveVertexPass(int startTrisCount, int targetTrisCount, double threshold, ResizableArray<bool> deleted0, ResizableArray<bool> deleted1, ref int deletedTris)
	{
		Triangle[] data = triangles.Data;
		int length = triangles.Length;
		Vertex[] data2 = vertices.Data;
		bool flag = base.PreserveBorders;
		int num = base.MaxVertexCount;
		if (num <= 0)
		{
			num = int.MaxValue;
		}
		for (int i = 0; i < length; i++)
		{
			if (data[i].dirty || data[i].deleted || data[i].err3 > threshold)
			{
				continue;
			}
			data[i].GetErrors(errArr);
			data[i].GetAttributeIndices(attributeIndexArr);
			for (int j = 0; j < 3; j++)
			{
				if (errArr[j] > threshold)
				{
					continue;
				}
				int num2 = (j + 1) % 3;
				int num3 = data[i][j];
				int num4 = data[i][num2];
				if (data2[num3].border != data2[num4].border || data2[num3].seam != data2[num4].seam || data2[num3].foldover != data2[num4].foldover || (flag && data2[num3].border) || (preserveSeams && data2[num3].seam) || (preserveFoldovers && data2[num3].foldover))
				{
					continue;
				}
				CalculateError(ref data2[num3], ref data2[num4], out var result, out var resultIndex);
				deleted0.Resize(data2[num3].tcount);
				deleted1.Resize(data2[num4].tcount);
				if (Flipped(ref result, num3, num4, ref data2[num3], deleted0.Data) || Flipped(ref result, num4, num3, ref data2[num4], deleted1.Data))
				{
					continue;
				}
				int num5 = attributeIndexArr[j];
				data2[num3].p = result;
				data2[num3].q += data2[num4].q;
				switch (resultIndex)
				{
				case 1:
				{
					int i3 = attributeIndexArr[num2];
					MoveVertexAttributes(num5, i3);
					break;
				}
				case 2:
				{
					int i2 = attributeIndexArr[num2];
					MergeVertexAttributes(num5, i2);
					break;
				}
				}
				if (data2[num3].seam)
				{
					num5 = -1;
				}
				int length2 = refs.Length;
				UpdateTriangles(num3, num5, ref data2[num3], deleted0, ref deletedTris);
				UpdateTriangles(num3, num5, ref data2[num4], deleted1, ref deletedTris);
				int num6 = refs.Length - length2;
				if (num6 <= data2[num3].tcount)
				{
					if (num6 > 0)
					{
						Ref[] data3 = refs.Data;
						Array.Copy(data3, length2, data3, data2[num3].tstart, num6);
					}
				}
				else
				{
					data2[num3].tstart = length2;
				}
				data2[num3].tcount = num6;
				remainingVertices--;
				break;
			}
			if (startTrisCount - deletedTris <= targetTrisCount && remainingVertices < num)
			{
				break;
			}
		}
	}

	private void UpdateMesh(int iteration)
	{
		Triangle[] data = triangles.Data;
		Vertex[] data2 = vertices.Data;
		int num = triangles.Length;
		int length = vertices.Length;
		if (iteration > 0)
		{
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				if (!data[i].deleted)
				{
					if (num2 != i)
					{
						data[num2] = data[i];
					}
					num2++;
				}
			}
			triangles.Resize(num2);
			data = triangles.Data;
			num = num2;
		}
		UpdateReferences();
		if (iteration != 0)
		{
			return;
		}
		Ref[] data3 = refs.Data;
		List<int> list = new List<int>(8);
		List<int> list2 = new List<int>(8);
		int num3 = 0;
		for (int j = 0; j < length; j++)
		{
			data2[j].border = false;
			data2[j].seam = false;
			data2[j].foldover = false;
		}
		int num4 = 0;
		double num5 = double.MaxValue;
		double num6 = double.MinValue;
		for (int k = 0; k < length; k++)
		{
			int tstart = data2[k].tstart;
			int tcount = data2[k].tcount;
			list.Clear();
			list2.Clear();
			num3 = 0;
			for (int l = 0; l < tcount; l++)
			{
				int tid = data3[tstart + l].tid;
				for (int m = 0; m < 3; m++)
				{
					int n = 0;
					int num7;
					for (num7 = data[tid][m]; n < num3 && list2[n] != num7; n++)
					{
					}
					if (n == num3)
					{
						list.Add(1);
						list2.Add(num7);
						num3++;
					}
					else
					{
						list[n]++;
					}
				}
			}
			for (int num8 = 0; num8 < num3; num8++)
			{
				if (list[num8] != 1)
				{
					continue;
				}
				int num7 = list2[num8];
				data2[num7].border = true;
				num4++;
				if (enableSmartLink)
				{
					if (data2[num7].p.x < num5)
					{
						num5 = data2[num7].p.x;
					}
					if (data2[num7].p.x > num6)
					{
						num6 = data2[num7].p.x;
					}
				}
			}
		}
		if (enableSmartLink)
		{
			BorderVertex[] array = new BorderVertex[num4];
			int num9 = 0;
			double num10 = num6 - num5;
			for (int num11 = 0; num11 < length; num11++)
			{
				if (data2[num11].border)
				{
					int hash = (int)(((data2[num11].p.x - num5) / num10 - 0.5) * 2147483647.0);
					array[num9] = new BorderVertex(num11, hash);
					num9++;
				}
			}
			Array.Sort(array, 0, num9, BorderVertexComparer.instance);
			for (int num12 = 0; num12 < num9; num12++)
			{
				int index = array[num12].index;
				if (index == -1)
				{
					continue;
				}
				Vector3d p = data2[index].p;
				for (int num13 = num12 + 1; num13 < num9; num13++)
				{
					int index2 = array[num13].index;
					if (index2 == -1)
					{
						continue;
					}
					if (array[num13].hash - array[num12].hash > 1)
					{
						break;
					}
					Vector3d p2 = data2[index2].p;
					double num14 = (p.x - p2.x) * (p.x - p2.x);
					double num15 = (p.y - p2.y) * (p.y - p2.y);
					double num16 = (p.z - p2.z) * (p.z - p2.z);
					if (num14 + num15 + num16 <= vertexLinkDistanceSqr)
					{
						array[num13].index = -1;
						data2[index].border = false;
						data2[index2].border = false;
						if (AreUVsTheSame(0, index, index2))
						{
							data2[index].foldover = true;
							data2[index2].foldover = true;
						}
						else
						{
							data2[index].seam = true;
							data2[index2].seam = true;
						}
						int tcount2 = data2[index2].tcount;
						int tstart2 = data2[index2].tstart;
						for (int num17 = 0; num17 < tcount2; num17++)
						{
							Ref @ref = data3[tstart2 + num17];
							data[@ref.tid][@ref.tvertex] = index;
						}
					}
				}
			}
			UpdateReferences();
		}
		for (int num18 = 0; num18 < length; num18++)
		{
			data2[num18].q = default(SymmetricMatrix);
		}
		for (int num19 = 0; num19 < num; num19++)
		{
			int v = data[num19].v0;
			int v2 = data[num19].v1;
			int v3 = data[num19].v2;
			Vector3d rhs = data2[v].p;
			Vector3d p3 = data2[v2].p;
			Vector3d p4 = data2[v3].p;
			Vector3d lhs = p3 - rhs;
			Vector3d rhs2 = p4 - rhs;
			Vector3d.Cross(ref lhs, ref rhs2, out var result);
			result.Normalize();
			data[num19].n = result;
			SymmetricMatrix symmetricMatrix = new SymmetricMatrix(result.x, result.y, result.z, 0.0 - Vector3d.Dot(ref result, ref rhs));
			data2[v].q += symmetricMatrix;
			data2[v2].q += symmetricMatrix;
			data2[v3].q += symmetricMatrix;
		}
		for (int num20 = 0; num20 < num; num20++)
		{
			Triangle triangle = data[num20];
			data[num20].err0 = CalculateError(ref data2[triangle.v0], ref data2[triangle.v1], out var result2, out var resultIndex);
			data[num20].err1 = CalculateError(ref data2[triangle.v1], ref data2[triangle.v2], out result2, out resultIndex);
			data[num20].err2 = CalculateError(ref data2[triangle.v2], ref data2[triangle.v0], out result2, out resultIndex);
			data[num20].err3 = MathHelper.Min(data[num20].err0, data[num20].err1, data[num20].err2);
		}
	}

	private void UpdateReferences()
	{
		int length = triangles.Length;
		int length2 = vertices.Length;
		Triangle[] data = triangles.Data;
		Vertex[] data2 = vertices.Data;
		for (int i = 0; i < length2; i++)
		{
			data2[i].tstart = 0;
			data2[i].tcount = 0;
		}
		for (int j = 0; j < length; j++)
		{
			data2[data[j].v0].tcount++;
			data2[data[j].v1].tcount++;
			data2[data[j].v2].tcount++;
		}
		int num = 0;
		remainingVertices = 0;
		for (int k = 0; k < length2; k++)
		{
			data2[k].tstart = num;
			if (data2[k].tcount > 0)
			{
				num += data2[k].tcount;
				data2[k].tcount = 0;
				remainingVertices++;
			}
		}
		refs.Resize(num);
		Ref[] data3 = refs.Data;
		for (int l = 0; l < length; l++)
		{
			int v = data[l].v0;
			int v2 = data[l].v1;
			int v3 = data[l].v2;
			int tstart = data2[v].tstart;
			int tcount = data2[v].tcount;
			int tstart2 = data2[v2].tstart;
			int tcount2 = data2[v2].tcount;
			int tstart3 = data2[v3].tstart;
			int tcount3 = data2[v3].tcount;
			data3[tstart + tcount].Set(l, 0);
			data3[tstart2 + tcount2].Set(l, 1);
			data3[tstart3 + tcount3].Set(l, 2);
			data2[v].tcount++;
			data2[v2].tcount++;
			data2[v3].tcount++;
		}
	}

	private void CompactMesh()
	{
		int num = 0;
		Vertex[] data = vertices.Data;
		int length = vertices.Length;
		for (int i = 0; i < length; i++)
		{
			data[i].tcount = 0;
		}
		Vector3[] array = ((vertNormals != null) ? vertNormals.Data : null);
		Vector4[] array2 = ((vertTangents != null) ? vertTangents.Data : null);
		Vector2[][] array3 = ((vertUV2D != null) ? vertUV2D.Data : null);
		Vector3[][] array4 = ((vertUV3D != null) ? vertUV3D.Data : null);
		Vector4[][] array5 = ((vertUV4D != null) ? vertUV4D.Data : null);
		Vector4[] array6 = ((vertColors != null) ? vertColors.Data : null);
		BoneWeight[] array7 = ((vertBoneWeights != null) ? vertBoneWeights.Data : null);
		Triangle[] data2 = triangles.Data;
		int length2 = triangles.Length;
		for (int j = 0; j < length2; j++)
		{
			Triangle triangle = data2[j];
			if (triangle.deleted)
			{
				continue;
			}
			if (triangle.va0 != triangle.v0)
			{
				int va = triangle.va0;
				int v = triangle.v0;
				data[va].p = data[v].p;
				if (array7 != null)
				{
					array7[va] = array7[v];
				}
				triangle.v0 = triangle.va0;
			}
			if (triangle.va1 != triangle.v1)
			{
				int va2 = triangle.va1;
				int v2 = triangle.v1;
				data[va2].p = data[v2].p;
				if (array7 != null)
				{
					array7[va2] = array7[v2];
				}
				triangle.v1 = triangle.va1;
			}
			if (triangle.va2 != triangle.v2)
			{
				int va3 = triangle.va2;
				int v3 = triangle.v2;
				data[va3].p = data[v3].p;
				if (array7 != null)
				{
					array7[va3] = array7[v3];
				}
				triangle.v2 = triangle.va2;
			}
			data2[num++] = triangle;
			data[triangle.v0].tcount = 1;
			data[triangle.v1].tcount = 1;
			data[triangle.v2].tcount = 1;
		}
		length2 = num;
		triangles.Resize(length2);
		data2 = triangles.Data;
		num = 0;
		for (int k = 0; k < length; k++)
		{
			Vertex vertex = data[k];
			if (vertex.tcount <= 0)
			{
				continue;
			}
			vertex.tstart = num;
			data[k] = vertex;
			if (num != k)
			{
				data[num].p = vertex.p;
				if (array != null)
				{
					array[num] = array[k];
				}
				if (array2 != null)
				{
					array2[num] = array2[k];
				}
				if (array3 != null)
				{
					for (int l = 0; l < 4; l++)
					{
						Vector2[] array8 = array3[l];
						if (array8 != null)
						{
							array8[num] = array8[k];
						}
					}
				}
				if (array4 != null)
				{
					for (int m = 0; m < 4; m++)
					{
						Vector3[] array9 = array4[m];
						if (array9 != null)
						{
							array9[num] = array9[k];
						}
					}
				}
				if (array5 != null)
				{
					for (int n = 0; n < 4; n++)
					{
						Vector4[] array10 = array5[n];
						if (array10 != null)
						{
							array10[num] = array10[k];
						}
					}
				}
				if (array6 != null)
				{
					array6[num] = array6[k];
				}
				if (array7 != null)
				{
					array7[num] = array7[k];
				}
			}
			num++;
		}
		for (int num2 = 0; num2 < length2; num2++)
		{
			Triangle triangle2 = data2[num2];
			triangle2.v0 = data[triangle2.v0].tstart;
			triangle2.v1 = data[triangle2.v1].tstart;
			triangle2.v2 = data[triangle2.v2].tstart;
			data2[num2] = triangle2;
		}
		length = num;
		vertices.Resize(length);
		if (array != null)
		{
			vertNormals.Resize(length, trimExess: true);
		}
		if (array2 != null)
		{
			vertTangents.Resize(length, trimExess: true);
		}
		if (array3 != null)
		{
			vertUV2D.Resize(length, trimExess: true);
		}
		if (array4 != null)
		{
			vertUV3D.Resize(length, trimExess: true);
		}
		if (array5 != null)
		{
			vertUV4D.Resize(length, trimExess: true);
		}
		if (array6 != null)
		{
			vertColors.Resize(length, trimExess: true);
		}
		if (array7 != null)
		{
			vertBoneWeights.Resize(length, trimExess: true);
		}
	}

	public override void Initialize(Mesh mesh)
	{
		if (mesh == null)
		{
			throw new ArgumentNullException("mesh");
		}
		int num = mesh.SubMeshCount;
		int triangleCount = mesh.TriangleCount;
		Vector3d[] array = mesh.Vertices;
		Vector3[] normals = mesh.Normals;
		Vector4[] tangents = mesh.Tangents;
		Vector4[] colors = mesh.Colors;
		BoneWeight[] boneWeights = mesh.BoneWeights;
		subMeshCount = num;
		vertices.Resize(array.Length);
		Vertex[] data = vertices.Data;
		for (int i = 0; i < array.Length; i++)
		{
			data[i] = new Vertex(array[i]);
		}
		triangles.Resize(triangleCount);
		Triangle[] data2 = triangles.Data;
		int num2 = 0;
		for (int j = 0; j < num; j++)
		{
			int[] indices = mesh.GetIndices(j);
			int num3 = indices.Length / 3;
			for (int k = 0; k < num3; k++)
			{
				int num4 = k * 3;
				int v = indices[num4];
				int v2 = indices[num4 + 1];
				int v3 = indices[num4 + 2];
				data2[num2++] = new Triangle(v, v2, v3, j);
			}
		}
		vertNormals = InitializeVertexAttribute(normals, "normals");
		vertTangents = InitializeVertexAttribute(tangents, "tangents");
		vertColors = InitializeVertexAttribute(colors, "colors");
		vertBoneWeights = InitializeVertexAttribute(boneWeights, "boneWeights");
		for (int l = 0; l < 4; l++)
		{
			int uVDimension = mesh.GetUVDimension(l);
			string attributeName = $"uv{l}";
			switch (uVDimension)
			{
			case 2:
			{
				if (vertUV2D == null)
				{
					vertUV2D = new UVChannels<Vector2>();
				}
				Vector2[] uVs2D = mesh.GetUVs2D(l);
				vertUV2D[l] = InitializeVertexAttribute(uVs2D, attributeName);
				break;
			}
			case 3:
			{
				if (vertUV3D == null)
				{
					vertUV3D = new UVChannels<Vector3>();
				}
				Vector3[] uVs3D = mesh.GetUVs3D(l);
				vertUV3D[l] = InitializeVertexAttribute(uVs3D, attributeName);
				break;
			}
			case 4:
			{
				if (vertUV4D == null)
				{
					vertUV4D = new UVChannels<Vector4>();
				}
				Vector4[] uVs4D = mesh.GetUVs4D(l);
				vertUV4D[l] = InitializeVertexAttribute(uVs4D, attributeName);
				break;
			}
			}
		}
	}

	public override void DecimateMesh(int targetTrisCount)
	{
		if (targetTrisCount < 0)
		{
			throw new ArgumentOutOfRangeException("targetTrisCount");
		}
		int deletedTris = 0;
		ResizableArray<bool> deleted = new ResizableArray<bool>(20);
		ResizableArray<bool> deleted2 = new ResizableArray<bool>(20);
		Triangle[] data = triangles.Data;
		int length = triangles.Length;
		int num = length;
		_ = vertices.Data;
		int num2 = base.MaxVertexCount;
		if (num2 <= 0)
		{
			num2 = int.MaxValue;
		}
		for (int i = 0; i < maxIterationCount; i++)
		{
			ReportStatus(i, num, num - deletedTris, targetTrisCount);
			if (num - deletedTris <= targetTrisCount && remainingVertices < num2)
			{
				break;
			}
			if (i % 5 == 0)
			{
				UpdateMesh(i);
				data = triangles.Data;
				length = triangles.Length;
				_ = vertices.Data;
			}
			for (int j = 0; j < length; j++)
			{
				data[j].dirty = false;
			}
			double num3 = 1E-09 * System.Math.Pow(i + 3, agressiveness);
			if (base.Verbose && i % 5 == 0)
			{
				Logging.LogVerbose("iteration {0} - triangles {1} threshold {2}", i, num - deletedTris, num3);
			}
			RemoveVertexPass(num, targetTrisCount, num3, deleted, deleted2, ref deletedTris);
		}
		CompactMesh();
	}

	public override void DecimateMeshLossless()
	{
		int deletedTris = 0;
		ResizableArray<bool> deleted = new ResizableArray<bool>(0);
		ResizableArray<bool> deleted2 = new ResizableArray<bool>(0);
		Triangle[] data = triangles.Data;
		int length = triangles.Length;
		int num = length;
		_ = vertices.Data;
		ReportStatus(0, num, num, -1);
		for (int i = 0; i < 9999; i++)
		{
			UpdateMesh(i);
			data = triangles.Data;
			length = triangles.Length;
			_ = vertices.Data;
			ReportStatus(i, num, length, -1);
			for (int j = 0; j < length; j++)
			{
				data[j].dirty = false;
			}
			double threshold = 0.001;
			if (base.Verbose)
			{
				Logging.LogVerbose("Lossless iteration {0}", i);
			}
			RemoveVertexPass(num, 0, threshold, deleted, deleted2, ref deletedTris);
			if (deletedTris <= 0)
			{
				break;
			}
			deletedTris = 0;
		}
		CompactMesh();
	}

	public override Mesh ToMesh()
	{
		int length = vertices.Length;
		int length2 = triangles.Length;
		Vector3d[] array = new Vector3d[length];
		int[][] array2 = new int[subMeshCount][];
		Vertex[] data = vertices.Data;
		for (int i = 0; i < length; i++)
		{
			array[i] = data[i].p;
		}
		Triangle[] data2 = triangles.Data;
		int[] array3 = new int[subMeshCount];
		int num = -1;
		for (int j = 0; j < length2; j++)
		{
			Triangle triangle = data2[j];
			if (triangle.subMeshIndex != num)
			{
				for (int k = num + 1; k < triangle.subMeshIndex; k++)
				{
					array3[k] = j;
				}
				array3[triangle.subMeshIndex] = j;
				num = triangle.subMeshIndex;
			}
		}
		for (int l = num + 1; l < subMeshCount; l++)
		{
			array3[l] = length2;
		}
		for (int m = 0; m < subMeshCount; m++)
		{
			int num2 = array3[m];
			if (num2 < length2)
			{
				int num3 = ((m + 1 < subMeshCount) ? array3[m + 1] : length2);
				int num4 = num3 - num2;
				if (num4 < 0)
				{
					num4 = 0;
				}
				int[] array4 = new int[num4 * 3];
				for (int n = num2; n < num3; n++)
				{
					Triangle triangle2 = data2[n];
					int num5 = (n - num2) * 3;
					array4[num5] = triangle2.v0;
					array4[num5 + 1] = triangle2.v1;
					array4[num5 + 2] = triangle2.v2;
				}
				array2[m] = array4;
			}
			else
			{
				array2[m] = new int[0];
			}
		}
		Mesh mesh = new Mesh(array, array2);
		if (vertNormals != null)
		{
			mesh.Normals = vertNormals.Data;
		}
		if (vertTangents != null)
		{
			mesh.Tangents = vertTangents.Data;
		}
		if (vertColors != null)
		{
			mesh.Colors = vertColors.Data;
		}
		if (vertBoneWeights != null)
		{
			mesh.BoneWeights = vertBoneWeights.Data;
		}
		if (vertUV2D != null)
		{
			for (int num6 = 0; num6 < 4; num6++)
			{
				if (vertUV2D[num6] != null)
				{
					Vector2[] data3 = vertUV2D[num6].Data;
					mesh.SetUVs(num6, data3);
				}
			}
		}
		if (vertUV3D != null)
		{
			for (int num7 = 0; num7 < 4; num7++)
			{
				if (vertUV3D[num7] != null)
				{
					Vector3[] data4 = vertUV3D[num7].Data;
					mesh.SetUVs(num7, data4);
				}
			}
		}
		if (vertUV4D != null)
		{
			for (int num8 = 0; num8 < 4; num8++)
			{
				if (vertUV4D[num8] != null)
				{
					Vector4[] data5 = vertUV4D[num8].Data;
					mesh.SetUVs(num8, data5);
				}
			}
		}
		return mesh;
	}
}
