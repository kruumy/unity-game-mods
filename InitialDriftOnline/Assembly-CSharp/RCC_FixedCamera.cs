using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/RCC Fixed Camera")]
public class RCC_FixedCamera : MonoBehaviour
{
	private Vector3 targetPosition;

	public float maxDistance = 50f;

	private float distance;

	public float minimumFOV = 20f;

	public float maximumFOV = 60f;

	public bool canTrackNow;

	private void LateUpdate()
	{
		if (canTrackNow && (bool)RCC_SceneManager.Instance.activePlayerCamera && (bool)RCC_SceneManager.Instance.activePlayerVehicle)
		{
			distance = Vector3.Distance(base.transform.position, RCC_SceneManager.Instance.activePlayerVehicle.transform.position);
			RCC_SceneManager.Instance.activePlayerCamera.targetFieldOfView = Mathf.Lerp((distance > maxDistance / 10f) ? maximumFOV : 70f, minimumFOV, distance * 1.5f / maxDistance);
			targetPosition = RCC_SceneManager.Instance.activePlayerVehicle.transform.position;
			targetPosition += RCC_SceneManager.Instance.activePlayerVehicle.transform.rotation * Vector3.forward * (RCC_SceneManager.Instance.activePlayerVehicle.speed * 0.05f);
			base.transform.Translate(-RCC_SceneManager.Instance.activePlayerVehicle.transform.forward * RCC_SceneManager.Instance.activePlayerVehicle.speed / 50f * Time.deltaTime);
			base.transform.LookAt(targetPosition);
			if (distance > maxDistance)
			{
				ChangePosition();
			}
		}
	}

	public void ChangePosition()
	{
		if (canTrackNow && (bool)RCC_SceneManager.Instance.activePlayerCamera && (bool)RCC_SceneManager.Instance.activePlayerVehicle)
		{
			float num = Random.Range(-15f, 15f);
			if (Physics.Raycast(RCC_SceneManager.Instance.activePlayerVehicle.transform.position, Quaternion.AngleAxis(num, RCC_SceneManager.Instance.activePlayerVehicle.transform.up) * RCC_SceneManager.Instance.activePlayerVehicle.transform.forward, out var hitInfo, maxDistance) && !hitInfo.transform.IsChildOf(RCC_SceneManager.Instance.activePlayerVehicle.transform) && !hitInfo.collider.isTrigger)
			{
				base.transform.position = hitInfo.point;
				base.transform.LookAt(RCC_SceneManager.Instance.activePlayerVehicle.transform.position + new Vector3(0f, Mathf.Clamp(num, 0.5f, 5f), 0f));
				base.transform.position += base.transform.rotation * Vector3.forward * 5f;
			}
			else
			{
				base.transform.position = RCC_SceneManager.Instance.activePlayerVehicle.transform.position + new Vector3(0f, Mathf.Clamp(num, 0f, 5f), 0f);
				base.transform.position += Quaternion.AngleAxis(num, RCC_SceneManager.Instance.activePlayerVehicle.transform.up) * RCC_SceneManager.Instance.activePlayerVehicle.transform.forward * (maxDistance * 0.9f);
			}
		}
	}
}
