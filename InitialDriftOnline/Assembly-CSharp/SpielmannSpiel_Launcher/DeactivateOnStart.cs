using System.Collections.Generic;
using UnityEngine;

namespace SpielmannSpiel_Launcher;

public class DeactivateOnStart : MonoBehaviour
{
	public List<GameObject> toDeactivate;

	private void Start()
	{
		int count = toDeactivate.Count;
		for (int i = 0; i < count; i++)
		{
			if (toDeactivate[i] != null)
			{
				toDeactivate[i].SetActive(value: false);
			}
		}
	}
}
