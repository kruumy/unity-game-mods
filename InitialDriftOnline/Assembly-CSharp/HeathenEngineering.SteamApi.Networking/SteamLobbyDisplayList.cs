using HeathenEngineering.SteamApi.Foundation;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace HeathenEngineering.SteamApi.Networking;

public class SteamLobbyDisplayList : MonoBehaviour
{
	public SteamSettings steamSettings;

	[FormerlySerializedAs("LobbySettings")]
	public SteamworksLobbySettings lobbySettings;

	[FormerlySerializedAs("Filter")]
	public LobbyHunterFilter filter;

	public LobbyRecordBehvaiour recordPrototype;

	public Transform collection;

	[FormerlySerializedAs("OnSearchStarted")]
	public UnityEvent onSearchStarted;

	[FormerlySerializedAs("OnSearchCompleted")]
	public UnityEvent onSearchCompleted;

	[FormerlySerializedAs("OnLobbySelected")]
	public UnitySteamIdEvent onLobbySelected;

	private void OnEnable()
	{
		if (lobbySettings != null && lobbySettings.Manager != null)
		{
			lobbySettings.OnLobbyMatchList.AddListener(HandleBrowseLobbies);
			return;
		}
		Debug.LogWarning("SteamLobbyDisplayList requires a HeathenSteamLobbySettings reference which has been registered to a HeathenSteamLobbyManager. If you have provided a HeathenSteamLobbySettings that has been applied to an active HeathenSteamLobbyManager then check to insure that the HeathenSteamLobbyManager has initalized before this control.");
		base.enabled = false;
	}

	private void OnDisable()
	{
		if (lobbySettings != null && lobbySettings.Manager != null)
		{
			lobbySettings.OnLobbyMatchList.RemoveListener(HandleBrowseLobbies);
		}
	}

	private void HandleBrowseLobbies(SteamLobbyLobbyList lobbies)
	{
		foreach (LobbyHunterLobbyRecord lobby in lobbies)
		{
			LobbyRecordBehvaiour component = Object.Instantiate(recordPrototype.gameObject, collection).GetComponent<LobbyRecordBehvaiour>();
			component.SetLobby(lobby, lobbySettings);
			component.OnSelected.AddListener(HandleOnSelected);
		}
		onSearchCompleted.Invoke();
	}

	private void HandleOnSelected(CSteamID lobbyId)
	{
		onLobbySelected.Invoke(lobbyId);
	}

	public void QuickMatch()
	{
		onSearchStarted.Invoke();
		lobbySettings.Manager.QuickMatch(filter, steamSettings.client.userData.DisplayName, autoCreate: true);
	}

	public void QuickMatch(string onCreateName)
	{
		onSearchStarted.Invoke();
		lobbySettings.Manager.QuickMatch(filter, onCreateName, autoCreate: true);
	}

	public void BrowseLobbies()
	{
		onSearchStarted.Invoke();
		lobbySettings.Manager.FindMatch(filter);
	}

	public void ClearLobbies()
	{
		while (collection.childCount > 0)
		{
			Transform child = collection.GetChild(0);
			GameObject gameObject = child.gameObject;
			gameObject.SetActive(value: false);
			child.parent = null;
			Object.Destroy(gameObject);
		}
	}
}
