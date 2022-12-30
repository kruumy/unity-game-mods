using CodeStage.AntiCheat.Storage;
using UnityEngine;
using ZionBandwidthOptimizer.Examples;

public class SRToffuArriver : MonoBehaviour
{
	public GameObject TofuManager;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponentInParent<RCC_PhotonNetwork>().isMine && ObscuredPrefs.GetBool("TOFU RUN"))
		{
			Debug.Log("FIN DE TOFU");
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Animator>().Play("CagetteDesappear");
			TofuManager.GetComponent<SRToffuManager>().FinDeLivraison();
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.GetComponentInParent<RCC_PhotonNetwork>().isMine)
		{
			other.gameObject.tag = "Player";
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInParent<SRPlayerCollider>().AppelRPCSetGhostModeV2(8);
		}
	}
}
