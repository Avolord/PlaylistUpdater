using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PlaylistUpdater
{
    class CoreConfigurationData
    {
        public Dictionary<string, string> Binaries { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Configs { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Destinations { get; set; } = new Dictionary<string, string>();
    }

    class CoreConfiguration : ExternalConfig<CoreConfigurationData>
    {
        public bool IsValid { get; private set; }

        public CoreConfiguration(string path)
        {
            Data = new CoreConfigurationData();
            Load(path);
        }

        public override void Load(string path)
        {
            if (File.Exists(path))
            {
                var serializeOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                string jsonString = File.ReadAllText(path);
                Data = JsonSerializer.Deserialize<CoreConfigurationData>(jsonString, serializeOptions);
                Validate();
            }
            else
            {
                Generate(Constants.DefaultPathPrefix + "settings.json");
            }
        }

        protected override void Generate(string destination)
        {
            Data.Binaries.Add("ffmpeg", @"ffmpeg.exe");
            Data.Binaries.Add("youtube_dl", @"youtube_dl.exe");
            Data.Configs.Add("playlist_data", "playlist_update_data.csv");
            Data.Destinations.Add("archives", @".\archives");
            Data.Destinations.Add("m3u", @".\m3u");

            Validate();

            File.WriteAllText("settings.json", JsonSerializer.Serialize<CoreConfigurationData>(Data));
        }

        private bool Validate()
        {
            IsValid = true;
            //Validate the binaries
            for (int i = 0; i < Data.Binaries.Count; i++)
            {
                KeyValuePair<string, string> entry = Data.Binaries.ElementAt(i);
                if (!File.Exists(entry.Value))
                {
                    string executablePath = ExecutableFinder.Find(entry.Key + ".exe");
                    if (executablePath != null)
                    {
                        Data.Binaries[entry.Key] = executablePath;
                    } else
                    {
                        Data.Binaries[entry.Key] = "";
                        IsValid = false;
                    }
                }
            }

            //validate the configs
            if(!Data.Configs.ContainsKey("playlist_data"))
            {
                Data.Configs.Add("playlist_data", "playlist_update_data.csv");
            }

            //validate the destinations
            if(!Data.Destinations.ContainsKey("archives"))
            {
                Data.Destinations.Add("archives", @".\archives");
            }
            if (!Data.Destinations.ContainsKey("m3u"))
            {
                Data.Destinations.Add("m3u", @".\m3u");
            }

            return IsValid;
        }
    }
}
