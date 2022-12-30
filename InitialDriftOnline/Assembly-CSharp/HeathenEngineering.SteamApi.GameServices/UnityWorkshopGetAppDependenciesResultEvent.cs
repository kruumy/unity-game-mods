using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.GameServices;

[Serializable]
public class UnityWorkshopGetAppDependenciesResultEvent : UnityEvent<GetAppDependenciesResult_t>
{
}
