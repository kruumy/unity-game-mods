using HarmonyLib;
using Il2CppAssets.Scripts.Unity.Player;
using MelonLoader;

[assembly: MelonInfo(typeof(ImNotHacking.Main), "ImNotHacking", "1.0.0", "kruumy")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace ImNotHacking
{
    public class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("ImNotHacking Loaded!");
        }

        [HarmonyPatch(typeof(Btd6Player.__c), "_CheckHakrStatus_b__190_0", typeof(Il2CppSystem.Collections.Generic.KeyValuePair<string, long>))]
        public static class _CheckHakrStatus_b__190_0Patch
        {
            [HarmonyPostfix]
            public static void Postfix(ref bool __result)
            {
                __result = false;
                MelonLogger.Msg("Modified _CheckHakrStatus_b__190_0 result");
            }
        }

        [HarmonyPatch(typeof(Btd6Player.__c), "_CheckHakrStatus_b__190_1", typeof(Il2CppSystem.Collections.Generic.KeyValuePair<string, long>))]
        public static class _CheckHakrStatus_b__190_1Patch
        {
            [HarmonyPostfix]
            public static void Postfix(ref bool __result)
            {
                __result = false;
                MelonLogger.Msg("Modified _CheckHakrStatus_b__190_1 result");
            }
        }

        [HarmonyPatch(typeof(Btd6Player), "CheckHakrStatus", new Type[] { })]
        public static class CheckHakrStatusPatch
        {
            [HarmonyPostfix]
            public static void Postfix(ref Il2CppSystem.Threading.Tasks.Task<Btd6Player.HakrStatus> __result)
            {
                Btd6Player.HakrStatus res = new Btd6Player.HakrStatus();
                res.ledrbrd = false;
                res.genrl = false;
                __result.m_result = res;
                MelonLogger.Msg("Modified CheckHakrStatus result");
            }
        }

        [HarmonyPatch(typeof(Btd6Player), "CheckMultiHakrStatus")]
        public static class CheckMultiHakrStatusPatch
        {
            [HarmonyPostfix]
            public static void Postfix(ref Il2CppSystem.Threading.Tasks.Task<bool> __result)
            {
                __result.m_result = false;
                MelonLogger.Msg("Modified CheckMultiHakrStatus result");
            }
        }

        [HarmonyPatch(typeof(Btd6Player), "Hakxr", MethodType.Setter)]
        public static class HakxrSetterPatch
        {
            [HarmonyPrefix]
            public static void Prefix(ref Btd6Player.HakrStatus value)
            {
                value.ledrbrd = false;
                value.genrl = false;
                MelonLogger.Msg("Modified HakxrSetter value");
            }
        }

        [HarmonyPatch(typeof(Btd6Player), "Hakxr", MethodType.Getter)]
        public static class HakxrGetterPatch
        {
            [HarmonyPostfix]
            public static void Postfix(ref Btd6Player.HakrStatus __result)
            {
                __result.ledrbrd = false;
                __result.genrl = false;
                MelonLogger.Msg("Modified HakxrGetter result");
            }
        }
    }
}