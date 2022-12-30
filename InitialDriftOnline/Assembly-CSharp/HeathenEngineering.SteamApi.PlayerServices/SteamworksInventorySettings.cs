using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HeathenEngineering.SteamApi.Foundation;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.PlayerServices;

[CreateAssetMenu(menuName = "Steamworks/Player Services/Inventory Settings")]
public class SteamworksInventorySettings : ScriptableObject
{
	public static SteamworksInventorySettings Current;

	public bool LogDebugMessages;

	public List<InventoryItemDefinition> ItemDefinitions = new List<InventoryItemDefinition>();

	public List<ItemGeneratorDefinition> ItemGenerators = new List<ItemGeneratorDefinition>();

	public List<TagGeneratorDefinition> TagGenerators = new List<TagGeneratorDefinition>();

	public List<InventoryItemBundleDefinition> ItemBundles = new List<InventoryItemBundleDefinition>();

	[HideInInspector]
	public UnityEvent ItemInstancesUpdated = new UnityEvent();

	[HideInInspector]
	public UnityItemDetailEvent ItemsGranted = new UnityItemDetailEvent();

	[HideInInspector]
	public UnityItemDetailEvent ItemsConsumed = new UnityItemDetailEvent();

	[HideInInspector]
	public UnityItemDetailEvent ItemsExchanged = new UnityItemDetailEvent();

	[HideInInspector]
	public UnityItemDetailEvent ItemsDroped = new UnityItemDetailEvent();

	private Dictionary<SteamItemDef_t, InventoryItemPointer> ItemPointerIndex = new Dictionary<SteamItemDef_t, InventoryItemPointer>();

	private Dictionary<SteamItemDef_t, InventoryItemDefinition> ItemDefinitionIndex = new Dictionary<SteamItemDef_t, InventoryItemDefinition>();

	private Dictionary<SteamItemDef_t, ItemGeneratorDefinition> ItemGeneratorIndex = new Dictionary<SteamItemDef_t, ItemGeneratorDefinition>();

	public bool IsActive
	{
		get
		{
			return Current == this;
		}
		set
		{
			if (value)
			{
				Register();
			}
			else if (Current == this)
			{
				Current = null;
			}
		}
	}

	public InventoryItemPointer this[SteamItemDef_t id]
	{
		get
		{
			if (ItemPointerIndex == null)
			{
				BuildIndex();
			}
			if (ItemPointerIndex.ContainsKey(id))
			{
				return ItemPointerIndex[id];
			}
			return null;
		}
	}

	public InventoryItemPointer this[SteamItemDetails_t item] => this[item.m_iDefinition];

	public InventoryItemPointer this[int itemId] => this[new SteamItemDef_t(itemId)];

	public void BuildIndex()
	{
		if (ItemPointerIndex == null)
		{
			ItemPointerIndex = new Dictionary<SteamItemDef_t, InventoryItemPointer>();
		}
		if (ItemDefinitionIndex == null)
		{
			ItemDefinitionIndex = new Dictionary<SteamItemDef_t, InventoryItemDefinition>();
		}
		if (ItemGeneratorIndex == null)
		{
			ItemGeneratorIndex = new Dictionary<SteamItemDef_t, ItemGeneratorDefinition>();
		}
		if (LogDebugMessages)
		{
			int num = ((ItemDefinitions != null) ? ItemDefinitions.Count : 0);
			int num2 = ((ItemGenerators != null) ? ItemGenerators.Count : 0);
			Debug.Log("Building internal indices for " + num + " items and " + num2 + " generators.");
		}
		foreach (InventoryItemDefinition itemDefinition in ItemDefinitions)
		{
			if (ItemDefinitionIndex.ContainsKey(itemDefinition.DefinitionID))
			{
				ItemDefinitionIndex[itemDefinition.DefinitionID] = itemDefinition;
			}
			else
			{
				ItemDefinitionIndex.Add(itemDefinition.DefinitionID, itemDefinition);
			}
			if (ItemPointerIndex.ContainsKey(itemDefinition.DefinitionID))
			{
				ItemPointerIndex[itemDefinition.DefinitionID] = itemDefinition;
			}
			else
			{
				ItemPointerIndex.Add(itemDefinition.DefinitionID, itemDefinition);
			}
		}
		foreach (ItemGeneratorDefinition itemGenerator in ItemGenerators)
		{
			if (ItemGeneratorIndex.ContainsKey(itemGenerator.DefinitionID))
			{
				ItemGeneratorIndex[itemGenerator.DefinitionID] = itemGenerator;
			}
			else
			{
				ItemGeneratorIndex.Add(itemGenerator.DefinitionID, itemGenerator);
			}
			if (ItemPointerIndex.ContainsKey(itemGenerator.DefinitionID))
			{
				ItemPointerIndex[itemGenerator.DefinitionID] = itemGenerator;
			}
			else
			{
				ItemPointerIndex.Add(itemGenerator.DefinitionID, itemGenerator);
			}
		}
	}

