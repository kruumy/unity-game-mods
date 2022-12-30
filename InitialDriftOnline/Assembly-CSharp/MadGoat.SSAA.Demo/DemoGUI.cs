using UnityEngine;

namespace MadGoat.SSAA.Demo;

public class DemoGUI : MonoBehaviour
{
	private MadGoatSSAA ssaa;

	private bool mode;

	private bool ultra;

	private float multiplier = 100f;

	private GUIStyle s;

	private float deltaTime;

	private void Start()
	{
		ssaa = Object.FindObjectOfType<MadGoatSSAA>();
	}

	private void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	}

	private void OnGUI()
	{
		GUI.contentColor = new Color(0f, 0f, 0f);
		float num = deltaTime * 1000f;
		float num2 = 1f / deltaTime;
		string text = $"{num:0.0} ms ({num2:0.} fps)";
		GUI.Label(new Rect(Screen.width - 150, 10f, 150f, 50f), text);
		if (GUI.Button(new Rect(20f, 10f, 120f, 20f), (!mode) ? "Switch to scaling" : "Switch to ssaa"))
		{
			mode = !mode;
		}
		if (mode)
		{
			GUI.Label(new Rect(20f, 50f, 100f, 20f), (int)multiplier + "%");
			multiplier = GUI.HorizontalSlider(new Rect(55f, 55f, 100f, 20f), multiplier, 50f, 200f);
			ssaa.SetAsScale((int)multiplier, Filter.BICUBIC, 0.8f, 0.7f);
		}
		else
		{
			if (GUI.Button(new Rect(20f, 50f, 80f, 20f), "off"))
			{
				ssaa.SetAsSSAA(SSAAMode.SSAA_OFF);
			}
			if (GUI.Button(new Rect(20f, 75f, 80f, 20f), "x0.5"))
			{
				ssaa.SetAsSSAA(SSAAMode.SSAA_HALF);
			}
			if (GUI.Button(new Rect(20f, 100f, 80f, 20f), "x2"))
			{
				ssaa.SetAsSSAA(SSAAMode.SSAA_X2);
			}
			if (GUI.Button(new Rect(20f, 125f, 80f, 20f), "x4"))
			{
				ssaa.SetAsSSAA(SSAAMode.SSAA_X4);
			}
		}
		GUI.contentColor = new Color(0f, 0f, 0f);
		ultra = GUI.Toggle(new Rect(20f, 150f, 150f, 20f), ultra, "Ultra Quality (FSSAA)");
		ssaa.SetPostAAMode(ultra ? PostAntiAliasingMode.FSSAA : PostAntiAliasingMode.Off);
	}
}
