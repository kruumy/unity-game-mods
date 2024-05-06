using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ThunderstoreUpdater
{
    public class Package
    {
        public static string GetThunderStoreURL(string author, string name)
        {
            return $"https://thunderstore.io/api/experimental/package/{author}/{name}";
        }

        public static async Task<Package> GetPackage( string author, string name )
        {
            using ( HttpClient client = new HttpClient() )
            {
                int attempts = 0;
                while ( attempts <= 5 ) 
                {
                    try
                    {
                        string rawJson = await client.GetStringAsync(GetThunderStoreURL(author, name));
                        if ( rawJson == "{\"detail\":\"Not found.\"}" )
                        {
                            return null;
                        }
                        return rawJson.FromJson<Package>();
                    }
                    catch
                    {
                        await Task.Delay( 1000 );
                        attempts++;
                    }
                }
            }
            return null;
        }

        public string _namespace { get; set; }
        public string name { get; set; }
        public string full_name { get; set; }
        public string owner { get; set; }
        public string package_url { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_updated { get; set; }
        public int rating_score { get; set; }
        public bool is_pinned { get; set; }
        public bool is_deprecated { get; set; }
        public int total_downloads { get; set; }
        public Latest latest { get; set; }

        public class Latest
        {
            public string _namespace { get; set; }
            public string name { get; set; }
            public string version_number { get; set; }
            public Version version => Version.ParseVersion(version_number);
            public string full_name { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
            public string[] dependencies { get; set; }
            public string download_url { get; set; }
            public int downloads { get; set; }
            public DateTime date_created { get; set; }
            public string website_url { get; set; }
            public bool is_active { get; set; }
        }

    }
}
