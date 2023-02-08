using System;
using System.IO;
using System.Threading;
using Thunderstore.PackageManager.ThunderstoreApi;

namespace Thunderstore.PackageManager.TUI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            PackageManager packageManager = new PackageManager(new System.IO.DirectoryInfo(Environment.CurrentDirectory));

            while (true)
            {
                DrawSelectMenu("Select An Option...", "Manage Existing Packages", "Install New Package", "Update All Packages", "Enable/Disable BepInEx");
                switch (WaitForKeyInput())
                {
                    case ConsoleKey.D1:
                        {
                            ThunderstoreApi.LocalPackage[] packages = packageManager.Packages;
                            string[] options = new string[packages.Length];
                            for (int i = 0; i < options.Length; i++)
                            {
                                options[i] = packages[i].FullName;
                            }
                            DrawSelectMenu("Select An Option...", options);
                            switch (WaitForStringInput())
                            {

                            }
                            break;
                        }
                    case ConsoleKey.D2:
                        {
                            Console.Clear();
                            Console.Write("Enter package link: ");
                            string link = Console.ReadLine();
                            link = link.Substring(link.IndexOf("/package/") + "/package/".Length);
                            string[] unsortedItems = link.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                            Console.WriteLine("Downloading...");
                            switch (unsortedItems.Length)
                            {
                                case 2:
                                    {
                                        packageManager.DownloadAndInstallPackage(Package.Get(unsortedItems[0], unsortedItems[1]).Result).Wait();
                                        break;
                                    }
                                case 3:
                                    {
                                        packageManager.DownloadAndInstallPackage(Package.Get(unsortedItems[0], unsortedItems[1], unsortedItems[2]).Result).Wait();
                                        break;
                                    }
                                default:
                                    {
                                        throw new ArgumentException(link, nameof(link));
                                    }
                            }
                            Console.WriteLine("Complete!");
                            Thread.Sleep(1000);
                            break;
                        }
                    case ConsoleKey.D3:
                        {
                            Console.Clear();
                            foreach ((LocalPackage package, System.Threading.Tasks.Task<LocalPackage.UpdateStatus> status) item in packageManager.UpdateAllPackagesEnumerable())
                            {
                                Console.Write($"{item.package.FullName}...");
                                Console.Write($"{item.status.Result}\n");
                            }
                            Console.WriteLine("Done!");
                            Thread.Sleep(1000);
                            break;
                        }
                    case ConsoleKey.D4:
                        {
                            Console.Clear();
                            if (File.Exists("winhttp.dll.bak"))
                            {
                                File.Move("winhttp.dll.bak", "winhttp.dll");
                                Console.WriteLine("Enabled BepInEx");
                            }
                            else if (File.Exists("winhttp.dll"))
                            {
                                File.Move("winhttp.dll", "winhttp.dll.bak");
                                Console.WriteLine("Disabled BepInEx");
                            }
                            Thread.Sleep(1000);
                            break;
                        }
                }
            }

        }

        public static void DrawSelectMenu(string topTitle, params string[] options)
        {
            Console.Clear();
            Console.WriteLine(topTitle);
            for (int i = 0; i < topTitle.Length; i++)
            {
                Console.Write('-');
            }
            Console.WriteLine();
            for (int i = 0; i < options.Length; i++)
            {
                Console.Write(i + 1);
                Console.Write(". ");
                Console.WriteLine(options[i]);
            }
        }

        public static ConsoleKey WaitForKeyInput()
        {
            return Console.ReadKey().Key;
        }
        public static string WaitForStringInput()
        {
            return Console.ReadLine();
        }
    }
}
