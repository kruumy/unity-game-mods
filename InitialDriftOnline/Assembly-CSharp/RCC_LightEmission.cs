using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Light/RCC Light Emission")]
public class RCC_LightEmission : MonoBehaviour
{
	private Light sharedLight;

	public Renderer lightRenderer;

	public int materialIndex;

	public bool noTexture;

	public bool applyAlpha;

	public float multiplier = 1f;

	private int emissionColorID;

	private int colorID;

	private Material material;

	private Color targetColor;

	private void Start()
	{
		sharedLight = GetComponent<Light>();
		material = lightRenderer.materials[materialIndex];
		material.EnableKeyword("_EMISSION");
		emissionColorID = Shader.PropertyToID("_EmissionColor");
		colorID = Shader.PropertyToID("_Color");
		if (!material.HasProperty(emissionColorID))
		{
			base.enabled = false;
		}
	}

	private void Update()
	{
		if (!sharedLight.enabled)
		{
			targetColor = Color.white * 0f;
		}
		if (!noTexture)
		{
			targetColor = Color.white * sharedLight.intensity * multiplier;
		}
		else
		{
			targetColor = sharedLight.color * sharedLight.intensity * multiplier;
		}
		if (applyAlpha)
		{
			material.SetColor(colorID, new Color(1f, 1f, 1f, sharedLight.intensity * multiplier));
		}
		if (material.GetColor(emissionColorID) != targetColor)
		{
			material.SetColor(emissionColorID, targetColor);
		}
	}
}
