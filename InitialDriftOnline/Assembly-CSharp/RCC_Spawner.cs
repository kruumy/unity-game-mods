using UnityEngine;

public class RCC_Spawner : MonoBehaviour
{
	private void Start()
	{
		int @int = PlayerPrefs.GetInt("SelectedRCCVehicle", 0);
		RCC.SpawnRCC(RCC_Vehicles.Instance.vehicles[@int], base.transform.position, base.transform.rotation, registerAsPlayerVehicle: true, isControllable: true, isEngineRunning: true);
	}
}
