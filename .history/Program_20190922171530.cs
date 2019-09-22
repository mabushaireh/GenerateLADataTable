﻿using System;
using System.Collections.Generic;
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

            var datatable = @"let data = datatable (" + appendColumns (columns) + ") [";
            datatable += Environment.NewLine;
            datatable += appendRows (columns);
            datatable += Environment.NewLine;
            datatable += "];";
            Console.WriteLine (datatable);
            // TODO: Save as .kql for Kusto explorer
            var logPath = ".\\Query.kql";
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