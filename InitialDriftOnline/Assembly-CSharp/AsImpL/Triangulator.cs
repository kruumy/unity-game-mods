using System.Collections.Generic;
using AsImpL.MathUtil;
using UnityEngine;

namespace AsImpL;

public static class Triangulator
{
	public static void Triangulate(DataSet dataSet, DataSet.FaceIndices[] face)
	{
		int num = face.Length;
		Vector3 planeNormal = FindPlaneNormal(dataSet, face);
		List<Vertex> list = new List<Vertex>();
		for (int i = 0; i < num; i++)
		{
			int vertIdx = face[i].vertIdx;
			list.Add(new Vertex(i, dataSet.vertList[vertIdx]));
		}
		List<Triangle> list2 = Triangulation.TriangulateByEarClipping(list, planeNormal, dataSet.CurrGroupName);
		for (int j = 0; j < list2.Count; j++)
		{
			int originalIndex = list2[j].v1.OriginalIndex;
			int originalIndex2 = list2[j].v2.OriginalIndex;
			int originalIndex3 = list2[j].v3.OriginalIndex;
			dataSet.AddFaceIndices(face[originalIndex]);
			dataSet.AddFaceIndices(face[originalIndex3]);
			dataSet.AddFaceIndices(face[originalIndex2]);
		}
	}

	public static Vector3 FindPlaneNormal(DataSet dataSet, DataSet.FaceIndices[] face)
	{
		int num = face.Length;
		bool num2 = dataSet.normalList.Count > 0;
		Vector3 result = Vector3.zero;
		if (num2)
		{
			for (int i = 0; i < num; i++)
			{
				int normIdx = face[i].normIdx;
				result += dataSet.normalList[normIdx];
			}
			result.Normalize();
		}
		else
		{
			Vector3 vert = dataSet.vertList[face[0].vertIdx];
			Vector3 vNext = dataSet.vertList[face[1].vertIdx];
			Vector3 vPrev = dataSet.vertList[face[num - 1].vertIdx];
			result = MathUtility.ComputeNormal(vert, vNext, vPrev);
		}
		return result;
	}
}
