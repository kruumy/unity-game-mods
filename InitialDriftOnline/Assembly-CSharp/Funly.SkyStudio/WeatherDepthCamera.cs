using UnityEngine;

namespace Funly.SkyStudio;

[RequireComponent(typeof(Camera))]
public class WeatherDepthCamera : MonoBehaviour
{
	private Camera m_DepthCamera;

	[Tooltip("Shader used to render out depth + normal texture. This should be the sky studio depth shader.")]
	public Shader depthShader;

	[HideInInspector]
	public RenderTexture overheadDepthTexture;

	[Tooltip("You can help increase performance by only rendering periodically some number of frames.")]
	[Range(1f, 60f)]
	public int renderFrameInterval = 5;

	[Tooltip("The resolution of the texture. Higher resolution uses more rendering time but makes more precise weather along edges.")]
	[Range(128f, 8192f)]
	public int textureResolution = 1024;

	private void Start()
	{
		m_DepthCamera = GetComponent<Camera>();
		m_DepthCamera.enabled = false;
	}

	private void Update()
	{
		if (m_DepthCamera.enabled)
		{
			m_DepthCamera.enabled = false;
		}
		if (Time.frameCount % renderFrameInterval == 0)
		{
			RenderOverheadCamera();
		}
	}

	private void RenderOverheadCamera()
	{
		PrepareRenderTexture();
		if (depthShader == null)
		{
			Debug.LogError("Can't render depth since depth shader is missing.");
			return;
		}
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = overheadDepthTexture;
		GL.Clear(clearDepth: true, clearColor: true, Color.black);
		m_DepthCamera.RenderWithShader(depthShader, "RenderType");
		RenderTexture.active = active;
		Shader.SetGlobalTexture("_OverheadDepthTex", overheadDepthTexture);
		Shader.SetGlobalVector("_OverheadDepthPosition", m_DepthCamera.transform.position);
		Shader.SetGlobalFloat("_OverheadDepthNearClip", m_DepthCamera.nearClipPlane);
		Shader.SetGlobalFloat("_OverheadDepthFarClip", m_DepthCamera.farClipPlane);
	}

	private void PrepareRenderTexture()
	{
		if (overheadDepthTexture == null)
		{
			int num = Mathf.ClosestPowerOfTwo(Mathf.FloorToInt(textureResolution));
			RenderTextureFormat format = RenderTextureFormat.ARGB32;
			overheadDepthTexture = new RenderTexture(num, num, 24, format, RenderTextureReadWrite.Linear);
			overheadDepthTexture.useMipMap = false;
			overheadDepthTexture.autoGenerateMips = false;
			overheadDepthTexture.filterMode = FilterMode.Point;
			overheadDepthTexture.antiAliasing = 2;
		}
		if (!overheadDepthTexture.IsCreated())
		{
			overheadDepthTexture.Create();
		}
		if (m_DepthCamera.targetTexture != overheadDepthTexture)
		{
			m_DepthCamera.targetTexture = overheadDepthTexture;
		}
	}
}
