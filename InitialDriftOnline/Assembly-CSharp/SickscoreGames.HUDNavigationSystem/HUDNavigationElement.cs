using UnityEngine;

namespace SickscoreGames.HUDNavigationSystem;

[AddComponentMenu("HUD Navigation System/HUD Navigation Element")]
public class HUDNavigationElement : MonoBehaviour
{
	public HUDNavigationElementSettings Settings;

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

	public NavigationElementEvent OnElementReady = new NavigationElementEvent();

	public NavigationUpdateEvent OnElementUpdate = new NavigationUpdateEvent();

	public NavigationTypeEvent OnAppear = new NavigationTypeEvent();

	public NavigationTypeEvent OnDisappear = new NavigationTypeEvent();

	public NavigationTypeEvent OnEnterRadius = new NavigationTypeEvent();

	public NavigationTypeEvent OnLeaveRadius = new NavigationTypeEvent();

	[HideInInspector]
	public bool IsActive = true;

	[HideInInspector]
	public HNSRadarPrefab Radar;

	[HideInInspector]
	public HNSCompassBarPrefab CompassBar;

	[HideInInspector]
	public HNSIndicatorPrefab Indicator;

	[HideInInspector]
	public HNSMinimapPrefab Minimap;

	[HideInInspector]
	public bool IsInRadarRadius;

	[HideInInspector]
	public bool IsInCompassBarRadius;

	[HideInInspector]
	public bool IsInIndicatorRadius;

	[HideInInspector]
	public bool IsInMinimapRadius;

	protected bool _isInitialized;

	protected virtual void Start()
	{
		if (HUDNavigationSystem.Instance == null)
		{
			Debug.LogError("HUDNavigationSystem not found in scene!");
			base.enabled = false;
		}
		else
		{
			InitializeSettings();
			Initialize();
		}
	}

	protected virtual void OnEnable()
	{
		if (_isInitialized)
		{
			Initialize();
		}
	}

	protected virtual void OnDisable()
	{
		if (HUDNavigationSystem.Instance != null)
		{
			HUDNavigationSystem.Instance.RemoveNavigationElement(this);
		}
		if (Radar != null)
		{
			Object.Destroy(Radar.gameObject);
		}
		if (CompassBar != null)
		{
			Object.Destroy(CompassBar.gameObject);
		}
		if (Indicator != null)
		{
			Object.Destroy(Indicator.gameObject);
		}
		if (Minimap != null)
		{
			Object.Destroy(Minimap.gameObject);
		}
	}

	protected virtual void Refresh()
	{
		base.enabled = false;
		Radar = null;
		CompassBar = null;
		Indicator = null;
		Minimap = null;
		CreateMarkerReferences();
		base.enabled = true;
	}

	protected virtual void InitializeSettings()
	{
		if (!(Settings == null))
		{
			Prefabs = Settings.Prefabs;
			hideInRadar = Settings.hideInRadar;
			ignoreRadarRadius = Settings.ignoreRadarRadius;
			ignoreRadarScaling = Settings.ignoreRadarScaling;
			ignoreRadarFading = Settings.ignoreRadarFading;
			rotateWithGameObject = Settings.rotateWithGameObject;
			useRadarHeightSystem = Settings.useRadarHeightSystem;
			hideInCompassBar = Settings.hideInCompassBar;
			ignoreCompassBarRadius = Settings.ignoreCompassBarRadius;
			useCompassBarDistanceText = Settings.useCompassBarDistanceText;
			compassBarDistanceTextFormat = Settings.compassBarDistanceTextFormat;
			showIndicator = Settings.showIndicator;
			showOffscreenIndicator = Settings.showOffscreenIndicator;
			ignoreIndicatorRadius = Settings.ignoreIndicatorRadius;
			ignoreIndicatorHideDistance = Settings.ignoreIndicatorHideDistance;
			ignoreIndicatorScaling = Settings.ignoreIndicatorScaling;
			ignoreIndicatorFading = Settings.ignoreIndicatorFading;
			useIndicatorDistanceText = Settings.useIndicatorDistanceText;
			showOffscreenIndicatorDistance = Settings.showOffscreenIndicatorDistance;
			indicatorOnscreenDistanceTextFormat = Settings.indicatorOnscreenDistanceTextFormat;
			indicatorOffscreenDistanceTextFormat = Settings.indicatorOffscreenDistanceTextFormat;
			hideInMinimap = Settings.hideInMinimap;
			ignoreMinimapRadius = Settings.ignoreMinimapRadius;
			ignoreMinimapScaling = Settings.ignoreMinimapScaling;
			ignoreMinimapFading = Settings.ignoreMinimapFading;
			rotateWithGameObjectMM = Settings.rotateWithGameObjectMM;
			useMinimapHeightSystem = Settings.useMinimapHeightSystem;
		}
	}

	protected virtual void Initialize()
	{
		CreateMarkerReferences();
		if (HUDNavigationSystem.Instance != null)
		{
			HUDNavigationSystem.Instance.AddNavigationElement(this);
		}
		_isInitialized = true;
		OnElementReady.Invoke(this);
	}

	protected virtual void CreateMarkerReferences()
	{
		CreateRadarMarker();
		CreateCompassBarMarker();
		CreateIndicatorMarker();
		CreateMinimapMarker();
	}

	private void CreateRadarMarker()
	{
		if (!(Prefabs.RadarPrefab == null))
		{
			GameObject gameObject = Object.Instantiate(Prefabs.RadarPrefab.gameObject, Vector3.zero, Quaternion.identity);
			gameObject.transform.SetParent(HUDNavigationCanvas.Instance.Radar.ElementContainer, worldPositionStays: false);
			gameObject.SetActive(value: false);
			Radar = gameObject.GetComponent<HNSRadarPrefab>();
		}
	}

	private void CreateCompassBarMarker()
	{
		if (!(Prefabs.CompassBarPrefab == null))
		{
			GameObject gameObject = Object.Instantiate(Prefabs.CompassBarPrefab.gameObject, Vector3.zero, Quaternion.identity);
			gameObject.transform.SetParent(HUDNavigationCanvas.Instance.CompassBar.ElementContainer, worldPositionStays: false);
			gameObject.SetActive(value: false);
			CompassBar = gameObject.GetComponent<HNSCompassBarPrefab>();
		}
	}

	private void CreateIndicatorMarker()
	{
		if (!(Prefabs.IndicatorPrefab == null))
		{
			GameObject gameObject = Object.Instantiate(Prefabs.IndicatorPrefab.gameObject, Vector3.zero, Quaternion.identity);
			gameObject.transform.SetParent(HUDNavigationCanvas.Instance.Indicator.ElementContainer, worldPositionStays: false);
			gameObject.SetActive(value: false);
			Indicator = gameObject.GetComponent<HNSIndicatorPrefab>();
		}
	}

	private void CreateMinimapMarker()
	{
		if (!(Prefabs.MinimapPrefab == null))
		{
			GameObject gameObject = Object.Instantiate(Prefabs.MinimapPrefab.gameObject, Vector3.zero, Quaternion.identity);
			gameObject.transform.SetParent(HUDNavigationCanvas.Instance.Minimap.ElementContainer, worldPositionStays: false);
			gameObject.SetActive(value: false);
			Minimap = gameObject.GetComponent<HNSMinimapPrefab>();
		}
	}
}
