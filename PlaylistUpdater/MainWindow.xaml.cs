using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PlaylistUpdater
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ContentManager.Init(ContentMain);
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ResizeBtn_Click(object sender, RoutedEventArgs e)
        {
            if(WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            } else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ContentManager.Navigate(ContentManager.Controls.HOME);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ContentManager.Navigate(ContentManager.Controls.SETTINGS);
        }

        private void PlaylistsButton_Click(object sender, RoutedEventArgs e)
        {
            ContentManager.Navigate(ContentManager.Controls.PLAYLISTS);
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            ContentManager.Navigate(ContentManager.Controls.ABOUT);
        }
    }
}
