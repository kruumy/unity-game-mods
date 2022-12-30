using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VehicleControllerMSACC : MonoBehaviour
{
	public WheelCollider rightFrontWheelCollider;

	public Transform rightFrontWheelMesh;

	[Space(5f)]
	public WheelCollider leftFrontWheelCollider;

	public Transform leftFrontWheelMesh;

	[Space(5f)]
	public WheelCollider rightRearWheelCollider;

	public Transform rightRearWheelMesh;

	[Space(5f)]
	public WheelCollider leftRearWheelCollider;

	public Transform leftRearWheelMesh;

	[Space(30f)]
	[Range(0.2f, 1.5f)]
	public float torqueForceWheel = 1f;

	public Transform centerOfMass;

	private Rigidbody rb;

	private float motorTorque;

	private float brakeTorque;

	private float KMh;

	private float angle;

	private float direction;

	private bool handBrake;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		if (rb.mass < 1000f)
		{
			rb.mass = 1000f;
		}
		rb.interpolation = RigidbodyInterpolation.Interpolate;
		rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		if (centerOfMass != null)
		{
			rb.centerOfMass = base.transform.InverseTransformPoint(centerOfMass.position);
		}
		else
		{
			rb.centerOfMass = Vector3.zero;
		}
		if (rightFrontWheelCollider != null && leftFrontWheelCollider != null && rightRearWheelCollider != null && leftRearWheelCollider != null)
		{
			GetComponentInChildren<WheelCollider>().ConfigureVehicleSubsteps(1000f, 20, 20);
		}
	}

	private void Update()
	{
		KMh = rb.velocity.magnitude * 3.6f;
		rb.drag = Mathf.Clamp(KMh / 250f * 0.075f, 0.001f, 0.075f);
		direction = Input.GetAxis("Horizontal");
		if (Mathf.Abs(direction) > 0.7f)
		{
			angle = Mathf.Lerp(angle, direction, Time.deltaTime * 4f);
		}
		else
		{
			angle = Mathf.Lerp(angle, direction, Time.deltaTime * 2f);
		}
		if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.1f)
		{
			motorTorque = 0f;
			brakeTorque = Mathf.Lerp(brakeTorque, rb.mass, Time.deltaTime * 2f);
		}
		else
		{
			motorTorque = Mathf.Lerp(motorTorque, Input.GetAxis("Vertical") * rb.mass * torqueForceWheel, Time.deltaTime);
			brakeTorque = 0f;
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			handBrake = true;
		}
		else
		{
			handBrake = false;
		}
		if (handBrake)
		{
			brakeTorque = float.MaxValue;
			motorTorque = 0f;
		}
		if (rightFrontWheelCollider != null && leftFrontWheelCollider != null && rightRearWheelCollider != null && leftRearWheelCollider != null)
		{
			ApplyTorque(motorTorque);
			ApplyBrakes(brakeTorque);
		}
	}

	private void FixedUpdate()
	{
		if (rightFrontWheelCollider != null && leftFrontWheelCollider != null && rightRearWheelCollider != null && leftRearWheelCollider != null)
		{
			DownForce();
			StabilizeVehicle();
			MeshUpdate();
		}
		if (Mathf.Abs(direction) < 0.9f)
		{
			Vector3 b = new Vector3(rb.angularVelocity.x, 0f, rb.angularVelocity.z);
			rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, b, Time.deltaTime * 2f);
		}
	}

	private void MeshUpdate()
	{
		rightFrontWheelCollider.steerAngle = angle * 30f;
		leftFrontWheelCollider.steerAngle = angle * 30f;
		WheelMeshUpdate(rightFrontWheelCollider, rightFrontWheelMesh);
		WheelMeshUpdate(leftFrontWheelCollider, leftFrontWheelMesh);
		WheelMeshUpdate(rightRearWheelCollider, rightRearWheelMesh);
		WheelMeshUpdate(leftRearWheelCollider, leftRearWheelMesh);
	}

	private void WheelMeshUpdate(WheelCollider collider, Transform wheelMesh)
	{
		collider.GetWorldPose(out var pos, out var quat);
		wheelMesh.position = pos;
		wheelMesh.rotation = quat;
	}

	private void DownForce()
	{
		bool isGrounded = rightFrontWheelCollider.isGrounded;
		bool isGrounded2 = leftFrontWheelCollider.isGrounded;
		bool isGrounded3 = rightRearWheelCollider.isGrounded;
		bool isGrounded4 = leftRearWheelCollider.isGrounded;
		if ((isGrounded && isGrounded2) || (isGrounded3 && isGrounded4))
		{
			float num = Mathf.Clamp(Mathf.Abs(Vector3.Dot(Vector3.up, base.transform.up)), 0.3f, 1f);
			rb.AddForce(-base.transform.up * (rb.mass * 2f * num + 0.8f * num * Mathf.Abs(KMh * 3f) * (rb.mass / 125f)));
		}
	}

	private void StabilizeVehicle()
	{
		float num = 1f;
		float num2 = 1f;
		float num3 = 1f;
		float num4 = 1f;
		WheelHit hit;
		bool groundHit = leftRearWheelCollider.GetGroundHit(out hit);
		if (groundHit)
		{
			num3 = (0f - leftRearWheelCollider.transform.InverseTransformPoint(hit.point).y - leftRearWheelCollider.radius) / leftRearWheelCollider.suspensionDistance;
		}
		bool groundHit2 = rightRearWheelCollider.GetGroundHit(out hit);
		if (groundHit2)
		{
			num4 = (0f - rightRearWheelCollider.transform.InverseTransformPoint(hit.point).y - rightRearWheelCollider.radius) / rightRearWheelCollider.suspensionDistance;
		}
		bool groundHit3 = leftFrontWheelCollider.GetGroundHit(out hit);
		if (groundHit3)
		{
			num = (0f - leftFrontWheelCollider.transform.InverseTransformPoint(hit.point).y - leftFrontWheelCollider.radius) / leftFrontWheelCollider.suspensionDistance;
		}
		bool groundHit4 = rightFrontWheelCollider.GetGroundHit(out hit);
		if (groundHit4)
		{
			num2 = (0f - rightFrontWheelCollider.transform.InverseTransformPoint(hit.point).y - rightFrontWheelCollider.radius) / rightFrontWheelCollider.suspensionDistance;
		}
		float num5 = Mathf.Clamp(Mathf.Abs(Vector3.Dot(Vector3.up, base.transform.up)), 0.3f, 1f);
		float num6 = (num3 - num4) * rb.mass * num5;
		float num7 = (num - num2) * rb.mass * num5;
		if (groundHit)
		{
			rb.AddForceAtPosition(leftRearWheelCollider.transform.up * (0f - num6), leftRearWheelCollider.transform.position);
		}
		if (groundHit2)
		{
			rb.AddForceAtPosition(rightRearWheelCollider.transform.up * num6, rightRearWheelCollider.transform.position);
		}
		if (groundHit3)
		{
			rb.AddForceAtPosition(leftFrontWheelCollider.transform.up * (0f - num7), leftFrontWheelCollider.transform.position);
		}
		if (groundHit4)
		{
			rb.AddForceAtPosition(rightFrontWheelCollider.transform.up * num7, rightFrontWheelCollider.transform.position);
		}
	}

	private void ApplyTorque(float torqueForce)
	{
		rightFrontWheelCollider.motorTorque = torqueForce;
		leftFrontWheelCollider.motorTorque = torqueForce;
		rightRearWheelCollider.motorTorque = torqueForce;
		leftRearWheelCollider.motorTorque = torqueForce;
	}

	private void ApplyBrakes(float brakeForce)
	{
		rightFrontWheelCollider.brakeTorque = brakeForce;
		leftFrontWheelCollider.brakeTorque = brakeForce;
		rightRearWheelCollider.brakeTorque = brakeForce;
		leftRearWheelCollider.brakeTorque = brakeForce;
	}
}
