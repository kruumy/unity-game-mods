using UnityEngine;

public class RoomListSky : MonoBehaviour
{
	public Material MidBox;

	public Light DirectionalLight;

	private void Start()
	{
		RenderSettings.skybox = MidBox;
		DirectionalLight.color = new Color32(byte.MaxValue, 190, 130, byte.MaxValue);
		DirectionalLight.intensity = 0.8f;
		RenderSettings.ambientIntensity = 0.8f;
		RenderSettings.reflectionIntensity = 0.5f;
		DirectionalLight.shadowStrength = 0.6f;
	}

	private void Update()
	{
	}
}
