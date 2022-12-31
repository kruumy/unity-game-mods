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

        internal static void Enable()
        {
            if (!IsOpen)
            {
                MelonEvents.OnGUI.Subscribe(Draw);
                IsOpen = true;
            }
            else throw new System.Exception("Menu Already Enabled");
        }

        internal static void Disable()
        {
            if (IsOpen)
            {
                MelonEvents.OnGUI.Unsubscribe(Draw);
                IsOpen = false;
            }
            else throw new System.Exception("Menu Already Disabled");
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