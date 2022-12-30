using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Mobile/RCC UI Steering Wheel")]
public class RCC_UISteeringWheelController : MonoBehaviour
{
	private RCC_Settings RCCSettingsInstance;

	private GameObject steeringWheelGameObject;

	private Image steeringWheelTexture;

	public float input;

	public float steeringWheelAngle;

	public float steeringWheelMaximumsteerAngle = 270f;

	public float steeringWheelResetPosSpeed = 20f;

	public float steeringWheelCenterDeadZoneRadius = 5f;

	private RectTransform steeringWheelRect;

	private CanvasGroup steeringWheelCanvasGroup;

	private float steeringWheelTempAngle;

	private float steeringWheelNewAngle;

	private bool steeringWheelPressed;

	private Vector2 steeringWheelCenter;

	private Vector2 steeringWheelTouchPos;

	private EventTrigger eventTrigger;

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

	private void Awake()
	{
		steeringWheelTexture = GetComponent<Image>();
	}

	private void Update()
	{
		if (RCCSettings.mobileController == RCC_Settings.MobileController.SteeringWheel)
		{
			SteeringWheelInit();
			SteeringWheelControlling();
			input = GetSteeringWheelInput();
		}
	}

	private void SteeringWheelInit()
	{
		if (!steeringWheelRect || (bool)steeringWheelTexture)
		{
			steeringWheelGameObject = steeringWheelTexture.gameObject;
			steeringWheelRect = steeringWheelTexture.rectTransform;
			steeringWheelCanvasGroup = steeringWheelTexture.GetComponent<CanvasGroup>();
			steeringWheelCenter = steeringWheelRect.position;
			SteeringWheelEventsInit();
		}
	}

	private void SteeringWheelEventsInit()
	{
		eventTrigger = steeringWheelGameObject.GetComponent<EventTrigger>();
		EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
		triggerEvent.AddListener(delegate(BaseEventData data)
		{
			PointerEventData pointerEventData2 = (PointerEventData)data;
			data.Use();
			steeringWheelPressed = true;
			steeringWheelTouchPos = pointerEventData2.position;
			steeringWheelTempAngle = Vector2.Angle(Vector2.up, pointerEventData2.position - steeringWheelCenter);
		});
		eventTrigger.triggers.Add(new EventTrigger.Entry
		{
			callback = triggerEvent,
			eventID = EventTriggerType.PointerDown
		});
		EventTrigger.TriggerEvent triggerEvent2 = new EventTrigger.TriggerEvent();
		triggerEvent2.AddListener(delegate(BaseEventData data)
		{
			PointerEventData pointerEventData = (PointerEventData)data;
			data.Use();
			steeringWheelTouchPos = pointerEventData.position;
		});
		eventTrigger.triggers.Add(new EventTrigger.Entry
		{
			callback = triggerEvent2,
			eventID = EventTriggerType.Drag
		});
		EventTrigger.TriggerEvent triggerEvent3 = new EventTrigger.TriggerEvent();
		triggerEvent3.AddListener(delegate
		{
			steeringWheelPressed = false;
		});
		eventTrigger.triggers.Add(new EventTrigger.Entry
		{
			callback = triggerEvent3,
			eventID = EventTriggerType.EndDrag
		});
	}

	public float GetSteeringWheelInput()
	{
		return Mathf.Round(steeringWheelAngle / steeringWheelMaximumsteerAngle * 100f) / 100f;
	}

	public bool isSteeringWheelPressed()
	{
		return steeringWheelPressed;
	}

	public void SteeringWheelControlling()
	{
		if (!steeringWheelCanvasGroup || !steeringWheelRect || RCCSettings.mobileController != RCC_Settings.MobileController.SteeringWheel)
		{
			if ((bool)steeringWheelGameObject)
			{
				steeringWheelGameObject.SetActive(value: false);
			}
			return;
		}
		if (!steeringWheelGameObject.activeSelf)
		{
			steeringWheelGameObject.SetActive(value: true);
		}
		if (steeringWheelPressed)
		{
			steeringWheelNewAngle = Vector2.Angle(Vector2.up, steeringWheelTouchPos - steeringWheelCenter);
			if (Vector2.Distance(steeringWheelTouchPos, steeringWheelCenter) > steeringWheelCenterDeadZoneRadius)
			{
				if (steeringWheelTouchPos.x > steeringWheelCenter.x)
				{
					steeringWheelAngle += steeringWheelNewAngle - steeringWheelTempAngle;
				}
				else
				{
					steeringWheelAngle -= steeringWheelNewAngle - steeringWheelTempAngle;
				}
			}
			if (steeringWheelAngle > steeringWheelMaximumsteerAngle)
			{
				steeringWheelAngle = steeringWheelMaximumsteerAngle;
			}
			else if (steeringWheelAngle < 0f - steeringWheelMaximumsteerAngle)
			{
				steeringWheelAngle = 0f - steeringWheelMaximumsteerAngle;
			}
			steeringWheelTempAngle = steeringWheelNewAngle;
		}
		else if (!Mathf.Approximately(0f, steeringWheelAngle))
		{
			float num = steeringWheelResetPosSpeed;
			if (Mathf.Abs(num) > Mathf.Abs(steeringWheelAngle))
			{
				steeringWheelAngle = 0f;
				return;
			}
			if (steeringWheelAngle > 0f)
			{
				steeringWheelAngle -= num;
			}
			else
			{
				steeringWheelAngle += num;
			}
		}
		steeringWheelRect.eulerAngles = new Vector3(0f, 0f, 0f - steeringWheelAngle);
	}

	private void OnDisable()
	{
		steeringWheelPressed = false;
		input = 0f;
	}
}
