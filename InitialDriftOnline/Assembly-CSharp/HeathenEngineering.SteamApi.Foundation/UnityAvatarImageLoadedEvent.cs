using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.Foundation;

[Serializable]
public class UnityAvatarImageLoadedEvent : UnityEvent<AvatarImageLoaded_t>
{
}
