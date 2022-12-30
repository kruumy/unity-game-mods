using UnityEngine;

namespace Funly.SkyStudio;

public class RotateBody : MonoBehaviour
{
	private float m_SpinSpeed;

	private bool m_AllowSpinning;

	public float SpinSpeed
	{
		get
		{
			return m_SpinSpeed;
		}
		set
		{
			m_SpinSpeed = value;
			UpdateOrbitBodyRotation();
		}
	}

	public bool AllowSpinning
	{
		get
		{
			return m_AllowSpinning;
		}
		set
		{
			m_AllowSpinning = value;
			UpdateOrbitBodyRotation();
		}
	}

	public void UpdateOrbitBodyRotation()
	{
		float num = (m_AllowSpinning ? 1 : 0);
		Vector3 euler = new Vector3(0f, -180f, (base.transform.localRotation.eulerAngles.z + -10f * SpinSpeed * Time.deltaTime) * num);
		base.transform.localRotation = Quaternion.Euler(euler);
	}
}
