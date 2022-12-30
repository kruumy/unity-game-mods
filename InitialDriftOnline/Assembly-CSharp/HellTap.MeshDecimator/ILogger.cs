namespace HellTap.MeshDecimator;

public interface ILogger
{
	void LogVerbose(string text);

	void LogWarning(string text);

	void LogError(string text);
}
