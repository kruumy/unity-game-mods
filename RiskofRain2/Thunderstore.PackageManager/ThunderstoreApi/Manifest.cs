using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Thunderstore.PackageManager.ThunderstoreApi
{
    public class Manifest
    {
        [DataMember(Name = "dependencies")]
        public readonly string[] Dependencies;

        [DataMember(Name = "description")]
        public readonly string Description;

        [DataMember(Name = "name")]
        public readonly string Name;

        [DataMember(Name = "version_number")]
        public readonly string VersionNumber;

        [DataMember(Name = "website_url")]
        public readonly string WebsiteUrl;

        public static Manifest Get(string manifestJson)
        {
            return PSTinyJson.JSONParser.FromJson<Manifest>(manifestJson);
        }

        public IEnumerable<Task<Package>> DependenciesToPackages(bool usePreferedVersion = false)
        {
            foreach (string dep in Dependencies)
            {
                string[] depSplit = dep.Split('-');
                if (usePreferedVersion)
                {
                    yield return Package.Get(depSplit[0], depSplit[1], depSplit[2]);
                }
                else
                {
                    yield return Package.Get(depSplit[0], depSplit[1]);
                }

            }
        }

    }
}
