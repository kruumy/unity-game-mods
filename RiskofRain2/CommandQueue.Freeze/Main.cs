using BepInEx;
using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CommandQueue.Freeze
{
    [BepInPlugin("com.kruumy.CommandQueue.Freeze", "CommandQueue.Freeze", "1.0.0")]
    [BepInDependency("com.kuberoot.commandqueue")]
    public class Main : BaseUnityPlugin
    {
        public void Awake()
        {
            Type QueueDisplay = Assembly.Load("CommandQueue").GetTypes().First(t => t.Name == "QueueDisplay");
            MethodInfo QueueDisplay_OnEnable = QueueDisplay.GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo QueueDisplay_OnDisable = QueueDisplay.GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.Public);

            Harmony harmony = new Harmony("com.kruumy.CommandQueue.Freeze");
            harmony.Patch(QueueDisplay_OnEnable,
                postfix: new HarmonyMethod(typeof(Main)
                .GetMethod(nameof(QueueDisplay_OnEnable_Postfix), BindingFlags.Static | BindingFlags.NonPublic)));

            harmony.Patch(QueueDisplay_OnDisable,
                postfix: new HarmonyMethod(typeof(Main)
                .GetMethod(nameof(QueueDisplay_OnDisable_Postfix), BindingFlags.Static | BindingFlags.NonPublic)));
        }


        private static void QueueDisplay_OnEnable_Postfix()
        {
            Time.timeScale = 0f;
        }
        private static void QueueDisplay_OnDisable_Postfix()
        {
            Time.timeScale = 1f;
        }
    }
}
