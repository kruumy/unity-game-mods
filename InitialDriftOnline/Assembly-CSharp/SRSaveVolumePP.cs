using UnityEngine;
using UnityEngine.Audio;

public class SRSaveVolumePP : MonoBehaviour
{
	public AudioMixer Master;

	private void Start()
	{
		if (PlayerPrefs.GetInt("FirstSetSound") == 0)
		{
			Master.SetFloat("master", -5f);
			PlayerPrefs.SetFloat("mastervolume", -5f);
			Master.SetFloat("sfx", -16f);
			PlayerPrefs.SetFloat("sfxvolume", -16f);
			Master.SetFloat("bgm", -12f);
			PlayerPrefs.SetFloat("bgmvolume", -12f);
		}
		else
		{
			Master.SetFloat("sfx", PlayerPrefs.GetFloat("sfxvolume"));
			Master.SetFloat("bgm", PlayerPrefs.GetFloat("bgmvolume"));
			Master.SetFloat("master", PlayerPrefs.GetFloat("mastervolume"));
		}
	}

	private void Update()
	{
	}
}
