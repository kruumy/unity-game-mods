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
        internal static void Draw()
        {
            BaseMenu = GUI.Window(WindowID, BaseMenu, DrawContents, "Save Editor");
        }
        private static void DrawContents(int windowId)
        {
            Contents.SetMoney.Button.Draw();
            Contents.SetMoney.TextField.Draw();

            Contents.SetLevel.Button.Draw();
            Contents.SetLevel.TextField.Draw();

            Contents.SetBoost.Button.Draw();
            Contents.SetBoost.TextField.Draw();
        }

    }
}
