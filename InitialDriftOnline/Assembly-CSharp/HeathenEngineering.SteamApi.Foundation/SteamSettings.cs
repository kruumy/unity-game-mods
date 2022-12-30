using System;
using System.Collections.Generic;
using HeathenEngineering.Events;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace HeathenEngineering.SteamApi.Foundation;

[HelpURL("https://heathen-engineering.github.io/steamworks-documentation/class_heathen_engineering_1_1_steam_api_1_1_foundation_1_1_steam_settings.html")]
[CreateAssetMenu(menuName = "Steamworks/Foundation/Steam Settings")]
public class SteamSettings : ScriptableObject
{
	[Serializable]
	public class SteamClan
	{
		public CSteamID id;

		public string displayName;

		public string tagString;

		public SteamUserData Owner;

		public List<SteamUserData> Officers;

		public SteamClan(CSteamID clanId)
		{
			id = clanId;
			displayName = SteamFriends.GetClanName(id);
			tagString = SteamFriends.GetClanTag(id);
			Owner = current.client.GetUserData(SteamFriends.GetClanOwner(id));
			int clanOfficerCount = SteamFriends.GetClanOfficerCount(id);
			Officers = new List<SteamUserData>();
			for (int i = 0; i < clanOfficerCount; i++)
			{
				Officers.Add(current.client.GetUserData(SteamFriends.GetClanOfficerByIndex(id, i)));
			}
		}

		public void OpenChat()
		{
			SteamFriends.ActivateGameOverlayToUser("chat", id);
		}

		public List<SteamUserData> GetChatMembers()
		{
			List<SteamUserData> list = new List<SteamUserData>();
			int clanChatMemberCount = SteamFriends.GetClanChatMemberCount(id);
			for (int i = 0; i < clanChatMemberCount; i++)
			{
				list.Add(current.client.GetUserData(SteamFriends.GetChatMemberByIndex(id, i)));
			}
			return list;
		}
	}

	[Serializable]
	public class GameClient
	{
		[Serializable]
		public class Overlay
		{
			public Vector2Int notificationInset;

			public ENotificationPosition notificationPosition = ENotificationPosition.k_EPositionBottomRight;

			private static bool _OverlayOpen;

			public bool IsEnabled => SteamUtils.IsOverlayEnabled();

			public bool IsOpen => _OverlayOpen;

			public void HandleOnOverlayOpen(GameOverlayActivated_t data)
			{
				_OverlayOpen = data.m_bActive == 1;
			}

			public void Invite(CSteamID lobbyId)
			{
				SteamFriends.ActivateGameOverlayInviteDialog(lobbyId);
			}

