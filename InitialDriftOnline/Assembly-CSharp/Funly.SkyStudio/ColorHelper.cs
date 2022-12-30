using UnityEngine;

namespace Funly.SkyStudio;

public abstract class ColorHelper
{
	public static Color ColorWithHex(uint hex)
	{
		return ColorWithHexAlpha((hex << 8) | 0xFFu);
	}

	public static Color ColorWithHexAlpha(uint hex)
	{
		float r = (float)((hex >> 24) & 0xFFu) / 255f;
		float g = (float)((hex >> 16) & 0xFFu) / 255f;
		float b = (float)((hex >> 8) & 0xFFu) / 255f;
		float a = (float)(hex & 0xFFu) / 255f;
		return new Color(r, g, b, a);
	}
}
