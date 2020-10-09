using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistUpdater
{
    class DownloadConfiguration
    {
        public bool ConvertToAudio { get; set; } = true;
        public string AudioFormat { get; set; } = "m4a";
        public bool UseExecCommand { get; set; } = false;
        public string ExecCommand { get; set; } = "";
        public string ArchivePath { get; set; } = "";
        public string DateAfter { get; set; } = "";
        public int PlaylistEnd { get; set; } = 20;
        public bool SkipDashManifest { get; set; } = true;
        public string FFMPEG { get; set; } = "";
        public bool EmbedThumbnail { get; set; } = true;
        public bool QuiteMode { get; set; } = true;
        public bool RestrictFilenames { get; set; } = true;
        public bool AddMetaData { get; set; } = true;
        public bool AddMetaDataFromTitle { get; set; } = true;
        public string MetaDataFromTitleFilter { get; set; } = "%(artist)s - %(title)s";
        public bool IgnoreErrors { get; set; } = true;
        public string Url { get; set; } = "";
        public string OutputFormat { get; set; } = "";

        public DownloadConfiguration(string ffmpeg)
        {
            FFMPEG = ffmpeg;
        }

        public string Generate()
        {
            StringBuilder sb = new StringBuilder("", 100);
            if(ConvertToAudio)
            {
                sb.AppendFormat("-x --audio-format {0}", AudioFormat);
            }
            if(UseExecCommand)
            {
                sb.AppendFormat(" --exec \"{0}\"", ExecCommand);
            }
            if(SkipDashManifest)
            {
                sb.Append(" --youtube-skip-dash-manifest");
            }
            if(EmbedThumbnail)
            {
                sb.Append(" --embed-thumbnail");
            }
            if(QuiteMode)
            {
                sb.Append(" --quiet");
            }
            if(RestrictFilenames)
            {
                sb.Append(" --restrict-filenames");
            }
            if(AddMetaData)
            {
                sb.Append(" --add-metadata");
            }
            if(AddMetaDataFromTitle)
            {
                sb.AppendFormat(" --metadata-from-title \"{0}\"", MetaDataFromTitleFilter);
            }
            if(IgnoreErrors)
            {
                sb.Append(" --ignore-errors");
            }

            sb.AppendFormat(" --download-archive \"{0}\" --dateafter {1} --playlist-end {2} --ffmpeg-location \"{3}\" {4} -o \"{5}\"",
                ArchivePath, DateAfter, PlaylistEnd.ToString(), FFMPEG, Url, OutputFormat);


            return sb.ToString();
        }
    }
}
