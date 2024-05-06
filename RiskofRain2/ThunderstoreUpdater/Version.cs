using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThunderstoreUpdater
{
    public struct Version
    {
        public int major;
        public int minor;
        public int patch;

        public Version( int major, int minor, int patch )
        {
            this.major = major;
            this.minor = minor;
            this.patch = patch;
        }

        public static Version ParseVersion(string version)
        {
            string[] versionSplit = version.Split('.');
            return new Version()
            {
                major = int.Parse(versionSplit[ 0 ]),
                minor = int.Parse(versionSplit[ 1 ]),
                patch = int.Parse(versionSplit[ 2 ])
            };
        }

        public override string ToString()
        {
            return $"{major}.{minor}.{patch}";
        }

        public static implicit operator Version( string version )
        {
            return ParseVersion(version);
        }

        public static implicit operator string( Version version )
        {
            return version.ToString();
        }

        public static bool operator >( Version v1, Version v2 )
        {
            if ( v1.major > v2.major )
                return true;
            else if ( v1.major < v2.major )
                return false;

            if ( v1.minor > v2.minor )
                return true;
            else if ( v1.minor < v2.minor )
                return false;

            return v1.patch > v2.patch;
        }

        public static bool operator <( Version v1, Version v2 )
        {
            return !(v1 >= v2);
        }

        public static bool operator >=( Version v1, Version v2 )
        {
            return v1 > v2 || v1 == v2;
        }

        public static bool operator <=( Version v1, Version v2 )
        {
            return v1 < v2 || v1 == v2;
        }

        public static bool operator ==( Version v1, Version v2 )
        {
            return v1.major == v2.major && v1.minor == v2.minor && v1.patch == v2.patch;
        }

        public static bool operator !=( Version v1, Version v2 )
        {
            return !(v1 == v2);
        }

        public override bool Equals( object obj )
        {
            if ( !(obj is Version) )
                return false;

            Version other = (Version)obj;
            return this == other;
        }

        public override int GetHashCode()
        {
            return major.GetHashCode() ^ minor.GetHashCode() ^ patch.GetHashCode();
        }
    }
}