			public void OpenStore()
			{
				OpenStore(SteamUtils.GetAppID(), EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
			}

			public void OpenStore(uint appId)
			{
				OpenStore(new AppId_t(appId), EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
			}

			public void OpenStore(uint appId, EOverlayToStoreFlag flag)
			{
				OpenStore(new AppId_t(appId), flag);
			}

			public void OpenStore(AppId_t appId, EOverlayToStoreFlag flag)
			{
				SteamFriends.ActivateGameOverlayToStore(appId, flag);
			}

			public void Open(string dialog)
			{
				SteamFriends.ActivateGameOverlay(dialog);
			}

			public void OpenWebPage(string URL)
			{
				SteamFriends.ActivateGameOverlayToWebPage(URL);
			}

			public void OpenFriends()
			{
				SteamFriends.ActivateGameOverlay("friends");
			}

			public void OpenCommunity()
			{
				SteamFriends.ActivateGameOverlay("community");
			}

			public void OpenPlayers()
			{
				SteamFriends.ActivateGameOverlay("players");
			}

			public void OpenSettings()
			{
				SteamFriends.ActivateGameOverlay("settings");
			}

			public void OpenOfficialGameGroup()
			{
				SteamFriends.ActivateGameOverlay("officialgamegroup");
			}

			public void OpenStats()
			{
				SteamFriends.ActivateGameOverlay("stats");
			}

			public void OpenAchievements()
			{
				SteamFriends.ActivateGameOverlay("achievements");
			}

			public void OpenChat(CSteamID user)
			{
				SteamFriends.ActivateGameOverlayToUser("Chat", user);
			}

			public void OpenProfile(CSteamID user)
			{
				SteamFriends.ActivateGameOverlayToUser("steamid", user);
			}

			public void OpenTrade(CSteamID user)
			{
				SteamFriends.ActivateGameOverlayToUser("jointrade", user);
			}

			public void OpenStats(CSteamID user)
			{
				SteamFriends.ActivateGameOverlayToUser("stats", user);
			}

			public void OpenAchievements(CSteamID user)
			{
				SteamFriends.ActivateGameOverlayToUser("achievements", user);
			}

			public void OpenFriendAdd(CSteamID user)
			{
				SteamFriends.ActivateGameOverlayToUser("friendadd", user);
			}

			public void OpenFriendRemove(CSteamID user)
			{
				SteamFriends.ActivateGameOverlayToUser("friendremove", user);
			}

			public void OpenRequestAccept(CSteamID user)
			{
				SteamFriends.ActivateGameOverlayToUser("friendrequestaccept", user);
			}

			public void OpenRequestIgnore(CSteamID user)
			{
				SteamFriends.ActivateGameOverlayToUser("friendrequestignore", user);
			}
		}

		public SteamUserData user;

		public Overlay overlay = new Overlay();

		public int lastKnownPlayerCount;

		public Dictionary<ulong, SteamUserData> knownUsers = new Dictionary<ulong, SteamUserData>();

		public List<SteamStatData> stats = new List<SteamStatData>();

		public List<SteamAchievementData> achievements = new List<SteamAchievementData>();

		private CGameID m_GameID;

		private Callback<AvatarImageLoaded_t> avatarLoadedCallback;

		private Callback<PersonaStateChange_t> personaStateChange;

		private Callback<UserStatsReceived_t> m_UserStatsReceived;

		private Callback<UserStatsStored_t> m_UserStatsStored;

		public Callback<GameOverlayActivated_t> m_GameOverlayActivated;

		private Callback<UserAchievementStored_t> m_UserAchievementStored;

		private Callback<GameConnectedFriendChatMsg_t> m_GameConnectedFrinedChatMsg;

		private CallResult<NumberOfCurrentPlayers_t> m_OnNumberOfCurrentPlayersCallResult;

		private CallResult<FriendsGetFollowerCount_t> m_FriendsGetFollowerCount;

		private Dictionary<CSteamID, Action<SteamUserData, int>> FollowCallbacks = new Dictionary<CSteamID, Action<SteamUserData, int>>();

		[HideInInspector]
		public UnityAvatarImageLoadedEvent onAvatarLoaded;

		[HideInInspector]
		public UnityPersonaStateChangeEvent onPersonaStateChanged;

		[HideInInspector]
		public UnityUserStatsReceivedEvent onUserStatsReceived;

		[HideInInspector]
		public UnityUserStatsStoredEvent onUserStatsStored;

		[HideInInspector]
		public UnityBoolEvent onOverlayActivated;

		[HideInInspector]
		public UnityUserAchievementStoredEvent onAchievementStored;

		[HideInInspector]
		public FriendChatMessageEvent onRecievedFriendChatMessage;

		[HideInInspector]
		public UnityNumberOfCurrentPlayersResultEvent onNumberOfCurrentPlayersResult;

		[Obsolete("Please use user as opposed to userData. This field will be removed in a future update.")]
		public SteamUserData userData
		{
			get
			{
				return user;
			}
			set
			{
				user = value;
			}
		}

		public void StoreStatsAndAchievements()
		{
			SteamUserStats.StoreStats();
		}

		public void RegisterAchievementsSystem()
		{
			m_GameID = new CGameID(SteamUtils.GetAppID());
			m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(HandleUserStatsReceived);
			m_UserStatsStored = Callback<UserStatsStored_t>.Create(HandleUserStatsStored);
			m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(HandleAchievementStored);
			m_FriendsGetFollowerCount = CallResult<FriendsGetFollowerCount_t>.Create(HandleGetFollowerCount);
			m_OnNumberOfCurrentPlayersCallResult = CallResult<NumberOfCurrentPlayers_t>.Create(OnNumberOfCurrentPlayers);
		}

		public bool RequestCurrentStats()
		{
			SteamAPICall_t numberOfCurrentPlayers = SteamUserStats.GetNumberOfCurrentPlayers();
			m_OnNumberOfCurrentPlayersCallResult.Set(numberOfCurrentPlayers);
			return SteamUserStats.RequestCurrentStats();
		}

		public void RefreshPlayerCount()
		{
			SteamAPICall_t numberOfCurrentPlayers = SteamUserStats.GetNumberOfCurrentPlayers();
			m_OnNumberOfCurrentPlayersCallResult.Set(numberOfCurrentPlayers);
		}

		private void OnNumberOfCurrentPlayers(NumberOfCurrentPlayers_t pCallback, bool bIOFailure)
		{
			if (!bIOFailure)
			{
				if (pCallback.m_bSuccess == 1)
				{
					lastKnownPlayerCount = pCallback.m_cPlayers;
				}
				if (onNumberOfCurrentPlayersResult != null)
				{
					onNumberOfCurrentPlayersResult.Invoke(pCallback);
				}
			}
		}

		private void HandleUserStatsReceived(UserStatsReceived_t pCallback)
		{
			if ((ulong)m_GameID != pCallback.m_nGameID)
			{
				return;
			}
			if (EResult.k_EResultOK == pCallback.m_eResult)
			{
				foreach (SteamAchievementData achievement in achievements)
				{
					if (SteamUserStats.GetAchievement(achievement.achievementId.ToString(), out achievement.isAchieved))
					{
						achievement.displayName = SteamUserStats.GetAchievementDisplayAttribute(achievement.achievementId, "name");
						achievement.displayDescription = SteamUserStats.GetAchievementDisplayAttribute(achievement.achievementId, "desc");
						achievement.hidden = SteamUserStats.GetAchievementDisplayAttribute(achievement.achievementId, "hidden") == "1";
					}
					else
					{
						Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + achievement.achievementId + "\nIs it registered in the Steam Partner site?");
					}
				}
				foreach (SteamStatData stat in stats)
				{
					int pData2;
					if (stat.DataType == SteamStatData.StatDataType.Float)
					{
						if (SteamUserStats.GetStat(stat.statName, out float pData))
						{
							stat.InternalUpdateValue(pData);
						}
						else
						{
							Debug.LogWarning("SteamUserStats.GetAchievement failed for Stat " + stat.statName + "\nIs it registered in the Steam Partner site and the correct data type?");
						}
					}
					else if (SteamUserStats.GetStat(stat.statName, out pData2))
					{
						stat.InternalUpdateValue(pData2);
					}
					else
					{
						Debug.LogWarning("SteamUserStats.GetAchievement failed for Stat " + stat.statName + "\nIs it registered in the Steam Partner site and the correct data type?");
					}
				}
				onUserStatsReceived.Invoke(pCallback);
			}
			else
			{
				Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
			}
		}

