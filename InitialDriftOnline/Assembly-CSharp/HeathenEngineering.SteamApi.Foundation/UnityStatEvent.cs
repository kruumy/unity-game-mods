using System;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.Foundation;

[Serializable]
public class UnityStatEvent : UnityEvent<SteamStatData>
{
}
