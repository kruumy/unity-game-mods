using System;
using HellTap.MeshDecimator.Algorithms;

namespace HellTap.MeshDecimator;

public static class MeshDecimation
{
	public static DecimationAlgorithm CreateAlgorithm(Algorithm algorithm, bool preserveBorders = false, bool preserveSeams = false, bool preserveFoldovers = false)
	{
		DecimationAlgorithm decimationAlgorithm = null;
		if ((uint)algorithm <= 1u)
		{
			return new FastQuadricMeshSimplification(preserveBorders, preserveSeams, preserveFoldovers);
		}
		throw new ArgumentException("The specified algorithm is not supported.", "algorithm");
	}

	public static Mesh DecimateMesh(Mesh mesh, int targetTriangleCount)
	{
		return DecimateMesh(Algorithm.Default, mesh, targetTriangleCount);
	}

	public static Mesh DecimateMesh(Algorithm algorithm, Mesh mesh, int targetTriangleCount, bool preserveBorders = false, bool preserveSeams = false, bool preserveFoldovers = false)
	{
		if (mesh == null)
		{
			throw new ArgumentNullException("mesh");
		}
		return DecimateMesh(CreateAlgorithm(algorithm, preserveBorders, preserveSeams, preserveFoldovers), mesh, targetTriangleCount);
	}

	public static Mesh DecimateMesh(DecimationAlgorithm algorithm, Mesh mesh, int targetTriangleCount)
	{
		if (algorithm == null)
		{
			throw new ArgumentNullException("algorithm");
		}
		if (mesh == null)
		{
			throw new ArgumentNullException("mesh");
		}
		int triangleCount = mesh.TriangleCount;
		if (targetTriangleCount > triangleCount)
		{
			targetTriangleCount = triangleCount;
		}
		else if (targetTriangleCount < 0)
		{
			targetTriangleCount = 0;
		}
		algorithm.Initialize(mesh);
		algorithm.DecimateMesh(targetTriangleCount);
		return algorithm.ToMesh();
	}

	public static Mesh DecimateMeshLossless(Mesh mesh)
	{
		return DecimateMeshLossless(Algorithm.Default, mesh);
	}

	public static Mesh DecimateMeshLossless(Algorithm algorithm, Mesh mesh, bool preserveBorders = false, bool preserveSeams = false, bool preserveFoldovers = false)
	{
		if (mesh == null)
		{
			throw new ArgumentNullException("mesh");
		}
		return DecimateMeshLossless(CreateAlgorithm(algorithm, preserveBorders, preserveSeams, preserveFoldovers), mesh);
	}

	public static Mesh DecimateMeshLossless(DecimationAlgorithm algorithm, Mesh mesh)
	{
		if (algorithm == null)
		{
			throw new ArgumentNullException("algorithm");
		}
		if (mesh == null)
		{
			throw new ArgumentNullException("mesh");
		}
		_ = mesh.TriangleCount;
		algorithm.Initialize(mesh);
		algorithm.DecimateMeshLossless();
		return algorithm.ToMesh();
	}
}
