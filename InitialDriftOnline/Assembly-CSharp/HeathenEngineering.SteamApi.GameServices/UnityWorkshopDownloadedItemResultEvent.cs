using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.GameServices;

[Serializable]
public class UnityWorkshopDownloadedItemResultEvent : UnityEvent<DownloadItemResult_t>
{
}
