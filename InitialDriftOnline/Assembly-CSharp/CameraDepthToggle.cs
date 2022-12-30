using UnityEngine;

[ExecuteInEditMode]
public class CameraDepthToggle : MonoBehaviour
{
	private void OnEnable()
	{
		Camera component = GetComponent<Camera>();
		if (component != null)
		{
			component.depthTextureMode |= DepthTextureMode.Depth;
		}
	}
}
