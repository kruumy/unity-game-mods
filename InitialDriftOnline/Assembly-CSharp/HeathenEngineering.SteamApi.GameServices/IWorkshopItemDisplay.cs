namespace HeathenEngineering.SteamApi.GameServices;

public interface IWorkshopItemDisplay
{
	HeathenWorkshopReadCommunityItem Data { get; }

	void RegisterData(HeathenWorkshopReadCommunityItem data);
}
