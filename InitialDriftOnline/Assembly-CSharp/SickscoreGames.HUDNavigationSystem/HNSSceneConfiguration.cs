using UnityEngine;

namespace SickscoreGames.HUDNavigationSystem;

[CreateAssetMenu(fileName = "New Scene Configuration", menuName = "Sickscore Games/HUD Navigation System/New Scene Configuration")]
public class HNSSceneConfiguration : ScriptableObject
{
	public bool overrideRadarSettings;

	public bool overrideCompassBarSettings;

	public bool overrideIndicatorSettings;

	public bool overrideMinimapSettings;

	public bool useRadar = true;

	public RadarModes radarMode;

	public float radarZoom = 1f;

	public float radarRadius = 50f;

	public float radarMaxRadius = 75f;

	public bool useRadarScaling = true;

	public float radarScaleDistance = 15f;

	public float radarMinScale = 0.8f;

	public bool useRadarFading = true;

	public float radarFadeDistance = 10f;

	public float radarMinFade;

	public bool useRadarHeightSystem = true;

	public float radarDistanceAbove = 10f;

	public float radarDistanceBelow = 10f;

	public bool useCompassBar = true;

	public float compassBarRadius = 150f;

	public bool useIndicators = true;

	public float indicatorRadius = 25f;

	public float indicatorHideDistance = 3f;

	public bool useOffscreenIndicators = true;

	public float indicatorOffscreenBorder = 0.075f;

	public bool useIndicatorScaling = true;

	public float indicatorScaleRadius = 15f;

	public float indicatorMinScale = 0.8f;

	public bool useIndicatorFading = true;

	public float indicatorFadeRadius = 15f;

	public float indicatorMinFade;

	public bool useMinimap = true;

	public HNSMapProfile minimapProfile;

	public MinimapModes minimapMode = MinimapModes.RotatePlayer;

	public float minimapScale = 0.25f;

	public float minimapRadius = 75f;

	public bool useMinimapScaling = true;

	public float minimapScaleDistance = 15f;

	public float minimapMinScale = 0.8f;

	public bool useMinimapFading = true;

	public float minimapFadeDistance = 10f;

	public float minimapMinFade;

	public bool showMinimapBounds = true;

	public bool useMinimapHeightSystem = true;

	public float minimapDistanceAbove = 10f;

	public float minimapDistanceBelow = 10f;
}
