using UnityEngine;
using UnityEngine.SceneManagement;

public class SRTransitionMap : MonoBehaviour
{
	private string StartingMap;

	public GameObject UIFadeout;

	public GameObject RCCCanvasPhoton;

	private int lint;

	private void Start()
	{
		StartingMap = SceneManager.GetActiveScene().name;
		lint = 0;
		Object.DontDestroyOnLoad(RCCCanvasPhoton);
	}

	private void Update()
	{
		if (UIFadeout.activeSelf && lint == 0)
		{
			lint = 1;
			Debug.Log("DONT DESTROY BTICH3");
		}
		if (GameObject.FindGameObjectsWithTag("CanvasFadeOut").Length > 1 && lint == 1)
		{
			Object.Destroy(RCCCanvasPhoton);
			Debug.Log("JACK");
		}
	}

	public void DisableFirst()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("CanvasFadeOut");
		if (array.Length > 1)
		{
			array[0].SetActive(value: false);
		}
	}
}
