namespace SpielmannSpiel_Launcher;

public class InitialLauncherScreenSettings
{
	public int launcherWidth;

	public int launcherHeight;

	public bool launcherFullScreen;

	public InitialLauncherScreenSettings()
	{
	}

	public InitialLauncherScreenSettings(int width, int height, bool fullScreen)
	{
		launcherWidth = width;
		launcherHeight = height;
		launcherFullScreen = fullScreen;
	}
}
