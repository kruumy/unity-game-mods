using UnityEngine;

namespace HellTap.MeshKit;

[DisallowMultipleComponent]
[AddComponentMenu("MeshKit/Invert Children At Runtime")]
public class InvertMeshAtRuntime : MonoBehaviour
{
	[Tooltip("Attempt to make all child objects containing MeshFilters Inverted too.")]
	public bool applyToChildren = true;

	[Tooltip("Invert the meshes within MeshFilters")]
	public bool applyToMeshFilters = true;

	[Tooltip("Invert the meshes within Skinned Mesh Renderers")]
	public bool applyToSkinnedMeshRenderers;

	[Tooltip("Only apply to GameObjects with active Renderer components.")]
	public bool onlyApplyToEnabledRenderers = true;

	private void Start()
	{
		MeshKit.InvertMesh(base.gameObject, applyToChildren, applyToMeshFilters, applyToSkinnedMeshRenderers, onlyApplyToEnabledRenderers);
		Object.Destroy(this);
	}
}
