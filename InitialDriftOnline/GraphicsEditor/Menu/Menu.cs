using MelonLoader;
using UnityEngine;

namespace GraphicsEditor.Menu
{
    public static class Menu
    {
        internal static Rect Dimensions = new Rect(10, 10, 240, 155);
        private static readonly int WindowID = new System.Random().Next();
        internal static bool IsOpen { get; private set; } = false;

        internal static void Close()
        {
            MelonEvents.OnGUI.Unsubscribe(Draw);
            IsOpen = false;
        }

        internal static void Open()
        {
            MelonEvents.OnGUI.Subscribe(Draw);
            IsOpen = true;
        }

        private static void Draw()
        {
            Dimensions = GUI.Window(WindowID, Dimensions, (int windowId) =>
            {
                Contents.Generics.ToggleSwitch(GraphicsWrapper.MotionBlur, x => GraphicsWrapper.MotionBlur = x, "MotionBlur");
                Contents.Generics.ToggleSwitch(GraphicsWrapper.Bloom, x => GraphicsWrapper.Bloom = x, "Bloom");
                Contents.Generics.ToggleSwitch(GraphicsWrapper.ChromaticAberration, x => GraphicsWrapper.ChromaticAberration = x, "ChromaticAberration");
                Contents.Generics.ToggleSwitch(GraphicsWrapper.AmbientOcclusion, x => GraphicsWrapper.AmbientOcclusion = x, "AmbientOcclusion");
                Contents.Generics.ToggleSwitch(GraphicsWrapper.DepthOfField, x => GraphicsWrapper.DepthOfField = x, "DepthOfField");
                Contents.Generics.Button(() => Preferences.Save(), "Save Values To File");
                GUI.DragWindow(new Rect(0, 0, Dimensions.width, 20));
            }, "Graphics Editor");
        }
    }
}
