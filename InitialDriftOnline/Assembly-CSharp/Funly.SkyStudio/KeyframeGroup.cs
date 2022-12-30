using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio;

[Serializable]
public class KeyframeGroup<T> : IKeyframeGroup where T : IBaseKeyframe
{
	public List<T> keyframes = new List<T>();

	[SerializeField]
	private string m_Name;

	[SerializeField]
	private string m_Id;

	public string name
	{
		get
		{
			return m_Name;
		}
		set
		{
			m_Name = value;
		}
	}

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

	public KeyframeGroup(string name)
	{
		this.name = name;
		id = Guid.NewGuid().ToString();
	}

	public void AddKeyFrame(T keyFrame)
	{
		keyframes.Add(keyFrame);
		SortKeyframes();
	}

	public void RemoveKeyFrame(T keyFrame)
	{
		if (keyframes.Count == 1)
		{
			Debug.LogError("You must have at least 1 keyframe in every group.");
			return;
		}
		keyframes.Remove(keyFrame);
		SortKeyframes();
	}

	public void RemoveKeyFrame(IBaseKeyframe keyframe)
	{
		RemoveKeyFrame((T)keyframe);
	}

	public int GetKeyFrameCount()
	{
		return keyframes.Count;
	}

	public T GetKeyframe(int index)
	{
		return keyframes[index];
	}

	public void SortKeyframes()
	{
		keyframes.Sort();
	}

	public float CurveAdjustedBlendingTime(InterpolationCurve curve, float t)
	{
		return curve switch
		{
			InterpolationCurve.Linear => t, 
			InterpolationCurve.EaseInEaseOut => Mathf.Clamp01((t < 0.5f) ? (2f * t * t) : (-1f + (4f - 2f * t) * t)), 
			_ => t, 
		};
	}

	public T GetPreviousKeyFrame(float time)
	{
		if (!GetSurroundingKeyFrames(time, out T beforeKeyframe, out T _))
		{
			return default(T);
		}
		return beforeKeyframe;
	}

	public bool GetSurroundingKeyFrames(float time, out T beforeKeyframe, out T afterKeyframe)
	{
		beforeKeyframe = default(T);
		afterKeyframe = default(T);
		if (GetSurroundingKeyFrames(time, out int beforeIndex, out int afterIndex))
		{
			beforeKeyframe = GetKeyframe(beforeIndex);
			afterKeyframe = GetKeyframe(afterIndex);
			return true;
		}
		return false;
	}

	public bool GetSurroundingKeyFrames(float time, out int beforeIndex, out int afterIndex)
	{
		beforeIndex = 0;
		afterIndex = 0;
		if (keyframes.Count == 0)
		{
			Debug.LogError("Can't return nearby keyframes since it's empty.");
			return false;
		}
		if (keyframes.Count == 1)
		{
			return true;
		}
		if (time < keyframes[0].time)
		{
			beforeIndex = keyframes.Count - 1;
			afterIndex = 0;
			return true;
		}
		int num = 0;
		for (int i = 0; i < keyframes.Count && !(keyframes[i].time >= time); i++)
		{
			num = i;
		}
		int num2 = (num + 1) % keyframes.Count;
		beforeIndex = num;
		afterIndex = num2;
		return true;
	}

	public static float ProgressBetweenSurroundingKeyframes(float time, BaseKeyframe beforeKey, BaseKeyframe afterKey)
	{
		return ProgressBetweenSurroundingKeyframes(time, beforeKey.time, afterKey.time);
	}

	public static float ProgressBetweenSurroundingKeyframes(float time, float beforeKeyTime, float afterKeyTime)
	{
		if (afterKeyTime > beforeKeyTime && time <= beforeKeyTime)
		{
			return 0f;
		}
		float num = WidthBetweenCircularValues(beforeKeyTime, afterKeyTime);
		return Mathf.Clamp01(WidthBetweenCircularValues(beforeKeyTime, time) / num);
	}

