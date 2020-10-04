
using System.IO;
using System.Text.Json;

namespace PlaylistUpdater
{
    class SettingsManager
    {
        public PlaylistConfiguration PlaylistConfiguration { get; }
        public CoreConfiguration Settings { get; }

        public SettingsManager(string path)
        {
            Settings = new CoreConfiguration(path);
            PlaylistConfiguration = new PlaylistConfiguration(path);
        }

        public void GenerateSettings(string destination)
        {

        }
    }
}
