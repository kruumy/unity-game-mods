using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniStorm.Utility;

public class LightningSystem : MonoBehaviour
{
	[HideInInspector]
	public int LightningGenerationDistance = 100;

	[HideInInspector]
	public LineRenderer LightningBolt;

	[HideInInspector]
	private List<Vector3> LightningPoints = new List<Vector3>();

	[HideInInspector]
	public bool AnimateLight;

	[HideInInspector]
	public float LightningLightIntensityMin = 1f;

	[HideInInspector]
	public float LightningLightIntensityMax = 1f;

	[HideInInspector]
	public float LightningLightIntensity;

	[HideInInspector]
	public Light LightningLightSource;

	[HideInInspector]
	public float LightningCurveMultipler = 1.45f;

	[HideInInspector]
	public AnimationCurve LightningCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	[HideInInspector]
	public float m_FlashSeconds = 0.5f;

	[HideInInspector]
	public Transform StartingPoint;

	[HideInInspector]
	public Transform EndingPoint;

	[HideInInspector]
	public int m_Segments = 45;

	[HideInInspector]
	public float Speed = 0.032f;

	[HideInInspector]
	public float Scale = 2f;

	[HideInInspector]
	public Transform PlayerTransform;

	[HideInInspector]
	public List<AudioClip> ThunderSounds = new List<AudioClip>();

	[HideInInspector]
	public int BoltIntensity = 10;

	[HideInInspector]
	public float LightningSpeed = 0.1f;

	[HideInInspector]
	public float StaticIntensity = 0.05f;

	private AudioSource AS;

	private Coroutine LightningCoroutine;

	private float m_FlashTimer;

	private float m_GenerateTimer;

	private float m_WidthTimer;

	public Material m_LightningMaterial;

	private Color m_LightningColor;

	private float PointIndex;

	private Vector3 m_LightningCurve;

	private bool Generated;

	private float LightningTime;

	private Perlin noise;

	private float CurrentIndex;

	private Vector3 Final;

	private void Start()
	{
		GameObject original = Resources.Load("Lightning Renderer") as GameObject;
		LightningBolt = Object.Instantiate(original, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
		LightningBolt.transform.SetParent(Object.FindObjectOfType<UniStormSystem>().transform);
		LightningBolt.name = "Lightning Renderer";
		GameObject original2 = Resources.Load("Lightning End Point") as GameObject;
		EndingPoint = Object.Instantiate(original2, Vector3.zero, Quaternion.identity).transform;
		EndingPoint.transform.SetParent(Object.FindObjectOfType<UniStormSystem>().transform);
		EndingPoint.name = "Lightning End Point";
		StartingPoint = new GameObject().transform;
		StartingPoint.transform.position = Vector3.zero;
		StartingPoint.SetParent(Object.FindObjectOfType<UniStormSystem>().transform);
		StartingPoint.name = "Lightning Start Point";
		m_Segments = 20;
		LightningBolt.positionCount = m_Segments;
		PointIndex = 1f / (float)m_Segments;
		for (int i = 0; i < LightningBolt.positionCount; i++)
		{
			LightningPoints.Add(base.transform.position);
		}
		base.gameObject.AddComponent<AudioSource>();
		AS = GetComponent<AudioSource>();
		AS.outputAudioMixerGroup = Object.FindObjectOfType<UniStormSystem>().UniStormAudioMixer.FindMatchingGroups("Master/Weather")[0];
		LightningBolt.enabled = false;
		m_LightningMaterial = LightningBolt.material;
		m_LightningMaterial.SetColor("_TintColor", UniStormSystem.Instance.LightningColor);
		m_LightningColor = m_LightningMaterial.GetColor("_TintColor");
		Vector3 vector = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y, PlayerTransform.position.z) + new Vector3(Random.insideUnitSphere.x, 0f, Random.insideUnitSphere.z) * LightningGenerationDistance;
		StartingPoint.position = vector + new Vector3(0f, 80f, 0f);
		EndingPoint.position = vector;
		LightningLightSource.transform.rotation = Quaternion.Euler(Random.Range(35, 85), Random.Range(0, 360), 0f);
		LightningLightIntensity = Random.Range(LightningLightIntensityMin, LightningLightIntensityMax);
		if (UniStormSystem.Instance.LightningStrikes == UniStormSystem.EnableFeature.Disabled)
		{
			EndingPoint.gameObject.SetActive(value: false);
		}
	}

	private void SetupLightningLight()
	{
		LightningLightSource.transform.rotation = Quaternion.Euler(Random.Range(10, 40), Random.Range(0, 360), 0f);
	}

