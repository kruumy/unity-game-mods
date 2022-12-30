using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace HeathenEngineering.SteamApi.Foundation.UI;

public class SteamUserIconButton : SteamUserFullIcon, IPointerClickHandler, IEventSystemHandler
{
	[FormerlySerializedAs("OnLeftClick")]
	public UnityPersonaEvent onLeftClick;

	[FormerlySerializedAs("OnMiddleClick")]
	public UnityPersonaEvent onMiddleClick;

	[FormerlySerializedAs("OnRightClick")]
	public UnityPersonaEvent onRightClick;

	[FormerlySerializedAs("OnLeftDoubleClick")]
	public UnityPersonaEvent onLeftDoubleClick;

	[FormerlySerializedAs("OnMiddleDoubleClick")]
	public UnityPersonaEvent onMiddleDoubleClick;

	[FormerlySerializedAs("OnRightDoubleClick")]
	public UnityPersonaEvent onRightDoubleClick;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (eventData.clickCount > 1)
			{
				onLeftDoubleClick.Invoke(userData);
			}
			else
			{
				onLeftClick.Invoke(userData);
			}
		}
		else if (eventData.button == PointerEventData.InputButton.Right)
		{
			if (eventData.clickCount > 1)
			{
				onRightDoubleClick.Invoke(userData);
			}
			else
			{
				onRightClick.Invoke(userData);
			}
		}
		else if (eventData.button == PointerEventData.InputButton.Middle)
		{
			if (eventData.clickCount > 1)
			{
				onMiddleDoubleClick.Invoke(userData);
			}
			else
			{
				onMiddleClick.Invoke(userData);
			}
		}
	}
}
