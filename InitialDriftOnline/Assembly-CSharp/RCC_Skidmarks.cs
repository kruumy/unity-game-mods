using UnityEngine;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/Skidmarks")]
public class RCC_Skidmarks : MonoBehaviour
{
	public class markSection
	{
		public Vector3 pos = Vector3.zero;

		public Vector3 normal = Vector3.zero;

		public Vector4 tangent = Vector4.zero;

		public Vector3 posl = Vector3.zero;

		public Vector3 posr = Vector3.zero;

		public float intensity;

		public int lastIndex;
	}

	private MeshFilter meshFilter;

	private Mesh mesh;

	public int maxMarks = 1024;

	public float markWidth = 0.275f;

	public float groundOffset = 0.02f;

	public float minDistance = 0.1f;

	private int indexShift;

	private int numMarks;

	public markSection[] skidmarks;

	public bool updated;

	private void Awake()
	{
		maxMarks = 1024;
		skidmarks = new markSection[maxMarks];
		for (int i = 0; i < maxMarks; i++)
		{
			skidmarks[i] = new markSection();
		}
		meshFilter = GetComponent<MeshFilter>();
		mesh = meshFilter.mesh;
	}

	private void Start()
	{
		maxMarks = 1024;
		if (base.transform.position != Vector3.zero)
		{
			base.transform.position = Vector3.zero;
		}
	}

	public int AddSkidMark(Vector3 pos, Vector3 normal, float intensity, int lastIndex)
	{
		if (intensity > 1f)
		{
			intensity = 1f;
		}
		if (intensity < 0f)
		{
			return -1;
		}
		if (lastIndex > 0 && (pos - skidmarks[lastIndex % maxMarks].pos).sqrMagnitude < minDistance)
		{
			return lastIndex;
		}
		markSection markSection = skidmarks[numMarks % maxMarks];
		markSection.pos = pos + normal * groundOffset;
		markSection.normal = normal;
		markSection.intensity = intensity;
		markSection.lastIndex = lastIndex;
		if (lastIndex != -1)
		{
			markSection markSection2 = skidmarks[lastIndex % maxMarks];
			Vector3 normalized = Vector3.Cross(markSection.pos - markSection2.pos, normal).normalized;
			markSection.posl = markSection.pos + normalized * markWidth * 0.5f;
			markSection.posr = markSection.pos - normalized * markWidth * 0.5f;
			markSection.tangent = new Vector4(normalized.x, normalized.y, normalized.z, 1f);
			if (markSection2.lastIndex == -1)
			{
				markSection2.tangent = markSection.tangent;
				markSection2.posl = markSection.pos + normalized * markWidth * 0.5f;
				markSection2.posr = markSection.pos - normalized * markWidth * 0.5f;
			}
		}
		numMarks++;
		updated = true;
		return numMarks - 1;
	}

	private void LateUpdate()
	{
		if (!updated)
		{
			return;
		}
		updated = false;
		mesh.Clear();
		int num = 0;
		for (int i = 0; i < numMarks && i < maxMarks; i++)
		{
			if (skidmarks[i].lastIndex != -1 && skidmarks[i].lastIndex > numMarks - maxMarks)
			{
				num++;
			}
		}
		Vector3[] array = new Vector3[num * 4];
		Vector3[] array2 = new Vector3[num * 4];
		Vector4[] array3 = new Vector4[num * 4];
		Color[] array4 = new Color[num * 4];
		Vector2[] array5 = new Vector2[num * 4];
		int[] array6 = new int[num * 6];
		num = 0;
		for (int j = 0; j < numMarks && j < maxMarks; j++)
		{
			if (skidmarks[j].lastIndex != -1 && skidmarks[j].lastIndex > numMarks - maxMarks)
			{
				markSection markSection = skidmarks[j];
				markSection markSection2 = skidmarks[markSection.lastIndex % maxMarks];
				array[num * 4] = markSection2.posl;
				array[num * 4 + 1] = markSection2.posr;
				array[num * 4 + 2] = markSection.posl;
				array[num * 4 + 3] = markSection.posr;
				array2[num * 4] = markSection2.normal;
				array2[num * 4 + 1] = markSection2.normal;
				array2[num * 4 + 2] = markSection.normal;
				array2[num * 4 + 3] = markSection.normal;
				array3[num * 4] = markSection2.tangent;
				array3[num * 4 + 1] = markSection2.tangent;
				array3[num * 4 + 2] = markSection.tangent;
				array3[num * 4 + 3] = markSection.tangent;
				array4[num * 4] = new Color(0f, 0f, 0f, markSection2.intensity);
				array4[num * 4 + 1] = new Color(0f, 0f, 0f, markSection2.intensity);
				array4[num * 4 + 2] = new Color(0f, 0f, 0f, markSection.intensity);
				array4[num * 4 + 3] = new Color(0f, 0f, 0f, markSection.intensity);
				array5[num * 4] = new Vector2(0f, 0f);
				array5[num * 4 + 1] = new Vector2(1f, 0f);
				array5[num * 4 + 2] = new Vector2(0f, 1f);
				array5[num * 4 + 3] = new Vector2(1f, 1f);
				array6[num * 6] = num * 4;
				array6[num * 6 + 2] = num * 4 + 1;
				array6[num * 6 + 1] = num * 4 + 2;
				array6[num * 6 + 3] = num * 4 + 2;
				array6[num * 6 + 5] = num * 4 + 1;
				array6[num * 6 + 4] = num * 4 + 3;
				num++;
			}
		}
		mesh.vertices = array;
		mesh.normals = array2;
		mesh.tangents = array3;
		mesh.triangles = array6;
		mesh.colors = array4;
		mesh.uv = array5;
	}
}
