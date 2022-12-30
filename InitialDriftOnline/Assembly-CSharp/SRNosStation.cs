using UnityEngine;
using ZionBandwidthOptimizer.Examples;

public class SRNosStation : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.GetComponentInParent<RCC_PhotonNetwork>().isMine)
		{
			Object.FindObjectOfType<SRNosManager>().ShowUIRefuel(jack: true);
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInParent<SRPlayerCollider>().AppelRPCSetGhostModeV2(10);
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.GetComponentInParent<RCC_PhotonNetwork>().isMine)
		{
			Object.FindObjectOfType<SRNosManager>().ShowUIRefuel(jack: false);
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInParent<SRPlayerCollider>().AppelRPCSetGhostModeV2(8);
		}
	}
}