	public void ClearItemCounts()
	{
		foreach (InventoryItemDefinition itemDefinition in ItemDefinitions)
		{
			if (itemDefinition.Instances == null)
			{
				itemDefinition.Instances = new List<SteamItemDetails_t>();
			}
			else
			{
				itemDefinition.Instances.Clear();
			}
		}
	}

	public static void InternalItemDetailUpdate(IEnumerable<SteamItemDetails_t> details)
	{
		if (Current != null && details != null)
		{
			Current.HandleItemDetailUpdate(details);
		}
	}

	public void HandleItemDetailUpdate(IEnumerable<SteamItemDetails_t> details)
	{
		bool flag = false;
		foreach (SteamItemDetails_t detail in details)
		{
			if (ItemDefinitionIndex.ContainsKey(detail.m_iDefinition))
			{
				flag = true;
				InventoryItemDefinition inventoryItemDefinition = ItemDefinitionIndex[detail.m_iDefinition];
				if (inventoryItemDefinition.Instances == null)
				{
					inventoryItemDefinition.Instances = new List<SteamItemDetails_t>();
				}
				else
				{
					inventoryItemDefinition.Instances.RemoveAll((SteamItemDetails_t p) => p.m_itemId == detail.m_itemId);
				}
				inventoryItemDefinition.Instances.Add(detail);
			}
			else if (LogDebugMessages)
			{
				Debug.LogWarning("No item definition found for item " + detail.m_iDefinition.m_SteamItemDef + " but an item instance " + detail.m_itemId.m_SteamItemInstanceID + " exists in the player's inventory with a unit count of " + detail.m_unQuantity + "\nConsider adding an item definition for this to your Steam Inventory Settings.");
			}
		}
		if (!flag)
		{
			return;
		}
		if (LogDebugMessages)
		{
			StringBuilder stringBuilder = new StringBuilder("Inventory Item Detail Update:\n");
			foreach (InventoryItemDefinition itemDefinition in ItemDefinitions)
			{
				stringBuilder.Append("\t[" + itemDefinition.name + "] has " + itemDefinition.Instances.Count + " instances for a sum of " + itemDefinition.Count + " units.\n");
			}
			Debug.Log(stringBuilder.ToString());
		}
		ItemInstancesUpdated.Invoke();
	}

	public void Register()
	{
		if (LogDebugMessages)
		{
			if (Current == null)
			{
				Debug.Log("Registering a new Steamworks Inventory Settings object [" + base.name + "]");
			}
			else if (Current != this)
			{
				Debug.Log("Replacing Steamworks Inventory Settings object [" + Current.name + "] with [" + base.name + "]");
			}
			if (Current != null)
			{
				Debug.Log("RE-Registering Steamworks Inventory Settings object [" + base.name + "]");
			}
		}
		BuildIndex();
		Current = this;
	}

	public T GetDefinition<T>(SteamItemDetails_t steamDetail) where T : InventoryItemDefinition
	{
		return this[steamDetail] as T;
	}

	public T GetDefinition<T>(SteamItemDef_t steamDefinition) where T : InventoryItemDefinition
	{
		return this[steamDefinition] as T;
	}

	public InventoryItemDefinition GetDefinition(SteamItemDetails_t steamDetail)
	{
		return this[steamDetail] as InventoryItemDefinition;
	}

	public InventoryItemDefinition GetDefinition(SteamItemDef_t steamDefinition)
	{
		return this[steamDefinition] as InventoryItemDefinition;
	}

	public InventoryItemDefinition GetDefinition(int steamDefinition)
	{
		return this[steamDefinition] as InventoryItemDefinition;
	}

	public void RefreshInventory()
	{
		foreach (InventoryItemDefinition itemDefinition in ItemDefinitions)
		{
			if (itemDefinition.Instances == null)
			{
				itemDefinition.Instances = new List<SteamItemDetails_t>();
			}
			else
			{
				itemDefinition.Instances.Clear();
			}
		}
		if (!SteamworksPlayerInventory.GetAllItems(null))
		{
			Debug.LogWarning("[SteamworksInventorySettings.RefreshInventory] - Call failed");
		}
	}

