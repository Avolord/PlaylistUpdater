using System;
using System.Data;
using System.Windows.Media;

namespace PlaylistUpdater
{
    class GridController
    {
        public enum CellType
        {
            PATH,
            DATE
        }

        public static void HandleCellClick(object sender, GridController.CellType cellType)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is System.Windows.Controls.DataGridRow row)
                {
                    if (row != null)
                    {
                        var data = row.Item as System.Data.DataRowView;

                        switch (cellType)
                        {
                            case CellType.PATH:
                                HandlePathCellClick(data.Row);
                                break;
                            case CellType.DATE:
                                break;
                            default:
                                break;
                        }
                    }

                    break;
                }
        }

        public static void HandlePathCellClick(DataRow row)
        {


            Console.WriteLine(row.Field<string>("LOCATION"));
        }
    }
}
