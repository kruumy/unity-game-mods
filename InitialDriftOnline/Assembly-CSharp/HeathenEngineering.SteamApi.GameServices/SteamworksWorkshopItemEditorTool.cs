using System;
using System.Collections.Generic;
using System.IO;
using HeathenEngineering.SteamApi.Foundation;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamApi.GameServices;

[Serializable]
public class SteamworksWorkshopItemEditorTool
{
	public AppId_t TargetApp;

	public PublishedFileId_t FileId;

	public SteamUserData Author;

	public EWorkshopFileType FileType;

	public string Title;

	public string Description;

	public ERemoteStoragePublishedFileVisibility Visibility = ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate;

	public List<string> Tags = new List<string>();

	public string ContentLocation;

	public Texture2D previewImage;

	public string PreviewImageLocation;

	public UnityWorkshopItemCreatedEvent Created = new UnityWorkshopItemCreatedEvent();

	public UnityWorkshopItemCreatedEvent CreateFailed = new UnityWorkshopItemCreatedEvent();

	public UnityWorkshopSubmitItemUpdateResultEvent Updated = new UnityWorkshopSubmitItemUpdateResultEvent();

	public UnityWorkshopSubmitItemUpdateResultEvent UpdateFailed = new UnityWorkshopSubmitItemUpdateResultEvent();

	private CallResult<CreateItemResult_t> m_CreatedItem;

	private CallResult<SubmitItemUpdateResult_t> m_SubmitItemUpdateResult;

	private CallResult<RemoteStorageDownloadUGCResult_t> m_RemoteStorageDownloadUGCResult;

	private bool processingCreateAndUpdate;

	private string processingChangeNote = "";

	public UGCUpdateHandle_t updateHandle;

	public bool HasFileId => FileId != PublishedFileId_t.Invalid;

	public bool HasAppId => TargetApp != AppId_t.Invalid;

	public SteamworksWorkshopItemEditorTool(AppId_t targetApp)
	{
		TargetApp = targetApp;
		m_SubmitItemUpdateResult = CallResult<SubmitItemUpdateResult_t>.Create(HandleItemUpdated);
		m_CreatedItem = CallResult<CreateItemResult_t>.Create(HandleItemCreate);
		m_RemoteStorageDownloadUGCResult = CallResult<RemoteStorageDownloadUGCResult_t>.Create(HandleUGCDownload);
	}

	public SteamworksWorkshopItemEditorTool(SteamUGCDetails_t itemDetails)
	{
		m_SubmitItemUpdateResult = CallResult<SubmitItemUpdateResult_t>.Create(HandleItemUpdated);
		m_CreatedItem = CallResult<CreateItemResult_t>.Create(HandleItemCreate);
		m_RemoteStorageDownloadUGCResult = CallResult<RemoteStorageDownloadUGCResult_t>.Create(HandleUGCDownload);
		TargetApp = itemDetails.m_nConsumerAppID;
		FileId = itemDetails.m_nPublishedFileId;
		Title = itemDetails.m_rgchTitle;
		Description = itemDetails.m_rgchDescription;
		Visibility = itemDetails.m_eVisibility;
		Author = SteamSettings.current.client.GetUserData(itemDetails.m_ulSteamIDOwner);
		SteamAPICall_t hAPICall = SteamRemoteStorage.UGCDownload(itemDetails.m_hPreviewFile, 1u);
		m_RemoteStorageDownloadUGCResult.Set(hAPICall, HandleUGCDownload);
	}

	public SteamworksWorkshopItemEditorTool(HeathenWorkshopReadCommunityItem itemDetails)
	{
		m_SubmitItemUpdateResult = CallResult<SubmitItemUpdateResult_t>.Create(HandleItemUpdated);
		m_CreatedItem = CallResult<CreateItemResult_t>.Create(HandleItemCreate);
		m_RemoteStorageDownloadUGCResult = CallResult<RemoteStorageDownloadUGCResult_t>.Create(HandleUGCDownload);
		TargetApp = itemDetails.TargetApp;
		FileId = itemDetails.FileId;
		Title = itemDetails.Title;
		Description = itemDetails.Description;
		Visibility = itemDetails.Visibility;
		Author = SteamSettings.current.client.GetUserData(itemDetails.Author);
		previewImage = itemDetails.previewImage;
		PreviewImageLocation = itemDetails.PreviewImageLocation;
	}