	private void GeneratePoints()
	{
		m_GenerateTimer += Time.deltaTime;
		if (noise == null)
		{
			noise = new Perlin();
		}
		float x = Time.time * -0.01f;
		m_LightningMaterial.SetTextureOffset("_MainTex", new Vector2(x, 0f));
		float num = Time.time * 1f;
		float num2 = Time.time * 1f;
		float num3 = Time.time * 1f;
		if (!(m_GenerateTimer <= 1f))
		{
			return;
		}
		for (int i = 0; i < LightningBolt.positionCount; i++)
		{
			Vector3 vector = Vector3.Lerp(StartingPoint.position, EndingPoint.position, (float)i * PointIndex);
			Vector3 b = Vector3.Lerp(StartingPoint.position, EndingPoint.position, (float)i * PointIndex);
			Vector3 vector2 = new Vector3(noise.Noise(num + vector.x, num + vector.y, num + vector.z), noise.Noise(num2 + vector.x, num2 + vector.y, num2 + vector.z), noise.Noise(num3 + vector.x, num3 + vector.y, num3 + vector.z));
			int num4 = 8;
			vector += vector2 * num4;
			if (CurrentIndex % 5f == 0f)
			{
				m_LightningCurve = new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f)) + m_LightningCurve;
			}
			if (i <= 1)
			{
				Final = Vector3.Lerp(vector + m_LightningCurve * i, b, (float)i * PointIndex);
			}
			else
			{
				Final = Vector3.Lerp(vector + m_LightningCurve * i, b, (float)i * PointIndex);
			}
			LightningPoints[i] = Vector3.Lerp(Final, EndingPoint.position, (float)i * PointIndex);
			if (i == LightningBolt.positionCount - 1)
			{
				m_GenerateTimer = 2f;
			}
			LightningBolt.SetPosition(i, LightningPoints[i]);
			CurrentIndex += 1f;
		}
	}

	private void Update()
	{
		if (AnimateLight)
		{
			LightningTime += Time.deltaTime * LightningCurveMultipler;
			float num = LightningCurve.Evaluate(LightningTime);
			LightningLightSource.intensity = num * LightningLightIntensity;
			Shader.SetGlobalFloat("_uLightning", LightningLightSource.intensity * 0.3f);
			if (LightningTime >= 1f)
			{
				LightningTime = 0f;
				AnimateLight = false;
				LightningLightSource.transform.rotation = Quaternion.Euler(Random.Range(35, 85), Random.Range(0, 360), 0f);
			}
		}
	}

	public void GenerateLightning()
	{
		Generated = true;
		Speed = LightningSpeed;
		Scale = BoltIntensity;
		UniStormSystem.Instance.LightningStruckObject = null;
		LightningLightIntensity = Random.Range(LightningLightIntensityMin, LightningLightIntensityMax);
		if (LightningCoroutine != null)
		{
			StopCoroutine(LightningCoroutine);
		}
		LightningCoroutine = StartCoroutine(DrawLightning());
	}

	private IEnumerator DrawLightning()
	{
		AnimateLight = true;
		LightningBolt.enabled = true;
		StartCoroutine(ThunderSoundDelay());
		LightningBolt.widthMultiplier = 0f;
		if (UniStormSystem.Instance.LightningStrikes == UniStormSystem.EnableFeature.Enabled)
		{
			EndingPoint.GetComponent<LightningStrike>().CreateLightningStrike();
			if (EndingPoint.GetComponent<LightningStrike>().HitPosition != Vector3.zero)
			{
				Vector3 vector = new Vector3(Random.Range(-70, 70), 100f, Random.Range(-70, 70));
				EndingPoint.position = EndingPoint.GetComponent<LightningStrike>().HitPosition;
				StartingPoint.position = new Vector3(EndingPoint.position.x, StartingPoint.position.y, EndingPoint.position.z) + vector;
			}
			if (EndingPoint.GetComponent<LightningStrike>().EmeraldAIAgentDetected && EndingPoint.GetComponent<LightningStrike>().HitAgent != null)
			{
				EndingPoint.position = EndingPoint.GetComponent<LightningStrike>().HitAgent.transform.position;
			}
			CurrentIndex = 0f;
			m_GenerateTimer = 0.2f;
			m_LightningCurve = new Vector3(Random.Range(-10, 10), 0f, Random.Range(-10, 10));
			while (Generated)
			{
				m_FlashTimer += Time.deltaTime;
				GeneratePoints();
				LightningBolt.widthMultiplier = 7f;
				Color lightningColor = m_LightningColor;
				lightningColor.a = Mathf.Lerp(1f, UniStormSystem.Instance.m_LightningLight.intensity / 1.75f, m_FlashTimer);
				m_LightningMaterial.SetColor("_TintColor", lightningColor);
				if (m_FlashTimer >= m_FlashSeconds)
				{
					m_WidthTimer += Time.deltaTime * 2f;
					Color lightningColor2 = m_LightningColor;
					lightningColor2.a = Mathf.Lerp(1f, 0f, m_WidthTimer);
					m_LightningMaterial.SetColor("_TintColor", lightningColor2);
					if (m_WidthTimer > 1f)
					{
						LightningBolt.widthMultiplier = 0f;
						Generated = false;
						m_WidthTimer = 0f;
					}
				}
				yield return null;
			}
		}
		m_FlashTimer = 0f;
		LightningBolt.widthMultiplier = 0f;
		LightningBolt.enabled = false;
		EndingPoint.GetComponent<LightningStrike>().HitPosition = Vector3.zero;
		Vector3 vector2 = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y, PlayerTransform.position.z) + new Vector3(Random.insideUnitSphere.x, 0f, Random.insideUnitSphere.z) * LightningGenerationDistance;
		Vector3 vector3 = new Vector3(0f, Random.Range(0, 40), 0f);
		Vector3 vector4 = new Vector3(Random.Range(-200, 200), 0f, Random.Range(-200, 200));
		StartingPoint.position = vector2 + new Vector3(0f, 200f, 0f) + vector4;
		EndingPoint.position = vector2 + vector3;
		Color lightningColor3 = m_LightningColor;
		lightningColor3.a = Mathf.Lerp(1f, 0f, m_WidthTimer);
		m_LightningMaterial.SetColor("_TintColor", lightningColor3);
	}

	private IEnumerator ThunderSoundDelay()
	{
		float seconds = Vector3.Distance(EndingPoint.position, PlayerTransform.position) / 50f;
		yield return new WaitForSeconds(seconds);
		AS.pitch = Random.Range(0.7f, 1.3f);
		if (ThunderSounds.Count > 0)
		{
			AudioClip audioClip = ThunderSounds[Random.Range(0, ThunderSounds.Count)];
			if (audioClip != null)
			{
				AS.PlayOneShot(audioClip);
			}
		}
	}
}