	public void GrantAllPromotionalItems()
	{
		if (!SteamworksPlayerInventory.GrantPromoItems(delegate(bool status, SteamItemDetails_t[] results)
		{
			ItemsGranted.Invoke(status, results);
		}))
		{
			Debug.LogWarning("[SteamworksInventorySettings.GrantAllPromotionalItems] - Call failed");
		}
	}

	public void GrantPromotionalItem(InventoryItemDefinition itemDefinition)
	{
		if (!SteamworksPlayerInventory.AddPromoItem(itemDefinition.DefinitionID, delegate(bool status, SteamItemDetails_t[] results)
		{
			ItemsGranted.Invoke(status, results);
		}))
		{
			Debug.LogWarning("[SteamworksInventorySettings.GrantPromotionalItem] - Call failed");
		}
	}

	public void GrantPromotionalItems(IEnumerable<InventoryItemDefinition> itemDefinitions)
	{
		List<SteamItemDef_t> list = new List<SteamItemDef_t>();
		foreach (InventoryItemDefinition itemDefinition in itemDefinitions)
		{
			list.Add(itemDefinition.DefinitionID);
		}
		if (!SteamworksPlayerInventory.AddPromoItems(list, delegate(bool status, SteamItemDetails_t[] results)
		{
			ItemsGranted.Invoke(status, results);
		}))
		{
			Debug.LogWarning("[SteamworksInventorySettings.GrantPromotionalItems] - Call failed");
		}
	}

	public bool CheckUserResult(SteamInventoryResult_t resultHandle, ulong user)
	{
		return SteamworksPlayerInventory.CheckResultSteamID(resultHandle, user);
	}

	public bool CheckUserResult(SteamInventoryResult_t resultHandle, CSteamID user)
	{
		return SteamworksPlayerInventory.CheckResultSteamID(resultHandle, user);
	}

	public bool CheckUserResult(SteamInventoryResult_t resultHandle, SteamUserData user)
	{
		return SteamworksPlayerInventory.CheckResultSteamID(resultHandle, user);
	}

	public void ConsumeItem(InventoryItemDefinition itemDefinition)
	{
		SteamItemDetails_t target = itemDefinition.Instances.FirstOrDefault((SteamItemDetails_t p) => p.m_unQuantity > 0);
		if (!SteamworksPlayerInventory.ConsumeItem(target.m_itemId, delegate(bool status, SteamItemDetails_t[] results)
		{
			if (status)
			{
				itemDefinition.Instances.RemoveAll((SteamItemDetails_t p) => p.m_itemId == target.m_itemId);
				target.m_unQuantity--;
				itemDefinition.Instances.Add(target);
				ItemInstancesUpdated.Invoke();
				ItemsConsumed.Invoke(status, results);
			}
		}))
		{
			Debug.LogWarning("[SteamworksInventorySettings.ConsumeItem] - Call failed");
		}
	}

	public void ConsumeItem(InventoryItemDefinition itemDefinition, int count)
	{
		if (count < 1)
		{
			Debug.LogWarning("Attempted to consume a number of items less than 1; this is not possible and was note requested.");
			return;
		}
		List<ExchangeItemCount> list = new List<ExchangeItemCount>();
		int num = 0;
		foreach (SteamItemDetails_t instance in itemDefinition.Instances)
		{
			if (instance.m_unQuantity >= count - num)
			{
				list.Add(new ExchangeItemCount
				{
					InstanceId = instance.m_itemId,
					Quantity = Convert.ToUInt32(count - num)
				});
				num = count;
				break;
			}
			list.Add(new ExchangeItemCount
			{
				InstanceId = instance.m_itemId,
				Quantity = instance.m_unQuantity
			});
			num += instance.m_unQuantity;
		}
		bool flag = true;
		foreach (ExchangeItemCount exchange in list)
		{
			if (!SteamworksPlayerInventory.ConsumeItem(exchange.InstanceId, exchange.Quantity, delegate(bool status, SteamItemDetails_t[] results)
			{
				if (status)
				{
					SteamItemDetails_t item = itemDefinition.Instances.FirstOrDefault((SteamItemDetails_t p) => p.m_itemId == exchange.InstanceId);
					itemDefinition.Instances.RemoveAll((SteamItemDetails_t p) => p.m_itemId == exchange.InstanceId);
					item.m_unQuantity -= Convert.ToUInt16(exchange.Quantity);
					itemDefinition.Instances.Add(item);
					ItemInstancesUpdated.Invoke();
					ItemsConsumed.Invoke(status, results);
				}
			}))
			{
				flag = false;
				Debug.LogWarning("Failed to consume all requested items");
				break;
			}
		}
		if (!flag)
		{
			Debug.LogWarning("[SteamworksInventorySettings.ConsumeItem] - Call failed");
		}
	}

