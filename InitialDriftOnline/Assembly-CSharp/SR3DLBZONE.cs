using UnityEngine;
using ZionBandwidthOptimizer.Examples;

public class SR3DLBZONE : MonoBehaviour
{
	public GameObject LeaderboardMapTime;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnTriggerEnter(Collider other)
	{
		_ = other.GetComponentInParent<RCC_PhotonNetwork>().isMine;
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.GetComponentInParent<RCC_PhotonNetwork>().isMine)
		{
			GetComponent<SR3DLB>().EnableLBB(jack: false);
		}
	}
}
