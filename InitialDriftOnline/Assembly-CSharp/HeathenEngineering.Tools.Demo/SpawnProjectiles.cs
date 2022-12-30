using UnityEngine;
using UnityEngine.UI;

namespace HeathenEngineering.Tools.Demo;

public class SpawnProjectiles : MonoBehaviour
{
	public ProjectileSpawner spawner;

	public Vector3[] spawnPoints;

	public float spawnPointRotationSpeed = 540f;

	public Text countLabel;

	public Toggle autoSpawn;

	public Slider speedSlider;

	private Quaternion rot;

	private Vector3 bulletScale = new Vector3(0.1f, 0.1f, 0.1f);

	private float spawnTime;

	private Quaternion right;

	private Quaternion left;

	private Quaternion back;

	private void Start()
	{
		rot = Quaternion.identity;
		right = Quaternion.Euler(0f, 90f, 0f);
		left = Quaternion.Euler(0f, 270f, 0f);
		back = Quaternion.Euler(0f, 180f, 0f);
	}

	private void Update()
	{
		countLabel.text = spawner.Renderer.instances.Count.ToString();
		rot *= Quaternion.Euler(0f, spawnPointRotationSpeed * Time.deltaTime, 0f);
		float num = 1f / Mathf.Sqrt(rot.x * rot.x + rot.y * rot.y + rot.z * rot.z + rot.w * rot.w);
		rot = new Quaternion(rot.x * num, rot.y * num, rot.z * num, rot.w * num);
		if (autoSpawn.isOn && spawnTime + speedSlider.value < Time.time)
		{
			Spawn();
			spawnTime = Time.time;
		}
	}

	public void Spawn()
	{
		Vector3[] array = spawnPoints;
		foreach (Vector3 position in array)
		{
			spawner.Spawn(position, rot, bulletScale);
			spawner.Spawn(position, rot * right, bulletScale);
			spawner.Spawn(position, rot * left, bulletScale);
			spawner.Spawn(position, rot * back, bulletScale);
		}
	}
}
