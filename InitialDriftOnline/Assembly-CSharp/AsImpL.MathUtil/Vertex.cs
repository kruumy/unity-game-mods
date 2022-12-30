using UnityEngine;

namespace AsImpL.MathUtil;

public class Vertex
{
	private Vertex prevVertex;

	private Vertex nextVertex;

	private float triangleArea;

	private bool triangleHasChanged;

	public Vector3 Position { get; private set; }

	public int OriginalIndex { get; private set; }

	public Vertex PreviousVertex
	{
		get
		{
			return prevVertex;
		}
		set
		{
			triangleHasChanged = prevVertex != value;
			prevVertex = value;
		}
	}

	public Vertex NextVertex
	{
		get
		{
			return nextVertex;
		}
		set
		{
			triangleHasChanged = nextVertex != value;
			nextVertex = value;
		}
	}

	public float TriangleArea
	{
		get
		{
			if (triangleHasChanged)
			{
				ComputeTriangleArea();
			}
			return triangleArea;
		}
	}

	public Vertex(int originalIndex, Vector3 position)
	{
		OriginalIndex = originalIndex;
		Position = position;
	}

	public Vector2 GetPosOnPlane(Vector3 planeNormal)
	{
		Quaternion quaternion = default(Quaternion);
		quaternion.SetFromToRotation(planeNormal, Vector3.back);
		Vector3 vector = quaternion * Position;
		return new Vector2(vector.x, vector.y);
	}

	private void ComputeTriangleArea()
	{
		Vector3 lhs = PreviousVertex.Position - Position;
		Vector3 rhs = NextVertex.Position - Position;
		triangleArea = Vector3.Cross(lhs, rhs).magnitude / 2f;
	}
}
