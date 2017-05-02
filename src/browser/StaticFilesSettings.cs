using System.IO;

namespace browser
{
    public class StaticFilesSettings
    {
        public StaticFilesSettings()
        {
            RootLocation = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot");
            RootPath = "/_browse";
        }
        public string RootPath { get; set; }
        public string RootLocation { get; set; }
    }
}