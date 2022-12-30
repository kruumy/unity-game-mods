using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SRMessageOther : MonoBehaviour
{
	public string UIMessagee;

	public TextMeshPro tmpingame;

	private void Start()
	{
		if (PlayerPrefs.GetInt("CHUTESOL") == 0)
		{
			GetComponent<Text>().text = UIMessagee;
			GetComponent<Animator>().Play("UIMessage");
		}
	}

	private void Update()
	{
		tmpingame = Object.FindObjectOfType<TextMeshPro>();
	}

	public void PlayText()
	{
		GetComponent<Text>().text = UIMessagee;
		GetComponent<Animator>().Play("UIMessage");
	}
}
