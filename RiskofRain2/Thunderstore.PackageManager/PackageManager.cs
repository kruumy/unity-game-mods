using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Thunderstore.PackageManager.ThunderstoreApi;

namespace Thunderstore.PackageManager
{
    public class PackageManager
    {
        public readonly DirectoryInfo gameDirectory;

        public PackageManager(DirectoryInfo gameDirectory)
        {
            if (!gameDirectory.Exists)
            {
                throw new DirectoryNotFoundException();
            }
            this.gameDirectory = gameDirectory;
        }

        public DirectoryInfo BepInExPluginsDirectory => gameDirectory.GetDirectory("BepInEx").GetDirectory("plugins");

        public LocalPackage[] Packages
        {
            get
            {
                List<LocalPackage> ret = new List<LocalPackage>();
                foreach (DirectoryInfo item in BepInExPluginsDirectory.GetDirectories())
                {
                    ret.Add(new LocalPackage(item));
                }
                return ret.ToArray();
            }
        }


        public async Task DownloadAndInstallPackage(Package package, bool overWrite = false, bool downloadDependencies = true)
        {
            if ((package.Name == "BepInExPack" && package.Namespace == "bbepis"))
            {
                return;
            }
            if (!overWrite && Packages.Any(p => package.FullName.Contains(p.FullName))) // TODO better check
            {
                return;
            }
            FileInfo zipPath = await package.Download(BepInExPluginsDirectory);
            DirectoryInfo extractFolder = zipPath.Directory.CreateSubdirectory(Path.GetFileNameWithoutExtension(zipPath.Name).Substring(0, zipPath.Name.LastIndexOf('-')));
            if (extractFolder.Exists)
            {
                extractFolder.Delete(true);
            }
            ZipFile.ExtractToDirectory(zipPath.FullName, extractFolder.FullName);
            zipPath.Delete();
            if (downloadDependencies)
            {
                foreach (Task<Package> dep in package.DependenciesToPackages(false))
                {
                    await DownloadAndInstallPackage(await dep, overWrite, downloadDependencies);
                }
            }
        }

        public IEnumerable<(LocalPackage package, Task<LocalPackage.UpdateStatus> status)> UpdateAllPackagesEnumerable()
        {
            foreach (LocalPackage package in Packages)
            {
                yield return (package, package.Update());
            }
        }

        public async Task UpdateAllPackages()
        {
            foreach (LocalPackage package in Packages)
            {
                await package.Update();
            }
        }
    }
}
