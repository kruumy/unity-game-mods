using System.Collections.Generic;
using HeathenEngineering.SteamApi.Foundation;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HeathenEngineering.SteamApi.GameServices;

public class HeathenWorkshopBrowser : MonoBehaviour
{
	public AppId_t CreatorAppId;

	public SteamSettings steamSettings;

	public GameObject WorkshopItemDisplayTemplate;

	public HeathenWorkshopItemQuery ActiveQuery;

	public Transform CollectionRoot;

	public bool GeneralSearchOnStart = true;

	public InputField searchBox;

	public Text currentCount;

	public Text totalCount;

	public Text currentPage;

	public UnityHeathenWorkshopItemQueryEvent QueryPrepared;

	public UnityEvent ResultsUpdated;

	private string lastSearchString = "";

	private void Awake()
	{
		SteamworksWorkshop.RegisterWorkshopSystem();
	}

	private void Start()
	{
		if (GeneralSearchOnStart)
		{
			SearchAll(string.Empty);
		}
	}

	private void Update()
	{
		if (ActiveQuery != null)
		{
			int num = (int)(ActiveQuery.Page * 50);
			if (num < ActiveQuery.matchedRecordCount)
			{
				currentCount.text = num - 49 + "-" + num;
			}
			else
			{
				int num2 = (int)(ActiveQuery.matchedRecordCount % 50u);
				num = num - 50 + num2;
				currentCount.text = num - (num - 1) + "-" + num;
			}
			totalCount.text = ActiveQuery.matchedRecordCount.ToString("N0");
			currentPage.text = ActiveQuery.Page.ToString();
		}
		else
		{
			currentCount.text = "0";
			totalCount.text = "0";
		}
	}

	private void ClearCollectionRoot()
	{
		List<GameObject> list = new List<GameObject>();
		foreach (Transform item in CollectionRoot)
		{
			list.Add(item.gameObject);
		}
		while (list.Count > 0)
		{
			GameObject obj = list[0];
			list.RemoveAt(0);
			Object.Destroy(obj);
		}
	}

	public void SearchAllFromInput()
	{
		SearchAll(searchBox.text);
	}

	public void SearchAll(string filter)
	{
		lastSearchString = filter;
		ActiveQuery = HeathenWorkshopItemQuery.Create(EUGCQuery.k_EUGCQuery_RankedByTrend, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items_ReadyToUse, CreatorAppId, steamSettings.applicationId);
		if (!string.IsNullOrEmpty(filter))
		{
			SteamworksWorkshop.WorkshopSetSearchText(ActiveQuery.handle, filter);
		}
		ActiveQuery.Execute(HandleResults);
	}

	public void PrepareSearchAll(string filter)
	{
		lastSearchString = filter;
		ActiveQuery = HeathenWorkshopItemQuery.Create(EUGCQuery.k_EUGCQuery_RankedByTrend, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items_ReadyToUse, CreatorAppId, steamSettings.applicationId);
		if (!string.IsNullOrEmpty(filter))
		{
			SteamworksWorkshop.WorkshopSetSearchText(ActiveQuery.handle, filter);
		}
		QueryPrepared.Invoke(ActiveQuery);
	}

	public void SearchFavoritesFromInput()
	{
		SearchFavorites(searchBox.text);
	}

	public void SearchFavorites(string filter)
	{
		lastSearchString = filter;
		ActiveQuery = HeathenWorkshopItemQuery.Create(SteamUser.GetSteamID().GetAccountID(), EUserUGCList.k_EUserUGCList_Favorited, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items_ReadyToUse, EUserUGCListSortOrder.k_EUserUGCListSortOrder_TitleAsc, CreatorAppId, steamSettings.applicationId);
		if (!string.IsNullOrEmpty(filter))
		{
			SteamworksWorkshop.WorkshopSetSearchText(ActiveQuery.handle, filter);
		}
		ActiveQuery.Execute(HandleResults);
	}

