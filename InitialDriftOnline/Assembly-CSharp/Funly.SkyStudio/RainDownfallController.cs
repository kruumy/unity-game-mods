using UnityEngine;

namespace Funly.SkyStudio;

[RequireComponent(typeof(AudioSource))]
public class RainDownfallController : MonoBehaviour, ISkyModule
{
	public MeshRenderer rainMeshRenderer;

	public Material rainMaterial;

	private MaterialPropertyBlock m_PropertyBlock;

	private AudioSource m_RainAudioSource;

	private float m_TimeOfDay;

	private SkyProfile m_SkyProfile;

	public void SetWeatherEnclosure(WeatherEnclosure enclosure)
	{
		if (rainMeshRenderer != null)
		{
			rainMeshRenderer.enabled = false;
			rainMeshRenderer = null;
		}
		if (!enclosure)
		{
			return;
		}
		rainMeshRenderer = enclosure.GetComponentInChildren<MeshRenderer>();
		if (!rainMeshRenderer)
		{
			Debug.LogError("Can't render rain since there's no MeshRenderer on the WeatherEnclosure");
			return;
		}
		m_PropertyBlock = new MaterialPropertyBlock();
		if ((bool)rainMaterial)
		{
			rainMeshRenderer.material = rainMaterial;
			rainMeshRenderer.enabled = true;
			UpdateForTimeOfDay(m_SkyProfile, m_TimeOfDay);
		}
	}

	private void Update()
	{
		if (!(m_SkyProfile == null))
		{
			UpdateForTimeOfDay(m_SkyProfile, m_TimeOfDay);
		}
	}

	public void UpdateForTimeOfDay(SkyProfile skyProfile, float timeOfDay)
	{
		m_SkyProfile = skyProfile;
		m_TimeOfDay = timeOfDay;
		if (!skyProfile)
		{
			return;
		}
		if (m_RainAudioSource == null)
		{
			m_RainAudioSource = GetComponent<AudioSource>();
		}
		if (skyProfile == null || !m_SkyProfile.IsFeatureEnabled("RainFeature"))
		{
			if (m_RainAudioSource != null)
			{
				m_RainAudioSource.enabled = false;
			}
			return;
		}
		if (!rainMaterial)
		{
			Debug.LogError("Can't render rain without a rain material");
			return;
		}
		if (!rainMeshRenderer)
		{
			Debug.LogError("Can't show rain without an enclosure mesh renderer.");
			return;
		}
		if (m_PropertyBlock == null)
		{
			m_PropertyBlock = new MaterialPropertyBlock();
		}
		rainMeshRenderer.enabled = true;
		rainMeshRenderer.material = rainMaterial;
		rainMeshRenderer.GetPropertyBlock(m_PropertyBlock);
		float numberPropertyValue = skyProfile.GetNumberPropertyValue("RainNearIntensityKey", timeOfDay);
		float numberPropertyValue2 = skyProfile.GetNumberPropertyValue("RainFarIntensityKey", timeOfDay);
		Texture texturePropertyValue = skyProfile.GetTexturePropertyValue("RainNearTextureKey", timeOfDay);
		Texture texturePropertyValue2 = skyProfile.GetTexturePropertyValue("RainFarTextureKey", timeOfDay);
		float numberPropertyValue3 = skyProfile.GetNumberPropertyValue("RainNearSpeedKey", timeOfDay);
		float numberPropertyValue4 = skyProfile.GetNumberPropertyValue("RainFarSpeedKey", timeOfDay);
		Color colorPropertyValue = m_SkyProfile.GetColorPropertyValue("RainTintColorKey", m_TimeOfDay);
		float numberPropertyValue5 = m_SkyProfile.GetNumberPropertyValue("RainWindTurbulenceKey", m_TimeOfDay);
		float numberPropertyValue6 = m_SkyProfile.GetNumberPropertyValue("RainWindTurbulenceSpeedKey", m_TimeOfDay);
		float numberPropertyValue7 = m_SkyProfile.GetNumberPropertyValue("RainNearTextureTiling", m_TimeOfDay);
		float numberPropertyValue8 = m_SkyProfile.GetNumberPropertyValue("RainFarTextureTiling", m_TimeOfDay);
		if (texturePropertyValue != null)
		{
			m_PropertyBlock.SetTexture("_NearTex", texturePropertyValue);
			m_PropertyBlock.SetVector("_NearTex_ST", new Vector4(numberPropertyValue7, numberPropertyValue7, numberPropertyValue7, 1f));
		}
		m_PropertyBlock.SetFloat("_NearDensity", numberPropertyValue);
		m_PropertyBlock.SetFloat("_NearRainSpeed", numberPropertyValue3);
		if (texturePropertyValue2 != null)
		{
			m_PropertyBlock.SetTexture("_FarTex", texturePropertyValue2);
			m_PropertyBlock.SetVector("_FarTex_ST", new Vector4(numberPropertyValue8, numberPropertyValue8, numberPropertyValue8, 1f));
		}
		m_PropertyBlock.SetFloat("_FarDensity", numberPropertyValue2);
		m_PropertyBlock.SetFloat("_FarRainSpeed", numberPropertyValue4);
		m_PropertyBlock.SetColor("_TintColor", colorPropertyValue);
		m_PropertyBlock.SetFloat("_Turbulence", numberPropertyValue5);
		m_PropertyBlock.SetFloat("_TurbulenceSpeed", numberPropertyValue6);
		rainMeshRenderer.SetPropertyBlock(m_PropertyBlock);
		if (skyProfile.IsFeatureEnabled("RainSoundFeature"))
		{
			m_RainAudioSource.enabled = true;
			m_RainAudioSource.volume = skyProfile.GetNumberPropertyValue("RainSoundVolume", timeOfDay);
		}
		else
		{
			m_RainAudioSource.enabled = false;
		}
	}
}
