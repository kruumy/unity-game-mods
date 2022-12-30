using UnityEngine;
using ZionBandwidthOptimizer.Examples;

public class SRspawnCor : MonoBehaviour
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
			other.GetComponentInParent<Rigidbody>().velocity = new Vector3(-12f, 0f, 12f);
		}
	}
}
