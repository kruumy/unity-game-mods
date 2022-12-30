using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/RCC Truck Trailer")]
[RequireComponent(typeof(Rigidbody))]
public class RCC_TruckTrailer : MonoBehaviour
{
	[Serializable]
	public class TrailerWheel
	{
		public WheelCollider wheelCollider;

		public Transform wheelModel;

		public float compression;

		public float wheelRotation;

		public void AddTorque(float torque)
		{
			wheelCollider.motorTorque = torque;
		}
	}

	public class JointRestrictions
	{
		public ConfigurableJointMotion motionX;

		public ConfigurableJointMotion motionY;

		public ConfigurableJointMotion motionZ;

		public ConfigurableJointMotion angularMotionX;

		public ConfigurableJointMotion angularMotionY;

		public ConfigurableJointMotion angularMotionZ;

		public void Get(ConfigurableJoint configurableJoint)
		{
			motionX = configurableJoint.xMotion;
			motionY = configurableJoint.yMotion;
			motionZ = configurableJoint.zMotion;
			angularMotionX = configurableJoint.angularXMotion;
			angularMotionY = configurableJoint.angularYMotion;
			angularMotionZ = configurableJoint.angularZMotion;
		}

		public void Set(ConfigurableJoint configurableJoint)
		{
			configurableJoint.xMotion = motionX;
			configurableJoint.yMotion = motionY;
			configurableJoint.zMotion = motionZ;
			configurableJoint.angularXMotion = angularMotionX;
			configurableJoint.angularYMotion = angularMotionY;
			configurableJoint.angularZMotion = angularMotionZ;
		}

		public void Reset(ConfigurableJoint configurableJoint)
		{
			configurableJoint.xMotion = ConfigurableJointMotion.Free;
			configurableJoint.yMotion = ConfigurableJointMotion.Free;
			configurableJoint.zMotion = ConfigurableJointMotion.Free;
			configurableJoint.angularXMotion = ConfigurableJointMotion.Free;
			configurableJoint.angularYMotion = ConfigurableJointMotion.Free;
			configurableJoint.angularZMotion = ConfigurableJointMotion.Free;
		}
	}

	private RCC_CarControllerV3 carController;

	private Rigidbody rigid;

	private ConfigurableJoint joint;

	public Transform COM;

	private bool isSleeping;

	public TrailerWheel[] trailerWheels;

	private WheelCollider[] allWheelColliders;

	private List<WheelCollider> leftWheelColliders = new List<WheelCollider>();

	private List<WheelCollider> rightWheelColliders = new List<WheelCollider>();

	public float antiRoll = 20000f;

	public bool attached;

	public JointRestrictions jointRestrictions = new JointRestrictions();

	private void Start()
	{
		rigid = GetComponent<Rigidbody>();
		joint = GetComponentInParent<ConfigurableJoint>();
		jointRestrictions.Get(joint);
		rigid.interpolation = RigidbodyInterpolation.None;
		rigid.interpolation = RigidbodyInterpolation.Interpolate;
		joint.configuredInWorldSpace = true;
		allWheelColliders = GetComponentsInChildren<WheelCollider>();
		for (int i = 0; i < allWheelColliders.Length; i++)
		{
			if (allWheelColliders[i].transform.localPosition.x < 0f)
			{
				leftWheelColliders.Add(allWheelColliders[i]);
			}
			else
			{
				rightWheelColliders.Add(allWheelColliders[i]);
			}
		}
		if ((bool)joint.connectedBody)
		{
			AttachTrailer(joint.connectedBody.gameObject.GetComponent<RCC_CarControllerV3>());
			return;
		}
		carController = null;
		joint.connectedBody = null;
		jointRestrictions.Reset(joint);
	}

	private void FixedUpdate()
	{
		attached = joint.connectedBody;
		rigid.centerOfMass = base.transform.InverseTransformPoint(COM.transform.position);
		if ((bool)carController)
		{
			AntiRollBars();
			for (int i = 0; i < trailerWheels.Length; i++)
			{
				trailerWheels[i].AddTorque(carController._gasInput * (attached ? 1f : 0f));
			}
		}
	}

	private void Update()
	{
		if (rigid.velocity.magnitude < 0.01f && Mathf.Abs(rigid.angularVelocity.magnitude) < 0.01f)
		{
			isSleeping = true;
		}
		else
		{
			isSleeping = false;
		}
		WheelAlign();
	}

