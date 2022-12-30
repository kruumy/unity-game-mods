using System.Collections.Generic;
using Steamworks;

namespace HeathenEngineering.SteamApi.PlayerServices;

public class ItemExchangeRecipe
{
	public SteamItemDef_t ItemToGenerate;

	public List<ExchangeItemCount> ItemsToConsume = new List<ExchangeItemCount>();

	public ItemExchangeRecipe()
	{
	}

	public ItemExchangeRecipe(SteamItemDef_t toGenerate, IEnumerable<ExchangeItemCount> toBeConsumed)
	{
		ItemToGenerate = toGenerate;
		ItemsToConsume = new List<ExchangeItemCount>(toBeConsumed);
	}

	public SteamItemInstanceID_t[] GetInstanceArray()
	{
		return ItemsToConsume.ConvertAll((ExchangeItemCount p) => p.InstanceId).ToArray();
	}

	public uint[] GetQuantityArray()
	{
		return ItemsToConsume.ConvertAll((ExchangeItemCount p) => p.Quantity).ToArray();
	}
}
