using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio;

[Serializable]
public class KeyframeGroupDictionary : ISerializationCallbackReceiver, IEnumerable<string>, IEnumerable
{
	[NonSerialized]
	private Dictionary<string, IKeyframeGroup> m_Groups = new Dictionary<string, IKeyframeGroup>();

	[SerializeField]
	private ColorGroupDictionary m_ColorGroup = new ColorGroupDictionary();

	[SerializeField]
	private NumberGroupDictionary m_NumberGroup = new NumberGroupDictionary();

	[SerializeField]
	private TextureGroupDictionary m_TextureGroup = new TextureGroupDictionary();

	[SerializeField]
	private SpherePointGroupDictionary m_SpherePointGroup = new SpherePointGroupDictionary();

	[SerializeField]
	private BoolGroupDictionary m_BoolGroup = new BoolGroupDictionary();

	public IKeyframeGroup this[string aKey]
	{
		get
		{
			return m_Groups[aKey];
		}
		set
		{
			m_Groups[aKey] = value;
		}
	}

	public bool ContainsKey(string key)
	{
		return m_Groups.ContainsKey(key);
	}

	public void Clear()
	{
		m_Groups.Clear();
	}

	public T GetGroup<T>(string propertyName) where T : class
	{
		if (typeof(T) == typeof(ColorKeyframeGroup))
		{
			return m_Groups[propertyName] as T;
		}
		if (typeof(T) == typeof(NumberKeyframeGroup))
		{
			return m_Groups[propertyName] as T;
		}
		if (typeof(T) == typeof(TextureKeyframeGroup))
		{
			return m_Groups[propertyName] as T;
		}
		if (typeof(T) == typeof(SpherePointGroupDictionary))
		{
			return m_Groups[propertyName] as T;
		}
		if (typeof(T) == typeof(BoolKeyframeGroup))
		{
			return m_Groups[propertyName] as T;
		}
		return null;
	}

	public void OnBeforeSerialize()
	{
		m_ColorGroup.Clear();
		m_NumberGroup.Clear();
		m_TextureGroup.Clear();
		m_SpherePointGroup.Clear();
		m_BoolGroup.Clear();
		foreach (string key in m_Groups.Keys)
		{
			IKeyframeGroup keyframeGroup = m_Groups[key];
			if (keyframeGroup is ColorKeyframeGroup)
			{
				m_ColorGroup[key] = keyframeGroup as ColorKeyframeGroup;
			}
			else if (keyframeGroup is NumberKeyframeGroup)
			{
				m_NumberGroup[key] = keyframeGroup as NumberKeyframeGroup;
			}
			else if (keyframeGroup is TextureKeyframeGroup)
			{
				m_TextureGroup[key] = keyframeGroup as TextureKeyframeGroup;
			}
			else if (keyframeGroup is SpherePointKeyframeGroup)
			{
				m_SpherePointGroup[key] = keyframeGroup as SpherePointKeyframeGroup;
			}
			else if (keyframeGroup is BoolKeyframeGroup)
			{
				m_BoolGroup[key] = keyframeGroup as BoolKeyframeGroup;
			}
		}
	}

	public void OnAfterDeserialize()
	{
		m_Groups.Clear();
		foreach (string key in m_ColorGroup.dict.Keys)
		{
			m_Groups[key] = m_ColorGroup[key];
		}
		foreach (string key2 in m_NumberGroup.dict.Keys)
		{
			m_Groups[key2] = m_NumberGroup[key2];
		}
		foreach (string key3 in m_TextureGroup.dict.Keys)
		{
			m_Groups[key3] = m_TextureGroup[key3];
		}
		foreach (string key4 in m_SpherePointGroup.dict.Keys)
		{
			m_Groups[key4] = m_SpherePointGroup[key4];
		}
		foreach (string key5 in m_BoolGroup.dict.Keys)
		{
			m_Groups[key5] = m_BoolGroup[key5];
		}
	}

	public IEnumerator<string> GetEnumerator()
	{
		return m_Groups.Keys.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
