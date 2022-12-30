using System.Collections.Generic;
using UnityEngine;

namespace NetOpt.NetOptDemo;

public class PackPantherSerializer : IDemoSerializer
{
	private PacketBuffer buffer;

	public void Initialize()
	{
		buffer = new PacketBuffer(16384);
	}

	public int Serialize(List<DemoEntity> entities)
	{
		PacketWriter packetWriter = new PacketWriter(buffer);
		for (int i = 0; i < entities.Count; i++)
		{
			DemoEntity demoEntity = entities[i];
			Vector3 logicalPosition = demoEntity.logicalPosition;
			packetWriter.PackFloat(logicalPosition.x);
			packetWriter.PackFloat(logicalPosition.y);
			packetWriter.PackFloat(logicalPosition.z);
			Quaternion logicalRotation = demoEntity.logicalRotation;
			packetWriter.PackFloat(logicalRotation.x);
			packetWriter.PackFloat(logicalRotation.y);
			packetWriter.PackFloat(logicalRotation.z);
			packetWriter.PackFloat(logicalRotation.w);
			Vector3 logicalScale = demoEntity.logicalScale;
			packetWriter.PackFloat(logicalScale.x);
			packetWriter.PackFloat(logicalScale.y);
			packetWriter.PackFloat(logicalScale.z);
		}
		return packetWriter.FlushFinalize();
	}

	public void Deserialize(List<DemoEntity> entities)
	{
		PacketReader packetReader = new PacketReader(buffer);
		for (int i = 0; i < entities.Count; i++)
		{
			DemoEntity demoEntity = entities[i];
			Vector3 serializedPosition = default(Vector3);
			packetReader.Unpack(out serializedPosition.x);
			packetReader.Unpack(out serializedPosition.y);
			packetReader.Unpack(out serializedPosition.z);
			Quaternion serializedRotation = default(Quaternion);
			packetReader.Unpack(out serializedRotation.x);
			packetReader.Unpack(out serializedRotation.y);
			packetReader.Unpack(out serializedRotation.z);
			packetReader.Unpack(out serializedRotation.w);
			Vector3 serializedScale = default(Vector3);
			packetReader.Unpack(out serializedScale.x);
			packetReader.Unpack(out serializedScale.y);
			packetReader.Unpack(out serializedScale.z);
			demoEntity.serializedPosition = serializedPosition;
			demoEntity.serializedRotation = serializedRotation;
			demoEntity.serializedScale = serializedScale;
		}
	}
}
