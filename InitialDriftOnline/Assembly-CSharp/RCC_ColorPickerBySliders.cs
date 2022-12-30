using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/RCC Color Picker By UI Sliders")]
public class RCC_ColorPickerBySliders : MonoBehaviour
{
	public Color color;

	public Slider redSlider;

	public Slider greenSlider;

	public Slider blueSlider;

	public void Update()
	{
		color = new Color(redSlider.value, greenSlider.value, blueSlider.value);
	}
}
