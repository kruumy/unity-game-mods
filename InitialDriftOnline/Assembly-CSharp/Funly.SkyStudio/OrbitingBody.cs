using UnityEngine;

namespace Funly.SkyStudio;

[ExecuteInEditMode]
public class OrbitingBody : MonoBehaviour
{
	private Transform m_PositionTransform;

	private RotateBody m_RotateBody;

	private SpherePoint m_SpherePoint = new SpherePoint(0f, 0f);

	private Vector3 m_CachedWorldDirection = Vector3.right;

	private Light m_BodyLight;

	public Transform positionTransform
	{
		get
		{
			if (m_PositionTransform == null)
			{
				m_PositionTransform = base.transform.Find("Position");
			}
			return m_PositionTransform;
		}
	}

	public RotateBody rotateBody
	{
		get
		{
			if (m_RotateBody == null)
			{
				Transform transform = positionTransform;
				if (!transform)
				{
					Debug.LogError("Can't return rotation body without a position transform game object");
					return null;
				}
				m_RotateBody = transform.GetComponent<RotateBody>();
			}
			return m_RotateBody;
		}
	}

	public SpherePoint Point
	{
		get
		{
			return m_SpherePoint;
		}
		set
		{
			if (m_SpherePoint == null)
			{
				m_SpherePoint = new SpherePoint(0f, 0f);
			}
			else
			{
				m_SpherePoint = value;
			}
			m_CachedWorldDirection = m_SpherePoint.GetWorldDirection();
			LayoutOribit();
		}
	}

	public Vector3 BodyGlobalDirection => m_CachedWorldDirection;

	public Light BodyLight
	{
		get
		{
			if (m_BodyLight == null)
			{
				m_BodyLight = base.transform.GetComponentInChildren<Light>();
				if (m_BodyLight != null)
				{
					m_BodyLight.transform.localRotation = Quaternion.identity;
				}
			}
			return m_BodyLight;
		}
	}

	public void ResetOrbit()
	{
		LayoutOribit();
		m_PositionTransform = null;
	}

	public void LayoutOribit()
	{
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		base.transform.forward = BodyGlobalDirection * -1f;
	}

	private void OnValidate()
	{
		LayoutOribit();
	}
}
