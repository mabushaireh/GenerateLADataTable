using System;
using System.Collections.Generic;

namespace GenerateLADataTable {
    class Program {
        static void Main (string[] args) {
            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader (@"D:\Source Code\Repos\GenerateLADataTable\119091822000867_Kusto query tab.csv");

            // Get the Header (Columns)
            var columns = new List<DataTableColumn> ();
            // Read first libe (Header or column names)
            var line1 = file.ReadLine ();
            // Comma separated
            var arrNames = line1.Split (',');

            foreach (var colName in arrNames) {
                var col = new DataTableColumn ();
                col.ColumnName = colName;
                columns.Add (col);
            }

            // get the rows
            while ((line = file.ReadLine ()) != null) {
                var values = line.Split (',');
                var index = 0;
                foreach (var value in values) {
                    columns[index++].Values.Add (value);
                }
            }

            file.Close ();

            var datatable = @"let data = datatable (" + appendColumns (columns) + ") [";
            datatable += Environment.NewLine;
            datatable += appendRows (columns);
            datatable += Environment.NewLine;
            datatable += "];";
            Console.WriteLine (datatable);
            TextCopy.Clipboard.SetText (datatable);

        }

        private static string appendRows (List<DataTableColumn> columns) {
            var rowsCount = columns[0].Values.Count;
            var rowsString = "";
            for (var i = 0; i < rowsCount; i++) {

                foreach (var col in columns) {
                    var value = col.Values[i];
                    if (string.IsNullOrEmpty (value)) {
                        rowsString += "'',";
                        continue;
                    }

                    if (value.Contains ("-")) {
                        rowsString += "'" + value + "',";
                        continue;
                    }

                    switch (col.Type) {
                    case DataType.Boolean:
                        rowsString += value + ",";
                        break;
                    case DataType.Datetime:
                        rowsString += "'" + value + "',";
                        break;
                    case DataType.Double:
                        rowsString += value + ",";
                        break;
                    case DataType.Int:
                        rowsString += value + ",";
                        break;
                    case DataType.String:
                        rowsString += "'" + value + "',";
                        break;
                    };
                }
                Console.WriteLine ($"Processed {i} of {rowsCount}");

                rowsString += Environment.NewLine;
            }
            rowsString = rowsString.Substring (0, rowsString.Length - 3);
            return rowsString;
        }

        private static string appendColumns (List<DataTableColumn> columns) {
            var colDefinition = "";

            foreach (var col in columns) {
                colDefinition += $"{col.ColumnName}: {col.Type.ToString().ToLower()},";
            }
            colDefinition = colDefinition.Substring (0, colDefinition.Length - 1);
            return colDefinition;
        }

    }
}