using MelonLoader;
using UnityEngine;

namespace RemoveInvisibleWalls
{
    public class Main : MelonMod
    {
        private static string[] BlackList = new string[]
        {
            "BetonBorder",
            "62WALL_SUB"
        };
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            LoggerInstance.Msg(sceneName);
            UnityEngine.GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<UnityEngine.GameObject>();
            foreach (GameObject obj in allObjects)
            {
                foreach (string ban in BlackList)
                {
                    if (obj.name.Contains(ban))
                    {
                        GameObject.Destroy(obj);
                        break;
                    }
                }
            }
        }
    }
}