		private void HandleUserStatsStored(UserStatsStored_t pCallback)
		{
			if ((ulong)m_GameID == pCallback.m_nGameID)
			{
				if (EResult.k_EResultOK == pCallback.m_eResult)
				{
					onUserStatsStored.Invoke(pCallback);
				}
				else if (EResult.k_EResultInvalidParam == pCallback.m_eResult)
				{
					Debug.Log("StoreStats - some failed to validate, re-syncing data now in an attempt to correct.");
					UserStatsReceived_t pCallback2 = default(UserStatsReceived_t);
					pCallback2.m_eResult = EResult.k_EResultOK;
					pCallback2.m_nGameID = (ulong)m_GameID;
					HandleUserStatsReceived(pCallback2);
				}
				else
				{
					Debug.Log("StoreStats - failed, " + pCallback.m_eResult);
				}
			}
		}

		private void HandleAchievementStored(UserAchievementStored_t pCallback)
		{
			if ((ulong)m_GameID == pCallback.m_nGameID)
			{
				if (pCallback.m_nMaxProgress == 0)
				{
					Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
				}
				else
				{
					Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' progress callback, (" + pCallback.m_nCurProgress + "," + pCallback.m_nMaxProgress + ")");
				}
				onAchievementStored.Invoke(pCallback);
			}
		}

		private void HandleGetFollowerCount(FriendsGetFollowerCount_t param, bool bIOFailure)
		{
			if (FollowCallbacks.ContainsKey(param.m_steamID))
			{
				Action<SteamUserData, int> action = FollowCallbacks[param.m_steamID];
				if (param.m_eResult != EResult.k_EResultOK || bIOFailure)
				{
					action?.Invoke(GetUserData(param.m_steamID), -1);
					FollowCallbacks.Remove(param.m_steamID);
				}
				else
				{
					action?.Invoke(GetUserData(param.m_steamID), param.m_nCount);
					FollowCallbacks.Remove(param.m_steamID);
				}
			}
		}

		public void UnlockAchievement(uint achievementIndex)
		{
			SteamAchievementData steamAchievementData = achievements[Convert.ToInt32(achievementIndex)];
			if (steamAchievementData != null && !steamAchievementData.isAchieved)
			{
				UnlockAchievementData(steamAchievementData);
			}
		}

