using UnityEngine;
using UnityEngine.EventSystems;

public class MSACCButtons : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	[HideInInspector]
	public float input;

	private bool pressing;

	public void OnPointerDown(PointerEventData eventData)
	{
		pressing = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		pressing = false;
	}

	private void Update()
	{
		if (pressing)
		{
			input += Time.deltaTime * 3f;
		}
		else
		{
			input -= Time.deltaTime * 3f;
		}
		input = Mathf.Clamp(input, 0f, 1f);
	}
}
