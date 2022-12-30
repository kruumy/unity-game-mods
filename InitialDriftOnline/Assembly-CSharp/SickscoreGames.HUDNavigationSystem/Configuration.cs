using System;

namespace SickscoreGames.HUDNavigationSystem;

[Serializable]
public struct Configuration
{
	public HNSSceneAsset _Scene;

	public bool _DisabledInScene;

	public HNSSceneConfiguration _Config;
}
