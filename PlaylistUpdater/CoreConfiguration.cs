using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PlaylistUpdater
{
    class CoreConfigurationDataf
    {
        public Dictionary<string, string> Binaries { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Configs { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Destinations { get; set; } = new Dictionary<string, string>();
    }

    class CoreConfiguration : ExternalConfig<Core.ConfigurationData>
    {
        public bool IsValid { get; private set; }

        public CoreConfiguration(string path)
        {
            Data = new Core.ConfigurationData();
            Load(path);
        }

        public override void Load(string path)
        {
            if (File.Exists(path))
            {
                var serializeOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                string jsonString = File.ReadAllText(path);
                Data = JsonSerializer.Deserialize<Core.ConfigurationData>(jsonString, serializeOptions);
                Validate();
            }
            else
            {
                Generate(Constants.DefaultPathPrefix + "settings.json");
            }
        }

        protected override void Generate(string destination)
        {
            Data = new Core.ConfigurationData();
            Validate();

            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            File.WriteAllText("settings.json", JsonSerializer.Serialize<Core.ConfigurationData>(Data, serializeOptions));
        }

        public void Generate_Test()
        {
            Core.MandatoryData mandatory = new Core.MandatoryData("ffmpeg.exe", "youtube_dl.exe");
            Core.NonMandatoryData nonMandatory = new Core.NonMandatoryData("playlist_update_data.csv", "archives", "m3u");
            Core.ConfigurationData d = new Core.ConfigurationData(mandatory, nonMandatory);

            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            Console.WriteLine(JsonSerializer.Serialize<Core.ConfigurationData>(d, serializeOptions));
        }

        private string ValidateBinary(string binary, string binaryPath)
        {
            //check if the file is valid
            if (!File.Exists(binaryPath))
            {
                //check if the file can be found in the environment variables
                string executablePath = ExecutableFinder.Find(binary);
                if (executablePath != null)
                {
                    return executablePath;
                }
                else
                {
                    IsValid = false;
                    return "";
                }
            }
            return binaryPath;
        }

        private bool Validate()
        {
            IsValid = true;

            //Validate the binaries
            Data.Binaries.FFmpeg = ValidateBinary("ffmpeg.exe", Data.Binaries.FFmpeg);
            Data.Binaries.YoutubeDl = ValidateBinary("youtube_dl.exe", Data.Binaries.YoutubeDl);

            return IsValid;
        }
    }
}
