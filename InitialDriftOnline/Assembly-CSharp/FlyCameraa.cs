using UnityEngine;

public class FlyCameraa : MonoBehaviour
{
	public float mainSpeed = 100f;

	public float shiftAdd = 250f;

	public float maxShift = 1000f;

	public float camSens = 0.25f;

	public bool rotateOnlyIfMousedown = true;

	public bool movementStaysFlat = true;

	private Vector3 lastMouse = new Vector3(255f, 255f, 255f);

	private float totalRun = 1f;

	private void Awake()
	{
	}

	private void Update()
	{
		lastMouse = Input.mousePosition - lastMouse;
		lastMouse = new Vector3((0f - lastMouse.y) * camSens, lastMouse.x * camSens, 0f);
		lastMouse = new Vector3(base.transform.eulerAngles.x + lastMouse.x, base.transform.eulerAngles.y + lastMouse.y, 0f);
		base.transform.eulerAngles = lastMouse;
		lastMouse = Input.mousePosition;
	}
}
