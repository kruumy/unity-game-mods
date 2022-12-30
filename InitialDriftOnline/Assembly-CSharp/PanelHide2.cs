using UnityEngine;

public class PanelHide2 : MonoBehaviour
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
