using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamApi.GameServices;

public static class SteamworksWorkshop
{
	private static CallResult<AddAppDependencyResult_t> m_AddAppDependencyResults;

	private static CallResult<AddUGCDependencyResult_t> m_AddUGCDependencyResults;

	private static CallResult<UserFavoriteItemsListChanged_t> m_UserFavoriteItemsListChanged;

	private static CallResult<CreateItemResult_t> m_CreatedItem;

	private static CallResult<DeleteItemResult_t> m_DeleteItem;

	private static Callback<DownloadItemResult_t> m_DownloadItem;

	private static CallResult<GetAppDependenciesResult_t> m_AppDependenciesResult;

	private static CallResult<GetUserItemVoteResult_t> m_GetUserItemVoteResult;

	private static CallResult<RemoveAppDependencyResult_t> m_RemoveAppDependencyResult;

	private static CallResult<RemoveUGCDependencyResult_t> m_RemoveDependencyResult;

	private static CallResult<SteamUGCRequestUGCDetailsResult_t> m_SteamUGCRequestUGCDetailsResult;

	private static CallResult<SteamUGCQueryCompleted_t> m_SteamUGCQueryCompleted;

	private static CallResult<SetUserItemVoteResult_t> m_SetUserItemVoteResult;

	private static CallResult<StartPlaytimeTrackingResult_t> m_StartPlaytimeTrackingResult;

	private static CallResult<StopPlaytimeTrackingResult_t> m_StopPlaytimeTrackingResult;

	private static CallResult<SubmitItemUpdateResult_t> m_SubmitItemUpdateResult;

	private static CallResult<RemoteStorageSubscribePublishedFileResult_t> m_RemoteStorageSubscribePublishedFileResult;

	private static CallResult<RemoteStorageUnsubscribePublishedFileResult_t> m_RemoteStorageUnsubscribePublishedFileResult;

	[HideInInspector]
	public static UnityWorkshopDownloadedItemResultEvent OnWorkshopItemDownloaded;

	[HideInInspector]
	public static UnityWorkshopItemCreatedEvent OnWorkshopItemCreated;

	[HideInInspector]
	public static UnityWorkshopItemCreatedEvent OnWorkshopItemCreateFailed;

	[HideInInspector]
	public static UnityWorkshopItemDeletedEvent OnWorkshopItemDeleted;

	[HideInInspector]
	public static UnityWorkshopItemDeletedEvent OnWorkshopItemDeleteFailed;

	[HideInInspector]
	public static UnityWorkshopFavoriteItemsListChangedEvent OnWorkshopFavoriteItemsChanged;

	[HideInInspector]
	public static UnityWorkshopFavoriteItemsListChangedEvent OnWorkshopFavoriteItemsChangeFailed;

	[HideInInspector]
	public static UnityWorkshopAddAppDependencyResultEvent OnWorkshopAddedAppDependency;

	[HideInInspector]
	public static UnityWorkshopAddAppDependencyResultEvent OnWorkshopAddAppDependencyFailed;

	[HideInInspector]
	public static UnityWorkshopAddDependencyResultEvent OnWorkshopAddDependency;

	[HideInInspector]
	public static UnityWorkshopAddDependencyResultEvent OnWorkshopAddDependencyFailed;

	[HideInInspector]
	public static UnityWorkshopGetAppDependenciesResultEvent OnWorkshopAppDependenciesResults;

	[HideInInspector]
	public static UnityWorkshopGetAppDependenciesResultEvent OnWorkshopAppDependenciesResultsFailed;

	[HideInInspector]
	public static UnityWorkshopGetUserItemVoteResultEvent OnWorkshopUserItemVoteResults;

	[HideInInspector]
	public static UnityWorkshopGetUserItemVoteResultEvent OnWorkshopUserItemVoteResultsFailed;

	[HideInInspector]
	public static UnityWorkshopRemoveAppDependencyResultEvent OnWorkshopRemoveAppDependencyResults;

	[HideInInspector]
	public static UnityWorkshopRemoveAppDependencyResultEvent OnWorkshopRemoveAppDependencyResultsFailed;

	[HideInInspector]
	public static UnityWorkshopRemoveUGCDependencyResultEvent OnWorkshopRemoveDependencyResults;

	[HideInInspector]
	public static UnityWorkshopRemoveUGCDependencyResultEvent OnWorkshopRemoveDependencyResultsFailed;

	[HideInInspector]
	public static UnityWorkshopSteamUGCRequestUGCDetailsResultEvent OnWorkshopRequestDetailsResults;

	[HideInInspector]
	public static UnityWorkshopSteamUGCRequestUGCDetailsResultEvent OnWorkshopRequestDetailsResultsFailed;

	[HideInInspector]
	public static UnityWorkshopSteamUGCQueryCompletedEvent OnWorkshopQueryCompelted;

	[HideInInspector]
	public static UnityWorkshopSteamUGCQueryCompletedEvent OnWorkshopQueryCompeltedFailed;

	[HideInInspector]
	public static UnityWorkshopSetUserItemVoteResultEvent OnWorkshopSetUserItemVoteResult;

	[HideInInspector]
	public static UnityWorkshopSetUserItemVoteResultEvent OnWorkshopSetUserItemVoteResultFailed;

