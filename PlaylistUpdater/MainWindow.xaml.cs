using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
            Updater.Instance = new Updater(@"..\..\resources\settings2.json");
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Updater.Instance.StartParallelAsync();
        }
    }
}
