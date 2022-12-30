using System.Collections;
using UnityEngine;

public class ImporterExample : MonoBehaviour
{
	public Browser browser;

	public AudioImporter importer;

	public AudioSource audioSource;

	private string path;

	private string currentDirectory;

	private void Awake()
	{
		if (!browser)
		{
			browser = Object.FindObjectOfType<Browser>();
		}
		if (!audioSource)
		{
			audioSource = Object.FindObjectOfType<SI_Music>().gameObject.GetComponent<AudioSource>();
		}
		browser.FileSelected += OnFileSelected;
		if (PlayerPrefs.GetString("currentDirectory") == "")
		{
			path = Application.dataPath;
			path = path.Replace("/Initial Drift_Data", "/MY_MUSIC");
			path = path.Replace("RCC RETRO MULTI/Assets", "MY_MUSIC");
			PlayerPrefs.SetString("currentDirectory", path);
		}
	}

	public void OnFileSelected(string path)
	{
		if (!audioSource)
		{
			audioSource = Object.FindObjectOfType<SI_Music>().gameObject.GetComponent<AudioSource>();
		}
		if ((bool)audioSource.clip)
		{
			Object.Destroy(audioSource.clip);
		}
		StartCoroutine(Import(path));
		Debug.Log("OnFileSelected, PATH : " + path);
		PlayerPrefs.SetString("MyRadio", "MyMusic");
	}

	public void OnFileSelectedNewByMagic(string path)
	{
		path = PlayerPrefs.GetString("currentDirectory");
		Object.Destroy(audioSource.clip);
		Debug.Log("OnFileSelected, PATH : " + path);
		PlayerPrefs.SetString("MyRadio", "MyMusic");
	}

	private IEnumerator Import(string path)
	{
		if (!browser)
		{
			browser = Object.FindObjectOfType<Browser>();
		}
		yield return new WaitForSeconds(0.3f);
		importer.Import(path);
		while (!importer.isInitialized && !importer.isError)
		{
			yield return null;
		}
		if (importer.isError)
		{
			Debug.LogError(importer.error);
		}
		if (!audioSource)
		{
			audioSource = Object.FindObjectOfType<SI_Music>().gameObject.GetComponent<AudioSource>();
		}
		audioSource.clip = importer.audioClip;
		audioSource.Play();
		yield return new WaitForSeconds(0.2f);
		Object.FindObjectOfType<SI_Music>().LaunchCountByImporter();
	}
}