	[HideInInspector]
	public static UnityWorkshopStartPlaytimeTrackingResultEvent OnWorkshopStartPlaytimeTrackingResult;

	[HideInInspector]
	public static UnityWorkshopStartPlaytimeTrackingResultEvent OnWorkshopStartPlaytimeTrackingResultFailed;

	[HideInInspector]
	public static UnityWorkshopStopPlaytimeTrackingResultEvent OnWorkshopStopPlaytimeTrackingResult;

	[HideInInspector]
	public static UnityWorkshopStopPlaytimeTrackingResultEvent OnWorkshopStopPlaytimeTrackingResultFailed;

	[HideInInspector]
	public static UnityWorkshopSubmitItemUpdateResultEvent OnWorkshopSubmitItemUpdateResult;

	[HideInInspector]
	public static UnityWorkshopSubmitItemUpdateResultEvent OnWorkshopSubmitItemUpdateResultFailed;

	[HideInInspector]
	public static UnityWorkshopRemoteStorageSubscribePublishedFileResultEvent OnWorkshopRemoteStorageSubscribeFileResult;

	[HideInInspector]
	public static UnityWorkshopRemoteStorageSubscribePublishedFileResultEvent OnWorkshopRemoteStorageSubscribeFileResultFailed;

	[HideInInspector]
	public static UnityWorkshopRemoteStorageUnsubscribePublishedFileResultEvent OnWorkshopRemoteStorageUnsubscribeFileResult;

	[HideInInspector]
	public static UnityWorkshopRemoteStorageUnsubscribePublishedFileResultEvent OnWorkshopRemoteStorageUnsubscribeFileResultFailed;

	private static bool UGCReg;

	public static void RegisterWorkshopSystem()
	{
		if (!UGCReg)
		{
			UGCReg = true;
			m_AddAppDependencyResults = CallResult<AddAppDependencyResult_t>.Create(HandleAddAppDependencyResult);
			m_AddUGCDependencyResults = CallResult<AddUGCDependencyResult_t>.Create(HandleAddUGCDependencyResult);
			m_UserFavoriteItemsListChanged = CallResult<UserFavoriteItemsListChanged_t>.Create(HandleUserFavoriteItemsListChanged);
			m_CreatedItem = CallResult<CreateItemResult_t>.Create(HandleCreatedItem);
			m_DeleteItem = CallResult<DeleteItemResult_t>.Create(HandleDeleteItem);
			m_DownloadItem = Callback<DownloadItemResult_t>.Create(HandleDownloadedItem);
			m_AppDependenciesResult = CallResult<GetAppDependenciesResult_t>.Create(HandleGetAppDependenciesResults);
			m_GetUserItemVoteResult = CallResult<GetUserItemVoteResult_t>.Create(HandleGetUserItemVoteResult);
			m_RemoveAppDependencyResult = CallResult<RemoveAppDependencyResult_t>.Create(HandleRemoveAppDependencyResult);
			m_RemoveDependencyResult = CallResult<RemoveUGCDependencyResult_t>.Create(HandleRemoveDependencyResult);
			m_SteamUGCRequestUGCDetailsResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(HandleRequestDetailsResult);
			m_SteamUGCQueryCompleted = CallResult<SteamUGCQueryCompleted_t>.Create(HandleQueryCompleted);
			m_SetUserItemVoteResult = CallResult<SetUserItemVoteResult_t>.Create(HandleSetUserItemVoteResult);
			m_StartPlaytimeTrackingResult = CallResult<StartPlaytimeTrackingResult_t>.Create(HandleStartPlaytimeTracking);
			m_StopPlaytimeTrackingResult = CallResult<StopPlaytimeTrackingResult_t>.Create(HandleStopPlaytimeTracking);
			m_SubmitItemUpdateResult = CallResult<SubmitItemUpdateResult_t>.Create(HandleItemUpdateResult);
			m_RemoteStorageSubscribePublishedFileResult = CallResult<RemoteStorageSubscribePublishedFileResult_t>.Create(HandleSubscribeFileResult);
			m_RemoteStorageUnsubscribePublishedFileResult = CallResult<RemoteStorageUnsubscribePublishedFileResult_t>.Create(HandleUnsubscribeFileResult);
			OnWorkshopItemDownloaded = new UnityWorkshopDownloadedItemResultEvent();
			OnWorkshopItemCreated = new UnityWorkshopItemCreatedEvent();
			OnWorkshopItemCreateFailed = new UnityWorkshopItemCreatedEvent();
			OnWorkshopItemDeleted = new UnityWorkshopItemDeletedEvent();
			OnWorkshopItemDeleteFailed = new UnityWorkshopItemDeletedEvent();
			OnWorkshopFavoriteItemsChanged = new UnityWorkshopFavoriteItemsListChangedEvent();
			OnWorkshopFavoriteItemsChangeFailed = new UnityWorkshopFavoriteItemsListChangedEvent();
			OnWorkshopAddedAppDependency = new UnityWorkshopAddAppDependencyResultEvent();
			OnWorkshopAddAppDependencyFailed = new UnityWorkshopAddAppDependencyResultEvent();
			OnWorkshopAddDependency = new UnityWorkshopAddDependencyResultEvent();
			OnWorkshopAddDependencyFailed = new UnityWorkshopAddDependencyResultEvent();
			OnWorkshopAppDependenciesResults = new UnityWorkshopGetAppDependenciesResultEvent();
			OnWorkshopAppDependenciesResultsFailed = new UnityWorkshopGetAppDependenciesResultEvent();
			OnWorkshopUserItemVoteResults = new UnityWorkshopGetUserItemVoteResultEvent();
			OnWorkshopUserItemVoteResultsFailed = new UnityWorkshopGetUserItemVoteResultEvent();
			OnWorkshopRemoveAppDependencyResults = new UnityWorkshopRemoveAppDependencyResultEvent();
			OnWorkshopRemoveAppDependencyResultsFailed = new UnityWorkshopRemoveAppDependencyResultEvent();
			OnWorkshopRemoveDependencyResults = new UnityWorkshopRemoveUGCDependencyResultEvent();
			OnWorkshopRemoveDependencyResultsFailed = new UnityWorkshopRemoveUGCDependencyResultEvent();
			OnWorkshopRequestDetailsResults = new UnityWorkshopSteamUGCRequestUGCDetailsResultEvent();
			OnWorkshopRequestDetailsResultsFailed = new UnityWorkshopSteamUGCRequestUGCDetailsResultEvent();
			OnWorkshopQueryCompelted = new UnityWorkshopSteamUGCQueryCompletedEvent();
			OnWorkshopQueryCompeltedFailed = new UnityWorkshopSteamUGCQueryCompletedEvent();
			OnWorkshopSetUserItemVoteResult = new UnityWorkshopSetUserItemVoteResultEvent();
			OnWorkshopSetUserItemVoteResultFailed = new UnityWorkshopSetUserItemVoteResultEvent();
			OnWorkshopStartPlaytimeTrackingResult = new UnityWorkshopStartPlaytimeTrackingResultEvent();
			OnWorkshopStartPlaytimeTrackingResultFailed = new UnityWorkshopStartPlaytimeTrackingResultEvent();
			OnWorkshopStopPlaytimeTrackingResult = new UnityWorkshopStopPlaytimeTrackingResultEvent();
			OnWorkshopStopPlaytimeTrackingResultFailed = new UnityWorkshopStopPlaytimeTrackingResultEvent();
			OnWorkshopSubmitItemUpdateResult = new UnityWorkshopSubmitItemUpdateResultEvent();
			OnWorkshopSubmitItemUpdateResultFailed = new UnityWorkshopSubmitItemUpdateResultEvent();
			OnWorkshopRemoteStorageSubscribeFileResult = new UnityWorkshopRemoteStorageSubscribePublishedFileResultEvent();
			OnWorkshopRemoteStorageSubscribeFileResultFailed = new UnityWorkshopRemoteStorageSubscribePublishedFileResultEvent();
			OnWorkshopRemoteStorageUnsubscribeFileResult = new UnityWorkshopRemoteStorageUnsubscribePublishedFileResultEvent();
			OnWorkshopRemoteStorageUnsubscribeFileResultFailed = new UnityWorkshopRemoteStorageUnsubscribePublishedFileResultEvent();
		}
	}

