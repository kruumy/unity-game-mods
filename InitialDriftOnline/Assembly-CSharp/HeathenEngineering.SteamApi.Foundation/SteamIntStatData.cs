using System;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamApi.Foundation;

[Serializable]
[CreateAssetMenu(menuName = "Steamworks/Foundation/Int Stat Data")]
public class SteamIntStatData : SteamStatData
{
	[SerializeField]
	private int value;

	public int Value
	{
		get
		{
			return value;
		}
		set
		{
			SetIntStat(value);
		}
	}

	public override StatDataType DataType => StatDataType.Int;

	public override float GetFloatValue()
	{
		return Value;
	}

	public override int GetIntValue()
	{
		return Value;
	}

	public override void SetFloatStat(float value)
	{
		if (this.value != (int)value)
		{
			this.value = (int)value;
			SteamUserStats.SetStat(statName, value);
			ValueChanged.Invoke(this);
		}
	}

	public override void SetIntStat(int value)
	{
		if (this.value != value)
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
		if (value != Value)
		{
			Value = value;
			ValueChanged.Invoke(this);
		}
	}

	internal override void InternalUpdateValue(float value)
	{
		int num = (int)value;
		if (num != Value)
		{
			Value = num;
			ValueChanged.Invoke(this);
		}
	}
}
