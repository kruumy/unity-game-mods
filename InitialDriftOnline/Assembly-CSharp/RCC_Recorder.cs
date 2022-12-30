using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/RCC Recorder")]
public class RCC_Recorder : MonoBehaviour
{
	[Serializable]
	public class Recorded
	{
		public string recordName = "New Record";

		[HideInInspector]
		public PlayerInput[] inputs;

		[HideInInspector]
		public PlayerTransform[] transforms;

		[HideInInspector]
		public PlayerRigidBody[] rigids;

		public Recorded(PlayerInput[] _inputs, PlayerTransform[] _transforms, PlayerRigidBody[] _rigids, string _recordName)
		{
			inputs = _inputs;
			transforms = _transforms;
			rigids = _rigids;
			recordName = _recordName;
		}
	}

	[Serializable]
	public class PlayerInput
	{
		public float gasInput;

		public float brakeInput;

		public float steerInput;

		public float handbrakeInput;

		public float clutchInput;

		public float boostInput;

		public float idleInput;

		public float fuelInput;

		public int direction = 1;

		public bool canGoReverse;

		public int currentGear;

		public bool changingGear;

		public RCC_CarControllerV3.IndicatorsOn indicatorsOn;

		public bool lowBeamHeadLightsOn;

		public bool highBeamHeadLightsOn;

		public PlayerInput(float _gasInput, float _brakeInput, float _steerInput, float _handbrakeInput, float _clutchInput, float _boostInput, float _idleInput, float _fuelInput, int _direction, bool _canGoReverse, int _currentGear, bool _changingGear, RCC_CarControllerV3.IndicatorsOn _indicatorsOn, bool _lowBeamHeadLightsOn, bool _highBeamHeadLightsOn)
		{
			gasInput = _gasInput;
			brakeInput = _brakeInput;
			steerInput = _steerInput;
			handbrakeInput = _handbrakeInput;
			clutchInput = _clutchInput;
			boostInput = _boostInput;
			idleInput = _idleInput;
			fuelInput = _fuelInput;
			direction = _direction;
			canGoReverse = _canGoReverse;
			currentGear = _currentGear;
			changingGear = _changingGear;
			indicatorsOn = _indicatorsOn;
			lowBeamHeadLightsOn = _lowBeamHeadLightsOn;
			highBeamHeadLightsOn = _highBeamHeadLightsOn;
		}
	}

	[Serializable]
	public class PlayerTransform
	{
		public Vector3 position;

		public Quaternion rotation;

		public PlayerTransform(Vector3 _pos, Quaternion _rot)
		{
			position = _pos;
			rotation = _rot;
		}
	}

	[Serializable]
	public class PlayerRigidBody
	{
		public Vector3 velocity;

		public Vector3 angularVelocity;

		public PlayerRigidBody(Vector3 _vel, Vector3 _angVel)
		{
			velocity = _vel;
			angularVelocity = _angVel;
		}
	}

	public enum Mode
	{
		Neutral,
		Play,
		Record
	}

	public Recorded recorded;

	public RCC_CarControllerV3 carController;

	public List<PlayerInput> Inputs = new List<PlayerInput>();

	public List<PlayerTransform> Transforms = new List<PlayerTransform>();

	public List<PlayerRigidBody> RigidBodies = new List<PlayerRigidBody>();

	public Mode mode;

	public void Record()
	{
		if (mode != Mode.Record)
		{
			mode = Mode.Record;
			Debug.Log("RECORDING EN COURS");
		}
		else
		{
			mode = Mode.Neutral;
			Debug.Log("SAVE RECORD");
			SaveRecord();
		}
		if (mode == Mode.Record)
		{
			Inputs.Clear();
			Transforms.Clear();
			RigidBodies.Clear();
		}
	}

	public void SaveRecord()
	{
		MonoBehaviour.print("Record saved!");
		recorded = new Recorded(Inputs.ToArray(), Transforms.ToArray(), RigidBodies.ToArray(), RCC_Records.Instance.records.Count + "_" + carController.transform.name);
		RCC_Records.Instance.records.Add(recorded);
	}

	public void Play()
	{
		if (recorded == null)
		{
			return;
		}
		if (mode != Mode.Play)
		{
			mode = Mode.Play;
		}
		else
		{
			mode = Mode.Neutral;
		}
		if (mode == Mode.Play)
		{
			carController.externalController = true;
		}
		else
		{
			carController.externalController = false;
		}
		if (mode == Mode.Play)
		{
			StartCoroutine(Replay());
			if (recorded != null && recorded.transforms.Length != 0)
			{
				carController.transform.position = recorded.transforms[0].position;
				carController.transform.rotation = recorded.transforms[0].rotation;
			}
			StartCoroutine(Revel());
		}
	}