	public static void WorkshopAddAppDependency(PublishedFileId_t fileId, AppId_t appId)
	{
		RegisterWorkshopSystem();
		SteamAPICall_t hAPICall = SteamUGC.AddAppDependency(fileId, appId);
		m_AddAppDependencyResults.Set(hAPICall, HandleAddAppDependencyResult);
	}

	public static void WorkshopAddDependency(PublishedFileId_t parentFileId, PublishedFileId_t childFileId)
	{
		RegisterWorkshopSystem();
		SteamAPICall_t hAPICall = SteamUGC.AddDependency(parentFileId, childFileId);
		m_AddUGCDependencyResults.Set(hAPICall, HandleAddUGCDependencyResult);
	}

	public static bool WorkshopAddExcludedTag(UGCQueryHandle_t handle, string tagName)
	{
		RegisterWorkshopSystem();
		return SteamUGC.AddExcludedTag(handle, tagName);
	}

	public static bool WorkshopAddItemKeyValueTag(UGCUpdateHandle_t handle, string key, string value)
	{
		RegisterWorkshopSystem();
		return SteamUGC.AddItemKeyValueTag(handle, key, value);
	}

	public static bool WorkshopAddItemPreviewFile(UGCUpdateHandle_t handle, string previewFile, EItemPreviewType type)
	{
		RegisterWorkshopSystem();
		return SteamUGC.AddItemPreviewFile(handle, previewFile, type);
	}

	public static bool WorkshopAddItemPreviewVideo(UGCUpdateHandle_t handle, string videoId)
	{
		RegisterWorkshopSystem();
		return SteamUGC.AddItemPreviewVideo(handle, videoId);
	}

	public static void WorkshopAddItemToFavorites(AppId_t appId, PublishedFileId_t fileId)
	{
		RegisterWorkshopSystem();
		SteamAPICall_t hAPICall = SteamUGC.AddItemToFavorites(appId, fileId);
		m_UserFavoriteItemsListChanged.Set(hAPICall, HandleUserFavoriteItemsListChanged);
	}

