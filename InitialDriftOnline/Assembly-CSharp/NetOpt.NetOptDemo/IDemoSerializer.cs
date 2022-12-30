using System.Collections.Generic;

namespace NetOpt.NetOptDemo;

public interface IDemoSerializer
{
	void Initialize();

	int Serialize(List<DemoEntity> entities);

	void Deserialize(List<DemoEntity> entities);
}
