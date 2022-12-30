using UnityEngine;

public class RCC_GetBounds : MonoBehaviour
{
	public static Vector3 GetBoundsCenter(Transform obj)
	{
		Renderer[] componentsInChildren = obj.GetComponentsInChildren<Renderer>();
		Bounds bounds = default(Bounds);
		bool flag = false;
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			if (!(renderer is TrailRenderer) && !(renderer is ParticleSystemRenderer))
			{
				if (!flag)
				{
					flag = true;
					bounds = renderer.bounds;
				}
				else
				{
					bounds.Encapsulate(renderer.bounds);
				}
			}
		}
		return bounds.center;
	}

	public static float MaxBoundsExtent(Transform obj)
	{
		Renderer[] componentsInChildren = obj.GetComponentsInChildren<Renderer>();
		Bounds bounds = default(Bounds);
		bool flag = false;
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			if (!(renderer is TrailRenderer) && !(renderer is ParticleSystemRenderer))
			{
				if (!flag)
				{
					flag = true;
					bounds = renderer.bounds;
				}
				else
				{
					bounds.Encapsulate(renderer.bounds);
				}
			}
		}
		return Mathf.Max(bounds.extents.x, bounds.extents.y, bounds.extents.z);
	}
}
