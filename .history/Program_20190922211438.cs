using System;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using ShellProgressBar;

namespace GenerateLADataTable {
    class Program {
        static void Main (string[] args) {
            if (args.Length == 0) {
                Console.WriteLine ("No arguments were passed.");
                Console.ReadLine (); // Keep the console open.
                return;
            }

            var filePath = args[0];
            Console.WriteLine ($"Processing {filePath}.");

            if (filePath.ToLower ().EndsWith (".xlsx")) {
                var package = new ExcelPackage (new System.IO.FileInfo (filePath));

                filePath = filePath.Replace (".xlsx", ".csv");
                package.ConvertToCsv (filePath);

                package.Dispose ();

            }

            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader (filePath);

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
            var dataTableName = "Data";

            var has = columns.FirstOrDefault (col => col.ColumnName == "Type");
            if (has != null) {
                dataTableName = has.Values[0];
            }

            var datatable = $"let {dataTableName} = datatable (" + appendColumns (columns) + ") [";
            datatable += Environment.NewLine;
            datatable += appendRows (columns);
            datatable += Environment.NewLine;
            datatable += "];";
            datatable += Environment.NewLine;
            datatable += dataTableName;
            Console.WriteLine (datatable);
            // TODO: Save as .kql for Kusto explorer
            var logPath = filePath.Replace (".csv", ".kql");
            var logFile = System.IO.File.Create (logPath);
            var logWriter = new System.IO.StreamWriter (logFile);
            logWriter.WriteLine (datatable);
            logWriter.Dispose ();
            TextCopy.Clipboard.SetText (datatable);

        }

        private static string appendRows (List<DataTableColumn> columns) {
            var rowsCount = columns[0].Values.Count;
            var rowsString = "";

            int totalTicks = rowsCount;
            var options = new ProgressBarOptions {
                ProgressCharacter = '─',
                ProgressBarOnBottom = true
            };

            using (var pbar = new ProgressBar (totalTicks, "progress", options)) {
                for (var i = 0; i < rowsCount; i++) {

                    foreach (var col in columns) {
                        var value = col.Values[i];

                        if (col.Type == DataType.Double || col.Type == DataType.Int || col.Type == DataType.Bool || col.Type == DataType.Datetime) {
                            if (string.IsNullOrEmpty (value)) {
                                rowsString += $"{col.Type.ToString().ToLower()}(null),";
                            } else {
                                rowsString += $"{col.Type.ToString().ToLower()}({value}),";
                            }
                            continue;
                        }

                        rowsString += "'" + value + "',";

                    }

                    pbar.Tick ();
                    rowsString += Environment.NewLine;
                    if (rowsString.Length > 2095000)
                        break;
                }

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