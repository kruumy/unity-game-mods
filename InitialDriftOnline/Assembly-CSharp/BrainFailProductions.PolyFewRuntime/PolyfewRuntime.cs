using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AsImpL;
using UnityEngine;
using UnityMeshSimplifier;

namespace BrainFailProductions.PolyFewRuntime;

public class PolyfewRuntime : MonoBehaviour
{
	[Serializable]
	public class ObjectMeshPairs : Dictionary<GameObject, MeshRendererPair>
	{
	}

	public enum MeshCombineTarget
	{
		SkinnedAndStatic,
		StaticOnly,
		SkinnedOnly
	}

	[Serializable]
	public class MeshRendererPair
	{
		public bool attachedToMeshFilter;

		public Mesh mesh;

		public MeshRendererPair(bool attachedToMeshFilter, Mesh mesh)
		{
			this.attachedToMeshFilter = attachedToMeshFilter;
			this.mesh = mesh;
		}

		public void Destruct()
		{
			if (mesh != null)
			{
				UnityEngine.Object.DestroyImmediate(mesh);
			}
		}
	}

	[Serializable]
	public class CustomMeshActionStructure
	{
		public MeshRendererPair meshRendererPair;

		public GameObject gameObject;

		public Action action;

		public CustomMeshActionStructure(MeshRendererPair meshRendererPair, GameObject gameObject, Action action)
		{
			this.meshRendererPair = meshRendererPair;
			this.gameObject = gameObject;
			this.action = action;
		}
	}

	[Serializable]
	public class SimplificationOptions
	{
		public float simplificationStrength;

		public bool simplifyMeshLossless;

		public bool enableSmartlinking = true;

		public bool recalculateNormals;

		public bool preserveUVSeamEdges;

		public bool preserveUVFoldoverEdges;

		public bool preserveBorderEdges;

		public bool regardPreservationSpheres;

		public List<PreservationSphere> preservationSpheres = new List<PreservationSphere>();

		public bool regardCurvature;

		public int maxIterations = 100;

		public float aggressiveness = 7f;

		public bool useEdgeSort;

		public SimplificationOptions()
		{
		}

		public SimplificationOptions(float simplificationStrength, bool simplifyOptimal, bool enableSmartlink, bool recalculateNormals, bool preserveUVSeamEdges, bool preserveUVFoldoverEdges, bool preserveBorderEdges, bool regardToleranceSphere, List<PreservationSphere> preservationSpheres, bool regardCurvature, int maxIterations, float aggressiveness, bool useEdgeSort)
		{
			this.simplificationStrength = simplificationStrength;
			simplifyMeshLossless = simplifyOptimal;
			enableSmartlinking = enableSmartlink;
			this.recalculateNormals = recalculateNormals;
			this.preserveUVSeamEdges = preserveUVSeamEdges;
			this.preserveUVFoldoverEdges = preserveUVFoldoverEdges;
			this.preserveBorderEdges = preserveBorderEdges;
			regardPreservationSpheres = regardToleranceSphere;
			this.preservationSpheres = preservationSpheres;
			this.regardCurvature = regardCurvature;
			this.maxIterations = maxIterations;
			this.aggressiveness = aggressiveness;
			this.useEdgeSort = useEdgeSort;
		}
	}

	[Serializable]
	public class PreservationSphere
	{
		public Vector3 worldPosition;

		public float diameter;

		public float preservationStrength = 100f;

		public PreservationSphere(Vector3 worldPosition, float diameter, float preservationStrength)
		{
			this.worldPosition = worldPosition;
			this.diameter = diameter;
			this.preservationStrength = preservationStrength;
		}
	}

	[Serializable]
	public class OBJImportOptions : ImportOptions
	{
	}

	[Serializable]
	public class OBJExportOptions
	{
		public readonly bool applyPosition = true;

		public readonly bool applyRotation = true;

		public readonly bool applyScale = true;

		public readonly bool generateMaterials = true;

		public readonly bool exportTextures = true;

		public OBJExportOptions(bool applyPosition, bool applyRotation, bool applyScale, bool generateMaterials, bool exportTextures)
		{
			this.applyPosition = applyPosition;
			this.applyRotation = applyRotation;
			this.applyScale = applyScale;
			this.generateMaterials = generateMaterials;
			this.exportTextures = exportTextures;
		}
	}

	public class ReferencedNumeric<T> where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
	{
		private T val;

		public T Value
		{
			get
			{
				return val;
			}
			set
			{
				val = value;
			}
		}

		public ReferencedNumeric(T value)
		{
			val = value;
		}
	}

	private const int MAX_LOD_COUNT = 8;

