using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/RCC UI Slider Text Reader")]
public class RCC_UISliderTextReader : MonoBehaviour
{
	public Slider slider;

	public Text text;

	private void Awake()
	{
		if (!slider)
		{
			slider = GetComponentInParent<Slider>();
		}
		if (!text)
		{
			text = GetComponentInChildren<Text>();
		}
	}

	private void Update()
	{
		if ((bool)slider && (bool)text)
		{
			text.text = slider.value.ToString("F1");
		}
	}
}
