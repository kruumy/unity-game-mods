using System;
using System.Collections.Generic;

namespace HeathenEngineering.SteamApi.PlayerServices;

[Serializable]
public class InventoryItemDefinitionCount
{
	public InventoryItemDefinition Item;

	public uint Count;

	public List<ExchangeItemCount> FetchFromItem(bool decriment)
	{
		return Item.FetchItemCount(Count, decriment);
	}

	public override string ToString()
	{
		return Item.DefinitionID.m_SteamItemDef + "x" + Count;
	}
}
