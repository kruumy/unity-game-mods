using System;

namespace HeathenEngineering.SteamApi.PlayerServices;

[Serializable]
public class ValveItemDefInventoryItemTag
{
	public string category;

	public string tag;

	public override string ToString()
	{
		return category + ":" + tag;
	}
}
