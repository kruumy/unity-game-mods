using UnityEngine;

public class PanelOpen : MonoBehaviour
{
	public GameObject Panel;

	public void OpenPanel()
	{
		if (Panel != null)
		{
			Panel.SetActive(value: true);
		}
	}
}
