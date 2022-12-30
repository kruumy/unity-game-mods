using System;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamApi.Foundation;

[Serializable]
[CreateAssetMenu(menuName = "Steamworks/Foundation/Float Stat Data")]
public class SteamFloatStatData : SteamStatData
{
	[SerializeField]
	private float value;

	public float Value
	{
		get
		{
			return value;
		}
		set
		{
			SetFloatStat(value);
		}
	}

	public override StatDataType DataType => StatDataType.Float;

	public override float GetFloatValue()
	{
		return Value;
	}

	public override int GetIntValue()
	{
		return (int)Value;
	}

	public override void SetFloatStat(float value)
	{
		if (this.value != value)
		{
			this.value = value;
			SteamUserStats.SetStat(statName, value);
			ValueChanged.Invoke(this);
		}
	}

	public override void SetIntStat(int value)
	{
		if (this.value != (float)value)
		{
			this.value = value;
			SteamUserStats.SetStat(statName, value);
			ValueChanged.Invoke(this);
		}
	}

	public override void StoreStats()
	{
		SteamUserStats.StoreStats();
	}

	internal override void InternalUpdateValue(int value)
	{
		if ((float)value != Value)
		{
			Value = value;
			ValueChanged.Invoke(this);
		}
	}

	internal override void InternalUpdateValue(float value)
	{
		if (value != Value)
		{
			Value = value;
			ValueChanged.Invoke(this);
		}
	}
}
