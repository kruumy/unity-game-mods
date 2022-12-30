using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Mobile/RCC UI Joystick")]
public class RCC_UIJoystick : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerUpHandler, IPointerDownHandler
{
	public RectTransform backgroundSprite;

	public RectTransform handleSprite;

	internal Vector2 inputVector = Vector2.zero;

	private Vector2 joystickPosition = Vector2.zero;

	private Camera _refCam = new Camera();

	public float inputHorizontal => inputVector.x;

	public float inputVertical => inputVector.y;

	private void Start()
	{
		joystickPosition = RectTransformUtility.WorldToScreenPoint(_refCam, backgroundSprite.position);
	}

	public void OnDrag(PointerEventData eventData)
	{
		Vector2 vector = eventData.position - joystickPosition;
		inputVector = ((vector.magnitude > backgroundSprite.sizeDelta.x / 2f) ? vector.normalized : (vector / (backgroundSprite.sizeDelta.x / 2f)));
		handleSprite.anchoredPosition = inputVector * backgroundSprite.sizeDelta.x / 2f * 1f;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		inputVector = Vector2.zero;
		handleSprite.anchoredPosition = Vector2.zero;
	}

	public virtual void OnPointerDown(PointerEventData eventData)
	{
	}
}
