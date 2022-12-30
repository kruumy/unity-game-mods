using UnityEngine;

namespace SickscoreGames.ExampleScene;

public class ExampleMouseLook : MonoBehaviour
{
	public enum RotationAxes
	{
		MouseX,
		MouseY
	}

	public float sensitivityX = 3f;

	public float sensitivityY = 3f;

	public Vector2 rotationLimitsX = new Vector2(-360f, 360f);

	public Vector2 rotationLimitsY = new Vector2(-60f, 60f);

	public float rotationSmooth = 8f;

	private Quaternion rotationOrigin;

	private float currentRotationX;

	private float currentRotationY;

	public RotationAxes axes;

	private void Awake()
	{
		rotationOrigin = base.transform.localRotation;
	}

	private void Update()
	{
		float axis = Input.GetAxis("Mouse X");
		float axis2 = Input.GetAxis("Mouse Y");
		if (axes == RotationAxes.MouseX)
		{
			currentRotationX += axis * sensitivityX;
			currentRotationX = ClampAngle(currentRotationX, rotationLimitsX.x, rotationLimitsX.y);
			Quaternion quaternion = Quaternion.AngleAxis(currentRotationX, Vector3.up);
			base.transform.localRotation = Quaternion.Lerp(base.transform.localRotation, rotationOrigin * quaternion, rotationSmooth * Time.deltaTime);
		}
		else
		{
			currentRotationY += axis2 * sensitivityY;
			currentRotationY = ClampAngle(currentRotationY, rotationLimitsY.x, rotationLimitsY.y);
			Quaternion quaternion2 = Quaternion.AngleAxis(0f - currentRotationY, Vector3.right);
			base.transform.localRotation = Quaternion.Lerp(base.transform.localRotation, rotationOrigin * quaternion2, rotationSmooth * Time.deltaTime);
		}
	}

	private float ClampAngle(float angle, float min, float max)
	{
		angle %= 360f;
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
}
