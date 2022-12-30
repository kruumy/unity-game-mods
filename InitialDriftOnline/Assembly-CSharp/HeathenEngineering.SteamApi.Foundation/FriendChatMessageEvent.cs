using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.Foundation;

[Serializable]
public class FriendChatMessageEvent : UnityEvent<SteamUserData, string, EChatEntryType>
{
}
