using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/AI/RCC AI Car Controller")]
public class RCC_AICarController : MonoBehaviour
{
	public enum AIType
	{
		FollowWaypoints,
		ChaseTarget
	}

	public delegate void onRCCAISpawned(RCC_AICarController RCCAI);

	public delegate void onRCCAIDestroyed(RCC_AICarController RCCAI);

	internal RCC_CarControllerV3 carController;

	public RCC_AIWaypointsContainer waypointsContainer;

	public int currentWaypoint;

	public Transform targetChase;

	public string targetTag = "Player";

	public AIType _AIType;

	public int wideRayLength = 20;

	public int tightRayLength = 20;

	public int sideRayLength = 3;

	public LayerMask obstacleLayers = -1;

	private float rayInput;

	private bool raycasting;

	private float resetTime;

	private float steerInput;

	private float gasInput;

	private float brakeInput;

	public bool limitSpeed;

	public float maximumSpeed = 100f;

	public bool smoothedSteer = true;

	private float maximumSpeedInBrakeZone;

	private bool inBrakeZone;

	public int lap;

	public int totalWaypointPassed;

	public int nextWaypointPassRadius = 40;

	public bool ignoreWaypointNow;

	private NavMeshAgent navigator;

	private GameObject detector;

	public float detectorRadius = 100f;

	public List<RCC_CarControllerV3> targetsInZone = new List<RCC_CarControllerV3>();

	public static event onRCCAISpawned OnRCCAISpawned;

	public static event onRCCAIDestroyed OnRCCAIDestroyed;

	private void Start()
	{
		carController = GetComponent<RCC_CarControllerV3>();
		carController.externalController = true;
		if (!waypointsContainer)
		{
			waypointsContainer = Object.FindObjectOfType(typeof(RCC_AIWaypointsContainer)) as RCC_AIWaypointsContainer;
		}
		GameObject gameObject = new GameObject("Navigator");
		gameObject.transform.SetParent(base.transform, worldPositionStays: false);
		navigator = gameObject.AddComponent<NavMeshAgent>();
		navigator.radius = 1f;
		navigator.speed = 1f;
		navigator.angularSpeed = 100000f;
		navigator.acceleration = 100000f;
		navigator.height = 1f;
		navigator.avoidancePriority = 99;
		detector = new GameObject("Detector");
		detector.transform.SetParent(base.transform, worldPositionStays: false);
		detector.gameObject.AddComponent<SphereCollider>();
		detector.GetComponent<SphereCollider>().isTrigger = true;
		detector.GetComponent<SphereCollider>().radius = detectorRadius;
	}

	private void OnEnable()
	{
		if (RCC_AICarController.OnRCCAISpawned != null)
		{
			RCC_AICarController.OnRCCAISpawned(this);
		}
	}

	private void Update()
	{
		if (!carController.canControl)
		{
			return;
		}
		navigator.transform.localPosition = Vector3.zero;
		navigator.transform.localPosition += Vector3.forward * carController.FrontLeftWheelCollider.transform.localPosition.z;
		for (int i = 0; i < targetsInZone.Count; i++)
		{
			if (targetsInZone[i] == null)
			{
				targetsInZone.RemoveAt(i);
			}
			if (!targetsInZone[i].gameObject.activeInHierarchy)
			{
				targetsInZone.RemoveAt(i);
			}
			else if (Vector3.Distance(base.transform.position, targetsInZone[i].transform.position) > detectorRadius * 1.25f)
			{
				targetsInZone.RemoveAt(i);
			}
		}
		if (targetsInZone.Count > 0)
		{
			targetChase = GetClosestEnemy(targetsInZone.ToArray());
		}
		else
		{
			targetChase = null;
		}
	}

	private void FixedUpdate()
	{
		if (carController.canControl)
		{
			Navigation();
			FixedRaycasts();
			FeedRCC();
			Resetting();
		}
	}