	public void Play(Recorded _recorded)
	{
		recorded = _recorded;
		MonoBehaviour.print("Replaying record " + recorded.recordName);
		if (recorded == null)
		{
			return;
		}
		if (mode != Mode.Play)
		{
			mode = Mode.Play;
		}
		else
		{
			mode = Mode.Neutral;
		}
		if (mode == Mode.Play)
		{
			carController.externalController = true;
		}
		else
		{
			carController.externalController = false;
		}
		if (mode == Mode.Play)
		{
			StartCoroutine(Replay());
			if (recorded != null && recorded.transforms.Length != 0)
			{
				carController.transform.position = recorded.transforms[0].position;
				carController.transform.rotation = recorded.transforms[0].rotation;
			}
			StartCoroutine(Revel());
		}
	}

	public void Stop()
	{
		mode = Mode.Neutral;
		carController.externalController = false;
	}

	private IEnumerator Replay()
	{
		for (int i = 0; i < recorded.inputs.Length; i++)
		{
			if (mode != Mode.Play)
			{
				break;
			}
			carController.externalController = true;
			carController.gasInput = recorded.inputs[i].gasInput;
			carController.brakeInput = recorded.inputs[i].brakeInput;
			carController.steerInput = recorded.inputs[i].steerInput;
			carController.handbrakeInput = recorded.inputs[i].handbrakeInput;
			carController.clutchInput = recorded.inputs[i].clutchInput;
			carController.boostInput = recorded.inputs[i].boostInput;
			carController.idleInput = recorded.inputs[i].idleInput;
			carController.fuelInput = recorded.inputs[i].fuelInput;
			carController.direction = recorded.inputs[i].direction;
			carController.canGoReverseNow = recorded.inputs[i].canGoReverse;
			carController.currentGear = recorded.inputs[i].currentGear;
			carController.changingGear = recorded.inputs[i].changingGear;
			carController.indicatorsOn = recorded.inputs[i].indicatorsOn;
			carController.lowBeamHeadLightsOn = recorded.inputs[i].lowBeamHeadLightsOn;
			carController.highBeamHeadLightsOn = recorded.inputs[i].highBeamHeadLightsOn;
			yield return new WaitForFixedUpdate();
		}
		mode = Mode.Neutral;
		carController.externalController = false;
	}

	private IEnumerator Repos()
	{
		for (int i = 0; i < recorded.transforms.Length; i++)
		{
			if (mode != Mode.Play)
			{
				break;
			}
			carController.transform.position = recorded.transforms[i].position;
			carController.transform.rotation = recorded.transforms[i].rotation;
			yield return new WaitForEndOfFrame();
		}
		mode = Mode.Neutral;
		carController.externalController = false;
	}

	private IEnumerator Revel()
	{
		for (int i = 0; i < recorded.rigids.Length; i++)
		{
			if (mode != Mode.Play)
			{
				break;
			}
			carController.rigid.velocity = recorded.rigids[i].velocity;
			carController.rigid.angularVelocity = recorded.rigids[i].angularVelocity;
			yield return new WaitForFixedUpdate();
		}
		mode = Mode.Neutral;
		carController.externalController = false;
	}

	private void FixedUpdate()
	{
		carController = RCC_SceneManager.Instance.activePlayerVehicle;
		if ((bool)carController)
		{
			switch (mode)
			{
			case Mode.Play:
				carController.externalController = true;
				break;
			case Mode.Record:
				Inputs.Add(new PlayerInput(carController.gasInput, carController.brakeInput, carController.steerInput, carController.handbrakeInput, carController.clutchInput, carController.boostInput, carController.idleInput, carController.fuelInput, carController.direction, carController.canGoReverseNow, carController.currentGear, carController.changingGear, carController.indicatorsOn, carController.lowBeamHeadLightsOn, carController.highBeamHeadLightsOn));
				Transforms.Add(new PlayerTransform(carController.transform.position, carController.transform.rotation));
				RigidBodies.Add(new PlayerRigidBody(carController.rigid.velocity, carController.rigid.angularVelocity));
				break;
			case Mode.Neutral:
				break;
			}
		}
	}
}
