using UnityEngine;

namespace HellTap.MeshDecimator.Unity.Loggers;

public sealed class UnityLogger : ILogger
{
	public void LogVerbose(string text)
	{
		Debug.Log(text);
	}

	public void LogWarning(string text)
	{
		Debug.LogWarning(text);
	}

	public void LogError(string text)
	{
		Debug.LogError(text);
	}
}
