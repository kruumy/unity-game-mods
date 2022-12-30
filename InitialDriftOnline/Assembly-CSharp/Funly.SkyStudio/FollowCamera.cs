using UnityEngine;

namespace Funly.SkyStudio;

[ExecuteInEditMode]
public class FollowCamera : MonoBehaviour
{
	public Camera followCamera;

	public Vector3 offset = Vector3.zero;

	private void Update()
	{
		Camera camera = ((!(followCamera != null)) ? Camera.main : followCamera);
		if (!(camera == null))
		{
			base.transform.position = camera.transform.TransformPoint(offset);
		}
	}
}
