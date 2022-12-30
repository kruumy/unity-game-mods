using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.Foundation;

[Serializable]
[CreateAssetMenu(menuName = "Steamworks/Foundation/User Data")]
public class SteamUserData : ScriptableObject
{
	public CSteamID id;

	[NonSerialized]
	public bool iconLoaded;

	[NonSerialized]
	public Texture2D avatar;

	public UnityEvent OnAvatarLoaded = new UnityEvent();

	public UnityEvent OnAvatarChanged = new UnityEvent();

	public UnityEvent OnNameChanged = new UnityEvent();

	public UnityEvent OnStateChange = new UnityEvent();

	public UnityEvent OnComeOnline = new UnityEvent();

	public UnityEvent OnGoneOffline = new UnityEvent();

	public UnityEvent OnGameChanged = new UnityEvent();

	[Obsolete("Please use id instead, this member will be removed in later updates", false)]
	public CSteamID SteamId
	{
		get
		{
			return id;
		}
		set
		{
			id = value;
		}
	}

	[Obsolete("Please use iconLoaded instead, this member will be removed in later updates", false)]
	public bool IconLoaded
	{
		get
		{
			return iconLoaded;
		}
		set
		{
			iconLoaded = value;
		}
	}

	[Obsolete("Please use avatar instead, this member will be removed in later updates", false)]
	public Texture2D Avatar
	{
		get
		{
			return avatar;
		}
		set
		{
			avatar = value;
		}
	}

	public string DisplayName => SteamFriends.GetFriendPersonaName(id);

	public EPersonaState State => SteamFriends.GetFriendPersonaState(id);

	public bool InGame
	{
		get
		{
			FriendGameInfo_t pFriendGameInfo;
			return SteamFriends.GetFriendGamePlayed(id, out pFriendGameInfo);
		}
	}

	public FriendGameInfo_t GameInfo
	{
		get
		{
			SteamFriends.GetFriendGamePlayed(id, out var pFriendGameInfo);
			return pFriendGameInfo;
		}
	}

	public int Level => SteamFriends.GetFriendSteamLevel(id);

	public string GetRichPresenceValue(string key)
	{
		return SteamFriends.GetFriendRichPresence(id, key);
	}

	public Dictionary<string, string> GetRichPresenceValues()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		int friendRichPresenceKeyCount = SteamFriends.GetFriendRichPresenceKeyCount(id);
		for (int i = 0; i < friendRichPresenceKeyCount; i++)
		{
			string friendRichPresenceKeyByIndex = SteamFriends.GetFriendRichPresenceKeyByIndex(id, i);
			dictionary.Add(friendRichPresenceKeyByIndex, SteamFriends.GetFriendRichPresence(id, friendRichPresenceKeyByIndex));
		}
		return dictionary;
	}

	public void ClearData()
	{
		id = default(CSteamID);
		iconLoaded = false;
		avatar = null;
	}

	public void OpenChat()
	{
		SteamFriends.ActivateGameOverlayToUser("Chat", id);
	}

	public void OpenProfile()
	{
		SteamFriends.ActivateGameOverlayToUser("steamid", id);
	}

	public void OpenTrade()
	{
		SteamFriends.ActivateGameOverlayToUser("jointrade", id);
	}

	public void OpenStats()
	{
		SteamFriends.ActivateGameOverlayToUser("stats", id);
	}

	public void OpenAchievements()
	{
		SteamFriends.ActivateGameOverlayToUser("achievements", id);
	}

	public void OpenFriendAdd()
	{
		SteamFriends.ActivateGameOverlayToUser("friendadd", id);
	}

	public void OpenFriendRemove()
	{
		SteamFriends.ActivateGameOverlayToUser("friendremove", id);
	}

	public void OpenRequestAccept()
	{
		SteamFriends.ActivateGameOverlayToUser("friendrequestaccept", id);
	}

	public void OpenRequestIgnore()
	{
		SteamFriends.ActivateGameOverlayToUser("friendrequestignore", id);
	}

	public bool SendMessage(string message)
	{
		return SteamFriends.ReplyToFriendMessage(id, message);
	}
}
