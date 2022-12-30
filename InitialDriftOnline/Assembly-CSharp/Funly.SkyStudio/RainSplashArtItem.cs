using UnityEngine;

namespace Funly.SkyStudio;

[CreateAssetMenu(fileName = "rainSplashArtItem.asset", menuName = "Sky Studio/Rain/Rain Splash Art Item")]
public class RainSplashArtItem : SpriteArtItem
{
	[Range(0f, 1f)]
	public float intensityMultiplier = 1f;

	[Range(0f, 1f)]
	public float scaleMultiplier = 1f;
}
