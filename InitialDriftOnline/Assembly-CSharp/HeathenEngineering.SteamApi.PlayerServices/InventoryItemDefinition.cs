using System;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamApi.PlayerServices;

public abstract class InventoryItemDefinition : InventoryItemPointer
{
	[SerializeField]
	public List<SteamItemDetails_t> Instances;

	public override InventoryItemType ItemType => InventoryItemType.ItemDefinition;

	public int Count
	{
		get
		{
			if (Instances != null)
			{
				return Instances.Sum((SteamItemDetails_t p) => p.m_unQuantity);
			}
			return 0;
		}
	}

	public void Consume(int count)
	{
		if (Count <= count)
		{
			return;
		}
		int num = 0;
		List<SteamItemDetails_t> list = new List<SteamItemDetails_t>();
		foreach (SteamItemDetails_t instance in Instances)
		{
			if (count - num >= instance.m_unQuantity)
			{
				num += instance.m_unQuantity;
				SteamworksPlayerInventory.ConsumeItem(instance.m_itemId, instance.m_unQuantity, delegate(bool status, SteamItemDetails_t[] results)
				{
					if (!status)
					{
						string[] obj2 = new string[5] { "Failed to consume (", null, null, null, null };
						ushort unQuantity = instance.m_unQuantity;
						obj2[1] = unQuantity.ToString();
						obj2[2] = ") units of item [";
						int steamItemDef2 = instance.m_iDefinition.m_SteamItemDef;
						obj2[3] = steamItemDef2.ToString();
						obj2[4] = "]";
						Debug.LogWarning(string.Concat(obj2));
						SteamworksInventorySettings.Current.ItemsConsumed.Invoke(status, results);
					}
				});
				SteamItemDetails_t item = instance;
				item.m_unQuantity = 0;
				list.Add(item);
				continue;
			}
			int need = count - num;
			num += need;
			SteamworksPlayerInventory.ConsumeItem(instance.m_itemId, Convert.ToUInt32(need), delegate(bool status, SteamItemDetails_t[] results)
			{
				if (!status)
				{
					string[] obj = new string[5]
					{
						"Failed to consume (",
						need.ToString(),
						") units of item [",
						null,
						null
					};
					int steamItemDef = instance.m_iDefinition.m_SteamItemDef;
					obj[3] = steamItemDef.ToString();
					obj[4] = "]";
					Debug.LogWarning(string.Concat(obj));
				}
				if (SteamworksInventorySettings.Current != null)
				{
					SteamworksInventorySettings.Current.ItemsConsumed.Invoke(status, results);
				}
			});
			SteamItemDetails_t item2 = instance;
			item2.m_unQuantity -= Convert.ToUInt16(need);
			list.Add(item2);
			break;
		}
		foreach (SteamItemDetails_t edit in list)
		{
			Instances.RemoveAll((SteamItemDetails_t p) => p.m_itemId == edit.m_itemId);
			Instances.Add(edit);
		}
	}

	public List<ExchangeItemCount> FetchItemCount(uint count, bool decriment)
	{
		if (Count >= count)
		{
			int num = 0;
			List<ExchangeItemCount> list = new List<ExchangeItemCount>();
			List<SteamItemDetails_t> list2 = new List<SteamItemDetails_t>();
			foreach (SteamItemDetails_t instance in Instances)
			{
				if (count - num >= instance.m_unQuantity)
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
				int num2 = Convert.ToInt32(count - num);
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
			if (decriment)
			{
				foreach (SteamItemDetails_t edit in list2)
				{
					Instances.RemoveAll((SteamItemDetails_t p) => p.m_itemId == edit.m_itemId);
					Instances.Add(edit);
				}
				return list;
			}
			return list;
		}
		return null;
	}

	public bool TransferQuantity(int source, uint quantity, int destination)
	{
		SteamItemDetails_t source2 = Instances[source];
		SteamItemInstanceID_t destination2 = SteamItemInstanceID_t.Invalid;
		if (destination > -1)
		{
			destination2 = Instances[destination].m_itemId;
		}
		return TransferQuantity(source2, quantity, destination2);
	}

	public bool TransferQuantity(SteamItemDetails_t source, uint quantity, SteamItemInstanceID_t destination)
	{
		if (source.m_unQuantity >= quantity)
		{
			return SteamworksPlayerInventory.TransferQuantity(source.m_itemId, quantity, destination, delegate
			{
				Instances.RemoveAll((SteamItemDetails_t p) => p.m_itemId == source.m_itemId);
				source.m_unQuantity -= Convert.ToUInt16(quantity);
				Instances.Add(source);
			});
		}
		return false;
	}

	public bool SplitInstance(SteamItemDetails_t source, uint quantity)
	{
		if (source.m_unQuantity >= quantity)
		{
			return SteamworksPlayerInventory.TransferQuantity(source.m_itemId, quantity, SteamItemInstanceID_t.Invalid, delegate
			{
				Instances.RemoveAll((SteamItemDetails_t p) => p.m_itemId == source.m_itemId);
				source.m_unQuantity -= Convert.ToUInt16(quantity);
				Instances.Add(source);
			});
		}
		Debug.LogWarning("Unable to split instance, insufficent units available to move.");
		return false;
	}

	public bool StackInstance(SteamItemDetails_t source, SteamItemInstanceID_t destination)
	{
		return TransferQuantity(source, source.m_unQuantity, destination);
	}

	public void Consolidate()
	{
		if (Instances != null)
		{
			if (Instances.Count > 1)
			{
				List<SteamItemInstanceID_t> removedInstances = new List<SteamItemInstanceID_t>();
				SteamItemDetails_t steamItemDetails_t = Instances[0];
				for (int i = 1; i < Instances.Count; i++)
				{
					SteamItemDetails_t toMove = Instances[i];
					if (!SteamworksPlayerInventory.TransferQuantity(toMove.m_itemId, toMove.m_unQuantity, steamItemDetails_t.m_itemId, delegate(bool result)
					{
						if (!result)
						{
							Debug.LogError("Failed to stack an instance, please refresh the item instances for item definition [" + base.name + "].");
						}
						else
						{
							removedInstances.Add(toMove.m_itemId);
						}
					}))
					{
						Debug.LogError("Steam activly refused a TransferItemQuantity request during the Consolodate operation. No further requests will be sent.");
					}
				}
				{
					foreach (SteamItemInstanceID_t instance in removedInstances)
					{
						Instances.RemoveAll((SteamItemDetails_t p) => p.m_itemId == instance);
					}
					return;
				}
			}
			Debug.LogWarning("Unable to consolodate items, this item only has 1 instance. No action will be taken.");
		}
		else
		{
			Debug.LogWarning("Unable to consolodate items, this item only has no instances. No action will be taken.");
		}
	}

	public void StartPurchase(uint quantity)
	{
		SteamItemDef_t[] pArrayItemDefs = new SteamItemDef_t[1] { DefinitionID };
		uint[] punArrayQuantity = new uint[1] { quantity };
		SteamInventory.StartPurchase(pArrayItemDefs, punArrayQuantity, 1u);
	}
}
