using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class DataTableRow
    {
        public List<DataTableCell> Cells
        {
            get;
            set;
        } = new List<DataTableCell>();
    }

    public class DataTableCell
    {
        public bool IsDBNull
        {
            get;
            set;
        }

        public string StringValue
        {
            get;
            set;
        } = string.Empty;

        public byte[] ByteValue
        {
            get;
            set;
        }
    }

    public class DataTableColumn
    {
        public string ColumnName
        {
            get;
            set;
        }

        public string ColumnType
        {
            get;
            set;
        }
    }

    public class DataTableObject
    {
        public string DBName
        {
            get;
            set;
        }

        public string TableName
        {
            get;
            set;
        }

        public List<DataTableColumn> Columns
        {
            get;
            set;
        } = new List<DataTableColumn>();

        public List<DataTableRow> Rows
        {
            get;
            set;
        } = new List<DataTableRow>();
    }
}
