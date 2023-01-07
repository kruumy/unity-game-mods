using EasyIMGUI.Controls.Automatic;
using EasyIMGUI.Controls.Extra;
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
            SingleButton UpTofuBtn = new SingleButton();
            UpTofuBtn.OnButtonPressed += (object ob, EventArgs e) => Teleport.UpTofu();
            UpTofuBtn.Content.text = "Up Tofu";
            SingleButton DownTofuBtn = new SingleButton();
            DownTofuBtn.Content.text = "Down Tofu";
            DownTofuBtn.OnButtonPressed += (object ob, EventArgs e) => Teleport.DownTofu();

            TextField PlayerNameField = new TextField
            {
                Value = string.Empty
            };

            SingleButton MeToPlayerBtn = new SingleButton();
            MeToPlayerBtn.Content.text = "Me To Player";
            MeToPlayerBtn.OnButtonPressed += (object ob, EventArgs e) => Teleport.MeToPlayer(PlayerNameField.Value);

            Root.Controls.Add(new Window()
            {
                Content =
                {
                    text = "Teleport Menu"
                },
                Dimensions = new Rect(10, 10, 300, 10),
                Controls =
                {
                    UpTofuBtn,
                    DownTofuBtn,
                    new Horizontal()
                    {
                        Controls =
                        {
                            MeToPlayerBtn,
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
                area.Controls.Add(new HorizontalCenter()
                {
                    Controls =
                    {
                        new EasyIMGUI.Controls.Automatic.Label()
                        {
                            Content = { text = "All Players" }
                        },
                    }
                });
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
