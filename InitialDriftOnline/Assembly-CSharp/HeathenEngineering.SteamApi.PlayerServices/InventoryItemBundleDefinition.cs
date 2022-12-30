using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamApi.PlayerServices;

[CreateAssetMenu(menuName = "Steamworks/Player Services/Item Bundle")]
public class InventoryItemBundleDefinition : InventoryItemPointer
{
	public List<InventoryItemPointerCount> Content;

	public override InventoryItemType ItemType => InventoryItemType.ItemBundle;

	public void StartPurchase(uint quantity)
	{
		SteamItemDef_t[] pArrayItemDefs = new SteamItemDef_t[1] { DefinitionID };
		uint[] punArrayQuantity = new uint[1] { quantity };
		SteamInventory.StartPurchase(pArrayItemDefs, punArrayQuantity, 1u);
	}
}
