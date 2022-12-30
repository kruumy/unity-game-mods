using System;

namespace HeathenEngineering.SteamApi.PlayerServices;

[Serializable]
public class TagGeneratorValue
{
	public string name;

	public int weight;

	public override string ToString()
	{
		return name + ":" + weight;
	}
}
