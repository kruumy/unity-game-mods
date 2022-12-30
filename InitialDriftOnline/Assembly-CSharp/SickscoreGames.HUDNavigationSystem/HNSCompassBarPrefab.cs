using UnityEngine;
using UnityEngine.UI;

namespace SickscoreGames.HUDNavigationSystem;

[AddComponentMenu("HUD Navigation System/HNS CompassBar Prefab")]
public class HNSCompassBarPrefab : HNSPrefab
{
	[Header("Icon")]
	[Tooltip("Assign an image component.")]
	public Image Icon;

	[Header("Distance Text")]
	[Tooltip("Assign the distance text component.")]
	public Text DistanceText;

	public override void ChangeIconColor(Color color)
	{
		base.ChangeIconColor(color);
		if (Icon != null)
		{
			Icon.color = color;
		}
	}
}
