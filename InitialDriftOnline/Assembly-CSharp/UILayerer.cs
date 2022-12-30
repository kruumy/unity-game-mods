using UnityEngine;

[AddComponentMenu("Scripts/SuperVHS/UI Layerer")]
[ExecuteInEditMode]
public class UILayerer : MonoBehaviour
{
	public Canvas TargetCanvas;

	private RenderTexture uiTargetTexture;

	private Camera uiCamera;

	private void OnEnable()
	{
		GameObject gameObject = new GameObject("SuperVHSUICamera", typeof(Camera));
		uiCamera = gameObject.GetComponent<Camera>();
		uiTargetTexture = new RenderTexture(Camera.main.pixelWidth, Camera.main.pixelHeight, 1);
		gameObject.hideFlags = HideFlags.HideAndDontSave;
		uiCamera.depth = -128f;
		uiCamera.clearFlags = CameraClearFlags.Color;
		uiCamera.backgroundColor = new Color(1f, 1f, 1f, 0f);
		uiCamera.cullingMask = LayerMask.GetMask("UI");
		uiCamera.targetTexture = uiTargetTexture;
		uiCamera.orthographic = true;
		uiCamera.orthographicSize = 2f;
		if (TargetCanvas == null)
		{
			Debug.LogError("SuperVHS - UI Layering is enabled, but TargetCanvas is null.");
			return;
		}
		TargetCanvas.renderMode = RenderMode.ScreenSpaceCamera;
		TargetCanvas.worldCamera = uiCamera;
	}

	private void OnDisable()
	{
		uiTargetTexture.Release();
		Object.DestroyImmediate(uiCamera.gameObject);
	}

	public RenderTexture CaptureUI()
	{
		if (uiCamera != null)
		{
			return uiCamera.activeTexture;
		}
		return null;
	}

	public void Refresh()
	{
		if (uiCamera == null)
		{
			OnEnable();
			return;
		}
		uiTargetTexture = new RenderTexture(Camera.main.pixelWidth, Camera.main.pixelHeight, 1);
		uiCamera.targetTexture = uiTargetTexture;
		if (base.gameObject.activeSelf)
		{
			TargetCanvas.worldCamera = uiCamera;
		}
	}
}
