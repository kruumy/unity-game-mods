using UnityEngine;

namespace Funly.SkyStudio;

public class BaseSpriteItemData
{
	public enum SpriteState
	{
		Unknown,
		NotStarted,
		Animating,
		Complete
	}

	public SpriteSheetData spriteSheetData;

	public float delay;

	public Matrix4x4 modelMatrix { get; protected set; }

	public SpriteState state { get; protected set; }

	public Vector3 spritePosition { get; set; }

	public float startTime { get; protected set; }

	public float endTime { get; protected set; }

	public BaseSpriteItemData()
	{
		state = SpriteState.NotStarted;
	}

	public void SetTRSMatrix(Vector3 worldPosition, Quaternion rotation, Vector3 scale)
	{
		spritePosition = worldPosition;
		modelMatrix = Matrix4x4.TRS(worldPosition, rotation, scale);
	}

	public void Start()
	{
		state = SpriteState.Animating;
		startTime = CalculateStartTimeWithDelay(delay);
		endTime = CalculateEndTime(startTime, spriteSheetData.frameCount, spriteSheetData.frameRate);
	}

	public void Continue()
	{
		if (state == SpriteState.Animating && Time.time > endTime)
		{
			state = SpriteState.Complete;
		}
	}

	public void Reset()
	{
		state = SpriteState.NotStarted;
		startTime = -1f;
		endTime = -1f;
	}

	public static float CalculateStartTimeWithDelay(float delay)
	{
		return Time.time + delay;
	}

	public static float CalculateEndTime(float startTime, int itemCount, int animationSpeed)
	{
		float num = 1f / (float)animationSpeed;
		float num2 = (float)itemCount * num;
		return startTime + num2;
	}
}