	public static bool WorkshopAddRequiredKeyValueTag(UGCQueryHandle_t handle, string key, string value)
	{
		RegisterWorkshopSystem();
		return SteamUGC.AddRequiredKeyValueTag(handle, key, value);
	}

	public static bool WorkshopAddRequiredTag(UGCQueryHandle_t handle, string tagName)
	{
		RegisterWorkshopSystem();
		return SteamUGC.AddRequiredTag(handle, tagName);
	}

	public static void WorkshopCreateItem(AppId_t appId, EWorkshopFileType type)
	{
		RegisterWorkshopSystem();
		SteamAPICall_t hAPICall = SteamUGC.CreateItem(appId, type);
		m_CreatedItem.Set(hAPICall, HandleCreatedItem);
	}

	public static UGCQueryHandle_t WorkshopCreateQueryAllRequest(EUGCQuery queryType, EUGCMatchingUGCType matchingFileType, AppId_t creatorAppId, AppId_t consumerAppId, uint page)
	{
		RegisterWorkshopSystem();
		return SteamUGC.CreateQueryAllUGCRequest(queryType, matchingFileType, creatorAppId, consumerAppId, page);
	}

	public static UGCQueryHandle_t WorkshopCreateQueryDetailsRequest(PublishedFileId_t[] fileIds)
	{
		RegisterWorkshopSystem();
		return SteamUGC.CreateQueryUGCDetailsRequest(fileIds, (uint)fileIds.GetLength(0));
	}

	public static UGCQueryHandle_t WorkshopCreateQueryDetailsRequest(List<PublishedFileId_t> fileIds)
	{
		RegisterWorkshopSystem();
		return SteamUGC.CreateQueryUGCDetailsRequest(fileIds.ToArray(), (uint)fileIds.Count);
	}

	public static UGCQueryHandle_t WorkshopCreateQueryDetailsRequest(IEnumerable<PublishedFileId_t> fileIds)
	{
		RegisterWorkshopSystem();
		List<PublishedFileId_t> list = new List<PublishedFileId_t>(fileIds);
		return SteamUGC.CreateQueryUGCDetailsRequest(list.ToArray(), (uint)list.Count);
	}

	public static UGCQueryHandle_t WorkshopCreateQueryUserRequest(AccountID_t accountId, EUserUGCList listType, EUGCMatchingUGCType matchingType, EUserUGCListSortOrder sortOrder, AppId_t creatorAppId, AppId_t consumerAppId, uint page)
	{
		RegisterWorkshopSystem();
		return SteamUGC.CreateQueryUserUGCRequest(accountId, listType, matchingType, sortOrder, creatorAppId, consumerAppId, page);
	}

	public static bool WorkshopReleaseQueryRequest(UGCQueryHandle_t handle)
	{
		RegisterWorkshopSystem();
		return SteamUGC.ReleaseQueryUGCRequest(handle);
	}

	public static void WorkshopDeleteItem(PublishedFileId_t fileId)
	{
		RegisterWorkshopSystem();
		SteamAPICall_t hAPICall = SteamUGC.DeleteItem(fileId);
		m_DeleteItem.Set(hAPICall, HandleDeleteItem);
	}

	public static bool WorkshopDownloadItem(PublishedFileId_t fileId, bool setHighPriority)
	{
		RegisterWorkshopSystem();
		return SteamUGC.DownloadItem(fileId, setHighPriority);
	}

	public static void WorkshopGetAppDependencies(PublishedFileId_t fileId)
	{
		RegisterWorkshopSystem();
		SteamAPICall_t appDependencies = SteamUGC.GetAppDependencies(fileId);
		m_AppDependenciesResult.Set(appDependencies, HandleGetAppDependenciesResults);
	}

	public static bool WorkshopGetItemDownloadInfo(PublishedFileId_t fileId, out float completion)
	{
		RegisterWorkshopSystem();
		ulong punBytesDownloaded;
		ulong punBytesTotal;
		bool itemDownloadInfo = SteamUGC.GetItemDownloadInfo(fileId, out punBytesDownloaded, out punBytesTotal);
		if (itemDownloadInfo)
		{
			completion = Convert.ToSingle((double)punBytesDownloaded / (double)punBytesTotal);
			return itemDownloadInfo;
		}
		completion = 0f;
		return itemDownloadInfo;
	}

	public static bool WorkshopGetItemInstallInfo(PublishedFileId_t fileId, out ulong sizeOnDisk, out string folderPath, out DateTime timeStamp)
	{
		RegisterWorkshopSystem();
		uint punTimeStamp;
		bool itemInstallInfo = SteamUGC.GetItemInstallInfo(fileId, out sizeOnDisk, out folderPath, 1024u, out punTimeStamp);
		timeStamp = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		timeStamp = timeStamp.AddSeconds(punTimeStamp);
		return itemInstallInfo;
	}

	public static bool WorkshopGetItemInstallInfo(PublishedFileId_t fileId, out ulong sizeOnDisk, out string folderPath, uint folderSize, out DateTime timeStamp)
	{
		RegisterWorkshopSystem();
		uint punTimeStamp;
		bool itemInstallInfo = SteamUGC.GetItemInstallInfo(fileId, out sizeOnDisk, out folderPath, folderSize, out punTimeStamp);
		timeStamp = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		timeStamp = timeStamp.AddSeconds(punTimeStamp);
		return itemInstallInfo;
	}

