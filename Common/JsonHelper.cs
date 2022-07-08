using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Version
    {
        public StickyNotesVersion StickyNotesVersion { get; set; }
        public UpdateAppVersion UpdateAppVersion { get; set; }
    }
    public class StickyNotesVersion
    {
        public int MajorVersionNumber { get; set; }
        public int MinorVersionNumber { get; set; }
        public int RevisionNumebr { get; set; }
    }
    public class UpdateAppVersion
    {
        public int MajorVersionNumber { get; set; }
        public int MinorVersionNumber { get; set; }
        public int RevisionNumebr { get; set; }
    }
    public class JsonHelper
    {
        public static Version ReadVersionFromFile(string versionFileName)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, versionFileName);
            var json = File.ReadAllText(filePath);
            var version = Newtonsoft.Json.JsonConvert.DeserializeObject<Version>(json);
            return version;
        }
        public  static void WriteVersionToFile(Version version,string versionFileName)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, versionFileName);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(version);
            File.WriteAllText(filePath, json);
        }
    }
}
