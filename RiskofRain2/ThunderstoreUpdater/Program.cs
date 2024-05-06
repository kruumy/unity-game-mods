using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThunderstoreUpdater
{
    internal class Program
    {
        public static DirectoryInfo PluginsDirectory { get; } = new DirectoryInfo(Environment.CurrentDirectory);
        public static List<Plugin> Plugins { get; set; } = new List<Plugin>();
        static void Main( string[] args )
        {
            foreach (var pluginDir in Directory.EnumerateDirectories(PluginsDirectory.FullName,"*",SearchOption.TopDirectoryOnly) )
            {
                try
                {
                    Plugins.Add(new Plugin(new DirectoryInfo(pluginDir)));
                }
                catch
                {
                    Console.WriteLine($"Could not create plugin definition with {Path.GetFileName(pluginDir)}");
                }
            }

            Console.WriteLine("\n--------------------------------");

            Console.WriteLine("Checking for updates...\n");
            int outOfDateCount = 0;
            int progressCount = 0;
            foreach (var plugin in Plugins )
            {
                Console.Title = $"{progressCount}/{Plugins.Count}";
                if( !plugin.IsLatestVersion().Result )
                {
                    Console.WriteLine($"{plugin.PluginDirectory.Name} is out of date!");
                    outOfDateCount++;
                }
                progressCount++;
            }
            Console.Title = string.Empty;

            if( outOfDateCount > 0 )
            {
                Console.WriteLine($"\n{outOfDateCount}/{Plugins.Count} plugins are out of date");
            }
            else
            {
                Console.WriteLine("All plugins are up to date!");
            }
            Console.ReadLine();
        }
    }
}