	public void PrepareSearchFavorites(string filter)
	{
		lastSearchString = filter;
		ActiveQuery = HeathenWorkshopItemQuery.Create(SteamUser.GetSteamID().GetAccountID(), EUserUGCList.k_EUserUGCList_Favorited, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items_ReadyToUse, EUserUGCListSortOrder.k_EUserUGCListSortOrder_TitleAsc, CreatorAppId, steamSettings.applicationId);
		if (!string.IsNullOrEmpty(filter))
		{
			SteamworksWorkshop.WorkshopSetSearchText(ActiveQuery.handle, filter);
		}
		QueryPrepared.Invoke(ActiveQuery);
	}

	public void SearchFollowedFromInput()
	{
		SearchFollowed(searchBox.text);
	}

	public void SearchFollowed(string filter)
	{
		lastSearchString = filter;
		ActiveQuery = HeathenWorkshopItemQuery.Create(SteamUser.GetSteamID().GetAccountID(), EUserUGCList.k_EUserUGCList_Followed, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items_ReadyToUse, EUserUGCListSortOrder.k_EUserUGCListSortOrder_TitleAsc, CreatorAppId, steamSettings.applicationId);
		if (!string.IsNullOrEmpty(filter))
		{
			SteamworksWorkshop.WorkshopSetSearchText(ActiveQuery.handle, filter);
		}
		ActiveQuery.Execute(HandleResults);
	}

	public void PrepareSearchFollowed(string filter)
	{
		lastSearchString = filter;
		ActiveQuery = HeathenWorkshopItemQuery.Create(SteamUser.GetSteamID().GetAccountID(), EUserUGCList.k_EUserUGCList_Followed, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items_ReadyToUse, EUserUGCListSortOrder.k_EUserUGCListSortOrder_TitleAsc, CreatorAppId, steamSettings.applicationId);
		if (!string.IsNullOrEmpty(filter))
		{
			SteamworksWorkshop.WorkshopSetSearchText(ActiveQuery.handle, filter);
		}
		QueryPrepared.Invoke(ActiveQuery);
	}

	public void ExeuctePreparedSearch()
	{
		if (ActiveQuery != null)
		{
			ActiveQuery.Execute(HandleResults);
		}
	}

	public void SetNextSearchPage()
	{
		if (ActiveQuery != null)
		{
			ActiveQuery.SetNextPage();
			if (!string.IsNullOrEmpty(lastSearchString))
			{
				SteamworksWorkshop.WorkshopSetSearchText(ActiveQuery.handle, lastSearchString);
			}
			ActiveQuery.Execute(HandleResults);
		}
	}

	public void SetPreviousSearchPage()
	{
		if (ActiveQuery != null)
		{
			ActiveQuery.SetPreviousPage();
			if (!string.IsNullOrEmpty(lastSearchString))
			{
				SteamworksWorkshop.WorkshopSetSearchText(ActiveQuery.handle, lastSearchString);
			}
			ActiveQuery.Execute(HandleResults);
		}
	}

	public void SetSearchPage(uint page)
	{
		if (ActiveQuery != null)
		{
			ActiveQuery.SetPage(page);
			if (!string.IsNullOrEmpty(lastSearchString))
			{
				SteamworksWorkshop.WorkshopSetSearchText(ActiveQuery.handle, lastSearchString);
			}
			ActiveQuery.Execute(HandleResults);
		}
	}

	private void HandleResults(HeathenWorkshopItemQuery query)
	{
		if (query == ActiveQuery)
		{
			ClearCollectionRoot();
			foreach (HeathenWorkshopReadCommunityItem results in query.ResultsList)
			{
				GameObject obj = Object.Instantiate(WorkshopItemDisplayTemplate, CollectionRoot);
				obj.GetComponent<Transform>().localPosition = Vector3.zero;
				obj.GetComponent<IWorkshopItemDisplay>().RegisterData(results);
			}
			ResultsUpdated.Invoke();
		}
		else
		{
			query.Dispose();
		}
	}
}
