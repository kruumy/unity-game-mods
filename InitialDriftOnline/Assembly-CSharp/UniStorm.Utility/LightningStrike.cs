using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniStorm.Utility;

public class LightningStrike : MonoBehaviour
{
	[HideInInspector]
	public GameObject LightningStrikeFire;

	[HideInInspector]
	public GameObject LightningStrikeEffect;

	[HideInInspector]
	public Vector3 HitPosition;

	[HideInInspector]
	public bool PlayerDetected;

	[HideInInspector]
	public int GroundStrikeOdds = 50;

	private int RaycastDistance = 75;

	[HideInInspector]
	public bool LightningGenerated;

	[HideInInspector]
	public LayerMask DetectionLayerMask;

	[HideInInspector]
	public bool ObjectDetected;

	[HideInInspector]
	public string FireTag = "Finish";

	[HideInInspector]
	public List<string> LightningFireTags = new List<string>();

	public GameObject HitObject;

	[HideInInspector]
	public string PlayerTag = "Player";

	[HideInInspector]
	public int EmeraldAIRagdollForce = 500;

	[HideInInspector]
	public int EmeraldAILightningDamage = 500;

	public bool EmeraldAIAgentDetected;

	public GameObject HitAgent;

	[HideInInspector]
	public string EmeraldAITag = "Respawn";

	private void Start()
	{
		UniStormSystem uniStormSystem = Object.FindObjectOfType<UniStormSystem>();
		uniStormSystem.m_LightningStrikeSystem = GetComponent<LightningStrike>();
		GroundStrikeOdds = uniStormSystem.LightningGroundStrikeOdds;
		LightningStrikeEffect = uniStormSystem.LightningStrikeEffect;
		LightningStrikeFire = uniStormSystem.LightningStrikeFire;
		DetectionLayerMask = uniStormSystem.DetectionLayerMask;
		LightningFireTags = uniStormSystem.LightningFireTags;
		GetComponent<SphereCollider>().radius = uniStormSystem.LightningDetectionDistance;
		PlayerTag = UniStormSystem.Instance.PlayerTag;
		EmeraldAITag = UniStormSystem.Instance.EmeraldAITag;
		EmeraldAIRagdollForce = UniStormSystem.Instance.EmeraldAIRagdollForce;
		EmeraldAILightningDamage = UniStormSystem.Instance.EmeraldAILightningDamage;
		HitPosition = Vector3.zero + new Vector3(0f, 1000f, 0f);
	}

	private void OnTriggerEnter(Collider C)
	{
		if (C.gameObject.layer != 2 && C.GetComponent<Terrain>() == null && (DetectionLayerMask.value & (1 << C.gameObject.layer)) != 0)
		{
			if (C.tag == PlayerTag)
			{
				HitPosition = C.transform.position;
				HitObject = C.gameObject;
				PlayerDetected = true;
			}
			else if ((C.tag != PlayerTag && C.tag != EmeraldAITag && UniStormSystem.Instance.LightningStrikesEmeraldAI == UniStormSystem.EnableFeature.Enabled) || (UniStormSystem.Instance.LightningStrikesEmeraldAI == UniStormSystem.EnableFeature.Disabled && C.tag != PlayerTag))
			{
				ObjectDetected = true;
				HitPosition = C.transform.position;
				HitObject = C.gameObject;
			}
			else if (C.tag == EmeraldAITag)
			{
				_ = UniStormSystem.Instance.LightningStrikesEmeraldAI;
			}
		}
	}

	public void CreateLightningStrike()
	{
		if (Random.Range(1, 101) <= GroundStrikeOdds)
		{
			RaycastDistance = 250;
		}
		else
		{
			RaycastDistance = 0;
		}
		if (!ObjectDetected)
		{
			HitPosition = base.transform.position;
		}
		if (!Physics.Raycast(new Vector3(HitPosition.x, HitPosition.y + 40f, HitPosition.z), -base.transform.up, out var hitInfo, RaycastDistance, DetectionLayerMask))
		{
			return;
		}
		Vector3 point = hitInfo.point;
		LightningGenerated = true;
		UniStormSystem.Instance.LightningStruckObject = HitObject;
		if (hitInfo.collider.GetComponent<Terrain>() != null && !ObjectDetected)
		{
			HitPosition = new Vector3(point.x, hitInfo.collider.GetComponent<Terrain>().SampleHeight(hitInfo.point) + 0.5f, point.z);
		}
		else
		{
			HitPosition = new Vector3(HitPosition.x, point.y + 0.5f, HitPosition.z);
		}
		if (!PlayerDetected && !EmeraldAIAgentDetected)
		{
			if (LightningFireTags.Contains(hitInfo.collider.tag))
			{
				UniStormPool.Spawn(LightningStrikeFire, HitPosition, Quaternion.identity).transform.SetParent(hitInfo.collider.transform);
			}
			UniStormPool.Spawn(LightningStrikeEffect, HitPosition, Quaternion.identity);
			UniStormSystem.Instance.OnLightningStrikeObjectEvent.Invoke();
		}
		else if (PlayerDetected)
		{
			if (LightningFireTags.Contains(hitInfo.collider.tag))
			{
				UniStormPool.Spawn(LightningStrikeFire, HitPosition, Quaternion.identity);
			}
			UniStormPool.Spawn(LightningStrikeEffect, HitPosition, Quaternion.identity);
			UniStormSystem.Instance.OnLightningStrikePlayerEvent.Invoke();
		}
		else
		{
			_ = EmeraldAIAgentDetected;
		}
		LightningGenerated = false;
		ObjectDetected = false;
		PlayerDetected = false;
		EmeraldAIAgentDetected = false;
		HitObject = null;
		HitAgent = null;
	}

	private IEnumerator ResetDelay()
	{
		yield return new WaitForSeconds(0.1f);
		EmeraldAIAgentDetected = false;
		HitObject = null;
		HitAgent = null;
	}
}
