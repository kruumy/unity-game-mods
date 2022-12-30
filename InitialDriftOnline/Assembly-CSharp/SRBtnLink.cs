using UnityEngine;

public class SRBtnLink : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OpenLink(string link)
	{
		Application.OpenURL(link);
	}
}
