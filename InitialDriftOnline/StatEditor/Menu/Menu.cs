using MelonLoader;
using UnityEngine;

namespace StatEditor.Menu
{
    public static class Menu
    {
        internal static Rect Dimensions = new Rect(10, 10, 300, 255);
        private static readonly int WindowID = new System.Random().Next();

        internal static bool IsOpen { get; private set; } = false;

        internal static void Open()
        {
            MelonEvents.OnGUI.Subscribe(Draw);
            IsOpen = true;
        }

        internal static void Close()
        {
            MelonEvents.OnGUI.Unsubscribe(Draw);
            IsOpen = false;
        }

        private static void Draw()
        {
            Dimensions = GUI.Window(WindowID, Dimensions, (int windowId) =>
            {
                Contents.Generics.SliderWithLabel(ref RCC_SceneManager.Instance.activePlayerVehicle.downForce, -500, 500, "downForce");
                Contents.Generics.SliderWithLabel(ref RCC_SceneManager.Instance.activePlayerVehicle.engineTorque, 0, 10000, "engineTorque");
                Contents.Generics.SliderWithLabel(ref RCC_SceneManager.Instance.activePlayerVehicle.brakeTorque, 0, 1000000, "brakeTorque");
                Contents.Generics.SliderWithLabel(ref RCC_SceneManager.Instance.activePlayerVehicle.maxspeed, 0, 1000, "maxspeed");
                Contents.Generics.SliderWithLabel(RCC_SceneManager.Instance.activePlayerVehicle.get_orgSteerAngle(), x => RCC_SceneManager.Instance.activePlayerVehicle.set_orgSteerAngle(x), 0, 180, "orgSteerAngle");
                Contents.Generics.Button(() => Preferences.Save(), "Save Values To File");
                GUI.DragWindow(new Rect(0, 0, Dimensions.width, 20));
            }, "Stat Editor");
        }



    }
}
