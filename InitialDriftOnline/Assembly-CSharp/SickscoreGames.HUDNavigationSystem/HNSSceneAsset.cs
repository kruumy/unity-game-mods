using System;
using UnityEngine;

namespace SickscoreGames.HUDNavigationSystem;

[Serializable]
public class HNSSceneAsset : ISerializationCallbackReceiver
{
	[SerializeField]
	private string _path = string.Empty;

	public string path
	{
		get
		{
			return _path;
		}
		set
		{
			_path = value;
		}
	}

	public static implicit operator string(HNSSceneAsset sceneReference)
	{
		return sceneReference.path;
	}

	public void OnBeforeSerialize()
	{
	}

	public void OnAfterDeserialize()
	{
	}
}
