using UnityEngine;

namespace SickscoreGames.ExampleScene;

public class ExampleResetPlayer : MonoBehaviour
{
	public Transform spawnPoint;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			other.gameObject.transform.position = spawnPoint.position;
			Rigidbody component = other.gameObject.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.velocity = other.transform.forward * 5f;
			}
		}
	}
}
