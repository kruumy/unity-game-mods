using MelonLoader;
using System.Linq;
using UnityEngine;

namespace RemovePixelationFilter
{
    public class Main : MelonMod
    {
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            Object.Destroy(Camera.main.GetComponents(typeof(PixelateImageEffect)).FirstOrDefault());
            MelonLogger.Msg("Destroyed PixelateImageEffect");
        }
    }
}
