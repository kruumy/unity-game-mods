using System;

namespace SpielmannSpiel_Launcher;

[Serializable]
public class FpsInfo
{
	public string name = "";

	public int fps;

	public override string ToString()
	{
		if (name != "")
		{
			return name;
		}
		return fps.ToString();
	}
}
