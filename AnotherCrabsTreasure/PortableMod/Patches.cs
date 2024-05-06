using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableMod
{
    public class Patches
    {
        [HarmonyPatch(typeof(PlatformPaths), "persistentDataPath", MethodType.Getter)]
        [HarmonyPostfix]
        static void DataPathPostfix( ref string __result )
        {
            __result = (Path.Combine(Environment.CurrentDirectory, "UserSettings") + @"\");
        }


        [HarmonyPatch(typeof(FileIO), "WriteAllText")]
        [HarmonyPrefix]
        static bool WriteAllTextPrefix( ref string path, ref string contents )
        {
            File.WriteAllText( path, contents );
            return false;
        }

        [HarmonyPatch(typeof(FileIO), "ReadAllText")]
        [HarmonyPrefix]
        static bool ReadAllTextPrefix( ref string path, ref string __result )
        {
            __result = File.ReadAllText(path);
            return false;
        }

        [HarmonyPatch(typeof(FileIO), "Exists")]
        [HarmonyPrefix]
        static bool ExistsPrefix( ref string path, ref bool __result )
        {
            __result = File.Exists(path);
            return false;
        }

        [HarmonyPatch(typeof(FileIO), "Delete")]
        [HarmonyPrefix]
        static bool DeletePrefix( ref string path )
        {
            File.Delete(path);
            return false;
        }

        [HarmonyPatch(typeof(FileIO), "WriteAllBytes")]
        [HarmonyPrefix]
        static bool WriteAllBytesPrefix( ref string path, ref byte[] bytes )
        {
            File.WriteAllBytes(path, bytes);
            return false;
        }

        [HarmonyPatch(typeof(FileIO), "ReadAllBytes")]
        [HarmonyPrefix]
        static bool ReadAllBytesPrefix( ref string path, ref byte[] __result )
        {
            __result = File.ReadAllBytes(path);
            return false;
        }
    }
}
