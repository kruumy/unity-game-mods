using UnityEngine;

namespace SickscoreGames.HUDNavigationSystem;

[CreateAssetMenu(fileName = "New Element Settings", menuName = "Sickscore Games/HUD Navigation System/New Element Settings")]
public class HUDNavigationElementSettings : ScriptableObject
{
	public HNSPrefabs Prefabs = new HNSPrefabs();

	public bool hideInRadar;

	public bool ignoreRadarRadius;

	public bool ignoreRadarScaling;

	public bool ignoreRadarFading;

	public bool rotateWithGameObject = true;

	public bool useRadarHeightSystem = true;

	public bool hideInCompassBar;

	public bool ignoreCompassBarRadius;

	public bool useCompassBarDistanceText = true;

	public string compassBarDistanceTextFormat = "{0}m";

	public bool showIndicator = true;

	public bool showOffscreenIndicator = true;

	public bool ignoreIndicatorRadius = true;

	public bool ignoreIndicatorHideDistance;

	public bool ignoreIndicatorScaling;

	public bool ignoreIndicatorFading;

	public bool useIndicatorDistanceText = true;

	public bool showOffscreenIndicatorDistance;

	public string indicatorOnscreenDistanceTextFormat = "{0}m";

	public string indicatorOffscreenDistanceTextFormat = "{0}";

	public bool hideInMinimap;

	public bool ignoreMinimapRadius;

	public bool ignoreMinimapScaling;

	public bool ignoreMinimapFading;

	public bool rotateWithGameObjectMM = true;

	public bool useMinimapHeightSystem = true;

	public void CopySettings(HUDNavigationElement element)
	{
		if (!(element == null))
		{
			Prefabs = element.Prefabs;
			hideInRadar = element.hideInRadar;
			ignoreRadarRadius = element.ignoreRadarRadius;
			ignoreRadarScaling = element.ignoreRadarScaling;
			ignoreRadarFading = element.ignoreRadarFading;
			rotateWithGameObject = element.rotateWithGameObject;
			useRadarHeightSystem = element.useRadarHeightSystem;
			hideInCompassBar = element.hideInCompassBar;
			ignoreCompassBarRadius = element.ignoreCompassBarRadius;
			useCompassBarDistanceText = element.useCompassBarDistanceText;
			compassBarDistanceTextFormat = element.compassBarDistanceTextFormat;
			showIndicator = element.showIndicator;
			showOffscreenIndicator = element.showOffscreenIndicator;
			ignoreIndicatorRadius = element.ignoreIndicatorRadius;
			ignoreIndicatorHideDistance = element.ignoreIndicatorHideDistance;
			ignoreIndicatorScaling = element.ignoreIndicatorScaling;
			ignoreIndicatorFading = element.ignoreIndicatorFading;
			useIndicatorDistanceText = element.useIndicatorDistanceText;
			showOffscreenIndicatorDistance = element.showOffscreenIndicatorDistance;
			indicatorOnscreenDistanceTextFormat = element.indicatorOnscreenDistanceTextFormat;
			indicatorOffscreenDistanceTextFormat = element.indicatorOffscreenDistanceTextFormat;
			hideInMinimap = element.hideInMinimap;
			ignoreMinimapRadius = element.ignoreMinimapRadius;
			ignoreMinimapScaling = element.ignoreMinimapScaling;
			ignoreMinimapFading = element.ignoreMinimapFading;
			rotateWithGameObjectMM = element.rotateWithGameObjectMM;
			useMinimapHeightSystem = element.useMinimapHeightSystem;
		}
	}
}