	private void Navigation()
	{
		float num = Mathf.Clamp(base.transform.InverseTransformDirection(navigator.desiredVelocity).x * 1.5f, -1f, 1f);
		switch (_AIType)
		{
		case AIType.FollowWaypoints:
		{
			if (!waypointsContainer)
			{
				Debug.LogError("Waypoints Container Couldn't Found!");
				Stop();
				return;
			}
			if ((bool)waypointsContainer && waypointsContainer.waypoints.Count < 1)
			{
				Debug.LogError("Waypoints Container Doesn't Have Any Waypoints!");
				Stop();
				return;
			}
			Vector3 vector = base.transform.InverseTransformPoint(new Vector3(waypointsContainer.waypoints[currentWaypoint].position.x, base.transform.position.y, waypointsContainer.waypoints[currentWaypoint].position.z));
			if (navigator.isOnNavMesh)
			{
				navigator.SetDestination(waypointsContainer.waypoints[currentWaypoint].position);
			}
			if (vector.magnitude < (float)nextWaypointPassRadius)
			{
				currentWaypoint++;
				totalWaypointPassed++;
				if (currentWaypoint >= waypointsContainer.waypoints.Count)
				{
					currentWaypoint = 0;
					lap++;
				}
			}
			break;
		}
		case AIType.ChaseTarget:
			if (!targetChase)
			{
				Stop();
				return;
			}
			if (navigator.isOnNavMesh)
			{
				navigator.SetDestination(targetChase.position);
			}
			break;
		}
		if (!inBrakeZone)
		{
			if (carController.speed >= 10f)
			{
				if (!carController.changingGear)
				{
					gasInput = Mathf.Clamp(1f - (Mathf.Abs(num / 10f) - Mathf.Abs(rayInput / 10f)), 0.75f, 1f);
				}
				else
				{
					gasInput = 0f;
				}
			}
			else if (!carController.changingGear)
			{
				gasInput = 1f;
			}
			else
			{
				gasInput = 0f;
			}
		}
		else if (!carController.changingGear)
		{
			gasInput = Mathf.Lerp(1f, 0f, carController.speed / maximumSpeedInBrakeZone);
		}
		else
		{
			gasInput = 0f;
		}
		steerInput = Mathf.Clamp(ignoreWaypointNow ? rayInput : (num + rayInput), -1f, 1f) * (float)carController.direction;
		if (!inBrakeZone)
		{
			if (carController.speed >= 25f)
			{
				brakeInput = Mathf.Lerp(0f, 0.25f, Mathf.Abs(steerInput));
			}
			else
			{
				brakeInput = 0f;
			}
		}
		else
		{
			brakeInput = Mathf.Lerp(0f, 1f, (carController.speed - maximumSpeedInBrakeZone) / maximumSpeedInBrakeZone);
		}
	}

	private void Resetting()
	{
		if (carController.speed <= 5f && base.transform.InverseTransformDirection(carController.rigid.velocity).z < 1f)
		{
			resetTime += Time.deltaTime;
		}
		if (resetTime >= 2f)
		{
			carController.direction = -1;
		}
		if (resetTime >= 4f || carController.speed >= 25f)
		{
			carController.direction = 1;
			resetTime = 0f;
		}
	}

	private void FixedRaycasts()
	{
		Vector3 position = base.transform.position;
		position += base.transform.forward * carController.FrontLeftWheelCollider.transform.localPosition.z;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		Debug.DrawRay(position, Quaternion.AngleAxis(25f, base.transform.up) * base.transform.forward * wideRayLength, Color.white);
		Debug.DrawRay(position, Quaternion.AngleAxis(-25f, base.transform.up) * base.transform.forward * wideRayLength, Color.white);
		Debug.DrawRay(position, Quaternion.AngleAxis(7f, base.transform.up) * base.transform.forward * tightRayLength, Color.white);
		Debug.DrawRay(position, Quaternion.AngleAxis(-7f, base.transform.up) * base.transform.forward * tightRayLength, Color.white);
		Debug.DrawRay(position, Quaternion.AngleAxis(90f, base.transform.up) * base.transform.forward * sideRayLength, Color.white);
		Debug.DrawRay(position, Quaternion.AngleAxis(-90f, base.transform.up) * base.transform.forward * sideRayLength, Color.white);
		if (Physics.Raycast(position, Quaternion.AngleAxis(25f, base.transform.up) * base.transform.forward, out var hitInfo, wideRayLength, obstacleLayers) && !hitInfo.collider.isTrigger && hitInfo.transform.root != base.transform)
		{
			Debug.DrawRay(position, Quaternion.AngleAxis(25f, base.transform.up) * base.transform.forward * wideRayLength, Color.red);
			num = Mathf.Lerp(-0.5f, 0f, hitInfo.distance / (float)wideRayLength);
			flag2 = true;
		}
		else
		{
			num = 0f;
			flag2 = false;
		}
		if (Physics.Raycast(position, Quaternion.AngleAxis(-25f, base.transform.up) * base.transform.forward, out hitInfo, wideRayLength, obstacleLayers) && !hitInfo.collider.isTrigger && hitInfo.transform.root != base.transform)
		{
			Debug.DrawRay(position, Quaternion.AngleAxis(-25f, base.transform.up) * base.transform.forward * wideRayLength, Color.red);
			num4 = Mathf.Lerp(0.5f, 0f, hitInfo.distance / (float)wideRayLength);
			flag5 = true;
		}
		else
		{
			num4 = 0f;
			flag5 = false;
		}
		if (Physics.Raycast(position, Quaternion.AngleAxis(7f, base.transform.up) * base.transform.forward, out hitInfo, tightRayLength, obstacleLayers) && !hitInfo.collider.isTrigger && hitInfo.transform.root != base.transform)
		{
			Debug.DrawRay(position, Quaternion.AngleAxis(7f, base.transform.up) * base.transform.forward * tightRayLength, Color.red);
			num3 = Mathf.Lerp(-1f, 0f, hitInfo.distance / (float)tightRayLength);
			flag = true;
		}
		else
		{
			num3 = 0f;
			flag = false;
		}
		if (Physics.Raycast(position, Quaternion.AngleAxis(-7f, base.transform.up) * base.transform.forward, out hitInfo, tightRayLength, obstacleLayers) && !hitInfo.collider.isTrigger && hitInfo.transform.root != base.transform)
		{
			Debug.DrawRay(position, Quaternion.AngleAxis(-7f, base.transform.up) * base.transform.forward * tightRayLength, Color.red);
			num2 = Mathf.Lerp(1f, 0f, hitInfo.distance / (float)tightRayLength);
			flag4 = true;
		}
		else
		{
			num2 = 0f;
			flag4 = false;
		}
		if (Physics.Raycast(position, Quaternion.AngleAxis(90f, base.transform.up) * base.transform.forward, out hitInfo, sideRayLength, obstacleLayers) && !hitInfo.collider.isTrigger && hitInfo.transform.root != base.transform)
		{
			Debug.DrawRay(position, Quaternion.AngleAxis(90f, base.transform.up) * base.transform.forward * sideRayLength, Color.red);
			num5 = Mathf.Lerp(-1f, 0f, hitInfo.distance / (float)sideRayLength);
			flag3 = true;
		}
		else
		{
			num5 = 0f;
			flag3 = false;
		}
		if (Physics.Raycast(position, Quaternion.AngleAxis(-90f, base.transform.up) * base.transform.forward, out hitInfo, sideRayLength, obstacleLayers) && !hitInfo.collider.isTrigger && hitInfo.transform.root != base.transform)
		{
			Debug.DrawRay(position, Quaternion.AngleAxis(-90f, base.transform.up) * base.transform.forward * sideRayLength, Color.red);
			num6 = Mathf.Lerp(1f, 0f, hitInfo.distance / (float)sideRayLength);
			flag6 = true;
		}
		else
		{
			num6 = 0f;
			flag6 = false;
		}
		if (flag2 || flag5 || flag || flag4 || flag3 || flag6)
		{
			raycasting = true;
		}
		else
		{
			raycasting = false;
		}
		if (raycasting)
		{
			rayInput = num + num2 + num3 + num4 + num5 + num6;
		}
		else
		{
			rayInput = 0f;
		}
		if (raycasting && Mathf.Abs(rayInput) > 0.5f)
		{
			ignoreWaypointNow = true;
		}
		else
		{
			ignoreWaypointNow = false;
		}
	}

