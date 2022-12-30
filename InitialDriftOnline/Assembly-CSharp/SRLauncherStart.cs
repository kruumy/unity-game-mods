using UnityEngine;
using UnityEngine.UI;

public class SRLauncherStart : MonoBehaviour
{
	public Button Startbtn;

	private void Start()
	{
		Startbtn.interactable = false;
		Startbtn.interactable = true;
		Startbtn.GetComponent<Button>().Select();
	}

	private void Update()
	{
	}
}
