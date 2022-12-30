using System;
using System.Collections.Generic;
using System.Linq;
using HeathenEngineering.SteamApi.Foundation;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamApi.PlayerServices;

public static class SteamworksPlayerInventory
{
	private class CallRequest
	{
		public CallRequestType Type;

		public CSteamID SteamUserId;

		public Action<bool> BoolCallback;

		public Action<bool, SteamItemDetails_t[]> DetailCallback;

		public Action<bool, byte[]> SerializationCallback;
	}

	private enum CallRequestType
	{
		AddPromoItem,
		AddPromoItems,
		ConsumeItem,
		ExchangeItems,
		GenerateItems,
		GetAllItems,
		DeserializeResult,
		GetItemsByID,
		GrantPromoItems,
		GetItemIDsToSerialize,
		TransferItemQuantity,
		TriggerItemDrop
	}

	private static bool callbacksRegistered;

	private static Dictionary<SteamInventoryResult_t, CallRequest> pendingCalls;

	private static Callback<SteamInventoryResultReady_t> m_SteamInventoryResultReady;

	private static CallResult<SteamInventoryEligiblePromoItemDefIDs_t> m_SteamInventoryEligiblePromoItemDefIDs;

	public static bool RegisterCallbacks()
	{
		if (SteamSettings.current.Initialized)
		{
			if (!callbacksRegistered)
			{
				callbacksRegistered = true;
				m_SteamInventoryResultReady = Callback<SteamInventoryResultReady_t>.Create(HandleSteamInventoryResult);
				m_SteamInventoryEligiblePromoItemDefIDs = CallResult<SteamInventoryEligiblePromoItemDefIDs_t>.Create(HandleEligiblePromoItemDefIDs);
				return true;
			}
			return true;
		}
		return false;
	}

	private static void ProcessDetailQuery(SteamInventoryResultReady_t param, CallRequest callRequest, string callerName)
	{
		try
		{
			if (param.m_result != EResult.k_EResultOK)
			{
				Debug.LogError(string.Concat("The call from ", callerName, " failed to process on steam as expected, EResult = ", param.m_result, ".\nThis will report as a failed call to the provided callback."));
				if (callRequest.DetailCallback != null)
				{
					callRequest.DetailCallback(arg1: false, null);
				}
			}
			else
			{
				uint punOutItemsArraySize = 10000u;
				SteamItemDetails_t[] pOutItemsArray = new SteamItemDetails_t[10000];
				if (SteamInventory.GetResultItems(param.m_handle, pOutItemsArray, ref punOutItemsArraySize))
				{
					SteamItemDetails_t[] array = new SteamItemDetails_t[punOutItemsArraySize];
					if (SteamInventory.GetResultItems(param.m_handle, array, ref punOutItemsArraySize))
					{
						try
						{
							SteamworksInventorySettings.InternalItemDetailUpdate(array);
							if (callRequest.DetailCallback != null)
							{
								callRequest.DetailCallback(arg1: true, array);
							}
						}
						catch (Exception exception)
						{
							Debug.LogError("The callback provided to the " + callerName + " request threw an exception when invoked!");
							Debug.LogException(exception);
						}
					}
					else
					{
						Debug.LogError("Steam Inventory " + callerName + " failed to retrive the resulting Inventory Item details.");
						try
						{
							if (callRequest.DetailCallback != null)
							{
								callRequest.DetailCallback(arg1: false, null);
							}
						}
						catch (Exception exception2)
						{
							Debug.LogError("The callback provided to the " + callerName + " request threw an exception when invoked!");
							Debug.LogException(exception2);
						}
					}
				}
				else if (callRequest.DetailCallback != null)
				{
					callRequest.DetailCallback(arg1: true, null);
				}
			}
		}
		catch (Exception exception3)
		{
			Debug.LogException(exception3);
			try
			{
				if (callRequest.DetailCallback != null)
				{
					callRequest.DetailCallback(arg1: false, null);
				}
			}
			catch (Exception exception4)
			{
				Debug.LogError("The callback provided to the " + callerName + " request threw an exception when invoked!");
				Debug.LogException(exception4);
			}
		}
		Debug.Log("Destroying handle: " + param.m_handle.ToString());
		SteamInventory.DestroyResult(param.m_handle);
	}

