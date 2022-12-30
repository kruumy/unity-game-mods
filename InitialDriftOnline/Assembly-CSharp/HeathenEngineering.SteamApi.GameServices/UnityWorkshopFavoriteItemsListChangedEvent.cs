using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.GameServices;

[Serializable]
public class UnityWorkshopFavoriteItemsListChangedEvent : UnityEvent<UserFavoriteItemsListChanged_t>
{
}
