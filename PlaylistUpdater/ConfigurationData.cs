using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PlaylistUpdater
{
    namespace Core
    {
        public class MandatoryData
        {
            [JsonPropertyName("ffmpeg")]
            public string FFmpeg { get; set; }
            [JsonPropertyName("youtube_dl")]
            public string YoutubeDl { get; set; }

            public MandatoryData(string ffmpeg, string youtubeDl)
            {
                FFmpeg = ffmpeg;
                YoutubeDl = youtubeDl;
            }

            public MandatoryData()
            {
                FFmpeg = "ffmpeg.exe";
                YoutubeDl = "youtube_dl.exe";
            }
        }

        public class NonMandatoryData
        {
            [JsonPropertyName("playlist_data")]
            public string PlaylistData { get; set; }
            [JsonPropertyName("archives")]
            public string Archives { get; set; }
            [JsonPropertyName("m3u")]
            public string PlaylistFiles { get; set; }

            public NonMandatoryData(string playlistData, string archives, string playlistFiles)
            {
                PlaylistData = playlistData;
                Archives = archives;
                PlaylistFiles = playlistFiles;
            }

            public NonMandatoryData()
            {
                PlaylistData = "playlist_update_data.csv";
                Archives = @".\archives";
                PlaylistFiles = @".\m3u";
            }
        }

        public partial class ConfigurationData
        {
            [JsonPropertyName("binaries")]
            public MandatoryData Binaries { get; set; }
            [JsonPropertyName("configs")]
            public NonMandatoryData Configs { get; set; }

            public ConfigurationData()
            {
                Binaries = new MandatoryData();
                Configs = new NonMandatoryData();
            }

            public ConfigurationData(MandatoryData binaries, NonMandatoryData configs)
            {
                Binaries = binaries;
                Configs = configs;
            }
        }
    }
}
