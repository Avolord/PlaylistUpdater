using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Data;

namespace PlaylistUpdater
{
    class PlaylistConfiguration : ExternalConfig<List<PlaylistEntry>>
    {
        private string FilePath;

        public PlaylistConfiguration(string path)
        {
            Data = new List<PlaylistEntry>();
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

                Data = JsonSerializer.Deserialize<List<PlaylistEntry>>(jsonString, serializeOptions);
                FilePath = path;
            }
            else
            {
                Generate(Constants.DefaultPathPrefix + "playlists.json");
            }
        }

        public override void Save(string path)
        {
            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            File.WriteAllText(path, JsonSerializer.Serialize<List<PlaylistEntry>>(Data, serializeOptions));
        }

        protected override void Generate(string destination)
        {
            FilePath = destination;
            Data = new List<PlaylistEntry>();

            Save(FilePath);
        }

        public void AddEntry(PlaylistEntry entry, bool saveChanges = false)
        {
            Data.Add(entry);
            if(saveChanges)
            {
                Save(FilePath);
            }
        }
    }
}
