using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio;

public class LightningController : MonoBehaviour, ISkyModule
{
	private SkyProfile m_SkyProfile;

	private float m_TimeOfDay;

	private List<LightningRenderer> m_LightningRenderers = new List<LightningRenderer>();

	private void Start()
	{
		if (!SystemInfo.supportsInstancing)
		{
			Debug.LogWarning("Can't render lightning since GPU instancing is not supported on this platform.");
			base.enabled = false;
		}
		else
		{
			ClearLightningRenderers();
		}
	}

	public void UpdateForTimeOfDay(SkyProfile skyProfile, float timeOfDay)
	{
		m_SkyProfile = skyProfile;
		m_TimeOfDay = timeOfDay;
	}

	public void Update()
	{
		if (m_SkyProfile == null || !m_SkyProfile.IsFeatureEnabled("LightningFeature"))
		{
			ClearLightningRenderers();
		}
		else if (!(m_SkyProfile.lightningArtSet == null) && m_SkyProfile.lightningArtSet.lightingStyleItems != null && m_SkyProfile.lightningArtSet.lightingStyleItems.Count != 0)
		{
			if (m_SkyProfile.lightningArtSet.lightingStyleItems.Count != m_LightningRenderers.Count)
			{
				ClearLightningRenderers();
				CreateLightningRenderers();
			}
			for (int i = 0; i < m_SkyProfile.lightningArtSet.lightingStyleItems.Count; i++)
			{
				LightningArtItem artItem = m_SkyProfile.lightningArtSet.lightingStyleItems[i];
				m_LightningRenderers[i].UpdateForTimeOfDay(m_SkyProfile, m_TimeOfDay, artItem);
			}
		}
	}

	public void ClearLightningRenderers()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Object.Destroy(base.transform.GetChild(i).gameObject);
		}
		m_LightningRenderers.Clear();
	}

	public void CreateLightningRenderers()
	{
		for (int i = 0; i < m_SkyProfile.lightningArtSet.lightingStyleItems.Count; i++)
		{
			LightningRenderer lightningRenderer = new GameObject("Lightning Renderer").AddComponent<LightningRenderer>();
			lightningRenderer.transform.parent = base.transform;
			m_LightningRenderers.Add(lightningRenderer);
		}
	}
}
