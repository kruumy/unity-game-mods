using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.Foundation;

[Serializable]
public class UnityPersonaStateChangeEvent : UnityEvent<PersonaStateChange_t>
{
}
