using System;

namespace HellTap.MeshDecimator.Loggers;

public sealed class ConsoleLogger : ILogger
{
	public void LogVerbose(string text)
	{
		Console.WriteLine(text);
	}

	public void LogWarning(string text)
	{
		Console.WriteLine(text);
	}

	public void LogError(string text)
	{
		Console.Error.WriteLine(text);
	}
}
