using System;

namespace HeathenEngineering.SteamApi.PlayerServices;

[Serializable]
public class ValveItemDefPriceCategory
{
	public uint version = 1u;

	public ValvePriceCategories price = ValvePriceCategories.VLV100;

	public override string ToString()
	{
		return version + ";" + price;
	}
}
