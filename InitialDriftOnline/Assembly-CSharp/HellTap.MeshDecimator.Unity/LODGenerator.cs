using System;
using System.Linq;
using HellTap.MeshDecimator.Algorithms;
using UnityEngine;

namespace HellTap.MeshDecimator.Unity;

public static class LODGenerator
{
	private const string ParentGameObjectName = "_LOD_";

	private static Renderer[] CombineRenderers(MeshRenderer[] meshRenderers, SkinnedMeshRenderer[] skinnedRenderers)
	{
		Renderer[] array = new Renderer[meshRenderers.Length + skinnedRenderers.Length];
		Array.Copy(meshRenderers, 0, array, 0, meshRenderers.Length);
		Array.Copy(skinnedRenderers, 0, array, meshRenderers.Length, skinnedRenderers.Length);
		return array;
	}

	private static UnityEngine.Mesh GenerateStaticLOD(Transform transform, MeshRenderer renderer, float quality, out Material[] materials, DecimationAlgorithm.StatusReportCallback statusCallback, bool preserveBorders = false, bool preserveSeams = false, bool preserveFoldovers = false)
	{
		Transform transform2 = renderer.transform;
		UnityEngine.Mesh sharedMesh = renderer.GetComponent<MeshFilter>().sharedMesh;
		Matrix4x4 transform3 = transform.worldToLocalMatrix * transform2.localToWorldMatrix;
		materials = renderer.sharedMaterials;
		return MeshDecimatorUtility.DecimateMesh(sharedMesh, transform3, quality, recalculateNormals: false, statusCallback, preserveBorders, preserveSeams, preserveFoldovers);
	}

	private static UnityEngine.Mesh GenerateStaticLOD(Transform transform, MeshRenderer[] renderers, float quality, out Material[] materials, DecimationAlgorithm.StatusReportCallback statusCallback, bool preserveBorders = false, bool preserveSeams = false, bool preserveFoldovers = false)
	{
		if (renderers.Length == 1)
		{
			return GenerateStaticLOD(transform, renderers[0], quality, out materials, statusCallback, preserveBorders, preserveSeams, preserveFoldovers);
		}
		UnityEngine.Mesh[] array = new UnityEngine.Mesh[renderers.Length];
		Matrix4x4[] array2 = new Matrix4x4[renderers.Length];
		Material[][] array3 = new Material[renderers.Length][];
		for (int i = 0; i < renderers.Length; i++)
		{
			MeshRenderer meshRenderer = renderers[i];
			Transform transform2 = meshRenderer.transform;
			MeshFilter component = meshRenderer.GetComponent<MeshFilter>();
			array[i] = component.sharedMesh;
			array2[i] = transform.worldToLocalMatrix * transform2.localToWorldMatrix;
			array3[i] = meshRenderer.sharedMaterials;
		}
		return MeshDecimatorUtility.DecimateMeshes(array, array2, array3, quality, recalculateNormals: false, out materials, statusCallback, preserveBorders, preserveSeams, preserveFoldovers);
	}

	private static UnityEngine.Mesh GenerateSkinnedLOD(Transform transform, SkinnedMeshRenderer renderer, float quality, out Material[] materials, out Transform[] mergedBones, DecimationAlgorithm.StatusReportCallback statusCallback, bool preserveBorders = false, bool preserveSeams = false, bool preserveFoldovers = false)
	{
		Transform transform2 = renderer.transform;
		UnityEngine.Mesh sharedMesh = renderer.sharedMesh;
		Matrix4x4 transform3 = transform.worldToLocalMatrix * transform2.localToWorldMatrix;
		materials = renderer.sharedMaterials;
		mergedBones = renderer.bones;
		return MeshDecimatorUtility.DecimateMesh(sharedMesh, transform3, quality, recalculateNormals: false, statusCallback, preserveBorders, preserveSeams, preserveFoldovers);
	}

