using System;
using System.Collections.Generic;
using Steamworks;

namespace HeathenEngineering.SteamApi.Networking;

[Serializable]
public struct LobbyHunterFilter
{
	public int maxResults;

	public bool useDistanceFilter;

	public ELobbyDistanceFilter distanceOption;

	public bool useSlotsAvailable;

	public int requiredOpenSlots;

	public List<LobbyHunterNearFilter> nearValues;

	public List<LobbyHunterNumericFilter> numberValues;

	public List<LobbyHunterStringFilter> stringValues;
}