	public static EItemState WorkshopGetItemState(PublishedFileId_t fileId)
	{
		RegisterWorkshopSystem();
		return (EItemState)SteamUGC.GetItemState(fileId);
	}

	public static bool WorkshopItemStateHasFlag(EItemState value, EItemState checkflag)
	{
		RegisterWorkshopSystem();
		return (value & checkflag) == checkflag;
	}

	public static bool WorkshopItemStateHasAllFlags(EItemState value, params EItemState[] checkflags)
	{
		RegisterWorkshopSystem();
		foreach (EItemState eItemState in checkflags)
		{
			if ((value & eItemState) != eItemState)
			{
				return false;
			}
		}
		return true;
	}

	public static EItemUpdateStatus WorkshopGetItemUpdateProgress(UGCUpdateHandle_t handle, out float completion)
	{
		RegisterWorkshopSystem();
		ulong punBytesProcessed;
		ulong punBytesTotal;
		EItemUpdateStatus itemUpdateProgress = SteamUGC.GetItemUpdateProgress(handle, out punBytesProcessed, out punBytesTotal);
		if (itemUpdateProgress != 0)
		{
			completion = Convert.ToSingle((double)punBytesProcessed / (double)punBytesTotal);
			return itemUpdateProgress;
		}
		completion = 0f;
		return itemUpdateProgress;
	}

	public static uint WorkshopGetNumSubscribedItems()
	{
		RegisterWorkshopSystem();
		return SteamUGC.GetNumSubscribedItems();
	}

	public static bool WorkshopGetQueryUGCAdditionalPreview(UGCQueryHandle_t handle, uint index, uint previewIndex, out string urlOrVideoId, uint urlOrVideoSize, string fileName, uint fileNameSize, out EItemPreviewType type)
	{
		RegisterWorkshopSystem();
		return SteamUGC.GetQueryUGCAdditionalPreview(handle, index, previewIndex, out urlOrVideoId, urlOrVideoSize, out fileName, fileNameSize, out type);
	}

	public static bool WorkshopGetQueryUGCChildren(UGCQueryHandle_t handle, uint index, PublishedFileId_t[] fileIds, uint maxEntries)
	{
		RegisterWorkshopSystem();
		return SteamUGC.GetQueryUGCChildren(handle, index, fileIds, maxEntries);
	}

