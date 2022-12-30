using System;
using UnityEngine;

namespace Funly.SkyStudio;

[Serializable]
public class BaseKeyframe : IComparable, IBaseKeyframe
{
	[SerializeField]
	public string m_Id;

	[SerializeField]
	private float m_Time;

	[SerializeField]
	private InterpolationCurve m_InterpolationCurve;

	[SerializeField]
	private InterpolationDirection m_InterpolationDirection;

	public string id
	{
		get
		{
			return m_Id;
		}
		set
		{
			m_Id = value;
		}
	}

	public float time
	{
		get
		{
			return m_Time;
		}
		set
		{
			m_Time = value;
		}
	}

	public InterpolationCurve interpolationCurve
	{
		get
		{
			return m_InterpolationCurve;
		}
		set
		{
			m_InterpolationCurve = value;
		}
	}

	public InterpolationDirection interpolationDirection
	{
		get
		{
			return m_InterpolationDirection;
		}
		set
		{
			m_InterpolationDirection = value;
		}
	}

	public BaseKeyframe(float time)
	{
		id = Guid.NewGuid().ToString();
		this.time = time;
	}

	public int CompareTo(object other)
	{
		BaseKeyframe baseKeyframe = other as BaseKeyframe;
		return time.CompareTo(baseKeyframe.time);
	}
}
