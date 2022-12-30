using System;
using Steamworks;

namespace HeathenEngineering.SteamApi.PlayerServices;

[Serializable]
public struct ExchangeItemCount
{
	public SteamItemInstanceID_t InstanceId;

	public uint Quantity;
}
