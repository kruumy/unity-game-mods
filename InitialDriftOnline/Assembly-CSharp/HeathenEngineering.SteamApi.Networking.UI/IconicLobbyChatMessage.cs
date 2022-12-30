using System;
using HeathenEngineering.SteamApi.Foundation.UI;
using HeathenEngineering.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HeathenEngineering.SteamApi.Networking.UI;

public class IconicLobbyChatMessage : HeathenUIBehaviour, ILobbyChatMessage
{
	public SteamUserIconButton PersonaButton;

	public Text Message;

	public DateTime timeStamp;

	public Text timeRecieved;

	public string timeFormat = "HH:mm:ss";

	public bool ShowStamp = true;

	public bool AllwaysShowStamp;

	[HideInInspector]
	public LobbyChatMessageData data;

	private bool processing;

	private int siblingIndex = -1;

	private void Update()
	{
		if (processing)
		{
			int num = base.selfTransform.GetSiblingIndex();
			if (num != siblingIndex)
			{
				siblingIndex = num;
				UpdatePersonaIconShow();
			}
		}
	}

	private void UpdatePersonaIconShow()
	{
		if (data == null || data.sender == null)
		{
			return;
		}
		if (siblingIndex == 0)
		{
			PersonaButton.gameObject.SetActive(value: true);
			return;
		}
		IconicLobbyChatMessage component = base.selfTransform.parent.GetChild(siblingIndex - 1).gameObject.GetComponent<IconicLobbyChatMessage>();
		if (component.data != null && component.data.sender != null && component.data.sender.userData.id == data.sender.userData.id)
		{
			PersonaButton.gameObject.SetActive(value: false);
		}
		else
		{
			PersonaButton.gameObject.SetActive(value: true);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (ShowStamp && !timeRecieved.gameObject.activeSelf)
		{
			timeRecieved.gameObject.SetActive(value: true);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!AllwaysShowStamp && timeRecieved.gameObject.activeSelf)
		{
			timeRecieved.gameObject.SetActive(value: false);
		}
	}

	public void RegisterChatMessage(LobbyChatMessageData data)
	{
		this.data = data;
		PersonaButton.gameObject.SetActive(value: true);
		PersonaButton.LinkSteamUser(data.sender.userData);
		Message.text = data.message;
		timeStamp = data.recievedTime;
		timeRecieved.text = timeStamp.ToString(timeFormat);
		if (ShowStamp && AllwaysShowStamp)
		{
			timeRecieved.gameObject.SetActive(value: true);
		}
		else
		{
			timeRecieved.gameObject.SetActive(value: false);
		}
		siblingIndex = base.selfTransform.GetSiblingIndex();
		UpdatePersonaIconShow();
		processing = true;
	}

	public void SetMessageText(string sender, string message)
	{
		PersonaButton.gameObject.SetActive(value: false);
		Message.text = message;
		timeStamp = DateTime.Now;
		timeRecieved.text = timeStamp.ToString(timeFormat);
		if (ShowStamp && AllwaysShowStamp)
		{
			timeRecieved.gameObject.SetActive(value: true);
		}
		else
		{
			timeRecieved.gameObject.SetActive(value: false);
		}
		processing = true;
	}
}
