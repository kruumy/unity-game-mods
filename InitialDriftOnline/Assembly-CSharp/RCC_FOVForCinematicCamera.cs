using UnityEngine;

public class RCC_FOVForCinematicCamera : MonoBehaviour
{
	private RCC_CinematicCamera cinematicCamera;

	public float FOV = 30f;

	private void Awake()
	{
		cinematicCamera = GetComponentInParent<RCC_CinematicCamera>();
	}

	private void Update()
	{
		cinematicCamera.targetFOV = FOV;
	}
}