	public static bool WorkshopGeQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, out string key, string value)
	{
		RegisterWorkshopSystem();
		bool queryUGCKeyValueTag = SteamUGC.GetQueryUGCKeyValueTag(handle, index, keyValueTagIndex, out key, 2048u, out value, 2048u);
		key = key.Trim();
		value = value.Trim();
		return queryUGCKeyValueTag;
	}

	public static bool WorkshopGeQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, out string key, uint keySize, string value, uint valueSize)
	{
		RegisterWorkshopSystem();
		return SteamUGC.GetQueryUGCKeyValueTag(handle, index, keyValueTagIndex, out key, keySize, out value, valueSize);
	}

	public static bool WorkshopGetQueryUGCMetadata(UGCQueryHandle_t handle, uint index, out string metadata, uint size)
	{
		RegisterWorkshopSystem();
		return SteamUGC.GetQueryUGCMetadata(handle, index, out metadata, size);
	}

	public static uint WorkshopGetQueryUGCNumAdditionalPreviews(UGCQueryHandle_t handle, uint index)
	{
		RegisterWorkshopSystem();
		return SteamUGC.GetQueryUGCNumAdditionalPreviews(handle, index);
	}

	public static uint WorkshopGetQueryUGCNumKeyValueTags(UGCQueryHandle_t handle, uint index)
	{
		RegisterWorkshopSystem();
		return SteamUGC.GetQueryUGCNumKeyValueTags(handle, index);
	}

	public static bool WorkshopGetQueryUGCPreviewURL(UGCQueryHandle_t handle, uint index, out string URL, uint urlSize)
	{
		RegisterWorkshopSystem();
		return SteamUGC.GetQueryUGCPreviewURL(handle, index, out URL, urlSize);
	}

	public static bool WorkshopGetQueryUGCResult(UGCQueryHandle_t handle, uint index, out SteamUGCDetails_t details)
	{
		RegisterWorkshopSystem();
		return SteamUGC.GetQueryUGCResult(handle, index, out details);
	}

	public static bool WorkshopGetQueryUGCStatistic(UGCQueryHandle_t handle, uint index, EItemStatistic statType, out ulong statValue)
	{
		RegisterWorkshopSystem();
		return SteamUGC.GetQueryUGCStatistic(handle, index, statType, out statValue);
	}

	public static uint WorkshopGetSubscribedItems(PublishedFileId_t[] fileIDs, uint maxEntries)
	{
		RegisterWorkshopSystem();
		return SteamUGC.GetSubscribedItems(fileIDs, maxEntries);
	}

	public static List<PublishedFileId_t> GetSubscribedItems()
	{
		uint num = WorkshopGetNumSubscribedItems();
		PublishedFileId_t[] array = new PublishedFileId_t[num];
		if (WorkshopGetSubscribedItems(array, num) != 0)
		{
			return new List<PublishedFileId_t>(array);
		}
		return null;
	}

	public static void WorkshopGetUserItemVote(PublishedFileId_t fileId)
	{
		RegisterWorkshopSystem();
		SteamAPICall_t userItemVote = SteamUGC.GetUserItemVote(fileId);
		m_GetUserItemVoteResult.Set(userItemVote, HandleGetUserItemVoteResult);
	}

	public static void WorkshopRemoveAppDependency(PublishedFileId_t fileId, AppId_t appId)
	{
		RegisterWorkshopSystem();
		SteamAPICall_t hAPICall = SteamUGC.RemoveAppDependency(fileId, appId);
		m_RemoveAppDependencyResult.Set(hAPICall, HandleRemoveAppDependencyResult);
	}

	public static void WorkshopRemoveDependency(PublishedFileId_t parentFileId, PublishedFileId_t childFileId)
	{
		RegisterWorkshopSystem();
		SteamAPICall_t hAPICall = SteamUGC.RemoveDependency(parentFileId, childFileId);
		m_RemoveDependencyResult.Set(hAPICall, HandleRemoveDependencyResult);
	}

	public static void WorkshopRemoveItemFromFavorites(AppId_t appId, PublishedFileId_t fileId)
	{
		RegisterWorkshopSystem();
		SteamAPICall_t hAPICall = SteamUGC.RemoveItemFromFavorites(appId, fileId);
		m_UserFavoriteItemsListChanged.Set(hAPICall, HandleUserFavoriteItemsListChanged);
	}

	public static bool WorkshopRemoveItemKeyValueTags(UGCUpdateHandle_t handle, string key)
	{
		RegisterWorkshopSystem();
		return SteamUGC.RemoveItemKeyValueTags(handle, key);
	}

	public static bool WorkshopRemoveItemPreview(UGCUpdateHandle_t handle, uint index)
	{
		RegisterWorkshopSystem();
		return SteamUGC.RemoveItemPreview(handle, index);
	}

	public static void WorkshopRequestDetails(PublishedFileId_t fileId, uint maxAgeSeconds)
	{
		RegisterWorkshopSystem();
		SteamAPICall_t hAPICall = SteamUGC.RequestUGCDetails(fileId, maxAgeSeconds);
		m_SteamUGCRequestUGCDetailsResult.Set(hAPICall, HandleRequestDetailsResult);
	}

	public static void WorkshopSendQueryUGCRequest(UGCQueryHandle_t handle)
	{
		RegisterWorkshopSystem();
		SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(handle);
		m_SteamUGCQueryCompleted.Set(hAPICall, HandleQueryCompleted);
	}

	public static bool WorkshopSetAllowCachedResponse(UGCQueryHandle_t handle, uint maxAgeSeconds)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetAllowCachedResponse(handle, maxAgeSeconds);
	}

	public static bool WorkshopSetCloudFileNameFilter(UGCQueryHandle_t handle, string fileName)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetCloudFileNameFilter(handle, fileName);
	}

	public static bool WorkshopSetItemContent(UGCUpdateHandle_t handle, string folder)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetItemContent(handle, folder);
	}

	public static bool WorkshopSetItemDescription(UGCUpdateHandle_t handle, string description)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetItemDescription(handle, description);
	}

	public static bool WorkshopSetItemMetadata(UGCUpdateHandle_t handle, string metadata)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetItemMetadata(handle, metadata);
	}

	public static bool WorkshopSetItemPreview(UGCUpdateHandle_t handle, string previewFile)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetItemPreview(handle, previewFile);
	}

	public static bool WorkshopSetItemTags(UGCUpdateHandle_t handle, List<string> tags)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetItemTags(handle, tags);
	}

	public static bool WorkshopSetItemTitle(UGCUpdateHandle_t handle, string title)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetItemTitle(handle, title);
	}

	public static bool WorkshopSetItemUpdateLanguage(UGCUpdateHandle_t handle, string language)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetItemUpdateLanguage(handle, language);
	}

	public static bool WorkshopSetItemVisibility(UGCUpdateHandle_t handle, ERemoteStoragePublishedFileVisibility visibility)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetItemVisibility(handle, visibility);
	}

	public static bool WorkshopSetLanguage(UGCQueryHandle_t handle, string language)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetLanguage(handle, language);
	}

	public static bool WorkshopSetMatchAnyTag(UGCQueryHandle_t handle, bool anyTag)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetMatchAnyTag(handle, anyTag);
	}

	public static bool WorkshopSetRankedByTrendDays(UGCQueryHandle_t handle, uint days)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetRankedByTrendDays(handle, days);
	}

	public static bool WorkshopSetReturnAdditionalPreviews(UGCQueryHandle_t handle, bool additionalPreviews)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetReturnAdditionalPreviews(handle, additionalPreviews);
	}

	public static bool WorkshopSetReturnChildren(UGCQueryHandle_t handle, bool returnChildren)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetReturnChildren(handle, returnChildren);
	}

	public static bool WorkshopSetReturnKeyValueTags(UGCQueryHandle_t handle, bool tags)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetReturnKeyValueTags(handle, tags);
	}

	public static bool WorkshopSetReturnLongDescription(UGCQueryHandle_t handle, bool longDescription)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetReturnLongDescription(handle, longDescription);
	}

	public static bool WorkshopSetReturnMetadata(UGCQueryHandle_t handle, bool metadata)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetReturnMetadata(handle, metadata);
	}

	public static bool WorkshopSetReturnOnlyIDs(UGCQueryHandle_t handle, bool onlyIds)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetReturnOnlyIDs(handle, onlyIds);
	}

	public static bool WorkshopSetReturnPlaytimeStats(UGCQueryHandle_t handle, uint days)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetReturnPlaytimeStats(handle, days);
	}

	public static bool WorkshopSetReturnTotalOnly(UGCQueryHandle_t handle, bool totalOnly)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetReturnTotalOnly(handle, totalOnly);
	}

	public static bool WorkshopSetSearchText(UGCQueryHandle_t handle, string text)
	{
		RegisterWorkshopSystem();
		return SteamUGC.SetSearchText(handle, text);
	}

	public static void WorkshopSetUserItemVote(PublishedFileId_t fileID, bool voteUp)
	{
		RegisterWorkshopSystem();
		SteamAPICall_t hAPICall = SteamUGC.SetUserItemVote(fileID, voteUp);
		m_SetUserItemVoteResult.Set(hAPICall, HandleSetUserItemVoteResult);
	}

	public static UGCUpdateHandle_t WorkshopStartItemUpdate(AppId_t appId, PublishedFileId_t fileID)
	{
		RegisterWorkshopSystem();
		return SteamUGC.StartItemUpdate(appId, fileID);
	}

	public static void WorkshopStartPlaytimeTracking(PublishedFileId_t[] fileIds, uint count)
	{
		RegisterWorkshopSystem();
		SteamAPICall_t hAPICall = SteamUGC.StartPlaytimeTracking(fileIds, count);
		m_StartPlaytimeTrackingResult.Set(hAPICall, HandleStartPlaytimeTracking);
	}

	public static void WorkshopStopPlaytimeTracking(PublishedFileId_t[] fileIds, uint count)
	{
		RegisterWorkshopSystem();
		SteamAPICall_t hAPICall = SteamUGC.StopPlaytimeTracking(fileIds, count);
		m_StopPlaytimeTrackingResult.Set(hAPICall, HandleStopPlaytimeTracking);
	}

	public static void WorkshopStopPlaytimeTrackingForAllItems()
	{
		RegisterWorkshopSystem();
		SteamAPICall_t hAPICall = SteamUGC.StopPlaytimeTrackingForAllItems();
		m_StopPlaytimeTrackingResult.Set(hAPICall, HandleStopPlaytimeTracking);
	}

	public static void WorkshopSubmitItemUpdate(UGCUpdateHandle_t handle, string changeNote)
	{
		RegisterWorkshopSystem();
		SteamAPICall_t hAPICall = SteamUGC.SubmitItemUpdate(handle, changeNote);
		m_SubmitItemUpdateResult.Set(hAPICall, HandleItemUpdateResult);
	}

	public static void WorkshopSubscribeItem(PublishedFileId_t fileId)
	{
		RegisterWorkshopSystem();
		SteamAPICall_t hAPICall = SteamUGC.SubscribeItem(fileId);
		m_RemoteStorageSubscribePublishedFileResult.Set(hAPICall, HandleSubscribeFileResult);
	}

	public static void WorkshopSuspendDownloads(bool suspend)
	{
		RegisterWorkshopSystem();
		SteamUGC.SuspendDownloads(suspend);
	}

	public static void WorkshopUnsubscribeItem(PublishedFileId_t fileId)
	{
		RegisterWorkshopSystem();
		SteamAPICall_t hAPICall = SteamUGC.UnsubscribeItem(fileId);
		m_RemoteStorageUnsubscribePublishedFileResult.Set(hAPICall, HandleUnsubscribeFileResult);
	}

	public static bool WorkshopUpdateItemPreviewFile(UGCUpdateHandle_t handle, uint index, string file)
	{
		RegisterWorkshopSystem();
		return SteamUGC.UpdateItemPreviewFile(handle, index, file);
	}

	public static bool WorkshopUpdateItemPreviewVideo(UGCUpdateHandle_t handle, uint index, string videoId)
	{
		RegisterWorkshopSystem();
		return SteamUGC.UpdateItemPreviewVideo(handle, index, videoId);
	}

	private static void HandleAddUGCDependencyResult(AddUGCDependencyResult_t param, bool bIOFailure)
	{
		if (!bIOFailure)
		{
			OnWorkshopAddDependency.Invoke(param);
		}
		else
		{
			OnWorkshopAddDependencyFailed.Invoke(param);
		}
	}

	private static void HandleAddAppDependencyResult(AddAppDependencyResult_t param, bool bIOFailure)
	{
		if (!bIOFailure)
		{
			OnWorkshopAddedAppDependency.Invoke(param);
		}
		else
		{
			OnWorkshopAddAppDependencyFailed.Invoke(param);
		}
	}

	private static void HandleUserFavoriteItemsListChanged(UserFavoriteItemsListChanged_t param, bool bIOFailure)
	{
		if (!bIOFailure)
		{
			OnWorkshopFavoriteItemsChanged.Invoke(param);
		}
		else
		{
			OnWorkshopFavoriteItemsChangeFailed.Invoke(param);
		}
	}

	private static void HandleCreatedItem(CreateItemResult_t param, bool bIOFailure)
	{
		if (!bIOFailure)
		{
			OnWorkshopItemCreated.Invoke(param);
		}
		else
		{
			OnWorkshopItemCreateFailed.Invoke(param);
		}
	}

	private static void HandleDeleteItem(DeleteItemResult_t param, bool bIOFailure)
	{
		if (!bIOFailure)
		{
			OnWorkshopItemDeleted.Invoke(param);
		}
		else
		{
			OnWorkshopItemDeleteFailed.Invoke(param);
		}
	}

	private static void HandleDownloadedItem(DownloadItemResult_t param)
	{
		OnWorkshopItemDownloaded.Invoke(param);
	}

	private static void HandleGetAppDependenciesResults(GetAppDependenciesResult_t param, bool bIOFailure)
	{
		if (!bIOFailure)
		{
			OnWorkshopAppDependenciesResults.Invoke(param);
		}
		else
		{
			OnWorkshopAppDependenciesResultsFailed.Invoke(param);
		}
	}

	private static void HandleGetUserItemVoteResult(GetUserItemVoteResult_t param, bool bIOFailure)
	{
		if (!bIOFailure)
		{
			OnWorkshopUserItemVoteResults.Invoke(param);
		}
		else
		{
			OnWorkshopUserItemVoteResultsFailed.Invoke(param);
		}
	}

	private static void HandleRemoveAppDependencyResult(RemoveAppDependencyResult_t param, bool bIOFailure)
	{
		if (!bIOFailure)
		{
			OnWorkshopRemoveAppDependencyResults.Invoke(param);
		}
		else
		{
			OnWorkshopRemoveAppDependencyResultsFailed.Invoke(param);
		}
	}

	private static void HandleRemoveDependencyResult(RemoveUGCDependencyResult_t param, bool bIOFailure)
	{
		if (!bIOFailure)
		{
			OnWorkshopRemoveDependencyResults.Invoke(param);
		}
		else
		{
			OnWorkshopRemoveDependencyResultsFailed.Invoke(param);
		}
	}

	private static void HandleRequestDetailsResult(SteamUGCRequestUGCDetailsResult_t param, bool bIOFailure)
	{
		if (!bIOFailure)
		{
			OnWorkshopRequestDetailsResults.Invoke(param);
		}
		else
		{
			OnWorkshopRequestDetailsResultsFailed.Invoke(param);
		}
	}

	private static void HandleQueryCompleted(SteamUGCQueryCompleted_t param, bool bIOFailure)
	{
		if (!bIOFailure)
		{
			OnWorkshopQueryCompelted.Invoke(param);
		}
		else
		{
			OnWorkshopQueryCompeltedFailed.Invoke(param);
		}
	}

	private static void HandleSetUserItemVoteResult(SetUserItemVoteResult_t param, bool bIOFailure)
	{
		if (!bIOFailure)
		{
			OnWorkshopSetUserItemVoteResult.Invoke(param);
		}
		else
		{
			OnWorkshopSetUserItemVoteResultFailed.Invoke(param);
		}
	}

	private static void HandleStartPlaytimeTracking(StartPlaytimeTrackingResult_t param, bool bIOFailure)
	{
		if (!bIOFailure)
		{
			OnWorkshopStartPlaytimeTrackingResult.Invoke(param);
		}
		else
		{
			OnWorkshopStartPlaytimeTrackingResultFailed.Invoke(param);
		}
	}

	private static void HandleStopPlaytimeTracking(StopPlaytimeTrackingResult_t param, bool bIOFailure)
	{
		if (!bIOFailure)
		{
			OnWorkshopStopPlaytimeTrackingResult.Invoke(param);
		}
		else
		{
			OnWorkshopStopPlaytimeTrackingResultFailed.Invoke(param);
		}
	}

	private static void HandleUnsubscribeFileResult(RemoteStorageUnsubscribePublishedFileResult_t param, bool bIOFailure)
	{
		if (!bIOFailure)
		{
			OnWorkshopRemoteStorageUnsubscribeFileResult.Invoke(param);
		}
		else
		{
			OnWorkshopRemoteStorageUnsubscribeFileResultFailed.Invoke(param);
		}
	}

	private static void HandleSubscribeFileResult(RemoteStorageSubscribePublishedFileResult_t param, bool bIOFailure)
	{
		if (!bIOFailure)
		{
			OnWorkshopRemoteStorageSubscribeFileResult.Invoke(param);
		}
		else
		{
			OnWorkshopRemoteStorageSubscribeFileResultFailed.Invoke(param);
		}
	}

	private static void HandleItemUpdateResult(SubmitItemUpdateResult_t param, bool bIOFailure)
	{
		if (!bIOFailure)
		{
			OnWorkshopSubmitItemUpdateResult.Invoke(param);
		}
		else
		{
			OnWorkshopSubmitItemUpdateResultFailed.Invoke(param);
		}
	}
}
