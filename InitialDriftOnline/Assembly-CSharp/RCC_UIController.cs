using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Mobile/RCC UI Controller Button")]
public class RCC_UIController : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	private RCC_Settings RCCSettingsInstance;

	private Button button;

	private Slider slider;

	internal float input;

	public bool pressing;

	private RCC_Settings RCCSettings
	{
		get
		{
			if (RCCSettingsInstance == null)
			{
				RCCSettingsInstance = RCC_Settings.Instance;
				return RCCSettingsInstance;
			}
			return RCCSettingsInstance;
		}
	}

	private float sensitivity => RCCSettings.UIButtonSensitivity;

	private float gravity => RCCSettings.UIButtonGravity;

	private void Awake()
	{
		button = GetComponent<Button>();
		slider = GetComponent<Slider>();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		pressing = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		pressing = false;
	}

	private void OnPress(bool isPressed)
	{
		if (isPressed)
		{
			pressing = true;
		}
		else
		{
			pressing = false;
		}
	}

	private void Update()
	{
		if ((bool)button && !button.interactable)
		{
			pressing = false;
			input = 0f;
			return;
		}
		if ((bool)slider && !slider.interactable)
		{
			pressing = false;
			input = 0f;
			slider.value = 0f;
			return;
		}
		if ((bool)slider)
		{
			if (pressing)
			{
				input = slider.value;
			}
			else
			{
				input = 0f;
			}
			slider.value = input;
		}
		else if (pressing)
		{
			input += Time.deltaTime * sensitivity;
		}
		else
		{
			input -= Time.deltaTime * gravity;
		}
		if (input < 0f)
		{
			input = 0f;
		}
		if (input > 1f)
		{
			input = 1f;
		}
	}

	private void OnDisable()
	{
		input = 0f;
		pressing = false;
	}
}
