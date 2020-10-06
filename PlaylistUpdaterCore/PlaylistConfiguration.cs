using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Data;

namespace PlaylistUpdater
{
    class PlaylistConfiguration : ExternalConfig<List<PlaylistUpdateData>>
    {
        public PlaylistConfiguration(string path)
        {
            Data = new List<PlaylistUpdateData>();
            Load(path);
        }

        public override void Load(string path)
        {
            if (File.Exists(path))
            {
                //Data = CsvImport.NewDataTable(path, ",", true);

                //ChangeColumnTypes(Data, Data.Columns[1].ColumnName, typeof(DateTime));
                //ChangeColumnTypes(Data, Data.Columns[5].ColumnName, typeof(bool));

                DataTable dt = CsvImport.NewDataTable(path, ",", true);
                foreach(DataRow row in dt.Rows)
                {
                    PlaylistUpdateData updateData = new PlaylistUpdateData()
                    {
                        Channel     = row.Field<string>("CHANNEL"),
                        Genre       = row.Field<string>("GENRE"),
                        Location    = row.Field<string>("LOCATION"),
                        Url         = row.Field<string>("URL"),
                        LastUpdated = ToDateTime(row.Field<string>("LAST_UPDATED")),
                        Sorted      = ToBoolean(row.Field<string>("SORTED"))
                    };
                    Data.Add(updateData);
                }
            }
            else
            {
                Generate(Constants.DefaultPathPrefix + "playlist_update_data.csv");
            }
        }

        protected override void Generate(string destination)
        {
            //generate an empty config file (just the headers)
        }

        private void ChangeColumnTypes(System.Data.DataTable dt, string columnName, Type type)
        {
            int ordinal = dt.Columns[columnName].Ordinal;

            dt.Columns.Add(columnName + "_new", type);
            foreach (System.Data.DataRow dr in dt.Rows)
            {   // Will need switch Case for others if Date is not the only one.
                if(type == typeof(DateTime))
                {
                    dr[columnName + "_new"] = ToDateTime(dr[columnName].ToString());
                } 
                else if(type == typeof(bool))
                {
                    dr[columnName + "_new"] = ToBoolean(dr[columnName].ToString());
                }  
            }
            dt.Columns.Remove(columnName);
            dt.Columns[columnName + "_new"].ColumnName = columnName;

            dt.Columns[columnName].SetOrdinal(ordinal);
        }

        private DateTime ToDateTime(string entry)
        {
            return DateTime.ParseExact(entry, "yyyyMMdd", new System.Globalization.CultureInfo("de-DE"));
        }

        private bool ToBoolean(string entry)
        {
            return entry == "Yes";
        }

    }
}
