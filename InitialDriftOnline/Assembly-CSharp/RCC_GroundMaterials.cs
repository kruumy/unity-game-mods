using System;
using UnityEngine;

[Serializable]
public class RCC_GroundMaterials : ScriptableObject
{
	[Serializable]
	public class GroundMaterialFrictions
	{
		public PhysicMaterial groundMaterial;

		public float forwardStiffness = 1f;

		public float sidewaysStiffness = 1f;

		public float slip = 0.25f;

		public float damp = 1f;

		[Range(0f, 1f)]
		public float volume = 1f;

		public GameObject groundParticles;

		public AudioClip groundSound;

		public RCC_Skidmarks skidmark;
	}

	[Serializable]
	public class TerrainFrictions
	{
		[Serializable]
		public class SplatmapIndexes
		{
			public int index;

			public PhysicMaterial groundMaterial;
		}

		public PhysicMaterial groundMaterial;

		public SplatmapIndexes[] splatmapIndexes;
	}

	private static RCC_GroundMaterials instance;

	public GroundMaterialFrictions[] frictions;

	public TerrainFrictions[] terrainFrictions;

	public static RCC_GroundMaterials Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Resources.Load("RCC Assets/RCC_GroundMaterials") as RCC_GroundMaterials;
			}
			return instance;
		}
	}
}
