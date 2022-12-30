using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class MSACCJoystick : UIBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler, IDragHandler
{
	public RectTransform _joystickGraphic;

	private Vector2 _axis;

	private bool _isDragging;

	[HideInInspector]
	public float joystickY;

	[HideInInspector]
	public float joystickX;

	private RectTransform _rectTransform;

	public RectTransform rectTransform
	{
		get
		{
			if (!_rectTransform)
			{
				_rectTransform = base.transform as RectTransform;
			}
			return _rectTransform;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (IsActive())
		{
			EventSystem.current.SetSelectedGameObject(base.gameObject, eventData);
			Vector2 axisMS = base.transform.InverseTransformPoint(eventData.position);
			axisMS.x /= rectTransform.sizeDelta.x * 0.5f;
			axisMS.y /= rectTransform.sizeDelta.y * 0.5f;
			SetAxisMS(axisMS);
			_isDragging = true;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		_isDragging = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out _axis);
		_axis.x /= rectTransform.sizeDelta.x * 0.5f;
		_axis.y /= rectTransform.sizeDelta.y * 0.5f;
		SetAxisMS(_axis);
	}

	private void OnDeselect()
	{
		_isDragging = false;
	}

	private void LateUpdate()
	{
		if (!_isDragging && _axis != Vector2.zero)
		{
			Vector2 axisMS = _axis - _axis * Time.deltaTime * 25f;
			if (axisMS.sqrMagnitude <= 0.1f)
			{
				axisMS = Vector2.zero;
			}
			SetAxisMS(axisMS);
		}
	}

	public void SetAxisMS(Vector2 axis)
	{
		_axis = Vector2.ClampMagnitude(axis, 1f);
		UpdateJoystickGraphicMS();
		joystickY = _axis.y;
		joystickX = _axis.x;
	}

	private void UpdateJoystickGraphicMS()
	{
		if ((bool)_joystickGraphic)
		{
			_joystickGraphic.localPosition = _axis * Mathf.Max(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y) * 0.5f;
		}
	}
}
