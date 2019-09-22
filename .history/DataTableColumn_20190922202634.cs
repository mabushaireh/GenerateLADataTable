using System;
using System.Collections.Generic;
using System.Globalization;

namespace GenerateLADataTable {
    public class DataTableColumn {
        public string ColumnName { get; set; }

        private DataType _type = DataType.Unknown;
        public DataType Type {
            get {
                if (this._type != DataType.Unknown)
                    return this._type;

                this._type = ParseString (this.Values[i]);
                return this._type;
            }

        }
    }

    public List<string> Values { get; set; }
    public DataTableColumn () => this.Values = new List<string> ();

    public static DataType ParseString (string[] values) {

        bool boolValue;
        Int32 intValue;
        double doubleValue;
        DateTime dateValue;

        var tempDataType = DataType.Unknown;

        foreach (var value in values) {
            if (string.IsNullOrEmpty (value)) {
                tempDataType = DataType.Unknown;
                continue;
            }

            if (double.TryParse (value, out doubleValue)) {
                return DataType.Double;
            }

            if (DateTime.TryParseExact (value, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dateValue))
                return DataType.Datetime;

            if (bool.TryParse (value, out boolValue))
                return DataType.Boolean;

            else if (Int32.TryParse (value, out intValue)) {
                tempDataType = DataType.Int;
                continue;
            } else {
                tempDataType = DataType.String;
                continue;
            }

        }
        // Place checks higher in if-else statement to give higher priority to type.
        return tempDataType;

    }
}