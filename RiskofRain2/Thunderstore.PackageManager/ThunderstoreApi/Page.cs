using System;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Thunderstore.PackageManager.ThunderstoreApi
{
    public class Page
    {
        [DataMember(Name = "namespace")]
        public readonly string Namespace;

        [DataMember(Name = "name")]
        public readonly string Name;

        [DataMember(Name = "full_name")]
        public readonly string FullName;

        [DataMember(Name = "owner")]
        public readonly string Owner;

        [DataMember(Name = "package_url")]
        public readonly string PackageUrl;

        [DataMember(Name = "date_created")]
        public readonly DateTime DateCreated;

        [DataMember(Name = "date_updated")]
        public readonly DateTime DateUpdated;

        [DataMember(Name = "rating_score")]
        public readonly int RatingScore;

        [DataMember(Name = "is_pinned")]
        public readonly bool IsPinned;

        [DataMember(Name = "is_deprecated")]
        public readonly bool IsDeprecated;

        [DataMember(Name = "total_downloads")]
        public readonly int TotalDownloads;

        [DataMember(Name = "latest")]
        public readonly Package Latest;

        [DataMember(Name = "community_listings")]
        public readonly CommunityListing[] CommunityListings;
        public class CommunityListing
        {

            [DataMember(Name = "has_nsfw_content")]
            public readonly bool HasNsfwContent;

            [DataMember(Name = "categories")]
            public readonly string[] Categories;

            [DataMember(Name = "community")]
            public readonly string Community;

            [DataMember(Name = "review_status")]
            public readonly string ReviewStatus;
            private CommunityListing()
            {
            }
        }
        private Page()
        {
        }

        public static async Task<Page> Get(string @namespace, string name)
        {
            string json = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                json = await client.GetStringAsync($"https://thunderstore.io/api/experimental/package/{@namespace}/{name}");
            }
            return PSTinyJson.JSONParser.FromJson<Page>(json);
        }
    }
}
