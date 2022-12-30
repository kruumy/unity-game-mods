using UnityEngine;

namespace SickscoreGames.ExampleScene;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class ExampleController : MonoBehaviour
{
	public float walkSpeed = 8f;

	public float runSpeed = 12f;

	public float jumpHeight = 3.25f;

	public float gravity = 28f;

	private Transform _transform;

	private Rigidbody _rigidbody;

	private bool isGrounded;

	private void Awake()
	{
		_transform = base.transform;
		_rigidbody = GetComponent<Rigidbody>();
		_rigidbody.freezeRotation = true;
		_rigidbody.useGravity = false;
	}

	private void FixedUpdate()
	{
		if (isGrounded)
		{
			Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
			float num = (Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed);
			direction = _transform.TransformDirection(direction) * num;
			Vector3 velocity = _rigidbody.velocity;
			Vector3 force = direction - velocity;
			force.x = Mathf.Clamp(force.x, -8f, 8f);
			force.z = Mathf.Clamp(force.z, -8f, 8f);
			force.y = 0f;
			_rigidbody.AddForce(force, ForceMode.VelocityChange);
			if (Input.GetKeyDown(KeyCode.Space))
			{
				_rigidbody.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
			}
		}
		_rigidbody.AddForce(new Vector3(0f, (0f - gravity) * _rigidbody.mass, 0f));
		isGrounded = false;
	}

	private void OnCollisionStay()
	{
		isGrounded = true;
	}

	private float CalculateJumpVerticalSpeed()
	{
		return Mathf.Sqrt(2f * jumpHeight * gravity);
	}
}
