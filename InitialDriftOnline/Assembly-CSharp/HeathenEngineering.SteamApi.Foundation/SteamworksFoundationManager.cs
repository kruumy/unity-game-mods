using System;
using System.Linq;
using System.Text;
using HeathenEngineering.Events;
using HeathenEngineering.Scriptable;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace HeathenEngineering.SteamApi.Foundation;

[HelpURL("https://heathen-engineering.github.io/steamworks-documentation/class_heathen_engineering_1_1_steam_api_1_1_foundation_1_1_steamworks_foundation_manager.html")]
[DisallowMultipleComponent]
public class SteamworksFoundationManager : MonoBehaviour
{
	[FormerlySerializedAs("Settings")]
	public SteamSettings settings;

	public BoolReference _doNotDistroyOnLoad = new BoolReference(value: false);

	[FormerlySerializedAs("OnSteamInitalized")]
	public UnityEvent onSteamInitalized;

	[FormerlySerializedAs("OnSteamInitalizationError")]
	public UnityStringEvent onSteamInitalizationError;

	[FormerlySerializedAs("OnOverlayActivated")]
	public UnityBoolEvent onOverlayActivated;

	[FormerlySerializedAs("OnUserStatsRecieved")]
	public UnityUserStatsReceivedEvent onUserStatsRecieved;

	[FormerlySerializedAs("OnUserStatsStored")]
	public UnityUserStatsStoredEvent onUserStatsStored;

	[FormerlySerializedAs("OnNumberOfCurrentPlayersResult")]
	public UnityNumberOfCurrentPlayersResultEvent onNumberOfCurrentPlayersResult;

	[FormerlySerializedAs("OnAchievementStored")]
	public UnityUserAchievementStoredEvent onAchievementStored;

	[FormerlySerializedAs("OnSteamIOnAvatarLoadednitalized")]
	public UnityAvatarImageLoadedEvent onAvatarLoaded;

	[FormerlySerializedAs("OnPersonaStateChanged")]
	public UnityPersonaStateChangeEvent onPersonaStateChanged;

	[FormerlySerializedAs("OnRecievedFriendChatMessage")]
	public FriendChatMessageEvent onRecievedFriendChatMessage;

	private static SteamworksFoundationManager s_instance;

	public static bool s_EverInialized;

	private ENotificationPosition currentNotificationPosition = ENotificationPosition.k_EPositionBottomRight;

	private Vector2Int currentNotificationIndent = Vector2Int.zero;

	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

	[Obsolete("This field exists to support a singleton based design model however singleton as a model created unnessisary dependencies between GameObjects. It is recomended that you use SteamSettings objects directly.", false)]
	public static SteamworksFoundationManager Instance => s_instance;

	[Obsolete("use SteamSettings.initialized", false)]
	public bool _initialized => settings.Initialized;

	[Obsolete("Reference the BoolVariable assigned to the SteamSettings.initialized member directly as opposed to using static members", false)]
	public static bool Initialized => Instance._initialized;

	[Obsolete("Use SteamSettings.client.userData", false)]
	public static SteamUserData _UserData
	{
		get
		{
			if (Instance != null && Instance.settings != null)
			{
				return Instance.settings.client.userData;
			}
			return null;
		}
	}

