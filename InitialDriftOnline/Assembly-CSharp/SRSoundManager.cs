using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SRSoundManager : MonoBehaviour
{
	public AudioMixer Master;

	public Slider SFX;

	public Slider BGM;

	public Slider MASTER;

	public GameObject EurobeatBtn;

	public GameObject PhonkBtn;

	public GameObject MyMusicBtn;

	private void Start()
	{
		if (PlayerPrefs.GetInt("FirstSetSound") == 0)
		{
			PlayerPrefs.SetInt("FirstSetSound", 1);
			PlayerPrefs.SetFloat("Sfxslidervalue", 0.15f);
			PlayerPrefs.SetFloat("Bgmslidervalue", 0.25f);
			PlayerPrefs.SetFloat("Masterslidervalue", 0.54f);
			SFX.value = PlayerPrefs.GetFloat("Sfxslidervalue");
			BGM.value = PlayerPrefs.GetFloat("Bgmslidervalue");
			MASTER.value = PlayerPrefs.GetFloat("Masterslidervalue");
		}
		else
		{
			SFX.value = PlayerPrefs.GetFloat("Sfxslidervalue");
			BGM.value = PlayerPrefs.GetFloat("Bgmslidervalue");
			MASTER.value = PlayerPrefs.GetFloat("Masterslidervalue");
		}
	}

	public void RPCSoundTroll(float SFXV, float MASTERV, float BGMV)
	{
		SFX.value = SFXV;
		Master.SetFloat("sfx", Mathf.Log10(SFXV) * 20f);
		BGM.value = BGMV;
		Master.SetFloat("bgm", Mathf.Log10(BGMV) * 20f);
		MASTER.value = MASTERV;
		Master.SetFloat("master", Mathf.Log10(MASTERV) * 20f);
	}

	private void Update()
	{
	}

	public void SetsfxVolume(float SliderValueE)
	{
		Master.SetFloat("sfx", Mathf.Log10(SliderValueE) * 20f);
		PlayerPrefs.SetFloat("sfxvolume", Mathf.Log10(SliderValueE) * 20f);
		PlayerPrefs.SetFloat("Sfxslidervalue", SliderValueE);
	}

	public void DefineMasterVolume(float SliderValueE)
	{
		Master.SetFloat("master", Mathf.Log10(SliderValueE) * 20f);
		PlayerPrefs.SetFloat("mastervolume", Mathf.Log10(SliderValueE) * 20f);
		PlayerPrefs.SetFloat("Masterslidervalue", SliderValueE);
	}

	public void SetbgmVolume(float SliderValueE)
	{
		Master.SetFloat("bgm", Mathf.Log10(SliderValueE) * 20f);
		PlayerPrefs.SetFloat("bgmvolume", Mathf.Log10(SliderValueE) * 20f);
		PlayerPrefs.SetFloat("Bgmslidervalue", SliderValueE);
	}

	public void SetBtnAnim()
	{
		if (PlayerPrefs.GetString("MyRadio") == "Eurobeat")
		{
			if ((bool)EurobeatBtn)
			{
				EurobeatBtn.GetComponent<Animator>().Play("MusicPlayUI_Running");
			}
			if ((bool)PhonkBtn)
			{
				PhonkBtn.GetComponent<Animator>().Play("MusicPlayUI_Off");
			}
			if ((bool)MyMusicBtn)
			{
				MyMusicBtn.GetComponent<Animator>().Play("MusicPlayUI_Off");
			}
		}
		else if (PlayerPrefs.GetString("MyRadio") == "Phonk" || PlayerPrefs.GetString("MyRadio") == "")
		{
			if ((bool)EurobeatBtn)
			{
				EurobeatBtn.GetComponent<Animator>().Play("MusicPlayUI_Off");
			}
			if ((bool)PhonkBtn)
			{
				PhonkBtn.GetComponent<Animator>().Play("MusicPlayUI_Running");
			}
			if ((bool)MyMusicBtn)
			{
				MyMusicBtn.GetComponent<Animator>().Play("MusicPlayUI_Off");
			}
		}
		else if (PlayerPrefs.GetString("MyRadio") == "MyMusic")
		{
			if ((bool)EurobeatBtn)
			{
				EurobeatBtn.GetComponent<Animator>().Play("MusicPlayUI_Off");
			}
			if ((bool)PhonkBtn)
			{
				PhonkBtn.GetComponent<Animator>().Play("MusicPlayUI_Off");
			}
			if ((bool)MyMusicBtn)
			{
				MyMusicBtn.GetComponent<Animator>().Play("MusicPlayUI_Running");
			}
		}
	}
}
