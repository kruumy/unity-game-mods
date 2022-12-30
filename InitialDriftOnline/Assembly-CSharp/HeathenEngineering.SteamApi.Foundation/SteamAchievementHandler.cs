using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.Foundation;

public class SteamAchievementHandler : MonoBehaviour
{
	public SteamAchievementData achievement;

	public UnityEvent onUnlock;

	private void OnEnable()
	{
		achievement.OnUnlock.AddListener(handleUnlock);
	}

	private void OnDisable()
	{
		achievement.OnUnlock.RemoveListener(handleUnlock);
	}

	private void handleUnlock()
	{
		onUnlock.Invoke();
	}
}
