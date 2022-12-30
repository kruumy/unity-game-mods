using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Mobile/RCC UI Drag")]
public class RCC_MobileUIDrag : MonoBehaviour, IDragHandler, IEventSystemHandler, IEndDragHandler
{
	private bool isPressing;

	public void OnDrag(PointerEventData data)
	{
		if (RCC_Settings.Instance.controllerType == RCC_Settings.ControllerType.Mobile)
		{
			isPressing = true;
			RCC_SceneManager.Instance.activePlayerCamera.OnDrag(data);
		}
	}

	public void OnEndDrag(PointerEventData data)
	{
		if (RCC_Settings.Instance.controllerType == RCC_Settings.ControllerType.Mobile)
		{
			isPressing = false;
		}
	}
}
