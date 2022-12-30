using System.Collections;
using System.Collections.Generic;
using System.IO;
using CodeStage.AntiCheat.Storage;
using UnityEngine;
using UnityEngine.UI;

public class SI_Music : MonoBehaviour
{
	private AudioSource audioSource;

	public AudioClip[] EurobeatRadio;

	public AudioClip[] PhonkRadio;

	public GameObject UIMessageTop;

	public List<string> ObjectsList = new List<string>(0);

	private static SI_Music instance;

	public void Awake()
	{
		if (instance == null)
		{
			instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void Start()
	{
		ListAllMp3(0);
		if ((bool)Object.FindObjectOfType<SRSoundManager>())
		{
			Object.FindObjectOfType<SRSoundManager>().SetBtnAnim();
		}
		if (!UIMessageTop)
		{
			UIMessageTop = GameObject.FindGameObjectWithTag("UIMessageTop");
		}
		audioSource = GetComponent<AudioSource>();
		if (PlayerPrefs.GetString("MyRadio") == "Eurobeat")
		{
			audioSource.clip = EurobeatRadio[Random.Range(0, EurobeatRadio.Length)];
		}
		else if (PlayerPrefs.GetString("MyRadio") == "Phonk" || PlayerPrefs.GetString("MyRadio") == "")
		{
			audioSource.clip = PhonkRadio[Random.Range(0, PhonkRadio.Length)];
		}
		else if (PlayerPrefs.GetString("MyRadio") == "MyMusic")
		{
			Debug.Log("GENRATION D'UNE MUSIC");
			if (ObjectsList.Count != 0)
			{
				string path = ObjectsList[Random.Range(0, ObjectsList.Count)];
				Object.FindObjectOfType<ImporterExample>().OnFileSelected(path);
			}
			else
			{
				audioSource.clip = EurobeatRadio[Random.Range(0, EurobeatRadio.Length)];
			}
		}
		audioSource.Play();
		if ((bool)UIMessageTop && PlayerPrefs.GetString("MyRadio") != "MyMusic")
		{
			UIMessageTop.GetComponent<Animator>().Play("Reset");
			UIMessageTop.GetComponent<Text>().text = audioSource.clip.name;
			StopAllCoroutines();
			StartCoroutine(Change());
			StartCoroutine(UITXT());
		}
		StartCoroutine(DPAPTEMPO());
	}

	private void Update()
	{
		if (!UIMessageTop)
		{
			UIMessageTop = GameObject.FindGameObjectWithTag("UIMessageTop");
		}
		string @string = PlayerPrefs.GetString("ControllerTypeChoose");
		if (Input.GetKeyDown(KeyCode.M) && ObscuredPrefs.GetInt("ONTYPING") == 0)
		{
			UIMessageTop.GetComponent<Text>().text = "";
			Start();
		}
		int @int = PlayerPrefs.GetInt("TEMPODPAD");
		if ((Input.GetAxis("Xbox_DPadVertical") == -1f && @int == 0 && @string == "Xbox360One") || (Input.GetAxis("Xbox_DPadVertical") == -1f && @int == 0 && @string == "LogitechSteeringWheel"))
		{
			PlayerPrefs.SetInt("TEMPODPAD", 10);
			UIMessageTop.GetComponent<Text>().text = "";
			Start();
		}
		if (Input.GetAxis("PS4_DPadHorizontal") == 1f && @int == 0 && @string == "PS4")
		{
			PlayerPrefs.SetInt("TEMPODPAD", 10);
			UIMessageTop.GetComponent<Text>().text = "";
			Start();
		}
	}

	private IEnumerator UITXT()
	{
		yield return new WaitForSeconds(0.3f);
		if ((bool)UIMessageTop)
		{
			UIMessageTop.GetComponent<Animator>().Play("UIMessage");
		}
	}

	private IEnumerator DPAPTEMPO()
	{
		yield return new WaitForSeconds(0.7f);
		PlayerPrefs.SetInt("TEMPODPAD", 0);
	}

	public void LaunchCountByImporter()
	{
		if ((bool)UIMessageTop)
		{
			UIMessageTop.GetComponent<Animator>().Play("Reset");
			UIMessageTop.GetComponent<Text>().text = audioSource.clip.name;
			StopAllCoroutines();
			StartCoroutine(Change());
			StartCoroutine(UITXT());
		}
		if ((bool)Object.FindObjectOfType<SRSoundManager>())
		{
			Object.FindObjectOfType<SRSoundManager>().SetBtnAnim();
		}
		StartCoroutine(DPAPTEMPO());
	}

	private IEnumerator Change()
	{
		yield return new WaitForSeconds(audioSource.clip.length);
		Start();
	}

	public void SetPhonk()
	{
		PlayerPrefs.SetString("MyRadio", "Phonk");
		Start();
	}

	public void SetEurobeat()
	{
		PlayerPrefs.SetString("MyRadio", "Eurobeat");
		Start();
	}

	public void ListAllMp3(int lol)
	{
		string @string = PlayerPrefs.GetString("currentDirectory");
		ObjectsList.Clear();
		int num = 0;
		string[] files = Directory.GetFiles(@string);
		foreach (string text in files)
		{
			if (text.Substring(text.Length - (text.Length - (text.Length - 3))) == "mp3")
			{
				num++;
				ObjectsList.Add(text ?? "");
			}
		}
		if (num == 0 && lol == 1)
		{
			Debug.Log("ERROROROROROR");
			GameObject obj = GameObject.FindGameObjectWithTag("MyMusicBtn");
			obj.GetComponentInChildren<Text>().color = new Color32(204, 0, 0, byte.MaxValue);
			obj.GetComponentInChildren<Text>().text = "NO MUSIC ->";
			StartCoroutine(NoMusicInFolder());
		}
		else if (lol == 1)
		{
			PlayerPrefs.SetString("MyRadio", "MyMusic");
			Start();
		}
	}

	private IEnumerator NoMusicInFolder()
	{
		yield return new WaitForSeconds(1f);
		GameObject obj = GameObject.FindGameObjectWithTag("MyMusicBtn");
		obj.GetComponentInChildren<Text>().text = "MY MUSIC";
		obj.GetComponentInChildren<Text>().color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
	}
}
