using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
	private Image bar;

	private Text txt;

	public Color AlerteColor = Color.red;

	private Color startColor;

	public float alerte = 25f;

	private float val;

	public float Val
	{
		get
		{
			return val;
		}
		set
		{
			val = value;
			val = Mathf.Clamp(val, 0f, 100f);
			UpdateValue();
		}
	}

	private void Awake()
	{
		bar = base.transform.Find("Bar").GetComponent<Image>();
		txt = bar.transform.Find("Text").GetComponent<Text>();
		startColor = bar.color;
		Val = 100f;
	}

	private void UpdateValue()
	{
		txt.text = (int)val + "%";
		bar.fillAmount = val / 100f;
		if (val <= alerte)
		{
			bar.color = AlerteColor;
		}
		else
		{
			bar.color = startColor;
		}
	}
}
