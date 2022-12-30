using System.Collections;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/RCC Chassis")]
public class RCC_Chassis : MonoBehaviour
{
	private RCC_Settings RCCSettingsInstance;

	private RCC_CarControllerV3 carController;

	private Rigidbody rigid;

	private float chassisVerticalLean = 4f;

	private float chassisHorizontalLean = 4f;

	private float horizontalLean;

	private float verticalLean;

	private RCC_Settings RCCSettings
	{
		get
		{
			if (RCCSettingsInstance == null)
			{
				RCCSettingsInstance = RCC_Settings.Instance;
				return RCCSettingsInstance;
			}
			return RCCSettingsInstance;
		}
	}

	private void Start()
	{
		carController = GetComponentInParent<RCC_CarControllerV3>();
		rigid = carController.GetComponent<Rigidbody>();
		if (!RCCSettings.dontUseChassisJoint)
		{
			ChassisJoint();
		}
	}

	private void OnEnable()
	{
		if (!RCCSettings.dontUseChassisJoint)
		{
			StartCoroutine("ReEnable");
		}
	}

	private IEnumerator ReEnable()
	{
		if ((bool)base.transform.parent.GetComponent<ConfigurableJoint>())
		{
			GameObject _joint = GetComponentInParent<ConfigurableJoint>().gameObject;
			_joint.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
			yield return new WaitForFixedUpdate();
			_joint.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
		}
	}

	private void ChassisJoint()
	{
		GameObject gameObject = new GameObject("Colliders");
		gameObject.transform.SetParent(GetComponentInParent<RCC_CarControllerV3>().transform, worldPositionStays: false);
		Transform[] componentsInChildren = GetComponentInParent<RCC_CarControllerV3>().chassis.GetComponentsInChildren<Transform>();
		foreach (Transform transform in componentsInChildren)
		{
			if (!transform.gameObject.activeSelf || !transform.GetComponent<Collider>())
			{
				continue;
			}
			if (transform.childCount >= 1)
			{
				Transform[] componentsInChildren2 = transform.GetComponentsInChildren<Transform>();
				foreach (Transform transform2 in componentsInChildren2)
				{
					if (transform2 != transform)
					{
						transform2.SetParent(base.transform);
					}
				}
			}
			GameObject obj = Object.Instantiate(transform.gameObject, transform.transform.position, transform.transform.rotation);
			obj.transform.SetParent(gameObject.transform, worldPositionStays: true);
			obj.transform.localScale = transform.lossyScale;
			Component[] components = obj.GetComponents(typeof(Component));
			foreach (Component component in components)
			{
				if (!(component is Transform) && !(component is Collider))
				{
					Object.Destroy(component);
				}
			}
		}
		GameObject gameObject2 = Object.Instantiate(RCCSettings.chassisJoint, Vector3.zero, Quaternion.identity);
		gameObject2.transform.SetParent(rigid.transform, worldPositionStays: false);
		gameObject2.GetComponent<ConfigurableJoint>().connectedBody = rigid;
		gameObject2.GetComponent<ConfigurableJoint>().autoConfigureConnectedAnchor = false;
		base.transform.SetParent(gameObject2.transform, worldPositionStays: false);
		Collider[] componentsInChildren3 = GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren3.Length; i++)
		{
			Object.Destroy(componentsInChildren3[i]);
		}
		GetComponentInParent<Rigidbody>().centerOfMass = new Vector3(rigid.centerOfMass.x, rigid.centerOfMass.y + 1f, rigid.centerOfMass.z);
	}

	private void Update()
	{
		chassisVerticalLean = carController.chassisVerticalLean;
		chassisHorizontalLean = carController.chassisHorizontalLean;
	}

	private void FixedUpdate()
	{
		if (RCCSettings.dontUseChassisJoint)
		{
			LegacyChassis();
		}
	}

	private void LegacyChassis()
	{
		Vector3 vector = rigid.transform.InverseTransformDirection(rigid.angularVelocity);
		verticalLean = Mathf.Clamp(Mathf.Lerp(verticalLean, rigid.angularVelocity.x * chassisVerticalLean, Time.fixedDeltaTime * 5f), -5f, 5f);
		horizontalLean = Mathf.Clamp(Mathf.Lerp(horizontalLean, vector.y * chassisHorizontalLean, Time.fixedDeltaTime * 5f), -5f, 5f);
		if (!float.IsNaN(verticalLean) && !float.IsNaN(horizontalLean) && !float.IsInfinity(verticalLean) && !float.IsInfinity(horizontalLean) && !Mathf.Approximately(verticalLean, 0f) && !Mathf.Approximately(horizontalLean, 0f))
		{
			Quaternion localRotation = Quaternion.Euler(verticalLean, base.transform.localRotation.y, horizontalLean);
			base.transform.localRotation = localRotation;
		}
	}
}
