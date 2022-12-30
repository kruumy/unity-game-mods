using UnityEngine;

namespace HellTap.MeshKit;

[DisallowMultipleComponent]
[AddComponentMenu("MeshKit/SeperateChildrenAtRuntime")]
public class SeperateChildrenAtRuntime : MonoBehaviour
{
	[Tooltip("After seperating meshes, MeshKit strips unused vertices making each mesh highly optimized and memory efficient. Unfortunatly this can heavily increase processing time, especially with large meshes.")]
	public bool stripUnusedVertices;

	[Tooltip("Only apply to GameObjects with active Renderer components.")]
	public bool onlyApplyToEnabledRenderers = true;

	private void Start()
	{
		MeshKit.SeparateMeshes(base.gameObject, onlyApplyToEnabledRenderers, stripUnusedVertices);
		Object.Destroy(this);
	}
}
