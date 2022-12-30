using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SickscoreGames.HUDNavigationSystem;

public class HNSMapProfile : ScriptableObject
{
	public Sprite MapTexture;

	public Vector2 MapTextureSize;

	public Color MapBackground = Color.black;

	public Bounds MapBounds = new Bounds(Vector3.zero, Vector3.one);

	[HideInInspector]
	public List<CustomLayer> CustomLayers = new List<CustomLayer>();

	public void Init(Sprite mapTexture, Vector2 mapTextureSize, Color mapBackground, Bounds mapBounds)
	{
		MapTexture = mapTexture;
		MapTextureSize = new Vector2((int)mapTextureSize.x, (int)mapTextureSize.y);
		MapBackground = mapBackground;
		MapBounds = mapBounds;
	}

	public GameObject GetCustomLayer(string name)
	{
		return CustomLayers.FirstOrDefault((CustomLayer cl) => cl.name.Equals(name))?.instance;
	}
}
