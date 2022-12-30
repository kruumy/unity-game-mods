using System.Collections.Generic;
using UnityEngine;

namespace AsImpL.MathUtil;

public static class Triangulation
{
	public static List<Triangle> TriangulateConvexPolygon(List<Vertex> vertices, bool preserveOriginalVertices = true)
	{
		List<Vertex> list = (preserveOriginalVertices ? new List<Vertex>(vertices) : vertices);
		List<Triangle> list2 = new List<Triangle>();
		while (list.Count != 3)
		{
			Vertex vertex = FindMaxAreaEarVertex(list);
			list2.Add(ClipTriangle(vertex, list));
		}
		list2.Add(new Triangle(list[0], list[1], list[2]));
		return list2;
	}

	public static List<Triangle> TriangulateByEarClipping(List<Vertex> origVertices, Vector3 planeNormal, string meshName, bool preserveOriginalVertices = true)
	{
		List<Triangle> list = new List<Triangle>();
		List<Vertex> list2 = (preserveOriginalVertices ? new List<Vertex>(origVertices) : origVertices);
		for (int i = 0; i < list2.Count; i++)
		{
			int index = MathUtility.ClampListIndex(i + 1, list2.Count);
			int index2 = MathUtility.ClampListIndex(i - 1, list2.Count);
			list2[i].PreviousVertex = list2[index2];
			list2[i].NextVertex = list2[index];
		}
		List<Vertex> list3 = FindEarVertices(list2, planeNormal);
		while (list2.Count != 3)
		{
			if (list3.Count == 0)
			{
				planeNormal = -planeNormal;
				list3 = FindEarVertices(list2, planeNormal);
			}
			if (list3.Count == 0)
			{
				Debug.LogWarningFormat("Cannot find a proper reprojection for mesh '{0}'. Using fallback polygon triangulation.", meshName);
				Triangle item = ClipTriangle(list2[0], list2);
				list.Add(item);
			}
			else
			{
				Triangle item2 = ClipEar(FindMaxAreaEarVertex(list3), list3, list2, planeNormal);
				list.Add(item2);
			}
		}
		list.Add(new Triangle(list2[0], list2[1], list2[2]));
		return list;
	}

	public static Triangle ClipTriangle(Vertex vertex, List<Vertex> vertices)
	{
		Vertex previousVertex = vertex.PreviousVertex;
		Vertex nextVertex = vertex.NextVertex;
		Triangle result = new Triangle(previousVertex, vertex, nextVertex);
		vertices.Remove(vertex);
		previousVertex.NextVertex = nextVertex;
		nextVertex.PreviousVertex = previousVertex;
		return result;
	}

	private static Triangle ClipEar(Vertex earVertex, List<Vertex> earVertices, List<Vertex> vertices, Vector3 planeNormal)
	{
		Vertex previousVertex = earVertex.PreviousVertex;
		Vertex nextVertex = earVertex.NextVertex;
		Triangle result = ClipTriangle(earVertex, vertices);
		earVertices.Remove(earVertex);
		if (IsVertexEar(previousVertex, vertices, planeNormal))
		{
			earVertices.Add(previousVertex);
		}
		else
		{
			earVertices.Remove(previousVertex);
		}
		if (IsVertexEar(nextVertex, vertices, planeNormal))
		{
			earVertices.Add(nextVertex);
			return result;
		}
		earVertices.Remove(nextVertex);
		return result;
	}

	private static Vertex FindMaxAreaEarVertex(List<Vertex> earVertices)
	{
		Vertex vertex = earVertices[0];
		foreach (Vertex earVertex in earVertices)
		{
			if (earVertex.TriangleArea < vertex.TriangleArea)
			{
				vertex = earVertex;
			}
		}
		return vertex;
	}

	private static List<Vertex> FindEarVertices(List<Vertex> vertices, Vector3 planeNormal)
	{
		List<Vertex> list = new List<Vertex>();
		for (int i = 0; i < vertices.Count; i++)
		{
			if (IsVertexEar(vertices[i], vertices, planeNormal))
			{
				list.Add(vertices[i]);
			}
		}
		return list;
	}

	private static bool IsVertexReflex(Vertex v, Vector3 vNormal)
	{
		Vector2 posOnPlane = v.PreviousVertex.GetPosOnPlane(vNormal);
		Vector2 posOnPlane2 = v.GetPosOnPlane(vNormal);
		Vector2 posOnPlane3 = v.NextVertex.GetPosOnPlane(vNormal);
		return !MathUtility.IsTriangleOrientedClockwise(posOnPlane, posOnPlane2, posOnPlane3);
	}

	private static bool IsVertexEar(Vertex v, List<Vertex> vertices, Vector3 planeNormal)
	{
		if (IsVertexReflex(v, planeNormal))
		{
			return false;
		}
		Vector2 posOnPlane = v.PreviousVertex.GetPosOnPlane(planeNormal);
		Vector2 posOnPlane2 = v.GetPosOnPlane(planeNormal);
		Vector2 posOnPlane3 = v.NextVertex.GetPosOnPlane(planeNormal);
		bool flag = false;
		for (int i = 0; i < vertices.Count; i++)
		{
			if (v != vertices[i] && v.PreviousVertex != vertices[i] && v.NextVertex != vertices[i] && IsVertexReflex(vertices[i], planeNormal))
			{
				Vector2 posOnPlane4 = vertices[i].GetPosOnPlane(planeNormal);
				if (MathUtility.IsPointInTriangle(posOnPlane, posOnPlane2, posOnPlane3, posOnPlane4))
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			return true;
		}
		return false;
	}
}