	private void FeedRCC()
	{
		if (carController.direction == 1)
		{
			if (!limitSpeed)
			{
				carController.gasInput = gasInput;
			}
			else
			{
				carController.gasInput = gasInput * Mathf.Clamp01(Mathf.Lerp(10f, 0f, carController.speed / maximumSpeed));
			}
		}
		else
		{
			carController.gasInput = 0f;
		}
		if (smoothedSteer)
		{
			carController.steerInput = Mathf.Lerp(carController.steerInput, steerInput, Time.deltaTime * 20f);
		}
		else
		{
			carController.steerInput = steerInput;
		}
		if (carController.direction == 1)
		{
			carController.brakeInput = brakeInput;
		}
		else
		{
			carController.brakeInput = gasInput;
		}
	}

	private void Stop()
	{
		gasInput = 0f;
		steerInput = 0f;
		brakeInput = 1f;
	}

	private void OnTriggerEnter(Collider col)
	{
		if ((bool)col.gameObject.GetComponent<RCC_AIBrakeZone>())
		{
			inBrakeZone = true;
			maximumSpeedInBrakeZone = col.gameObject.GetComponent<RCC_AIBrakeZone>().targetSpeed;
		}
		if (col.attachedRigidbody != null && (bool)col.gameObject.GetComponentInParent<RCC_CarControllerV3>() && col.gameObject.GetComponentInParent<RCC_CarControllerV3>().transform.CompareTag(targetTag) && !targetsInZone.Contains(col.gameObject.GetComponentInParent<RCC_CarControllerV3>()))
		{
			targetsInZone.Add(col.gameObject.GetComponentInParent<RCC_CarControllerV3>());
		}
	}

	private void OnTriggerExit(Collider col)
	{
		if ((bool)col.gameObject.GetComponent<RCC_AIBrakeZone>())
		{
			inBrakeZone = false;
			maximumSpeedInBrakeZone = 0f;
		}
	}

	private Transform GetClosestEnemy(RCC_CarControllerV3[] enemies)
	{
		Transform result = null;
		float num = float.PositiveInfinity;
		Vector3 position = base.transform.position;
		foreach (RCC_CarControllerV3 rCC_CarControllerV in enemies)
		{
			float sqrMagnitude = (rCC_CarControllerV.transform.position - position).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				num = sqrMagnitude;
				result = rCC_CarControllerV.transform;
			}
		}
		return result;
	}

	private void OnDestroy()
	{
		if (RCC_AICarController.OnRCCAIDestroyed != null)
		{
			RCC_AICarController.OnRCCAIDestroyed(this);
		}
	}
}
