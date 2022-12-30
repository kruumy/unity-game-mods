using System;
using System.Collections;
using System.Collections.Generic;
using HellTap.MeshDecimator.Unity;
using UnityEngine;
using UnityEngine.Rendering;

namespace HellTap.MeshKit;

public class MeshKit : MonoBehaviour
{
	public class BatchMeshes
	{
		public Material[] key;

		public Mesh originalMesh;

		public Mesh[] splitMeshes;

		public ArrayList gos;
	}

	[Serializable]
	public class AutoLODSettings
	{
		private const string _UNTAGGED = "Untagged";

		[Header("LOD Distance")]
		[Range(0.01f, 100f)]
		[Tooltip("At what distance should this LOD be shown? 100 is used for the best quality mesh.")]
		public float lodDistancePercentage = 50f;

		[Header("Decimation")]
		[Range(0.01f, 1f)]
		[Tooltip("When decimating, a value of 0 will reduce mesh complexity as much as possible. 1 will preserve it.")]
		public float quality = 0.8f;

		[Header("Renderers")]
		[Tooltip("The Skin Quality setting used in the Renderer.")]
		public SkinQuality skinQuality;

		[Tooltip("The Recieve Shadows setting used in the Renderer.")]
		public bool receiveShadows = true;

		[Tooltip("The Shadow Casting setting used in the Renderer.")]
		public ShadowCastingMode shadowCasting = ShadowCastingMode.On;

		[Tooltip("The Motion Vectors setting used in the Renderer.")]
		public MotionVectorGenerationMode motionVectors = MotionVectorGenerationMode.Object;

		[Tooltip("The Skinned Motion Vectors setting used in the Renderer.")]
		public bool skinnedMotionVectors = true;

		[Tooltip("The Light Probe Usage setting found in the Renderer.")]
		public LightProbeUsage lightProbeUsage = LightProbeUsage.BlendProbes;

		[Tooltip("The Reflection Probe Usage setting found in the Renderer.")]
		public ReflectionProbeUsage reflectionProbeUsage = ReflectionProbeUsage.BlendProbes;

		[Header("GameObject")]
		[Tooltip("The tag to use on the GameObject.")]
		public string tag;

		[Tooltip("The layer to use on the GameObject.")]
		public int layer;

		public AutoLODSettings(float lodDistancePercentageValue, float qualityValue = 0.8f)
		{
			lodDistancePercentage = lodDistancePercentageValue;
			quality = qualityValue;
			skinQuality = SkinQuality.Auto;
			receiveShadows = true;
			shadowCasting = ShadowCastingMode.On;
			motionVectors = MotionVectorGenerationMode.Object;
			skinnedMotionVectors = true;
			lightProbeUsage = LightProbeUsage.BlendProbes;
			reflectionProbeUsage = ReflectionProbeUsage.BlendProbes;
			tag = "Untagged";
			layer = 0;
		}

		public AutoLODSettings(float lodDistancePercentage, float quality, SkinQuality skinQuality, bool receiveShadows, ShadowCastingMode shadowCasting, MotionVectorGenerationMode motionVectors, bool skinnedMotionVectors, LightProbeUsage lightProbeUsage = LightProbeUsage.BlendProbes, ReflectionProbeUsage reflectionProbeUsage = ReflectionProbeUsage.BlendProbes, string tag = "Untagged", int layer = 0)
		{
			this.lodDistancePercentage = lodDistancePercentage;
			this.quality = quality;
			this.skinQuality = skinQuality;
			this.receiveShadows = receiveShadows;
			this.shadowCasting = shadowCasting;
			this.motionVectors = motionVectors;
			this.skinnedMotionVectors = skinnedMotionVectors;
			this.lightProbeUsage = lightProbeUsage;
			this.reflectionProbeUsage = reflectionProbeUsage;
			this.tag = tag;
			this.layer = layer;
		}

		public LODSettings ToLODSettings()
		{
			return new LODSettings(quality, lodDistancePercentage, skinQuality, receiveShadows, shadowCasting, motionVectors, skinnedMotionVectors, lightProbeUsage, reflectionProbeUsage, tag, layer);
		}
	}

	private static bool debug = true;

	private const int maxVertices = 65534;

	private static MeshKit _instance;

	public static MeshKit com
	{
		get
		{
			if (_instance == null)
			{
				if (UnityEngine.Object.FindObjectsOfType<MeshKit>().Length > 1)
				{
					Debug.LogWarning("MESHKIT: There is more than 1 MeshKit Manager in this scene. You do not need to add this script as it is created dynamically. Please remove all MeshKit.cs components from your scene.");
				}
				_instance = UnityEngine.Object.FindObjectOfType<MeshKit>();
				if (_instance == null)
				{
					_instance = new GameObject("MeshKit Manager")
					{
						hideFlags = HideFlags.HideAndDontSave
					}.AddComponent<MeshKit>();
				}
			}
			return _instance;
		}
	}

	public static Mesh RebuildMesh(Mesh m, bool optionStripNormals, bool optionStripTangents, bool optionStripColors, bool optionStripUV2, bool optionStripUV3, bool optionStripUV4, bool optionStripUV5, bool optionStripUV6, bool optionStripUV7, bool optionStripUV8, bool optionRebuildNormals, bool optionRebuildTangents, float rebuildNormalsAngle = -1f)
	{
		if (m != null)
		{
			Mesh mesh = new Mesh();
			mesh.Clear();
			mesh.vertices = m.vertices;
			mesh.uv = m.uv;
			mesh.uv2 = m.uv2;
			mesh.uv3 = m.uv3;
			mesh.uv4 = m.uv4;
			mesh.uv5 = m.uv5;
			mesh.uv6 = m.uv6;
			mesh.uv7 = m.uv7;
			mesh.uv8 = m.uv8;
			if (optionStripUV2)
			{
				mesh.uv2 = new Vector2[0];
			}
			if (optionStripUV3)
			{
				mesh.uv3 = new Vector2[0];
			}
			if (optionStripUV4)
			{
				mesh.uv4 = new Vector2[0];
			}
			if (optionStripUV5)
			{
				mesh.uv5 = new Vector2[0];
			}
			if (optionStripUV6)
			{
				mesh.uv6 = new Vector2[0];
			}
			if (optionStripUV7)
			{
				mesh.uv7 = new Vector2[0];
			}
			if (optionStripUV8)
			{
				mesh.uv8 = new Vector2[0];
			}
			mesh.triangles = m.triangles;
			if (optionStripColors)
			{
				mesh.colors32 = new Color32[0];
			}
			else
			{
				mesh.colors32 = m.colors32;
			}
			mesh.subMeshCount = m.subMeshCount;
			mesh.bindposes = m.bindposes;
			mesh.boneWeights = m.boneWeights;
			if (optionStripNormals)
			{
				mesh.normals = new Vector3[0];
			}
			else
			{
				mesh.normals = m.normals;
			}
			if (optionStripTangents)
			{
				mesh.tangents = new Vector4[0];
			}
			else
			{
				mesh.tangents = m.tangents;
			}
			if (optionRebuildNormals)
			{
				if (rebuildNormalsAngle < 0f)
				{
					mesh.RecalculateNormals();
				}
				else
				{
					mesh.RecalculateNormalsBasedOnAngleThreshold(rebuildNormalsAngle);
				}
			}
			if (optionRebuildTangents)
			{
				mesh = CreateTangents(mesh);
			}
			return mesh;
		}
		return m;
	}

	public static Mesh StripUnusedVertices(Mesh m, bool optimize)
	{
		return Strip(m, optimize, stripNormals: false, stripTangents: false, stripColors: false, stripUV: false, stripUV2: false, stripUV3: false, stripUV4: false, stripUV5: false, stripUV6: false, stripUV7: false, stripUV8: false, stripBoneWeights: false, stripBindPoses: false);
	}

