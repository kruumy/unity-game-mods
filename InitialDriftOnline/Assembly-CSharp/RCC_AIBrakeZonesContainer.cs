using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/AI/RCC AI Brake Zones Container")]
public class RCC_AIBrakeZonesContainer : MonoBehaviour
{
	public List<Transform> brakeZones = new List<Transform>();

	private void OnDrawGizmos()
	{
		for (int i = 0; i < brakeZones.Count; i++)
		{
			Gizmos.matrix = brakeZones[i].transform.localToWorldMatrix;
			Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
			Vector3 size = brakeZones[i].GetComponent<BoxCollider>().size;
			Gizmos.DrawCube(Vector3.zero, size);
		}
	}
}
