using UnityEngine;
using UnityEngine.Rendering;

namespace HellTap.MeshKit;

[ExecuteInEditMode]
[DisallowMultipleComponent]
[AddComponentMenu("MeshKit/AutoLOD At Runtime")]
public class AutoLODAtRuntime : MonoBehaviour
{
	[Header("LOD Settings")]
	[Tooltip("These options will be used to setup the LOD Group. The group at the top of the list will be closest to the camera.")]
	public MeshKit.AutoLODSettings[] levels = new MeshKit.AutoLODSettings[3]
	{
		new MeshKit.AutoLODSettings(50f),
		new MeshKit.AutoLODSettings(16f, 0.65f, SkinQuality.Bone2, receiveShadows: true, ShadowCastingMode.Off, MotionVectorGenerationMode.Object, skinnedMotionVectors: false),
		new MeshKit.AutoLODSettings(7f, 0.4f, SkinQuality.Bone1, receiveShadows: false, ShadowCastingMode.Off, MotionVectorGenerationMode.Object, skinnedMotionVectors: false)
	};

	[Header("LOD Culling Distance")]
	[Tooltip("At what distance should this LOD Group be hidden?")]
	[Range(0f, 100f)]
	public float cullingDistancePercentage = 1f;

	[Header("Decimation Options")]
	[Tooltip("If there are gaps showing up in the mesh, you can try to stop the decimator from removing borders. This will affect the decimator's ability to reduce complexity.")]
	public bool preserveBorders;

	[Tooltip("If there are gaps showing up in the mesh, you can try to stop the decimator from removing seams. This will affect the decimator's ability to reduce complexity.")]
	public bool preserveSeams;

	[Tooltip("If there are gaps showing up in the mesh, you can try to stop the decimator from removing UV foldovers. This will affect the decimator's ability to reduce complexity.")]
	public bool preserveFoldovers;

	private void Start()
	{
		if (Application.isPlaying)
		{
			MeshKit.AutoLOD(base.gameObject, levels, cullingDistancePercentage, preserveBorders, preserveSeams, preserveFoldovers);
			Object.Destroy(this);
		}
	}
}
