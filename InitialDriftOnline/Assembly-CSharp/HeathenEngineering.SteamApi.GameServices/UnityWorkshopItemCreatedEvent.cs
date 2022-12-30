using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.GameServices;

[Serializable]
public class UnityWorkshopItemCreatedEvent : UnityEvent<CreateItemResult_t>
{
}
