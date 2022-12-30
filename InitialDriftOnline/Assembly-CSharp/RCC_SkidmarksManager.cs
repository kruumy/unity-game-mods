using UnityEngine;

public class RCC_SkidmarksManager : MonoBehaviour
{
	public RCC_Skidmarks[] skidmarks;

	public int[] skidmarksIndexes;

	private int _lastGroundIndex;

	private void Start()
	{
		skidmarks = new RCC_Skidmarks[RCC_GroundMaterials.Instance.frictions.Length];
		skidmarksIndexes = new int[skidmarks.Length];
		for (int i = 0; i < skidmarks.Length; i++)
		{
			skidmarks[i] = Object.Instantiate(RCC_GroundMaterials.Instance.frictions[i].skidmark, Vector3.zero, Quaternion.identity);
			skidmarks[i].transform.name = skidmarks[i].transform.name + "_" + RCC_GroundMaterials.Instance.frictions[i].groundMaterial.name;
			skidmarks[i].transform.SetParent(base.transform, worldPositionStays: true);
		}
	}

	public int AddSkidMark(Vector3 pos, Vector3 normal, float intensity, int lastIndex, int groundIndex)
	{
		if (_lastGroundIndex != groundIndex)
		{
			_lastGroundIndex = groundIndex;
			return -1;
		}
		skidmarksIndexes[groundIndex] = skidmarks[groundIndex].AddSkidMark(pos, normal, intensity, lastIndex);
		return skidmarksIndexes[groundIndex];
	}
}
