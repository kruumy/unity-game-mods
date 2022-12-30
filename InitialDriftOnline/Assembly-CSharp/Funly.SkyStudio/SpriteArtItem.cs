using UnityEngine;

namespace Funly.SkyStudio;

public class SpriteArtItem : ScriptableObject
{
	public Mesh mesh;

	public Material material;

	public Texture spriteSheetTexture;

	public int rows;

	public int columns;

	public int totalFrames;

	public int animateSpeed;

	[Tooltip("Color that will be multiplied against the base lightning bolt text color")]
	public Color tintColor = Color.white;
}
