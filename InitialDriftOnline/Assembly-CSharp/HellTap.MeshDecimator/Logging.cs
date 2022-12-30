using HellTap.MeshDecimator.Loggers;

namespace HellTap.MeshDecimator;

public static class Logging
{
	private static ILogger logger;

	private static object syncObj;

	public static ILogger Logger
	{
		get
		{
			return logger;
		}
		set
		{
			lock (syncObj)
			{
				logger = value;
			}
		}
	}

	static Logging()
	{
		logger = null;
		syncObj = new object();
		logger = new ConsoleLogger();
	}

	public static void LogVerbose(string text)
	{
		lock (syncObj)
		{
			if (logger != null)
			{
				logger.LogVerbose(text);
			}
		}
	}

	public static void LogVerbose(string format, params object[] args)
	{
		LogVerbose(string.Format(format, args));
	}

	public static void LogWarning(string text)
	{
		lock (syncObj)
		{
			if (logger != null)
			{
				logger.LogWarning(text);
			}
		}
	}

	public static void LogWarning(string format, params object[] args)
	{
		LogWarning(string.Format(format, args));
	}

	public static void LogError(string text)
	{
		lock (syncObj)
		{
			if (logger != null)
			{
				logger.LogError(text);
			}
		}
	}

	public static void LogError(string format, params object[] args)
	{
		LogError(string.Format(format, args));
	}
}
