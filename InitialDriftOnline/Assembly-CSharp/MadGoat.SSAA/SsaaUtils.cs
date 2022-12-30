using UnityEngine;

namespace MadGoat.SSAA;

public static class SsaaUtils
{
	public const string ssaaversion = "2.0.12";

	public static void CopyFrom(this Camera current, Camera other, RenderTexture rt)
	{
		current.CopyFrom(other);
		current.targetTexture = rt;
	}
}
