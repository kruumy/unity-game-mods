using System.Collections.Generic;
using UnityEngine;

namespace NetOpt.NetOptDemo;

public class PackPantherQuantizedSerializer : IDemoSerializer
{
	protected PacketBuffer buffer;

	public void Initialize()
	{
		buffer = new PacketBuffer(16384);
	}

	public virtual int Serialize(List<DemoEntity> entities)
	{
		PacketWriter packetWriter = new PacketWriter(buffer);
		for (int i = 0; i < entities.Count; i++)
		{
			DemoEntity demoEntity = entities[i];
			Vector3 logicalPosition = demoEntity.logicalPosition;
			packetWriter.PackFloat(logicalPosition.x, -32f, 31f, 0.01f);
			packetWriter.PackFloat(logicalPosition.y, -32f, 31f, 0.01f);
			packetWriter.PackFloat(logicalPosition.z, -32f, 31f, 0.01f);
			Quaternion logicalRotation = demoEntity.logicalRotation;
			packetWriter.PackFloat(logicalRotation.x, -1f, 1f, 0.01f);
			packetWriter.PackFloat(logicalRotation.y, -1f, 1f, 0.01f);
			packetWriter.PackFloat(logicalRotation.z, -1f, 1f, 0.01f);
			packetWriter.PackFloat(logicalRotation.w, -1f, 1f, 0.01f);
			Vector3 logicalScale = demoEntity.logicalScale;
			packetWriter.PackFloat(logicalScale.x, 0f, 7f, 0.01f);
			packetWriter.PackFloat(logicalScale.y, 0f, 7f, 0.01f);
			packetWriter.PackFloat(logicalScale.z, 0f, 7f, 0.01f);
		}
		return packetWriter.FlushFinalize();
	}

	public virtual void Deserialize(List<DemoEntity> entities)
	{
		PacketReader packetReader = new PacketReader(buffer);
		for (int i = 0; i < entities.Count; i++)
		{
			DemoEntity demoEntity = entities[i];
			Vector3 serializedPosition = default(Vector3);
			packetReader.Unpack(out serializedPosition.x, -32f, 31f, 0.01f);
			packetReader.Unpack(out serializedPosition.y, -32f, 31f, 0.01f);
			packetReader.Unpack(out serializedPosition.z, -32f, 31f, 0.01f);
			Quaternion serializedRotation = default(Quaternion);
			packetReader.Unpack(out serializedRotation.x, -1f, 1f, 0.01f);
			packetReader.Unpack(out serializedRotation.y, -1f, 1f, 0.01f);
			packetReader.Unpack(out serializedRotation.z, -1f, 1f, 0.01f);
			packetReader.Unpack(out serializedRotation.w, -1f, 1f, 0.01f);
			Vector3 serializedScale = default(Vector3);
			packetReader.Unpack(out serializedScale.x, 0f, 7f, 0.01f);
			packetReader.Unpack(out serializedScale.y, 0f, 7f, 0.01f);
			packetReader.Unpack(out serializedScale.z, 0f, 7f, 0.01f);
			demoEntity.serializedPosition = serializedPosition;
			demoEntity.serializedRotation = serializedRotation;
			demoEntity.serializedScale = serializedScale;
		}
	}
}
