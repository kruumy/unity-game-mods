using UnityEngine;

public class RCC_APIExample : MonoBehaviour
{
	public RCC_CarControllerV3 spawnVehiclePrefab;

	private RCC_CarControllerV3 currentVehiclePrefab;

	public Transform spawnTransform;

	public bool playerVehicle;

	public bool controllable;

	public bool engineRunning;

	public void Spawn()
	{
		currentVehiclePrefab = RCC.SpawnRCC(spawnVehiclePrefab, spawnTransform.position, spawnTransform.rotation, playerVehicle, controllable, engineRunning);
	}

	public void SetPlayer()
	{
		RCC.RegisterPlayerVehicle(currentVehiclePrefab);
	}

	public void SetControl(bool control)
	{
		RCC.SetControl(currentVehiclePrefab, control);
	}

	public void SetEngine(bool engine)
	{
		RCC.SetEngine(currentVehiclePrefab, engine);
	}

	public void DeRegisterPlayer()
	{
		RCC.DeRegisterPlayerVehicle();
	}
}
