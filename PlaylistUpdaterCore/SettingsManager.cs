
using System;
using System.IO;
using System.Text.Json;

namespace PlaylistUpdater
{
    class SettingsManager
    {
        public PlaylistConfiguration PlaylistConfiguration { get; }
        public CoreConfiguration CoreConfiguration { get; }

        public SettingsManager(string path)
        {
            CoreConfiguration = new CoreConfiguration(path);

            if(CoreConfiguration.IsValid)
            {
                string configPath = CoreConfiguration.Data.Configs["playlist_data"];
                PlaylistConfiguration = new PlaylistConfiguration(configPath);
            } else
            {
                throw new Exception("The Binaries are not configured correctly!");
            }
        }
    }
}
