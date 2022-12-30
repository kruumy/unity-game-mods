using UnityEngine;

namespace MadGoat.Core.DemoUtils;

public class SimpleCameraRotate : MonoBehaviour
{
	public float speed = 10f;

	public bool showGUI = true;

	private void Update()
	{
		base.transform.Rotate(new Vector3(0f, speed * Time.deltaTime, 0f));
	}

	private void OnGUI()
	{
		if (showGUI)
		{
			GUI.Label(new Rect(20f, Screen.height - 70, 100f, 100f), "Camera speed");
			speed = 0f - GUI.HorizontalSlider(new Rect(20f, Screen.height - 50, 100f, 20f), 0f - speed, 0f, 50f);
		}
	}
}
