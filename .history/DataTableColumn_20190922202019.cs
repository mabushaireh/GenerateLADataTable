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

                if (this.Values.Count > 0) {
                    for (var i = 0; i < this.Values.Count; i++) {
                        if (string.IsNullOrEmpty (this.Values[i]))
                            continue;
                        else {
                            this._type = ParseString (this.Values[i]);
                            return this._type;
                        }
                    }
                    this._type = DataType.String;
                    return this._type;

                } else
                    this._type = DataType.String;

                return this._type;
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

                if (bool.TryParse (value, out boolValue))
                    return DataType.Boolean;
                else if (Int32.TryParse (value, out intValue))
                    return DataType.Int;
                else if (double.TryParse (value, out doubleValue))
                    return DataType.Double;
                else if (DateTime.TryParseExact (value, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dateValue))
                    return DataType.Datetime;
                else return DataType.String;

            }
            // Place checks higher in if-else statement to give higher priority to type.

        }
    }

}