	private static void ProcessSerializationRequest(SteamInventoryResultReady_t param, CallRequest callRequest)
	{
		if (callRequest.SerializationCallback != null)
		{
			try
			{
				if (param.m_result == EResult.k_EResultOK)
				{
					if (SteamInventory.SerializeResult(param.m_handle, null, out var punOutBufferSize))
					{
						byte[] array = new byte[punOutBufferSize];
						if (SteamInventory.SerializeResult(param.m_handle, array, out punOutBufferSize))
						{
							callRequest.SerializationCallback(arg1: true, array);
						}
						else
						{
							Debug.LogError("Steam Inventory - Serialize Result: Failed to load the serialized results to memory.");
							callRequest.SerializationCallback(arg1: false, null);
						}
					}
					else
					{
						Debug.LogError("Steam Inventory - Serialize Result: Failed to calculate the size requirement for the serialized result.");
						callRequest.SerializationCallback(arg1: false, null);
					}
				}
				else
				{
					Debug.LogError("Steam Inventory - Serialize Result: Steamworks Result state: " + param.m_result.ToString() + ".");
					callRequest.SerializationCallback(arg1: false, null);
				}
			}
			catch (Exception exception)
			{
				Debug.LogError("The callback provided to the Consumption Request threw an exception when invoked!");
				Debug.LogException(exception);
			}
		}
		SteamInventory.DestroyResult(param.m_handle);
	}

	private static void ProcessDeserializeRequest(SteamInventoryResultReady_t param, CallRequest callRequest)
	{
		try
		{
			uint punOutItemsArraySize = 10000u;
			SteamItemDetails_t[] pOutItemsArray = new SteamItemDetails_t[10000];
			if (SteamInventory.GetResultItems(param.m_handle, pOutItemsArray, ref punOutItemsArraySize))
			{
				SteamItemDetails_t[] array = new SteamItemDetails_t[punOutItemsArraySize];
				if (SteamInventory.GetResultItems(param.m_handle, array, ref punOutItemsArraySize))
				{
					try
					{
						if (SteamInventory.CheckResultSteamID(param.m_handle, callRequest.SteamUserId))
						{
							if (callRequest.DetailCallback != null)
							{
								callRequest.DetailCallback(arg1: true, array);
							}
						}
						else
						{
							Debug.LogWarning("Deserialize results returned successfuly however found that the results did not match the Steam User ID and thus are invalid.\nThis is a security measure to insure users cannot spoof inventory results.");
							if (callRequest.DetailCallback != null)
							{
								callRequest.DetailCallback(arg1: false, null);
							}
						}
					}
					catch (Exception exception)
					{
						Debug.LogError("The callback provided to the Deserialize Result request threw an exception when invoked!");
						Debug.LogException(exception);
					}
				}
				else
				{
					Debug.LogError("Steam Inventory Deserialize Result failed to retrive the resulting Inventory Item details.");
					try
					{
						if (callRequest.DetailCallback != null)
						{
							callRequest.DetailCallback(arg1: false, null);
						}
					}
					catch (Exception exception2)
					{
						Debug.LogError("The callback provided to the Deserialize Result request threw an exception when invoked!");
						Debug.LogException(exception2);
					}
				}
			}
		}
		catch (Exception exception3)
		{
			Debug.LogException(exception3);
			try
			{
				if (callRequest.DetailCallback != null)
				{
					callRequest.DetailCallback(arg1: false, null);
				}
			}
			catch (Exception exception4)
			{
				Debug.LogError("The callback provided to the Deserialize Result request threw an exception when invoked!");
				Debug.LogException(exception4);
			}
		}
		SteamInventory.DestroyResult(param.m_handle);
	}

