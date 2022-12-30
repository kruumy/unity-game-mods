using CodeStage.AntiCheat.Storage;
using UnityEngine;
using ZionBandwidthOptimizer.Examples;

public class SRLightTunerArea : MonoBehaviour
{
	public GameObject Light_UIManager;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.GetComponentInParent<RCC_PhotonNetwork>().isMine && PlayerPrefs.GetInt("ImInRun") == 0 && !ObscuredPrefs.GetBool("TOFU RUN") && (bool)other.GetComponent<BoxCollider>())
		{
			Light_UIManager.GetComponent<SRLightTunerUI>().SetUI(jack: true);
			other.gameObject.GetComponentInParent<SRPlayerCollider>().AppelRPCSetGhostModeV2(10);
		}
		else if (other.GetComponentInParent<RCC_PhotonNetwork>().isMine)
		{
			other.gameObject.GetComponentInParent<SRPlayerCollider>().AppelRPCSetGhostModeV2(10);
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.GetComponentInParent<RCC_PhotonNetwork>().isMine && PlayerPrefs.GetInt("ImInRun") == 0 && !ObscuredPrefs.GetBool("TOFU RUN") && (bool)other.GetComponent<BoxCollider>())
		{
			Light_UIManager.GetComponent<SRLightTunerUI>().SetUI(jack: false);
			Light_UIManager.GetComponent<SRLightTunerUI>().SendMyLightColor();
			other.gameObject.GetComponentInParent<SRPlayerCollider>().AppelRPCSetGhostModeV2(8);
		}
		else if (other.GetComponentInParent<RCC_PhotonNetwork>().isMine)
		{
			Light_UIManager.GetComponent<SRLightTunerUI>().SetUI(jack: false);
			other.gameObject.GetComponentInParent<SRPlayerCollider>().AppelRPCSetGhostModeV2(8);
		}
	}
}
