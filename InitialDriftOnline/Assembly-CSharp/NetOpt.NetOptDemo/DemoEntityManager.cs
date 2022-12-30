using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace NetOpt.NetOptDemo;

public class DemoEntityManager : MonoBehaviour
{
	public enum DemoSerializer
	{
		BinaryFormatter,
		BinaryWriter,
		PACKR,
		PACKRQUANT,
		PACKRQUANTCOMPR,
		PACKRQUANTDELTACOMPR
	}

	public Camera cam;

	private Bounds bounds = new Bounds(Vector3.zero, new Vector3(19f, 1f, 19f));

	public GameObject entityPrefab;

	public List<DemoEntity> entities;

	public IDemoSerializer serializer;

	public Text dataStatText;

	public Text serializeStatText;

	public Text deserializeStatText;

	public Text entityStatText;

	private const float TickRate = 60f;

	private float timer;

	public void OnDropdownChange(Dropdown change)
	{
		switch (change.value)
		{
		case 0:
			SetSerializer(new BinaryFormatterSerializer());
			break;
		case 1:
			SetSerializer(new BinaryWriterReaderSerializer());
			break;
		case 2:
			SetSerializer(new PackPantherSerializer());
			break;
		case 3:
			SetSerializer(new PackPantherQuantizedSerializer());
			break;
		case 4:
			SetSerializer(new PackPantherQuantizedCompressedSerializer());
			break;
		case 5:
			SetSerializer(new PackPantherQuantizedDiffSerializer());
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public void SetSerializer(IDemoSerializer serializer)
	{
		this.serializer = serializer;
		this.serializer.Initialize();
	}

	private void Start()
	{
		SetSerializer(new BinaryFormatterSerializer());
		timer = Time.fixedUnscaledTime;
	}

	private void Update()
	{
		if (Time.fixedUnscaledTime > timer)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			int num = serializer.Serialize(entities);
			double totalMilliseconds = stopwatch.Elapsed.TotalMilliseconds;
			serializeStatText.text = $"Time/serialize (ms): {totalMilliseconds:N5}";
			dataStatText.text = $"Kbit/sec (Kbps): {(float)num * 8f / 1000f * 60f:N0}";
			Stopwatch stopwatch2 = Stopwatch.StartNew();
			serializer.Deserialize(entities);
			double totalMilliseconds2 = stopwatch2.Elapsed.TotalMilliseconds;
			deserializeStatText.text = $"Time/deserialize (ms): {totalMilliseconds2:N5}";
			entityStatText.text = $"Entities (transforms): {entities.Count:N0}";
			timer += 1f / 60f;
			UserInput();
		}
	}

	private void UserInput()
	{
		if (Input.GetMouseButton(0))
		{
			if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out var hitInfo) && bounds.Contains(hitInfo.point))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(entityPrefab, hitInfo.point, Quaternion.identity);
				entities.Add(gameObject.GetComponent<DemoEntity>());
			}
		}
		else if (Input.GetMouseButton(1) && entities.Count > 0)
		{
			UnityEngine.Object.Destroy(entities[entities.Count - 1].gameObject);
			entities.RemoveAt(entities.Count - 1);
		}
	}
}
