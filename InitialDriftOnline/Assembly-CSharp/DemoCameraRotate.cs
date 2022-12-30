using UnityEngine;

public class DemoCameraRotate : MonoBehaviour
{
	public float RotationSpeed = 2f;

	private void Update()
	{
		base.transform.Rotate(0f, (0f - RotationSpeed) * Time.deltaTime * 180f, 0f);
	}
}
