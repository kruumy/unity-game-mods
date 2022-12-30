using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/RCC Animator Controller")]
public class RCC_CharacterController : MonoBehaviour
{
	private RCC_CarControllerV3 carController;

	private Rigidbody carRigid;

	public Animator animator;

	public string driverSteeringParameter;

	public string driverShiftingGearParameter;

	public string driverDangerParameter;

	public string driverReversingParameter;

	public float steerInput;

	public float directionInput;

	public bool reversing;

	public float impactInput;

	public float gearInput;

	private void Start()
	{
		if (!animator)
		{
			animator = GetComponentInChildren<Animator>();
		}
		carController = GetComponent<RCC_CarControllerV3>();
		carRigid = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		steerInput = Mathf.Lerp(steerInput, carController.steerInput, Time.deltaTime * 5f);
		directionInput = carRigid.transform.InverseTransformDirection(carRigid.velocity).z;
		impactInput -= Time.deltaTime * 5f;
		if (impactInput < 0f)
		{
			impactInput = 0f;
		}
		if (impactInput > 1f)
		{
			impactInput = 1f;
		}
		if (directionInput <= -2f)
		{
			reversing = true;
		}
		else if (directionInput > -1f)
		{
			reversing = false;
		}
		if (carController.changingGear)
		{
			gearInput = 1f;
		}
		else
		{
			gearInput -= Time.deltaTime * 5f;
		}
		if (gearInput < 0f)
		{
			gearInput = 0f;
		}
		if (gearInput > 1f)
		{
			gearInput = 1f;
		}
		if (!reversing)
		{
			animator.SetBool(driverReversingParameter, value: false);
		}
		else
		{
			animator.SetBool(driverReversingParameter, value: true);
		}
		if (impactInput > 0.5f)
		{
			animator.SetBool(driverDangerParameter, value: true);
		}
		else
		{
			animator.SetBool(driverDangerParameter, value: false);
		}
		if (gearInput > 0.5f)
		{
			animator.SetBool(driverShiftingGearParameter, value: true);
		}
		else
		{
			animator.SetBool(driverShiftingGearParameter, value: false);
		}
		animator.SetFloat(driverSteeringParameter, steerInput);
	}

	private void OnCollisionEnter(Collision col)
	{
		if (!(col.relativeVelocity.magnitude < 2.5f))
		{
			impactInput = 1f;
		}
	}
}
