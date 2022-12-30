using System;
using HeathenEngineering.SteamApi.Foundation;
using Steamworks;

namespace HeathenEngineering.SteamApi.PlayerServices;

[Serializable]
public class ValveItemDefPromoRule
{
	public ValveItemDefPromoRuleType type = ValveItemDefPromoRuleType.played;

	public AppId_t app;

	public uint minutes;

	public SteamAchievementData achievment;

	public override string ToString()
	{
		return type switch
		{
			ValveItemDefPromoRuleType.manual => "manual", 
			ValveItemDefPromoRuleType.owns => "owns:" + app.ToString(), 
			ValveItemDefPromoRuleType.played => "played:" + app.ToString() + "/" + minutes, 
			ValveItemDefPromoRuleType.achievement => "ach:" + achievment.achievementId, 
			_ => string.Empty, 
		};
	}
}
