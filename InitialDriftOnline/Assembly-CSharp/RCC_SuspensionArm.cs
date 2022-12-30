using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/RCC Visual Axle (Suspension Distance Based)")]
public class RCC_SuspensionArm : MonoBehaviour
{
	public enum SuspensionType
	{
		Position,
		Rotation
	}

	public enum Axis
	{
		X,
		Y,
		Z
	}

	public RCC_WheelCollider wheelcollider;

	public SuspensionType suspensionType;

	public Axis axis;

	private Vector3 orgPos;

	private Vector3 orgRot;

	private float totalSuspensionDistance;

	public float offsetAngle = 30f;

	public float angleFactor = 150f;

	private void Start()
	{
		orgPos = base.transform.localPosition;
		orgRot = base.transform.localEulerAngles;
		totalSuspensionDistance = GetSuspensionDistance();
	}

	private void Update()
	{
		float num = GetSuspensionDistance() - totalSuspensionDistance;
		base.transform.localPosition = orgPos;
		base.transform.localEulerAngles = orgRot;
		switch (suspensionType)
		{
		case SuspensionType.Position:
			switch (axis)
			{
			case Axis.X:
				base.transform.position += base.transform.right * num;
				break;
			case Axis.Y:
				base.transform.position += base.transform.up * num;
				break;
			case Axis.Z:
				base.transform.position += base.transform.forward * num;
				break;
			}
			break;
		case SuspensionType.Rotation:
			switch (axis)
			{
			case Axis.X:
				base.transform.Rotate(Vector3.right, num * angleFactor - offsetAngle, Space.Self);
				break;
			case Axis.Y:
				base.transform.Rotate(Vector3.up, num * angleFactor - offsetAngle, Space.Self);
				break;
			case Axis.Z:
				base.transform.Rotate(Vector3.forward, num * angleFactor - offsetAngle, Space.Self);
				break;
			}
			break;
		}
	}

	private float GetSuspensionDistance()
	{
		wheelcollider.wheelCollider.GetWorldPose(out var pos, out var _);
		return wheelcollider.transform.InverseTransformPoint(pos).y;
	}
}
