using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RCC_AIO : MonoBehaviour
{
	private static RCC_AIO instance;

	public GameObject levels;

	public GameObject back;

	private AsyncOperation async;

	public Slider slider;

	private void Start()
	{
		if ((bool)instance)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Update()
	{
		if (async != null && !async.isDone)
		{
			if (!slider.gameObject.activeSelf)
			{
				slider.gameObject.SetActive(value: true);
			}
			slider.value = async.progress;
		}
		else if (slider.gameObject.activeSelf)
		{
			slider.gameObject.SetActive(value: false);
		}
	}

	public void LoadLevel(string levelName)
	{
		async = SceneManager.LoadSceneAsync(levelName);
	}

	public void ToggleMenu(GameObject menu)
	{
		levels.SetActive(value: false);
		back.SetActive(value: false);
		menu.SetActive(value: true);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
