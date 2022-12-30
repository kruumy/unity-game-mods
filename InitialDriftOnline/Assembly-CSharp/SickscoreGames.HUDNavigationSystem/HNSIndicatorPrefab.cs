using UnityEngine;
using UnityEngine.UI;

namespace SickscoreGames.HUDNavigationSystem;

[AddComponentMenu("HUD Navigation System/HNS Indicator Prefab")]
public class HNSIndicatorPrefab : HNSPrefab
{
	[Header("Onscreen")]
	[Tooltip("Assign the transform for the onscreen marker.")]
	public RectTransform OnscreenRect;

	[Tooltip("Assign an onscreen image component.")]
	public Image OnscreenIcon;

	[Header("Offscreen")]
	[Tooltip("Assign the transform for the offscreen marker.")]
	public RectTransform OffscreenRect;

	[Tooltip("Assign the transform which should rotate towards the offscreen element.")]
	public RectTransform OffscreenPointer;

	[Tooltip("Assign an offscreen image component.")]
	public Image OffscreenIcon;

	[Header("Distance Text")]
	[Tooltip("(optional) Assign an onscreen distance text component.")]
	public Text OnscreenDistanceText;

	[Tooltip("(optional) Assign an offscreen distance text component.")]
	public Text OffscreenDistanceText;

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

	public void ChangeOffscreenIconColor(Color color)
	{
		if (OffscreenIcon != null)
		{
			OffscreenIcon.color = color;
		}
	}

	public override void ChangeIconColor(Color color)
	{
		base.ChangeIconColor(color);
		if (OnscreenIcon != null)
		{
			OnscreenIcon.color = color;
		}
	}
}
