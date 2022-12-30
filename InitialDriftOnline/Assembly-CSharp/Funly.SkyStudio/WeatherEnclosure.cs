using UnityEngine;

namespace Funly.SkyStudio;

[RequireComponent(typeof(MeshRenderer))]
public class WeatherEnclosure : MonoBehaviour
{
	public Vector2 nearTextureTiling = new Vector3(1f, 1f);

	public Vector2 farTextureTiling = new Vector2(1f, 1f);
}
