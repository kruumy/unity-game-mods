using System;
using HellTap.MeshDecimator.Math;

namespace HellTap.MeshDecimator.Algorithms;

public abstract class DecimationAlgorithm
{
	public delegate void StatusReportCallback(int iteration, int originalTris, int currentTris, int targetTris);

	private bool preserveBorders;

	private int maxVertexCount;

	private bool verbose;

	private StatusReportCallback statusReportInvoker;

	[Obsolete("Use the 'DecimationAlgorithm.PreserveBorders' property instead.", false)]
	public bool KeepBorders
	{
		get
		{
			return preserveBorders;
		}
		set
		{
			preserveBorders = value;
		}
	}

	public bool PreserveBorders
	{
		get
		{
			return preserveBorders;
		}
		set
		{
			preserveBorders = value;
		}
	}

	[Obsolete("This feature has been removed, for more details why please read the readme.", true)]
	public bool KeepLinkedVertices
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public int MaxVertexCount
	{
		get
		{
			return maxVertexCount;
		}
		set
		{
			maxVertexCount = MathHelper.Max(value, 0);
		}
	}

	public bool Verbose
	{
		get
		{
			return verbose;
		}
		set
		{
			verbose = value;
		}
	}

	public event StatusReportCallback StatusReport
	{
		add
		{
			statusReportInvoker = (StatusReportCallback)Delegate.Combine(statusReportInvoker, value);
		}
		remove
		{
			statusReportInvoker = (StatusReportCallback)Delegate.Remove(statusReportInvoker, value);
		}
	}

	protected void ReportStatus(int iteration, int originalTris, int currentTris, int targetTris)
	{
		statusReportInvoker?.Invoke(iteration, originalTris, currentTris, targetTris);
	}

	public abstract void Initialize(Mesh mesh);

	public abstract void DecimateMesh(int targetTrisCount);

	public abstract void DecimateMeshLossless();

	public abstract Mesh ToMesh();
}
