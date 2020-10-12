using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PlaylistUpdater
{
    public static class ColorPalette
    {
        public static SolidColorBrush BaseDark { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#373a40"));
        public static SolidColorBrush BaseLight { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#686d76"));
        public static SolidColorBrush Accent { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#19d3da"));
        public static SolidColorBrush Highlight { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#eeeeee"));

    }
}