	private static void ProcessTransferRequest(SteamInventoryResultReady_t param, CallRequest callRequest)
	{
		try
		{
			if (param.m_result == EResult.k_EResultOK)
			{
				try
				{
					if (callRequest.BoolCallback != null)
					{
						callRequest.BoolCallback(obj: true);
					}
				}
				catch (Exception exception)
				{
					Debug.LogError("The callback provided to the Transfer Item Quantity request threw an exception when invoked!");
					Debug.LogException(exception);
				}
			}
			else
			{
				try
				{
					Debug.LogError("Steam Inventory - Transfer Item Quantity: Steamworks Result state: " + param.m_result.ToString() + ".");
					if (callRequest.BoolCallback != null)
					{
						callRequest.BoolCallback(obj: false);
					}
				}
				catch (Exception exception2)
				{
					Debug.LogError("The callback provided to the Transfer Item Quantity request threw an exception when invoked!");
					Debug.LogException(exception2);
				}
			}
		}
		catch (Exception exception3)
		{
			Debug.LogException(exception3);
			try
			{
				if (callRequest.BoolCallback != null)
				{
					callRequest.BoolCallback(obj: false);
				}
			}
			catch (Exception exception4)
			{
				Debug.LogError("The callback provided to the Transfer Item Quantity request threw an exception when invoked!");
				Debug.LogException(exception4);
			}
		}
		SteamInventory.DestroyResult(param.m_handle);
	}

	private static void HandleSteamInventoryResult(SteamInventoryResultReady_t param)
	{
		if (pendingCalls.ContainsKey(param.m_handle))
		{
			CallRequest callRequest = pendingCalls[param.m_handle];
			pendingCalls.Remove(param.m_handle);
			switch (callRequest.Type)
			{
			case CallRequestType.AddPromoItem:
				ProcessDetailQuery(param, callRequest, "Add Promo Item");
				break;
			case CallRequestType.AddPromoItems:
				ProcessDetailQuery(param, callRequest, "Add Promo Items");
				break;
			case CallRequestType.ConsumeItem:
				ProcessDetailQuery(param, callRequest, "Consume Items");
				break;
			case CallRequestType.ExchangeItems:
				ProcessDetailQuery(param, callRequest, "Exchange Items");
				break;
			case CallRequestType.GenerateItems:
				ProcessDetailQuery(param, callRequest, "Generate Items");
				break;
			case CallRequestType.GetAllItems:
				ProcessDetailQuery(param, callRequest, "Get All Items");
				break;
			case CallRequestType.GetItemsByID:
				ProcessDetailQuery(param, callRequest, "Get Items By ID");
				break;
			case CallRequestType.GetItemIDsToSerialize:
				ProcessSerializationRequest(param, callRequest);
				break;
			case CallRequestType.DeserializeResult:
				ProcessDeserializeRequest(param, callRequest);
				break;
			case CallRequestType.TransferItemQuantity:
				ProcessDetailQuery(param, callRequest, "Transfer Item Quantity");
				break;
			case CallRequestType.TriggerItemDrop:
				ProcessDetailQuery(param, callRequest, "Trigger Item Drop");
				break;
			case CallRequestType.GrantPromoItems:
				ProcessDetailQuery(param, callRequest, "Grant Promo Items");
				break;
			}
		}
		else
		{
			Debug.LogWarning("Handling an unidentified Steam Inventory Result request. This may lead to a leak in that the handle may not be disposed correctly.");
		}
	}

