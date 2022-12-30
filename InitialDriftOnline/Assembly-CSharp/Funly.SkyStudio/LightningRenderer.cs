using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio;

[RequireComponent(typeof(AudioSource))]
public class LightningRenderer : BaseSpriteInstancedRenderer
{
	private static List<LightningSpawnArea> m_SpawnAreas = new List<LightningSpawnArea>();

	private float m_LightningProbability;

	private float m_NextSpawnTime;

	private SkyProfile m_SkyProfile;

	private LightningArtItem m_Style;

	private float m_TimeOfDay;

	private AudioSource m_AudioSource;

	private float m_LightningIntensity;

	private float m_ThunderSoundDelay;

	private float m_SpawnCoolDown;

	private const float k_ProbabiltyCheckInterval = 0.5f;

	private void Start()
	{
		if (!SystemInfo.supportsInstancing)
		{
			Debug.LogError("Can't render lightning since GPU instancing is not supported on this platform.");
			base.enabled = false;
		}
		else
		{
			m_AudioSource = GetComponent<AudioSource>();
		}
	}

	protected override Bounds CalculateMeshBounds()
	{
		return new Bounds(Vector3.zero, new Vector3(500f, 500f, 500f));
	}

	protected override BaseSpriteItemData CreateSpriteItemData()
	{
		return new BaseSpriteItemData();
	}

	protected override bool IsRenderingEnabled()
	{
		if (m_SkyProfile == null || !m_SkyProfile.IsFeatureEnabled("LightningFeature"))
		{
			return false;
		}
		if (m_SpawnAreas.Count == 0)
		{
			return false;
		}
		return true;
	}

	protected override void CalculateSpriteTRS(BaseSpriteItemData data, out Vector3 spritePosition, out Quaternion spriteRotation, out Vector3 spriteScale)
	{
		LightningSpawnArea randomLightningSpawnArea = GetRandomLightningSpawnArea();
		float num = CalculateLightningBoltScaleForArea(randomLightningSpawnArea);
		spriteScale = new Vector3(num, num, num);
		spritePosition = GetRandomWorldPositionInsideSpawnArea(randomLightningSpawnArea);
		if (Camera.main == null)
		{
			Debug.LogError("Can't billboard lightning to viewer since there is no main camera tagged.");
			spriteRotation = randomLightningSpawnArea.transform.rotation;
		}
		else
		{
			spriteRotation = Quaternion.LookRotation(spritePosition - Camera.main.transform.position, Vector3.up);
		}
	}

	protected override void ConfigureSpriteItemData(BaseSpriteItemData data)
	{
		if (m_SkyProfile.IsFeatureEnabled("ThunderFeature"))
		{
			Invoke("PlayThunderBoltSound", m_ThunderSoundDelay);
		}
	}

	protected override void PrepareDataArraysForRendering(int instanceId, BaseSpriteItemData data)
	{
	}

	protected override void PopulatePropertyBlockForRendering(ref MaterialPropertyBlock propertyBlock)
	{
		propertyBlock.SetFloat("_Intensity", m_LightningIntensity);
	}

	protected override int GetNextSpawnCount()
	{
		if (m_NextSpawnTime > Time.time)
		{
			return 0;
		}
		m_NextSpawnTime = Time.time + 0.5f;
		if (Random.value < m_LightningProbability)
		{
			m_NextSpawnTime += m_SpawnCoolDown;
			return 1;
		}
		return 0;
	}

	public void UpdateForTimeOfDay(SkyProfile skyProfile, float timeOfDay, LightningArtItem artItem)
	{
		m_SkyProfile = skyProfile;
		m_TimeOfDay = timeOfDay;
		m_Style = artItem;
		if (m_SkyProfile == null)
		{
			Debug.LogError("Assigned null sky profile!");
		}
		else if (m_Style == null)
		{
			Debug.LogError("Can't render lightning without an art item");
		}
		else
		{
			SyncDataFromSkyProfile();
		}
	}

	private void SyncDataFromSkyProfile()
	{
		m_LightningProbability = m_SkyProfile.GetNumberPropertyValue("LightningProbabilityKey", m_TimeOfDay);
		m_LightningIntensity = m_SkyProfile.GetNumberPropertyValue("LightningIntensityKey", m_TimeOfDay);
		m_SpawnCoolDown = m_SkyProfile.GetNumberPropertyValue("LightningStrikeCoolDown", m_TimeOfDay);
		m_ThunderSoundDelay = m_SkyProfile.GetNumberPropertyValue("ThunderSoundDelayKey", m_TimeOfDay);
		m_LightningProbability *= m_Style.strikeProbability;
		m_LightningIntensity *= m_Style.intensity;
		m_SpriteSheetLayout.columns = m_Style.columns;
		m_SpriteSheetLayout.rows = m_Style.rows;
		m_SpriteSheetLayout.frameCount = m_Style.totalFrames;
		m_SpriteSheetLayout.frameRate = m_Style.animateSpeed;
		m_SpriteTexture = m_Style.spriteSheetTexture;
		m_TintColor = m_Style.tintColor * m_SkyProfile.GetColorPropertyValue("LightningTintColorKey", m_TimeOfDay);
		modelMesh = m_Style.mesh;
		if (renderMaterial == null)
		{
			renderMaterial = new Material(m_Style.material);
			renderMaterial.SetTexture("_SpriteSheetTex", m_Style.spriteSheetTexture);
		}
	}

	private LightningSpawnArea GetRandomLightningSpawnArea()
	{
		if (m_SpawnAreas.Count == 0)
		{
			return null;
		}
		int index = Mathf.RoundToInt(Random.Range(0, m_SpawnAreas.Count)) % m_SpawnAreas.Count;
		return m_SpawnAreas[index];
	}

	private void PlayThunderBoltSound()
	{
		if (m_Style.thunderSound != null)
		{
			m_AudioSource.volume = m_SkyProfile.GetNumberPropertyValue("ThunderSoundVolumeKey", m_TimeOfDay);
			m_AudioSource.PlayOneShot(m_Style.thunderSound);
		}
	}

	public static void AddSpawnArea(LightningSpawnArea area)
	{
		if (!m_SpawnAreas.Contains(area))
		{
			m_SpawnAreas.Add(area);
		}
	}

	public static void RemoveSpawnArea(LightningSpawnArea area)
	{
		if (m_SpawnAreas.Contains(area))
		{
			m_SpawnAreas.Remove(area);
		}
	}

	private Vector3 GetRandomWorldPositionInsideSpawnArea(LightningSpawnArea area)
	{
		float x = Random.Range(0f - area.lightningArea.x, area.lightningArea.x) / 2f;
		float z = Random.Range(0f - area.lightningArea.z, area.lightningArea.z) / 2f;
		float y = 0f;
		if (m_Style.alignment == LightningArtItem.Alignment.TopAlign)
		{
			y = area.lightningArea.y / 2f - m_Style.size / 2f;
		}
		return area.transform.TransformPoint(new Vector3(x, y, z));
	}

	private float CalculateLightningBoltScaleForArea(LightningSpawnArea area)
	{
		if (m_Style.alignment == LightningArtItem.Alignment.ScaleToFit)
		{
			return area.lightningArea.y / 2f;
		}
		return m_Style.size;
	}
}
