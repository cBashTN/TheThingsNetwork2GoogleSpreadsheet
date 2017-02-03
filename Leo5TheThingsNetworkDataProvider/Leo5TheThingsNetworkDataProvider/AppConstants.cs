using System;
using System.IO;

namespace Leo5TheThingsNetworkDataProvider
{

    public static class AppConstants
    {

        public static string DataStorageFilePath = Path.Combine(Environment.CurrentDirectory, "values.txt");
        public static string LogStorageFilePath = Path.Combine(Environment.CurrentDirectory, "log.txt");

        public static string SpreadsheetId = "1zyTU824zYBeQab6t-LuFtKJish2ltotuEQ_e6wAKyUc";
        public const int MaxFields = 200;
        public static string Range = "Sheet1!A2:S220";

        public static string CredPath = Path.Combine(Environment.CurrentDirectory, "scopeCredentials.json");

        public const string DeviceSwVersion = "16.12";
        public const string DeviceId = "30.4";

    }
}