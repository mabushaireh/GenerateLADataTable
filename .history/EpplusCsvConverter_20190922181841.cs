using System.Collections.Generic;
using System.IO;
using System.Text;
using OfficeOpenXml;
namespace GenerateLADataTable {
    public static class EpplusCsvConverter {
        public static void ConvertToCsv (this ExcelPackage package, string outputfile) {
            var worksheet = package.Workbook.Worksheets[0];

            var maxColumnNumber = worksheet.Dimension.End.Column;
            var currentRow = new List<string> (maxColumnNumber);
            var totalRowCount = worksheet.Dimension.End.Row;
            var currentRowNum = 1;

            var memory = new MemoryStream ();

            using (var writer = new StreamWriter (memory, Encoding.ASCII)) {
                while (currentRowNum <= totalRowCount) {
                    BuildRow (worksheet, currentRow, currentRowNum, maxColumnNumber);
                    WriteRecordToFile (currentRow, writer, currentRowNum, totalRowCount);
                    currentRow.Clear ();
                    currentRowNum++;
                }
            }

            memory.WriteTo(new FileStream( outputfile, FileMode.OpenOrCreate));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record">List of cell values</param>
        /// <param name="sw">Open Writer to file</param>
        /// <param name="rowNumber">Current row num</param>
        /// <param name="totalRowCount"></param>
        /// <remarks>Avoiding writing final empty line so bulk import processes can work.</remarks>
        private static void WriteRecordToFile (List<string> record, StreamWriter sw, int rowNumber, int totalRowCount) {
            var commaDelimitedRecord = record.ToDelimitedString (",");

            if (rowNumber == totalRowCount) {
                sw.Write (commaDelimitedRecord);
            } else {
                sw.WriteLine (commaDelimitedRecord);
            }
        }

        private static void BuildRow (ExcelWorksheet worksheet, List<string> currentRow, int currentRowNum, int maxColumnNumber) {
            for (int i = 1; i <= maxColumnNumber; i++) {
                var cell = worksheet.Cells[currentRowNum, i];
                if (cell == null) {
                    // add a cell value for empty cells to keep data aligned.
                    AddCellValue (string.Empty, currentRow);
                } else {
                    AddCellValue (GetCellText (cell), currentRow);
                }
            }
        }

        /// <summary>
        /// Can't use .Text: http://epplus.codeplex.com/discussions/349696
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static string GetCellText (ExcelRangeBase cell) {
            return cell.Value == null ? string.Empty : cell.Value.ToString ();
        }

        private static void AddCellValue (string s, List<string> record) {
            record.Add (string.Format ("{0}{1}{0}", '"', s));
        }
    }
}