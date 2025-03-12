using HighFPSSupport;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(Mod), nameof(HighFPSSupport), "1.0.0", "kruumy")]
[assembly: MelonGame("RubyDev", "Tiny Rogues")]
namespace HighFPSSupport
{
    public class Mod : MelonMod
    {
        public override void OnLateUpdate()
        {
            Time.fixedDeltaTime = Time.deltaTime;
        }
    }
}
