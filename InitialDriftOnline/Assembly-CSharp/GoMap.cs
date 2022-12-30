using HeathenEngineering.SteamApi.Foundation;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GoMap : MonoBehaviour
{
	public SteamUserData datauser;

	public Text username;

	public InputField usernameIFF;

	public AudioMixer Master;

	public Slider SFX;

	public Slider BGM;

	public Slider MASTER;

	private void Start()
	{
		if (datauser.DisplayName == "" || !datauser)
		{
			string @string = PlayerPrefs.GetString("PLAYERNAMEE");
			username.text = @string;
			usernameIFF.text = @string;
			if (@string == "")
			{
				int num = Random.Range(100, 9999);
				username.text = "PLAYER" + num;
				usernameIFF.text = "PLAYER" + num;
				PlayerPrefs.SetString("PLAYERNAMEE", username.text);
			}
		}
		else
		{
			username.text = datauser.DisplayName;
			usernameIFF.text = datauser.DisplayName;
			PlayerPrefs.SetString("PLAYERNAMEE", datauser.DisplayName);
		}
	}

	public void UpSound(float SFXV, float MASTERV, float BGMV)
	{
		SFX.value = SFXV;
		Master.SetFloat("sfx", Mathf.Log10(SFXV) * 20f);
		BGM.value = BGMV;
		Master.SetFloat("bgm", Mathf.Log10(BGMV) * 20f);
		MASTER.value = MASTERV;
		Master.SetFloat("master", Mathf.Log10(MASTERV) * 20f);
	}
}
