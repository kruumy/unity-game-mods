using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MouseAimAssist
{
    public class Patches
    {
        [HarmonyLib.HarmonyPatch(typeof(MouseLook), "UpdateAimAssist")]
        [HarmonyLib.HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> UpdateAimAssistTranspiler( IEnumerable<CodeInstruction> instructions )
        {
            bool startReturning = false;
            foreach ( var instruction in instructions )
            {
                if ( !startReturning && instruction.opcode == OpCodes.Ldarg_0 ) // if (!this.AimbotEnabled)
                {
                    startReturning = true;
                }
                if(startReturning)
                {
                    yield return instruction;
                }
            }
        }
    }
}
