namespace HellTap.MeshDecimator.Collections;

internal sealed class UVChannels<TVec>
{
	private ResizableArray<TVec>[] channels;

	private TVec[][] channelsData;

	public TVec[][] Data
	{
		get
		{
			for (int i = 0; i < 4; i++)
			{
				if (channels[i] != null)
				{
					channelsData[i] = channels[i].Data;
				}
				else
				{
					channelsData[i] = null;
				}
			}
			return channelsData;
		}
	}

	public ResizableArray<TVec> this[int index]
	{
		get
		{
			return channels[index];
		}
		set
		{
			channels[index] = value;
		}
	}

	public UVChannels()
	{
		channels = new ResizableArray<TVec>[4];
		channelsData = new TVec[4][];
	}

	public void Resize(int capacity, bool trimExess = false)
	{
		for (int i = 0; i < 4; i++)
		{
			if (channels[i] != null)
			{
				channels[i].Resize(capacity, trimExess);
			}
		}
	}
}