	public static bool GetAllItems(Action<bool, SteamItemDetails_t[]> callback)
	{
		if (!RegisterCallbacks())
		{
			Debug.LogError("Attempted call to Get All Items before the Steam Foundation Manager Initialized");
			return false;
		}
		if (pendingCalls == null)
		{
			pendingCalls = new Dictionary<SteamInventoryResult_t, CallRequest>();
		}
		CallRequest value = new CallRequest
		{
			Type = CallRequestType.GetAllItems,
			DetailCallback = callback
		};
		if (SteamInventory.GetAllItems(out var pResultHandle))
		{
			if (pendingCalls.ContainsKey(pResultHandle))
			{
				Debug.LogError("Attempting to add a callback listener on an existing handle. This sugests a handle leak.");
				pendingCalls.Remove(pResultHandle);
			}
			pendingCalls.Add(pResultHandle, value);
			return true;
		}
		Debug.LogWarning("Failed to request Get All Items from the Steamworks Steam Inventory service. This call is not valid from a Steam Game Server");
		return false;
	}

	public static bool AddPromoItem(SteamItemDef_t itemDefinition, Action<bool, SteamItemDetails_t[]> callback)
	{
		if (!RegisterCallbacks())
		{
			Debug.LogError("Attempted call to Add Promo Item before the Steam Foundation Manager Initialized");
			return false;
		}
		if (pendingCalls == null)
		{
			pendingCalls = new Dictionary<SteamInventoryResult_t, CallRequest>();
		}
		CallRequest value = new CallRequest
		{
			Type = CallRequestType.AddPromoItem,
			DetailCallback = callback
		};
		if (SteamInventory.AddPromoItem(out var pResultHandle, itemDefinition))
		{
			if (pendingCalls.ContainsKey(pResultHandle))
			{
				Debug.LogError("Attempting to add a callback listener on an existing handle. This sugests a handle leak.");
				pendingCalls.Remove(pResultHandle);
			}
			pendingCalls.Add(pResultHandle, value);
			return true;
		}
		Debug.LogWarning("Failed to request Add Promo Item from the Steamworks Steam Inventory service. This call is not valid from a Steam Game Server");
		return false;
	}

	public static bool AddPromoItems(IEnumerable<SteamItemDef_t> itemDefinitions, Action<bool, SteamItemDetails_t[]> callback)
	{
		if (!RegisterCallbacks())
		{
			Debug.LogError("Attempted call to Add Promo Item before the Steam Foundation Manager Initialized");
			return false;
		}
		if (pendingCalls == null)
		{
			pendingCalls = new Dictionary<SteamInventoryResult_t, CallRequest>();
		}
		CallRequest value = new CallRequest
		{
			Type = CallRequestType.AddPromoItem,
			DetailCallback = callback
		};
		if (SteamInventory.AddPromoItems(out var pResultHandle, itemDefinitions.ToArray(), Convert.ToUInt32(itemDefinitions.Count())))
		{
			if (pendingCalls.ContainsKey(pResultHandle))
			{
				Debug.LogError("Attempting to add a callback listener on an existing handle. This sugests a handle leak.");
				pendingCalls.Remove(pResultHandle);
			}
			pendingCalls.Add(pResultHandle, value);
			return true;
		}
		Debug.LogWarning("Failed to request Add Promo Item from the Steamworks Steam Inventory service. This call is not valid from a Steam Game Server");
		return false;
	}

	public static bool CheckResultSteamID(SteamInventoryResult_t resultHandle, CSteamID steamIDExpected)
	{
		return SteamInventory.CheckResultSteamID(resultHandle, steamIDExpected);
	}

	public static bool CheckResultSteamID(SteamInventoryResult_t resultHandle, SteamUserData steamUserExpected)
	{
		return SteamInventory.CheckResultSteamID(resultHandle, steamUserExpected.id);
	}

	public static bool CheckResultSteamID(SteamInventoryResult_t resultHandle, ulong steamIDExpected)
	{
		return SteamInventory.CheckResultSteamID(resultHandle, new CSteamID(steamIDExpected));
	}

