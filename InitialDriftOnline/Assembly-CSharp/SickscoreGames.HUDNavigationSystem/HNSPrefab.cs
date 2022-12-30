using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SickscoreGames.HUDNavigationSystem;

public class HNSPrefab : MonoBehaviour
{
	[HideInInspector]
	public List<CustomTransform> CustomTransforms = new List<CustomTransform>();

	[HideInInspector]
	public RectTransform PrefabRect;

	[HideInInspector]
	public CanvasGroup PrefabCanvasGroup;

	protected virtual void OnEnable()
	{
		PrefabRect = GetComponent<RectTransform>();
	}

	public Transform GetCustomTransform(string name)
	{
		return CustomTransforms.FirstOrDefault((CustomTransform ct) => ct.name.Equals(name))?.transform;
	}

	public virtual void ChangeIconColor(Color color)
	{
	}
}
