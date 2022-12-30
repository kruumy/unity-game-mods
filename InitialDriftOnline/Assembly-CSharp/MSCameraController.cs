using UnityEngine;

public class MSCameraController : MonoBehaviour
{
	[Tooltip("Here you must associate the object that the cameras will follow. If you leave this variable empty, the cameras will follow the object in which this script was placed.")]
	public Transform target;

	[Tooltip("In this variable, it is possible to define which will be the first camera used by the player, in case several cameras are being used.")]
	public int startCameraIndex;

	[Space(7f)]
	[Tooltip("Here you must associate all the cameras that you want to control by this script, associating each one with an index and selecting your preferences.")]
	public MSACC_CameraType[] cameras = new MSACC_CameraType[0];

	[Tooltip("Here you can configure the cameras, deciding their speed of movement, rotation, zoom, among other options.")]
	public MSACC_CameraSetting cameraSettings;

	private bool orbitalAtiv;

	private bool orbital_AtivTemp;

	private float rotacX;

	private float rotacY;

	private float tempoOrbit;

	private float rotacXETS;

	private float rotacYETS;

	private Vector2 cameraRotationFly;

	private bool changeCam;

	private GameObject[] objPosicStopCameras;

	private Quaternion[] originalRotation;

	private GameObject[] originalPosition;

	private Vector3[] originalPositionETS;

	private float[] xOrbit;

	private float[] yOrbit;

	private float[] distanceFromOrbitalCamera;

	private float[] initialFieldOfView;

	private float[] camFollowPlayerDistance;

	private int index;

	private int lastIndex;

	private Transform targetTransform;

	private GameObject playerCamsObj;

	[HideInInspector]
	public float _horizontalInputMSACC;

	[HideInInspector]
	public float _verticalInputMSACC;

	[HideInInspector]
	public float _scrollInputMSACC;

	[HideInInspector]
	public bool _enableMobileInputs;

	[HideInInspector]
	public int _mobileInputsIndex;

	private void OnValidate()
	{
		startCameraIndex = Mathf.Clamp(startCameraIndex, 0, cameras.Length - 1);
		if (cameras != null)
		{
			for (int i = 0; i < cameras.Length; i++)
			{
				if (cameras[i].volume == 0f)
				{
					cameras[i].volume = 1f;
				}
			}
		}
		if (cameraSettings == null)
		{
			return;
		}
		if (cameraSettings.PlaneX_Z != null)
		{
			cameraSettings.PlaneX_Z.minXPosition = Mathf.Clamp(cameraSettings.PlaneX_Z.minXPosition, -99999f, cameraSettings.PlaneX_Z.maxXPosition - 10f);
			cameraSettings.PlaneX_Z.maxXPosition = Mathf.Clamp(cameraSettings.PlaneX_Z.maxXPosition, cameraSettings.PlaneX_Z.minXPosition + 10f, 99999f);
			cameraSettings.PlaneX_Z.minZPosition = Mathf.Clamp(cameraSettings.PlaneX_Z.minZPosition, -99999f, cameraSettings.PlaneX_Z.maxZPosition - 10f);
			cameraSettings.PlaneX_Z.maxZPosition = Mathf.Clamp(cameraSettings.PlaneX_Z.maxZPosition, cameraSettings.PlaneX_Z.minZPosition + 10f, 99999f);
			cameraSettings.PlaneX_Z.normalYPosition = Mathf.Clamp(cameraSettings.PlaneX_Z.normalYPosition, cameraSettings.PlaneX_Z.limitYPosition, 99999f);
			cameraSettings.PlaneX_Z.limitYPosition = Mathf.Clamp(cameraSettings.PlaneX_Z.limitYPosition, -99999f, cameraSettings.PlaneX_Z.normalYPosition);
			float num = 99999f;
			float num2 = (cameraSettings.PlaneX_Z.maxXPosition - cameraSettings.PlaneX_Z.minXPosition) * 0.25f;
			float num3 = (cameraSettings.PlaneX_Z.maxZPosition - cameraSettings.PlaneX_Z.minZPosition) * 0.25f;
			if (num2 < num)
			{
				num = num2;
			}
			if (num3 < num)
			{
				num = num3;
			}
			cameraSettings.PlaneX_Z.edgeDistanceToStartDescending = Mathf.Clamp(cameraSettings.PlaneX_Z.edgeDistanceToStartDescending, 1f, num);
		}
		if (cameraSettings.followPlayer != null)
		{
			cameraSettings.followPlayer.minDistance = Mathf.Clamp(cameraSettings.followPlayer.minDistance, 1f, cameraSettings.followPlayer.maxDistance);
			cameraSettings.followPlayer.maxDistance = Mathf.Clamp(cameraSettings.followPlayer.maxDistance, cameraSettings.followPlayer.minDistance, 200f);
		}
	}

