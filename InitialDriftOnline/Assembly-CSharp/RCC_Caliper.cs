using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/RCC Visual Brake Caliper")]
public class RCC_Caliper : MonoBehaviour
{
	public RCC_WheelCollider wheelCollider;

	private GameObject newPivot;

	private Quaternion defLocalRotation;

	private void Start()
	{
		if (!wheelCollider)
		{
			Debug.LogError("WheelCollider is not selected for this caliper named " + base.transform.name);
			base.enabled = false;
			return;
		}
		newPivot = new GameObject("Pivot_" + base.transform.name);
		newPivot.transform.SetParent(wheelCollider.wheelCollider.transform, worldPositionStays: false);
		base.transform.SetParent(newPivot.transform, worldPositionStays: true);
		defLocalRotation = newPivot.transform.localRotation;
	}

	private void Update()
	{
		if ((bool)wheelCollider.wheelModel && (bool)wheelCollider.wheelCollider)
		{
			newPivot.transform.position = new Vector3(wheelCollider.wheelModel.transform.position.x, wheelCollider.wheelModel.transform.position.y, wheelCollider.wheelModel.transform.position.z);
			newPivot.transform.localRotation = defLocalRotation * Quaternion.AngleAxis(wheelCollider.wheelCollider.steerAngle, Vector3.up);
		}
	}
}
