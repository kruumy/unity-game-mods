using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.GameServices;

[Serializable]
public class UnityWorkshopRemoteStorageUnsubscribePublishedFileResultEvent : UnityEvent<RemoteStorageUnsubscribePublishedFileResult_t>
{
}
