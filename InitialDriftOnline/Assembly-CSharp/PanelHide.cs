using UnityEngine;

public class PanelHide : MonoBehaviour
{
	public GameObject Panel;

	public void OpenPanel()
	{
		if (Panel != null)
		{
			Panel.SetActive(value: false);
		}
	}
}