	private void OnDrawGizmosSelected()
	{
		bool flag = false;
		if (cameras != null)
		{
			for (int i = 0; i < cameras.Length; i++)
			{
				if (cameras[i].rotationType == MSACC_CameraType.TipoRotac.PlaneX_Z)
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			Gizmos.color = Color.red;
			Vector3 from = new Vector3(cameraSettings.PlaneX_Z.minXPosition, cameraSettings.PlaneX_Z.limitYPosition, cameraSettings.PlaneX_Z.minZPosition);
			Vector3 vector = new Vector3(cameraSettings.PlaneX_Z.maxXPosition, cameraSettings.PlaneX_Z.limitYPosition, cameraSettings.PlaneX_Z.minZPosition);
			Vector3 vector2 = new Vector3(cameraSettings.PlaneX_Z.minXPosition, cameraSettings.PlaneX_Z.limitYPosition, cameraSettings.PlaneX_Z.maxZPosition);
			Vector3 vector3 = new Vector3(cameraSettings.PlaneX_Z.maxXPosition, cameraSettings.PlaneX_Z.limitYPosition, cameraSettings.PlaneX_Z.maxZPosition);
			Gizmos.DrawLine(from, vector);
			Gizmos.DrawLine(from, vector2);
			Gizmos.DrawLine(vector2, vector3);
			Gizmos.DrawLine(vector3, vector);
			Gizmos.color = Color.green;
			float edgeDistanceToStartDescending = cameraSettings.PlaneX_Z.edgeDistanceToStartDescending;
			Vector3 vector4 = new Vector3(cameraSettings.PlaneX_Z.minXPosition + edgeDistanceToStartDescending, cameraSettings.PlaneX_Z.normalYPosition, cameraSettings.PlaneX_Z.minZPosition + edgeDistanceToStartDescending);
			Vector3 to = new Vector3(cameraSettings.PlaneX_Z.maxXPosition - edgeDistanceToStartDescending, cameraSettings.PlaneX_Z.normalYPosition, cameraSettings.PlaneX_Z.minZPosition + edgeDistanceToStartDescending);
			Vector3 vector5 = new Vector3(cameraSettings.PlaneX_Z.minXPosition + edgeDistanceToStartDescending, cameraSettings.PlaneX_Z.normalYPosition, cameraSettings.PlaneX_Z.maxZPosition - edgeDistanceToStartDescending);
			Vector3 vector6 = new Vector3(cameraSettings.PlaneX_Z.maxXPosition - edgeDistanceToStartDescending, cameraSettings.PlaneX_Z.normalYPosition, cameraSettings.PlaneX_Z.maxZPosition - edgeDistanceToStartDescending);
			Gizmos.DrawLine(vector4, to);
			Gizmos.DrawLine(vector4, vector5);
			Gizmos.DrawLine(vector5, vector6);
			Gizmos.DrawLine(vector6, to);
			Gizmos.color = Color.red;
			Gizmos.DrawLine(from, vector4);
			Gizmos.DrawLine(vector, to);
			Gizmos.DrawLine(vector2, vector5);
			Gizmos.DrawLine(vector3, vector6);
		}
	}

	private void Awake()
	{
		if ((bool)target)
		{
			targetTransform = target;
		}
		else
		{
			targetTransform = base.transform;
		}
		GameObject gameObject = new GameObject("PlayerCams");
		gameObject.transform.parent = targetTransform;
		objPosicStopCameras = new GameObject[cameras.Length];
		originalRotation = new Quaternion[cameras.Length];
		originalPosition = new GameObject[cameras.Length];
		originalPositionETS = new Vector3[cameras.Length];
		xOrbit = new float[cameras.Length];
		yOrbit = new float[cameras.Length];
		distanceFromOrbitalCamera = new float[cameras.Length];
		initialFieldOfView = new float[cameras.Length];
		camFollowPlayerDistance = new float[cameras.Length];
		changeCam = false;
		orbitalAtiv = false;
		orbital_AtivTemp = false;
		for (int i = 0; i < cameras.Length; i++)
		{
			if ((bool)cameras[i]._camera)
			{
				if (cameras[i].volume == 0f)
				{
					cameras[i].volume = 1f;
				}
				initialFieldOfView[i] = cameras[i]._camera.fieldOfView;
				if (cameras[i].rotationType == MSACC_CameraType.TipoRotac.FirstPerson)
				{
					cameras[i]._camera.transform.parent = gameObject.transform;
					originalRotation[i] = cameras[i]._camera.transform.localRotation;
				}
				if (cameras[i].rotationType == MSACC_CameraType.TipoRotac.FollowPlayer)
				{
					cameras[i]._camera.transform.parent = gameObject.transform;
					originalPosition[i] = new GameObject("positionFollowPlayerCamera" + i);
					originalPosition[i].transform.parent = gameObject.transform;
					originalPosition[i].transform.position = cameras[i]._camera.transform.position;
					if (cameraSettings.ajustTheLayers)
					{
						targetTransform.gameObject.layer = 2;
						Transform[] componentsInChildren = targetTransform.gameObject.GetComponentsInChildren<Transform>(includeInactive: true);
						for (int j = 0; j < componentsInChildren.Length; j++)
						{
							componentsInChildren[j].gameObject.layer = 2;
						}
					}
					camFollowPlayerDistance[i] = Vector3.Distance(cameras[i]._camera.transform.position, targetTransform.position);
				}
				if (cameras[i].rotationType == MSACC_CameraType.TipoRotac.Orbital)
				{
					cameras[i]._camera.transform.parent = gameObject.transform;
					cameras[i]._camera.transform.LookAt(target);
					xOrbit[i] = cameras[i]._camera.transform.eulerAngles.y;
					yOrbit[i] = cameras[i]._camera.transform.eulerAngles.x;
					if (cameraSettings.ajustTheLayers)
					{
						targetTransform.gameObject.layer = 2;
						Transform[] componentsInChildren = targetTransform.gameObject.GetComponentsInChildren<Transform>(includeInactive: true);
						for (int j = 0; j < componentsInChildren.Length; j++)
						{
							componentsInChildren[j].gameObject.layer = 2;
						}
					}
				}
				distanceFromOrbitalCamera[i] = Vector3.Distance(cameras[i]._camera.transform.position, targetTransform.position);
				distanceFromOrbitalCamera[i] = Mathf.Clamp(distanceFromOrbitalCamera[i], cameraSettings.orbital.minDistance, cameraSettings.orbital.maxDistance);
				if (cameras[i].rotationType == MSACC_CameraType.TipoRotac.Stop)
				{
					cameras[i]._camera.transform.parent = gameObject.transform;
				}
				if (cameras[i].rotationType == MSACC_CameraType.TipoRotac.StraightStop)
				{
					cameras[i]._camera.transform.parent = gameObject.transform;
					objPosicStopCameras[i] = new GameObject("positionStraightStopCamera" + i);
					objPosicStopCameras[i].transform.parent = cameras[i]._camera.transform;
					objPosicStopCameras[i].transform.localPosition = new Vector3(0f, 0f, 1f);
					objPosicStopCameras[i].transform.parent = gameObject.transform;
				}
				if (cameras[i].rotationType == MSACC_CameraType.TipoRotac.OrbitalThatFollows)
				{
					cameras[i]._camera.transform.parent = gameObject.transform;
					xOrbit[i] = cameras[i]._camera.transform.eulerAngles.x;
					yOrbit[i] = cameras[i]._camera.transform.eulerAngles.y;
					originalPosition[i] = new GameObject("positionCameraFollowPlayer" + i);
					originalPosition[i].transform.parent = gameObject.transform;
					originalPosition[i].transform.position = cameras[i]._camera.transform.position;
					if (cameraSettings.ajustTheLayers)
					{
						targetTransform.gameObject.layer = 2;
						Transform[] componentsInChildren = targetTransform.gameObject.GetComponentsInChildren<Transform>(includeInactive: true);
						for (int j = 0; j < componentsInChildren.Length; j++)
						{
							componentsInChildren[j].gameObject.layer = 2;
						}
					}
				}
				if (cameras[i].rotationType == MSACC_CameraType.TipoRotac.ETS_StyleCamera)
				{
					cameras[i]._camera.transform.parent = gameObject.transform;
					originalRotation[i] = cameras[i]._camera.transform.localRotation;
					originalPositionETS[i] = cameras[i]._camera.transform.localPosition;
				}
				if (cameras[i].rotationType == MSACC_CameraType.TipoRotac.FlyCamera_OnlyWindows)
				{
					cameras[i]._camera.transform.parent = null;
				}
				if (cameras[i].rotationType == MSACC_CameraType.TipoRotac.LookAtThePlayer)
				{
					cameras[i]._camera.transform.parent = null;
				}
				if (cameras[i].rotationType == MSACC_CameraType.TipoRotac.PlaneX_Z)
				{
					Vector3 euler = new Vector3(90f, cameras[i]._camera.transform.eulerAngles.y, cameras[i]._camera.transform.eulerAngles.z);
					cameras[i]._camera.transform.rotation = Quaternion.Euler(euler);
					cameras[i]._camera.transform.parent = gameObject.transform;
					cameras[i]._camera.transform.position = new Vector3(targetTransform.position.x, cameraSettings.PlaneX_Z.normalYPosition, targetTransform.position.z);
				}
				if (cameras[i]._camera.GetComponent<AudioListener>() == null)
				{
					cameras[i]._camera.transform.gameObject.AddComponent(typeof(AudioListener));
				}
			}
			else
			{
				Debug.LogWarning("There is no camera associated with the index " + i);
			}
		}
		playerCamsObj = gameObject;
	}

	private void Start()
	{
		index = startCameraIndex;
		lastIndex = startCameraIndex;
		_enableMobileInputs = false;
		EnableCameras(index);
	}

	private void EnableCameras(int index)
	{
		if (cameras.Length == 0)
		{
			return;
		}
		changeCam = true;
		for (int i = 0; i < cameras.Length; i++)
		{
			if (!cameras[i]._camera)
			{
				continue;
			}
			if (i == index)
			{
				cameras[i]._camera.gameObject.SetActive(value: true);
				if (cameras[i].rotationType == MSACC_CameraType.TipoRotac.FirstPerson)
				{
					rotacX = 0f;
					rotacY = 0f;
				}
				if (cameras[i].rotationType == MSACC_CameraType.TipoRotac.OrbitalThatFollows)
				{
					tempoOrbit = 0f;
				}
				if (cameras[i].rotationType == MSACC_CameraType.TipoRotac.ETS_StyleCamera)
				{
					rotacXETS = 0f;
					rotacYETS = 0f;
				}
				if (cameras[i].rotationType == MSACC_CameraType.TipoRotac.FlyCamera_OnlyWindows)
				{
					cameras[i]._camera.transform.position = cameras[lastIndex]._camera.transform.position;
					cameraRotationFly = new Vector2(cameras[lastIndex]._camera.transform.eulerAngles.y, 0f);
				}
			}
			else
			{
				cameras[i]._camera.gameObject.SetActive(value: false);
			}
		}
	}

	private void ManageCameras()
	{
		if (changeCam)
		{
			changeCam = false;
			for (int i = 0; i < cameras.Length; i++)
			{
				if (cameras[i].rotationType == MSACC_CameraType.TipoRotac.FollowPlayer || cameras[i].rotationType == MSACC_CameraType.TipoRotac.PlaneX_Z)
				{
					if (cameras[i]._camera.isActiveAndEnabled)
					{
						cameras[i]._camera.transform.parent = null;
					}
					else
					{
						cameras[i]._camera.transform.parent = playerCamsObj.transform;
					}
				}
				if (cameras[i].rotationType == MSACC_CameraType.TipoRotac.Orbital || cameras[i].rotationType == MSACC_CameraType.TipoRotac.OrbitalThatFollows)
				{
					cameras[i]._camera.transform.LookAt(target);
					xOrbit[i] = cameras[i]._camera.transform.eulerAngles.y;
					yOrbit[i] = cameras[i]._camera.transform.eulerAngles.x;
					distanceFromOrbitalCamera[i] = Vector3.Distance(cameras[i]._camera.transform.position, targetTransform.position);
					distanceFromOrbitalCamera[i] = Mathf.Clamp(distanceFromOrbitalCamera[i], cameraSettings.orbital.minDistance, cameraSettings.orbital.maxDistance);
				}
				if (cameras[i].rotationType == MSACC_CameraType.TipoRotac.PlaneX_Z)
				{
					float x = Mathf.Clamp(targetTransform.position.x, cameraSettings.PlaneX_Z.minXPosition, cameraSettings.PlaneX_Z.maxXPosition);
					float z = Mathf.Clamp(targetTransform.position.z, cameraSettings.PlaneX_Z.minZPosition, cameraSettings.PlaneX_Z.maxZPosition);
					Vector3 position = new Vector3(x, cameraSettings.PlaneX_Z.limitYPosition, z);
					cameras[i]._camera.transform.position = position;
				}
			}
			if (cameras[index].rotationType == MSACC_CameraType.TipoRotac.Stop || cameras[index].rotationType == MSACC_CameraType.TipoRotac.StraightStop || cameras[index].rotationType == MSACC_CameraType.TipoRotac.LookAtThePlayer || cameras[index].rotationType == MSACC_CameraType.TipoRotac.FlyCamera_OnlyWindows || cameras[index].rotationType == MSACC_CameraType.TipoRotac.PlaneX_Z)
			{
				_mobileInputsIndex = 0;
			}
			if (cameras[index].rotationType == MSACC_CameraType.TipoRotac.FirstPerson || cameras[index].rotationType == MSACC_CameraType.TipoRotac.ETS_StyleCamera || cameras[index].rotationType == MSACC_CameraType.TipoRotac.Orbital || cameras[index].rotationType == MSACC_CameraType.TipoRotac.OrbitalThatFollows)
			{
				_mobileInputsIndex = 1;
			}
			if (cameras[index].rotationType == MSACC_CameraType.TipoRotac.FollowPlayer)
			{
				if (cameraSettings.followPlayer.useScrool)
				{
					_mobileInputsIndex = 2;
				}
				else
				{
					_mobileInputsIndex = 0;
				}
			}
		}
		AudioListener.volume = cameras[index].volume;
		float num = Mathf.Clamp(1f / Time.timeScale, 0.01f, 1f);
		switch (cameras[index].rotationType)
		{
		case MSACC_CameraType.TipoRotac.StraightStop:
		{
			Quaternion b9 = Quaternion.LookRotation(objPosicStopCameras[index].transform.position - cameras[index]._camera.transform.position, Vector3.up);
			cameras[index]._camera.transform.rotation = Quaternion.Slerp(cameras[index]._camera.transform.rotation, b9, Time.deltaTime * 15f);
			break;
		}
		case MSACC_CameraType.TipoRotac.LookAtThePlayer:
			cameras[index]._camera.transform.LookAt(targetTransform.position);
			break;
		case MSACC_CameraType.TipoRotac.FirstPerson:
		{
			float num14 = _horizontalInputMSACC;
			float num15 = _verticalInputMSACC;
			if (cameraSettings.firstPerson.invertXInput)
			{
				num14 = 0f - _horizontalInputMSACC;
			}
			if (cameraSettings.firstPerson.invertYInput)
			{
				num15 = 0f - _verticalInputMSACC;
			}
			if (cameraSettings.firstPerson.rotateWhenClick)
			{
				if (Input.GetKey(cameraSettings.firstPerson.keyToRotate) || _enableMobileInputs)
				{
					rotacX += num14 * cameraSettings.firstPerson.sensibilityX;
					rotacY += num15 * cameraSettings.firstPerson.sensibilityY;
				}
			}
			else
			{
				rotacX += num14 * cameraSettings.firstPerson.sensibilityX;
				rotacY += num15 * cameraSettings.firstPerson.sensibilityY;
			}
			rotacX = MSADCCClampAngle(rotacX, 0f - cameraSettings.firstPerson.horizontalAngle, cameraSettings.firstPerson.horizontalAngle);
			rotacY = MSADCCClampAngle(rotacY, 0f - cameraSettings.firstPerson.verticalAngle, cameraSettings.firstPerson.verticalAngle);
			Quaternion quaternion3 = Quaternion.AngleAxis(rotacX, Vector3.up);
			Quaternion quaternion4 = Quaternion.AngleAxis(rotacY, -Vector3.right);
			Quaternion b5 = originalRotation[index] * quaternion3 * quaternion4;
			cameras[index]._camera.transform.localRotation = Quaternion.Lerp(cameras[index]._camera.transform.localRotation, b5, Time.deltaTime * 10f * num);
			cameras[index]._camera.fieldOfView -= _scrollInputMSACC * cameraSettings.firstPerson.speedScroolZoom * 50f;
			if (cameras[index]._camera.fieldOfView < initialFieldOfView[index] - cameraSettings.firstPerson.maxScroolZoom)
			{
				cameras[index]._camera.fieldOfView = initialFieldOfView[index] - cameraSettings.firstPerson.maxScroolZoom;
			}
			if (cameras[index]._camera.fieldOfView > initialFieldOfView[index])
			{
				cameras[index]._camera.fieldOfView = initialFieldOfView[index];
			}
			break;
		}
		case MSACC_CameraType.TipoRotac.FollowPlayer:
		{
			RaycastHit hitInfo3;
			if (cameraSettings.followPlayer.useScrool)
			{
				float t = Time.deltaTime * cameraSettings.followPlayer.displacementSpeed * (camFollowPlayerDistance[index] * 0.1f);
				camFollowPlayerDistance[index] -= _scrollInputMSACC * (cameraSettings.followPlayer.scroolSpeed * 50f);
				camFollowPlayerDistance[index] = Mathf.Clamp(camFollowPlayerDistance[index], cameraSettings.followPlayer.minDistance, cameraSettings.followPlayer.maxDistance);
				Vector3 normalized = (targetTransform.position - originalPosition[index].transform.position).normalized;
				Vector3 vector4 = targetTransform.position - normalized * camFollowPlayerDistance[index];
				if (!Physics.Linecast(targetTransform.position, vector4))
				{
					cameras[index]._camera.transform.position = Vector3.Lerp(cameras[index]._camera.transform.position, vector4, t);
				}
				else if (Physics.Linecast(targetTransform.position, vector4, out hitInfo3))
				{
					if (cameraSettings.followPlayer.ignoreCollision)
					{
						cameras[index]._camera.transform.position = Vector3.Lerp(cameras[index]._camera.transform.position, vector4, t);
					}
					else
					{
						cameras[index]._camera.transform.position = Vector3.Lerp(cameras[index]._camera.transform.position, hitInfo3.point, t);
					}
				}
			}
			else if (!Physics.Linecast(targetTransform.position, originalPosition[index].transform.position))
			{
				cameras[index]._camera.transform.position = Vector3.Lerp(cameras[index]._camera.transform.position, originalPosition[index].transform.position, Time.deltaTime * cameraSettings.followPlayer.displacementSpeed);
			}
			else if (Physics.Linecast(base.transform.position, originalPosition[index].transform.position, out hitInfo3))
			{
				if (cameraSettings.followPlayer.ignoreCollision)
				{
					cameras[index]._camera.transform.position = Vector3.Lerp(cameras[index]._camera.transform.position, originalPosition[index].transform.position, Time.deltaTime * cameraSettings.followPlayer.displacementSpeed);
				}
				else
				{
					cameras[index]._camera.transform.position = Vector3.Lerp(cameras[index]._camera.transform.position, hitInfo3.point, Time.deltaTime * cameraSettings.followPlayer.displacementSpeed);
				}
			}
			if (cameraSettings.followPlayer.customLookAt)
			{
				Quaternion b8 = Quaternion.LookRotation(targetTransform.position - cameras[index]._camera.transform.position, Vector3.up);
				cameras[index]._camera.transform.rotation = Quaternion.Slerp(cameras[index]._camera.transform.rotation, b8, Time.deltaTime * cameraSettings.followPlayer.spinSpeedCustomLookAt);
			}
			else
			{
				cameras[index]._camera.transform.LookAt(targetTransform.position);
			}
			break;
		}
		case MSACC_CameraType.TipoRotac.Orbital:
		{
			float num5 = cameraSettings.orbital.minDistance;
			if (Physics.Linecast(targetTransform.position, cameras[index]._camera.transform.position, out var hitInfo) && !cameraSettings.orbital.ignoreCollision)
			{
				distanceFromOrbitalCamera[index] = Vector3.Distance(targetTransform.position, hitInfo.point);
				num5 = Mathf.Clamp(distanceFromOrbitalCamera[index], num5 * 0.5f, cameraSettings.orbital.maxDistance);
			}
			float num6 = _horizontalInputMSACC;
			float num7 = _verticalInputMSACC;
			if (cameraSettings.orbital.invertXInput)
			{
				num6 = 0f - _horizontalInputMSACC;
			}
			if (cameraSettings.orbital.invertYInput)
			{
				num7 = 0f - _verticalInputMSACC;
			}
			if (cameraSettings.orbital.rotateWhenClick)
			{
				if (Input.GetKey(cameraSettings.orbital.keyToRotate) || _enableMobileInputs)
				{
					xOrbit[index] += num6 * (cameraSettings.orbital.sensibility * distanceFromOrbitalCamera[index]) / (distanceFromOrbitalCamera[index] * 0.5f);
					yOrbit[index] -= num7 * cameraSettings.orbital.sensibility * (cameraSettings.orbital.speedYAxis * 10f);
				}
			}
			else
			{
				xOrbit[index] += num6 * (cameraSettings.orbital.sensibility * distanceFromOrbitalCamera[index]) / (distanceFromOrbitalCamera[index] * 0.5f);
				yOrbit[index] -= num7 * cameraSettings.orbital.sensibility * (cameraSettings.orbital.speedYAxis * 10f);
			}
			yOrbit[index] = MSADCCClampAngle(yOrbit[index], cameraSettings.orbital.minAngleY, cameraSettings.orbital.maxAngleY);
			Quaternion quaternion = Quaternion.Euler(yOrbit[index], xOrbit[index], 0f);
			distanceFromOrbitalCamera[index] = Mathf.Clamp(distanceFromOrbitalCamera[index] - _scrollInputMSACC * (cameraSettings.orbital.speedScrool * 50f), num5, cameraSettings.orbital.maxDistance);
			Vector3 vector2 = new Vector3(0f, 0f, 0f - distanceFromOrbitalCamera[index]);
			Vector3 b2 = quaternion * vector2 + targetTransform.position;
			Vector3 position2 = cameras[index]._camera.transform.position;
			Quaternion rotation = cameras[index]._camera.transform.rotation;
			cameras[index]._camera.transform.rotation = Quaternion.Lerp(rotation, quaternion, Time.deltaTime * 5f * num);
			cameras[index]._camera.transform.position = Vector3.Lerp(position2, b2, Time.deltaTime * 5f * num);
			break;
		}
		case MSACC_CameraType.TipoRotac.OrbitalThatFollows:
		{
			float num11 = 0f;
			float num12 = 0f;
			float scrollInputMSACC = _scrollInputMSACC;
			if (cameraSettings.OrbitalThatFollows.rotateWhenClick)
			{
				if (Input.GetKey(cameraSettings.OrbitalThatFollows.keyToRotate) || _enableMobileInputs)
				{
					num11 = _horizontalInputMSACC;
					num12 = _verticalInputMSACC;
					if (cameraSettings.OrbitalThatFollows.invertXInput)
					{
						num11 = 0f - _horizontalInputMSACC;
					}
					if (cameraSettings.OrbitalThatFollows.invertYInput)
					{
						num12 = 0f - _verticalInputMSACC;
					}
				}
			}
			else
			{
				num11 = _horizontalInputMSACC;
				num12 = _verticalInputMSACC;
				if (cameraSettings.OrbitalThatFollows.invertXInput)
				{
					num11 = 0f - _horizontalInputMSACC;
				}
				if (cameraSettings.OrbitalThatFollows.invertYInput)
				{
					num12 = 0f - _verticalInputMSACC;
				}
			}
			if (num11 > 0f || num12 > 0f || scrollInputMSACC > 0f)
			{
				orbitalAtiv = true;
				tempoOrbit = 0f;
				if (!orbital_AtivTemp)
				{
					orbital_AtivTemp = true;
					xOrbit[index] = cameras[index]._camera.transform.eulerAngles.y;
					yOrbit[index] = cameras[index]._camera.transform.eulerAngles.x;
				}
			}
			else
			{
				tempoOrbit += Time.deltaTime;
				if (tempoOrbit > cameraSettings.OrbitalThatFollows.timeToReset)
				{
					tempoOrbit = cameraSettings.OrbitalThatFollows.timeToReset + 0.1f;
				}
			}
			switch (cameraSettings.OrbitalThatFollows.ResetControlType)
			{
			case MSACC_SettingsCameraOrbitalThatFollows.ResetTimeType.Time:
				if (tempoOrbit > cameraSettings.OrbitalThatFollows.timeToReset)
				{
					orbitalAtiv = false;
					orbital_AtivTemp = false;
				}
				break;
			case MSACC_SettingsCameraOrbitalThatFollows.ResetTimeType.Input_OnlyWindows:
				if (Input.GetKeyDown(cameraSettings.OrbitalThatFollows.resetKey))
				{
					orbitalAtiv = false;
					orbital_AtivTemp = false;
				}
				break;
			}
			RaycastHit hitInfo2;
			if (orbitalAtiv)
			{
				float num13 = cameraSettings.OrbitalThatFollows.minDistance;
				if (Physics.Linecast(targetTransform.position, cameras[index]._camera.transform.position, out hitInfo2) && !cameraSettings.OrbitalThatFollows.ignoreCollision)
				{
					distanceFromOrbitalCamera[index] = Vector3.Distance(targetTransform.position, hitInfo2.point);
					num13 = Mathf.Clamp(distanceFromOrbitalCamera[index], num13 * 0.5f, cameraSettings.OrbitalThatFollows.maxDistance);
				}
				xOrbit[index] += num11 * (cameraSettings.OrbitalThatFollows.sensibility * distanceFromOrbitalCamera[index]) / (distanceFromOrbitalCamera[index] * 0.5f);
				yOrbit[index] -= num12 * cameraSettings.OrbitalThatFollows.sensibility * (cameraSettings.OrbitalThatFollows.speedYAxis * 10f);
				yOrbit[index] = MSADCCClampAngle(yOrbit[index], cameraSettings.OrbitalThatFollows.minAngleY, cameraSettings.OrbitalThatFollows.maxAngleY);
				Quaternion quaternion2 = Quaternion.Euler(yOrbit[index], xOrbit[index], 0f);
				distanceFromOrbitalCamera[index] = Mathf.Clamp(distanceFromOrbitalCamera[index] - scrollInputMSACC * (cameraSettings.OrbitalThatFollows.speedScrool * 50f), num13, cameraSettings.OrbitalThatFollows.maxDistance);
				Vector3 vector3 = new Vector3(0f, 0f, 0f - distanceFromOrbitalCamera[index]);
				Vector3 b3 = quaternion2 * vector3 + targetTransform.position;
				Vector3 position3 = cameras[index]._camera.transform.position;
				Quaternion rotation2 = cameras[index]._camera.transform.rotation;
				cameras[index]._camera.transform.rotation = Quaternion.Lerp(rotation2, quaternion2, Time.deltaTime * 5f * num);
				cameras[index]._camera.transform.position = Vector3.Lerp(position3, b3, Time.deltaTime * 5f * num);
				break;
			}
			if (!Physics.Linecast(targetTransform.position, originalPosition[index].transform.position))
			{
				cameras[index]._camera.transform.position = Vector3.Lerp(cameras[index]._camera.transform.position, originalPosition[index].transform.position, Time.deltaTime * cameraSettings.OrbitalThatFollows.displacementSpeed);
			}
			else if (Physics.Linecast(targetTransform.position, originalPosition[index].transform.position, out hitInfo2))
			{
				if (cameraSettings.OrbitalThatFollows.ignoreCollision)
				{
					cameras[index]._camera.transform.position = Vector3.Lerp(cameras[index]._camera.transform.position, originalPosition[index].transform.position, Time.deltaTime * cameraSettings.OrbitalThatFollows.displacementSpeed);
				}
				else
				{
					cameras[index]._camera.transform.position = Vector3.Lerp(cameras[index]._camera.transform.position, hitInfo2.point, Time.deltaTime * cameraSettings.OrbitalThatFollows.displacementSpeed);
				}
			}
			if (cameraSettings.OrbitalThatFollows.customLookAt)
			{
				Quaternion b4 = Quaternion.LookRotation(targetTransform.position - cameras[index]._camera.transform.position, Vector3.up);
				cameras[index]._camera.transform.rotation = Quaternion.Slerp(cameras[index]._camera.transform.rotation, b4, Time.deltaTime * cameraSettings.OrbitalThatFollows.spinSpeedCustomLookAt);
			}
			else
			{
				cameras[index]._camera.transform.LookAt(targetTransform.position);
			}
			break;
		}
		case MSACC_CameraType.TipoRotac.ETS_StyleCamera:
		{
			float num16 = _horizontalInputMSACC;
			float num17 = _verticalInputMSACC;
			if (cameraSettings.ETS_StyleCamera.invertXInput)
			{
				num16 = 0f - _horizontalInputMSACC;
			}
			if (cameraSettings.ETS_StyleCamera.invertYInput)
			{
				num17 = 0f - _verticalInputMSACC;
			}
			if (cameraSettings.ETS_StyleCamera.rotateWhenClick)
			{
				if (Input.GetKey(cameraSettings.ETS_StyleCamera.keyToRotate) || _enableMobileInputs)
				{
					rotacXETS += num16 * cameraSettings.ETS_StyleCamera.sensibilityX;
					rotacYETS += num17 * cameraSettings.ETS_StyleCamera.sensibilityY;
				}
			}
			else
			{
				rotacXETS += num16 * cameraSettings.ETS_StyleCamera.sensibilityX;
				rotacYETS += num17 * cameraSettings.ETS_StyleCamera.sensibilityY;
			}
			Vector3 b6 = new Vector3(originalPositionETS[index].x + Mathf.Clamp(rotacXETS / 50f + cameraSettings.ETS_StyleCamera.ETS_CameraShift / 3f, 0f - cameraSettings.ETS_StyleCamera.ETS_CameraShift, 0f), originalPositionETS[index].y, originalPositionETS[index].z);
			cameras[index]._camera.transform.localPosition = Vector3.Lerp(cameras[index]._camera.transform.localPosition, b6, Time.deltaTime * 10f);
			rotacXETS = MSADCCClampAngle(rotacXETS, -180f, 80f);
			rotacYETS = MSADCCClampAngle(rotacYETS, -60f, 60f);
			Quaternion quaternion5 = Quaternion.AngleAxis(rotacXETS, Vector3.up);
			Quaternion quaternion6 = Quaternion.AngleAxis(rotacYETS, -Vector3.right);
			Quaternion b7 = originalRotation[index] * quaternion5 * quaternion6;
			cameras[index]._camera.transform.localRotation = Quaternion.Lerp(cameras[index]._camera.transform.localRotation, b7, Time.deltaTime * 10f * num);
			cameras[index]._camera.fieldOfView -= _scrollInputMSACC * cameraSettings.ETS_StyleCamera.speedScroolZoom * 50f;
			if (cameras[index]._camera.fieldOfView < initialFieldOfView[index] - cameraSettings.ETS_StyleCamera.maxScroolZoom)
			{
				cameras[index]._camera.fieldOfView = initialFieldOfView[index] - cameraSettings.ETS_StyleCamera.maxScroolZoom;
			}
			if (cameras[index]._camera.fieldOfView > initialFieldOfView[index])
			{
				cameras[index]._camera.fieldOfView = initialFieldOfView[index];
			}
			break;
		}
		case MSACC_CameraType.TipoRotac.FlyCamera_OnlyWindows:
		{
			float num8 = _horizontalInputMSACC;
			float num9 = _verticalInputMSACC;
			if (cameraSettings.FlyCamera_OnlyWindows.invertXInput)
			{
				num8 = 0f - _horizontalInputMSACC;
			}
			if (cameraSettings.FlyCamera_OnlyWindows.invertYInput)
			{
				num9 = 0f - _verticalInputMSACC;
			}
			cameraRotationFly.x += num8 * cameraSettings.FlyCamera_OnlyWindows.sensibilityX * 15f * Time.deltaTime;
			cameraRotationFly.y += num9 * cameraSettings.FlyCamera_OnlyWindows.sensibilityY * 15f * Time.deltaTime;
			cameraRotationFly.y = Mathf.Clamp(cameraRotationFly.y, -90f, 90f);
			cameras[index]._camera.transform.rotation = Quaternion.AngleAxis(cameraRotationFly.x, Vector3.up);
			cameras[index]._camera.transform.rotation *= Quaternion.AngleAxis(cameraRotationFly.y, Vector3.left);
			float num10 = cameraSettings.FlyCamera_OnlyWindows.movementSpeed;
			if (Input.GetKey(cameraSettings.FlyCamera_OnlyWindows.speedKeyCode))
			{
				num10 *= 3f;
			}
			cameras[index]._camera.transform.position += cameras[index]._camera.transform.right * num10 * Input.GetAxis(cameraSettings.FlyCamera_OnlyWindows.horizontalMove) * Time.deltaTime;
			cameras[index]._camera.transform.position += cameras[index]._camera.transform.forward * num10 * Input.GetAxis(cameraSettings.FlyCamera_OnlyWindows.verticalMove) * Time.deltaTime;
			if (Input.GetKey(cameraSettings.FlyCamera_OnlyWindows.moveUp))
			{
				cameras[index]._camera.transform.position += Vector3.up * num10 * Time.deltaTime;
			}
			if (Input.GetKey(cameraSettings.FlyCamera_OnlyWindows.moveDown))
			{
				cameras[index]._camera.transform.position -= Vector3.up * num10 * Time.deltaTime;
			}
			break;
		}
		case MSACC_CameraType.TipoRotac.PlaneX_Z:
		{
			float num2 = cameraSettings.PlaneX_Z.normalYPosition;
			float num3 = cameraSettings.PlaneX_Z.normalYPosition;
			float num4 = (cameraSettings.PlaneX_Z.normalYPosition - cameraSettings.PlaneX_Z.limitYPosition) / cameraSettings.PlaneX_Z.edgeDistanceToStartDescending;
			float x2 = Mathf.Clamp(targetTransform.position.x, cameraSettings.PlaneX_Z.minXPosition, cameraSettings.PlaneX_Z.maxXPosition);
			float z2 = Mathf.Clamp(targetTransform.position.z, cameraSettings.PlaneX_Z.minZPosition, cameraSettings.PlaneX_Z.maxZPosition);
			Vector3 vector = new Vector3(x2, cameraSettings.PlaneX_Z.normalYPosition, z2);
			if (vector.x < cameraSettings.PlaneX_Z.minXPosition + cameraSettings.PlaneX_Z.edgeDistanceToStartDescending)
			{
				num2 = cameraSettings.PlaneX_Z.limitYPosition + (vector.x - cameraSettings.PlaneX_Z.minXPosition) * num4;
			}
			if (vector.x > cameraSettings.PlaneX_Z.maxXPosition - cameraSettings.PlaneX_Z.edgeDistanceToStartDescending)
			{
				num2 = cameraSettings.PlaneX_Z.limitYPosition + (cameraSettings.PlaneX_Z.maxXPosition - vector.x) * num4;
			}
			if (vector.z < cameraSettings.PlaneX_Z.minZPosition + cameraSettings.PlaneX_Z.edgeDistanceToStartDescending)
			{
				num3 = cameraSettings.PlaneX_Z.limitYPosition + (vector.z - cameraSettings.PlaneX_Z.minZPosition) * num4;
			}
			if (vector.z > cameraSettings.PlaneX_Z.maxZPosition - cameraSettings.PlaneX_Z.edgeDistanceToStartDescending)
			{
				num3 = cameraSettings.PlaneX_Z.limitYPosition + (cameraSettings.PlaneX_Z.maxZPosition - vector.z) * num4;
			}
			float y = Mathf.Min(num2, num3, vector.y);
			vector = new Vector3(x2, y, z2);
			cameras[index]._camera.transform.position = Vector3.Lerp(cameras[index]._camera.transform.position, vector, Time.deltaTime * cameraSettings.PlaneX_Z.displacementSpeed);
			switch (cameraSettings.PlaneX_Z.SelectRotation)
			{
			case MSACC_SettingsCameraPlaneX_Z.PlaneXZRotationType.KeepTheRotationFixed:
			{
				Vector3 euler = new Vector3(90f, 0f, 0f);
				cameras[index]._camera.transform.rotation = Quaternion.Euler(euler);
				break;
			}
			case MSACC_SettingsCameraPlaneX_Z.PlaneXZRotationType.LookAt:
				cameras[index]._camera.transform.LookAt(targetTransform.position);
				break;
			case MSACC_SettingsCameraPlaneX_Z.PlaneXZRotationType.OptimizedLookAt:
			{
				Quaternion b = Quaternion.LookRotation(targetTransform.position - cameras[index]._camera.transform.position, Vector3.up);
				cameras[index]._camera.transform.rotation = Quaternion.Slerp(cameras[index]._camera.transform.rotation, b, Time.deltaTime * cameraSettings.PlaneX_Z.spinSpeedCustomLookAt);
				break;
			}
			}
			break;
		}
		case MSACC_CameraType.TipoRotac.Stop:
			break;
		}
	}

	public static float MSADCCClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	public void MSADCCChangeCameras()
	{
		if (Time.timeScale > 0f)
		{
			if (index < cameras.Length - 1)
			{
				lastIndex = index;
				index++;
				EnableCameras(index);
			}
			else if (index >= cameras.Length - 1)
			{
				lastIndex = index;
				index = 0;
				EnableCameras(index);
			}
		}
	}

	private void Update()
	{
		if (!_enableMobileInputs)
		{
			_horizontalInputMSACC = Input.GetAxis(cameraSettings.inputMouseX);
			_verticalInputMSACC = Input.GetAxis(cameraSettings.inputMouseY);
			_scrollInputMSACC = Input.GetAxis(cameraSettings.inputMouseScrollWheel);
		}
		_horizontalInputMSACC = Mathf.Clamp(_horizontalInputMSACC, -1f, 1f);
		_verticalInputMSACC = Mathf.Clamp(_verticalInputMSACC, -1f, 1f);
		_scrollInputMSACC = Mathf.Clamp(_scrollInputMSACC, -1f, 1f);
		if (!_enableMobileInputs && Time.timeScale > 0f)
		{
			if (Input.GetKeyDown(cameraSettings.cameraSwitchKey) && index < cameras.Length - 1)
			{
				lastIndex = index;
				index++;
				EnableCameras(index);
			}
			else if (Input.GetKeyDown(cameraSettings.cameraSwitchKey) && index >= cameras.Length - 1)
			{
				lastIndex = index;
				index = 0;
				EnableCameras(index);
			}
		}
		if (cameraSettings.camerasUpdateMode == MSACC_CameraSetting.UpdateMode.Update && cameras.Length != 0 && Time.timeScale > 0f && (bool)cameras[index]._camera)
		{
			ManageCameras();
		}
	}

	private void LateUpdate()
	{
		if (cameraSettings.camerasUpdateMode == MSACC_CameraSetting.UpdateMode.LateUpdate && cameras.Length != 0 && Time.timeScale > 0f && (bool)cameras[index]._camera)
		{
			ManageCameras();
		}
	}

	private void FixedUpdate()
	{
		if (cameraSettings.camerasUpdateMode == MSACC_CameraSetting.UpdateMode.FixedUpdate && cameras.Length != 0 && Time.timeScale > 0f && (bool)cameras[index]._camera)
		{
			ManageCameras();
		}
	}
}
