using MelonLoader;
using UnityEngine;

namespace SaveEditor.Menu
{
    internal static class Menu
    {
        internal static Rect BaseMenu = new Rect(10, 10, 300, 500);
        internal const int Margin = 5;
        internal const int TitleBarHeight = 10;
        internal const int RowHeight = 25;
        private static int WindowID = new System.Random().Next();

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
            BaseMenu = GUI.Window(WindowID, BaseMenu, (int windowId) =>
            {
                Contents.SetMoney.Button.Draw();
                Contents.SetMoney.TextField.Draw();

                Contents.SetLevel.Button.Draw();
                Contents.SetLevel.TextField.Draw();

                Contents.SetBoost.Button.Draw();
                Contents.SetBoost.TextField.Draw();
            }, "Save Editor");
        }
    }
}