		public void UnlockAchievementData(SteamAchievementData data)
		{
			data.Unlock();
		}

		public void ClearAchievement(uint achievementIndex)
		{
			SteamAchievementData steamAchievementData = achievements[Convert.ToInt32(achievementIndex)];
			if (steamAchievementData != null && !steamAchievementData.isAchieved)
			{
				ClearAchievement(steamAchievementData);
			}
		}

		public void ClearAchievement(SteamAchievementData data)
		{
			data.ClearAchievement();
		}

		public void HandleOnOverlayOpen(GameOverlayActivated_t data)
		{
			overlay.HandleOnOverlayOpen(data);
			onOverlayActivated.Invoke(overlay.IsOpen);
		}

		public void SetNotificationPosition(ENotificationPosition position)
		{
			SteamUtils.SetOverlayNotificationPosition(overlay.notificationPosition);
			overlay.notificationPosition = position;
		}

		public void SetNotificationInset(int X, int Y)
		{
			SteamUtils.SetOverlayNotificationInset(X, Y);
			overlay.notificationInset = new Vector2Int(X, Y);
		}

		public void SetNotificationInset(Vector2Int inset)
		{
			SteamUtils.SetOverlayNotificationInset(inset.x, inset.y);
			overlay.notificationInset = inset;
		}

		public List<SteamUserData> ListFriends(EFriendFlags friendFlags = EFriendFlags.k_EFriendFlagImmediate)
		{
			List<SteamUserData> list = new List<SteamUserData>();
			int friendCount = SteamFriends.GetFriendCount(friendFlags);
			for (int i = 0; i < friendCount; i++)
			{
				list.Add(GetUserData(SteamFriends.GetFriendByIndex(i, friendFlags)));
			}
			return list;
		}

		public List<SteamClan> ListClans()
		{
			List<SteamClan> list = new List<SteamClan>();
			int clanCount = SteamFriends.GetClanCount();
			for (int i = 0; i < clanCount; i++)
			{
				list.Add(new SteamClan(SteamFriends.GetClanByIndex(i)));
			}
			return list;
		}

		public void GetFollowerCount(CSteamID followingThisUser, Action<SteamUserData, int> callback)
		{
			FollowCallbacks.Add(followingThisUser, callback);
			SteamFriends.GetFollowerCount(followingThisUser);
		}

		public bool SetRichPresence(string key, string value)
		{
			return SteamFriends.SetRichPresence(key, value);
		}

		public void ClearRichPresence()
		{
			SteamFriends.ClearRichPresence();
		}

		public void RegisterFriendsSystem(SteamUserData data = null)
		{
			avatarLoadedCallback = Callback<AvatarImageLoaded_t>.Create(HandleAvatarLoaded);
			personaStateChange = Callback<PersonaStateChange_t>.Create(HandlePersonaStatReceived);
			m_GameConnectedFrinedChatMsg = Callback<GameConnectedFriendChatMsg_t>.Create(HandleGameConnectedFriendMsg);
			if (onRecievedFriendChatMessage == null)
			{
				onRecievedFriendChatMessage = new FriendChatMessageEvent();
			}
			if (onAvatarLoaded == null)
			{
				onAvatarLoaded = new UnityAvatarImageLoadedEvent();
			}
			if (onPersonaStateChanged == null)
			{
				onPersonaStateChanged = new UnityPersonaStateChangeEvent();
			}
			if (data != null)
			{
				user = data;
			}
			if (user == null)
			{
				user = ScriptableObject.CreateInstance<SteamUserData>();
			}
			user.id = SteamUser.GetSteamID();
			knownUsers.Clear();
			knownUsers.Add(user.id.m_SteamID, user);
			int largeFriendAvatar = SteamFriends.GetLargeFriendAvatar(user.id);
			if (largeFriendAvatar > 0)
			{
				ApplyAvatarImage(user, largeFriendAvatar);
			}
		}

