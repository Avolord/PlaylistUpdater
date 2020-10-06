using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistUpdater
{
    class TestClass
    {
        public struct Binaries
        {
            public string youtube_dl;
            public string ffmpeg;
            public string ffplay;
            public string ffprobe;
        }
        public Binaries binaries { get; set; } = new Binaries() { youtube_dl = "dgg", ffmpeg = "", ffplay = "", ffprobe = "" };
    }
}
