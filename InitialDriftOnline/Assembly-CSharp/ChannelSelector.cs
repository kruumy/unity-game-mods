using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChannelSelector : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public string Channel;

	public void SetChannel(string channel)
	{
		Channel = channel;
		GetComponentInChildren<Text>().text = Channel;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Object.FindObjectOfType<ChatGui>().ShowChannel(Channel);
	}
}