		private void HandleAvatarLoaded(AvatarImageLoaded_t data)
		{
			if (knownUsers.ContainsKey(data.m_steamID.m_SteamID))
			{
				SteamUserData steamUserData = knownUsers[data.m_steamID.m_SteamID];
				ApplyAvatarImage(steamUserData, data.m_iImage);
				if (steamUserData.OnAvatarLoaded == null)
				{
					steamUserData.OnAvatarLoaded = new UnityEvent();
				}
				steamUserData.OnAvatarLoaded.Invoke();
			}
			else
			{
				SteamUserData steamUserData2 = ScriptableObject.CreateInstance<SteamUserData>();
				steamUserData2.id = data.m_steamID;
				knownUsers.Add(steamUserData2.id.m_SteamID, steamUserData2);
				ApplyAvatarImage(steamUserData2, data.m_iImage);
				steamUserData2.OnAvatarLoaded.Invoke();
			}
			onAvatarLoaded.Invoke(data);
		}

		private void HandleGameConnectedFriendMsg(GameConnectedFriendChatMsg_t callback)
		{
			SteamFriends.GetFriendMessage(callback.m_steamIDUser, callback.m_iMessageID, out var pvData, 2048, out var peChatEntryType);
			onRecievedFriendChatMessage.Invoke(GetUserData(callback.m_steamIDUser), pvData, peChatEntryType);
		}

		private void HandlePersonaStatReceived(PersonaStateChange_t pCallback)
		{
			SteamUserData steamUserData = null;
			if (knownUsers.ContainsKey(pCallback.m_ulSteamID))
			{
				steamUserData = knownUsers[pCallback.m_ulSteamID];
			}
			else
			{
				steamUserData = ScriptableObject.CreateInstance<SteamUserData>();
				steamUserData.id = new CSteamID(pCallback.m_ulSteamID);
				knownUsers.Add(steamUserData.id.m_SteamID, steamUserData);
			}
			switch (pCallback.m_nChangeFlags)
			{
			case EPersonaChange.k_EPersonaChangeAvatar:
				try
				{
					int largeFriendAvatar = SteamFriends.GetLargeFriendAvatar(steamUserData.id);
					if (largeFriendAvatar > 0)
					{
						steamUserData.iconLoaded = true;
						SteamUtils.GetImageSize(largeFriendAvatar, out var pnWidth, out var pnHeight);
						byte[] array = new byte[4 * pnWidth * pnHeight];
						if (SteamUtils.GetImageRGBA(largeFriendAvatar, array, array.Length))
						{
							steamUserData.avatar.LoadRawTextureData(SteamUtilities.FlipImageBufferVertical((int)pnWidth, (int)pnHeight, array));
							steamUserData.avatar.Apply();
							steamUserData.OnAvatarChanged.Invoke();
						}
					}
				}
				catch
				{
				}
				break;
			case EPersonaChange.k_EPersonaChangeComeOnline:
				if (steamUserData.OnComeOnline != null)
				{
					steamUserData.OnComeOnline.Invoke();
				}
				if (steamUserData.OnStateChange != null)
				{
					steamUserData.OnStateChange.Invoke();
				}
				break;
			case EPersonaChange.k_EPersonaChangeGamePlayed:
				if (steamUserData.OnGameChanged != null)
				{
					steamUserData.OnGameChanged.Invoke();
				}
				if (steamUserData.OnStateChange != null)
				{
					steamUserData.OnStateChange.Invoke();
				}
				break;
			case EPersonaChange.k_EPersonaChangeGoneOffline:
				if (steamUserData.OnGoneOffline != null)
				{
					steamUserData.OnGoneOffline.Invoke();
				}
				if (steamUserData.OnStateChange != null)
				{
					steamUserData.OnStateChange.Invoke();
				}
				break;
			case EPersonaChange.k_EPersonaChangeName:
				if (steamUserData.OnNameChanged != null)
				{
					steamUserData.OnNameChanged.Invoke();
				}
				break;
			}
			onPersonaStateChanged.Invoke(pCallback);
		}

		private void ApplyAvatarImage(SteamUserData user, int imageId)
		{
			SteamUtils.GetImageSize(imageId, out var pnWidth, out var pnHeight);
			user.avatar = new Texture2D((int)pnWidth, (int)pnHeight, TextureFormat.RGBA32, mipChain: false);
			int num = (int)(pnWidth * pnHeight * 4);
			byte[] array = new byte[num];
			SteamUtils.GetImageRGBA(imageId, array, num);
			user.avatar.LoadRawTextureData(SteamUtilities.FlipImageBufferVertical((int)pnWidth, (int)pnHeight, array));
			user.avatar.Apply();
			user.iconLoaded = true;
		}

		public bool ListenForFriendMessages(bool isOn)
		{
			return SteamFriends.SetListenForFriendsMessages(isOn);
		}

		public bool SendFriendChatMessage(SteamUserData friend, string message)
		{
			return friend.SendMessage(message);
		}

