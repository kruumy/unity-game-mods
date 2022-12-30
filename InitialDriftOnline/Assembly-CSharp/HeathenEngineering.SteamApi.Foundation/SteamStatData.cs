using System;
using UnityEngine;

namespace HeathenEngineering.SteamApi.Foundation;

[Serializable]
public abstract class SteamStatData : ScriptableObject
{
	public enum StatDataType
	{
		Int,
		Float
	}

	public string statName;

	public UnityStatEvent ValueChanged;

	public abstract StatDataType DataType { get; }

	internal abstract void InternalUpdateValue(int value);

	internal abstract void InternalUpdateValue(float value);

	public abstract int GetIntValue();

	public abstract float GetFloatValue();

	public abstract void SetIntStat(int value);

	public abstract void SetFloatStat(float value);

	public abstract void StoreStats();
}
