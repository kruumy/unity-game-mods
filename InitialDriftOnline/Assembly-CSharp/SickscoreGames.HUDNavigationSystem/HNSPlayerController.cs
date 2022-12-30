using UnityEngine;

namespace SickscoreGames.HUDNavigationSystem;

[AddComponentMenu("HUD Navigation System/HNS Player Controller")]
public class HNSPlayerController : MonoBehaviour
{
	private void Start()
	{
		if (HUDNavigationSystem.Instance != null)
		{
			HUDNavigationSystem.Instance.ChangePlayerController(base.gameObject.transform);
		}
	}
}
