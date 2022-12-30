using UnityEngine;

namespace HellTap.MeshKit;

[DisallowMultipleComponent]
[AddComponentMenu("MeshKit/Decimate Mesh At Runtime")]
public class DecimateMeshAtRuntime : MonoBehaviour
{
	[Header("Selection")]
	[Tooltip("Attempt to decimate all child objects too.")]
	public bool applyToChildren = true;

	[Tooltip("Decimate the meshes within MeshFilters")]
	public bool applyToMeshFilters = true;

	[Tooltip("Decimate the meshes within Skinned Mesh Renderers")]
	public bool applyToSkinnedMeshRenderers = true;

	[Tooltip("Only apply to GameObjects with active Renderer components.")]
	public bool onlyApplyToEnabledRenderers = true;

	[Header("Decimator")]
	[Tooltip("Set the quality of the decimation. 0 = No details, 1 = Full details.")]
	[Range(0f, 1f)]
	public float decimatorQuality = 0.8f;

	[Tooltip("Recalculate the mesh's normals after it has been decimated.")]
	public bool recalculateNormals;

	[Header("Try these options if gaps appear in the Mesh")]
	[Tooltip("Preserve border vertices.")]
	public bool preserveBorders;

	[Tooltip("Preserve seams on the Mesh.")]
	public bool preserveSeams;

	[Tooltip("Preserve UV foldovers.")]
	public bool preserveFoldovers;

	private void Start()
	{
		MeshKit.DecimateMesh(base.gameObject, applyToChildren, applyToMeshFilters, applyToSkinnedMeshRenderers, onlyApplyToEnabledRenderers, decimatorQuality, recalculateNormals, preserveBorders, preserveSeams, preserveFoldovers);
		Object.Destroy(this);
	}
}