	public void ConsumeItem(InventoryItemDefinition itemDefinition, SteamItemInstanceID_t instanceId, int count)
	{
		SteamItemDetails_t target = itemDefinition.Instances.FirstOrDefault((SteamItemDetails_t p) => p.m_itemId == instanceId);
		if (!SteamworksPlayerInventory.ConsumeItem(instanceId, delegate(bool status, SteamItemDetails_t[] results)
		{
			if (status)
			{
				itemDefinition.Instances.RemoveAll((SteamItemDetails_t p) => p.m_itemId == target.m_itemId);
				target.m_unQuantity -= Convert.ToUInt16(count);
				itemDefinition.Instances.Add(target);
				ItemInstancesUpdated.Invoke();
				ItemsConsumed.Invoke(status, results);
			}
		}))
		{
			Debug.LogWarning("[SteamworksInventorySettings.ConsumeItem] - Call failed");
		}
	}

	public void ConsumeItem(SteamItemInstanceID_t instanceId, int count)
	{
		InventoryItemDefinition itemDefinition = ItemDefinitions.FirstOrDefault((InventoryItemDefinition p) => p.Instances.Any((SteamItemDetails_t i) => i.m_itemId == instanceId));
		if (itemDefinition == null)
		{
			Debug.LogError("Unable to locate the Item Definition for Item Instance " + instanceId.m_SteamItemInstanceID);
			return;
		}
		SteamItemDetails_t target = itemDefinition.Instances.FirstOrDefault((SteamItemDetails_t p) => p.m_itemId == instanceId);
		if (!SteamworksPlayerInventory.ConsumeItem(instanceId, delegate(bool status, SteamItemDetails_t[] results)
		{
			if (status)
			{
				itemDefinition.Instances.RemoveAll((SteamItemDetails_t p) => p.m_itemId == target.m_itemId);
				target.m_unQuantity -= Convert.ToUInt16(count);
				itemDefinition.Instances.Add(target);
				ItemInstancesUpdated.Invoke();
				ItemsConsumed.Invoke(status, results);
			}
		}))
		{
			Debug.LogWarning("[SteamworksInventorySettings.ConsumeItem] - Call failed");
		}
	}

	public void ExchangeItems(InventoryItemDefinition itemToCraft, CraftingRecipe recipe)
	{
		itemToCraft.Craft(recipe);
	}

	public void ExchangeItems(InventoryItemDefinition itemToCraft, int recipeIndex)
	{
		itemToCraft.Craft(recipeIndex);
	}

	public void TriggerItemDrop(ItemGeneratorDefinition generator, bool postDropRefresh)
	{
		generator.TriggerDrop(delegate(bool status, SteamItemDetails_t[] results)
		{
			if (status)
			{
				if (LogDebugMessages)
				{
					Debug.Log("Item Drop for [" + generator.name + "] completed with status " + status.ToString() + " and " + results.Count() + " instances effected.");
				}
				if (postDropRefresh)
				{
					RefreshInventory();
				}
			}
			ItemsDroped.Invoke(status, results);
		});
	}

	public void TriggerItemDrop(ItemGeneratorDefinition generator)
	{
		TriggerItemDrop(generator, postDropRefresh: false);
	}

	public void TriggerItemDropAndRefresh(ItemGeneratorDefinition generator)
	{
		TriggerItemDrop(generator, postDropRefresh: true);
	}

	public bool TransferQuantity(InventoryItemDefinition item, SteamItemDetails_t source, uint quantity, SteamItemInstanceID_t destination)
	{
		return item.TransferQuantity(source, quantity, destination);
	}

	public bool SplitInstance(InventoryItemDefinition item, SteamItemDetails_t source, uint quantity)
	{
		return item.SplitInstance(source, quantity);
	}

	public bool StackInstance(InventoryItemDefinition item, SteamItemDetails_t source, SteamItemInstanceID_t destination)
	{
		return item.StackInstance(source, destination);
	}

	public void Consolidate(InventoryItemDefinition item)
	{
		item.Consolidate();
	}
}
