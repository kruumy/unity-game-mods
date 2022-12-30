using System;
using UnityEngine;

namespace SickscoreGames.HUDNavigationSystem;

[Serializable]
public class CustomLayer
{
	[Tooltip("Enter a unique name for this layer.")]
	public string name;

	[Tooltip("Assign the sprite texture you want to add.")]
	public Sprite sprite;

	[Tooltip("If checked, this layer will be enabled by default.")]
	public bool enabled;

	[HideInInspector]
	public GameObject instance;
}