		public bool SendFriendChatMessage(ulong friendId, string message)
		{
			return SendFriendChatMessage(new CSteamID(friendId), message);
		}

		public bool SendFriendChatMessage(CSteamID friend, string message)
		{
			return SteamFriends.ReplyToFriendMessage(friend, message);
		}

		public void RefreshAvatar(SteamUserData userData)
		{
			int largeFriendAvatar = SteamFriends.GetLargeFriendAvatar(userData.id);
			if (largeFriendAvatar > 0)
			{
				ApplyAvatarImage(userData, largeFriendAvatar);
			}
		}

		public SteamUserData GetUserData(CSteamID steamID)
		{
			if (knownUsers.ContainsKey(steamID.m_SteamID))
			{
				SteamUserData result = knownUsers[steamID.m_SteamID];
				int largeFriendAvatar = SteamFriends.GetLargeFriendAvatar(steamID);
				if (largeFriendAvatar > 0)
				{
					ApplyAvatarImage(result, largeFriendAvatar);
				}
				return result;
			}
			SteamUserData steamUserData = ScriptableObject.CreateInstance<SteamUserData>();
			steamUserData.id = steamID;
			knownUsers.Add(steamID.m_SteamID, steamUserData);
			int largeFriendAvatar2 = SteamFriends.GetLargeFriendAvatar(steamID);
			if (largeFriendAvatar2 > 0)
			{
				ApplyAvatarImage(steamUserData, largeFriendAvatar2);
			}
			return steamUserData;
		}

		public SteamUserData GetUserData(ulong steamID)
		{
			return GetUserData(new CSteamID(steamID));
		}
	}

	public static SteamSettings current;

	[FormerlySerializedAs("ApplicationId")]
	public AppId_t applicationId = new AppId_t(0u);

	public bool isDebugging;

	public GameClient client = new GameClient();

	public bool Initialized { get; private set; }

	[Obsolete("Use SteamSettings.client.overlay.notificationPosition", false)]
	public ENotificationPosition NotificationPosition => client.overlay.notificationPosition;

	[Obsolete("UserData is deprecated and will be removed in a later update, use SteamSettings.client.userData in its place", false)]
	public SteamUserData UserData => client.userData;

	[Obsolete("LastKnownPlayerCount is deprecated and will be removed in a later update, use SteamSettings.client.lastKnownPlayerCount in its place", false)]
	public int LastKnownPlayerCount => client.lastKnownPlayerCount;

	[Obsolete("Overlay is deprecated and will be removed in a later update, use SteamSettings.client.overlay in its place", false)]
	public GameClient.Overlay Overlay => client.overlay;

	[Obsolete("Use SteamSettings.client.knownUsers", false)]
	public Dictionary<ulong, SteamUserData> KnownUsers => client.knownUsers;

	[Obsolete("Use SteamSettings.client.onAvatarLoaded", false)]
	public UnityAvatarImageLoadedEvent OnAvatarLoaded => client.onAvatarLoaded;

	[Obsolete("Use SteamSettings.client.onPersonaStateChanged", false)]
	public UnityPersonaStateChangeEvent OnPersonaStateChanged => client.onPersonaStateChanged;

	[Obsolete("Use SteamSettings.client.onUserStatsReceived", false)]
	public UnityUserStatsReceivedEvent OnUserStatsReceived => client.onUserStatsReceived;

	[Obsolete("Use SteamSettings.client.onUserStatsStored", false)]
	public UnityUserStatsStoredEvent OnUserStatsStored => client.onUserStatsStored;

	[Obsolete("Use SteamSettings.client.onOverlayActivated", false)]
	public UnityBoolEvent OnOverlayActivated => client.onOverlayActivated;

	[Obsolete("Use SteamSettings.client.onAchievementStored", false)]
	public UnityUserAchievementStoredEvent OnAchievementStored => client.onAchievementStored;

	[Obsolete("Use SteamSettings.client.onRecievedFriendChatMessage", false)]
	public FriendChatMessageEvent OnRecievedFriendChatMessage => client.onRecievedFriendChatMessage;

	[Obsolete("Use SteamSettings.client.onNumberOfCurrentPlayersResult", false)]
	public UnityNumberOfCurrentPlayersResultEvent OnNumberOfCurrentPlayersResult => client.onNumberOfCurrentPlayersResult;

	public void Init()
	{
		current = this;
		Initialized = SteamAPI.Init();
	}
}
