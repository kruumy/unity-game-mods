using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamApi.PlayerServices;

[CreateAssetMenu(menuName = "Steamworks/Player Services/Inventory Item Generator")]
public class ItemGeneratorDefinition : InventoryItemPointer
{
	public List<InventoryItemPointerCount> Content;

	public override InventoryItemType ItemType => InventoryItemType.ItemGenerator;

	public void TriggerDrop(Action<bool, SteamItemDetails_t[]> callback)
	{
		if (!SteamworksPlayerInventory.TriggerItemDrop(DefinitionID, callback))
		{
			Debug.LogWarning("[ItemGeneratorDefinition.TriggerDrop] - Call failed.");
		}
	}

	public void TriggerDrop()
	{
		if (!SteamworksPlayerInventory.TriggerItemDrop(DefinitionID, delegate(bool status, SteamItemDetails_t[] results)
		{
			if (!status)
			{
				Debug.LogWarning("[ItemGeneratorDefinition.TriggerDrop] - Call returned an error status.");
			}
		}))
		{
			Debug.LogWarning("[ItemGeneratorDefinition.TriggerDrop] - Call failed.");
		}
	}
}
