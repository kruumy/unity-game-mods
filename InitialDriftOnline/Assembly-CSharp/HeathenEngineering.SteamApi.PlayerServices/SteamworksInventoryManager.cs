using System.Collections.Generic;
using HeathenEngineering.SteamApi.Foundation;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.PlayerServices;

public class SteamworksInventoryManager : MonoBehaviour
{
	public SteamworksInventorySettings Settings;

	public bool RefreshOnStart = true;

	public UnityEvent ItemInstancesUpdated;

	public UnityItemDetailEvent ItemsGranted;

	public UnityItemDetailEvent ItemsConsumed;

	public UnityItemDetailEvent ItemsExchanged;

	public UnityItemDetailEvent ItemsDroped;

	public InventoryItemDefinition this[SteamItemDetails_t item] => GetDefinition(item);

	public InventoryItemDefinition this[SteamItemDef_t item] => GetDefinition(item);

	public InventoryItemDefinition this[int itemId] => GetDefinition(itemId);

	private void OnEnable()
	{
		if (Settings == null)
		{
			Debug.LogWarning("Steamworks Inventory Manager requires a Steamworks Inventory Settings object to funciton!\nThis componenet will be disabled.");
			base.enabled = false;
			return;
		}
		Settings.Register();
		if (Settings.ItemInstancesUpdated == null)
		{
			Settings.ItemInstancesUpdated = new UnityEvent();
		}
		Settings.ItemInstancesUpdated.AddListener(ItemInstancesUpdated.Invoke);
		if (Settings.ItemsGranted == null)
		{
			Settings.ItemsGranted = new UnityItemDetailEvent();
		}
		Settings.ItemsGranted.AddListener(ItemsGranted.Invoke);
		if (Settings.ItemsConsumed == null)
		{
			Settings.ItemsConsumed = new UnityItemDetailEvent();
		}
		Settings.ItemsConsumed.AddListener(ItemsConsumed.Invoke);
		if (Settings.ItemsExchanged == null)
		{
			Settings.ItemsExchanged = new UnityItemDetailEvent();
		}
		Settings.ItemsExchanged.AddListener(ItemsExchanged.Invoke);
		if (Settings.ItemsDroped == null)
		{
			Settings.ItemsDroped = new UnityItemDetailEvent();
		}
		Settings.ItemsDroped.AddListener(ItemsDroped.Invoke);
	}

	private void OnDisable()
	{
		if (!(Settings == null))
		{
			if (Settings.ItemInstancesUpdated != null)
			{
				Settings.ItemInstancesUpdated.RemoveListener(ItemInstancesUpdated.Invoke);
			}
			if (Settings.ItemsGranted != null)
			{
				Settings.ItemsGranted.RemoveListener(ItemsGranted.Invoke);
			}
			if (Settings.ItemsConsumed != null)
			{
				Settings.ItemsConsumed.RemoveListener(ItemsConsumed.Invoke);
			}
			if (Settings.ItemsExchanged != null)
			{
				Settings.ItemsExchanged.RemoveListener(ItemsExchanged.Invoke);
			}
			if (Settings.ItemsDroped != null)
			{
				Settings.ItemsDroped.RemoveListener(ItemsDroped.Invoke);
			}
		}
	}

	private void Start()
	{
		if (Settings != null && RefreshOnStart)
		{
			Settings.ClearItemCounts();
			Settings.RefreshInventory();
		}
	}

	public T GetDefinition<T>(SteamItemDetails_t steamDetail) where T : InventoryItemDefinition
	{
		return Settings.GetDefinition<T>(steamDetail);
	}

	public T GetDefinition<T>(SteamItemDef_t steamDefinition) where T : InventoryItemDefinition
	{
		return Settings.GetDefinition<T>(steamDefinition);
	}

	public InventoryItemDefinition GetDefinition(SteamItemDetails_t steamDetail)
	{
		return Settings.GetDefinition(steamDetail);
	}

	public InventoryItemDefinition GetDefinition(SteamItemDef_t steamDefinition)
	{
		return Settings.GetDefinition(steamDefinition);
	}

	public InventoryItemDefinition GetDefinition(int steamDefinition)
	{
		return Settings.GetDefinition(steamDefinition);
	}

	public void RefreshInventory()
	{
		Settings.RefreshInventory();
	}

	public void GrantAllPromotionalItems()
	{
		Settings.GrantAllPromotionalItems();
	}

	public void GrantPromotionalItem(InventoryItemDefinition itemDefinition)
	{
		Settings.GrantPromotionalItem(itemDefinition);
	}

	public void GrantPromotionalItems(IEnumerable<InventoryItemDefinition> itemDefinitions)
	{
		Settings.GrantPromotionalItems(itemDefinitions);
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
		Settings.ConsumeItem(itemDefinition);
	}

	public void ConsumeItem(InventoryItemDefinition itemDefinition, int count)
	{
		Settings.ConsumeItem(itemDefinition, count);
	}

	public void ConsumeItem(InventoryItemDefinition itemDefinition, SteamItemInstanceID_t instanceId, int count)
	{
		Settings.ConsumeItem(itemDefinition, instanceId, count);
	}

	public void ConsumeItem(SteamItemInstanceID_t instanceId, int count)
	{
		Settings.ConsumeItem(instanceId, count);
	}

	public void ExchangeItems(InventoryItemDefinition itemToCraft, CraftingRecipe recipe)
	{
		itemToCraft.Craft(recipe);
	}

	public void ExchangeItems(InventoryItemDefinition itemToCraft, int recipeIndex)
	{
		itemToCraft.Craft(recipeIndex);
	}

	public void TriggerItemDrop(ItemGeneratorDefinition generator, bool postDropRefresh = false)
	{
		Settings.TriggerItemDrop(generator, postDropRefresh);
	}

	public void Consolidate(InventoryItemDefinition item)
	{
		item.Consolidate();
	}

	public void TriggerItemDrop(ItemGeneratorDefinition generator)
	{
		TriggerItemDrop(generator, postDropRefresh: false);
	}

	public void TriggerItemDropAndRefresh(ItemGeneratorDefinition generator)
	{
		TriggerItemDrop(generator, postDropRefresh: true);
	}
}