	public static bool ConsumeItem(SteamItemInstanceID_t instanceId, Action<bool, SteamItemDetails_t[]> callback)
	{
		if (!RegisterCallbacks())
		{
			Debug.LogError("Attempted call to Consume Item before the Steam Foundation Manager Initialized");
			return false;
		}
		if (pendingCalls == null)
		{
			pendingCalls = new Dictionary<SteamInventoryResult_t, CallRequest>();
		}
		CallRequest value = new CallRequest
		{
			Type = CallRequestType.ConsumeItem,
			DetailCallback = callback
		};
		if (!SteamInventory.ConsumeItem(out var pResultHandle, instanceId, 1u))
		{
			Debug.LogWarning("Failed to request Consume Item from the Steamworks Steam Inventory service. This call is not valid from a Steam Game Server");
			return false;
		}
		if (pendingCalls.ContainsKey(pResultHandle))
		{
			Debug.LogError("Attempting to add a callback listener on an existing handle. This sugests a handle leak.");
			pendingCalls.Remove(pResultHandle);
		}
		pendingCalls.Add(pResultHandle, value);
		return true;
	}

	public static bool ConsumeItem(SteamItemInstanceID_t instanceId, uint quantity, Action<bool, SteamItemDetails_t[]> callback)
	{
		if (!RegisterCallbacks())
		{
			Debug.LogError("Attempted call to Consume Item before the Steam Foundation Manager Initialized");
			return false;
		}
		if (pendingCalls == null)
		{
			pendingCalls = new Dictionary<SteamInventoryResult_t, CallRequest>();
		}
		CallRequest value = new CallRequest
		{
			Type = CallRequestType.ConsumeItem,
			DetailCallback = callback
		};
		if (!SteamInventory.ConsumeItem(out var pResultHandle, instanceId, quantity))
		{
			Debug.LogWarning("Failed to request Consume Item from the Steamworks Steam Inventory service. This call is not valid from a Steam Game Server");
			return false;
		}
		if (pendingCalls.ContainsKey(pResultHandle))
		{
			Debug.LogError("Attempting to add a callback listener on an existing handle. This sugests a handle leak.");
			pendingCalls.Remove(pResultHandle);
		}
		pendingCalls.Add(pResultHandle, value);
		return true;
	}

	public static bool DeserializeResult(byte[] buffer, CSteamID fromUser, Action<bool, SteamItemDetails_t[]> callback)
	{
		if (!RegisterCallbacks())
		{
			Debug.LogError("Attempted call to Deserialize Result before the Steam Foundation Manager Initialized");
			return false;
		}
		if (pendingCalls == null)
		{
			pendingCalls = new Dictionary<SteamInventoryResult_t, CallRequest>();
		}
		CallRequest value = new CallRequest
		{
			Type = CallRequestType.DeserializeResult,
			SteamUserId = fromUser,
			DetailCallback = callback
		};
		if (SteamInventory.DeserializeResult(out var pOutResultHandle, buffer, Convert.ToUInt32(buffer.Length)))
		{
			if (pendingCalls.ContainsKey(pOutResultHandle))
			{
				Debug.LogError("Attempting to add a callback listener on an existing handle. This sugests a handle leak.");
				pendingCalls.Remove(pOutResultHandle);
			}
			pendingCalls.Add(pOutResultHandle, value);
			return true;
		}
		Debug.LogWarning("Failed to request Deserialize Result from the Steamworks Steam Inventory service. This should never happen according to Steamworks documentaiton ... please contact Valve's partner support for more information.");
		return false;
	}

	public static bool DeserializeResult(byte[] buffer, SteamUserData fromUser, Action<bool, SteamItemDetails_t[]> callback)
	{
		return DeserializeResult(buffer, fromUser.id, callback);
	}

	public static bool DeserializeResult(byte[] buffer, ulong fromUser, Action<bool, SteamItemDetails_t[]> callback)
	{
		return DeserializeResult(buffer, new CSteamID(fromUser), callback);
	}

