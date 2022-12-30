using UnityEngine;

public class FreeCamera : MonoBehaviour
{
	[SerializeField]
	private float MoveSpeed;

	[SerializeField]
	private float MoveAccelerationSpeed;

	[SerializeField]
	private float RotateSpeed;

	private Vector3 Velocity;

	private Vector3 MousePos;

	private void Update()
	{
		Velocity.z = Mathf.MoveTowards(Velocity.z, Input.GetAxis("Vertical") * MoveSpeed, MoveAccelerationSpeed * Time.deltaTime);
		Velocity.x = Mathf.MoveTowards(Velocity.x, Input.GetAxis("Horizontal") * MoveSpeed, MoveAccelerationSpeed * Time.deltaTime);
		Velocity.y = Mathf.MoveTowards(Velocity.y, (float)(Input.GetKey(KeyCode.E) ? 1 : (Input.GetKey(KeyCode.Q) ? (-1) : 0)) * MoveSpeed * 0.3f, MoveAccelerationSpeed * Time.deltaTime);
		base.transform.position += base.transform.TransformDirection(Velocity);
		_ = Input.mousePosition - MousePos;
		MousePos = Input.mousePosition;
		base.transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * RotateSpeed, Vector3.left);
		base.transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotateSpeed, Vector3.up);
		Vector3 eulerAngles = base.transform.eulerAngles;
		eulerAngles.z = 0f;
		base.transform.eulerAngles = eulerAngles;
		if (Input.GetKey(KeyCode.LeftAlt))
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
		else
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
		Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView + Input.mouseScrollDelta.y, 5f, 150f);
	}
}
