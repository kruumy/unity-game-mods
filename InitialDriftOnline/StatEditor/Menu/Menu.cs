using MelonLoader;
using UnityEngine;

namespace StatEditor.Menu
{
    public static class Menu
    {
        internal static Rect Dimensions = new Rect(10, 10, 300, 115);
        private static readonly int WindowID = new System.Random().Next();

        internal static bool IsOpen { get; private set; } = false;

        internal static void Open()
        {
            MelonEvents.OnGUI.Subscribe(Draw);
            IsOpen = true;
            Cursor.visible = true;
        }

        internal static void Close()
        {
            MelonEvents.OnGUI.Unsubscribe(Draw);
            IsOpen = false;
            Cursor.visible = false;
        }

        private static void Draw()
        {
            Dimensions = GUI.Window(WindowID, Dimensions, (int windowId) =>
            {
                Contents.DownForce.Label.Draw();
                Contents.DownForce.HorizontalSlider.Draw();

                GUI.DragWindow(new Rect(0, 0, Dimensions.width, 20));
            }, "Stat Editor");
        }
    }
}
