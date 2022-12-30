using UnityEngine;

public class RCC_FuelStation : MonoBehaviour
{
	private RCC_CarControllerV3 targetVehicle;

	public float refillSpeed = 1f;

	private void OnTriggerStay(Collider col)
	{
		if (targetVehicle == null && (bool)col.gameObject.GetComponentInParent<RCC_CarControllerV3>())
		{
			targetVehicle = col.gameObject.GetComponentInParent<RCC_CarControllerV3>();
		}
		if ((bool)targetVehicle)
		{
			targetVehicle.fuelTank += refillSpeed * Time.deltaTime;
		}
	}

	private void OnTriggerExit(Collider col)
	{
		if ((bool)col.gameObject.GetComponentInParent<RCC_CarControllerV3>())
		{
			targetVehicle = null;
		}
	}
}