	[Obsolete("Use SteamSettings.client.userData", false)]
	public SteamUserData UserData
	{
		get
		{
			if (settings != null)
			{
				return settings.client.userData;
			}
			return null;
		}
	}

	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	private void Awake()
	{
		if (s_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		s_instance = this;
		if (s_EverInialized)
		{
			onSteamInitalizationError.Invoke("Tried to Initialize the SteamAPI twice in one session!");
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		if (_doNotDistroyOnLoad.Value)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
			onSteamInitalizationError.Invoke("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.");
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
			onSteamInitalizationError.Invoke("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.");
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary(settings.applicationId))
			{
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException ex)
		{
			Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + ex, this);
			onSteamInitalizationError.Invoke("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + ex);
			Application.Quit();
			return;
		}
		settings.Init();
		if (!settings.Initialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			onSteamInitalizationError.Invoke("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.");
		}
		else
		{
			s_EverInialized = true;
			onSteamInitalized.Invoke();
			Debug.Log("Steam client Initalized!");
		}
	}

	private void OnEnable()
	{
		if (s_instance == null)
		{
			s_instance = this;
		}
		if (settings.Initialized)
		{
			Debug.Log("Client Startup Detected!");
			if (m_SteamAPIWarningMessageHook == null)
			{
				m_SteamAPIWarningMessageHook = SteamAPIDebugTextHook;
				SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
			}
			settings.client.m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(settings.client.HandleOnOverlayOpen);
			settings.client.onOverlayActivated.AddListener(onOverlayActivated.Invoke);
			settings.client.RegisterAchievementsSystem();
			settings.client.onAchievementStored.AddListener(onAchievementStored.Invoke);
			settings.client.onUserStatsReceived.AddListener(onUserStatsRecieved.Invoke);
			settings.client.onUserStatsStored.AddListener(onUserStatsStored.Invoke);
			settings.client.onNumberOfCurrentPlayersResult.AddListener(onNumberOfCurrentPlayersResult.Invoke);
			settings.client.RequestCurrentStats();
			settings.client.RegisterFriendsSystem(settings.client.user);
			settings.client.onAvatarLoaded.AddListener(onAvatarLoaded.Invoke);
			settings.client.onPersonaStateChanged.AddListener(onPersonaStateChanged.Invoke);
			settings.client.onRecievedFriendChatMessage.AddListener(onRecievedFriendChatMessage.Invoke);
		}
	}

	private void OnDisable()
	{
		settings.client.onOverlayActivated.RemoveListener(onOverlayActivated.Invoke);
		settings.client.onAchievementStored.RemoveListener(onAchievementStored.Invoke);
		settings.client.onUserStatsReceived.RemoveListener(onUserStatsRecieved.Invoke);
		settings.client.onUserStatsStored.RemoveListener(onUserStatsStored.Invoke);
		settings.client.onNumberOfCurrentPlayersResult.RemoveListener(onNumberOfCurrentPlayersResult.Invoke);
		settings.client.onAvatarLoaded.RemoveListener(onAvatarLoaded.Invoke);
		settings.client.onPersonaStateChanged.RemoveListener(onPersonaStateChanged.Invoke);
		settings.client.onRecievedFriendChatMessage.RemoveListener(onRecievedFriendChatMessage.Invoke);
	}

	private void OnDestroy()
	{
		if (settings != null && settings.client.userData != null)
		{
			settings.client.userData.ClearData();
		}
		if (!(s_instance != this))
		{
			s_instance = null;
			if (settings.Initialized)
			{
				SteamAPI.Shutdown();
			}
		}
	}

	private void Update()
	{
		if (!settings.Initialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
		if (settings != null)
		{
			if (currentNotificationPosition != settings.client.overlay.notificationPosition)
			{
				currentNotificationPosition = settings.client.overlay.notificationPosition;
				settings.client.SetNotificationPosition(settings.client.overlay.notificationPosition);
			}
			if (currentNotificationIndent != settings.client.overlay.notificationInset)
			{
				currentNotificationIndent = settings.client.overlay.notificationInset;
				settings.client.SetNotificationInset(settings.client.overlay.notificationInset);
			}
		}
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

	[Obsolete("Use SteamSettings.client.SetNotificationPosition", false)]
	public static void _SetNotificationPosition(ENotificationPosition position)
	{
		if (Instance != null && Instance.settings != null)
		{
			Instance.settings.client.SetNotificationPosition(position);
		}
	}

	[Obsolete("Use SteamSettings.client.SetNotificationInset", false)]
	public static void _SetNotificationInset(Vector2Int inset)
	{
		if (Instance != null && Instance.settings != null)
		{
			Instance.settings.client.SetNotificationInset(inset);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenStore", false)]
	public static void _OpenStore()
	{
		if (Instance != null && Instance.settings != null)
		{
			Instance.settings.client.overlay.OpenStore();
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenStore", false)]
	public static void _OpenStore(uint appId)
	{
		if (Instance != null && Instance.settings != null)
		{
			Instance.settings.client.overlay.OpenStore(appId);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenStore", false)]
	public static void _OpenStore(uint appId, EOverlayToStoreFlag flag)
	{
		if (Instance != null && Instance.settings != null)
		{
			Instance.settings.client.overlay.OpenStore(appId, flag);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenStore", false)]
	public static void _OpenStore(AppId_t appId, EOverlayToStoreFlag flag)
	{
		if (Instance != null && Instance.settings != null)
		{
			Instance.settings.client.overlay.OpenStore(appId, flag);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.Open", false)]
	public static void _Open(string dialog)
	{
		if (Instance != null && Instance.settings != null)
		{
			Instance.settings.client.overlay.Open(dialog);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenWebPage", false)]
	public static void _OpenWebPage(string URL)
	{
		if (Instance != null && Instance.settings != null)
		{
			Instance.settings.client.overlay.OpenWebPage(URL);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenFriends", false)]
	public static void _OpenFriends()
	{
		if (Instance != null && Instance.settings != null)
		{
			Instance.settings.client.overlay.OpenFriends();
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenCommunity", false)]
	public static void _OpenCommunity()
	{
		if (Instance != null && Instance.settings != null)
		{
			Instance.settings.client.overlay.OpenCommunity();
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenPlayers", false)]
	public static void _OpenPlayers()
	{
		if (Instance != null && Instance.settings != null)
		{
			Instance.settings.client.overlay.OpenPlayers();
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenSettings", false)]
	public static void _OpenSettings()
	{
		if (Instance != null && Instance.settings != null)
		{
			Instance.settings.client.overlay.OpenSettings();
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenOfficialGameGroup", false)]
	public static void _OpenOfficialGameGroup()
	{
		if (Instance != null && Instance.settings != null)
		{
			Instance.settings.client.overlay.OpenOfficialGameGroup();
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenStats", false)]
	public static void _OpenStats()
	{
		if (Instance != null && Instance.settings != null)
		{
			Instance.settings.client.overlay.OpenStats();
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenAchievements", false)]
	public static void _OpenAchievements()
	{
		if (Instance != null && Instance.settings != null)
		{
			Instance.settings.client.overlay.OpenAchievements();
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenChat", false)]
	public static void _OpenChat(SteamUserData user)
	{
		if (user.id.m_SteamID != SteamUser.GetSteamID().m_SteamID)
		{
			Instance.settings.client.overlay.OpenChat(user.id);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenProfile", false)]
	public static void _OpenProfile(SteamUserData user)
	{
		if (user.id.m_SteamID != SteamUser.GetSteamID().m_SteamID)
		{
			Instance.settings.client.overlay.OpenProfile(user.id);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenTrade", false)]
	public static void _OpenTrade(SteamUserData user)
	{
		if (user.id.m_SteamID != SteamUser.GetSteamID().m_SteamID)
		{
			Instance.settings.client.overlay.OpenTrade(user.id);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenStats", false)]
	public static void _OpenStats(SteamUserData user)
	{
		if (user.id.m_SteamID != SteamUser.GetSteamID().m_SteamID)
		{
			Instance.settings.client.overlay.OpenStats(user.id);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenAchievements", false)]
	public static void _OpenAchievements(SteamUserData user)
	{
		if (user.id.m_SteamID != SteamUser.GetSteamID().m_SteamID)
		{
			Instance.settings.client.overlay.OpenAchievements(user.id);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenFriendAdd", false)]
	public static void _OpenFriendAdd(SteamUserData user)
	{
		if (user.id.m_SteamID != SteamUser.GetSteamID().m_SteamID)
		{
			Instance.settings.client.overlay.OpenFriendAdd(user.id);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenFriendRemove", false)]
	public static void _OpenFriendRemove(SteamUserData user)
	{
		if (user.id.m_SteamID != SteamUser.GetSteamID().m_SteamID)
		{
			Instance.settings.client.overlay.OpenFriendRemove(user.id);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenRequestAccept", false)]
	public static void _OpenRequestAccept(SteamUserData user)
	{
		if (user.id.m_SteamID != SteamUser.GetSteamID().m_SteamID)
		{
			Instance.settings.client.overlay.OpenRequestAccept(user.id);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenRequestIgnore", false)]
	public static void _OpenRequestIgnore(SteamUserData user)
	{
		if (user.id.m_SteamID != SteamUser.GetSteamID().m_SteamID)
		{
			Instance.settings.client.overlay.OpenRequestIgnore(user.id);
		}
	}

	[Obsolete("Use SteamSettings.client.SetNotificationPosition", false)]
	public void SetNotificationPosition(ENotificationPosition position)
	{
		if (settings != null)
		{
			settings.client.SetNotificationPosition(position);
		}
	}

	[Obsolete("Use SteamSettings.client.SetNotificationInset", false)]
	public void SetNotificationInset(Vector2Int inset)
	{
		if (settings != null)
		{
			settings.client.SetNotificationInset(inset);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenStore", false)]
	public void OpenStore()
	{
		if (settings != null)
		{
			settings.client.overlay.OpenStore();
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenStore", false)]
	public void OpenStore(uint appId)
	{
		if (settings != null)
		{
			settings.client.overlay.OpenStore(appId);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenStore", false)]
	public void OpenStore(uint appId, EOverlayToStoreFlag flag)
	{
		if (settings != null)
		{
			settings.client.overlay.OpenStore(appId, flag);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenStore", false)]
	public void OpenStore(AppId_t appId, EOverlayToStoreFlag flag)
	{
		if (settings != null)
		{
			settings.client.overlay.OpenStore(appId, flag);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.Open", false)]
	public void Open(string dialog)
	{
		if (settings != null)
		{
			settings.client.overlay.Open(dialog);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenWebPage", false)]
	public void OpenWebPage(string URL)
	{
		if (settings != null)
		{
			settings.client.overlay.OpenWebPage(URL);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenFriends", false)]
	public void OpenFriends()
	{
		if (settings != null)
		{
			settings.client.overlay.OpenFriends();
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenCommunity", false)]
	public void OpenCommunity()
	{
		if (settings != null)
		{
			settings.client.overlay.OpenCommunity();
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenPlayers", false)]
	public void OpenPlayers()
	{
		if (settings != null)
		{
			settings.client.overlay.OpenPlayers();
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenSettings", false)]
	public void OpenSettings()
	{
		if (settings != null)
		{
			settings.client.overlay.OpenSettings();
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenOfficialGameGroup", false)]
	public void OpenOfficialGameGroup()
	{
		if (settings != null)
		{
			settings.client.overlay.OpenOfficialGameGroup();
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenStats", false)]
	public void OpenStats()
	{
		if (settings != null)
		{
			settings.client.overlay.OpenStats();
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenAchievements", false)]
	public void OpenAchievements()
	{
		if (settings != null)
		{
			settings.client.overlay.OpenAchievements();
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenChat", false)]
	public void OpenChat(SteamUserData user)
	{
		if (user.id.m_SteamID != SteamUser.GetSteamID().m_SteamID)
		{
			settings.client.overlay.OpenChat(user.id);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenProfile", false)]
	public void OpenProfile(SteamUserData user)
	{
		if (user.id.m_SteamID != SteamUser.GetSteamID().m_SteamID)
		{
			settings.client.overlay.OpenProfile(user.id);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenTrade", false)]
	public void OpenTrade(SteamUserData user)
	{
		if (user.id.m_SteamID != SteamUser.GetSteamID().m_SteamID)
		{
			settings.client.overlay.OpenTrade(user.id);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenStats", false)]
	public void OpenStats(SteamUserData user)
	{
		if (user.id.m_SteamID != SteamUser.GetSteamID().m_SteamID)
		{
			settings.client.overlay.OpenStats(user.id);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenAchievements", false)]
	public void OpenAchievements(SteamUserData user)
	{
		if (user.id.m_SteamID != SteamUser.GetSteamID().m_SteamID)
		{
			settings.client.overlay.OpenAchievements(user.id);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenFriendAdd", false)]
	public void OpenFriendAdd(SteamUserData user)
	{
		if (user.id.m_SteamID != SteamUser.GetSteamID().m_SteamID)
		{
			settings.client.overlay.OpenFriendAdd(user.id);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenFriendRemove", false)]
	public void OpenFriendRemove(SteamUserData user)
	{
		if (user.id.m_SteamID != SteamUser.GetSteamID().m_SteamID)
		{
			settings.client.overlay.OpenFriendRemove(user.id);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenRequestAccept", false)]
	public void OpenRequestAccept(SteamUserData user)
	{
		if (user.id.m_SteamID != SteamUser.GetSteamID().m_SteamID)
		{
			settings.client.overlay.OpenRequestAccept(user.id);
		}
	}

	[Obsolete("Use SteamSettings.client.overlay.OpenRequestIgnore", false)]
	public void OpenRequestIgnore(SteamUserData user)
	{
		if (user.id.m_SteamID != SteamUser.GetSteamID().m_SteamID)
		{
			settings.client.overlay.OpenRequestIgnore(user.id);
		}
	}

	[Obsolete("Use SteamSettings.client.GetUserData", false)]
	public static string _GetUserName(ulong steamId)
	{
		SteamUserData steamUserData = _GetUserData(steamId);
		if (steamUserData != null)
		{
			return steamUserData.DisplayName;
		}
		return string.Empty;
	}

	[Obsolete("Use SteamSettings.client.GetUserData", false)]
	public static FriendGameInfo_t _GetUserGameInfo(ulong steamId)
	{
		SteamUserData steamUserData = _GetUserData(steamId);
		if (steamUserData != null)
		{
			return steamUserData.GameInfo;
		}
		return default(FriendGameInfo_t);
	}

	[Obsolete("Use SteamSettings.client.GetUserData", false)]
	public static Texture2D _GetUserAvatar(ulong steamId)
	{
		SteamUserData steamUserData = _GetUserData(steamId);
		if (steamUserData != null)
		{
			if (!steamUserData.iconLoaded)
			{
				_RefreshAvatar(steamUserData);
			}
			return steamUserData.avatar;
		}
		return null;
	}

	[Obsolete("Use SteamSettings.client.GetUserData", false)]
	public static SteamUserData _GetUserData(ulong steamId)
	{
		return Instance.settings.client.GetUserData(new CSteamID(steamId));
	}

	[Obsolete("Use SteamSettings.client.GetUserData", false)]
	public static string _GetUserName(CSteamID steamId)
	{
		SteamUserData steamUserData = _GetUserData(steamId);
		if (steamUserData != null)
		{
			return steamUserData.DisplayName;
		}
		return string.Empty;
	}

	[Obsolete("Use SteamSettings.client.GetUserData", false)]
	public static FriendGameInfo_t _GetUserGameInfo(CSteamID steamId)
	{
		SteamUserData steamUserData = _GetUserData(steamId);
		if (steamUserData != null)
		{
			return steamUserData.GameInfo;
		}
		return default(FriendGameInfo_t);
	}

	[Obsolete("Use SteamSettings.client.GetUserData", false)]
	public static Texture2D _GetUserAvatar(CSteamID steamId)
	{
		SteamUserData steamUserData = _GetUserData(steamId);
		if (steamUserData != null)
		{
			if (!steamUserData.iconLoaded)
			{
				_RefreshAvatar(steamUserData);
			}
			return steamUserData.avatar;
		}
		return null;
	}

	[Obsolete("Use SteamSettings.client.GetUserData", false)]
	public static SteamUserData _GetUserData(CSteamID steamId)
	{
		return Instance.settings.client.GetUserData(steamId);
	}

	[Obsolete("Use SteamSettings.client.RefreshAvatar", false)]
	public static void _RefreshAvatar(SteamUserData userData)
	{
		Instance.settings.client.RefreshAvatar(userData);
	}

	[Obsolete("Use SteamSettings.client.GetUserData", false)]
	public string GetUserName(ulong steamId)
	{
		SteamUserData userData = GetUserData(steamId);
		if (userData != null)
		{
			return userData.DisplayName;
		}
		return string.Empty;
	}

	[Obsolete("Use SteamSettings.client.GetUserData", false)]
	public FriendGameInfo_t GetUserGameInfo(ulong steamId)
	{
		SteamUserData userData = GetUserData(steamId);
		if (userData != null)
		{
			return userData.GameInfo;
		}
		return default(FriendGameInfo_t);
	}

	[Obsolete("Use SteamSettings.client.GetUserData", false)]
	public Texture2D GetUserAvatar(ulong steamId)
	{
		SteamUserData userData = GetUserData(steamId);
		if (userData != null)
		{
			if (!userData.iconLoaded)
			{
				RefreshAvatar(userData);
			}
			return userData.avatar;
		}
		return null;
	}

	[Obsolete("Use SteamSettings.client.GetUserData", false)]
	public SteamUserData GetUserData(ulong steamId)
	{
		return settings.client.GetUserData(new CSteamID(steamId));
	}

	[Obsolete("Use SteamSettings.client.GetUserData", false)]
	public string GetUserName(CSteamID steamId)
	{
		SteamUserData userData = GetUserData(steamId);
		if (userData != null)
		{
			return userData.DisplayName;
		}
		return string.Empty;
	}

	[Obsolete("Use SteamSettings.client.GetUserData", false)]
	public FriendGameInfo_t GetUserGameInfo(CSteamID steamId)
	{
		SteamUserData userData = GetUserData(steamId);
		if (userData != null)
		{
			return userData.GameInfo;
		}
		return default(FriendGameInfo_t);
	}

	[Obsolete("Use SteamSettings.client.GetUserData", false)]
	public Texture2D GetUserAvatar(CSteamID steamId)
	{
		SteamUserData userData = GetUserData(steamId);
		if (userData != null)
		{
			if (!userData.iconLoaded)
			{
				RefreshAvatar(userData);
			}
			return userData.avatar;
		}
		return null;
	}

	[Obsolete("Use SteamSettings.client.GetUserData", false)]
	public SteamUserData GetUserData(CSteamID steamId)
	{
		return settings.client.GetUserData(steamId);
	}

	[Obsolete("Use SteamSettings.client.RefreshAvatar", false)]
	public void RefreshAvatar(SteamUserData userData)
	{
		settings.client.RefreshAvatar(userData);
	}

	[Obsolete("Use SteamSettings.client.StoreStatsAndAchievements", false)]
	public void StoreStatsAndAchievements()
	{
		settings.client.StoreStatsAndAchievements();
	}

	[Obsolete("Use SteamSettings.client.achievements", false)]
	public SteamAchievementData GetAchievement(string achievementId)
	{
		if (settings != null)
		{
			if (settings.client.achievements.Exists((SteamAchievementData a) => a.achievementId == achievementId))
			{
				return settings.client.achievements.FirstOrDefault((SteamAchievementData a) => a.achievementId == achievementId);
			}
			return null;
		}
		return null;
	}

	[Obsolete("Use SteamSettings.client.achievements", false)]
	public SteamAchievementData GetAchievement(int achievementIndex)
	{
		if (settings != null)
		{
			if (settings.client.achievements.Count > achievementIndex && achievementIndex > -1)
			{
				return settings.client.achievements[achievementIndex];
			}
			return null;
		}
		return null;
	}

	[Obsolete("Use SteamSettings.client.UnlockAchievementData", false)]
	public void UnlockAchievement(SteamAchievementData achievementData)
	{
		achievementData.Unlock();
	}

	[Obsolete("Use SteamSettings.client.UnlockAchievementData", false)]
	public void UnlockAchievement(string achievementId)
	{
		if (settings != null && settings.client.achievements.Exists((SteamAchievementData a) => a.achievementId == achievementId))
		{
			settings.client.achievements.FirstOrDefault((SteamAchievementData a) => a.achievementId == achievementId).Unlock();
		}
	}

	[Obsolete("Use SteamSettings.client.UnlockAchievementData", false)]
	public void UnlockAchievement(int achievementIndex)
	{
		if (settings != null && settings.client.achievements.Count > achievementIndex && achievementIndex > -1)
		{
			settings.client.achievements[achievementIndex].Unlock();
		}
	}

	[Obsolete("Use SteamSettings.client.achievements", false)]
	public bool IsAchievementAchieved(string achievementId)
	{
		SteamAchievementData achievement = GetAchievement(achievementId);
		if (achievement != null)
		{
			return achievement.isAchieved;
		}
		return false;
	}

	[Obsolete("Use SteamSettings.client.achievements", false)]
	public bool IsAchievementAchieved(int achievementIndex)
	{
		SteamAchievementData achievement = GetAchievement(achievementIndex);
		if (achievement != null)
		{
			return achievement.isAchieved;
		}
		return false;
	}

	[Obsolete("Use SteamSettings.client.achievements", false)]
	public bool AchievementExists(string achievementId)
	{
		return GetAchievement(achievementId) != null;
	}

	[Obsolete("Use SteamSettings.client.achievements", false)]
	public bool AchievementExists(int achievementIndex)
	{
		return GetAchievement(achievementIndex) != null;
	}

	[Obsolete("Use SteamSettings.client.StoreStatsAndAchievements", false)]
	public static void _StoreStatsAndAchievements()
	{
		Instance.settings.client.StoreStatsAndAchievements();
	}

	[Obsolete("Use SteamSettings.client.achievements", false)]
	public static SteamAchievementData _GetAchievement(string achievementId)
	{
		if (Instance != null)
		{
			return Instance.GetAchievement(achievementId);
		}
		return null;
	}

	[Obsolete("Use SteamSettings.client.achievements", false)]
	public static SteamAchievementData _GetAchievement(int achievementIndex)
	{
		if (Instance != null)
		{
			return Instance.GetAchievement(achievementIndex);
		}
		return null;
	}

	[Obsolete("Use SteamSettings.client.UnlockAchievementData", false)]
	public static void _UnlockAchievement(SteamAchievementData achievementData)
	{
		if (Instance != null)
		{
			Instance.UnlockAchievement(achievementData);
		}
	}

	[Obsolete("Use SteamSettings.client.UnlockAchievementData", false)]
	public static void _UnlockAchievement(string achievementId)
	{
		if (Instance != null)
		{
			Instance.UnlockAchievement(achievementId);
		}
	}

	[Obsolete("Use SteamSettings.client.UnlockAchievementData", false)]
	public static void _UnlockAchievement(int achievementIndex)
	{
		if (Instance != null)
		{
			Instance.UnlockAchievement(achievementIndex);
		}
	}

	[Obsolete("Use SteamSettings.client.achievements", false)]
	public static bool _IsAchievementAchieved(string achievementId)
	{
		if (Instance != null)
		{
			return Instance.AchievementExists(achievementId);
		}
		return false;
	}

	[Obsolete("Use SteamSettings.client.achievements", false)]
	public static bool _IsAchievementAchieved(int achievementIndex)
	{
		if (Instance != null)
		{
			return Instance.IsAchievementAchieved(achievementIndex);
		}
		return false;
	}

	[Obsolete("Use SteamSettings.client.achievements", false)]
	public static bool _AchievementExists(string achievementId)
	{
		if (Instance != null)
		{
			return Instance.AchievementExists(achievementId);
		}
		return false;
	}

	[Obsolete("Use SteamSettings.client.achievements", false)]
	public static bool _AchievementExists(int achievementIndex)
	{
		if (Instance != null)
		{
			return Instance.AchievementExists(achievementIndex);
		}
		return false;
	}
}
