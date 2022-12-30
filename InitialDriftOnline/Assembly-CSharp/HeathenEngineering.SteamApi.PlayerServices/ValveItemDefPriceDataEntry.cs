using System;

namespace HeathenEngineering.SteamApi.PlayerServices;

[Serializable]
public class ValveItemDefPriceDataEntry
{
	public string currencyCode = "EUR";

	public uint value = 100u;

	public override string ToString()
	{
		return currencyCode + value.ToString("000");
	}
}
