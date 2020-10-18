
using System;
using System.Text.Json.Serialization;

namespace PlaylistUpdater
{
    class PlaylistEntry
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("last_updated")]
        public DateTime LastUpdated { get; set; }
        [JsonPropertyName("channel")]
        public string Channel { get; set; }
        [JsonPropertyName("genres")]
        public string[] Genres { get; set; }
        [JsonPropertyName("location")]
        public string Location { get; set; }
        [JsonPropertyName("sorted")]
        public bool Sorted { get; set; } = false;

    }
}

//"URL","LAST_UPDATED","CHANNEL","GENRE","LOCATION","SORTED"