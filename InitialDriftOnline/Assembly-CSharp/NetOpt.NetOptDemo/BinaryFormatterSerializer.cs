using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace NetOpt.NetOptDemo;

public class BinaryFormatterSerializer : IDemoSerializer
{
	[Serializable]
	public struct Vector3S
	{
		public float x;

		public float y;

		public float z;

		public static explicit operator Vector3S(Vector3 other)
		{
			return new Vector3S(other);
		}

		public static implicit operator Vector3(Vector3S other)
		{
			return new Vector3(other.x, other.y, other.z);
		}

		public Vector3S(Vector3 other)
		{
			x = other.x;
			y = other.y;
			z = other.z;
		}
	}

	[Serializable]
	public struct QuaternionS
	{
		public float x;

		public float y;

		public float z;

		public float w;

		public static explicit operator QuaternionS(Quaternion other)
		{
			return new QuaternionS(other);
		}

		public static implicit operator Quaternion(QuaternionS other)
		{
			return new Quaternion(other.x, other.y, other.z, other.w);
		}

		public QuaternionS(Quaternion other)
		{
			x = other.x;
			y = other.y;
			z = other.z;
			w = other.w;
		}
	}

	private MemoryStream _stream;

	public void Initialize()
	{
		_stream = new MemoryStream(16384);
	}

	public int Serialize(List<DemoEntity> entities)
	{
		_stream.Seek(0L, SeekOrigin.Begin);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		for (int i = 0; i < entities.Count; i++)
		{
			DemoEntity demoEntity = entities[i];
			binaryFormatter.Serialize(_stream, (Vector3S)demoEntity.logicalPosition);
			binaryFormatter.Serialize(_stream, (QuaternionS)demoEntity.logicalRotation);
			binaryFormatter.Serialize(_stream, (Vector3S)demoEntity.logicalScale);
		}
		return (int)_stream.Position;
	}

	public void Deserialize(List<DemoEntity> entities)
	{
		_stream.Seek(0L, SeekOrigin.Begin);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		for (int i = 0; i < entities.Count; i++)
		{
			DemoEntity demoEntity = entities[i];
			demoEntity.serializedPosition = (Vector3S)binaryFormatter.Deserialize(_stream);
			demoEntity.serializedRotation = (QuaternionS)binaryFormatter.Deserialize(_stream);
			demoEntity.serializedScale = (Vector3S)binaryFormatter.Deserialize(_stream);
		}
	}
}
