using System;
using System.Collections.Generic;
using UnityEngine;

namespace NetOpt.NetOptDemo;

[Serializable]
public class PackPantherQuantizedDiffSerializer : IDemoSerializer
{
	protected PacketBuffer sourceBuffer;

	protected PacketBuffer targetBuffer;

	protected PacketDelta delta;

	public void Initialize()
	{
		sourceBuffer = new PacketBuffer(16384);
		targetBuffer = new PacketBuffer(16384);
	}

	public virtual int Serialize(List<DemoEntity> entities)
	{
		PacketWriter packetWriter = new PacketWriter(targetBuffer);
		for (int i = 0; i < entities.Count; i++)
		{
			DemoEntity demoEntity = entities[i];
			Vector3 logicalPosition = demoEntity.logicalPosition;
			packetWriter.PackFloat(logicalPosition.x, -1024f, 1023f, 0.01f);
			packetWriter.PackFloat(logicalPosition.y, -1024f, 1023f, 0.01f);
			packetWriter.PackFloat(logicalPosition.z, -1024f, 1023f, 0.01f);
			Quaternion logicalRotation = demoEntity.logicalRotation;
			packetWriter.PackFloat(logicalRotation.x, -1f, 1f, 0.01f);
			packetWriter.PackFloat(logicalRotation.y, -1f, 1f, 0.01f);
			packetWriter.PackFloat(logicalRotation.z, -1f, 1f, 0.01f);
			packetWriter.PackFloat(logicalRotation.w, -1f, 1f, 0.01f);
			Vector3 logicalScale = demoEntity.logicalScale;
			packetWriter.PackFloat(logicalScale.x, 0f, 15f, 0.01f);
			packetWriter.PackFloat(logicalScale.y, 0f, 15f, 0.01f);
			packetWriter.PackFloat(logicalScale.z, 0f, 15f, 0.01f);
		}
		packetWriter.FlushFinalize();
		delta = DeltaCompressor.Encode(sourceBuffer, targetBuffer);
		return delta.Length;
	}

	public virtual void Deserialize(List<DemoEntity> entities)
	{
		sourceBuffer = DeltaCompressor.Decode(sourceBuffer, delta);
		PacketReader packetReader = new PacketReader(sourceBuffer);
		for (int i = 0; i < entities.Count; i++)
		{
			DemoEntity demoEntity = entities[i];
			Vector3 serializedPosition = default(Vector3);
			packetReader.Unpack(out serializedPosition.x, -1024f, 1023f, 0.01f);
			packetReader.Unpack(out serializedPosition.y, -1024f, 1023f, 0.01f);
			packetReader.Unpack(out serializedPosition.z, -1024f, 1023f, 0.01f);
			Quaternion serializedRotation = default(Quaternion);
			packetReader.Unpack(out serializedRotation.x, -1f, 1f, 0.01f);
			packetReader.Unpack(out serializedRotation.y, -1f, 1f, 0.01f);
			packetReader.Unpack(out serializedRotation.z, -1f, 1f, 0.01f);
			packetReader.Unpack(out serializedRotation.w, -1f, 1f, 0.01f);
			Vector3 serializedScale = default(Vector3);
			packetReader.Unpack(out serializedScale.x, 0f, 15f, 0.01f);
			packetReader.Unpack(out serializedScale.y, 0f, 15f, 0.01f);
			packetReader.Unpack(out serializedScale.z, 0f, 15f, 0.01f);
			demoEntity.serializedPosition = serializedPosition;
			demoEntity.serializedRotation = serializedRotation;
			demoEntity.serializedScale = serializedScale;
		}
	}
}
