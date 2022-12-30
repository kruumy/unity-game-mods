using UnityEngine;
using UnityEngine.EventSystems;

public class RCC_CameraCarSelection : MonoBehaviour
{
	public Transform target;

	public float distance = 10f;

	public float xSpeed = 250f;

	public float ySpeed = 120f;

	public float yMinLimit = -20f;

	public float yMaxLimit = 80f;

	private float x;

	private float y;

	private bool selfTurn = true;

	private float selfTurnTime;

	private void Start()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		x = eulerAngles.y;
		y = eulerAngles.x;
	}

	private void LateUpdate()
	{
		if ((bool)target)
		{
			if (selfTurn)
			{
				x += xSpeed / 2f * Time.deltaTime;
			}
			y = ClampAngle(y, yMinLimit, yMaxLimit);
			Quaternion quaternion = Quaternion.Euler(y, x, 0f);
			Vector3 position = quaternion * new Vector3(0f, 0f, 0f - distance) + target.position;
			base.transform.rotation = quaternion;
			base.transform.position = position;
			if (selfTurnTime <= 1f)
			{
				selfTurnTime += Time.deltaTime;
			}
			if (selfTurnTime >= 1f)
			{
				selfTurn = true;
			}
		}
	}

	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	public void OnDrag(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		x += pointerEventData.delta.x * xSpeed * 0.02f;
		y -= pointerEventData.delta.y * ySpeed * 0.02f;
		y = ClampAngle(y, yMinLimit, yMaxLimit);
		Quaternion quaternion = Quaternion.Euler(y, x, 0f);
		Vector3 position = quaternion * new Vector3(0f, 0f, 0f - distance) + target.position;
		base.transform.rotation = quaternion;
		base.transform.position = position;
		selfTurn = false;
		selfTurnTime = 0f;
	}
}