	private static UnityEngine.Mesh GenerateSkinnedLOD(Transform transform, SkinnedMeshRenderer[] renderers, float quality, out Material[] materials, out Transform[] mergedBones, DecimationAlgorithm.StatusReportCallback statusCallback, bool preserveBorders = false, bool preserveSeams = false, bool preserveFoldovers = false)
	{
		if (renderers.Length == 1)
		{
			return GenerateSkinnedLOD(transform, renderers[0], quality, out materials, out mergedBones, statusCallback, preserveBorders, preserveSeams, preserveFoldovers);
		}
		UnityEngine.Mesh[] array = new UnityEngine.Mesh[renderers.Length];
		Matrix4x4[] array2 = new Matrix4x4[renderers.Length];
		Material[][] array3 = new Material[renderers.Length][];
		Transform[][] array4 = new Transform[renderers.Length][];
		for (int i = 0; i < renderers.Length; i++)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = renderers[i];
			Transform transform2 = skinnedMeshRenderer.transform;
			array[i] = skinnedMeshRenderer.sharedMesh;
			array2[i] = transform.worldToLocalMatrix * transform2.localToWorldMatrix;
			array3[i] = skinnedMeshRenderer.sharedMaterials;
			array4[i] = skinnedMeshRenderer.bones;
		}
		return MeshDecimatorUtility.DecimateMeshes(array, array2, array3, array4, quality, recalculateNormals: false, out materials, out mergedBones, statusCallback, preserveBorders, preserveSeams, preserveFoldovers);
	}

	private static Transform FindRootBone(Transform transform, Transform[] transforms)
	{
		Transform result = null;
		for (int i = 0; i < transforms.Length; i++)
		{
			if (transforms[i].parent == transform)
			{
				result = transforms[i];
				if (transforms[i].childCount > 0)
				{
					break;
				}
			}
		}
		return result;
	}

	private static void SetupLODRenderer(Renderer renderer, LODSettings settings)
	{
		renderer.shadowCastingMode = settings.shadowCasting;
		renderer.receiveShadows = settings.receiveShadows;
		renderer.motionVectorGenerationMode = settings.motionVectors;
		renderer.lightProbeUsage = settings.lightProbeUsage;
		renderer.reflectionProbeUsage = settings.reflectionProbeUsage;
		SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
		if (skinnedMeshRenderer != null)
		{
			skinnedMeshRenderer.skinnedMotionVectors = settings.skinnedMotionVectors;
			skinnedMeshRenderer.quality = settings.skinQuality;
		}
		GameObject gameObject = renderer.gameObject;
		if (gameObject != null)
		{
			if (string.IsNullOrEmpty(settings.tag))
			{
				settings.tag = "Untagged";
			}
			gameObject.tag = settings.tag;
			gameObject.layer = settings.layer;
		}
	}

	public static void GenerateLODs(GameObject gameObj, LODSettings[] levels, LODStatusReportCallback statusCallback = null, bool preserveBorders = false, bool preserveSeams = false, bool preserveFoldovers = false)
	{
		DestroyLODs(gameObj);
		Transform transform = gameObj.transform;
		MeshRenderer[] componentsInChildren = gameObj.GetComponentsInChildren<MeshRenderer>();
		SkinnedMeshRenderer[] componentsInChildren2 = gameObj.GetComponentsInChildren<SkinnedMeshRenderer>();
		if (componentsInChildren.Length == 0 && componentsInChildren2.Length == 0)
		{
			return;
		}
		Transform transform2 = new GameObject("_LOD_").transform;
		transform2.parent = transform;
		transform2.localPosition = Vector3.zero;
		transform2.localRotation = Quaternion.identity;
		transform2.localScale = Vector3.one;
		LODGroup lODGroup = gameObj.GetComponent<LODGroup>();
		if (lODGroup == null)
		{
			lODGroup = gameObj.AddComponent<LODGroup>();
		}
		float num = 0.5f;
		LOD[] array = new LOD[levels.Length + 1];
		Renderer[] renderers = CombineRenderers(componentsInChildren, componentsInChildren2);
		array[0] = new LOD(num, renderers);
		num *= 0.5f;
		float max = 1f;
		int levelIndex;
		for (levelIndex = 0; levelIndex < levels.Length; levelIndex++)
		{
			LODSettings settings = levels[levelIndex];
			float num2 = Mathf.Clamp(settings.quality, 0.01f, max);
			num *= num2 * num2;
			GameObject gameObject = new GameObject($"Level{levelIndex}");
			Transform transform3 = gameObject.transform;
			transform3.parent = transform2;
			transform3.localPosition = Vector3.zero;
			transform3.localRotation = Quaternion.identity;
			transform3.localScale = Vector3.one;
			DecimationAlgorithm.StatusReportCallback statusCallback2 = null;
			if (statusCallback != null)
			{
				statusCallback2 = delegate(int iteration, int originalTris, int currentTris, int targetTris)
				{
					statusCallback(levelIndex, iteration, originalTris, currentTris, targetTris);
				};
			}
			GameObject gameObject2 = gameObject;
			GameObject gameObject3 = gameObject;
			Transform transform4 = transform3;
			Transform transform5 = transform3;
			if (componentsInChildren.Length != 0 && componentsInChildren2.Length != 0)
			{
				gameObject2 = new GameObject("Static", typeof(MeshFilter), typeof(MeshRenderer));
				transform4 = gameObject2.transform;
				transform4.parent = transform3;
				transform4.localPosition = Vector3.zero;
				transform4.localRotation = Quaternion.identity;
				transform4.localScale = Vector3.one;
				gameObject3 = new GameObject("Skinned", typeof(SkinnedMeshRenderer));
				transform5 = gameObject3.transform;
				transform5.parent = transform3;
				transform5.localPosition = Vector3.zero;
				transform5.localRotation = Quaternion.identity;
				transform5.localScale = Vector3.one;
			}
			Renderer[] array2 = null;
			Renderer[] array3 = null;
			if (componentsInChildren.Length != 0)
			{
				if (settings.combineMeshes)
				{
					Material[] materials;
					UnityEngine.Mesh mesh = GenerateStaticLOD(transform, componentsInChildren, num2, out materials, statusCallback2, preserveBorders, preserveSeams, preserveFoldovers);
					mesh.name = $"{gameObj.name}_static{levelIndex}";
					gameObject2.AddComponent<MeshFilter>().sharedMesh = mesh;
					MeshRenderer meshRenderer = gameObject2.AddComponent<MeshRenderer>();
					meshRenderer.sharedMaterials = materials;
					SetupLODRenderer(meshRenderer, settings);
					array2 = new Renderer[1] { meshRenderer };
				}
				else
				{
					array2 = new Renderer[componentsInChildren.Length];
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						MeshRenderer meshRenderer2 = componentsInChildren[i];
						Material[] materials2;
						UnityEngine.Mesh mesh2 = GenerateStaticLOD(transform, meshRenderer2, num2, out materials2, statusCallback2, preserveBorders, preserveSeams, preserveFoldovers);
						mesh2.name = $"{gameObj.name}_static{levelIndex}_{i}";
						GameObject gameObject4 = new GameObject(meshRenderer2.name, typeof(MeshFilter), typeof(MeshRenderer));
						gameObject4.transform.parent = transform4;
						gameObject4.transform.localPosition = Vector3.zero;
						gameObject4.transform.localRotation = Quaternion.identity;
						gameObject4.transform.localScale = Vector3.one;
						gameObject4.GetComponent<MeshFilter>().sharedMesh = mesh2;
						MeshRenderer component = gameObject4.GetComponent<MeshRenderer>();
						component.sharedMaterials = materials2;
						SetupLODRenderer(component, settings);
						array2[i] = component;
					}
				}
			}
			if (componentsInChildren2.Length != 0)
			{
				if (settings.combineMeshes)
				{
					Material[] materials3;
					Transform[] mergedBones;
					UnityEngine.Mesh mesh3 = GenerateSkinnedLOD(transform, componentsInChildren2, num2, out materials3, out mergedBones, statusCallback2, preserveBorders, preserveSeams, preserveFoldovers);
					mesh3.name = $"{gameObj.name}_skinned{levelIndex}";
					Transform rootBone = FindRootBone(transform, mergedBones);
					SkinnedMeshRenderer skinnedMeshRenderer = gameObject3.AddComponent<SkinnedMeshRenderer>();
					skinnedMeshRenderer.sharedMesh = mesh3;
					skinnedMeshRenderer.sharedMaterials = materials3;
					skinnedMeshRenderer.rootBone = rootBone;
					skinnedMeshRenderer.bones = mergedBones;
					SetupLODRenderer(skinnedMeshRenderer, settings);
					array3 = new Renderer[1] { skinnedMeshRenderer };
				}
				else
				{
					array3 = new Renderer[componentsInChildren2.Length];
					for (int j = 0; j < componentsInChildren2.Length; j++)
					{
						SkinnedMeshRenderer skinnedMeshRenderer2 = componentsInChildren2[j];
						Material[] materials4;
						Transform[] mergedBones2;
						UnityEngine.Mesh mesh4 = GenerateSkinnedLOD(transform, skinnedMeshRenderer2, num2, out materials4, out mergedBones2, statusCallback2, preserveBorders, preserveSeams, preserveFoldovers);
						mesh4.name = $"{gameObj.name}_skinned{levelIndex}_{j}";
						Transform rootBone2 = FindRootBone(transform, mergedBones2);
						GameObject gameObject5 = new GameObject(skinnedMeshRenderer2.name, typeof(SkinnedMeshRenderer));
						gameObject5.transform.parent = transform5;
						gameObject5.transform.localPosition = Vector3.zero;
						gameObject5.transform.localRotation = Quaternion.identity;
						gameObject5.transform.localScale = Vector3.one;
						SkinnedMeshRenderer component2 = gameObject5.GetComponent<SkinnedMeshRenderer>();
						component2.sharedMesh = mesh4;
						component2.sharedMaterials = materials4;
						component2.rootBone = rootBone2;
						component2.bones = mergedBones2;
						SetupLODRenderer(component2, settings);
						array3[j] = component2;
					}
				}
			}
			Renderer[] renderers2 = ((array2 != null && array3 != null) ? array2.Concat(array3).ToArray() : ((array2 == null) ? ((array3 == null) ? new Renderer[0] : array3) : array2));
			max = num2;
			array[levelIndex + 1] = new LOD(num, renderers2);
		}
		lODGroup.SetLODs(array);
	}

	private static void CopyStaticFlags(GameObject source, GameObject destination)
	{
	}

	public static void DestroyLODs(GameObject gameObj)
	{
		if (gameObj == null)
		{
			throw new ArgumentNullException("gameObj");
		}
		Transform transform = gameObj.transform.Find("_LOD_");
		if (!(transform != null))
		{
			return;
		}
		if (Application.isPlaying)
		{
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		else
		{
			UnityEngine.Object.DestroyImmediate(transform.gameObject);
		}
		LODGroup component = gameObj.GetComponent<LODGroup>();
		if (component != null)
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(component);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(component);
			}
		}
	}
}