	public static bool ExchangeItems(ItemExchangeRecipe recipe, Action<bool, SteamItemDetails_t[]> callback)
	{
		if (!RegisterCallbacks())
		{
			Debug.LogError("Attempted call to Exchange Items before the Steam Foundation Manager Initialized");
			return false;
		}
		if (pendingCalls == null)
		{
			pendingCalls = new Dictionary<SteamInventoryResult_t, CallRequest>();
		}
		CallRequest value = new CallRequest
		{
			Type = CallRequestType.ExchangeItems,
			DetailCallback = callback
		};
		if (SteamInventory.ExchangeItems(out var pResultHandle, new SteamItemDef_t[1] { recipe.ItemToGenerate }, new uint[1] { 1u }, 1u, recipe.GetInstanceArray(), recipe.GetQuantityArray(), Convert.ToUInt32(recipe.ItemsToConsume.Count)))
		{
			if (pendingCalls.ContainsKey(pResultHandle))
			{
				Debug.LogError("Attempting to add a callback listener on an existing handle. This sugests a handle leak.");
				pendingCalls.Remove(pResultHandle);
			}
			pendingCalls.Add(pResultHandle, value);
			return true;
		}
		Debug.LogWarning("Failed to request Exchange Items from the Steamworks Steam Inventory service. This call is not valid from a Steam Game Server");
		return false;
	}

	public static bool ExchangeItems(SteamItemDef_t toGenerate, IEnumerable<ExchangeItemCount> toBeConsumed, Action<bool, SteamItemDetails_t[]> callback)
	{
		return ExchangeItems(new ItemExchangeRecipe(toGenerate, toBeConsumed), callback);
	}

	public static bool DeveloperOnlyGenerateItems(List<GenerateItemCount> ItemDefinitions, Action<bool, SteamItemDetails_t[]> callback)
	{
		if (!RegisterCallbacks())
		{
			Debug.LogError("Attempted call to Generate Items before the Steam Foundation Manager Initialized");
			return false;
		}
		if (pendingCalls == null)
		{
			pendingCalls = new Dictionary<SteamInventoryResult_t, CallRequest>();
		}
		CallRequest value = new CallRequest
		{
			Type = CallRequestType.GenerateItems,
			DetailCallback = callback
		};
		List<SteamItemDef_t> list = ItemDefinitions.ConvertAll((GenerateItemCount p) => p.ItemId);
		List<uint> list2 = ItemDefinitions.ConvertAll((GenerateItemCount p) => p.Quantity);
		if (SteamInventory.GenerateItems(out var pResultHandle, list.ToArray(), list2.ToArray(), Convert.ToUInt32(ItemDefinitions.Count)))
		{
			if (pendingCalls.ContainsKey(pResultHandle))
			{
				Debug.LogError("Attempting to add a callback listener on an existing handle. This sugests a handle leak.");
				pendingCalls.Remove(pResultHandle);
			}
			pendingCalls.Add(pResultHandle, value);
			return true;
		}
		Debug.LogWarning("Failed to request Generate Items from the Steamworks Steam Inventory service. This call is not valid from a Steam Game Server");
		return false;
	}

	public static bool GetItemsByID(IEnumerable<SteamItemInstanceID_t> InstanceIDs, Action<bool, SteamItemDetails_t[]> callback)
	{
		if (!RegisterCallbacks())
		{
			Debug.LogError("Attempted call to Get Items By ID before the Steam Foundation Manager Initialized");
			return false;
		}
		if (pendingCalls == null)
		{
			pendingCalls = new Dictionary<SteamInventoryResult_t, CallRequest>();
		}
		CallRequest value = new CallRequest
		{
			Type = CallRequestType.GetItemsByID,
			DetailCallback = callback
		};
		if (SteamInventory.GetItemsByID(out var pResultHandle, InstanceIDs.ToArray(), Convert.ToUInt32(InstanceIDs.Count())))
		{
			if (pendingCalls.ContainsKey(pResultHandle))
			{
				Debug.LogError("Attempting to add a callback listener on an existing handle. This sugests a handle leak.");
				pendingCalls.Remove(pResultHandle);
			}
			pendingCalls.Add(pResultHandle, value);
			return true;
		}
		Debug.LogWarning("Failed to request Get Items By ID from the Steamworks Steam Inventory service. This call is not valid from a Steam Game Server");
		return false;
	}

