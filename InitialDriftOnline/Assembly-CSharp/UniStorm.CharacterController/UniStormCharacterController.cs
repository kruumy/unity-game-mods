using UnityEngine;

namespace UniStorm.CharacterController;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
public class UniStormCharacterController : MonoBehaviour
{
	public float walkSpeed = 6f;

	public float runSpeed = 12f;

	public float gravity = 10f;

	public float maxVelocityChange = 10f;

	public bool canJump = true;

	public float jumpHeight = 2f;

	public bool onlyJumpOnUntagged = true;

	public AudioClip footStepSound;

	public float runFootStepSeconds = 0.5f;

	public float walkFootStepSeconds = 0.75f;

	private float footStepTimer;

	private AudioSource audioSource;

	private bool grounded;

	private float rayDistance;

	private RaycastHit hit;

	private Rigidbody rb;

	private Vector3 velocity;

	private Vector3 velocityChange;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;
		rb.useGravity = false;
	}

	private void FixedUpdate()
	{
		if (grounded)
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
				direction = base.transform.TransformDirection(direction);
				direction *= runSpeed;
				velocity = rb.velocity;
				velocityChange = direction - velocity;
				velocityChange.x = Mathf.Clamp(velocityChange.x, 0f - maxVelocityChange, maxVelocityChange);
				velocityChange.z = Mathf.Clamp(velocityChange.z, 0f - maxVelocityChange, maxVelocityChange);
				velocityChange.y = 0f;
				rb.AddForce(velocityChange, ForceMode.VelocityChange);
				if (Input.GetKey(KeyCode.W))
				{
					footStepTimer += Time.deltaTime;
				}
				if (footStepTimer >= runFootStepSeconds && audioSource != null)
				{
					audioSource.pitch = Random.Range(0.9f, 1.1f);
					audioSource.PlayOneShot(footStepSound);
					footStepTimer = 0f;
				}
			}
			if (!Input.GetKey(KeyCode.LeftShift))
			{
				Vector3 direction2 = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
				direction2 = base.transform.TransformDirection(direction2);
				direction2 *= walkSpeed;
				velocity = rb.velocity;
				velocityChange = direction2 - velocity;
				velocityChange.x = Mathf.Clamp(velocityChange.x, 0f - maxVelocityChange, maxVelocityChange);
				velocityChange.z = Mathf.Clamp(velocityChange.z, 0f - maxVelocityChange, maxVelocityChange);
				velocityChange.y = 0f;
				rb.AddForce(velocityChange, ForceMode.VelocityChange);
				if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
				{
					footStepTimer += Time.deltaTime;
					if (footStepTimer >= walkFootStepSeconds && audioSource != null)
					{
						audioSource.pitch = Random.Range(0.9f, 1.1f);
						audioSource.PlayOneShot(footStepSound);
						footStepTimer = 0f;
					}
				}
			}
			if (canJump && Input.GetButton("Jump"))
			{
				rb.velocity = new Vector3(velocity.x, CalculateJumpVerticalwalkSpeed(), velocity.z);
			}
		}
		rb.AddForce(new Vector3(0f, (0f - gravity) * rb.mass, 0f));
		grounded = false;
	}

	private void OnCollisionStay(Collision col)
	{
		if (col.gameObject.tag == "Untagged" || !onlyJumpOnUntagged)
		{
			grounded = true;
		}
	}

	private float CalculateJumpVerticalwalkSpeed()
	{
		return Mathf.Sqrt(2f * jumpHeight * gravity);
	}
}
