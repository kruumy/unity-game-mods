using System.Collections.Generic;
using HeathenEngineering.CommandSystem;
using HeathenEngineering.Tools;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HeathenEngineering.SteamApi.Networking.UI;

public class SteamworksLobbyChat : HeathenUIBehaviour
{
	[Header("Settings")]
	public SteamworksLobbySettings LobbySettings;

	[Tooltip("Optional, if provided all messages will be tested for a command before they sent to Steam\nAll recieved messages will be tested for commands before they displayed")]
	public CommandParser CommandParser;

	public int maxMessages;

	public bool sendOnKeyCode;

	public KeyCode SendCode = KeyCode.Return;

	[Header("UI Elements")]
	public ScrollRect scrollRect;

	public RectTransform collection;

	public InputField input;

	[Header("Templates")]
	public GameObject selfMessagePrototype;

	public GameObject othersMessagePrototype;

	public GameObject sysMessagePrototype;

	[Header("Events")]
	public UnityEvent NewMessageRecieved;

	[HideInInspector]
	public List<GameObject> messages;

	private void OnEnable()
	{
		if (LobbySettings != null && LobbySettings.Manager != null)
		{
			LobbySettings.OnChatMessageReceived.AddListener(HandleLobbyChatMessage);
			return;
		}
		Debug.LogWarning("Lobby Chat was unable to locate the Lobby Manager, A Heathen Steam Lobby Manager must register the Lobby Settings before this control can initalize.\nIf you have referenced a Lobby Settings object that is registered on a Heathen Lobby Manager then make sure the Heathen Lobby Manager is configured to execute before Lobby Chat.");
		base.enabled = false;
	}

	private void OnDisable()
	{
		if (LobbySettings != null)
		{
			LobbySettings.OnChatMessageReceived.RemoveListener(HandleLobbyChatMessage);
		}
	}

	private void Update()
	{
		if (EventSystem.current.currentSelectedGameObject == input.gameObject && Input.GetKeyDown(SendCode))
		{
			SendChatMessage();
		}
	}

	private void HandleLobbyChatMessage(LobbyChatMessageData data)
	{
		if (CommandParser == null || !CommandParser.TryCallCommand(data.message, userOnly: false, out var _))
		{
			bool num = data.sender.userData.id.m_SteamID != SteamUser.GetSteamID().m_SteamID;
			GameObject gameObject = Object.Instantiate(num ? othersMessagePrototype : selfMessagePrototype, collection);
			gameObject.GetComponent<ILobbyChatMessage>().RegisterChatMessage(data);
			messages.Add(gameObject);
			Canvas.ForceUpdateCanvases();
			if (messages.Count > maxMessages)
			{
				GameObject gameObject2 = messages[0];
				messages.Remove(gameObject2);
				Object.Destroy(gameObject2.gameObject);
			}
			Canvas.ForceUpdateCanvases();
			scrollRect.verticalNormalizedPosition = 0f;
			if (num)
			{
				NewMessageRecieved.Invoke();
			}
		}
	}

	public void ClearMessages()
	{
		while (messages.Count > 0)
		{
			GameObject obj = messages[0];
			messages.RemoveAt(0);
			Object.Destroy(obj);
		}
	}

	public void SendChatMessage(string message)
	{
		if (!LobbySettings.InLobby)
		{
			return;
		}
		string errorMessage = string.Empty;
		if (CommandParser == null || !CommandParser.TryCallCommand(message, userOnly: true, out errorMessage))
		{
			if (!string.IsNullOrEmpty(errorMessage))
			{
				SendSystemMessage("", errorMessage);
				return;
			}
			LobbySettings.SendChatMessage(message);
			input.ActivateInputField();
		}
	}

	public void SendChatMessage()
	{
		if (!string.IsNullOrEmpty(input.text) && LobbySettings.InLobby)
		{
			SendChatMessage(input.text);
			input.text = string.Empty;
		}
		else if (!LobbySettings.InLobby)
		{
			Debug.LogWarning("Attempted to send a lobby chat message without an established connection");
		}
	}

	public void SendSystemMessage(string sender, string message)
	{
		GameObject gameObject = Object.Instantiate(sysMessagePrototype, collection);
		gameObject.GetComponent<ILobbyChatMessage>().SetMessageText(sender, message);
		messages.Add(gameObject);
		Canvas.ForceUpdateCanvases();
		if (messages.Count > maxMessages)
		{
			GameObject gameObject2 = messages[0];
			messages.Remove(gameObject2);
			Object.Destroy(gameObject2.gameObject);
		}
		Canvas.ForceUpdateCanvases();
		scrollRect.verticalNormalizedPosition = 0f;
	}
}
