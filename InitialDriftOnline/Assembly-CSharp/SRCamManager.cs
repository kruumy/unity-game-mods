using UnityEngine;

public class SRCamManager : MonoBehaviour
{
	public GameObject FromAkina;

	public GameObject FromAkagi;

	public GameObject FromUSUI;

	private void Start()
	{
		if (PlayerPrefs.GetString("WhereYouFrom") == "AKAGI")
		{
			FromAkagi.SetActive(value: true);
			FromAkina.SetActive(value: false);
			FromUSUI.SetActive(value: false);
		}
		else if (PlayerPrefs.GetString("WhereYouFrom") == "AKINA")
		{
			FromAkina.SetActive(value: true);
			FromAkagi.SetActive(value: false);
			FromUSUI.SetActive(value: false);
		}
		else if (PlayerPrefs.GetString("WhereYouFrom") == "USUI")
		{
			FromAkina.SetActive(value: false);
			FromAkagi.SetActive(value: false);
			FromUSUI.SetActive(value: true);
		}
	}

	private void Update()
	{
	}
}
