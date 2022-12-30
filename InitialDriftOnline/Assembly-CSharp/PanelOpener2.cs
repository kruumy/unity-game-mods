using UnityEngine;

public class PanelOpener2 : MonoBehaviour
{
	public GameObject Panel;

	public void OpenPanel()
	{
		if (Panel != null)
		{
			bool activeSelf = Panel.activeSelf;
			Panel.SetActive(!activeSelf);
		}
	}
}
