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

            Updater updater = new Updater(@"..\..\resources\settings2.json");
            //updater.StartAsync();
            updater.StartParallelAsync();

            //for (int i = 0; i < 10; i++)
            //{
            //    Task.Run(() => Console.WriteLine(CommandHandler.Execute("CMD.exe", @"/c F:\Music\ytdl\youtube-dl.exe -U")));
            //}
            //var outp = updater.GetPlaylistItemData("PLSrMfDSgltgfvmLgVH1TE9vCq6rRmugOi", "1-2");

            Console.WriteLine(DateTime.Now.ToString("yyyyMMdd"));
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
