using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistUpdater
{
    class Updater
    {
        public PlaylistConfiguration PlaylistConfiguration { get; private set; }
        public CoreConfiguration CoreConfiguration { get; private set; }

        public Updater(string settingsPath)
        {
            LoadSettings(settingsPath);
        }

        public string[] GetPlaylistItemData(string url, string range = "1", string filter = "--get-id")
        {
            string arguments = filter + " --playlist-items " + range + " " + url;
            string output = CommandHandler.Execute(@"F:\Music\ytdl\youtube-dl.exe", arguments);

            return output.Split('\n');
        }

        public void LoadSettings(string filename)
        {
            CoreConfiguration = new CoreConfiguration(filename);

            if (CoreConfiguration.IsValid)
            {
                string configPath = CoreConfiguration.Data.Configs.PlaylistData;
                PlaylistConfiguration = new PlaylistConfiguration(configPath);
            }
            else
            {
                throw new Exception("The Binaries are not configured correctly!");
            }
        }

        public void Start()
        { 
            if(!Directory.Exists(CoreConfiguration.Data.Configs.Archives))
            {
                Directory.CreateDirectory(CoreConfiguration.Data.Configs.Archives);
            }

            if (CoreConfiguration.IsValid)
            {
                //TODO: Parallelize the foreach loop with tasks.
                //      DownloadConfigObject would have to be stateless then.
                foreach (PlaylistUpdateData data in PlaylistConfiguration.Data)
                {
                    if (data.Sorted)
                    {
                        UpdatePlaylist_Sorted(data);
                    }
                    else
                    {
                        UpdatePlaylist_Unsorted(data);
                    }

                    data.LastUpdated = DateTime.Now;
                }
            }
        }

        public async Task StartAsync()
        {
            if (!CoreConfiguration.IsValid)
            {
                return;
            }

            if (!Directory.Exists(CoreConfiguration.Data.Configs.Archives))
            {
                Directory.CreateDirectory(CoreConfiguration.Data.Configs.Archives);
            }

            //TODO: Parallelize the foreach loop with tasks.
            //      DownloadConfigObject would have to be stateless then.
            foreach (PlaylistUpdateData data in PlaylistConfiguration.Data)
            {
                //Console.WriteLine("Queueing {0}'s Playlist...", data.Channel);
                string output;
                if (data.Sorted)
                {
                    output = await Task<string>.Run(() => UpdatePlaylist_Sorted(data));
                }
                else
                {
                    output = await Task<string>.Run(() => UpdatePlaylist_Unsorted(data));
                }

                data.LastUpdated = DateTime.Now;
                Console.WriteLine("Output: {0}", output);
            }

            Console.WriteLine("Done!");
        }

        public async Task StartParallelAsync()
        {
            if (!CoreConfiguration.IsValid)
            {
                return;
            }

            if (!Directory.Exists(CoreConfiguration.Data.Configs.Archives))
            {
                Directory.CreateDirectory(CoreConfiguration.Data.Configs.Archives);
            }

            List<Task<string>> tasks = new List<Task<string>>();

            //TODO: Parallelize the foreach loop with tasks.
            //      DownloadConfigObject would have to be stateless then.
            foreach (PlaylistUpdateData data in PlaylistConfiguration.Data)
            {
                //Console.WriteLine("Queueing {0}'s Playlist...", data.Channel);
                if (data.Sorted)
                {
                    tasks.Add(Task<string>.Run(() => UpdatePlaylist_Sorted(data)));
                }
                else
                {
                    tasks.Add(Task<string>.Run(() => UpdatePlaylist_Unsorted(data)));
                }
            }

            var results = await Task.WhenAll(tasks);

            foreach(var item in results)
            {
                Console.WriteLine("Output: {0}", item);
            }

            Console.WriteLine("Done!");
        }

        public string UpdatePlaylist_Unsorted(PlaylistUpdateData playlistUpdateData)
        {
            DownloadConfiguration dlConf = new DownloadConfiguration(CoreConfiguration.Data.Binaries.FFmpeg);

            dlConf.Url = playlistUpdateData.Url;
            dlConf.ArchivePath = CoreConfiguration.Data.Configs.Archives + playlistUpdateData.Channel + ".archive";
            dlConf.OutputFormat = playlistUpdateData.Location + @"\%(title)s.%(ext)s";
            dlConf.DateAfter = playlistUpdateData.LastUpdated.ToString("yyyyMMdd");
            dlConf.UseExecCommand = true;
            dlConf.ExecCommand = "echo {}";

            if (!File.Exists(dlConf.ArchivePath))
            {
                File.Create(dlConf.ArchivePath);
            }

            Console.WriteLine("Updating {0}'s Playlist...", playlistUpdateData.Channel);

            string output = CommandHandler.Cmd(CoreConfiguration.Data.Binaries.YoutubeDl + " " + dlConf.Generate());

            playlistUpdateData.LastUpdated = DateTime.Now;

            return output;
        }

        public string UpdatePlaylist_Sorted(PlaylistUpdateData playlistUpdateData)
        {
            return "";
        }
    }
}
