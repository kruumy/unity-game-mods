using Photon.Pun;
using UnityEngine;

public class SRspawnco2 : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.GetComponentInParent<PhotonView>().IsMine)
		{
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(-14f, -2f, -2f);
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.GetComponentInParent<PhotonView>().IsMine)
		{
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(-14f, -2f, -2f);
		}
	}
}
