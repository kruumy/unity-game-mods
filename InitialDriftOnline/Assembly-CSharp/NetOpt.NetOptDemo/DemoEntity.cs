using UnityEngine;

namespace NetOpt.NetOptDemo;

public class DemoEntity : MonoBehaviour
{
	private Bounds bounds = new Bounds(Vector3.zero, new Vector3(19f, 1f, 19f));

	public float speed = 3f;

	public Vector3 serializedPosition;

	public Quaternion serializedRotation;

	public Vector3 serializedScale;

	public Vector3 logicalPosition;

	public Quaternion logicalRotation;

	public Vector3 logicalScale;

	private Vector3 currentTarget;

	private void Start()
	{
		logicalPosition = base.transform.position;
		logicalRotation = base.transform.rotation;
		logicalScale = base.transform.localScale;
		logicalPosition = RandomPosition();
		serializedPosition = logicalPosition;
	}

	private Vector3 RandomPosition()
	{
		return new Vector3(Random.Range(bounds.min.x, bounds.max.x), 0.5f, Random.Range(bounds.min.z, bounds.max.z));
	}

	private void Update()
	{
		if (Vector3.Distance(logicalPosition, currentTarget) <= 1f)
		{
			currentTarget = RandomPosition();
		}
		Vector3 normalized = (currentTarget - logicalPosition).normalized;
		logicalPosition += normalized * (speed * Time.deltaTime);
		logicalRotation = Quaternion.LookRotation(normalized);
		logicalScale = Vector3.one;
		base.transform.position = Vector3.Lerp(base.transform.position, serializedPosition, Time.deltaTime * 6f);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, serializedRotation, Time.deltaTime * 6f);
	}
}
