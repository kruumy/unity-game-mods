using System.IO;

namespace Thunderstore.PackageManager
{
    public static class DirectoryInfoExtensions
    {
        public static DirectoryInfo GetDirectory(this DirectoryInfo dir, string addition)
        {
            return new DirectoryInfo(Path.Combine(dir.FullName, addition));
        }

        public static FileInfo GetFile(this DirectoryInfo dir, string addition)
        {
            return new FileInfo(Path.Combine(dir.FullName, addition));
        }
    }
}
