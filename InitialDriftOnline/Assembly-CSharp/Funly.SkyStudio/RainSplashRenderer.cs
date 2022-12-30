using UnityEngine;

namespace Funly.SkyStudio;

public class RainSplashRenderer : BaseSpriteInstancedRenderer
{
	private Camera m_DepthCamera;

	private float[] m_StartSplashYPositions = new float[1000];

	private float[] m_DepthUs = new float[1000];

	private float[] m_DepthVs = new float[1000];

	private float m_SplashAreaStart;

	private float m_SplashAreaLength;

	private float m_SplashScale;

	private float m_SplashScaleVarience;

	private float m_SplashItensity;

	private float m_SplashSurfaceOffset;

	private SkyProfile m_SkyProfile;

	private float m_TimeOfDay;

	private RainSplashArtItem m_Style;

	private Bounds m_Bounds = new Bounds(Vector3.zero, new Vector3(100f, 100f, 100f));

	private void Start()
	{
		if (!SystemInfo.supportsInstancing)
		{
			Debug.LogError("Can't render rain splashes since GPU instancing is not supported on this platform.");
			base.enabled = false;
			return;
		}
		WeatherDepthCamera weatherDepthCamera = Object.FindObjectOfType<WeatherDepthCamera>();
		if (weatherDepthCamera == null)
		{
			Debug.LogError("Can't generate splashes without a RainDepthCamera in the scene");
			base.enabled = false;
		}
		else
		{
			m_DepthCamera = weatherDepthCamera.GetComponent<Camera>();
		}
	}

	protected override Bounds CalculateMeshBounds()
	{
		return m_Bounds;
	}

	protected override BaseSpriteItemData CreateSpriteItemData()
	{
		return new RainSplashData();
	}

	protected override bool IsRenderingEnabled()
	{
		if (m_SkyProfile == null)
		{
			return false;
		}
		if (!m_SkyProfile.IsFeatureEnabled("RainSplashFeature"))
		{
			return false;
		}
		if (base.m_ViewerCamera == null)
		{
			Debug.LogError("Can't render ground raindrops since no active camera has the MainCamera tag applied.");
			return false;
		}
		return true;
	}

	protected override int GetNextSpawnCount()
	{
		int num = base.maxSprites - m_Active.Count;
		if (num <= 0)
		{
			return 0;
		}
		return num;
	}

	protected override void CalculateSpriteTRS(BaseSpriteItemData data, out Vector3 spritePosition, out Quaternion spriteRotation, out Vector3 spriteScale)
	{
		float num = Random.Range(m_SplashScale * (1f - m_SplashScaleVarience), m_SplashScale);
		spritePosition = data.spritePosition;
		spriteRotation = Quaternion.identity;
		spriteScale = new Vector3(num, num, num);
	}

	protected override void ConfigureSpriteItemData(BaseSpriteItemData data)
	{
		data.spritePosition = CreateWorldSplashPoint();
		data.delay = Random.Range(0f, 0.5f);
	}

	protected override void PrepareDataArraysForRendering(int instanceId, BaseSpriteItemData data)
	{
		RainSplashData rainSplashData = data as RainSplashData;
		Vector3 vector = m_DepthCamera.WorldToScreenPoint(rainSplashData.spritePosition);
		Vector2 vector2 = (rainSplashData.depthTextureUV = new Vector2(vector.x / (float)m_DepthCamera.pixelWidth, vector.y / (float)m_DepthCamera.pixelHeight));
		m_StartSplashYPositions[instanceId] = rainSplashData.spritePosition.y;
		m_DepthUs[instanceId] = rainSplashData.depthTextureUV.x;
		m_DepthVs[instanceId] = rainSplashData.depthTextureUV.y;
	}

	protected override void PopulatePropertyBlockForRendering(ref MaterialPropertyBlock propertyBlock)
	{
		propertyBlock.SetFloat("_Intensity", m_SplashItensity);
		propertyBlock.SetFloatArray("_OverheadDepthU", m_DepthUs);
		propertyBlock.SetFloatArray("_OverheadDepthV", m_DepthVs);
		propertyBlock.SetFloatArray("_SplashStartYPosition", m_StartSplashYPositions);
		propertyBlock.SetFloat("_SplashGroundOffset", m_SplashSurfaceOffset);
	}

	public void UpdateForTimeOfDay(SkyProfile skyProfile, float timeOfDay, RainSplashArtItem style)
	{
		m_SkyProfile = skyProfile;
		m_TimeOfDay = timeOfDay;
		m_Style = style;
		if (!(m_SkyProfile == null))
		{
			SyncDataFromSkyProfile();
		}
	}

	private void SyncDataFromSkyProfile()
	{
		base.maxSprites = (int)m_SkyProfile.GetNumberPropertyValue("RainSplashMaxConcurrentKey", m_TimeOfDay);
		m_SplashAreaStart = m_SkyProfile.GetNumberPropertyValue("RainSplashAreaStartKey", m_TimeOfDay);
		m_SplashAreaLength = m_SkyProfile.GetNumberPropertyValue("RainSplashAreaLengthKey", m_TimeOfDay);
		m_SplashScale = m_SkyProfile.GetNumberPropertyValue("RainSplashScaleKey", m_TimeOfDay);
		m_SplashScaleVarience = m_SkyProfile.GetNumberPropertyValue("RainSplashScaleVarienceKey", m_TimeOfDay);
		m_SplashItensity = m_SkyProfile.GetNumberPropertyValue("RainSplashIntensityKey", m_TimeOfDay);
		m_SplashSurfaceOffset = m_SkyProfile.GetNumberPropertyValue("RainSplashSurfaceOffsetKey", m_TimeOfDay);
		m_SplashScale *= m_Style.scaleMultiplier;
		m_SplashItensity *= m_Style.intensityMultiplier;
		m_SpriteSheetLayout.columns = m_Style.columns;
		m_SpriteSheetLayout.rows = m_Style.rows;
		m_SpriteSheetLayout.frameCount = m_Style.totalFrames;
		m_SpriteSheetLayout.frameRate = m_Style.animateSpeed;
		m_SpriteTexture = m_Style.spriteSheetTexture;
		m_TintColor = m_Style.tintColor * m_SkyProfile.GetColorPropertyValue("RainSplashTintColorKey", m_TimeOfDay);
		modelMesh = m_Style.mesh;
		if (renderMaterial == null)
		{
			renderMaterial = new Material(m_Style.material);
			renderMaterial.SetTexture("_SpriteSheetTex", m_Style.spriteSheetTexture);
		}
	}

	private Vector3 CreateWorldSplashPoint()
	{
		float y = Random.Range(0f, -170f);
		Vector3 vector = Quaternion.Euler(new Vector3(0f, y, 0f)) * Vector3.right;
		float num = Random.Range(m_SplashAreaStart, m_SplashAreaStart + m_SplashAreaLength);
		Vector3 position = vector.normalized * num;
		return base.m_ViewerCamera.transform.TransformPoint(position);
	}
}