	public bool CreateAndUpdate(string changeNote)
	{
		if (TargetApp == AppId_t.Invalid)
		{
			Debug.LogError("HeathenWorkshopItem|CreateAndUpdate ... Create operation aborted, the current AppId is invalid.");
			return false;
		}
		if (string.IsNullOrEmpty(Title))
		{
			Debug.LogError("HeathenWorkshopItem|CreateAndUpdate ... operation aborted, Title is null or empty and must have a value.");
			return false;
		}
		if (string.IsNullOrEmpty(ContentLocation))
		{
			Debug.LogError("HeathenWorkshopItem|CreateAndUpdate ... operation aborted, Content location is null or empty and must have a value.");
			return false;
		}
		if (string.IsNullOrEmpty(PreviewImageLocation))
		{
			Debug.LogError("HeathenWorkshopItem|CreateAndUpdate ... operation aborted, Preview image location is null or empty and must have a value.");
			return false;
		}
		processingChangeNote = changeNote;
		processingCreateAndUpdate = true;
		SteamAPICall_t hAPICall = SteamUGC.CreateItem(TargetApp, FileType);
		m_CreatedItem.Set(hAPICall, HandleItemCreate);
		return true;
	}

	public bool Update(string changeNote)
	{
		if (TargetApp == AppId_t.Invalid)
		{
			Debug.LogError("HeathenWorkshopItem|Update ... Update operation aborted, the current AppId is invalid.");
			return false;
		}
		if (FileId == PublishedFileId_t.Invalid)
		{
			Debug.LogError("HeathenWorkshopItem|Update ... Update operation aborted, the current FileId is invalid.");
			return false;
		}
		if (!Directory.Exists(ContentLocation))
		{
			Debug.LogError("HeathenWorkshopItem|Update ... Failed to update item content location, [" + ContentLocation + "] does not exist, this must be a valid folder path.");
			return false;
		}
		if (!File.Exists(PreviewImageLocation))
		{
			Debug.LogError("HeathenWorkshopItem|Update ... Failed to update item preview, [" + PreviewImageLocation + "] does not exist, this must be a valid file path.");
			return false;
		}
		updateHandle = SteamUGC.StartItemUpdate(TargetApp, FileId);
		if (!SteamUGC.SetItemTitle(updateHandle, Title))
		{
			Debug.LogError("HeathenWorkshopItem|Update ... Failed to update item title, item has not been updated.");
			return false;
		}
		if (!SteamUGC.SetItemDescription(updateHandle, Description))
		{
			Debug.LogError("HeathenWorkshopItem|Update ... Failed to update item description, item has not been updated.");
			return false;
		}
		if (!SteamUGC.SetItemVisibility(updateHandle, Visibility))
		{
			Debug.LogError("HeathenWorkshopItem|Update ... Failed to update item visibility, item has not been updated.");
			return false;
		}
		if (!SteamUGC.SetItemTags(updateHandle, Tags))
		{
			Debug.LogError("HeathenWorkshopItem|Update ... Failed to update item tags, item has not been updated.");
			return false;
		}
		if (!SteamUGC.SetItemContent(updateHandle, ContentLocation))
		{
			Debug.LogError("HeathenWorkshopItem|Update ... Failed to update item content location, item has not been updated.");
			return false;
		}
		if (!SteamUGC.SetItemPreview(updateHandle, PreviewImageLocation))
		{
			Debug.LogError("HeathenWorkshopItem|Update ... Failed to update item preview, item has not been updated.");
			return false;
		}
		SteamAPICall_t hAPICall = SteamUGC.SubmitItemUpdate(updateHandle, changeNote);
		m_SubmitItemUpdateResult.Set(hAPICall, HandleItemUpdated);
		return true;
	}

	public EItemUpdateStatus GetItemUpdateProgress(out ulong bytesProcessed, out ulong bytesTotal)
	{
		if (updateHandle != UGCUpdateHandle_t.Invalid)
		{
			return SteamUGC.GetItemUpdateProgress(updateHandle, out bytesProcessed, out bytesTotal);
		}
		bytesProcessed = 0uL;
		bytesTotal = 0uL;
		return EItemUpdateStatus.k_EItemUpdateStatusInvalid;
	}

	private void HandleItemUpdated(SubmitItemUpdateResult_t param, bool bIOFailure)
	{
		if (bIOFailure)
		{
			UpdateFailed.Invoke(param);
		}
		else
		{
			Updated.Invoke(param);
		}
	}

	private void HandleItemCreate(CreateItemResult_t param, bool bIOFailure)
	{
		if (bIOFailure)
		{
			CreateFailed.Invoke(param);
		}
		else
		{
			Author = SteamSettings.current.client.GetUserData(SteamUser.GetSteamID());
			FileId = param.m_nPublishedFileId;
			Created.Invoke(param);
		}
		if (processingCreateAndUpdate)
		{
			processingCreateAndUpdate = false;
			Update(processingChangeNote);
			processingChangeNote = string.Empty;
		}
	}

	private void HandleUGCDownload(RemoteStorageDownloadUGCResult_t param, bool bIOFailure)
	{
		PreviewImageLocation = param.m_pchFileName;
		if (SteamUtilities.LoadImageFromDisk(param.m_pchFileName, out var texture))
		{
			previewImage = texture;
		}
		else
		{
			Debug.LogError("Failed to load preview image (" + param.m_pchFileName + ") from disk!");
		}
	}
}
