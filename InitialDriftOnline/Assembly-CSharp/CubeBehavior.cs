using System;
using UnityEngine;

public class CubeBehavior : MonoBehaviour
{
	public Vector3 SpeedPerAxis = Vector3.zero;

	public float BounceSpeed = 1f;

	public float BounceMagnitude = 0.3f;

	private float startY;

	private void Start()
	{
		startY = base.transform.position.y;
	}

	private void Update()
	{
		base.transform.Rotate(SpeedPerAxis * 180f * Time.deltaTime);
		base.transform.transform.position = new Vector3(0f, startY + BounceMagnitude * Mathf.Sin(Time.timeSinceLevelLoad * (float)Math.PI * BounceSpeed), 0f);
	}
}
