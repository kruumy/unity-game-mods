using System.Collections.Generic;
using UnityEngine;

namespace UniStorm.Utility;

public static class UniStormPool
{
	private class Pool
	{
		private int nextId = 1;

		private Stack<GameObject> inactive;

		private GameObject prefab;

		public Pool(GameObject prefab, int initialQty)
		{
			this.prefab = prefab;
			inactive = new Stack<GameObject>(initialQty);
		}

		public GameObject Spawn(Vector3 pos, Quaternion rot)
		{
			GameObject gameObject;
			if (inactive.Count == 0)
			{
				gameObject = Object.Instantiate(prefab, pos, rot);
				gameObject.name = prefab.name + " (" + nextId++ + ")";
				gameObject.AddComponent<PoolMember>().myPool = this;
			}
			else
			{
				gameObject = inactive.Pop();
				if (gameObject == null)
				{
					return Spawn(pos, rot);
				}
			}
			gameObject.transform.position = pos;
			gameObject.transform.rotation = rot;
			gameObject.SetActive(value: true);
			return gameObject;
		}

		public void Despawn(GameObject obj)
		{
			obj.SetActive(value: false);
			inactive.Push(obj);
		}
	}

	private class PoolMember : MonoBehaviour
	{
		public Pool myPool;
	}

	private const int DEFAULT_POOL_SIZE = 3;

	private static Dictionary<GameObject, Pool> pools;

	private static void Init(GameObject prefab = null, int qty = 3)
	{
		if (pools == null)
		{
			pools = new Dictionary<GameObject, Pool>();
		}
		if (prefab != null && !pools.ContainsKey(prefab))
		{
			pools[prefab] = new Pool(prefab, qty);
		}
	}

	public static void Preload(GameObject prefab, int qty = 1)
	{
		Init(prefab, qty);
		GameObject[] array = new GameObject[qty];
		for (int i = 0; i < qty; i++)
		{
			array[i] = Spawn(prefab, Vector3.zero, Quaternion.identity);
		}
		for (int j = 0; j < qty; j++)
		{
			Despawn(array[j]);
		}
	}

	public static GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
	{
		Init(prefab);
		return pools[prefab].Spawn(pos, rot);
	}

	public static void Despawn(GameObject obj)
	{
		PoolMember component = obj.GetComponent<PoolMember>();
		if (component == null)
		{
			Debug.Log("Object '" + obj.name + "' wasn't spawned from a pool. Destroying it instead.");
			Object.Destroy(obj);
		}
		else
		{
			component.myPool.Despawn(obj);
		}
	}
}
