using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SpielmannSpiel_Launcher;

public class GameSampleScene : MonoBehaviour
{
	public Text fps;

	public Text selectedQuality;

	private void Start()
	{
		string[] names = QualitySettings.names;
		selectedQuality.text = names[QualitySettings.GetQualityLevel()];
	}

	private void Update()
	{
		fps.text = $"{1f / Time.smoothDeltaTime:0.00}";
	}

	public void back()
	{
		SceneManager.LoadScene(0, LoadSceneMode.Single);
	}

	public void loadExitScene()
	{
		SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1, LoadSceneMode.Single);
	}

	public void quit()
	{
		Application.Quit();
	}
}
