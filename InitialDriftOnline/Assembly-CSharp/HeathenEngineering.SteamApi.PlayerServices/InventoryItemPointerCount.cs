using System;

namespace HeathenEngineering.SteamApi.PlayerServices;

[Serializable]
public class InventoryItemPointerCount
{
	public InventoryItemPointer Item;

	public uint Count;

	public override string ToString()
	{
		return Item.DefinitionID.m_SteamItemDef + "x" + Count;
	}
}
