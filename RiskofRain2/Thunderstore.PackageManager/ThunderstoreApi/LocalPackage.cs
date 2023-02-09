using System;
using System.IO;
using System.Threading.Tasks;

namespace Thunderstore.PackageManager.ThunderstoreApi
{
    public class LocalPackage
    {
        public readonly DirectoryInfo directory;
        public readonly string FullName;
        public readonly Manifest Manifest;
        public readonly string Name;
        public readonly string Namespace;

        public LocalPackage(DirectoryInfo directory)
        {
            this.directory = directory;
            FullName = directory.Name;
            string[] each = FullName.Split('-');
            Namespace = each[0];
            try
            {
                Name = each[1];
            }
            catch (IndexOutOfRangeException)
            {
                Name = null;
            }
            try
            {
                Manifest = Manifest.Get(File.ReadAllText(directory.GetFile("manifest.json").FullName));
            }
            catch (FileNotFoundException)
            {
                Manifest = null;
            }
        }

        public async Task<Package> GetLatestPackage()
        {
            return (await GetPage()).Latest;
        }

        public async Task<Package> GetPackage()
        {
            if (Manifest != null)
            {
                return await Package.Get(Namespace, Name, Manifest.VersionNumber);
            }
            else
            {
                return (await GetPage()).Latest;
            }
        }

        public async Task<Page> GetPage()
        {
            return await Page.Get(Namespace, Name);
        }

        public async Task<bool> IsLatestVersion()
        {
            Package package = await GetLatestPackage();
            return IsLatestVersion(package);
        }

        public bool IsLatestVersion(Package latestPackage)
        {
            return latestPackage.VersionNumber == this.Manifest.VersionNumber;
        }

        public async Task<UpdateStatus> Update()
        {
            try
            {
                Package latestPackage = await GetLatestPackage();
                if (!IsLatestVersion(latestPackage))
                {

                    await new PackageManager(directory.Parent).DownloadAndInstallPackage(latestPackage, true, false);
                    return UpdateStatus.Updated;
                }
                else
                {
                    return UpdateStatus.IsAlreadyUpdated;
                }
            }
            catch
            {
                return UpdateStatus.UnhandledExceptionThrown;
            }
        }

        public enum UpdateStatus
        {
            Updated,
            IsAlreadyUpdated,
            UnhandledExceptionThrown
        }
    }
}
