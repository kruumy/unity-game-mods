using HarmonyLib;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(SmootherGame.Main), "SmootherGame", "1.0.0", "kruumy")]
[assembly: MelonGame("Playstack", "AK-xolotl")]

namespace SmootherGame
{
    public class Main : MelonMod
    {
        public override void OnUpdate()
        {
            Time.fixedDeltaTime = Time.deltaTime; // TODO: enable interpolation instead
        }
    }
}
