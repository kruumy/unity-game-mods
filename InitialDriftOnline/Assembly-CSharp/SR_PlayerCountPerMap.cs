using UnityEngine;
using UnityEngine.UI;

public class SR_PlayerCountPerMap : MonoBehaviour
{
	public Text UsuiPlayercount;

	private float lastupdate;

	private float timeago;

	private int iro;

	private int usui;

	private int haruna;

	private int akagi;

	private void Start()
	{
		iro = PlayerPrefs.GetInt("iropcount");
		usui = PlayerPrefs.GetInt("usuipcount");
		haruna = PlayerPrefs.GetInt("harunapcount");
		akagi = PlayerPrefs.GetInt("akagipcount");
		lastupdate = PlayerPrefs.GetFloat("actualtimetimer");
	}

	private void Update()
	{
	}

	public void Updatedata()
	{
		Debug.Log("Mise à jour des stats");
		iro = PlayerPrefs.GetInt("iropcount");
		usui = PlayerPrefs.GetInt("usuipcount");
		haruna = PlayerPrefs.GetInt("harunapcount");
		akagi = PlayerPrefs.GetInt("akagipcount");
		lastupdate = PlayerPrefs.GetFloat("actualtimetimer");
		UsuiPlayercount.text = "IROHAZAKA\n" + iro + "\n\nUSUI\n" + usui + "\n\nHARUNA\n" + haruna + "\n\nakagi\n" + akagi;
	}
}
