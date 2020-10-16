
using System;

namespace PlaylistUpdater
{
    class PlaylistEntry
    {
        public string Url { get; set; }
        public DateTime LastUpdated { get; set; } 
        public string Channel { get; set; }
        public string Genre { get; set; }
        public string Location { get; set; }
        public bool Sorted { get; set; }

    }
}

//"URL","LAST_UPDATED","CHANNEL","GENRE","LOCATION","SORTED"