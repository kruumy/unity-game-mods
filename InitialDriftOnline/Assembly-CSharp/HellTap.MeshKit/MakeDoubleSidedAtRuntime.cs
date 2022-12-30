using UnityEngine;

namespace HellTap.MeshKit;

[DisallowMultipleComponent]
[AddComponentMenu("MeshKit/Double-Sided Children At Runtime")]
public class MakeDoubleSidedAtRuntime : MonoBehaviour
{
	[Tooltip("Attempt to make all child objects containing MeshFilters Double-Sided too.")]
	public bool applyToChildren = true;

	[Tooltip("Make the meshes within MeshFilters Double-Sided")]
	public bool applyToMeshFilters = true;

	[Tooltip("Make the meshes within Skinned Mesh Renderers Double-Sided")]
	public bool applyToSkinnedMeshRenderers;

	[Tooltip("Only apply to GameObjects with active Renderer components.")]
	public bool onlyApplyToEnabledRenderers = true;

	private void Start()
	{
		MeshKit.MakeDoubleSided(base.gameObject, applyToChildren, applyToMeshFilters, applyToSkinnedMeshRenderers, onlyApplyToEnabledRenderers);
		Object.Destroy(this);
	}
}
