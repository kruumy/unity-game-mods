using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamApi.PlayerServices.Demo;

public class SteamworksInventoryDemonstrations : MonoBehaviour
{
	public void getAllTest()
	{
		if (SteamworksPlayerInventory.GetAllItems(delegate(bool status, SteamItemDetails_t[] results)
		{
			if (status)
			{
				Debug.Log("Query returned " + results.Length + " items.");
			}
			else
			{
				Debug.Log("Query failed.");
			}
		}))
		{
			Debug.Log("Get All Items request sent to Steam.");
		}
		else
		{
			Debug.Log("Get All Items failed to send to Steam.");
		}
	}

	public void grantPromoTest()
	{
		if (SteamworksPlayerInventory.GrantPromoItems(delegate(bool status, SteamItemDetails_t[] results)
		{
			if (status)
			{
				Debug.Log("Granted " + results.Length + " promo items.");
			}
			else
			{
				Debug.Log("Grant Promo Items Failed.");
			}
		}))
		{
			Debug.Log("Grant Promo Items request sent to Steam.");
		}
		else
		{
			Debug.Log("Grant Promo Items failed to send to Steam.");
		}
	}
}
