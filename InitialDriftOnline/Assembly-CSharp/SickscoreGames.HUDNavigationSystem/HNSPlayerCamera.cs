using UnityEngine;

namespace SickscoreGames.HUDNavigationSystem;

[AddComponentMenu("HUD Navigation System/HNS Player Camera")]
public class HNSPlayerCamera : MonoBehaviour
{
	private void Start()
	{
		if (HUDNavigationSystem.Instance != null)
		{
			Camera component = base.gameObject.GetComponent<Camera>();
			HUDNavigationSystem.Instance.ChangePlayerCamera(component);
		}
	}
}
