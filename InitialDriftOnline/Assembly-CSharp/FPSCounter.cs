using UnityEngine;

public class FPSCounter : MonoBehaviour
{
	public bool SetTargetFrameRate = true;

	private float deltaTime;

	private void Start()
	{

    }

	private void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

	private void OnGUI()
	{
		int width = Screen.width;
		int height = Screen.height;
		GUIStyle gUIStyle = new GUIStyle();
		Rect position = new Rect(0f, 0f, width, height * 2 / 100);
		gUIStyle.alignment = TextAnchor.UpperLeft;
		gUIStyle.fontSize = height * 2 / 100;
		gUIStyle.normal.textColor = new Color(1f, 1f, 1f, 1f);
		float f = 1f / deltaTime;
		GUI.Label(position, Mathf.RoundToInt(f) + " fps", gUIStyle);
	}
}
