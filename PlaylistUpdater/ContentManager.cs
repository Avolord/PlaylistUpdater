using PlaylistUpdater.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PlaylistUpdater
{
    static class ContentManager
    {
        public enum Controls
        {
            HOME,
            PLAYLISTS,
            SETTINGS,
            ABOUT
        }

        //public UserControl Home { get; set; }
        //public UserControl Playlists { get; set; }
        //public UserControl Settings { get; set; }
        //public UserControl About { get; set; }

        public static ContentControl Controller { get; private set; }

        public static void Init (ContentControl controller)
        {
            Controller = controller;
            controller.Content = new HomeControl();
        }

        public static void Navigate(ContentManager.Controls Type)
        {
            if (Controller != null)
            {
                switch (Type)
                {
                    case Controls.HOME:
                        Controller.Content = new HomeControl();
                        break;
                    case Controls.PLAYLISTS:
                        Controller.Content = new PlaylistsControl();
                        break;
                    case Controls.SETTINGS:
                        Controller.Content = new SettingsControl();
                        break;
                    case Controls.ABOUT:
                        Controller.Content = new AboutControl();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