	public static float WidthBetweenCircularValues(float begin, float end)
	{
		if (begin <= end)
		{
			return end - begin;
		}
		return 1f - begin + end;
	}

	public void TrimToSingleKeyframe()
	{
		if (keyframes.Count != 1)
		{
			keyframes.RemoveRange(1, keyframes.Count - 1);
		}
	}

	public InterpolationDirection GetShortestInterpolationDirection(float previousKeyValue, float nextKeyValue, float minValue, float maxValue)
	{
		CalculateCircularDistances(previousKeyValue, nextKeyValue, minValue, maxValue, out var forwardDistance, out var reverseDistance);
		if (reverseDistance > forwardDistance)
		{
			return InterpolationDirection.Reverse;
		}
		return InterpolationDirection.Foward;
	}

	public void CalculateCircularDistances(float previousKeyValue, float nextKeyValue, float minValue, float maxValue, out float forwardDistance, out float reverseDistance)
	{
		if (nextKeyValue < previousKeyValue)
		{
			forwardDistance = maxValue - previousKeyValue + (nextKeyValue - minValue);
		}
		else
		{
			forwardDistance = nextKeyValue - previousKeyValue;
		}
		reverseDistance = minValue + maxValue - forwardDistance;
	}

	public float InterpolateFloat(InterpolationCurve curve, InterpolationDirection direction, float time, float beforeTime, float nextTime, float previousKeyValue, float nextKeyValue, float minValue, float maxValue)
	{
		float t = ProgressBetweenSurroundingKeyframes(time, beforeTime, nextTime);
		float num = CurveAdjustedBlendingTime(curve, t);
		if (direction == InterpolationDirection.Auto)
		{
			return AutoInterpolation(num, previousKeyValue, nextKeyValue);
		}
		InterpolationDirection interpolationDirection = direction;
		CalculateCircularDistances(previousKeyValue, nextKeyValue, minValue, maxValue, out var forwardDistance, out var reverseDistance);
		if (interpolationDirection == InterpolationDirection.ShortestPath)
		{
			interpolationDirection = ((reverseDistance > forwardDistance) ? InterpolationDirection.Foward : InterpolationDirection.Reverse);
		}
		switch (interpolationDirection)
		{
		case InterpolationDirection.Foward:
			return ForwardInterpolation(num, previousKeyValue, nextKeyValue, minValue, maxValue, forwardDistance);
		case InterpolationDirection.Reverse:
			return ReverseInterpolation(num, previousKeyValue, nextKeyValue, minValue, maxValue, reverseDistance);
		default:
			Debug.LogError(string.Concat("Unhandled interpolation direction: ", interpolationDirection, ", returning min value."));
			return minValue;
		}
	}

	public float AutoInterpolation(float curvedTime, float previousValue, float nextValue)
	{
		return Mathf.Lerp(previousValue, nextValue, curvedTime);
	}

	public float ForwardInterpolation(float time, float previousKeyValue, float nextKeyValue, float minValue, float maxValue, float distance)
	{
		if (previousKeyValue <= nextKeyValue)
		{
			return Mathf.Lerp(previousKeyValue, nextKeyValue, time);
		}
		float num = time * distance;
		float num2 = maxValue - previousKeyValue;
		if (num <= num2)
		{
			return previousKeyValue + num;
		}
		return minValue + (num - num2);
	}

	public float ReverseInterpolation(float time, float previousKeyValue, float nextKeyValue, float minValue, float maxValue, float distance)
	{
		if (nextKeyValue <= previousKeyValue)
		{
			return Mathf.Lerp(previousKeyValue, nextKeyValue, time);
		}
		float num = time * distance;
		float num2 = previousKeyValue - minValue;
		if (num <= num2)
		{
			return previousKeyValue - num;
		}
		return maxValue - (num - num2);
	}
}
