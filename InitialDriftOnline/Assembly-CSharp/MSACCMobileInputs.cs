using UnityEngine;
using UnityEngine.UI;

public class MSACCMobileInputs : MonoBehaviour
{
	[Header("Controller")]
	[Tooltip("Here you must associate a camera controller of type 'MSCameraController' that will receive the inputs of this object.")]
	public MSCameraController cameraController;

	[Header("Settings")]
	[Tooltip("Here it is possible to define the speed at which the variable that controls the scroll will be incremented or decremented by the buttons.")]
	[Range(0.1f, 1f)]
	public float sensibilityScroll = 0.5f;

	[Range(0.1f, 1f)]
	[Tooltip("Here you can set the X-axis sensitivity of the virtual joystick.")]
	public float sensibilityX = 0.5f;

	[Range(0.1f, 1f)]
	[Tooltip("Here you can set the Y-axis sensitivity of the virtual joystick.")]
	public float sensibilityY = 0.5f;

	[Tooltip("If this variable is true, the X-axis inputs of the virtual joystick will be reversed.")]
	public bool invertJoystickX;

	[Tooltip("If this variable is true, the Y-axis inputs of the virtual joystick will be reversed.")]
	public bool invertJoystickY;

	private MSACCJoystick joystick;

	private MSACCButtons scrollUpButton;

	private MSACCButtons scrollDownButton;

	private Button changeCamerasButton;

	private Vector2 joystickInput;

	private float scrollInput;

	private bool error;

	private void Awake()
	{
		error = false;
		if ((bool)cameraController)
		{
			joystick = base.transform.root.Find("Canvas/Joystick").GetComponent<MSACCJoystick>();
			scrollUpButton = base.transform.root.Find("Canvas/scrollUp").GetComponent<MSACCButtons>();
			scrollDownButton = base.transform.root.Find("Canvas/scrollDown").GetComponent<MSACCButtons>();
			changeCamerasButton = base.transform.root.Find("Canvas/changeCamerasButton").GetComponent<Button>();
			if (!joystick)
			{
				Debug.LogWarning("The prefab " + base.transform.root.name + " had its structure modified and this interferes in the correct functioning of the code. The controller will be disabled to avoid problems.");
				error = true;
				base.transform.root.gameObject.SetActive(value: false);
			}
			if ((bool)changeCamerasButton)
			{
				changeCamerasButton.onClick = new Button.ButtonClickedEvent();
				changeCamerasButton.onClick.AddListener(delegate
				{
					cameraController.MSADCCChangeCameras();
				});
			}
			else
			{
				Debug.LogWarning("The prefab " + base.transform.root.name + " had its structure modified and this interferes in the correct functioning of the code. The controller will be disabled to avoid problems.");
				error = true;
				base.transform.root.gameObject.SetActive(value: false);
			}
			if (!scrollUpButton)
			{
				Debug.LogWarning("The prefab " + base.transform.root.name + " had its structure modified and this interferes in the correct functioning of the code. The controller will be disabled to avoid problems.");
				error = true;
				base.transform.root.gameObject.SetActive(value: false);
			}
			if (!scrollDownButton)
			{
				Debug.LogWarning("The prefab " + base.transform.root.name + " had its structure modified and this interferes in the correct functioning of the code. The controller will be disabled to avoid problems.");
				error = true;
				base.transform.root.gameObject.SetActive(value: false);
			}
		}
		else
		{
			Debug.LogWarning("No 'camera controller' was associated to object " + base.transform.root.name + ", so it was disabled from the scene.");
			error = true;
			base.transform.root.gameObject.SetActive(value: false);
		}
	}

	private void Update()
	{
		if (!error)
		{
			cameraController._enableMobileInputs = true;
			EnableMobileInputs(cameraController._mobileInputsIndex);
			joystickInput = new Vector2(joystick.joystickX, joystick.joystickY);
			if (invertJoystickX)
			{
				joystickInput = new Vector2(0f - joystickInput.x, joystickInput.y);
			}
			if (invertJoystickY)
			{
				joystickInput = new Vector2(joystickInput.x, 0f - joystickInput.y);
			}
			scrollInput = (scrollUpButton.input - scrollDownButton.input) * 0.02f;
			cameraController._horizontalInputMSACC = joystickInput.x * sensibilityX;
			cameraController._verticalInputMSACC = joystickInput.y * sensibilityY;
			cameraController._scrollInputMSACC = scrollInput * sensibilityScroll;
		}
	}

	private void EnableMobileInputs(int index)
	{
		if (index == 0)
		{
			joystick.gameObject.SetActive(value: false);
			scrollUpButton.gameObject.SetActive(value: false);
			scrollDownButton.gameObject.SetActive(value: false);
		}
		if (index == 1)
		{
			joystick.gameObject.SetActive(value: true);
			scrollUpButton.gameObject.SetActive(value: true);
			scrollDownButton.gameObject.SetActive(value: true);
		}
		if (index == 2)
		{
			joystick.gameObject.SetActive(value: false);
			scrollUpButton.gameObject.SetActive(value: true);
			scrollDownButton.gameObject.SetActive(value: true);
		}
	}
}
