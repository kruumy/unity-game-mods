using UnityEngine;

namespace SickscoreGames.ExampleScene;

public class ExampleRotatePrism : MonoBehaviour
{
	[Range(0f, 100f)]
	public float rotationSpeed = 75f;

	private void Update()
	{
		if (rotationSpeed > 0f)
		{
			base.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
		}
	}
}
