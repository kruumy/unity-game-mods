using System;
using HeathenEngineering.SteamApi.Foundation.UI;
using HeathenEngineering.Tools;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HeathenEngineering.SteamApi.Networking;

public class SteamworksLobbyChatMessage : HeathenUIBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public SteamUserIconButton PersonaButton;

	public Text Message;

	public DateTime timeStamp;

	public Text timeRecieved;

	public bool ShowStamp = true;

	public bool AllwaysShowStamp;

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
}
