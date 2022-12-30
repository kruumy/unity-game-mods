using System;
using System.Collections.Generic;
using System.Text;

namespace HeathenEngineering.SteamApi.PlayerServices;

[Serializable]
public class ValveItemDefPriceData
{
	public uint version = 1u;

	public List<ValveItemDefPriceDataEntry> values;

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (ValveItemDefPriceDataEntry value in values)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(",");
			}
			stringBuilder.Append(value.ToString());
		}
		return version + ";" + stringBuilder.ToString();
	}
}
