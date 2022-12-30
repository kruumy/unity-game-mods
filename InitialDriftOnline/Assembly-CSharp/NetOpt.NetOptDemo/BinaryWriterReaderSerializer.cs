using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace NetOpt.NetOptDemo;

public class BinaryWriterReaderSerializer : IDemoSerializer
{
	private MemoryStream _stream;

	public void Initialize()
	{
		_stream = new MemoryStream(16384);
	}

	public int Serialize(List<DemoEntity> entities)
	{
		_stream.Seek(0L, SeekOrigin.Begin);
		BinaryWriter binaryWriter = new BinaryWriter(_stream);
		for (int i = 0; i < entities.Count; i++)
		{
			DemoEntity demoEntity = entities[i];
			Vector3 logicalPosition = demoEntity.logicalPosition;
			binaryWriter.Write(logicalPosition.x);
			binaryWriter.Write(logicalPosition.y);
			binaryWriter.Write(logicalPosition.z);
			Quaternion logicalRotation = demoEntity.logicalRotation;
			binaryWriter.Write(logicalRotation.x);
			binaryWriter.Write(logicalRotation.y);
			binaryWriter.Write(logicalRotation.z);
			binaryWriter.Write(logicalRotation.w);
			Vector3 logicalScale = demoEntity.logicalScale;
			binaryWriter.Write(logicalScale.x);
			binaryWriter.Write(logicalScale.y);
			binaryWriter.Write(logicalScale.z);
		}
		binaryWriter.Flush();
		return (int)_stream.Position;
	}

	public void Deserialize(List<DemoEntity> entities)
	{
		_stream.Seek(0L, SeekOrigin.Begin);
		BinaryReader binaryReader = new BinaryReader(_stream);
		for (int i = 0; i < entities.Count; i++)
		{
			DemoEntity demoEntity = entities[i];
			Vector3 serializedPosition = new Vector3
			{
				x = binaryReader.ReadSingle(),
				y = binaryReader.ReadSingle(),
				z = binaryReader.ReadSingle()
			};
			Quaternion serializedRotation = new Quaternion
			{
				x = binaryReader.ReadSingle(),
				y = binaryReader.ReadSingle(),
				z = binaryReader.ReadSingle(),
				w = binaryReader.ReadSingle()
			};
			Vector3 serializedScale = new Vector3
			{
				x = binaryReader.ReadSingle(),
				y = binaryReader.ReadSingle(),
				z = binaryReader.ReadSingle()
			};
			demoEntity.serializedPosition = serializedPosition;
			demoEntity.serializedRotation = serializedRotation;
			demoEntity.serializedScale = serializedScale;
		}
	}
}
