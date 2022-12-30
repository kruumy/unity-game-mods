using HeathenEngineering.SteamApi.Foundation;
using Steamworks;

namespace HeathenEngineering.SteamApi.Networking;

public struct LobbyGameServerInformation
{
	public uint ipAddress;

	public ushort port;

	public CSteamID serverId;

	public string StringAddress
	{
		get
		{
			return SteamUtilities.IPUintToString(ipAddress);
		}
		set
		{
			ipAddress = SteamUtilities.IPStringToUint(value);
		}
	}

	public string StringPort
	{
		get
		{
			return port.ToString();
		}
		set
		{
			ushort result = 0;
			if (ushort.TryParse(value, out result))
			{
				port = result;
			}
		}
	}
}
