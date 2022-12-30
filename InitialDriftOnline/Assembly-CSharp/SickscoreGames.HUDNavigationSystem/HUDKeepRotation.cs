using UnityEngine;

namespace SickscoreGames.HUDNavigationSystem;

public class HUDKeepRotation : MonoBehaviour
{
	private Transform _transform;

	private Quaternion _rotation;

	private void Awake()
	{
		_transform = base.transform;
		_rotation = _transform.rotation;
	}

	private void LateUpdate()
	{
		_transform.rotation = _rotation;
	}
}
