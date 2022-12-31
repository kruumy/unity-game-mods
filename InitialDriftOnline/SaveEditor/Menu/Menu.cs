using MelonLoader;
using UnityEngine;

namespace SaveEditor.Menu
{
    internal static class Menu
    {
        internal static Rect Dimensions = new Rect(10, 10, 300, 115);
        internal static readonly int Margin = 5;
        internal static readonly int TitleBarHeight = 15;
        internal static readonly int RowHeight = 25;
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
                Contents.SetMoney.Button.Draw();
                Contents.SetMoney.TextField.Draw();

                Contents.SetLevel.Button.Draw();
                Contents.SetLevel.TextField.Draw();

                Contents.SetBoost.Button.Draw();
                Contents.SetBoost.TextField.Draw();

                GUI.DragWindow(new Rect(0, 0, 10000, 20));
            }, "Save Editor");
        }
    }
}