using System;
using System.Collections.Generic;
using System.Text.Json;
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


            //PlaylistConfiguration config = new PlaylistConfiguration(@"..\..\resources\playlist_update_data.csv");

            //PlaylistUpdateDataGrid.DataContext = config.Data;

            ScriptController.RunCommand(ScriptController.PowershellJob, ScriptController.LoadScriptFromFile(@"..\..\resources\playlistUpdaterCore.ps1"));

            SettingsManager settingsManager = new SettingsManager(@"..\..\resources\settings.json");

            Console.WriteLine("The CoreConfiguration is " + ((settingsManager.CoreConfiguration.IsValid) ? "Valid!" : "Invalid!"));

            //PlaylistUpdateDataGrid.DataContext = settingsManager.PlaylistConfiguration.Data;
        }



        private void DataGridCell_LocationCell_Clicked(object sender, MouseButtonEventArgs e)
        {
            GridController.HandleCellClick(sender, GridController.CellType.PATH);
        }

        private void DataGridCell_LocationButton_Clicked(object sender, RoutedEventArgs e)
        {
            GridController.HandleCellClick(sender, GridController.CellType.PATH);
        }
    }
}
