using EasyIMGUI.Controls.Automatic;
using System;
using System.Linq;
using UnityEngine;

namespace TeleportMenu
{
    public static class GUI
    {
        public static readonly EasyIMGUI.MelonLoader.Interface.Menu Root = new EasyIMGUI.MelonLoader.Interface.Menu();

        public static void Initialize()
        {
            SingleButton btn1 = new SingleButton();
            btn1.OnButtonPressed += (object ob, EventArgs e) => Teleport.UpTofu();
            btn1.Content.text = "Up Tofu";
            SingleButton btn2 = new SingleButton();
            btn2.Content.text = "Down Tofu";
            btn2.OnButtonPressed += (object ob, EventArgs e) => Teleport.DownTofu();

            TextField PlayerNameField = new TextField
            {
                Value = string.Empty
            };

            SingleButton btn3 = new SingleButton();
            btn3.Content.text = "To Player";
            btn3.OnButtonPressed += (object ob, EventArgs e) => Teleport.ToPlayer(PlayerNameField.Value);

            Root.Controls.Add(new Window()
            {
                Content =
                {
                    text = "Teleport Menu"
                },
                Dimensions = new Rect(10, 10, 300, 10),
                Controls =
                {
                    btn1,
                    btn2,
                    new Horizontal()
                    {
                        Controls =
                        {
                            btn3,
                            PlayerNameField
                        }
                    }
                }
            });
            Root.OnOpen += (object ob, EventArgs e) =>
            {
                Window win = Root.Controls[0] as Window;

                if (win.Controls.Last() is Vertical v)
                {
                    _ = win.Controls.Remove(v);
                }

                Vertical area = new Vertical();
                area.Controls.Add(new EasyIMGUI.Controls.Automatic.Space() { Value = 25 });
                foreach (string name in RCC_SceneManager.Instance.get_AllPlayerNames())
                {
                    SingleButton btn = new SingleButton();
                    btn.Content.text = name;
                    btn.OnButtonPressed += (object ob2, EventArgs e2) => PlayerNameField.Value = name;
                    area.Controls.Add(btn);
                }
                win.Dimensions = new Rect(win.Dimensions.x, win.Dimensions.y, win.Dimensions.width, 0);
                win.Controls.Add(area);
            };
        }
    }
}