	public static bool GrantPromoItems(Action<bool, SteamItemDetails_t[]> callback)
	{
		if (!RegisterCallbacks())
		{
			Debug.LogError("Attempted call to Grant Promo Items before the Steam Foundation Manager Initialized");
			return false;
		}
		if (pendingCalls == null)
		{
			pendingCalls = new Dictionary<SteamInventoryResult_t, CallRequest>();
		}
		CallRequest value = new CallRequest
		{
			Type = CallRequestType.GrantPromoItems,
			DetailCallback = callback
		};
		if (SteamInventory.GrantPromoItems(out var pResultHandle))
		{
			if (pendingCalls.ContainsKey(pResultHandle))
			{
				Debug.LogError("Attempting to add a callback listener on an existing handle. This sugests a handle leak.");
				pendingCalls.Remove(pResultHandle);
			}
			pendingCalls.Add(pResultHandle, value);
			return true;
		}
		Debug.LogWarning("Failed to request Grant Promo Items from the Steamworks Steam Inventory service. This call is not valid from a Steam Game Server");
		return false;
	}

	public static bool RequestEligiblePromoItemDefinitionsIDs(CSteamID steamID, Action<bool, SteamItemDef_t[]> callback)
	{
		if (!RegisterCallbacks())
		{
			Debug.LogError("Attempted call to Grant Promo Items before the Steam Foundation Manager Initialized");
			return false;
		}
		if (pendingCalls == null)
		{
			pendingCalls = new Dictionary<SteamInventoryResult_t, CallRequest>();
		}
		try
		{
			SteamAPICall_t hAPICall = SteamInventory.RequestEligiblePromoItemDefinitionsIDs(steamID);
			m_SteamInventoryEligiblePromoItemDefIDs.Set(hAPICall, delegate(SteamInventoryEligiblePromoItemDefIDs_t param, bool bIOFailure)
			{
				SteamItemDef_t[] array = new SteamItemDef_t[param.m_numEligiblePromoItemDefs];
				uint punItemDefIDsArraySize = Convert.ToUInt32(param.m_numEligiblePromoItemDefs);
				if (SteamInventory.GetEligiblePromoItemDefinitionIDs(steamID, array, ref punItemDefIDsArraySize))
				{
					callback(arg1: true, array);
				}
				else
				{
					callback(arg1: false, new SteamItemDef_t[0]);
				}
			});
			return true;
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			return false;
		}
	}

	public static bool SerializeResults(IEnumerable<SteamItemInstanceID_t> InstanceIDs, Action<bool, byte[]> callback)
	{
		if (!RegisterCallbacks())
		{
			Debug.LogError("Attempted call to Get Items By ID before the Steam Foundation Manager Initialized");
			return false;
		}
		if (pendingCalls == null)
		{
			pendingCalls = new Dictionary<SteamInventoryResult_t, CallRequest>();
		}
		CallRequest value = new CallRequest
		{
			Type = CallRequestType.GetItemIDsToSerialize,
			SerializationCallback = callback
		};
		if (SteamInventory.GetItemsByID(out var pResultHandle, InstanceIDs.ToArray(), Convert.ToUInt32(InstanceIDs.Count())))
		{
			if (pendingCalls.ContainsKey(pResultHandle))
			{
				Debug.LogError("Attempting to add a callback listener on an existing handle. This sugests a handle leak.");
				pendingCalls.Remove(pResultHandle);
			}
			pendingCalls.Add(pResultHandle, value);
			return true;
		}
		Debug.LogWarning("Failed to request Get Items By ID from the Steamworks Steam Inventory service. This call is not valid from a Steam Game Server");
		return false;
	}

