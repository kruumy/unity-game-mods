using UnityEngine;
using UnityEngine.UI;

namespace Funly.SkyStudio;

[RequireComponent(typeof(RawImage))]
public class LoadOverheadDepthTexture : MonoBehaviour
{
	private WeatherDepthCamera m_RainCamera;

	private RawImage m_Image;

	private void Start()
	{
		m_RainCamera = Object.FindObjectOfType<WeatherDepthCamera>();
		m_Image = GetComponent<RawImage>();
	}

	private void Update()
	{
		m_Image.texture = m_RainCamera.overheadDepthTexture;
	}
}