	public void WheelAlign()
	{
		if (isSleeping)
		{
			return;
		}
		for (int i = 0; i < trailerWheels.Length; i++)
		{
			if (!trailerWheels[i].wheelModel)
			{
				Debug.LogError(base.transform.name + " wheel of the " + base.transform.name + " is missing wheel model. This wheel is disabled");
				base.enabled = false;
				break;
			}
			WheelHit hit;
			bool groundHit = trailerWheels[i].wheelCollider.GetGroundHit(out hit);
			float compression = trailerWheels[i].compression;
			compression = ((!groundHit) ? trailerWheels[i].wheelCollider.suspensionDistance : (1f - (Vector3.Dot(trailerWheels[i].wheelCollider.transform.position - hit.point, trailerWheels[i].wheelCollider.transform.up) - trailerWheels[i].wheelCollider.radius * trailerWheels[i].wheelCollider.transform.lossyScale.y) / trailerWheels[i].wheelCollider.suspensionDistance));
			trailerWheels[i].compression = Mathf.Lerp(trailerWheels[i].compression, compression, Time.deltaTime * 50f);
			trailerWheels[i].wheelModel.position = trailerWheels[i].wheelCollider.transform.position;
			trailerWheels[i].wheelModel.position += trailerWheels[i].wheelCollider.transform.up * (trailerWheels[i].compression - 1f) * trailerWheels[i].wheelCollider.suspensionDistance;
			trailerWheels[i].wheelRotation += trailerWheels[i].wheelCollider.rpm * 6f * Time.deltaTime;
			trailerWheels[i].wheelModel.rotation = trailerWheels[i].wheelCollider.transform.rotation * Quaternion.Euler(trailerWheels[i].wheelRotation, trailerWheels[i].wheelCollider.steerAngle, trailerWheels[i].wheelCollider.transform.rotation.z);
			float num = (0f - trailerWheels[i].wheelCollider.transform.InverseTransformPoint(hit.point).y - trailerWheels[i].wheelCollider.radius * trailerWheels[i].wheelCollider.transform.lossyScale.y) / trailerWheels[i].wheelCollider.suspensionDistance;
			Debug.DrawLine(hit.point, hit.point + trailerWheels[i].wheelCollider.transform.up * (hit.force / rigid.mass), ((double)num <= 0.0) ? Color.magenta : Color.white);
			Debug.DrawLine(hit.point, hit.point - trailerWheels[i].wheelCollider.transform.forward * hit.forwardSlip * 2f, Color.green);
			Debug.DrawLine(hit.point, hit.point - trailerWheels[i].wheelCollider.transform.right * hit.sidewaysSlip * 2f, Color.red);
		}
	}

	public void DetachTrailer()
	{
		carController = null;
		joint.connectedBody = null;
		jointRestrictions.Reset(joint);
		if ((bool)RCC_SceneManager.Instance.activePlayerCamera)
		{
			StartCoroutine(RCC_SceneManager.Instance.activePlayerCamera.AutoFocus());
		}
	}

	public void AttachTrailer(RCC_CarControllerV3 vehicle)
	{
		carController = vehicle;
		antiRoll = vehicle.antiRollRearHorizontal;
		joint.connectedBody = vehicle.rigid;
		jointRestrictions.Set(joint);
		vehicle.attachedTrailer = this;
		if ((bool)RCC_SceneManager.Instance.activePlayerCamera)
		{
			StartCoroutine(RCC_SceneManager.Instance.activePlayerCamera.AutoFocus(base.transform, carController.transform));
		}
	}

	public void AntiRollBars()
	{
		for (int i = 0; i < leftWheelColliders.Count; i++)
		{
			float num = 1f;
			float num2 = 1f;
			WheelHit hit;
			bool groundHit = leftWheelColliders[i].GetGroundHit(out hit);
			if (groundHit)
			{
				num = (0f - leftWheelColliders[i].transform.InverseTransformPoint(hit.point).y - leftWheelColliders[i].radius) / leftWheelColliders[i].suspensionDistance;
			}
			bool groundHit2 = rightWheelColliders[i].GetGroundHit(out hit);
			if (groundHit2)
			{
				num2 = (0f - rightWheelColliders[i].transform.InverseTransformPoint(hit.point).y - rightWheelColliders[i].radius) / rightWheelColliders[i].suspensionDistance;
			}
			float num3 = (num - num2) * antiRoll;
			if (groundHit)
			{
				rigid.AddForceAtPosition(leftWheelColliders[i].transform.up * (0f - num3), leftWheelColliders[i].transform.position);
			}
			if (groundHit2)
			{
				rigid.AddForceAtPosition(rightWheelColliders[i].transform.up * num3, rightWheelColliders[i].transform.position);
			}
			float num4 = 1f;
			float num5 = 1f;
			WheelHit hit2;
			bool groundHit3 = leftWheelColliders[i].GetGroundHit(out hit2);
			if (groundHit3)
			{
				num4 = (0f - leftWheelColliders[i].transform.InverseTransformPoint(hit2.point).y - leftWheelColliders[i].radius) / leftWheelColliders[i].suspensionDistance;
			}
			bool groundHit4 = rightWheelColliders[i].GetGroundHit(out hit2);
			if (groundHit4)
			{
				num5 = (0f - rightWheelColliders[i].transform.InverseTransformPoint(hit2.point).y - rightWheelColliders[i].radius) / rightWheelColliders[i].suspensionDistance;
			}
			float num6 = (num4 - num5) * antiRoll;
			if (groundHit3)
			{
				rigid.AddForceAtPosition(leftWheelColliders[i].transform.up * (0f - num6), leftWheelColliders[i].transform.position);
			}
			if (groundHit4)
			{
				rigid.AddForceAtPosition(rightWheelColliders[i].transform.up * num6, rightWheelColliders[i].transform.position);
			}
		}
	}

	private void OnTriggerEnter(Collider col)
	{
	}
}
