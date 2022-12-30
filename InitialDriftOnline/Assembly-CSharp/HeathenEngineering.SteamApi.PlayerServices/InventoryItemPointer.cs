using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamApi.PlayerServices;

public abstract class InventoryItemPointer : ScriptableObject
{
	public SteamItemDef_t DefinitionID;

	public List<CraftingRecipe> Recipes;

	[HideInInspector]
	public List<ValveItemDefAttribute> ValveItemDefAttributes;

	public abstract InventoryItemType ItemType { get; }

	public CraftingRecipe this[string name] => Recipes?.FirstOrDefault((CraftingRecipe p) => p.name == name);

	public ItemExchangeRecipe PrepareItemExchange(CraftingRecipe recipe, out Dictionary<InventoryItemDefinition, List<SteamItemDetails_t>> Edits)
	{
		ItemExchangeRecipe itemExchangeRecipe = new ItemExchangeRecipe();
		itemExchangeRecipe.ItemToGenerate = DefinitionID;
		itemExchangeRecipe.ItemsToConsume = new List<ExchangeItemCount>();
		foreach (InventoryItemDefinitionCount item3 in recipe.Items)
		{
			if (item3.Item.Count < item3.Count)
			{
				Debug.LogError("InventoryItemPointer.Craft - Failed to fetch the required items for the recipe, insufficent supply of '" + item3.Item.name + "'.");
				Edits = null;
				return null;
			}
		}
		Edits = new Dictionary<InventoryItemDefinition, List<SteamItemDetails_t>>();
		foreach (InventoryItemDefinitionCount item4 in recipe.Items)
		{
			if (item4.Item.Count >= item4.Count)
			{
				int num = 0;
				List<ExchangeItemCount> list = new List<ExchangeItemCount>();
				List<SteamItemDetails_t> list2 = new List<SteamItemDetails_t>();
				foreach (SteamItemDetails_t instance in item4.Item.Instances)
				{
					if (item4.Count - num >= instance.m_unQuantity)
					{
						num += instance.m_unQuantity;
						list.Add(new ExchangeItemCount
						{
							InstanceId = instance.m_itemId,
							Quantity = instance.m_unQuantity
						});
						SteamItemDetails_t item = instance;
						item.m_unQuantity = 0;
						list2.Add(item);
						continue;
					}
					int num2 = Convert.ToInt32(item4.Count - num);
					num += num2;
					list.Add(new ExchangeItemCount
					{
						InstanceId = instance.m_itemId,
						Quantity = Convert.ToUInt32(num2)
					});
					SteamItemDetails_t item2 = instance;
					item2.m_unQuantity -= Convert.ToUInt16(num2);
					list2.Add(item2);
					break;
				}
				Edits.Add(item4.Item, list2);
				itemExchangeRecipe.ItemsToConsume.AddRange(list);
				continue;
			}
			Debug.LogWarning("Crafting request was unable to complete due to insuffient resources.");
			return null;
		}
		return itemExchangeRecipe;
	}

	public void Craft(CraftingRecipe recipe)
	{
		Dictionary<InventoryItemDefinition, List<SteamItemDetails_t>> edits;
		ItemExchangeRecipe itemExchangeRecipe = PrepareItemExchange(recipe, out edits);
		if (itemExchangeRecipe.ItemsToConsume == null || itemExchangeRecipe.ItemsToConsume.Count < 1)
		{
			Debug.LogWarning("Attempted to craft item [" + base.name + "] with no items to consume selected!\nThis will be refused by Steam so will not be sent!");
		}
		else if (itemExchangeRecipe != null && !SteamworksPlayerInventory.ExchangeItems(itemExchangeRecipe, delegate(bool status, SteamItemDetails_t[] results)
		{
			if (status)
			{
				foreach (KeyValuePair<InventoryItemDefinition, List<SteamItemDetails_t>> item2 in edits)
				{
					foreach (SteamItemDetails_t item in item2.Value)
					{
						item2.Key.Instances.RemoveAll((SteamItemDetails_t p) => p.m_itemId == item.m_itemId);
						item2.Key.Instances.Add(item);
					}
				}
				if (SteamworksInventorySettings.Current != null && SteamworksInventorySettings.Current.LogDebugMessages)
				{
					StringBuilder stringBuilder = new StringBuilder("Inventory Item [" + base.name + "] Crafted,\nItems Consumed:\n");
					foreach (InventoryItemDefinitionCount item3 in recipe.Items)
					{
						stringBuilder.Append("\t" + item3.Count + " [" + item3.Item.name + "]");
					}
				}
			}
			else if (SteamworksInventorySettings.Current != null && SteamworksInventorySettings.Current.LogDebugMessages)
			{
				Debug.LogWarning("Request to craft item [" + base.name + "] failed, confirm the item and recipie configurations are correct in the app settings.");
			}
			if (SteamworksInventorySettings.Current != null)
			{
				SteamworksInventorySettings.Current.ItemsExchanged.Invoke(status, results);
			}
		}) && SteamworksInventorySettings.Current != null)
		{
			SteamworksInventorySettings.Current.ItemsExchanged.Invoke(arg0: false, new SteamItemDetails_t[0]);
			if (SteamworksInventorySettings.Current.LogDebugMessages)
			{
				Debug.LogWarning("Request to craft item [" + base.name + "] was refused by Steam.");
			}
		}
	}

	public void Craft(int recipeIndex)
	{
		CraftingRecipe recipe = Recipes[recipeIndex];
		Craft(recipe);
	}

	public void GrantPromoItem()
	{
		SteamworksPlayerInventory.AddPromoItem(DefinitionID, delegate(bool status, SteamItemDetails_t[] results)
		{
			if (SteamworksInventorySettings.Current != null)
			{
				SteamworksInventorySettings.Current.ItemsGranted.Invoke(status, results);
			}
		});
	}
}
