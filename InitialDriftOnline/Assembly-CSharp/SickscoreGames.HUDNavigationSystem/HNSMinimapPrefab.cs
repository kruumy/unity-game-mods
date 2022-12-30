using UnityEngine;
using UnityEngine.UI;

namespace SickscoreGames.HUDNavigationSystem;

[AddComponentMenu("HUD Navigation System/HNS Minimap Prefab")]
public class HNSMinimapPrefab : HNSPrefab
{
	[Header("Icon")]
	[Tooltip("Assign an image component.")]
	public Image Icon;

	[Header("Height Arrows")]
	[Tooltip("Assign the above arrow image component.")]
	public Image ArrowAbove;

	[Tooltip("Assign the above arrow image component.")]
	public Image ArrowBelow;

	protected override void OnEnable()
	{
		base.OnEnable();
		PrefabCanvasGroup = GetComponent<CanvasGroup>();
		if (PrefabCanvasGroup == null)
		{
			PrefabCanvasGroup = base.gameObject.AddComponent<CanvasGroup>();
			CanvasGroup prefabCanvasGroup = PrefabCanvasGroup;
			bool interactable = (PrefabCanvasGroup.blocksRaycasts = false);
			prefabCanvasGroup.interactable = interactable;
		}
	}

	public override void ChangeIconColor(Color color)
	{
		base.ChangeIconColor(color);
		if (Icon != null)
		{
			Icon.color = color;
		}
	}
}
