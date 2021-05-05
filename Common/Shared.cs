using System.Configuration;

namespace Common
{
    public static class Shared
    {
        public readonly static string connString = ConfigurationManager.ConnectionStrings["TheBody"].ConnectionString;
        public readonly static string MD5_KEY = "ZXCzxc123!@#";
        public readonly static string Session_Customer = "Session_Customer";
        public readonly static string AdminAddress = "http://localhost:49992";
        public readonly static string Session_Admin = "Session_Admin";
    }
}
