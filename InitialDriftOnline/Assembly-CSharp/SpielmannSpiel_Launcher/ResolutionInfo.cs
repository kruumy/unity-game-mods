using UnityEngine;

namespace SpielmannSpiel_Launcher;

public class ResolutionInfo
{
	public string label = "";

	public Vector2 size;

	public Resolution resolution;

	public ResolutionInfo(Resolution res)
	{
		resolution = res;
		size = new Vector2(res.width, res.height);
		label = res.width + " x " + res.height;
	}

	public override string ToString()
	{
		return label;
	}
}
