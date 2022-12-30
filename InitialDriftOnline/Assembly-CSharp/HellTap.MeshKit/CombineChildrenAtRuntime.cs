using UnityEngine;

namespace HellTap.MeshKit;

[DisallowMultipleComponent]
[AddComponentMenu("MeshKit/Combine Children At Runtime")]
public class CombineChildrenAtRuntime : MonoBehaviour
{
	[Header("SubMesh Options   (this can take a while in large scenes)")]
	[Tooltip("This GameObject and it's children will be scanned for submeshes. If found, they will be broken apart and rebuilt before the combine process begins.\n\nNOTE: This should generally not be used in runtime builds as it is a very expensive operation which can take several minutes or more to complete!")]
	public bool seperateSubMeshesFirst;

	[Tooltip("After seperating meshes, MeshKit strips unused vertices making each mesh highly optimized and memory efficient. Unfortunatly this can heavily increase processing time, especially with large meshes.")]
	public bool stripUnusedVertices;

	[Tooltip("Only Seperates SubMeshes which have Renderers that are enabled.")]
	public bool onlySeperateEnabledRenderers = true;

	[Header("Combine Options")]
	[Tooltip("Only GameObjects with their Renderer component enabled will be combined.")]
	[Range(16000f, 65534f)]
	public int maximumVerticesPerObject = 65534;

	[Tooltip("Only GameObjects with their Renderer component enabled will be combined.")]
	public bool onlyCombineEnabledRenderers = true;

	[Tooltip("Apply Unity's mesh optimization function to the combined Meshes.\n\nNOTE: This increases the time it takes to combine objects.")]
	public bool optimizeCombinedMeshes;

	[Tooltip("Adds Mesh Colliders to the new combined objects. It's usually a good idea to check \"Delete Objects With Disabled Renderers\" when selecting this option.")]
	public bool createMeshCollidersOnNewObjects;

	[Tooltip("Use -1 to create new GameObjects using the Default layer. Alternatively, enter a layer index to use ( 0 - 31 ).")]
	public int createNewObjectsWithLayer = -1;

	[Tooltip("Leave this blank to create untagged GameObjects or enter the name of the tag to set.")]
	public string createNewObjectsWithTag = "Untagged";

	[Header("Cleanup Options")]
	[Tooltip("Destroys all GameObjects originally used to create the combined mesh (with the exception of those that have Colliders attached to them).")]
	public bool destroyOriginalObjects = true;

	[Tooltip("Destroys all GameObjects that are in this group with disabled Renderer components (This includes objects with active Colliders).")]
	public bool destroyObjectsWithDisabledRenderers;

	[Tooltip("Destroys any empty GameObjects which do not have any components or children. ")]
	public bool destroyEmptyObjects = true;

	private void Start()
	{
		if (seperateSubMeshesFirst)
		{
			MeshKit.SeparateMeshes(base.gameObject, onlySeperateEnabledRenderers, stripUnusedVertices);
		}
		MeshKit.CombineChildren(base.gameObject, optimizeCombinedMeshes, createNewObjectsWithLayer, createNewObjectsWithTag, onlyCombineEnabledRenderers, createMeshCollidersOnNewObjects, destroyOriginalObjects, destroyObjectsWithDisabledRenderers, destroyEmptyObjects, maximumVerticesPerObject);
	}
}