	public static Mesh Strip(Mesh m, bool optimize, bool stripNormals, bool stripTangents, bool stripColors, bool stripUV, bool stripUV2, bool stripUV3, bool stripUV4, bool stripUV5, bool stripUV6, bool stripUV7, bool stripUV8, bool stripBoneWeights, bool stripBindPoses)
	{
		int[] triangles = m.triangles;
		Vector3[] vertices = m.vertices;
		Vector3[] normals = m.normals;
		Vector4[] tangents = m.tangents;
		Color32[] colors = m.colors32;
		BoneWeight[] boneWeights = m.boneWeights;
		Matrix4x4[] bindposes = m.bindposes;
		Vector2[] uv = m.uv;
		Vector2[] uv2 = m.uv2;
		Vector2[] uv3 = m.uv3;
		Vector2[] uv4 = m.uv4;
		Vector2[] uv5 = m.uv5;
		Vector2[] uv6 = m.uv6;
		Vector2[] uv7 = m.uv7;
		Vector2[] uv8 = m.uv8;
		if (m != null && triangles.Length != 0 && vertices.Length != 0)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			bool flag9 = false;
			bool flag10 = false;
			bool flag11 = false;
			bool flag12 = false;
			bool flag13 = false;
			Vector3[] array = new Vector3[0];
			Vector4[] array2 = new Vector4[0];
			Color32[] array3 = new Color32[0];
			BoneWeight[] array4 = new BoneWeight[0];
			Matrix4x4[] array5 = new Matrix4x4[0];
			Vector2[] array6 = new Vector2[0];
			Vector2[] array7 = new Vector2[0];
			Vector2[] array8 = new Vector2[0];
			Vector2[] array9 = new Vector2[0];
			Vector2[] array10 = new Vector2[0];
			Vector2[] array11 = new Vector2[0];
			Vector2[] array12 = new Vector2[0];
			Vector2[] array13 = new Vector2[0];
			if (!stripNormals && normals.Length != 0 && normals.Length == vertices.Length)
			{
				flag = true;
			}
			if (!stripTangents && tangents.Length != 0 && tangents.Length == vertices.Length)
			{
				flag2 = true;
			}
			if (!stripColors && m.colors32.Length != 0 && m.colors32.Length == m.colors32.Length)
			{
				flag3 = true;
			}
			if (!stripBoneWeights && m.boneWeights.Length != 0 && m.boneWeights.Length == vertices.Length)
			{
				flag6 = true;
			}
			if (!stripBindPoses && m.bindposes.Length != 0 && m.bindposes.Length == vertices.Length)
			{
				flag7 = true;
			}
			if (!stripUV && m.uv.Length != 0 && m.uv.Length == vertices.Length)
			{
				flag4 = true;
			}
			if (!stripUV2 && m.uv2.Length != 0 && m.uv2.Length == vertices.Length)
			{
				flag5 = true;
			}
			if (!stripUV3 && m.uv3.Length != 0 && m.uv3.Length == vertices.Length)
			{
				flag8 = true;
			}
			if (!stripUV4 && m.uv4.Length != 0 && m.uv4.Length == vertices.Length)
			{
				flag9 = true;
			}
			if (!stripUV5 && m.uv5.Length != 0 && m.uv5.Length == vertices.Length)
			{
				flag10 = true;
			}
			if (!stripUV6 && m.uv6.Length != 0 && m.uv6.Length == vertices.Length)
			{
				flag11 = true;
			}
			if (!stripUV7 && m.uv7.Length != 0 && m.uv7.Length == vertices.Length)
			{
				flag12 = true;
			}
			if (!stripUV8 && m.uv8.Length != 0 && m.uv8.Length == vertices.Length)
			{
				flag13 = true;
			}
			int num = 0;
			int[] array14 = new int[triangles.Length];
			foreach (int num2 in triangles)
			{
				if (num > 0)
				{
					bool flag14 = false;
					for (int j = 0; j < num; j++)
					{
						if (array14[j] == num2)
						{
							flag14 = true;
							break;
						}
					}
					if (!flag14)
					{
						array14[num] = num2;
						num++;
					}
				}
				else
				{
					array14[num] = num2;
					num++;
				}
			}
			int[] array15 = new int[num];
			for (int k = 0; k < num; k++)
			{
				array15[k] = array14[k];
			}
			if (flag)
			{
				array = new Vector3[num];
			}
			if (flag2)
			{
				array2 = new Vector4[num];
			}
			if (flag3)
			{
				array3 = new Color32[num];
			}
			if (flag6)
			{
				array4 = new BoneWeight[num];
			}
			if (flag7)
			{
				array5 = new Matrix4x4[num];
			}
			if (flag4)
			{
				array6 = new Vector2[num];
			}
			if (flag5)
			{
				array7 = new Vector2[num];
			}
			if (flag8)
			{
				array8 = new Vector2[num];
			}
			if (flag9)
			{
				array9 = new Vector2[num];
			}
			if (flag10)
			{
				array10 = new Vector2[num];
			}
			if (flag11)
			{
				array11 = new Vector2[num];
			}
			if (flag12)
			{
				array12 = new Vector2[num];
			}
			if (flag13)
			{
				array13 = new Vector2[num];
			}
			Vector3[] array16 = new Vector3[num];
			for (int l = 0; l < num; l++)
			{
				int num3 = array15[l];
				array16[l] = vertices[num3];
				if (flag)
				{
					array[l] = normals[num3];
				}
				if (flag2)
				{
					array2[l] = tangents[num3];
				}
				if (flag3)
				{
					array3[l] = colors[num3];
				}
				if (flag6)
				{
					array4[l] = boneWeights[num3];
				}
				if (flag7)
				{
					array5[l] = bindposes[num3];
				}
				if (flag4)
				{
					array6[l] = uv[num3];
				}
				if (flag5)
				{
					array7[l] = uv2[num3];
				}
				if (flag8)
				{
					array8[l] = uv3[num3];
				}
				if (flag9)
				{
					array9[l] = uv4[num3];
				}
				if (flag10)
				{
					array10[l] = uv5[num3];
				}
				if (flag11)
				{
					array11[l] = uv6[num3];
				}
				if (flag12)
				{
					array12[l] = uv7[num3];
				}
				if (flag13)
				{
					array13[l] = uv8[num3];
				}
			}
			int[] array17 = triangles;
			int num4 = 0;
			int[] array18 = triangles;
			foreach (int num5 in array18)
			{
				for (int num6 = 0; num6 < num; num6++)
				{
					if (num5 == array15[num6])
					{
						array17[num4] = num6;
					}
				}
				num4++;
			}
			m.Clear();
			m.vertices = array16;
			if (flag)
			{
				m.normals = array;
			}
			if (flag2)
			{
				m.tangents = array2;
			}
			if (flag3)
			{
				m.colors32 = array3;
			}
			if (flag4)
			{
				m.uv = array6;
			}
			if (flag5)
			{
				m.uv2 = array7;
			}
			if (flag8)
			{
				m.uv3 = array8;
			}
			if (flag9)
			{
				m.uv4 = array9;
			}
			if (flag10)
			{
				m.uv5 = array10;
			}
			if (flag11)
			{
				m.uv6 = array11;
			}
			if (flag12)
			{
				m.uv7 = array12;
			}
			if (flag13)
			{
				m.uv8 = array13;
			}
			if (flag6)
			{
				m.boneWeights = array4;
			}
			if (flag7)
			{
				m.bindposes = array5;
			}
			m.triangles = array17;
			m.RecalculateBounds();
		}
		return m;
	}

	public static Mesh[] SplitMesh(Mesh mesh, bool stripUnusedVertices)
	{
		if (mesh != null)
		{
			if (mesh.subMeshCount > 1)
			{
				if (debug)
				{
					Debug.Log("MESHKIT: " + mesh.name + " has " + mesh.subMeshCount + " submeshes.");
				}
				Mesh[] array = new Mesh[mesh.subMeshCount];
				for (int i = 0; i < mesh.subMeshCount; i++)
				{
					Mesh mesh2 = new Mesh();
					mesh2.Clear();
					mesh2.vertices = mesh.vertices;
					mesh2.triangles = mesh.GetTriangles(i);
					mesh2.bindposes = mesh.bindposes;
					mesh2.boneWeights = mesh.boneWeights;
					mesh2.uv = mesh.uv;
					mesh2.uv2 = mesh.uv2;
					mesh2.uv3 = mesh.uv3;
					mesh2.uv4 = mesh.uv4;
					mesh2.uv5 = mesh.uv5;
					mesh2.uv6 = mesh.uv6;
					mesh2.uv7 = mesh.uv7;
					mesh2.uv8 = mesh.uv8;
					mesh2.colors32 = mesh.colors32;
					mesh2.subMeshCount = 1;
					mesh2.normals = mesh.normals;
					mesh2.tangents = mesh.tangents;
					mesh2 = (array[i] = StripUnusedVertices(mesh2, optimize: true));
					array[i].name = mesh.name + " - MeshKit Separated [" + i + "]";
				}
				return array;
			}
			Debug.Log("MESHKIT: " + mesh.name + " hasn't got any submeshes. This mesh will be skipped.");
		}
		return null;
	}

	public static Mesh CreateTangents(Mesh mesh)
	{
		int[] triangles = mesh.triangles;
		Vector3[] vertices = mesh.vertices;
		Vector2[] uv = mesh.uv;
		Vector3[] normals = mesh.normals;
		if (uv.Length == 0)
		{
			Debug.LogWarning("MESHKIT: Tangents couldn't be created for a Mesh because it didn't have UVs! Skipping ...");
			return mesh;
		}
		int num = triangles.Length;
		int num2 = vertices.Length;
		Vector3[] array = new Vector3[num2];
		Vector3[] array2 = new Vector3[num2];
		Vector4[] array3 = new Vector4[num2];
		for (long num3 = 0L; num3 < num; num3 += 3)
		{
			long num4 = triangles[num3];
			long num5 = triangles[num3 + 1];
			long num6 = triangles[num3 + 2];
			Vector3 vector = vertices[num4];
			Vector3 vector2 = vertices[num5];
			Vector3 vector3 = vertices[num6];
			Vector2 vector4 = uv[num4];
			Vector2 vector5 = uv[num5];
			Vector2 vector6 = uv[num6];
			float num7 = vector2.x - vector.x;
			float num8 = vector3.x - vector.x;
			float num9 = vector2.y - vector.y;
			float num10 = vector3.y - vector.y;
			float num11 = vector2.z - vector.z;
			float num12 = vector3.z - vector.z;
			float num13 = vector5.x - vector4.x;
			float num14 = vector6.x - vector4.x;
			float num15 = vector5.y - vector4.y;
			float num16 = vector6.y - vector4.y;
			float num17 = num13 * num16 - num14 * num15;
			float num18 = ((num17 == 0f) ? 0f : (1f / num17));
			Vector3 vector7 = new Vector3((num16 * num7 - num15 * num8) * num18, (num16 * num9 - num15 * num10) * num18, (num16 * num11 - num15 * num12) * num18);
			Vector3 vector8 = new Vector3((num13 * num8 - num14 * num7) * num18, (num13 * num10 - num14 * num9) * num18, (num13 * num12 - num14 * num11) * num18);
			array[num4] += vector7;
			array[num5] += vector7;
			array[num6] += vector7;
			array2[num4] += vector8;
			array2[num5] += vector8;
			array2[num6] += vector8;
		}
		for (long num19 = 0L; num19 < num2; num19++)
		{
			Vector3 normal = normals[num19];
			Vector3 tangent = array[num19];
			Vector3.OrthoNormalize(ref normal, ref tangent);
			array3[num19].x = tangent.x;
			array3[num19].y = tangent.y;
			array3[num19].z = tangent.z;
			array3[num19].w = ((Vector3.Dot(Vector3.Cross(normal, tangent), array2[num19]) < 0f) ? (-1f) : 1f);
		}
		mesh.tangents = array3;
		return mesh;
	}

	public static Mesh InvertMesh(Mesh mesh)
	{
		if (mesh.subMeshCount == 1)
		{
			int[] triangles = mesh.triangles;
			int num = triangles.Length / 3;
			for (int i = 0; i < num; i++)
			{
				int num2 = triangles[i * 3];
				triangles[i * 3] = triangles[i * 3 + 1];
				triangles[i * 3 + 1] = num2;
			}
			mesh.triangles = triangles;
			Vector3[] normals = mesh.normals;
			for (int j = 0; j < normals.Length; j++)
			{
				normals[j] = -normals[j];
			}
			mesh.normals = normals;
		}
		return mesh;
	}

	public static Mesh MakeDoubleSidedMesh(Mesh mesh)
	{
		if (mesh.subMeshCount == 1)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			bool flag9 = false;
			bool flag10 = false;
			bool flag11 = false;
			bool flag12 = false;
			if (mesh.normals.Length != 0 && mesh.normals.Length == mesh.vertices.Length)
			{
				flag = true;
			}
			if (mesh.tangents.Length != 0 && mesh.tangents.Length == mesh.vertices.Length)
			{
				flag2 = true;
			}
			if (mesh.colors32.Length != 0 && mesh.colors32.Length == mesh.colors32.Length)
			{
				flag3 = true;
			}
			if (mesh.boneWeights.Length != 0 && mesh.boneWeights.Length == mesh.vertices.Length)
			{
				flag4 = true;
			}
			if (mesh.uv.Length != 0 && mesh.uv.Length == mesh.vertices.Length)
			{
				flag5 = true;
			}
			if (mesh.uv2.Length != 0 && mesh.uv2.Length == mesh.vertices.Length)
			{
				flag6 = true;
			}
			if (mesh.uv3.Length != 0 && mesh.uv3.Length == mesh.vertices.Length)
			{
				flag7 = true;
			}
			if (mesh.uv4.Length != 0 && mesh.uv4.Length == mesh.vertices.Length)
			{
				flag8 = true;
			}
			if (mesh.uv5.Length != 0 && mesh.uv5.Length == mesh.vertices.Length)
			{
				flag9 = true;
			}
			if (mesh.uv6.Length != 0 && mesh.uv6.Length == mesh.vertices.Length)
			{
				flag10 = true;
			}
			if (mesh.uv7.Length != 0 && mesh.uv7.Length == mesh.vertices.Length)
			{
				flag11 = true;
			}
			if (mesh.uv8.Length != 0 && mesh.uv8.Length == mesh.vertices.Length)
			{
				flag12 = true;
			}
			Vector3[] vertices = mesh.vertices;
			Vector3[] normals = mesh.normals;
			Color32[] colors = mesh.colors32;
			BoneWeight[] boneWeights = mesh.boneWeights;
			Matrix4x4[] bindposes = mesh.bindposes;
			Vector2[] uv = mesh.uv;
			Vector2[] uv2 = mesh.uv2;
			Vector2[] uv3 = mesh.uv3;
			Vector2[] uv4 = mesh.uv4;
			Vector2[] uv5 = mesh.uv5;
			Vector2[] uv6 = mesh.uv6;
			Vector2[] uv7 = mesh.uv7;
			Vector2[] uv8 = mesh.uv8;
			int num = vertices.Length;
			Vector3[] array = new Vector3[num * 2];
			Vector3[] array2 = new Vector3[0];
			Vector4[] tangents = new Vector4[0];
			Color32[] array3 = new Color32[0];
			BoneWeight[] array4 = new BoneWeight[0];
			Vector2[] array5 = new Vector2[0];
			Vector2[] array6 = new Vector2[0];
			Vector2[] array7 = new Vector2[0];
			Vector2[] array8 = new Vector2[0];
			Vector2[] array9 = new Vector2[0];
			Vector2[] array10 = new Vector2[0];
			Vector2[] array11 = new Vector2[0];
			Vector2[] array12 = new Vector2[0];
			if (flag)
			{
				array2 = new Vector3[num * 2];
			}
			if (flag3)
			{
				array3 = new Color32[num * 2];
			}
			if (flag4)
			{
				array4 = new BoneWeight[num * 2];
			}
			if (flag5)
			{
				array5 = new Vector2[num * 2];
			}
			if (flag6)
			{
				array6 = new Vector2[num * 2];
			}
			if (flag7)
			{
				array7 = new Vector2[num * 2];
			}
			if (flag8)
			{
				array8 = new Vector2[num * 2];
			}
			if (flag9)
			{
				array9 = new Vector2[num * 2];
			}
			if (flag10)
			{
				array10 = new Vector2[num * 2];
			}
			if (flag11)
			{
				array11 = new Vector2[num * 2];
			}
			if (flag12)
			{
				array12 = new Vector2[num * 2];
			}
			int[] triangles = mesh.triangles;
			int num2 = triangles.Length;
			int[] array13 = new int[num2 * 2];
			for (int i = 0; i < num; i++)
			{
				array[i] = (array[i + num] = vertices[i]);
				if (flag5)
				{
					array5[i] = (array5[i + num] = uv[i]);
				}
				if (flag6)
				{
					array6[i] = (array6[i + num] = uv2[i]);
				}
				if (flag7)
				{
					array7[i] = (array7[i + num] = uv3[i]);
				}
				if (flag8)
				{
					array8[i] = (array8[i + num] = uv4[i]);
				}
				if (flag9)
				{
					array9[i] = (array9[i + num] = uv5[i]);
				}
				if (flag10)
				{
					array10[i] = (array10[i + num] = uv6[i]);
				}
				if (flag11)
				{
					array11[i] = (array11[i + num] = uv7[i]);
				}
				if (flag12)
				{
					array12[i] = (array12[i + num] = uv8[i]);
				}
				if (flag)
				{
					array2[i] = normals[i];
				}
				if (flag3)
				{
					array3[i] = colors[i];
				}
				if (flag4)
				{
					array4[i] = boneWeights[i];
				}
				if (flag)
				{
					array2[i + num] = -normals[i];
				}
				if (flag3)
				{
					array3[i + num] = colors[i];
				}
				if (flag4)
				{
					array4[i + num] = boneWeights[i];
				}
			}
			for (int j = 0; j < num2; j += 3)
			{
				array13[j] = triangles[j];
				array13[j + 1] = triangles[j + 1];
				array13[j + 2] = triangles[j + 2];
				int num3 = j + num2;
				array13[num3] = triangles[j] + num;
				array13[num3 + 2] = triangles[j + 1] + num;
				array13[num3 + 1] = triangles[j + 2] + num;
			}
			mesh.vertices = array;
			mesh.uv = array5;
			mesh.uv2 = array6;
			mesh.uv3 = array7;
			mesh.uv4 = array8;
			mesh.uv5 = array9;
			mesh.uv6 = array10;
			mesh.uv7 = array11;
			mesh.uv8 = array12;
			mesh.colors32 = array3;
			mesh.normals = array2;
			mesh.tangents = tangents;
			mesh.boneWeights = array4;
			mesh.bindposes = bindposes;
			mesh.triangles = array13;
			if (flag2)
			{
				mesh = CreateTangents(mesh);
			}
		}
		return mesh;
	}

	public static void InvertMesh(GameObject go, bool recursive, bool optionUseMeshFilters, bool optionUseSkinnedMeshRenderers, bool enabledRenderersOnly)
	{
		com.StartCoroutine(com.InvertMeshAtRuntime(go, recursive, optionUseMeshFilters, optionUseSkinnedMeshRenderers, enabledRenderersOnly));
	}

	public IEnumerator InvertMeshAtRuntime(GameObject go, bool recursive, bool optionUseMeshFilters, bool optionUseSkinnedMeshRenderers, bool enabledRenderersOnly)
	{
		if (!recursive)
		{
			bool flag = false;
			if (optionUseMeshFilters && go.GetComponent<MeshFilter>() != null && go.GetComponent<MeshFilter>().sharedMesh != null && (!enabledRenderersOnly || (enabledRenderersOnly && go.GetComponent<Renderer>() != null && go.GetComponent<Renderer>().enabled)))
			{
				InvertMesh(go.GetComponent<MeshFilter>().mesh);
				flag = true;
			}
			if (optionUseSkinnedMeshRenderers && go.GetComponent<SkinnedMeshRenderer>() != null && go.GetComponent<SkinnedMeshRenderer>().sharedMesh != null && (!enabledRenderersOnly || (enabledRenderersOnly && go.GetComponent<SkinnedMeshRenderer>() != null && go.GetComponent<SkinnedMeshRenderer>().enabled)))
			{
				SkinnedMeshRenderer component = go.GetComponent<SkinnedMeshRenderer>();
				Mesh mesh2 = (component.sharedMesh = UnityEngine.Object.Instantiate(component.sharedMesh));
				InvertMesh(mesh2);
				flag = true;
			}
			if (!flag)
			{
				Debug.Log("MESHKIT: The GameObject " + go.name + " does not have a Mesh.");
			}
			yield break;
		}
		MeshFilter[] array = new MeshFilter[0];
		if (optionUseMeshFilters)
		{
			array = go.GetComponentsInChildren<MeshFilter>();
		}
		if (array.Length != 0)
		{
			MeshFilter[] array2 = array;
			foreach (MeshFilter meshFilter in array2)
			{
				if (meshFilter != null && meshFilter.sharedMesh != null && (!enabledRenderersOnly || (enabledRenderersOnly && meshFilter.gameObject.GetComponent<Renderer>() != null && meshFilter.gameObject.GetComponent<Renderer>().enabled)))
				{
					InvertMesh(meshFilter.mesh);
					yield return 0;
				}
			}
		}
		SkinnedMeshRenderer[] array3 = new SkinnedMeshRenderer[0];
		if (optionUseSkinnedMeshRenderers)
		{
			array3 = go.GetComponentsInChildren<SkinnedMeshRenderer>();
		}
		if (array3.Length == 0)
		{
			yield break;
		}
		SkinnedMeshRenderer[] array4 = array3;
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in array4)
		{
			if (skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh != null && (!enabledRenderersOnly || (enabledRenderersOnly && skinnedMeshRenderer.gameObject.GetComponent<SkinnedMeshRenderer>() != null && skinnedMeshRenderer.gameObject.GetComponent<SkinnedMeshRenderer>().enabled)))
			{
				Mesh mesh4 = (skinnedMeshRenderer.sharedMesh = UnityEngine.Object.Instantiate(skinnedMeshRenderer.sharedMesh));
				InvertMesh(mesh4);
				yield return 0;
			}
		}
	}

	public static void MakeDoubleSided(GameObject go, bool recursive, bool optionUseMeshFilters, bool optionUseSkinnedMeshRenderers, bool enabledRenderersOnly)
	{
		com.StartCoroutine(com.MakeDoubleSidedAtRuntime(go, recursive, optionUseMeshFilters, optionUseSkinnedMeshRenderers, enabledRenderersOnly));
	}

	public IEnumerator MakeDoubleSidedAtRuntime(GameObject go, bool recursive, bool optionUseMeshFilters, bool optionUseSkinnedMeshRenderers, bool enabledRenderersOnly)
	{
		if (!recursive)
		{
			bool flag = false;
			if (optionUseMeshFilters && go.GetComponent<MeshFilter>() != null && go.GetComponent<MeshFilter>().sharedMesh != null && (!enabledRenderersOnly || (enabledRenderersOnly && go.GetComponent<Renderer>() != null && go.GetComponent<Renderer>().enabled)))
			{
				MakeDoubleSidedMesh(go.GetComponent<MeshFilter>().mesh);
				flag = true;
			}
			if (optionUseSkinnedMeshRenderers && go.GetComponent<SkinnedMeshRenderer>() != null && go.GetComponent<SkinnedMeshRenderer>().sharedMesh != null && (!enabledRenderersOnly || (enabledRenderersOnly && go.GetComponent<SkinnedMeshRenderer>() != null && go.GetComponent<SkinnedMeshRenderer>().enabled)))
			{
				SkinnedMeshRenderer component = go.GetComponent<SkinnedMeshRenderer>();
				Mesh mesh2 = (component.sharedMesh = UnityEngine.Object.Instantiate(component.sharedMesh));
				MakeDoubleSidedMesh(mesh2);
				flag = true;
			}
			if (!flag)
			{
				Debug.Log("MESHKIT: The GameObject " + go.name + " does not have a Mesh.");
			}
			yield break;
		}
		MeshFilter[] array = new MeshFilter[0];
		if (optionUseMeshFilters)
		{
			array = go.GetComponentsInChildren<MeshFilter>();
		}
		if (array.Length != 0)
		{
			MeshFilter[] array2 = array;
			foreach (MeshFilter meshFilter in array2)
			{
				if (meshFilter != null && meshFilter.sharedMesh != null && (!enabledRenderersOnly || (enabledRenderersOnly && meshFilter.gameObject.GetComponent<Renderer>() != null && meshFilter.gameObject.GetComponent<Renderer>().enabled)))
				{
					MakeDoubleSidedMesh(meshFilter.mesh);
					yield return 0;
				}
			}
		}
		SkinnedMeshRenderer[] array3 = new SkinnedMeshRenderer[0];
		if (optionUseSkinnedMeshRenderers)
		{
			array3 = go.GetComponentsInChildren<SkinnedMeshRenderer>();
		}
		if (array3.Length == 0)
		{
			yield break;
		}
		SkinnedMeshRenderer[] array4 = array3;
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in array4)
		{
			if (skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh != null && (!enabledRenderersOnly || (enabledRenderersOnly && skinnedMeshRenderer.gameObject.GetComponent<SkinnedMeshRenderer>() != null && skinnedMeshRenderer.gameObject.GetComponent<SkinnedMeshRenderer>().enabled)))
			{
				Mesh mesh4 = (skinnedMeshRenderer.sharedMesh = UnityEngine.Object.Instantiate(skinnedMeshRenderer.sharedMesh));
				MakeDoubleSidedMesh(mesh4);
				yield return 0;
			}
		}
	}

	public static void CombineChildren(GameObject go, bool optimizeMeshes, int createNewObjectsWithLayer, string createNewObjectsWithTag, bool enabledRenderersOnly, bool createNewObjectsWithMeshColliders, bool deleteSourceObjects, bool deleteObjectsWithDisabledRenderers, bool deleteEmptyObjects, int userMaxVertices = 65534)
	{
		com.StartCoroutine(com.CombineChildrenAtRuntime(go, optimizeMeshes, createNewObjectsWithLayer, createNewObjectsWithTag, enabledRenderersOnly, createNewObjectsWithMeshColliders, deleteSourceObjects, deleteObjectsWithDisabledRenderers, deleteEmptyObjects, userMaxVertices));
	}

	public IEnumerator CombineChildrenAtRuntime(GameObject go, bool optimizeMeshes, int createNewObjectsWithLayer, string createNewObjectsWithTag, bool enabledRenderersOnly, bool createNewObjectsWithMeshColliders, bool deleteSourceObjects, bool deleteObjectsWithDisabledRenderers, bool deleteEmptyObjects, int userMaxVertices = 65534)
	{
		if (userMaxVertices > 65534)
		{
			Debug.Log("MESHKIT: Maximum vertices cannot be higher than " + 65534 + ". Your settings have been changed.");
			userMaxVertices = 65534;
		}
		Matrix4x4 worldToLocalMatrix = go.transform.worldToLocalMatrix;
		Dictionary<Material, List<CombineInstance>> dictionary = new Dictionary<Material, List<CombineInstance>>();
		MeshRenderer[] componentsInChildren = go.GetComponentsInChildren<MeshRenderer>(includeInactive: true);
		ArrayList arrayList = new ArrayList();
		arrayList.Clear();
		MeshRenderer[] array = componentsInChildren;
		foreach (MeshRenderer meshRenderer in array)
		{
			if (!(meshRenderer.gameObject.GetComponent<MeshFilter>() != null) || !(meshRenderer.gameObject.GetComponent<MeshFilter>().sharedMesh != null) || meshRenderer.gameObject.GetComponent<MeshFilter>().sharedMesh.subMeshCount != 1 || (enabledRenderersOnly && !meshRenderer.enabled))
			{
				continue;
			}
			Material[] sharedMaterials = meshRenderer.sharedMaterials;
			foreach (Material material in sharedMaterials)
			{
				if (material != null && !dictionary.ContainsKey(material))
				{
					dictionary.Add(material, new List<CombineInstance>());
				}
			}
		}
		int num = 0;
		MeshFilter[] componentsInChildren2 = go.GetComponentsInChildren<MeshFilter>(includeInactive: true);
		MeshFilter[] _arr = new MeshFilter[0];
		MeshFilter[] array2 = componentsInChildren2;
		foreach (MeshFilter meshFilter in array2)
		{
			if (!(meshFilter.sharedMesh == null) && (!(meshFilter.gameObject.GetComponent<MeshRenderer>() != null && enabledRenderersOnly) || meshFilter.gameObject.GetComponent<MeshRenderer>().enabled) && meshFilter.sharedMesh.vertexCount < userMaxVertices && meshFilter.sharedMesh.subMeshCount <= 1)
			{
				Arrays.AddItemFastest(ref _arr, meshFilter);
				CombineInstance item = default(CombineInstance);
				item.mesh = meshFilter.sharedMesh;
				item.transform = worldToLocalMatrix * meshFilter.transform.localToWorldMatrix;
				dictionary[meshFilter.GetComponent<Renderer>().sharedMaterial].Add(item);
				arrayList.Add(meshFilter.GetComponent<Renderer>());
				meshFilter.GetComponent<Renderer>().enabled = false;
				num++;
			}
		}
		if (debug)
		{
			Debug.Log("MESHKIT: Mesh Combinations: " + num);
		}
		if (debug)
		{
			Debug.Log("MESHKIT: Material Count: " + dictionary.Keys.Count);
		}
		if (dictionary.Keys.Count < num)
		{
			foreach (Material key in dictionary.Keys)
			{
				float num2 = 0f;
				CombineInstance[] array3 = dictionary[key].ToArray();
				foreach (CombineInstance combineInstance in array3)
				{
					num2 += (float)combineInstance.mesh.vertexCount;
				}
				if (num2 < (float)userMaxVertices)
				{
					GameObject gameObject = new GameObject("Combined " + key.name + "  [" + key.shader.name + "]");
					gameObject.transform.parent = go.transform;
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localRotation = Quaternion.identity;
					gameObject.transform.localScale = Vector3.one;
					gameObject.AddComponent<MeshFilter>().mesh.CombineMeshes(dictionary[key].ToArray(), mergeSubMeshes: true, useMatrices: true);
					gameObject.AddComponent<MeshRenderer>().material = key;
					if (createNewObjectsWithLayer >= 0)
					{
						gameObject.layer = createNewObjectsWithLayer;
					}
					if (createNewObjectsWithTag != "")
					{
						gameObject.tag = createNewObjectsWithTag;
					}
					if (createNewObjectsWithMeshColliders)
					{
						gameObject.AddComponent<MeshCollider>();
					}
					continue;
				}
				int num3 = 0;
				int num4 = 0;
				float num5 = 0f;
				ArrayList arrayList2 = new ArrayList();
				arrayList2.Clear();
				array3 = dictionary[key].ToArray();
				for (int i = 0; i < array3.Length; i++)
				{
					CombineInstance combineInstance2 = array3[i];
					if (num5 + (float)combineInstance2.mesh.vertexCount >= (float)userMaxVertices && arrayList2.Count > 0)
					{
						GameObject gameObject2 = new GameObject("Combined_" + key.name + "_" + num4 + "  [" + key.shader.name + "]");
						gameObject2.transform.parent = go.transform;
						gameObject2.transform.localPosition = Vector3.zero;
						gameObject2.transform.localRotation = Quaternion.identity;
						gameObject2.transform.localScale = Vector3.one;
						MeshFilter meshFilter2 = gameObject2.AddComponent<MeshFilter>();
						CombineInstance[] combine = (CombineInstance[])arrayList2.ToArray(typeof(CombineInstance));
						meshFilter2.mesh.CombineMeshes(combine, mergeSubMeshes: true, useMatrices: true);
						gameObject2.AddComponent<MeshRenderer>().material = key;
						if (createNewObjectsWithLayer >= 0)
						{
							gameObject2.layer = createNewObjectsWithLayer;
						}
						if (createNewObjectsWithTag != string.Empty)
						{
							gameObject2.tag = createNewObjectsWithTag;
						}
						if (createNewObjectsWithMeshColliders)
						{
							gameObject2.AddComponent<MeshCollider>();
						}
						num5 = 0f;
						arrayList2 = new ArrayList();
						arrayList2.Clear();
						num4++;
					}
					if (num5 + (float)combineInstance2.mesh.vertexCount < (float)userMaxVertices)
					{
						arrayList2.Add(combineInstance2);
						num5 += (float)combineInstance2.mesh.vertexCount;
						if (num3 == dictionary[key].Count - 1)
						{
							GameObject gameObject3 = new GameObject("Combined_" + key.name + "_" + num4 + "  [" + key.shader.name + "]");
							gameObject3.transform.parent = go.transform;
							gameObject3.transform.localPosition = Vector3.zero;
							gameObject3.transform.localRotation = Quaternion.identity;
							gameObject3.transform.localScale = Vector3.one;
							MeshFilter meshFilter3 = gameObject3.AddComponent<MeshFilter>();
							CombineInstance[] combine2 = (CombineInstance[])arrayList2.ToArray(typeof(CombineInstance));
							meshFilter3.mesh.CombineMeshes(combine2, mergeSubMeshes: true, useMatrices: true);
							gameObject3.AddComponent<MeshRenderer>().material = key;
							if (createNewObjectsWithLayer >= 0)
							{
								gameObject3.layer = createNewObjectsWithLayer;
							}
							if (createNewObjectsWithTag != string.Empty)
							{
								gameObject3.tag = createNewObjectsWithTag;
							}
							if (createNewObjectsWithMeshColliders)
							{
								gameObject3.AddComponent<MeshCollider>();
							}
						}
					}
					else if (combineInstance2.mesh.vertexCount >= 65534)
					{
						Debug.Log("MESHKIT: MeshKit detected a Mesh called \"" + combineInstance2.mesh.name + "\" with " + combineInstance2.mesh.vertexCount + " vertices using the material \"" + key.name + "\". This is beyond Unity's limitations and cannot be combined. This mesh was skipped.");
					}
					num3++;
				}
			}
			if (deleteSourceObjects)
			{
				array = componentsInChildren;
				foreach (MeshRenderer meshRenderer2 in array)
				{
					if ((bool)meshRenderer2.gameObject.GetComponent<MeshFilter>() && meshRenderer2.gameObject.GetComponent<MeshFilter>().sharedMesh != null && meshRenderer2.gameObject.GetComponent<MeshFilter>().sharedMesh.subMeshCount > 1)
					{
						continue;
					}
					if (meshRenderer2.gameObject.GetComponent<Collider>() == null)
					{
						UnityEngine.Object.Destroy(meshRenderer2.gameObject);
						continue;
					}
					if (meshRenderer2.gameObject.GetComponent<MeshRenderer>() != null)
					{
						UnityEngine.Object.Destroy(meshRenderer2.gameObject.GetComponent<MeshRenderer>());
					}
					if (meshRenderer2.gameObject.GetComponent<MeshFilter>() != null)
					{
						UnityEngine.Object.Destroy(meshRenderer2.gameObject.GetComponent<MeshFilter>());
					}
				}
			}
			if (deleteObjectsWithDisabledRenderers)
			{
				Renderer[] componentsInChildren3 = go.GetComponentsInChildren<Renderer>(includeInactive: true);
				foreach (Renderer renderer in componentsInChildren3)
				{
					if (renderer != null && !renderer.enabled && renderer.gameObject != null)
					{
						UnityEngine.Object.Destroy(renderer.gameObject);
					}
				}
			}
			if (!deleteEmptyObjects)
			{
				yield break;
			}
			bool foundEmptyObjects = false;
			while (!foundEmptyObjects)
			{
				foundEmptyObjects = false;
				Transform[] componentsInChildren4 = go.GetComponentsInChildren<Transform>(includeInactive: true);
				foreach (Transform transform in componentsInChildren4)
				{
					if (transform.gameObject.GetComponents<Component>().Length == 1 && transform.childCount == 0)
					{
						UnityEngine.Object.Destroy(transform.gameObject);
						foundEmptyObjects = true;
					}
				}
				yield return 0;
			}
			yield break;
		}
		if (debug)
		{
			Debug.Log("MESHKIT: No meshes require combining in this group ...");
		}
		foreach (MeshRenderer item2 in arrayList)
		{
			if (item2 != null)
			{
				item2.enabled = true;
			}
		}
	}

	public static bool ListContains(ArrayList list, Material[] key, Mesh originalMesh, MeshFilter mf)
	{
		if (list != null && list.Count > 0)
		{
			foreach (BatchMeshes item in list)
			{
				if (item.key.Length != key.Length || !(item.originalMesh == originalMesh))
				{
					continue;
				}
				bool flag = false;
				for (int i = 0; i < item.key.Length; i++)
				{
					if (item.key[i] != key[i])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					item.gos.Add(mf.gameObject);
					return true;
				}
			}
		}
		return false;
	}

	public static void SeparateMeshes(GameObject go, bool onlyApplyToEnabledRenderers, bool stripUnusedVertices)
	{
		ArrayList arrayList = new ArrayList();
		arrayList.Clear();
		MeshFilter[] componentsInChildren = go.GetComponentsInChildren<MeshFilter>();
		if (debug)
		{
			Debug.Log(componentsInChildren.Length + " MeshFilters found for processing ...");
		}
		int num = 0;
		MeshFilter[] array = componentsInChildren;
		foreach (MeshFilter meshFilter in array)
		{
			if (meshFilter != null && meshFilter.sharedMesh != null && meshFilter.sharedMesh.subMeshCount > 1 && meshFilter.gameObject.GetComponent<MeshRenderer>() != null && meshFilter.gameObject.GetComponent<MeshRenderer>().sharedMaterials.Length > 1 && (!onlyApplyToEnabledRenderers || (onlyApplyToEnabledRenderers && meshFilter.gameObject.GetComponent<MeshRenderer>().enabled)) && !ListContains(arrayList, meshFilter.gameObject.GetComponent<MeshRenderer>().sharedMaterials, meshFilter.sharedMesh, meshFilter))
			{
				BatchMeshes batchMeshes = new BatchMeshes();
				batchMeshes.key = meshFilter.gameObject.GetComponent<MeshRenderer>().sharedMaterials;
				batchMeshes.originalMesh = meshFilter.sharedMesh;
				batchMeshes.gos = new ArrayList();
				batchMeshes.gos.Clear();
				batchMeshes.gos.Add(meshFilter.gameObject);
				arrayList.Add(batchMeshes);
				num++;
			}
		}
		if (debug)
		{
			Debug.Log("numberOfWorkableMFs: " + num);
		}
		if (num == 0)
		{
			Debug.Log("MESHKIT: No objects require seperating into independant meshes.");
		}
		if (arrayList == null || arrayList.Count <= 0)
		{
			return;
		}
		int num2 = 0;
		foreach (BatchMeshes item in arrayList)
		{
			if (item == null)
			{
				continue;
			}
			GameObject gameObject = item.gos[0] as GameObject;
			if (gameObject != null && gameObject.GetComponent<MeshFilter>() != null)
			{
				MeshFilter component = gameObject.GetComponent<MeshFilter>();
				item.splitMeshes = SplitMesh(component.sharedMesh, stripUnusedVertices);
				if (item.splitMeshes.Length != 0)
				{
					RebuildSeparatedObjects(item);
				}
				num2++;
			}
		}
	}

	public static void RebuildSeparatedObjects(BatchMeshes bm)
	{
		if (bm == null || bm.key.Length == 0 || bm.splitMeshes.Length == 0 || bm.gos.Count <= 0)
		{
			return;
		}
		GameObject[] array = (GameObject[])bm.gos.ToArray(typeof(GameObject));
		foreach (GameObject gameObject in array)
		{
			for (int j = 0; j < bm.splitMeshes.Length; j++)
			{
				if (bm.splitMeshes[j] != null)
				{
					GameObject gameObject2 = new GameObject(bm.splitMeshes[j].name);
					gameObject2.tag = gameObject.tag;
					gameObject2.layer = gameObject.layer;
					gameObject2.transform.position = gameObject.transform.position;
					gameObject2.transform.rotation = gameObject.transform.rotation;
					gameObject2.transform.parent = gameObject.transform.parent;
					gameObject2.transform.localScale = gameObject.transform.localScale;
					gameObject2.transform.parent = gameObject.transform;
					gameObject2.AddComponent<MeshFilter>().sharedMesh = bm.splitMeshes[j];
					MeshRenderer meshRenderer = gameObject2.AddComponent<MeshRenderer>();
					meshRenderer.receiveShadows = gameObject.GetComponent<MeshRenderer>().receiveShadows;
					meshRenderer.probeAnchor = gameObject.GetComponent<MeshRenderer>().probeAnchor;
					meshRenderer.reflectionProbeUsage = gameObject.GetComponent<MeshRenderer>().reflectionProbeUsage;
					meshRenderer.shadowCastingMode = gameObject.GetComponent<MeshRenderer>().shadowCastingMode;
					if (j < bm.key.Length)
					{
						meshRenderer.sharedMaterial = bm.key[j];
					}
					else if (j > 0 && bm.key[j - 1] != null)
					{
						meshRenderer.sharedMaterial = bm.key[j - 1];
						Debug.LogWarning("MESHKIT: The MeshRenderer for \"" + gameObject2.name + "\" has been setup with less materials than submeshes and now has missing textures. MeshKit has tried to help by using the material of the previous submesh. To fix this you should increase the number of materials in the original MeshRenderer to match the number of submeshes.\n", gameObject2);
					}
					else
					{
						meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
						Debug.LogWarning("MESHKIT: The MeshRenderer for \"" + gameObject2.name + "\" has been setup with less materials than submeshes and now has missing textures - to fix this you should increase the number of materials in the original MeshRenderer to match the number of submeshes.\n", gameObject2);
					}
					meshRenderer.lightProbeUsage = gameObject.GetComponent<MeshRenderer>().lightProbeUsage;
					gameObject.GetComponent<MeshRenderer>().enabled = false;
				}
			}
		}
	}

	public static void Rebuild(GameObject go, bool recursive, bool optionUseMeshFilters, bool optionUseSkinnedMeshRenderers, bool optionStripNormals, bool optionStripTangents, bool optionStripColors, bool optionStripUV2, bool optionStripUV3, bool optionStripUV4, bool optionStripUV5, bool optionStripUV6, bool optionStripUV7, bool optionStripUV8, bool optionRebuildNormals, bool optionRebuildTangents, float rebuildNormalsAngle = -1f)
	{
		if (optionStripNormals)
		{
			optionStripTangents = true;
		}
		if (optionRebuildTangents)
		{
			optionRebuildNormals = true;
		}
		MeshFilter[] array = new MeshFilter[0];
		if (optionUseMeshFilters)
		{
			array = ((!recursive) ? go.GetComponents<MeshFilter>() : go.GetComponentsInChildren<MeshFilter>());
		}
		MeshFilter[] array2 = array;
		foreach (MeshFilter meshFilter in array2)
		{
			if (meshFilter != null && meshFilter.sharedMesh != null && meshFilter.sharedMesh.subMeshCount == 1)
			{
				Mesh mesh = RebuildMesh(meshFilter.sharedMesh, optionStripNormals, optionStripTangents, optionStripColors, optionStripUV2, optionStripUV3, optionStripUV4, optionStripUV5, optionStripUV6, optionStripUV7, optionStripUV8, optionRebuildNormals, optionRebuildTangents, rebuildNormalsAngle);
				if (mesh != null)
				{
					meshFilter.sharedMesh = mesh;
				}
				else
				{
					Debug.Log("MESHKIT: Couldn't rebuild MeshFilter's Mesh on GameObject: " + meshFilter.gameObject.name);
				}
			}
		}
		SkinnedMeshRenderer[] array3 = new SkinnedMeshRenderer[0];
		if (optionUseSkinnedMeshRenderers)
		{
			array3 = ((!recursive) ? go.GetComponents<SkinnedMeshRenderer>() : go.GetComponentsInChildren<SkinnedMeshRenderer>());
		}
		SkinnedMeshRenderer[] array4 = array3;
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in array4)
		{
			if (skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh != null && skinnedMeshRenderer.sharedMesh.subMeshCount == 1)
			{
				Mesh mesh2 = RebuildMesh(skinnedMeshRenderer.sharedMesh, optionStripNormals, optionStripTangents, optionStripColors, optionStripUV2, optionStripUV3, optionStripUV4, optionStripUV5, optionStripUV6, optionStripUV7, optionStripUV8, optionRebuildNormals, optionRebuildTangents, rebuildNormalsAngle);
				if (mesh2 != null)
				{
					skinnedMeshRenderer.sharedMesh = mesh2;
				}
				else
				{
					Debug.Log("MESHKIT: Couldn't rebuild SkinnedMeshRenderer's Mesh on GameObject: " + skinnedMeshRenderer.gameObject.name);
				}
			}
		}
	}

	public static Mesh DecimateMesh(SkinnedMeshRenderer smr, float quality, bool recalculateNormals, bool preserveBorders = false, bool preserveSeams = false, bool preserveFoldovers = false)
	{
		if (smr != null && smr.sharedMesh != null)
		{
			Transform transform = smr.transform;
			Mesh sharedMesh = smr.sharedMesh;
			Matrix4x4 matrix4x = smr.transform.worldToLocalMatrix * transform.localToWorldMatrix;
			quality = Mathf.Clamp01(quality);
			Mesh mesh = MeshDecimatorUtility.DecimateMesh(sharedMesh, matrix4x, quality, recalculateNormals, null, preserveBorders, preserveSeams, preserveFoldovers);
			if (mesh != null)
			{
				smr.sharedMesh = mesh;
				return mesh;
			}
			Debug.LogWarning("MESHKIT: A skinned mesh couldn't be decimated. Skipping ...");
			return null;
		}
		Debug.LogWarning("MESHKIT: SkinnedMeshRenderer or its shared mesh was null. Skipping ...");
		return null;
	}

	public static Mesh DecimateMesh(MeshFilter mf, float quality, bool recalculateNormals, bool preserveBorders = false, bool preserveSeams = false, bool preserveFoldovers = false)
	{
		if (mf != null && mf.sharedMesh != null)
		{
			Transform transform = mf.transform;
			Mesh sharedMesh = mf.sharedMesh;
			Matrix4x4 matrix4x = mf.transform.worldToLocalMatrix * transform.localToWorldMatrix;
			quality = Mathf.Clamp01(quality);
			Mesh mesh = MeshDecimatorUtility.DecimateMesh(sharedMesh, matrix4x, quality, recalculateNormals, null, preserveBorders, preserveSeams, preserveFoldovers);
			if (mesh != null)
			{
				mf.sharedMesh = mesh;
				return mesh;
			}
			Debug.LogWarning("MESH KIT DECIMATOR: A mesh couldn't be decimated. Skipping ...");
			return null;
		}
		Debug.LogWarning("MESH KIT DECIMATOR: MeshFilter or its shared mesh was null. Skipping ...");
		return null;
	}

	public static void DecimateMesh(GameObject go, bool recursive, bool optionUseMeshFilters, bool optionUseSkinnedMeshRenderers, bool enabledRenderersOnly, float quality, bool recalculateNormals, bool preserveBorders = false, bool preserveSeams = false, bool preserveFoldovers = false)
	{
		com.StartCoroutine(com.DecimateMeshAtRuntime(go, recursive, optionUseMeshFilters, optionUseSkinnedMeshRenderers, enabledRenderersOnly, quality, recalculateNormals, preserveBorders, preserveSeams, preserveFoldovers));
	}

	public IEnumerator DecimateMeshAtRuntime(GameObject go, bool recursive, bool optionUseMeshFilters, bool optionUseSkinnedMeshRenderers, bool enabledRenderersOnly, float quality, bool recalculateNormals, bool preserveBorders = false, bool preserveSeams = false, bool preserveFoldovers = false)
	{
		if (!recursive)
		{
			if (((optionUseMeshFilters && go.GetComponent<MeshFilter>() != null && go.GetComponent<MeshFilter>().sharedMesh != null) || (optionUseSkinnedMeshRenderers && go.GetComponent<SkinnedMeshRenderer>() != null && (bool)go.GetComponent<SkinnedMeshRenderer>().sharedMesh)) && (!enabledRenderersOnly || (enabledRenderersOnly && go.GetComponent<Renderer>() != null && go.GetComponent<Renderer>().enabled)))
			{
				if (optionUseSkinnedMeshRenderers && go.GetComponent<SkinnedMeshRenderer>() != null && (!enabledRenderersOnly || (enabledRenderersOnly && go.GetComponent<SkinnedMeshRenderer>().enabled)))
				{
					DecimateMesh(go.GetComponent<SkinnedMeshRenderer>(), quality, recalculateNormals, preserveBorders, preserveSeams, preserveFoldovers);
				}
				else if (optionUseMeshFilters && go.GetComponent<MeshFilter>() != null && (!enabledRenderersOnly || (enabledRenderersOnly && go.GetComponent<Renderer>().enabled)))
				{
					DecimateMesh(go.GetComponent<MeshFilter>(), quality, recalculateNormals, preserveBorders, preserveSeams, preserveFoldovers);
				}
			}
			else
			{
				Debug.Log("MESHKIT: The GameObject " + go.name + " does not have a Mesh.");
			}
			yield break;
		}
		MeshFilter[] array = new MeshFilter[0];
		if (optionUseMeshFilters)
		{
			array = go.GetComponentsInChildren<MeshFilter>();
		}
		if (array.Length != 0)
		{
			MeshFilter[] array2 = array;
			foreach (MeshFilter meshFilter in array2)
			{
				if (meshFilter != null && meshFilter.sharedMesh != null && (!enabledRenderersOnly || (enabledRenderersOnly && meshFilter.gameObject.GetComponent<Renderer>() != null && meshFilter.gameObject.GetComponent<Renderer>().enabled)))
				{
					DecimateMesh(meshFilter, quality, recalculateNormals, preserveBorders, preserveSeams, preserveFoldovers);
					yield return 0;
				}
			}
		}
		SkinnedMeshRenderer[] array3 = new SkinnedMeshRenderer[0];
		if (optionUseSkinnedMeshRenderers)
		{
			array3 = go.GetComponentsInChildren<SkinnedMeshRenderer>();
		}
		if (array3.Length == 0)
		{
			yield break;
		}
		SkinnedMeshRenderer[] array4 = array3;
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in array4)
		{
			if (skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh != null && (!enabledRenderersOnly || (enabledRenderersOnly && skinnedMeshRenderer.enabled)))
			{
				DecimateMesh(skinnedMeshRenderer, quality, recalculateNormals, preserveBorders, preserveSeams, preserveFoldovers);
				yield return 0;
			}
		}
	}

	public static LODSettings[] AutoLODSetttingsToLODSettings(AutoLODSettings[] autoLODSettings)
	{
		if (autoLODSettings == null || autoLODSettings.Length == 0)
		{
			return new LODSettings[0];
		}
		LODSettings[] array = new LODSettings[autoLODSettings.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = autoLODSettings[i].ToLODSettings();
		}
		return array;
	}

	public static void AutoLOD(GameObject go, bool preserveBorders = false, bool preserveSeams = false, bool preserveFoldovers = false)
	{
		AutoLOD(go, new LODSettings[3]
		{
			new LODSettings(0.8f, 50f, SkinQuality.Auto, receiveShadows: true, ShadowCastingMode.On),
			new LODSettings(0.65f, 16f, SkinQuality.Bone2, receiveShadows: true, ShadowCastingMode.Off, MotionVectorGenerationMode.Object, skinnedMotionVectors: false),
			new LODSettings(0.4f, 7f, SkinQuality.Bone1, receiveShadows: false, ShadowCastingMode.Off, MotionVectorGenerationMode.Object, skinnedMotionVectors: false)
		}, 1f, preserveBorders, preserveSeams, preserveFoldovers);
	}

	public static void AutoLOD(GameObject go, AutoLODSettings[] levels, float cullingDistance = 1f, bool preserveBorders = false, bool preserveSeams = false, bool preserveFoldovers = false)
	{
		AutoLOD(go, AutoLODSetttingsToLODSettings(levels), cullingDistance, preserveBorders, preserveSeams, preserveFoldovers);
	}

	public static void AutoLOD(GameObject go, LODSettings[] levels, float cullingDistance = 1f, bool preserveBorders = false, bool preserveSeams = false, bool preserveFoldovers = false)
	{
		LODGroup[] componentsInChildren = go.GetComponentsInChildren<LODGroup>();
		MeshFilter[] componentsInChildren2 = go.GetComponentsInChildren<MeshFilter>();
		SkinnedMeshRenderer[] componentsInChildren3 = go.GetComponentsInChildren<SkinnedMeshRenderer>();
		if (levels == null || levels.Length == 0)
		{
			Debug.LogWarning("MESHKIT: Cannot create LODs on GameObject " + go.name + " because no LODSettings were passed.");
			return;
		}
		if (componentsInChildren.Length != 0)
		{
			Debug.LogWarning("MESHKIT: Cannot create LODs on GameObject " + go.name + " because it already contains LOD Groups.");
			return;
		}
		if (componentsInChildren2.Length != 0 && componentsInChildren3.Length != 0)
		{
			Debug.LogWarning("MESHKIT: Cannot create LODs on GameObject " + go.name + " because it contains both Mesh Filters and Skinned Mesh Renderers.");
			return;
		}
		if (componentsInChildren3.Length > 1)
		{
			Debug.LogWarning("MESHKIT: Cannot create LODs on GameObject " + go.name + " because it contains multiple Skinned Mesh Renderers.");
			return;
		}
		if (componentsInChildren2.Length == 0 && componentsInChildren3.Length == 1 && go.GetComponent<SkinnedMeshRenderer>() == null)
		{
			Debug.LogWarning("MESHKIT: Cannot create LODs on GameObject " + go.name + " because it contains multiple Skinned Mesh Renderers.");
			return;
		}
		for (int i = 0; i < levels.Length; i++)
		{
			if (i > 0 && levels[i].lodDistancePercentage > levels[i - 1].lodDistancePercentage)
			{
				levels[i].lodDistancePercentage = levels[i - 1].lodDistancePercentage - 0.01f;
				if (levels[i].lodDistancePercentage < 0f)
				{
					levels[i].lodDistancePercentage = 0f;
				}
			}
		}
		if (levels.Length >= 1 && cullingDistance >= levels[levels.Length - 1].lodDistancePercentage)
		{
			cullingDistance = levels[levels.Length - 1].lodDistancePercentage - 0.01f;
			if (cullingDistance < 0f)
			{
				cullingDistance = 0f;
			}
		}
		LODGenerator.GenerateLODs(go, levels, null, preserveBorders, preserveSeams, preserveFoldovers);
		LODGroup component = go.GetComponent<LODGroup>();
		if (!(component != null))
		{
			return;
		}
		LOD[] lODs = component.GetLODs();
		if (lODs.Length == levels.Length + 1)
		{
			for (int j = 0; j < lODs.Length; j++)
			{
				if (j < lODs.Length - 1)
				{
					lODs[j].screenRelativeTransitionHeight = levels[j].lodDistancePercentage * 0.01f;
				}
				else if (j == lODs.Length - 1)
				{
					lODs[j].screenRelativeTransitionHeight = cullingDistance * 0.01f;
				}
			}
			component.SetLODs(lODs);
		}
		else
		{
			Debug.Log("MESHKIT: The LODGroup lengths don't match on GameObject: " + go.name + ". Couldn't update custom distances.");
		}
	}
}
