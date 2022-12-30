using System;
using System.IO;
using UnityEngine;

namespace AsImpL;

public class TextureLoader : MonoBehaviour
{
	private class TgaHeader
	{
		public byte identSize;

		public byte colorMapType;

		public byte imageType;

		public ushort colorMapStart;

		public ushort colorMapLength;

		public byte colorMapBits;

		public ushort xStart;

		public ushort ySstart;

		public ushort width;

		public ushort height;

		public byte bits;

		public byte descriptor;
	}

	public static Texture2D LoadTextureFromUrl(string url)
	{
		url = ((!url.StartsWith("file:///")) ? Path.GetFullPath(url) : url.Substring("file:///".Length));
		return LoadTexture(url);
	}

	public static Texture2D LoadTexture(string fileName)
	{
		switch (Path.GetExtension(fileName).ToLower())
		{
		case ".png":
		case ".jpg":
		{
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.LoadImage(File.ReadAllBytes(fileName));
			return texture2D;
		}
		case ".dds":
			return LoadDDSManual(fileName);
		case ".tga":
			return LoadTGA(fileName);
		default:
			Debug.Log("texture not supported : " + fileName);
			return null;
		}
	}

	public static Texture2D LoadTGA(string fileName)
	{
		using FileStream tGAStream = File.OpenRead(fileName);
		return LoadTGA(tGAStream);
	}

	public static Texture2D LoadDDSManual(string ddsPath)
	{
		try
		{
			byte[] array = File.ReadAllBytes(ddsPath);
			if (array[4] != 124)
			{
				throw new Exception("Invalid DDS DXTn texture. Unable to read");
			}
			int height = array[13] * 256 + array[12];
			int width = array[17] * 256 + array[16];
			byte num = array[87];
			TextureFormat textureFormat = TextureFormat.DXT5;
			if (num == 49)
			{
				textureFormat = TextureFormat.DXT1;
			}
			if (num == 53)
			{
				textureFormat = TextureFormat.DXT5;
			}
			int num2 = 128;
			byte[] array2 = new byte[array.Length - num2];
			Buffer.BlockCopy(array, num2, array2, 0, array.Length - num2);
			FileInfo fileInfo = new FileInfo(ddsPath);
			Texture2D texture2D = new Texture2D(width, height, textureFormat, mipChain: false);
			texture2D.LoadRawTextureData(array2);
			texture2D.Apply();
			texture2D.name = fileInfo.Name;
			return texture2D;
		}
		catch (Exception ex)
		{
			Debug.LogError("Could not load DDS: " + ex);
			return new Texture2D(8, 8);
		}
	}

	public static Texture2D LoadTGA(Stream TGAStream)
	{
		try
		{
			using BinaryReader binaryReader = new BinaryReader(TGAStream);
			TgaHeader tgaHeader = LoadTgaHeader(binaryReader);
			short num = (short)tgaHeader.width;
			short num2 = (short)tgaHeader.height;
			int bits = tgaHeader.bits;
			bool flag = (tgaHeader.descriptor & 0x20) == 32;
			Texture2D texture2D = new Texture2D(num, num2);
			Color32[] array = new Color32[num * num2];
			int num3 = num * num2;
			switch (bits)
			{
			case 32:
			{
				for (int k = 1; k <= num2; k++)
				{
					for (int l = 0; l < num; l++)
					{
						byte b2 = binaryReader.ReadByte();
						byte g2 = binaryReader.ReadByte();
						byte r2 = binaryReader.ReadByte();
						byte a = binaryReader.ReadByte();
						int num5 = ((!flag) ? (num3 - (num2 - k + 1) * num + l) : (num3 - k * num + l));
						array[num5] = new Color32(r2, g2, b2, a);
					}
				}
				break;
			}
			case 24:
			{
				for (int i = 1; i <= num2; i++)
				{
					for (int j = 0; j < num; j++)
					{
						byte b = binaryReader.ReadByte();
						byte g = binaryReader.ReadByte();
						byte r = binaryReader.ReadByte();
						int num4 = ((!flag) ? (num3 - (num2 - i + 1) * num + j) : (num3 - i * num + j));
						array[num4] = new Color32(r, g, b, byte.MaxValue);
					}
				}
				break;
			}
			default:
				throw new Exception("TGA texture had non 32/24 bit depth.");
			}
			texture2D.SetPixels32(array);
			texture2D.Apply();
			return texture2D;
		}
		catch (Exception message)
		{
			Debug.LogWarning(message);
			return null;
		}
	}

	private static TgaHeader LoadTgaHeader(BinaryReader r)
	{
		TgaHeader tgaHeader = new TgaHeader();
		r.BaseStream.Seek(0L, SeekOrigin.Current);
		tgaHeader.identSize = r.ReadByte();
		tgaHeader.colorMapType = r.ReadByte();
		tgaHeader.imageType = r.ReadByte();
		tgaHeader.colorMapStart = r.ReadUInt16();
		tgaHeader.colorMapLength = r.ReadUInt16();
		tgaHeader.colorMapBits = r.ReadByte();
		tgaHeader.xStart = r.ReadUInt16();
		tgaHeader.ySstart = r.ReadUInt16();
		tgaHeader.width = r.ReadUInt16();
		tgaHeader.height = r.ReadUInt16();
		tgaHeader.bits = r.ReadByte();
		tgaHeader.descriptor = r.ReadByte();
		Debug.LogFormat("TGA descriptor = {0}", tgaHeader.descriptor);
		if (tgaHeader.imageType == 0)
		{
			new Exception("TGA image contains no data.");
		}
		if (tgaHeader.imageType > 10)
		{
			new Exception("compressed TGA not supported.");
		}
		if (tgaHeader.imageType == 1 || tgaHeader.imageType == 9)
		{
			new Exception("color indexed TGA not supported.");
		}
		if (tgaHeader.bits != 24 && tgaHeader.bits != 32)
		{
			throw new Exception("only 24/32 bits TGA supported.");
		}
		if (tgaHeader.width <= 0 || tgaHeader.height <= 0)
		{
			throw new Exception("TGA texture has invalid size.");
		}
		r.BaseStream.Seek(tgaHeader.identSize, SeekOrigin.Current);
		return tgaHeader;
	}
}
