using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio;

[Serializable]
public class SerializableDictionary<K, V> : ISerializationCallbackReceiver
{
	[NonSerialized]
	public Dictionary<K, V> dict = new Dictionary<K, V>();

	[SerializeField]
	public List<K> m_Keys = new List<K>();

	[SerializeField]
	public List<V> m_Values = new List<V>();

	public V this[K aKey]
	{
		get
		{
			return dict[aKey];
		}
		set
		{
			dict[aKey] = value;
		}
	}

	public void Clear()
	{
		dict.Clear();
	}

	public void OnBeforeSerialize()
	{
		m_Keys.Clear();
		m_Values.Clear();
		foreach (K key in dict.Keys)
		{
			m_Keys.Add(key);
			m_Values.Add(dict[key]);
		}
	}

	public void OnAfterDeserialize()
	{
		if (m_Keys.Count != m_Values.Count)
		{
			Debug.LogError("Can't restore dictionry with unbalaned key/values");
			return;
		}
		dict.Clear();
		for (int i = 0; i < m_Keys.Count; i++)
		{
			dict[m_Keys[i]] = m_Values[i];
		}
	}
}
