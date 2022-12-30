using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio;

public class RainSplashController : MonoBehaviour, ISkyModule
{
	private SkyProfile m_SkyProfile;

	private float m_TimeOfDay;

	private List<RainSplashRenderer> m_SplashRenderers = new List<RainSplashRenderer>();

	private void Start()
	{
		if (!SystemInfo.supportsInstancing)
		{
			Debug.LogWarning("Can't render rain splashes since GPU instancing is not supported on this platform.");
			base.enabled = false;
		}
		else
		{
			ClearSplashRenderers();
		}
	}

	public void UpdateForTimeOfDay(SkyProfile skyProfile, float timeOfDay)
	{
		m_SkyProfile = skyProfile;
		m_TimeOfDay = timeOfDay;
	}

	private void Update()
	{
		if (m_SkyProfile == null || !m_SkyProfile.IsFeatureEnabled("RainSplashFeature"))
		{
			ClearSplashRenderers();
			return;
		}
		if (m_SkyProfile.rainSplashArtSet == null || m_SkyProfile.rainSplashArtSet.rainSplashArtItems == null || m_SkyProfile.rainSplashArtSet.rainSplashArtItems.Count == 0)
		{
			ClearSplashRenderers();
			return;
		}
		if (m_SkyProfile.rainSplashArtSet.rainSplashArtItems.Count != m_SplashRenderers.Count)
		{
			ClearSplashRenderers();
			CreateSplashRenderers();
		}
		for (int i = 0; i < m_SkyProfile.rainSplashArtSet.rainSplashArtItems.Count; i++)
		{
			RainSplashArtItem style = m_SkyProfile.rainSplashArtSet.rainSplashArtItems[i];
			m_SplashRenderers[i].UpdateForTimeOfDay(m_SkyProfile, m_TimeOfDay, style);
		}
	}

	public void ClearSplashRenderers()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Object.Destroy(base.transform.GetChild(i).gameObject);
		}
		m_SplashRenderers.Clear();
	}

	public void CreateSplashRenderers()
	{
		for (int i = 0; i < m_SkyProfile.rainSplashArtSet.rainSplashArtItems.Count; i++)
		{
			RainSplashRenderer rainSplashRenderer = new GameObject("Rain Splash Renderer").AddComponent<RainSplashRenderer>();
			rainSplashRenderer.transform.parent = base.transform;
			m_SplashRenderers.Add(rainSplashRenderer);
		}
	}
}
