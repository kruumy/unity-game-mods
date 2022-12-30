using UnityEngine;

namespace UniStorm.Effects;

public class ScreenSpaceCloudShadows : MonoBehaviour
{
	[HideInInspector]
	public float Fade = 0.33f;

	[HideInInspector]
	public RenderTexture CloudShadowTexture;

	[HideInInspector]
	public Color ShadowColor = Color.white;

	[HideInInspector]
	public float CloudTextureScale = 0.1f;

	[HideInInspector]
	[Range(0f, 1f)]
	public float BottomThreshold;

	[HideInInspector]
	[Range(0f, 1f)]
	public float TopThreshold = 1f;

	[HideInInspector]
	public float ShadowIntensity = 1f;

	[HideInInspector]
	public Material ScreenSpaceShadowsMaterial;

	[HideInInspector]
	public Vector3 ShadowDirection;

	private void OnEnable()
	{
		ScreenSpaceShadowsMaterial = new Material(Shader.Find("UniStorm/Celestial/Screen Space Cloud Shadows"));
		GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		if (Application.isPlaying)
		{
			ScreenSpaceShadowsMaterial.SetMatrix("_CamToWorld", UniStormSystem.Instance.PlayerCamera.cameraToWorldMatrix);
			ScreenSpaceShadowsMaterial.SetTexture("_CloudTex", CloudShadowTexture);
			ScreenSpaceShadowsMaterial.SetFloat("_CloudTexScale", CloudTextureScale + UniStormSystem.Instance.m_CurrentCloudHeight * 1E-06f * 2f);
			ScreenSpaceShadowsMaterial.SetFloat("_BottomThreshold", BottomThreshold);
			ScreenSpaceShadowsMaterial.SetFloat("_TopThreshold", TopThreshold);
			ScreenSpaceShadowsMaterial.SetFloat("_CloudShadowIntensity", ShadowIntensity);
			ScreenSpaceShadowsMaterial.SetFloat("_CloudMovementSpeed", (float)UniStormSystem.Instance.CloudSpeed * -0.005f);
			ScreenSpaceShadowsMaterial.SetVector("_SunDirection", new Vector3(ShadowDirection.x, ShadowDirection.y, ShadowDirection.z));
			ScreenSpaceShadowsMaterial.SetFloat("_Fade", Fade);
			Graphics.Blit(src, dest, ScreenSpaceShadowsMaterial);
		}
	}
}
