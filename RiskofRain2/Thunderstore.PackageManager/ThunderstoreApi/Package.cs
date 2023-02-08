using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Thunderstore.PackageManager.ThunderstoreApi
{
    public class Package : Manifest
    {
        [DataMember(Name = "date_created")]
        public readonly DateTime DateCreated;

        [DataMember(Name = "downloads")]
        public readonly int Downloads;

        [DataMember(Name = "download_url")]
        public readonly string DownloadUrl;

        [DataMember(Name = "full_name")]
        public readonly string FullName;

        [DataMember(Name = "icon")]
        public readonly string Icon;

        [DataMember(Name = "is_active")]
        public readonly bool IsActive;

        [DataMember(Name = "namespace")]
        public readonly string Namespace;

        public async Task<byte[]> Download()
        {
            using (HttpClient client = new HttpClient())
            {
                return await client.GetByteArrayAsync(DownloadUrl);
            }
        }

        public async Task<FileInfo> Download(DirectoryInfo directory)
        {
            FileInfo path = directory.GetFile(FullName + ".zip");
            File.WriteAllBytes(path.FullName, await Download());
            return path;
        }

        public static async Task<Package> Get(string @namespace, string name, string version)
        {
            string json = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                json = await client.GetStringAsync($"https://thunderstore.io/api/experimental/package/{@namespace}/{name}/{version}");
            }
            return PSTinyJson.JSONParser.FromJson<Package>(json);
        }

        public static async Task<Package> Get(string @namespace, string name)
        {
            string json = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                json = await client.GetStringAsync($"https://thunderstore.io/api/experimental/package/{@namespace}/{name}");
            }
            return PSTinyJson.JSONParser.FromJson<Page>(json).Latest;
        }

        public async Task<bool> IsLatestVersion()
        {
            Page page = await Page.Get(Namespace, Name);
            return page.Latest.VersionNumber == this.VersionNumber;
        }
    }
}
