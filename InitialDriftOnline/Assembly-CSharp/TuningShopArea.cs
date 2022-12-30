using UnityEngine;
using ZionBandwidthOptimizer.Examples;

public class TuningShopArea : MonoBehaviour
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
			Object.FindObjectOfType<Becquet>().UICMD(jack: true);
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().AppelRPCSetGhostModeV2(10);
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.GetComponentInParent<RCC_PhotonNetwork>().isMine)
		{
			Object.FindObjectOfType<Becquet>().UICMD(jack: false);
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().AppelRPCSetGhostModeV2(8);
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SkinManager>().SendMySkinColorToOther();
		}
	}
}
