using System.Collections;
using UnityEngine;

namespace UniStorm.Utility;

public class DespawnObjectUniStorm : MonoBehaviour
{
	public int Seconds = 3;

	private void OnEnable()
	{
		StartCoroutine(Despawn());
	}

	private IEnumerator Despawn()
	{
		yield return new WaitForSeconds(Seconds);
		UniStormPool.Despawn(base.gameObject);
	}
}
