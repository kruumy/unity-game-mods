using System;
using System.Collections.Generic;
using HeathenEngineering.SteamApi.Foundation;
using HeathenEngineering.Tools;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.GameServices;

[Serializable]
public class HeathenWorkshopReadCommunityItem
{
	public string Title;

	public string Description;

	public AppId_t TargetApp;

	public PublishedFileId_t FileId;

	public CSteamID Author;

	public DateTime CreatedOn;

	public DateTime LastUpdated;

	public uint UpVotes;

	public uint DownVotes;

	public float VoteScore;

	public bool IsBanned;

	public bool IsTagsTruncated;

	public bool IsSubscribed;

	public int FileSize;

	[EnumFlags]
	public EItemState StateFlags;

	public ERemoteStoragePublishedFileVisibility Visibility = ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate;

	public List<string> Tags = new List<string>();

	public Texture2D previewImage;

	public string PreviewImageLocation;

	public SteamUGCDetails_t SourceItemDetails;

	public UnityEvent PreviewImageUpdated = new UnityEvent();

	public CallResult<RemoteStorageDownloadUGCResult_t> m_RemoteStorageDownloadUGCResult;

	public HeathenWorkshopReadCommunityItem(SteamUGCDetails_t itemDetails)
	{
		SourceItemDetails = itemDetails;
		if (itemDetails.m_eFileType != 0)
		{
			Debug.LogWarning("HeathenWorkshopReadItem is designed to display File Type = Community Item, this item is not a community item and may not load correctly.");
		}
		m_RemoteStorageDownloadUGCResult = CallResult<RemoteStorageDownloadUGCResult_t>.Create(HandleUGCDownload);
		TargetApp = itemDetails.m_nConsumerAppID;
		FileId = itemDetails.m_nPublishedFileId;
		Title = itemDetails.m_rgchTitle;
		Description = itemDetails.m_rgchDescription;
		Visibility = itemDetails.m_eVisibility;
		Author = new CSteamID(itemDetails.m_ulSteamIDOwner);
		CreatedOn = SteamUtilities.ConvertUnixDate(itemDetails.m_rtimeCreated);
		LastUpdated = SteamUtilities.ConvertUnixDate(itemDetails.m_rtimeUpdated);
		UpVotes = itemDetails.m_unVotesUp;
		DownVotes = itemDetails.m_unVotesDown;
		VoteScore = itemDetails.m_flScore;
		IsBanned = itemDetails.m_bBanned;
		IsTagsTruncated = itemDetails.m_bTagsTruncated;
		FileSize = itemDetails.m_nFileSize;
		Visibility = itemDetails.m_eVisibility;
		Tags.AddRange(itemDetails.m_rgchTags.Split(','));
		uint num = (uint)(StateFlags = (EItemState)SteamUGC.GetItemState(FileId));
		IsSubscribed = SteamUtilities.WorkshopItemStateHasFlag(StateFlags, EItemState.k_EItemStateSubscribed);
		if (itemDetails.m_nPreviewFileSize > 0)
		{
			SteamAPICall_t hAPICall = SteamRemoteStorage.UGCDownload(itemDetails.m_hPreviewFile, 1u);
			m_RemoteStorageDownloadUGCResult.Set(hAPICall, HandleUGCDownloadPreviewFile);
		}
		else
		{
			Debug.LogWarning("Item [" + Title + "] has no preview file!");
		}
	}

	private void HandleUGCDownload(RemoteStorageDownloadUGCResult_t param, bool bIOFailure)
	{
		if (!bIOFailure)
		{
			Debug.LogError("UGC Download generic handler loaded without failure.");
		}
		else
		{
			Debug.LogError("UGC Download request failed.");
		}
	}

	private void HandleUGCDownloadPreviewFile(RemoteStorageDownloadUGCResult_t param, bool bIOFailure)
	{
		if (!bIOFailure)
		{
			if (param.m_eResult == EResult.k_EResultOK)
			{
				byte[] array = new byte[param.m_nSizeInBytes];
				SteamRemoteStorage.UGCRead(param.m_hFile, array, param.m_nSizeInBytes, 0u, EUGCReadAction.k_EUGCRead_ContinueReadingUntilFinished);
				previewImage = new Texture2D(2, 2);
				previewImage.LoadImage(array);
				PreviewImageLocation = param.m_pchFileName;
			}
			else
			{
				Debug.LogError("UGC Download: unexpected result state: " + param.m_eResult.ToString() + "\nImage will not be loaded.");
			}
		}
		else
		{
			Debug.LogError("UGC Download request failed.");
		}
	}
}
