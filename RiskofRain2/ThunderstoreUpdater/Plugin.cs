using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThunderstoreUpdater
{
    public class Plugin
    {
        public Version Version { get; }
        public string Name { get; } 
        public string Author { get; }
        public DirectoryInfo PluginDirectory { get; }

        public Plugin( DirectoryInfo PluginDirectory )
        {
            this.PluginDirectory = PluginDirectory;
            string[] pluginDirNameSplit = PluginDirectory.Name.Split('-');
            Name = pluginDirNameSplit[ 1 ];
            Author = pluginDirNameSplit[ 0 ];
            Version = Version.ParseVersion(pluginDirNameSplit[ 2 ]);
        }

        public override string ToString()
        {
            return PluginDirectory.Name;
        }


        public async Task<bool> IsLatestVersion()
        {
            Package package = await Package.GetPackage(Author, Name);
            if ( package == null ) 
            {
                Console.WriteLine($"Warning: {this.PluginDirectory.Name} API response was null");
                return true;
            }
            if( package.latest.version == new Version(0,0,0))
            {
                Console.WriteLine($"Warning: {this.PluginDirectory.Name} package latest version was 0.0.0");
            }
            return package.latest.version <= Version; 
        }

    }
}
