using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/RCC UI Info Displayer")]
[RequireComponent(typeof(Text))]
public class RCC_InfoLabel : MonoBehaviour
{
	private static RCC_InfoLabel instance;

	private Text text;

	private float timer = 1f;

	public static RCC_InfoLabel Instance
	{
		get
		{
			if (instance == null && (bool)Object.FindObjectOfType<RCC_InfoLabel>())
			{
				instance = Object.FindObjectOfType<RCC_InfoLabel>();
			}
			return instance;
		}
	}

	private void Start()
	{
		text = GetComponent<Text>();
		text.enabled = false;
	}

	private void Update()
	{
		if (timer < 1f)
		{
			if (!text.enabled)
			{
				text.enabled = true;
			}
		}
		else if (text.enabled)
		{
			text.enabled = false;
		}
		timer += Time.deltaTime;
	}

	public void ShowInfo(string info)
	{
		if ((bool)text)
		{
			text.text = info;
			timer = 0f;
		}
	}

	private IEnumerator ShowInfoCo(string info, float time)
	{
		text.enabled = true;
		text.text = info;
		yield return new WaitForSeconds(time);
		text.enabled = false;
	}
}
