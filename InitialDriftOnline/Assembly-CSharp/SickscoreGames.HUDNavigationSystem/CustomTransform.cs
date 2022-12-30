using System;
using UnityEngine;

namespace SickscoreGames.HUDNavigationSystem;

[Serializable]
public class CustomTransform
{
	[Tooltip("Enter a unique name for this transform.")]
	public string name;

	[Tooltip("Assign the transform you want to add.")]
	public Transform transform;
}