	public static int SimplifyObjectDeep(GameObject toSimplify, SimplificationOptions simplificationOptions, Action<GameObject, MeshRendererPair> OnEachMeshSimplified)
	{
		if (simplificationOptions == null)
		{
			throw new ArgumentNullException("simplificationOptions", "You must provide a SimplificationOptions object.");
		}
		int totalTriangles = 0;
		float simplificationStrength = simplificationOptions.simplificationStrength;
		if (toSimplify == null)
		{
			throw new ArgumentNullException("toSimplify", "You must provide a gameobject to simplify.");
		}
		if (!simplificationOptions.simplifyMeshLossless)
		{
			if (!(simplificationStrength >= 0f) || !(simplificationStrength <= 100f))
			{
				throw new ArgumentOutOfRangeException("simplificationStrength", "The allowed values for simplification strength are between [0-100] inclusive.");
			}
			if (Mathf.Approximately(simplificationStrength, 0f))
			{
				return -1;
			}
		}
		if (simplificationOptions.regardPreservationSpheres && (simplificationOptions.preservationSpheres == null || simplificationOptions.preservationSpheres.Count == 0))
		{
			simplificationOptions.preservationSpheres = new List<PreservationSphere>();
			simplificationOptions.regardPreservationSpheres = false;
		}
		ObjectMeshPairs objectMeshPairs = GetObjectMeshPairs(toSimplify, includeInactive: true);
		if (!AreAnyFeasibleMeshes(objectMeshPairs))
		{
			throw new InvalidOperationException("No mesh/meshes found nested under the provided gameobject to simplify.");
		}
		bool flag = false;
		if (CountTriangles(objectMeshPairs) >= 2000 && objectMeshPairs.Count >= 2)
		{
			flag = true;
		}
		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			flag = false;
		}
		float quality = 1f - simplificationStrength / 100f;
		int count = objectMeshPairs.Count;
		int meshesHandled = 0;
		int threadsRunning = 0;
		bool isError = false;
		string error = "";
		object threadLock1 = new object();
		object threadLock2 = new object();
		object threadLock3 = new object();
		if (flag)
		{
			List<CustomMeshActionStructure> meshAssignments = new List<CustomMeshActionStructure>();
			List<CustomMeshActionStructure> callbackFlusher = new List<CustomMeshActionStructure>();
			foreach (KeyValuePair<GameObject, MeshRendererPair> item3 in objectMeshPairs)
			{
				GameObject gameObject = item3.Key;
				int num;
				if (gameObject == null)
				{
					num = meshesHandled;
					meshesHandled = num + 1;
					continue;
				}
				MeshRendererPair meshRendererPair = item3.Value;
				if (meshRendererPair.mesh == null)
				{
					num = meshesHandled;
					meshesHandled = num + 1;
					continue;
				}
				MeshSimplifier meshSimplifier = new MeshSimplifier();
				SetParametersForSimplifier(simplificationOptions, meshSimplifier);
				ToleranceSphere[] array = new ToleranceSphere[simplificationOptions.preservationSpheres.Count];
				if (!meshRendererPair.attachedToMeshFilter && simplificationOptions.regardPreservationSpheres)
				{
					meshSimplifier.isSkinned = true;
					SkinnedMeshRenderer component = gameObject.GetComponent<SkinnedMeshRenderer>();
					meshSimplifier.boneWeightsOriginal = meshRendererPair.mesh.boneWeights;
					meshSimplifier.bindPosesOriginal = meshRendererPair.mesh.bindposes;
					meshSimplifier.bonesOriginal = component.bones;
					int num2 = 0;
					foreach (PreservationSphere preservationSphere in simplificationOptions.preservationSpheres)
					{
						gameObject.transform.InverseTransformPoint(preservationSphere.worldPosition);
						ToleranceSphere toleranceSphere = (array[num2] = new ToleranceSphere
						{
							diameter = preservationSphere.diameter,
							localToWorldMatrix = gameObject.transform.localToWorldMatrix,
							worldPosition = preservationSphere.worldPosition,
							targetObject = gameObject,
							preservationStrength = preservationSphere.preservationStrength
						});
						num2++;
					}
					meshSimplifier.toleranceSpheres = array;
				}
				else if (meshRendererPair.attachedToMeshFilter && simplificationOptions.regardPreservationSpheres)
				{
					int num3 = 0;
					foreach (PreservationSphere preservationSphere2 in simplificationOptions.preservationSpheres)
					{
						ToleranceSphere toleranceSphere2 = (array[num3] = new ToleranceSphere
						{
							diameter = preservationSphere2.diameter,
							localToWorldMatrix = gameObject.transform.localToWorldMatrix,
							worldPosition = preservationSphere2.worldPosition,
							targetObject = gameObject,
							preservationStrength = preservationSphere2.preservationStrength
						});
						num3++;
					}
					meshSimplifier.toleranceSpheres = array;
				}
				meshSimplifier.Initialize(meshRendererPair.mesh, simplificationOptions.regardPreservationSpheres);
				num = threadsRunning;
				threadsRunning = num + 1;
				while (callbackFlusher.Count > 0)
				{
					CustomMeshActionStructure customMeshActionStructure = callbackFlusher[0];
					callbackFlusher.RemoveAt(0);
					if (customMeshActionStructure != null)
					{
						OnEachMeshSimplified?.Invoke(customMeshActionStructure.gameObject, customMeshActionStructure.meshRendererPair);
					}
				}
				Task.Factory.StartNew(delegate
				{
					CustomMeshActionStructure item = new CustomMeshActionStructure(meshRendererPair, gameObject, delegate
					{
						Mesh mesh2 = meshSimplifier.ToMesh();
						AssignReducedMesh(gameObject, meshRendererPair.mesh, mesh2, meshRendererPair.attachedToMeshFilter, assignBindposes: true);
						if (meshSimplifier.RecalculateNormals)
						{
							mesh2.RecalculateNormals();
							mesh2.RecalculateTangents();
						}
						totalTriangles += mesh2.triangles.Length / 3;
					});
					try
					{
						if (!simplificationOptions.simplifyMeshLossless)
						{
							meshSimplifier.SimplifyMesh(quality);
						}
						else
						{
							meshSimplifier.SimplifyMeshLossless();
						}
						lock (threadLock1)
						{
							meshAssignments.Add(item);
							int num6 = threadsRunning;
							threadsRunning = num6 - 1;
							num6 = meshesHandled;
							meshesHandled = num6 + 1;
						}
						lock (threadLock3)
						{
							CustomMeshActionStructure item2 = new CustomMeshActionStructure(meshRendererPair, gameObject, delegate
							{
							});
							callbackFlusher.Add(item2);
						}
					}
					catch (Exception ex)
					{
						lock (threadLock2)
						{
							int num6 = threadsRunning;
							threadsRunning = num6 - 1;
							num6 = meshesHandled;
							meshesHandled = num6 + 1;
							isError = true;
							error = ex.ToString();
						}
					}
				}, CancellationToken.None, TaskCreationOptions.RunContinuationsAsynchronously, TaskScheduler.Current);
			}
			while (callbackFlusher.Count > 0)
			{
				CustomMeshActionStructure customMeshActionStructure2 = callbackFlusher[0];
				callbackFlusher.RemoveAt(0);
				if (customMeshActionStructure2 != null)
				{
					OnEachMeshSimplified?.Invoke(customMeshActionStructure2.gameObject, customMeshActionStructure2.meshRendererPair);
				}
			}
			while (meshesHandled < count && !isError)
			{
				while (callbackFlusher.Count > 0)
				{
					CustomMeshActionStructure customMeshActionStructure3 = callbackFlusher[0];
					callbackFlusher.RemoveAt(0);
					if (customMeshActionStructure3 != null)
					{
						OnEachMeshSimplified?.Invoke(customMeshActionStructure3.gameObject, customMeshActionStructure3.meshRendererPair);
					}
				}
			}
			if (!isError)
			{
				foreach (CustomMeshActionStructure item4 in meshAssignments)
				{
					item4?.action();
				}
			}
		}
		else
		{
			foreach (KeyValuePair<GameObject, MeshRendererPair> item5 in objectMeshPairs)
			{
				GameObject key = item5.Key;
				if (key == null)
				{
					continue;
				}
				MeshRendererPair value = item5.Value;
				if (value.mesh == null)
				{
					continue;
				}
				MeshSimplifier meshSimplifier2 = new MeshSimplifier();
				SetParametersForSimplifier(simplificationOptions, meshSimplifier2);
				ToleranceSphere[] array2 = new ToleranceSphere[simplificationOptions.preservationSpheres.Count];
				if (!value.attachedToMeshFilter && simplificationOptions.regardPreservationSpheres)
				{
					meshSimplifier2.isSkinned = true;
					SkinnedMeshRenderer component2 = key.GetComponent<SkinnedMeshRenderer>();
					meshSimplifier2.boneWeightsOriginal = value.mesh.boneWeights;
					meshSimplifier2.bindPosesOriginal = value.mesh.bindposes;
					meshSimplifier2.bonesOriginal = component2.bones;
					int num4 = 0;
					foreach (PreservationSphere preservationSphere3 in simplificationOptions.preservationSpheres)
					{
						ToleranceSphere toleranceSphere3 = (array2[num4] = new ToleranceSphere
						{
							diameter = preservationSphere3.diameter,
							localToWorldMatrix = key.transform.localToWorldMatrix,
							worldPosition = preservationSphere3.worldPosition,
							targetObject = key,
							preservationStrength = preservationSphere3.preservationStrength
						});
						num4++;
					}
					meshSimplifier2.toleranceSpheres = array2;
				}
				else if (value.attachedToMeshFilter && simplificationOptions.regardPreservationSpheres)
				{
					int num5 = 0;
					foreach (PreservationSphere preservationSphere4 in simplificationOptions.preservationSpheres)
					{
						ToleranceSphere toleranceSphere4 = (array2[num5] = new ToleranceSphere
						{
							diameter = preservationSphere4.diameter,
							localToWorldMatrix = key.transform.localToWorldMatrix,
							worldPosition = preservationSphere4.worldPosition,
							targetObject = key,
							preservationStrength = preservationSphere4.preservationStrength
						});
						num5++;
					}
					meshSimplifier2.toleranceSpheres = array2;
				}
				meshSimplifier2.Initialize(value.mesh, simplificationOptions.regardPreservationSpheres);
				if (!simplificationOptions.simplifyMeshLossless)
				{
					meshSimplifier2.SimplifyMesh(quality);
				}
				else
				{
					meshSimplifier2.SimplifyMeshLossless();
				}
				OnEachMeshSimplified?.Invoke(key, value);
				Mesh mesh = meshSimplifier2.ToMesh();
				mesh.bindposes = value.mesh.bindposes;
				mesh.name = value.mesh.name.Replace("-POLY_REDUCED", "") + "-POLY_REDUCED";
				if (meshSimplifier2.RecalculateNormals)
				{
					mesh.RecalculateNormals();
					mesh.RecalculateTangents();
				}
				if (value.attachedToMeshFilter)
				{
					MeshFilter component3 = key.GetComponent<MeshFilter>();
					if (component3 != null)
					{
						component3.sharedMesh = mesh;
					}
				}
				else
				{
					SkinnedMeshRenderer component4 = key.GetComponent<SkinnedMeshRenderer>();
					if (component4 != null)
					{
						component4.sharedMesh = mesh;
					}
				}
				totalTriangles += mesh.triangles.Length / 3;
			}
		}
		return totalTriangles;
	}

	public static ObjectMeshPairs SimplifyObjectDeep(GameObject toSimplify, SimplificationOptions simplificationOptions)
	{
		if (simplificationOptions == null)
		{
			throw new ArgumentNullException("simplificationOptions", "You must provide a SimplificationOptions object.");
		}
		float simplificationStrength = simplificationOptions.simplificationStrength;
		ObjectMeshPairs toReturn = new ObjectMeshPairs();
		if (toSimplify == null)
		{
			throw new ArgumentNullException("toSimplify", "You must provide a gameobject to simplify.");
		}
		if (!simplificationOptions.simplifyMeshLossless)
		{
			if (!(simplificationStrength >= 0f) || !(simplificationStrength <= 100f))
			{
				throw new ArgumentOutOfRangeException("simplificationStrength", "The allowed values for simplification strength are between [0-100] inclusive.");
			}
			if (Mathf.Approximately(simplificationStrength, 0f))
			{
				return null;
			}
		}
		if (simplificationOptions.regardPreservationSpheres && (simplificationOptions.preservationSpheres == null || simplificationOptions.preservationSpheres.Count == 0))
		{
			simplificationOptions.preservationSpheres = new List<PreservationSphere>();
			simplificationOptions.regardPreservationSpheres = false;
		}
		ObjectMeshPairs objectMeshPairs = GetObjectMeshPairs(toSimplify, includeInactive: true);
		if (!AreAnyFeasibleMeshes(objectMeshPairs))
		{
			throw new InvalidOperationException("No mesh/meshes found nested under the provided gameobject to simplify.");
		}
		bool flag = false;
		if (CountTriangles(objectMeshPairs) >= 2000 && objectMeshPairs.Count >= 2)
		{
			flag = true;
		}
		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			flag = false;
		}
		float quality = 1f - simplificationStrength / 100f;
		int count = objectMeshPairs.Count;
		int meshesHandled = 0;
		int threadsRunning = 0;
		bool isError = false;
		string error = "";
		object threadLock1 = new object();
		object threadLock2 = new object();
		if (flag)
		{
			List<CustomMeshActionStructure> meshAssignments = new List<CustomMeshActionStructure>();
			foreach (KeyValuePair<GameObject, MeshRendererPair> item2 in objectMeshPairs)
			{
				GameObject gameObject = item2.Key;
				int num;
				if (gameObject == null)
				{
					num = meshesHandled;
					meshesHandled = num + 1;
					continue;
				}
				MeshRendererPair meshRendererPair = item2.Value;
				if (meshRendererPair.mesh == null)
				{
					num = meshesHandled;
					meshesHandled = num + 1;
					continue;
				}
				MeshSimplifier meshSimplifier = new MeshSimplifier();
				SetParametersForSimplifier(simplificationOptions, meshSimplifier);
				ToleranceSphere[] array = new ToleranceSphere[simplificationOptions.preservationSpheres.Count];
				if (!meshRendererPair.attachedToMeshFilter && simplificationOptions.regardPreservationSpheres)
				{
					meshSimplifier.isSkinned = true;
					SkinnedMeshRenderer component = gameObject.GetComponent<SkinnedMeshRenderer>();
					meshSimplifier.boneWeightsOriginal = meshRendererPair.mesh.boneWeights;
					meshSimplifier.bindPosesOriginal = meshRendererPair.mesh.bindposes;
					meshSimplifier.bonesOriginal = component.bones;
					int num2 = 0;
					foreach (PreservationSphere preservationSphere in simplificationOptions.preservationSpheres)
					{
						ToleranceSphere toleranceSphere = (array[num2] = new ToleranceSphere
						{
							diameter = preservationSphere.diameter,
							localToWorldMatrix = gameObject.transform.localToWorldMatrix,
							worldPosition = preservationSphere.worldPosition,
							targetObject = gameObject,
							preservationStrength = preservationSphere.preservationStrength
						});
						num2++;
					}
					meshSimplifier.toleranceSpheres = array;
				}
				else if (meshRendererPair.attachedToMeshFilter && simplificationOptions.regardPreservationSpheres)
				{
					int num3 = 0;
					foreach (PreservationSphere preservationSphere2 in simplificationOptions.preservationSpheres)
					{
						ToleranceSphere toleranceSphere2 = (array[num3] = new ToleranceSphere
						{
							diameter = preservationSphere2.diameter,
							localToWorldMatrix = gameObject.transform.localToWorldMatrix,
							worldPosition = preservationSphere2.worldPosition,
							targetObject = gameObject,
							preservationStrength = preservationSphere2.preservationStrength
						});
						num3++;
					}
					meshSimplifier.toleranceSpheres = array;
				}
				meshSimplifier.Initialize(meshRendererPair.mesh, simplificationOptions.regardPreservationSpheres);
				num = threadsRunning;
				threadsRunning = num + 1;
				Task.Factory.StartNew(delegate
				{
					CustomMeshActionStructure item = new CustomMeshActionStructure(meshRendererPair, gameObject, delegate
					{
						Mesh mesh2 = meshSimplifier.ToMesh();
						mesh2.bindposes = meshRendererPair.mesh.bindposes;
						mesh2.name = meshRendererPair.mesh.name.Replace("-POLY_REDUCED", "") + "-POLY_REDUCED";
						if (meshSimplifier.RecalculateNormals)
						{
							mesh2.RecalculateNormals();
							mesh2.RecalculateTangents();
						}
						MeshRendererPair value4 = new MeshRendererPair(meshRendererPair.attachedToMeshFilter, mesh2);
						toReturn.Add(gameObject, value4);
					});
					try
					{
						if (!simplificationOptions.simplifyMeshLossless)
						{
							meshSimplifier.SimplifyMesh(quality);
						}
						else
						{
							meshSimplifier.SimplifyMeshLossless();
						}
						lock (threadLock1)
						{
							meshAssignments.Add(item);
							int num6 = threadsRunning;
							threadsRunning = num6 - 1;
							num6 = meshesHandled;
							meshesHandled = num6 + 1;
						}
					}
					catch (Exception ex)
					{
						lock (threadLock2)
						{
							int num6 = threadsRunning;
							threadsRunning = num6 - 1;
							num6 = meshesHandled;
							meshesHandled = num6 + 1;
							isError = true;
							error = ex.ToString();
						}
					}
				}, CancellationToken.None, TaskCreationOptions.RunContinuationsAsynchronously, TaskScheduler.Current);
			}
			while (meshesHandled < count && !isError)
			{
			}
			if (!isError)
			{
				foreach (CustomMeshActionStructure item3 in meshAssignments)
				{
					item3?.action();
				}
			}
		}
		else
		{
			foreach (KeyValuePair<GameObject, MeshRendererPair> item4 in objectMeshPairs)
			{
				GameObject key = item4.Key;
				if (key == null)
				{
					continue;
				}
				MeshRendererPair value = item4.Value;
				if (value.mesh == null)
				{
					continue;
				}
				MeshSimplifier meshSimplifier2 = new MeshSimplifier();
				SetParametersForSimplifier(simplificationOptions, meshSimplifier2);
				ToleranceSphere[] array2 = new ToleranceSphere[simplificationOptions.preservationSpheres.Count];
				if (!value.attachedToMeshFilter && simplificationOptions.regardPreservationSpheres)
				{
					meshSimplifier2.isSkinned = true;
					SkinnedMeshRenderer component2 = key.GetComponent<SkinnedMeshRenderer>();
					meshSimplifier2.boneWeightsOriginal = value.mesh.boneWeights;
					meshSimplifier2.bindPosesOriginal = value.mesh.bindposes;
					meshSimplifier2.bonesOriginal = component2.bones;
					int num4 = 0;
					foreach (PreservationSphere preservationSphere3 in simplificationOptions.preservationSpheres)
					{
						ToleranceSphere toleranceSphere3 = (array2[num4] = new ToleranceSphere
						{
							diameter = preservationSphere3.diameter,
							localToWorldMatrix = key.transform.localToWorldMatrix,
							worldPosition = preservationSphere3.worldPosition,
							targetObject = key,
							preservationStrength = preservationSphere3.preservationStrength
						});
						num4++;
					}
					meshSimplifier2.toleranceSpheres = array2;
				}
				else if (value.attachedToMeshFilter && simplificationOptions.regardPreservationSpheres)
				{
					int num5 = 0;
					foreach (PreservationSphere preservationSphere4 in simplificationOptions.preservationSpheres)
					{
						ToleranceSphere toleranceSphere4 = (array2[num5] = new ToleranceSphere
						{
							diameter = preservationSphere4.diameter,
							localToWorldMatrix = key.transform.localToWorldMatrix,
							worldPosition = preservationSphere4.worldPosition,
							targetObject = key,
							preservationStrength = preservationSphere4.preservationStrength
						});
						num5++;
					}
					meshSimplifier2.toleranceSpheres = array2;
				}
				meshSimplifier2.Initialize(value.mesh, simplificationOptions.regardPreservationSpheres);
				if (!simplificationOptions.simplifyMeshLossless)
				{
					meshSimplifier2.SimplifyMesh(quality);
				}
				else
				{
					meshSimplifier2.SimplifyMeshLossless();
				}
				Mesh mesh = meshSimplifier2.ToMesh();
				mesh.bindposes = value.mesh.bindposes;
				mesh.name = value.mesh.name.Replace("-POLY_REDUCED", "") + "-POLY_REDUCED";
				if (meshSimplifier2.RecalculateNormals)
				{
					mesh.RecalculateNormals();
					mesh.RecalculateTangents();
				}
				if (value.attachedToMeshFilter)
				{
					MeshFilter component3 = key.GetComponent<MeshFilter>();
					MeshRendererPair value2 = new MeshRendererPair(attachedToMeshFilter: true, mesh);
					toReturn.Add(key, value2);
					if (component3 != null)
					{
						component3.sharedMesh = mesh;
					}
				}
				else
				{
					SkinnedMeshRenderer component4 = key.GetComponent<SkinnedMeshRenderer>();
					MeshRendererPair value3 = new MeshRendererPair(attachedToMeshFilter: false, mesh);
					toReturn.Add(key, value3);
					if (component4 != null)
					{
						component4.sharedMesh = mesh;
					}
				}
			}
		}
		return toReturn;
	}

	public static int SimplifyObjectDeep(ObjectMeshPairs objectMeshPairs, SimplificationOptions simplificationOptions, Action<GameObject, MeshRendererPair> OnEachMeshSimplified)
	{
		if (simplificationOptions == null)
		{
			throw new ArgumentNullException("simplificationOptions", "You must provide a SimplificationOptions object.");
		}
		int totalTriangles = 0;
		float simplificationStrength = simplificationOptions.simplificationStrength;
		if (objectMeshPairs == null)
		{
			throw new ArgumentNullException("objectMeshPairs", "You must provide the objectMeshPairs structure to simplify.");
		}
		if (!simplificationOptions.simplifyMeshLossless)
		{
			if (!(simplificationStrength >= 0f) || !(simplificationStrength <= 100f))
			{
				throw new ArgumentOutOfRangeException("simplificationStrength", "The allowed values for simplification strength are between [0-100] inclusive.");
			}
			if (Mathf.Approximately(simplificationStrength, 0f))
			{
				return -1;
			}
		}
		if (!AreAnyFeasibleMeshes(objectMeshPairs))
		{
			throw new InvalidOperationException("No mesh/meshes found nested under the provided gameobject to simplify.");
		}
		if (simplificationOptions.regardPreservationSpheres && (simplificationOptions.preservationSpheres == null || simplificationOptions.preservationSpheres.Count == 0))
		{
			simplificationOptions.preservationSpheres = new List<PreservationSphere>();
			simplificationOptions.regardPreservationSpheres = false;
		}
		bool flag = false;
		if (CountTriangles(objectMeshPairs) >= 2000 && objectMeshPairs.Count >= 2)
		{
			flag = true;
		}
		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			flag = false;
		}
		float quality = 1f - simplificationStrength / 100f;
		int count = objectMeshPairs.Count;
		int meshesHandled = 0;
		int threadsRunning = 0;
		bool isError = false;
		string error = "";
		object threadLock1 = new object();
		object threadLock2 = new object();
		object threadLock3 = new object();
		if (flag)
		{
			List<CustomMeshActionStructure> meshAssignments = new List<CustomMeshActionStructure>();
			List<CustomMeshActionStructure> callbackFlusher = new List<CustomMeshActionStructure>();
			foreach (KeyValuePair<GameObject, MeshRendererPair> objectMeshPair in objectMeshPairs)
			{
				GameObject gameObject = objectMeshPair.Key;
				int num;
				if (gameObject == null)
				{
					num = meshesHandled;
					meshesHandled = num + 1;
					continue;
				}
				MeshRendererPair meshRendererPair = objectMeshPair.Value;
				if (meshRendererPair.mesh == null)
				{
					num = meshesHandled;
					meshesHandled = num + 1;
					continue;
				}
				MeshSimplifier meshSimplifier = new MeshSimplifier();
				SetParametersForSimplifier(simplificationOptions, meshSimplifier);
				ToleranceSphere[] array = new ToleranceSphere[simplificationOptions.preservationSpheres.Count];
				if (!meshRendererPair.attachedToMeshFilter && simplificationOptions.regardPreservationSpheres)
				{
					meshSimplifier.isSkinned = true;
					SkinnedMeshRenderer component = gameObject.GetComponent<SkinnedMeshRenderer>();
					meshSimplifier.boneWeightsOriginal = meshRendererPair.mesh.boneWeights;
					meshSimplifier.bindPosesOriginal = meshRendererPair.mesh.bindposes;
					meshSimplifier.bonesOriginal = component.bones;
					int num2 = 0;
					foreach (PreservationSphere preservationSphere in simplificationOptions.preservationSpheres)
					{
						ToleranceSphere toleranceSphere = (array[num2] = new ToleranceSphere
						{
							diameter = preservationSphere.diameter,
							localToWorldMatrix = gameObject.transform.localToWorldMatrix,
							worldPosition = preservationSphere.worldPosition,
							targetObject = gameObject,
							preservationStrength = preservationSphere.preservationStrength
						});
						num2++;
					}
					meshSimplifier.toleranceSpheres = array;
				}
				else if (meshRendererPair.attachedToMeshFilter && simplificationOptions.regardPreservationSpheres)
				{
					int num3 = 0;
					foreach (PreservationSphere preservationSphere2 in simplificationOptions.preservationSpheres)
					{
						ToleranceSphere toleranceSphere2 = (array[num3] = new ToleranceSphere
						{
							diameter = preservationSphere2.diameter,
							localToWorldMatrix = gameObject.transform.localToWorldMatrix,
							worldPosition = preservationSphere2.worldPosition,
							targetObject = gameObject,
							preservationStrength = preservationSphere2.preservationStrength
						});
						num3++;
					}
					meshSimplifier.toleranceSpheres = array;
				}
				meshSimplifier.Initialize(meshRendererPair.mesh, simplificationOptions.regardPreservationSpheres);
				num = threadsRunning;
				threadsRunning = num + 1;
				while (callbackFlusher.Count > 0)
				{
					CustomMeshActionStructure customMeshActionStructure = callbackFlusher[0];
					callbackFlusher.RemoveAt(0);
					if (customMeshActionStructure != null)
					{
						OnEachMeshSimplified?.Invoke(customMeshActionStructure.gameObject, customMeshActionStructure.meshRendererPair);
					}
				}
				Task.Factory.StartNew(delegate
				{
					CustomMeshActionStructure item = new CustomMeshActionStructure(meshRendererPair, gameObject, delegate
					{
						Mesh mesh2 = meshSimplifier.ToMesh();
						AssignReducedMesh(gameObject, meshRendererPair.mesh, mesh2, meshRendererPair.attachedToMeshFilter, assignBindposes: true);
						if (meshSimplifier.RecalculateNormals)
						{
							mesh2.RecalculateNormals();
							mesh2.RecalculateTangents();
						}
						totalTriangles += mesh2.triangles.Length / 3;
					});
					try
					{
						if (!simplificationOptions.simplifyMeshLossless)
						{
							meshSimplifier.SimplifyMesh(quality);
						}
						else
						{
							meshSimplifier.SimplifyMeshLossless();
						}
						lock (threadLock1)
						{
							meshAssignments.Add(item);
							int num6 = threadsRunning;
							threadsRunning = num6 - 1;
							num6 = meshesHandled;
							meshesHandled = num6 + 1;
						}
						lock (threadLock3)
						{
							CustomMeshActionStructure item2 = new CustomMeshActionStructure(meshRendererPair, gameObject, delegate
							{
							});
							callbackFlusher.Add(item2);
						}
					}
					catch (Exception ex)
					{
						lock (threadLock2)
						{
							int num6 = threadsRunning;
							threadsRunning = num6 - 1;
							num6 = meshesHandled;
							meshesHandled = num6 + 1;
							isError = true;
							error = ex.ToString();
						}
					}
				}, CancellationToken.None, TaskCreationOptions.RunContinuationsAsynchronously, TaskScheduler.Current);
			}
			while (callbackFlusher.Count > 0)
			{
				CustomMeshActionStructure customMeshActionStructure2 = callbackFlusher[0];
				callbackFlusher.RemoveAt(0);
				if (customMeshActionStructure2 != null)
				{
					OnEachMeshSimplified?.Invoke(customMeshActionStructure2.gameObject, customMeshActionStructure2.meshRendererPair);
				}
			}
			while (meshesHandled < count && !isError)
			{
				while (callbackFlusher.Count > 0)
				{
					CustomMeshActionStructure customMeshActionStructure3 = callbackFlusher[0];
					callbackFlusher.RemoveAt(0);
					if (customMeshActionStructure3 != null)
					{
						OnEachMeshSimplified?.Invoke(customMeshActionStructure3.gameObject, customMeshActionStructure3.meshRendererPair);
					}
				}
			}
			if (!isError)
			{
				foreach (CustomMeshActionStructure item3 in meshAssignments)
				{
					item3?.action();
				}
			}
		}
		else
		{
			foreach (KeyValuePair<GameObject, MeshRendererPair> objectMeshPair2 in objectMeshPairs)
			{
				GameObject key = objectMeshPair2.Key;
				if (key == null)
				{
					continue;
				}
				MeshRendererPair value = objectMeshPair2.Value;
				if (value.mesh == null)
				{
					continue;
				}
				MeshSimplifier meshSimplifier2 = new MeshSimplifier();
				SetParametersForSimplifier(simplificationOptions, meshSimplifier2);
				ToleranceSphere[] array2 = new ToleranceSphere[simplificationOptions.preservationSpheres.Count];
				if (!value.attachedToMeshFilter && simplificationOptions.regardPreservationSpheres)
				{
					meshSimplifier2.isSkinned = true;
					SkinnedMeshRenderer component2 = key.GetComponent<SkinnedMeshRenderer>();
					meshSimplifier2.boneWeightsOriginal = value.mesh.boneWeights;
					meshSimplifier2.bindPosesOriginal = value.mesh.bindposes;
					meshSimplifier2.bonesOriginal = component2.bones;
					int num4 = 0;
					foreach (PreservationSphere preservationSphere3 in simplificationOptions.preservationSpheres)
					{
						ToleranceSphere toleranceSphere3 = (array2[num4] = new ToleranceSphere
						{
							diameter = preservationSphere3.diameter,
							localToWorldMatrix = key.transform.localToWorldMatrix,
							worldPosition = preservationSphere3.worldPosition,
							targetObject = key,
							preservationStrength = preservationSphere3.preservationStrength
						});
						num4++;
					}
					meshSimplifier2.toleranceSpheres = array2;
				}
				else if (value.attachedToMeshFilter && simplificationOptions.regardPreservationSpheres)
				{
					int num5 = 0;
					foreach (PreservationSphere preservationSphere4 in simplificationOptions.preservationSpheres)
					{
						ToleranceSphere toleranceSphere4 = (array2[num5] = new ToleranceSphere
						{
							diameter = preservationSphere4.diameter,
							localToWorldMatrix = key.transform.localToWorldMatrix,
							worldPosition = preservationSphere4.worldPosition,
							targetObject = key,
							preservationStrength = preservationSphere4.preservationStrength
						});
						num5++;
					}
					meshSimplifier2.toleranceSpheres = array2;
				}
				meshSimplifier2.Initialize(value.mesh, simplificationOptions.regardPreservationSpheres);
				if (!simplificationOptions.simplifyMeshLossless)
				{
					meshSimplifier2.SimplifyMesh(quality);
				}
				else
				{
					meshSimplifier2.SimplifyMeshLossless();
				}
				OnEachMeshSimplified?.Invoke(key, value);
				Mesh mesh = meshSimplifier2.ToMesh();
				mesh.bindposes = value.mesh.bindposes;
				mesh.name = value.mesh.name.Replace("-POLY_REDUCED", "") + "-POLY_REDUCED";
				if (meshSimplifier2.RecalculateNormals)
				{
					mesh.RecalculateNormals();
					mesh.RecalculateTangents();
				}
				if (value.attachedToMeshFilter)
				{
					MeshFilter component3 = key.GetComponent<MeshFilter>();
					if (component3 != null)
					{
						component3.sharedMesh = mesh;
					}
				}
				else
				{
					SkinnedMeshRenderer component4 = key.GetComponent<SkinnedMeshRenderer>();
					if (component4 != null)
					{
						component4.sharedMesh = mesh;
					}
				}
				totalTriangles += mesh.triangles.Length / 3;
			}
		}
		return totalTriangles;
	}

	public static List<Mesh> SimplifyMeshes(List<Mesh> meshesToSimplify, SimplificationOptions simplificationOptions, Action<Mesh> OnEachMeshSimplified)
	{
		List<Mesh> simplifiedMeshes = new List<Mesh>();
		if (simplificationOptions == null)
		{
			throw new ArgumentNullException("simplificationOptions", "You must provide a SimplificationOptions object.");
		}
		float simplificationStrength = simplificationOptions.simplificationStrength;
		if (meshesToSimplify == null)
		{
			throw new ArgumentNullException("meshesToSimplify", "You must provide a meshes list to simplify.");
		}
		if (meshesToSimplify.Count == 0)
		{
			throw new InvalidOperationException("You must provide a non-empty list of meshes to simplify.");
		}
		if (!simplificationOptions.simplifyMeshLossless)
		{
			if (!(simplificationStrength >= 0f) || !(simplificationStrength <= 100f))
			{
				throw new ArgumentOutOfRangeException("simplificationStrength", "The allowed values for simplification strength are between [0-100] inclusive.");
			}
			if (Mathf.Approximately(simplificationStrength, 0f))
			{
				return null;
			}
		}
		if (CountTriangles(meshesToSimplify) >= 2000)
		{
			_ = meshesToSimplify.Count;
			_ = 2;
		}
		_ = Application.platform;
		_ = 17;
		float quality = 1f - simplificationStrength / 100f;
		int count = meshesToSimplify.Count;
		int meshesHandled = 0;
		int threadsRunning = 0;
		bool isError = false;
		string error = "";
		object threadLock1 = new object();
		object threadLock2 = new object();
		object threadLock3 = new object();
		if (true)
		{
			List<CustomMeshActionStructure> meshAssignments = new List<CustomMeshActionStructure>();
			List<CustomMeshActionStructure> callbackFlusher = new List<CustomMeshActionStructure>();
			foreach (Mesh meshToSimplify in meshesToSimplify)
			{
				int num;
				if (meshToSimplify == null)
				{
					num = meshesHandled;
					meshesHandled = num + 1;
					continue;
				}
				MeshSimplifier meshSimplifier = new MeshSimplifier();
				SetParametersForSimplifier(simplificationOptions, meshSimplifier);
				meshSimplifier.Initialize(meshToSimplify);
				num = threadsRunning;
				threadsRunning = num + 1;
				while (callbackFlusher.Count > 0)
				{
					CustomMeshActionStructure customMeshActionStructure = callbackFlusher[0];
					callbackFlusher.RemoveAt(0);
					OnEachMeshSimplified?.Invoke(customMeshActionStructure.meshRendererPair.mesh);
				}
				Task.Factory.StartNew(delegate
				{
					CustomMeshActionStructure item = new CustomMeshActionStructure(null, null, delegate
					{
						Mesh mesh2 = meshSimplifier.ToMesh();
						mesh2.bindposes = meshToSimplify.bindposes;
						mesh2.name = meshToSimplify.name.Replace("-POLY_REDUCED", "") + "-POLY_REDUCED";
						if (meshSimplifier.RecalculateNormals)
						{
							mesh2.RecalculateNormals();
							mesh2.RecalculateTangents();
						}
						simplifiedMeshes.Add(mesh2);
					});
					try
					{
						if (!simplificationOptions.simplifyMeshLossless)
						{
							meshSimplifier.SimplifyMesh(quality);
						}
						else
						{
							meshSimplifier.SimplifyMeshLossless();
						}
						lock (threadLock1)
						{
							meshAssignments.Add(item);
							int num2 = threadsRunning;
							threadsRunning = num2 - 1;
							num2 = meshesHandled;
							meshesHandled = num2 + 1;
						}
						lock (threadLock3)
						{
							CustomMeshActionStructure item2 = new CustomMeshActionStructure(new MeshRendererPair(attachedToMeshFilter: true, meshToSimplify), null, delegate
							{
							});
							callbackFlusher.Add(item2);
						}
					}
					catch (Exception ex)
					{
						lock (threadLock2)
						{
							int num2 = threadsRunning;
							threadsRunning = num2 - 1;
							num2 = meshesHandled;
							meshesHandled = num2 + 1;
							isError = true;
							error = ex.ToString();
						}
					}
				}, CancellationToken.None, TaskCreationOptions.RunContinuationsAsynchronously, TaskScheduler.Current);
			}
			while (callbackFlusher.Count > 0)
			{
				CustomMeshActionStructure customMeshActionStructure2 = callbackFlusher[0];
				callbackFlusher.RemoveAt(0);
				OnEachMeshSimplified?.Invoke(customMeshActionStructure2.meshRendererPair.mesh);
			}
			while (meshesHandled < count && !isError)
			{
				while (callbackFlusher.Count > 0)
				{
					CustomMeshActionStructure customMeshActionStructure3 = callbackFlusher[0];
					callbackFlusher.RemoveAt(0);
					OnEachMeshSimplified?.Invoke(customMeshActionStructure3.meshRendererPair.mesh);
				}
			}
			if (!isError)
			{
				foreach (CustomMeshActionStructure item3 in meshAssignments)
				{
					item3?.action();
				}
			}
		}
		else
		{
			foreach (Mesh item4 in meshesToSimplify)
			{
				if (!(item4 == null))
				{
					MeshSimplifier meshSimplifier2 = new MeshSimplifier();
					SetParametersForSimplifier(simplificationOptions, meshSimplifier2);
					meshSimplifier2.Initialize(item4);
					if (!simplificationOptions.simplifyMeshLossless)
					{
						meshSimplifier2.SimplifyMesh(quality);
					}
					else
					{
						meshSimplifier2.SimplifyMeshLossless();
					}
					OnEachMeshSimplified?.Invoke(item4);
					Mesh mesh = meshSimplifier2.ToMesh();
					mesh.bindposes = item4.bindposes;
					mesh.name = item4.name.Replace("-POLY_REDUCED", "") + "-POLY_REDUCED";
					if (meshSimplifier2.RecalculateNormals)
					{
						mesh.RecalculateNormals();
						mesh.RecalculateTangents();
					}
					simplifiedMeshes.Add(mesh);
				}
			}
		}
		return simplifiedMeshes;
	}

	public static ObjectMeshPairs GetObjectMeshPairs(GameObject forObject, bool includeInactive)
	{
		if (forObject == null)
		{
			throw new ArgumentNullException("forObject", "You must provide a gameobject to get the ObjectMeshPairs for.");
		}
		ObjectMeshPairs objectMeshPairs = new ObjectMeshPairs();
		MeshFilter[] componentsInChildren = forObject.GetComponentsInChildren<MeshFilter>(includeInactive);
		if (componentsInChildren != null && componentsInChildren.Length != 0)
		{
			MeshFilter[] array = componentsInChildren;
			foreach (MeshFilter meshFilter in array)
			{
				if ((bool)meshFilter.sharedMesh)
				{
					MeshRendererPair value = new MeshRendererPair(attachedToMeshFilter: true, meshFilter.sharedMesh);
					objectMeshPairs.Add(meshFilter.gameObject, value);
				}
			}
		}
		SkinnedMeshRenderer[] componentsInChildren2 = forObject.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive);
		if (componentsInChildren2 != null && componentsInChildren2.Length != 0)
		{
			SkinnedMeshRenderer[] array2 = componentsInChildren2;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in array2)
			{
				if ((bool)skinnedMeshRenderer.sharedMesh)
				{
					MeshRendererPair value2 = new MeshRendererPair(attachedToMeshFilter: false, skinnedMeshRenderer.sharedMesh);
					objectMeshPairs.Add(skinnedMeshRenderer.gameObject, value2);
				}
			}
		}
		return objectMeshPairs;
	}

	public static void CombineMeshesInGameObject(GameObject forObject, bool skipInactiveChildObjects, Action<string, string> OnError, MeshCombineTarget combineTarget = MeshCombineTarget.SkinnedAndStatic)
	{
		if (forObject == null)
		{
			OnError?.Invoke("Argument Null Exception", "You must provide a gameobject whose meshes will be combined.");
			return;
		}
		Renderer[] childRenderersForCombining = UtilityServicesRuntime.GetChildRenderersForCombining(forObject, skipInactiveChildObjects);
		if (childRenderersForCombining == null || childRenderersForCombining.Length == 0)
		{
			OnError?.Invoke("Operation Failed", "No feasible renderers found under the provided object to combine.");
			return;
		}
		MeshRenderer[] array = null;
		HashSet<Transform> hashSet = new HashSet<Transform>();
		SkinnedMeshRenderer[] array2 = null;
		HashSet<Transform> hashSet2 = new HashSet<Transform>();
		MeshCombiner.StaticRenderer[] array3 = null;
		MeshCombiner.SkinnedRenderer[] array4 = null;
		if (skipInactiveChildObjects)
		{
			array = (from renderer in childRenderersForCombining
				where renderer.enabled && renderer as MeshRenderer != null && renderer.transform.GetComponent<MeshFilter>() != null && renderer.transform.GetComponent<MeshFilter>().sharedMesh != null
				select renderer as MeshRenderer).ToArray();
			array2 = (from renderer in childRenderersForCombining
				where renderer.enabled && renderer as SkinnedMeshRenderer != null && renderer.transform.GetComponent<SkinnedMeshRenderer>().sharedMesh != null
				select renderer as SkinnedMeshRenderer).ToArray();
		}
		else
		{
			array = (from renderer in childRenderersForCombining
				where renderer as MeshRenderer != null && renderer.transform.GetComponent<MeshFilter>() != null && renderer.transform.GetComponent<MeshFilter>().sharedMesh != null
				select renderer as MeshRenderer).ToArray();
			array2 = (from renderer in childRenderersForCombining
				where renderer as SkinnedMeshRenderer != null && renderer.transform.GetComponent<SkinnedMeshRenderer>().sharedMesh != null
				select renderer as SkinnedMeshRenderer).ToArray();
		}
		MeshRenderer[] array5;
		if (array != null)
		{
			array5 = array;
			foreach (MeshRenderer meshRenderer in array5)
			{
				hashSet.Add(meshRenderer.transform);
			}
		}
		if (array2 != null)
		{
			SkinnedMeshRenderer[] array6 = array2;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in array6)
			{
				hashSet2.Add(skinnedMeshRenderer.transform);
			}
		}
		switch (combineTarget)
		{
		case MeshCombineTarget.StaticOnly:
			array2 = new SkinnedMeshRenderer[0];
			break;
		case MeshCombineTarget.SkinnedOnly:
			array = new MeshRenderer[0];
			break;
		}
		array3 = MeshCombiner.GetStaticRenderers(array);
		array4 = MeshCombiner.GetSkinnedRenderers(array2);
		int num = array2.Where((SkinnedMeshRenderer renderer) => renderer.sharedMesh != null).Count();
		int num2 = ((array3 != null) ? array3.Length : 0);
		_ = array4?.Length;
		if ((num2 == 0 || num2 == 1) && (num == 0 || num == 1))
		{
			string arg = "Nothing combined in GameObject \"" + forObject.name + "\". Not enough feasible renderers/meshes to combine.";
			switch (combineTarget)
			{
			case MeshCombineTarget.StaticOnly:
				arg = "Nothing combined in GameObject \"" + forObject.name + "\". Not enough feasible static meshes to combine. Consider selecting the option of combining both skinned and static meshes.";
				break;
			case MeshCombineTarget.SkinnedOnly:
				arg = "Nothing combined in GameObject \"" + forObject.name + "\". Not enough feasible skinned meshes to combine. Consider selecting the option of combining both skinned and static meshes.";
				break;
			}
			OnError?.Invoke("Operation Failed", arg);
			return;
		}
		SkinnedMeshRenderer[] renderersActuallyCombined = null;
		MeshCombiner.StaticRenderer[] array7 = MeshCombiner.CombineStaticMeshes(forObject.transform, -1, array, autoName: false);
		MeshCombiner.SkinnedRenderer[] array8 = MeshCombiner.CombineSkinnedMeshes(forObject.transform, -1, array2, ref renderersActuallyCombined, autoName: false);
		if (renderersActuallyCombined != null)
		{
			SkinnedMeshRenderer[] array6 = renderersActuallyCombined;
			for (int i = 0; i < array6.Length; i++)
			{
				array6[i].enabled = false;
			}
		}
		if (array != null)
		{
			array5 = array;
			for (int i = 0; i < array5.Length; i++)
			{
				array5[i].enabled = false;
			}
		}
		int num3 = ((array7 != null) ? array7.Length : 0);
		int num4 = ((array8 != null) ? array8.Length : 0);
		Transform parentTransform = forObject.transform;
		HashSet<Transform> hashSet3 = new HashSet<Transform>();
		HashSet<Transform> hashSet4 = new HashSet<Transform>();
		for (int j = 0; j < num3; j++)
		{
			MeshCombiner.StaticRenderer staticRenderer = array7[j];
			Mesh mesh = staticRenderer.mesh;
			MeshRenderer meshRenderer2 = UtilityServicesRuntime.CreateStaticLevelRenderer(string.Format("{0}_combined_static", staticRenderer.name.Replace("_combined", "")), parentTransform, staticRenderer.transform, mesh, staticRenderer.materials);
			hashSet3.Add(meshRenderer2.transform);
			meshRenderer2.transform.parent = forObject.transform;
		}
		for (int k = 0; k < num4; k++)
		{
			MeshCombiner.SkinnedRenderer skinnedRenderer = array8[k];
			Mesh mesh2 = skinnedRenderer.mesh;
			SkinnedMeshRenderer skinnedMeshRenderer2 = UtilityServicesRuntime.CreateSkinnedLevelRenderer(string.Format("{0}_combined_skinned", skinnedRenderer.name.Replace("_combined", "")), parentTransform, skinnedRenderer.transform, mesh2, skinnedRenderer.materials, skinnedRenderer.rootBone, skinnedRenderer.bones);
			hashSet4.Add(skinnedMeshRenderer2.transform);
			skinnedMeshRenderer2.transform.parent = forObject.transform;
		}
		GameObject gameObject = new GameObject(forObject.name + "_bonesHiererachy");
		gameObject.transform.parent = forObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		Transform[] array9 = new Transform[forObject.transform.childCount];
		for (int l = 0; l < forObject.transform.childCount; l++)
		{
			array9[l] = forObject.transform.GetChild(l);
		}
		foreach (Transform transform in array9)
		{
			switch (combineTarget)
			{
			case MeshCombineTarget.SkinnedAndStatic:
				if (!hashSet4.Contains(transform) && !hashSet3.Contains(transform))
				{
					transform.parent = gameObject.transform;
				}
				break;
			case MeshCombineTarget.StaticOnly:
				if (!hashSet3.Contains(transform) && !hashSet2.Contains(transform))
				{
					transform.parent = gameObject.transform;
				}
				break;
			default:
				if (!hashSet4.Contains(transform) && !hashSet.Contains(transform))
				{
					transform.parent = gameObject.transform;
				}
				break;
			}
		}
		if (renderersActuallyCombined != null)
		{
			SkinnedMeshRenderer[] array6 = renderersActuallyCombined;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer3 in array6)
			{
				if (!(skinnedMeshRenderer3 == null))
				{
					skinnedMeshRenderer3.sharedMesh = null;
				}
			}
		}
		if (array == null)
		{
			return;
		}
		array5 = array;
		foreach (MeshRenderer meshRenderer3 in array5)
		{
			if (!(meshRenderer3 == null))
			{
				MeshFilter component = meshRenderer3.GetComponent<MeshFilter>();
				if (!(component == null))
				{
					component.sharedMesh = null;
				}
			}
		}
	}

	public static GameObject CombineMeshesFromRenderers(Transform rootTransform, MeshRenderer[] originalMeshRenderers, SkinnedMeshRenderer[] originalSkinnedMeshRenderers, Action<string, string> OnError)
	{
		if (rootTransform == null)
		{
			OnError?.Invoke("Argument Null Exception", "You must provide a root transform to create the combined meshes based from.");
			return null;
		}
		if ((originalMeshRenderers == null || originalMeshRenderers.Length == 0) && (originalSkinnedMeshRenderers == null || originalSkinnedMeshRenderers.Length == 0))
		{
			OnError?.Invoke("Operation Failed", "Both the Static and Skinned renderers list is empty. Atleast one of them must be non empty.");
			return null;
		}
		if (originalMeshRenderers == null)
		{
			originalMeshRenderers = new MeshRenderer[0];
		}
		if (originalSkinnedMeshRenderers == null)
		{
			originalSkinnedMeshRenderers = new SkinnedMeshRenderer[0];
		}
		originalMeshRenderers = (from renderer in originalMeshRenderers
			where renderer.transform.GetComponent<MeshFilter>() != null && renderer.transform.GetComponent<MeshFilter>().sharedMesh != null
			select (renderer)).ToArray();
		originalSkinnedMeshRenderers = (from renderer in originalSkinnedMeshRenderers
			where renderer.transform.GetComponent<SkinnedMeshRenderer>().sharedMesh != null
			select (renderer)).ToArray();
		if ((originalMeshRenderers == null || originalMeshRenderers.Length == 0) && (originalSkinnedMeshRenderers == null || originalSkinnedMeshRenderers.Length == 0))
		{
			OnError?.Invoke("Operation Failed", "Couldn't combine any meshes. Couldn't find any feasible renderers in the provided lists to combine.");
			return null;
		}
		SkinnedMeshRenderer[] renderersActuallyCombined = null;
		MeshCombiner.StaticRenderer[] array = MeshCombiner.CombineStaticMeshes(rootTransform, -1, originalMeshRenderers, autoName: false);
		MeshCombiner.SkinnedRenderer[] array2 = MeshCombiner.CombineSkinnedMeshes(rootTransform, -1, originalSkinnedMeshRenderers, ref renderersActuallyCombined, autoName: false);
		if ((array == null || array.Length == 0) && (array2 == null || array2.Length == 0))
		{
			OnError?.Invoke("Operation Failed", "Couldn't combine any meshes due to unknown reasons.");
			return null;
		}
		GameObject gameObject = new GameObject(rootTransform.name + "_Combined_Meshes");
		Transform parentTransform = gameObject.transform;
		if (array != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				MeshCombiner.StaticRenderer staticRenderer = array[i];
				Mesh mesh = staticRenderer.mesh;
				UtilityServicesRuntime.CreateStaticLevelRenderer(string.Format("{0}_combined_static", staticRenderer.name.Replace("_combined", "")), parentTransform, staticRenderer.transform, mesh, staticRenderer.materials);
			}
		}
		if (array2 != null)
		{
			for (int j = 0; j < array2.Length; j++)
			{
				MeshCombiner.SkinnedRenderer skinnedRenderer = array2[j];
				Mesh mesh2 = skinnedRenderer.mesh;
				UtilityServicesRuntime.CreateSkinnedLevelRenderer(string.Format("{0}_combined_skinned", skinnedRenderer.name.Replace("_combined", "")), parentTransform, skinnedRenderer.transform, mesh2, skinnedRenderer.materials, skinnedRenderer.rootBone, skinnedRenderer.bones);
			}
		}
		return gameObject;
	}

	public static async void ImportOBJFromFileSystem(string objAbsolutePath, string texturesFolderPath, string materialsFolderPath, Action<GameObject> OnSuccess, Action<Exception> OnError, OBJImportOptions importOptions = null)
	{
		UtilityServicesRuntime.OBJExporterImporter oBJExporterImporter = new UtilityServicesRuntime.OBJExporterImporter();
		bool isWorking = true;
		try
		{
			await oBJExporterImporter.ImportFromLocalFileSystem(objAbsolutePath, texturesFolderPath, materialsFolderPath, delegate(GameObject importedObject)
			{
				isWorking = false;
				OnSuccess(importedObject);
			}, importOptions);
		}
		catch (Exception obj)
		{
			isWorking = false;
			OnError(obj);
		}
		while (isWorking)
		{
			await Task.Delay(1);
		}
	}

	public static async void ImportOBJFromNetwork(string objURL, string objName, string diffuseTexURL, string bumpTexURL, string specularTexURL, string opacityTexURL, string materialURL, ReferencedNumeric<float> downloadProgress, Action<GameObject> OnSuccess, Action<Exception> OnError, OBJImportOptions importOptions = null)
	{
		UtilityServicesRuntime.OBJExporterImporter oBJExporterImporter = new UtilityServicesRuntime.OBJExporterImporter();
		bool isWorking = true;
		oBJExporterImporter.ImportFromNetwork(objURL, objName, diffuseTexURL, bumpTexURL, specularTexURL, opacityTexURL, materialURL, downloadProgress, delegate(GameObject importedObject)
		{
			isWorking = false;
			OnSuccess(importedObject);
		}, delegate(Exception ex)
		{
			isWorking = false;
			OnError(ex);
		}, importOptions);
		while (isWorking)
		{
			await Task.Delay(1);
		}
	}

	public static async void ExportGameObjectToOBJ(GameObject toExport, string exportPath, Action OnSuccess, Action<Exception> OnError, OBJExportOptions exportOptions = null)
	{
		UtilityServicesRuntime.OBJExporterImporter oBJExporterImporter = new UtilityServicesRuntime.OBJExporterImporter();
		bool isWorking = true;
		try
		{
			oBJExporterImporter.ExportGameObjectToOBJ(toExport, exportPath, exportOptions, delegate
			{
				isWorking = false;
				OnSuccess();
			});
		}
		catch (Exception obj)
		{
			isWorking = false;
			OnError(obj);
		}
		while (isWorking)
		{
			await Task.Delay(1);
		}
	}

	public static int CountTriangles(bool countDeep, GameObject forObject)
	{
		int num = 0;
		if (forObject == null)
		{
			return 0;
		}
		if (countDeep)
		{
			MeshFilter[] componentsInChildren = forObject.GetComponentsInChildren<MeshFilter>(includeInactive: true);
			if (componentsInChildren != null && componentsInChildren.Length != 0)
			{
				MeshFilter[] array = componentsInChildren;
				foreach (MeshFilter meshFilter in array)
				{
					if ((bool)meshFilter.sharedMesh)
					{
						num += meshFilter.sharedMesh.triangles.Length / 3;
					}
				}
			}
			SkinnedMeshRenderer[] componentsInChildren2 = forObject.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);
			if (componentsInChildren2 != null && componentsInChildren2.Length != 0)
			{
				SkinnedMeshRenderer[] array2 = componentsInChildren2;
				foreach (SkinnedMeshRenderer skinnedMeshRenderer in array2)
				{
					if ((bool)skinnedMeshRenderer.sharedMesh)
					{
						num += skinnedMeshRenderer.sharedMesh.triangles.Length / 3;
					}
				}
			}
		}
		else
		{
			MeshFilter component = forObject.GetComponent<MeshFilter>();
			SkinnedMeshRenderer component2 = forObject.GetComponent<SkinnedMeshRenderer>();
			if ((bool)component && (bool)component.sharedMesh)
			{
				num = component.sharedMesh.triangles.Length / 3;
			}
			else if ((bool)component2 && (bool)component2.sharedMesh)
			{
				num = component2.sharedMesh.triangles.Length / 3;
			}
		}
		return num;
	}

	public static int CountTriangles(List<Mesh> toCount)
	{
		int num = 0;
		if (toCount == null || toCount.Count == 0)
		{
			return 0;
		}
		foreach (Mesh item in toCount)
		{
			if (item != null)
			{
				num += item.triangles.Length / 3;
			}
		}
		return num;
	}

	private static void SetParametersForSimplifier(SimplificationOptions simplificationOptions, MeshSimplifier meshSimplifier)
	{
		meshSimplifier.RecalculateNormals = simplificationOptions.recalculateNormals;
		meshSimplifier.EnableSmartLink = simplificationOptions.enableSmartlinking;
		meshSimplifier.PreserveUVSeamEdges = simplificationOptions.preserveUVSeamEdges;
		meshSimplifier.PreserveUVFoldoverEdges = simplificationOptions.preserveUVFoldoverEdges;
		meshSimplifier.PreserveBorderEdges = simplificationOptions.preserveBorderEdges;
		meshSimplifier.MaxIterationCount = simplificationOptions.maxIterations;
		meshSimplifier.Aggressiveness = simplificationOptions.aggressiveness;
		meshSimplifier.RegardCurvature = simplificationOptions.regardCurvature;
		meshSimplifier.UseSortedEdgeMethod = simplificationOptions.useEdgeSort;
	}

	private static bool AreAnyFeasibleMeshes(ObjectMeshPairs objectMeshPairs)
	{
		if (objectMeshPairs == null || objectMeshPairs.Count == 0)
		{
			return false;
		}
		foreach (KeyValuePair<GameObject, MeshRendererPair> objectMeshPair in objectMeshPairs)
		{
			MeshRendererPair value = objectMeshPair.Value;
			GameObject key = objectMeshPair.Key;
			if (key == null || value == null)
			{
				continue;
			}
			if (value.attachedToMeshFilter)
			{
				if (!(key.GetComponent<MeshFilter>() == null) && !(value.mesh == null))
				{
					return true;
				}
			}
			else if (!value.attachedToMeshFilter && !(key.GetComponent<SkinnedMeshRenderer>() == null) && !(value.mesh == null))
			{
				return true;
			}
		}
		return false;
	}

	private static void AssignReducedMesh(GameObject gameObject, Mesh originalMesh, Mesh reducedMesh, bool attachedToMeshfilter, bool assignBindposes)
	{
		if (assignBindposes)
		{
			reducedMesh.bindposes = originalMesh.bindposes;
		}
		reducedMesh.name = originalMesh.name.Replace("-POLY_REDUCED", "") + "-POLY_REDUCED";
		if (attachedToMeshfilter)
		{
			MeshFilter component = gameObject.GetComponent<MeshFilter>();
			if (component != null)
			{
				component.sharedMesh = reducedMesh;
			}
		}
		else
		{
			SkinnedMeshRenderer component2 = gameObject.GetComponent<SkinnedMeshRenderer>();
			if (component2 != null)
			{
				component2.sharedMesh = reducedMesh;
			}
		}
	}

	private static int CountTriangles(ObjectMeshPairs objectMeshPairs)
	{
		int num = 0;
		if (objectMeshPairs == null)
		{
			return 0;
		}
		foreach (KeyValuePair<GameObject, MeshRendererPair> objectMeshPair in objectMeshPairs)
		{
			if (!(objectMeshPair.Key == null) && objectMeshPair.Value != null && !(objectMeshPair.Value.mesh == null))
			{
				num += objectMeshPair.Value.mesh.triangles.Length / 3;
			}
		}
		return num;
	}
}
