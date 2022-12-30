using System.Collections.Generic;

namespace NetOpt.NetOptDemo;

public class PackPantherQuantizedCompressedSerializer : PackPantherQuantizedSerializer
{
	public override int Serialize(List<DemoEntity> entities)
	{
		int bytesToCompress = base.Serialize(entities);
		buffer = Compressor.Compress(buffer, bytesToCompress, Compressor.Algorithm.ZSTDF);
		return buffer.Length;
	}

	public override void Deserialize(List<DemoEntity> entities)
	{
		buffer = Compressor.Decompress(buffer, 16384, Compressor.Algorithm.ZSTDF);
		base.Deserialize(entities);
	}
}