	public static bool TransferQuantity(SteamItemInstanceID_t sourceItem, uint quantityToMove, SteamItemInstanceID_t destinationItem, Action<bool> callback)
	{
		if (!RegisterCallbacks())
		{
			Debug.LogError("Attempted call to Stack Items before the Steam Foundation Manager Initialized");
			return false;
		}
		if (pendingCalls == null)
		{
			pendingCalls = new Dictionary<SteamInventoryResult_t, CallRequest>();
		}
		CallRequest value = new CallRequest
		{
			Type = CallRequestType.TransferItemQuantity,
			BoolCallback = callback
		};
		if (SteamInventory.TransferItemQuantity(out var pResultHandle, sourceItem, quantityToMove, destinationItem))
		{
			if (pendingCalls.ContainsKey(pResultHandle))
			{
				Debug.LogError("Attempting to add a callback listener on an existing handle. This sugests a handle leak.");
				pendingCalls.Remove(pResultHandle);
			}
			pendingCalls.Add(pResultHandle, value);
			return true;
		}
		Debug.LogWarning("Failed to request Stack Item (Transfer Item Quantity) from the Steamworks Steam Inventory service. This call is not valid from a Steam Game Server");
		return false;
	}

	public static bool SplitItems(SteamItemInstanceID_t sourceItem, uint quantityToMove, Action<bool> callback)
	{
		if (!RegisterCallbacks())
		{
			Debug.LogError("Attempted call to Stack Items before the Steam Foundation Manager Initialized");
			return false;
		}
		if (pendingCalls == null)
		{
			pendingCalls = new Dictionary<SteamInventoryResult_t, CallRequest>();
		}
		CallRequest value = new CallRequest
		{
			Type = CallRequestType.TransferItemQuantity,
			BoolCallback = callback
		};
		if (SteamInventory.TransferItemQuantity(out var pResultHandle, sourceItem, quantityToMove, SteamItemInstanceID_t.Invalid))
		{
			if (pendingCalls.ContainsKey(pResultHandle))
			{
				Debug.LogError("Attempting to add a callback listener on an existing handle. This sugests a handle leak.");
				pendingCalls.Remove(pResultHandle);
			}
			pendingCalls.Add(pResultHandle, value);
			return true;
		}
		Debug.LogWarning("Failed to request Stack Item (Transfer Item Quantity) from the Steamworks Steam Inventory service. This call is not valid from a Steam Game Server");
		return false;
	}

	public static bool TriggerItemDrop(SteamItemDef_t dropListDefinition, Action<bool, SteamItemDetails_t[]> callback)
	{
		if (!RegisterCallbacks())
		{
			Debug.LogError("Attempted call to Trigger Item Drop before the Steam Foundation Manager Initialized");
			return false;
		}
		if (pendingCalls == null)
		{
			pendingCalls = new Dictionary<SteamInventoryResult_t, CallRequest>();
		}
		CallRequest value = new CallRequest
		{
			Type = CallRequestType.TriggerItemDrop,
			DetailCallback = callback
		};
		if (SteamInventory.TriggerItemDrop(out var pResultHandle, dropListDefinition))
		{
			if (pendingCalls.ContainsKey(pResultHandle))
			{
				Debug.LogError("Attempting to add a callback listener on an existing handle. This sugests a handle leak.");
				pendingCalls.Remove(pResultHandle);
			}
			Debug.Log("Generated new handle: " + pResultHandle.ToString());
			pendingCalls.Add(pResultHandle, value);
			return true;
		}
		Debug.LogWarning("Failed to request Trigger Item Drop from the Steamworks Steam Inventory service. This call is not valid from a Steam Game Server");
		return false;
	}

	private static void HandleEligiblePromoItemDefIDs(SteamInventoryEligiblePromoItemDefIDs_t param, bool bIOFailure)
	{
		Debug.Log("Handle Eligible Promo Item Def IDs default deligate called!");
	}
}
