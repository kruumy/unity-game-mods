using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SickscoreGames.HUDNavigationSystem;

[AddComponentMenu("HUD Navigation System/HNS Scene Manager")]
[DisallowMultipleComponent]
public class HUDNavigationSceneManager : MonoBehaviour
{
	private static HUDNavigationSceneManager _Instance;

	public List<Configuration> Configurations;

	private HUDNavigationSystem _HUDNavigationSystem;

	public static HUDNavigationSceneManager Instance
	{
		get
		{
			if (_Instance == null)
			{
				_Instance = Object.FindObjectOfType<HUDNavigationSceneManager>();
			}
			return _Instance;
		}
	}

	private void OnEnable()
	{
		SceneManager.activeSceneChanged += OnSceneChanged;
	}

	private void OnDisable()
	{
		SceneManager.activeSceneChanged -= OnSceneChanged;
	}

	private void Awake()
	{
		if (_Instance != null)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			_Instance = this;
		}
	}

	private void Start()
	{
		if (_HUDNavigationSystem == null)
		{
			_HUDNavigationSystem = HUDNavigationSystem.Instance;
		}
		if (_HUDNavigationSystem != null && _HUDNavigationSystem.KeepAliveOnLoad)
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	private void OnSceneChanged(Scene scene, Scene nextScene)
	{
		if (Configurations.Count <= 0)
		{
			Debug.LogWarning("[HNS SceneManager] Could't find any scene configuration!");
			return;
		}
		Configuration configuration = Configurations.Where((Configuration c) => c._Scene != null && c._Config != null && c._Scene.path.Equals(nextScene.path)).FirstOrDefault();
		HNSSceneConfiguration config = configuration._Config;
		if (config == null && !configuration._DisabledInScene)
		{
			Debug.Log("[HNS SceneManager] Configuration is missing for current scene!");
			return;
		}
		if (_HUDNavigationSystem == null)
		{
			_HUDNavigationSystem = HUDNavigationSystem.Instance;
			if (_HUDNavigationSystem == null)
			{
				Debug.LogError("[HNS SceneManager] HUDNavigationSystem not found in scene!");
				base.enabled = false;
				return;
			}
		}
		_HUDNavigationSystem.EnableSystem(!configuration._DisabledInScene);
		if (config != null)
		{
			_HUDNavigationSystem.ApplySceneConfiguration(config);
		}
	}
}
