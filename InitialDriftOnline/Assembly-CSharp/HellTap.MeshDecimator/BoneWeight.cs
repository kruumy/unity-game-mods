using System;
using HellTap.MeshDecimator.Math;

namespace HellTap.MeshDecimator;

public struct BoneWeight : IEquatable<BoneWeight>
{
	public int boneIndex0;

	public int boneIndex1;

	public int boneIndex2;

	public int boneIndex3;

	public float boneWeight0;

	public float boneWeight1;

	public float boneWeight2;

	public float boneWeight3;

	public BoneWeight(int boneIndex0, int boneIndex1, int boneIndex2, int boneIndex3, float boneWeight0, float boneWeight1, float boneWeight2, float boneWeight3)
	{
		this.boneIndex0 = boneIndex0;
		this.boneIndex1 = boneIndex1;
		this.boneIndex2 = boneIndex2;
		this.boneIndex3 = boneIndex3;
		this.boneWeight0 = boneWeight0;
		this.boneWeight1 = boneWeight1;
		this.boneWeight2 = boneWeight2;
		this.boneWeight3 = boneWeight3;
	}

	public static bool operator ==(BoneWeight lhs, BoneWeight rhs)
	{
		if (lhs.boneIndex0 == rhs.boneIndex0 && lhs.boneIndex1 == rhs.boneIndex1 && lhs.boneIndex2 == rhs.boneIndex2 && lhs.boneIndex3 == rhs.boneIndex3)
		{
			return new Vector4(lhs.boneWeight0, lhs.boneWeight1, lhs.boneWeight2, lhs.boneWeight3) == new Vector4(rhs.boneWeight0, rhs.boneWeight1, rhs.boneWeight2, rhs.boneWeight3);
		}
		return false;
	}

	public static bool operator !=(BoneWeight lhs, BoneWeight rhs)
	{
		return !(lhs == rhs);
	}

	private void MergeBoneWeight(int boneIndex, float weight)
	{
		if (boneIndex == boneIndex0)
		{
			boneWeight0 = (boneWeight0 + weight) * 0.5f;
		}
		else if (boneIndex == boneIndex1)
		{
			boneWeight1 = (boneWeight1 + weight) * 0.5f;
		}
		else if (boneIndex == boneIndex2)
		{
			boneWeight2 = (boneWeight2 + weight) * 0.5f;
		}
		else if (boneIndex == boneIndex3)
		{
			boneWeight3 = (boneWeight3 + weight) * 0.5f;
		}
		else if (boneWeight0 == 0f)
		{
			boneIndex0 = boneIndex;
			boneWeight0 = weight;
		}
		else if (boneWeight1 == 0f)
		{
			boneIndex1 = boneIndex;
			boneWeight1 = weight;
		}
		else if (boneWeight2 == 0f)
		{
			boneIndex2 = boneIndex;
			boneWeight2 = weight;
		}
		else if (boneWeight3 == 0f)
		{
			boneIndex3 = boneIndex;
			boneWeight3 = weight;
		}
		Normalize();
	}

	private void Normalize()
	{
		float num = (float)System.Math.Sqrt(boneWeight0 * boneWeight0 + boneWeight1 * boneWeight1 + boneWeight2 * boneWeight2 + boneWeight3 * boneWeight3);
		if (num > float.Epsilon)
		{
			boneWeight0 /= num;
			boneWeight1 /= num;
			boneWeight2 /= num;
			boneWeight3 /= num;
		}
		else
		{
			boneWeight0 = (boneWeight1 = (boneWeight2 = (boneWeight3 = 0f)));
		}
	}

	public override int GetHashCode()
	{
		return boneIndex0.GetHashCode() ^ (boneIndex1.GetHashCode() << 2) ^ (boneIndex2.GetHashCode() >> 2) ^ (boneIndex3.GetHashCode() >> 1) ^ (boneWeight0.GetHashCode() << 5) ^ (boneWeight1.GetHashCode() << 4) ^ (boneWeight2.GetHashCode() >> 4) ^ (boneWeight3.GetHashCode() >> 3);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is BoneWeight boneWeight))
		{
			return false;
		}
		if (boneIndex0 == boneWeight.boneIndex0 && boneIndex1 == boneWeight.boneIndex1 && boneIndex2 == boneWeight.boneIndex2 && boneIndex3 == boneWeight.boneIndex3 && boneWeight0 == boneWeight.boneWeight0 && boneWeight1 == boneWeight.boneWeight1 && boneWeight2 == boneWeight.boneWeight2)
		{
			return boneWeight3 == boneWeight.boneWeight3;
		}
		return false;
	}

	public bool Equals(BoneWeight other)
	{
		if (boneIndex0 == other.boneIndex0 && boneIndex1 == other.boneIndex1 && boneIndex2 == other.boneIndex2 && boneIndex3 == other.boneIndex3 && boneWeight0 == other.boneWeight0 && boneWeight1 == other.boneWeight1 && boneWeight2 == other.boneWeight2)
		{
			return boneWeight3 == other.boneWeight3;
		}
		return false;
	}

	public override string ToString()
	{
		return string.Format("({0}:{4:F1}, {1}:{5:F1}, {2}:{6:F1}, {3}:{7:F1})", boneIndex0, boneIndex1, boneIndex2, boneIndex3, boneWeight0, boneWeight1, boneWeight2, boneWeight3);
	}

	public static void Merge(ref BoneWeight a, ref BoneWeight b)
	{
		if (b.boneWeight0 > 0f)
		{
			a.MergeBoneWeight(b.boneIndex0, b.boneWeight0);
		}
		if (b.boneWeight1 > 0f)
		{
			a.MergeBoneWeight(b.boneIndex1, b.boneWeight1);
		}
		if (b.boneWeight2 > 0f)
		{
			a.MergeBoneWeight(b.boneIndex2, b.boneWeight2);
		}
		if (b.boneWeight3 > 0f)
		{
			a.MergeBoneWeight(b.boneIndex3, b.boneWeight3);
		}
	}
}
